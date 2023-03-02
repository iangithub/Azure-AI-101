using Azure;
using Azure.AI.TextAnalytics;
using CustomerServiceLinebot.Models;
using DotNetLineBotSdk.Helpers;
using DotNetLineBotSdk.Message;
using DotNetLineBotSdk.MessageAction;
using DotNetLineBotSdk.MessageEvent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace CustomerServiceLinebot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinebotController : ControllerBase
    {
        private string channel_Access_Token = "linebot access token";
        private string azureEndpoint = "your azureEndpoint";
        private string azureApiKey = "your azureApiKey";

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var replyEvent = new ReplyEvent(channel_Access_Token);

            try
            {
                //Get Post RawData (json format)
                var req = this.HttpContext.Request;
                using (var bodyReader = new StreamReader(stream: req.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024, leaveOpen: true))
                {
                    var body = await bodyReader.ReadToEndAsync();
                    var lineReceMsg = ReceivedMessageConvert.ReceivedMessage(body);

                    if (lineReceMsg != null && lineReceMsg.Events[0].Type == WebhookEventType.message.ToString())
                    {
                        //get user msg
                        var userMsg = lineReceMsg.Events[0].Message.Text;
                        var cusFeedback = new CusFeedbackModel(userMsg);


                        //send to azure Summarize service
                        await Summarization(cusFeedback);

                        await Sentiment(cusFeedback);

                        await ExtractKeyPhrases(cusFeedback);

                        //send reply msg
                        var txtMessage = new TextMessage(cusFeedback.Summary);
                        await replyEvent.ReplyAsync(lineReceMsg.Events[0].ReplyToken,
                                                   new List<IMessage>() {
                                                       txtMessage
                                                   });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok();
            }
            return Ok();
        }

        //Summarize information
        private async Task Summarization(CusFeedbackModel cusFeedback)
        {
            var client = new TextAnalyticsClient(new Uri(azureEndpoint), new AzureKeyCredential(azureApiKey));

            string result = string.Empty;
            // Prepare analyze operation input. You can add multiple documents to this list and perform the same
            // operation to all of them.
            var batchInput = new List<string>
            {
                cusFeedback.Feedback
            };

            //MaxSentenceCount (指定傳回的句子數目,def:3) : 1~20 
            TextAnalyticsActions actions = new TextAnalyticsActions()
            {
                ExtractSummaryActions = new List<ExtractSummaryAction>() { new ExtractSummaryAction() { MaxSentenceCount = 1, OrderBy = SummarySentencesOrder.Rank } }
            };

            // Start analysis process.
            AnalyzeActionsOperation operation = await client.StartAnalyzeActionsAsync(batchInput, actions, "zh");
            await operation.WaitForCompletionAsync();

            // View operation results.
            await foreach (AnalyzeActionsResult documentsInPage in operation.Value)
            {
                IReadOnlyCollection<ExtractSummaryActionResult> summaryResults = documentsInPage.ExtractSummaryResults;

                foreach (ExtractSummaryActionResult summaryActionResults in summaryResults)
                {
                    foreach (ExtractSummaryResult documentResults in summaryActionResults.DocumentsResults)
                    {
                        result += $"Extracted the following {documentResults.Sentences.Count} sentence(s): \n";

                        foreach (SummarySentence sentence in documentResults.Sentences)
                        {
                            result += $"摘要: {sentence.Text} \n";
                        }
                    }
                }
            }
            cusFeedback.Summary = result;

            Console.WriteLine("========== Summarization ===================");
            Console.WriteLine(result);
        }

        //情感分析和意見挖掘 Analyze sentiment and opinion-mining
        private async Task Sentiment(CusFeedbackModel cusFeedback)
        {
           
            var result = string.Empty;

            var client = new TextAnalyticsClient(new Uri(azureEndpoint), new AzureKeyCredential(azureApiKey));

            var documents = new List<string>
            {
                cusFeedback.Feedback
            };

            AnalyzeSentimentResultCollection reviews = client.AnalyzeSentimentBatch(documents, language: "zh-hant", options: new AnalyzeSentimentOptions()
            {
                IncludeOpinionMining = true //是否包含意見挖掘
            });

            foreach (AnalyzeSentimentResult review in reviews)
            {
                result += $"Document sentiment: {review.DocumentSentiment.Sentiment}\n"
                    + $"\tPositive score: {review.DocumentSentiment.ConfidenceScores.Positive:0.00}\n"
                    + $"\tNegative score: {review.DocumentSentiment.ConfidenceScores.Negative:0.00}\n"
                    + $"\tNeutral score: {review.DocumentSentiment.ConfidenceScores.Neutral:0.00}\n";

                result += "\n";

                foreach (SentenceSentiment sentence in review.DocumentSentiment.Sentences)
                {
                    switch (sentence.Sentiment)
                    {
                        case TextSentiment.Positive:
                            result += $"😀 Text:{sentence.Text}\n";
                            break;
                        case TextSentiment.Neutral:
                            result += $"😶 Text:{sentence.Text}\n";
                            break;
                        case TextSentiment.Negative:
                            result += $"🙁 Text:{sentence.Text}\n";
                            break;
                        case TextSentiment.Mixed:
                            result += $"🤔 Text:{sentence.Text}\n";
                            break;
                    }

                    foreach (SentenceOpinion sentenceOpinion in sentence.Opinions)
                    {
                        result += $"\tTarget: {sentenceOpinion.Target.Text}, Value: {sentenceOpinion.Target.Sentiment}\n"
                            + $"\tTarget positive score: {sentenceOpinion.Target.ConfidenceScores.Positive:0.00}\n"
                            + $"\tTarget negative score: {sentenceOpinion.Target.ConfidenceScores.Negative:0.00}\n";

                        foreach (AssessmentSentiment assessment in sentenceOpinion.Assessments)
                        {
                            result += $"\t\tRelated Assessment: {assessment.Text}, Value: {assessment.Sentiment}\n"
                                + $"\t\tRelated Assessment positive score: {assessment.ConfidenceScores.Positive:0.00}\n"
                                + $"\t\tRelated Assessment negative score: {assessment.ConfidenceScores.Negative:0.00}\n";
                        }
                    }
                    result += "\n";
                }
            }
            Console.WriteLine("========== Analyze sentiment and opinion-mining ===================");
            Console.WriteLine(result);
        }

        //關鍵片語擷取
        private async Task ExtractKeyPhrases(CusFeedbackModel cusFeedback)
        {
            string result = string.Empty;
            var client = new TextAnalyticsClient(new Uri(azureEndpoint), new AzureKeyCredential(azureApiKey));

            Console.WriteLine("=========== Key Phrases =====================");

            var response = client.ExtractKeyPhrases(cusFeedback.Feedback, "zh_cht");
            foreach (string keyphrase in response.Value)
            {
                Console.WriteLine($"\t{keyphrase}");
            }
        }
    }
}

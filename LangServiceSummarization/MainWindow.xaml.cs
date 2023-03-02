<<<<<<< HEAD
﻿using Analytics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangServiceSummarization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void DoAnalytics_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
                var uri = "https://{Service url from azure}/text/analytics/v3.2-preview.2/analyze";

                //授權金鑰
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{Subscription-Key}");

                //文字內容
                var reqModel = new ReqModel();
                reqModel.analysisInput = new Analysisinput()
                {
                    documents = new List<Document>() {
                                                new Document() { id = "1", language = "zh-hans", text = this.ReqText.Text }
                                            }
                };
                //任務參數
                var task = new Tasks();
                task.extractiveSummarizationTasks = new List<Extractivesummarizationtask>()
                                                    { new Extractivesummarizationtask() {
                                                        parameters = new Parameters() { sentenceCount = 3,sortBy="Rank" }
                                                        }
                                                    };
                reqModel.tasks = task;
                byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reqModel, Formatting.Indented));
                string resultUrl = string.Empty;
                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var resp = await client.PostAsync(uri, content);
                    //由Response Header取得分析結果的API URL
                    resultUrl = resp.Headers.GetValues("operation-location").FirstOrDefault();
                }

                //分析需要時間，可採輪詢方式向API發出請求，判斷Stats=success狀態，這裡簡化寫法直接等待5秒
                await Task.Delay(5000);

                //取得分析結果
                var analyticsResp = await client.GetAsync(resultUrl);
                var resultString = await analyticsResp.Content.ReadAsStringAsync();
                foreach (var item in JsonConvert.DeserializeObject<AnalyticsResponse>(resultString)
                    .tasks.extractiveSummarizationTasks[0].results.documents[0].sentences)
                {
                    this.Analytics.Text += "\n\n\n" + item.text;
                }
            }
        }
    }
}
=======
﻿using Analytics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangServiceSummarization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void DoAnalytics_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
                var uri = "https://{Service url from azure}/text/analytics/v3.2-preview.2/analyze";

                //授權金鑰
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{Subscription-Key}");

                //文字內容
                var reqModel = new ReqModel();
                reqModel.analysisInput = new Analysisinput()
                {
                    documents = new List<Document>() {
                                                new Document() { id = "1", language = "zh-hans", text = this.ReqText.Text }
                                            }
                };
                //任務參數
                var task = new Tasks();
                task.extractiveSummarizationTasks = new List<Extractivesummarizationtask>()
                                                    { new Extractivesummarizationtask() {
                                                        parameters = new Parameters() { sentenceCount = 3,sortBy="Rank" }
                                                        }
                                                    };
                reqModel.tasks = task;
                byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reqModel, Formatting.Indented));
                string resultUrl = string.Empty;
                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var resp = await client.PostAsync(uri, content);
                    //由Response Header取得分析結果的API URL
                    resultUrl = resp.Headers.GetValues("operation-location").FirstOrDefault();
                }

                //分析需要時間，可採輪詢方式向API發出請求，判斷Stats=success狀態，這裡簡化寫法直接等待5秒
                await Task.Delay(5000);

                //取得分析結果
                var analyticsResp = await client.GetAsync(resultUrl);
                var resultString = await analyticsResp.Content.ReadAsStringAsync();
                foreach (var item in JsonConvert.DeserializeObject<AnalyticsResponse>(resultString)
                    .tasks.extractiveSummarizationTasks[0].results.documents[0].sentences)
                {
                    this.Analytics.Text += "\n\n\n" + item.text;
                }
            }
        }
    }
}
>>>>>>> f1b05a6653254629fbd859ee4ac91730cc60a4ea

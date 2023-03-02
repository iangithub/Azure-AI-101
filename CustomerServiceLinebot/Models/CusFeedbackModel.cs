using Azure.AI.TextAnalytics;

namespace CustomerServiceLinebot.Models
{
    public class CusFeedbackModel
    {
        public string UserId { get; set; }
        public string Feedback { get; private set; }
        public string Summary { get; set; }
        public CusFeedbackModel(string feedback)
        {
            Feedback = feedback;
        }
    }
}

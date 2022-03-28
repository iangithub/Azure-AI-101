namespace Analytics
{
    public class AnalyticsResult
    {
        public Document[] documents { get; set; }
        public object[] errors { get; set; }
        public string modelVersion { get; set; }
    }

    public class Document
    {
        public string id { get; set; }
        public string[] keyPhrases { get; set; }
        public object[] warnings { get; set; }
    }
}
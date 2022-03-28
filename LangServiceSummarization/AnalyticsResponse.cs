using System;

namespace Analytics
{
    public class AnalyticsResponse
    {
        public string jobId { get; set; }
        public DateTime lastUpdateDateTime { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime expirationDateTime { get; set; }
        public string status { get; set; }
        public object[] errors { get; set; }
        public Tasks tasks { get; set; }
    }

    public class Tasks
    {
        public int completed { get; set; }
        public int failed { get; set; }
        public int inProgress { get; set; }
        public int total { get; set; }
        public Extractivesummarizationtask[] extractiveSummarizationTasks { get; set; }
    }

    public class Extractivesummarizationtask
    {
        public DateTime lastUpdateDateTime { get; set; }
        public string state { get; set; }
        public Results results { get; set; }
    }

    public class Results
    {
        public Document[] documents { get; set; }
        public object[] errors { get; set; }
        public string modelVersion { get; set; }
    }

    public class Document
    {
        public string id { get; set; }
        public Sentence[] sentences { get; set; }
        public object[] warnings { get; set; }
    }

    public class Sentence
    {
        public string text { get; set; }
        public float rankScore { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
    }
}
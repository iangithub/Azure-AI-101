<<<<<<< HEAD
﻿namespace Analytics
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
        public Entity[] entities { get; set; }
        public object[] warnings { get; set; }
    }

    public class Entity
    {
        public string text { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
        public float confidenceScore { get; set; }
    }
}
=======
﻿namespace Analytics
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
        public Entity[] entities { get; set; }
        public object[] warnings { get; set; }
    }

    public class Entity
    {
        public string text { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
        public float confidenceScore { get; set; }
    }
}
>>>>>>> f1b05a6653254629fbd859ee4ac91730cc60a4ea

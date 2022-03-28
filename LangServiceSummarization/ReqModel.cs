
using System.Collections.Generic;

public class ReqModel
{
    public Analysisinput analysisInput { get; set; }
    public Tasks tasks { get; set; }
}

public class Analysisinput
{
    public List<Document> documents { get; set; }
}

public class Document
{
    public string language { get; set; }
    public string id { get; set; }
    public string text { get; set; }
}

public class Tasks
{
    public List<Extractivesummarizationtask> extractiveSummarizationTasks { get; set; }
}

public class Extractivesummarizationtask
{
    public Parameters parameters { get; set; }
}

public class Parameters
{
    public string modelversion { get; set; }
    public int sentenceCount { get; set; }
    public string sortBy { get; set; }
}

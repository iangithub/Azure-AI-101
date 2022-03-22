
public class EntitiesRecognition
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
    public string redactedText { get; set; }
}

public class Entity
{
    public string category { get; set; }
    public float confidenceScore { get; set; }
    public int length { get; set; }
    public int offset { get; set; }
    public string text { get; set; }
}

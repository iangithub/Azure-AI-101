
public class ReqModel
{
    public Document[] documents { get; set; }
}

public class Document
{
    public string language { get; set; }
    public string id { get; set; }
    public string text { get; set; }
}


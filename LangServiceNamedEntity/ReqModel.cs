
public class ReqModel
{
    public Document[] documents { get; set; }
}

public class Document
{
    public string id { get; set; }
    public string language { get; set; }
    public string text { get; set; }
}

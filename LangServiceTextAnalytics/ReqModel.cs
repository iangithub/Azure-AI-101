
public class ReqModel
{
    public TextContent[] documents { get; set; }
}

public class TextContent
{
    public string id { get; set; }
    public string language { get; set; }
    public string text { get; set; }
}


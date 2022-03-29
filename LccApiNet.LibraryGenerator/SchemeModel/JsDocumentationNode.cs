using Newtonsoft.Json;

namespace LccApiNet.LibraryGenerator.SchemeModel;

public class JsDocumentationNode
{
    [JsonProperty("text")]
    public string Text { get; set; } = null!;

    [JsonProperty("isReference")] 
    public bool IsReference { get; set; }
}
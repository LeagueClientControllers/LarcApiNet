using Newtonsoft.Json;

namespace LccApiNet.LibraryGenerator.Model;

public class ApiEnum
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("docs")]
    public JsDocumentationNode Docs { get; set; } = null!;

    [JsonProperty("members")]
    public ApiEnumMember[] Members { get; set; } = null!;
}
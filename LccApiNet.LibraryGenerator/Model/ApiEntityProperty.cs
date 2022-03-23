using Newtonsoft.Json;

namespace LccApiNet.LibraryGenerator.Model;

public class ApiEntityProperty
{
    [JsonProperty("name")] 
    public string Name { get; set; } = null!;

    [JsonProperty("jsonName")] 
    public string JsonName { get; set; } = null!;

    [JsonProperty("type")] 
    public ApiPropertyType Type { get; set; } = null!;

    [JsonProperty("initialValue")] 
    public object InitialValue { get; set; } = null!;

    [JsonProperty("docs")] 
    public JsDocumentationNode Docs { get; set; } = null!;
}
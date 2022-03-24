using Newtonsoft.Json;

namespace LccApiNet.LibraryGenerator.Model;

public class ApiModel
{
    [JsonProperty("declarations")]
    public ApiEntityDeclaration[] Declarations { get; set; } = null!;
    
    [JsonProperty("entities")]
    public ApiEntity[] Entity { get; set; } = null!;
    
    [JsonProperty("enums")]
    public ApiEnum[] Enums { get; set; } = null!;
}
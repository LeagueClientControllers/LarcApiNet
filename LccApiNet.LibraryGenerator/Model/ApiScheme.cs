using Newtonsoft.Json;

namespace LccApiNet.LibraryGenerator.Model;

public class ApiScheme
{
    [JsonProperty("apiVersion")] 
    public string ApiVersion { get; set; } = null!;
    
    [JsonProperty("schemeVersion")] 
    public string SchemeVersion { get; set; } = null!;
    
    [JsonProperty("generatedAt")] 
    public string GeneratedAt { get; set; } = null!;
    
    [JsonProperty("categories")] 
    public ApiCategory[] Categories { get; set; } = null!;
}
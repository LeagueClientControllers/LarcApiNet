using Newtonsoft.Json;

namespace LccApiNet.LibraryGenerator.Model;

public class ApiCategory
{
    [JsonProperty("name")] 
    public string Name { get; set; } = null!;
    
    [JsonProperty("methods")] 
    public ApiMethod[] Methods { get; set; } = null!;
    
}
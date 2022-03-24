using Newtonsoft.Json;

namespace LccApiNet.LibraryGenerator.Model;

public class ApiEntityDeclaration
{
    [JsonProperty("id")] 
    public int Id { get; set; }
    
    [JsonProperty("name")] 
    public string Name { get; set; } = null!;
    
    [JsonProperty("path")] 
    public string Path { get; set; } = null!;
}
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.JsonNet;

using Newtonsoft.Json;

namespace LccApiNet.LibraryGenerator.SchemeModel;

public class ApiEntityDeclaration
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("kind")]
    [JsonConverter(typeof(SmartEnumNameConverter<ApiEntityKind, int>))]
    public ApiEntityKind Kind { get; set; } = null!;
    
    [JsonProperty("name")] 
    public string Name { get; set; } = null!;
    
    [JsonProperty("path")] 
    public string Path { get; set; } = null!;
}

public class ApiEntityKind : SmartEnum<ApiEntityKind> {
    
    public ApiEntityKind(string name, int value) : base(name, value) { }

    public static readonly ApiEntityKind Event      = new ApiEntityKind("Event", 1);
    public static readonly ApiEntityKind Simple     = new ApiEntityKind("Simple", 2);
    public static readonly ApiEntityKind Response   = new ApiEntityKind("Response", 3);
    public static readonly ApiEntityKind Parameters = new ApiEntityKind("Parameters", 4);
}
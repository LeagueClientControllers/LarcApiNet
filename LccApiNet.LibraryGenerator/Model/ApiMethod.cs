using Ardalis.SmartEnum;
using Newtonsoft.Json;

namespace LccApiNet.LibraryGenerator.Model;

public class ApiMethod
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
    
    [JsonProperty("docs")]
    public JsDocumentationNode[] Docs { get; set; } = null!;
    
    [JsonProperty("parametersId")]
    public int ParametersId { get; set; }
    
    [JsonProperty("responseId")]
    public int ResponseId { get; set; }
    
    [JsonProperty("requireAccessToken")]
    public bool RequireAccessToken { get; set; }
    
    [JsonProperty("AccessibleFrom")]
    public MethodAccessPolicy AccessibleFrom { get; set; } = null!;
}

public class MethodAccessPolicy : SmartEnum<MethodAccessPolicy >
{
    public MethodAccessPolicy(string name, int value): base(name, value) {}
    
    public static readonly PrimitiveType Controller = new PrimitiveType("Controller", 1);
    public static readonly PrimitiveType Device     = new PrimitiveType("Device", 2);
    public static readonly PrimitiveType Both       = new PrimitiveType("Both", 3);
}
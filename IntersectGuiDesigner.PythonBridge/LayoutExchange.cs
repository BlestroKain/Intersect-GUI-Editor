using Newtonsoft.Json;

namespace IntersectGuiDesigner.PythonBridge;

public sealed class LayoutDocument
{
    [JsonProperty("canvas")]
    public LayoutCanvas Canvas { get; init; } = new();

    [JsonProperty("nodes")]
    public List<LayoutNode> Nodes { get; init; } = new();
}

public sealed class LayoutCanvas
{
    [JsonProperty("width")]
    public int Width { get; init; }

    [JsonProperty("height")]
    public int Height { get; init; }
}

public sealed class LayoutNode
{
    [JsonProperty("id")]
    public string Id { get; init; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; init; } = string.Empty;

    [JsonProperty("parent_id")]
    public string? ParentId { get; init; }

    [JsonProperty("bounds")]
    public LayoutRect? Bounds { get; init; }

    [JsonProperty("computed")]
    public LayoutRect Computed { get; init; } = new();
}

public sealed class LayoutRect
{
    [JsonProperty("x")]
    public int X { get; init; }

    [JsonProperty("y")]
    public int Y { get; init; }

    [JsonProperty("width")]
    public int Width { get; init; }

    [JsonProperty("height")]
    public int Height { get; init; }
}

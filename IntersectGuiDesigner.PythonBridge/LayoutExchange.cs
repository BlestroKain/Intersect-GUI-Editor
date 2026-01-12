using Newtonsoft.Json;

namespace IntersectGuiDesigner.PythonBridge;

public sealed class LayoutDocument
{
    [JsonProperty("schema_version")]
    public string SchemaVersion { get; init; } = "1.0";

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

    [JsonProperty("bounds", NullValueHandling = NullValueHandling.Ignore)]
    public LayoutRect? Bounds { get; init; }

    [JsonProperty("computed")]
    public LayoutRect Computed { get; init; } = new();

    [JsonProperty("dock", NullValueHandling = NullValueHandling.Ignore)]
    public string? Dock { get; init; }

    [JsonProperty("padding", NullValueHandling = NullValueHandling.Ignore)]
    public LayoutThickness? Padding { get; init; }
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

public sealed class LayoutThickness
{
    [JsonProperty("left")]
    public int Left { get; init; }

    [JsonProperty("top")]
    public int Top { get; init; }

    [JsonProperty("right")]
    public int Right { get; init; }

    [JsonProperty("bottom")]
    public int Bottom { get; init; }
}

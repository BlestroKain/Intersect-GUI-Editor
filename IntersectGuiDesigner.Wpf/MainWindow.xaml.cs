using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using IntersectGuiDesigner.Core;
using IntersectGuiDesigner.PythonBridge;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace IntersectGuiDesigner.Wpf;

public partial class MainWindow : Window
{
    private UiNode? _rootNode;
    private readonly MainWindowViewModel _viewModel = new();
    private const string RendererBackendEnvironmentVariable = "INTERSECT_RENDERER_BACKEND";
    private const string RustRendererExecutableEnvironmentVariable = "INTERSECT_RUST_RENDERER_EXE";
    private const string RustRendererDefaultRelativePath = "Renderers/IntersectGuiDesigner.RustRenderer.exe";
    private const string LayoutSchemaVersion = "1.0";

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _viewModel;
    }

    private void OpenJson_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Open GUI JSON"
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        var rootObject = GuiJsonDocument.Load(dialog.FileName);
        var rootName = Path.GetFileNameWithoutExtension(dialog.FileName);
        var rootNode = UiNodeTreeBuilder.Build(rootName, rootObject);

        _rootNode = rootNode;
        _viewModel.LoadFromRoot(rootNode);
    }

    private void ExportLayoutJson_OnClick(object sender, RoutedEventArgs e)
    {
        if (_rootNode is null)
        {
            MessageBox.Show(this, "Load a GUI JSON file before exporting.", "No layout loaded", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var dialog = new SaveFileDialog
        {
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Export Layout JSON",
            FileName = $"{_rootNode.Name}.layout.json"
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        var layoutDocument = BuildLayoutDocument(_rootNode);
        var json = JsonConvert.SerializeObject(layoutDocument, Formatting.Indented);
        File.WriteAllText(dialog.FileName, json);
    }

    private void RenderWireframe_OnClick(object sender, RoutedEventArgs e)
    {
        if (_rootNode is null)
        {
            MessageBox.Show(this, "Load a GUI JSON file before rendering.", "No layout loaded", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var rendererBackend = ResolveRendererBackend();
        var dialog = new SaveFileDialog
        {
            Filter = rendererBackend == RendererBackend.Rust
                ? "SVG files (*.svg)|*.svg|All files (*.*)|*.*"
                : "PNG files (*.png)|*.png|All files (*.*)|*.*",
            Title = "Render Wireframe",
            FileName = rendererBackend == RendererBackend.Rust
                ? $"{_rootNode.Name}.wireframe.svg"
                : $"{_rootNode.Name}.wireframe.png"
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        var layoutPath = WriteLayoutDocument(_rootNode, dialog.FileName);
        if (rendererBackend == RendererBackend.Rust)
        {
            if (!RenderWithRust(layoutPath, dialog.FileName))
            {
                return;
            }

            if (string.Equals(Path.GetExtension(dialog.FileName), ".png", StringComparison.OrdinalIgnoreCase))
            {
                WireframeImage.Source = LoadImage(dialog.FileName);
            }
            else
            {
                WireframeImage.Source = null;
            }

            return;
        }

        if (!RenderWithPython(layoutPath, dialog.FileName))
        {
            return;
        }

        WireframeImage.Source = LoadImage(dialog.FileName);
    }

    private string WriteLayoutDocument(UiNode rootNode, string outputPath)
    {
        var layoutDocument = BuildLayoutDocument(rootNode);
        var layoutPath = Path.ChangeExtension(outputPath, ".json");
        var json = JsonConvert.SerializeObject(layoutDocument, Formatting.Indented);
        File.WriteAllText(layoutPath, json);
        return layoutPath;
    }

    private bool RenderWithPython(string layoutPath, string outputPath)
    {
        var scriptPath = Path.Combine(AppContext.BaseDirectory, "Scripts", "render_wireframe.py");
        if (!File.Exists(scriptPath))
        {
            MessageBox.Show(this, $"Python renderer script was not found at {scriptPath}.", "Renderer missing", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        var pythonExecutable = Environment.GetEnvironmentVariable("PYTHON_EXE");
        var result = PythonScriptRunner.Run(
            scriptPath,
            new[] { "--input", layoutPath, "--output", outputPath },
            pythonExecutable: pythonExecutable);

        if (result.ExitCode != 0)
        {
            var message = string.IsNullOrWhiteSpace(result.StandardError) ? result.StandardOutput : result.StandardError;
            MessageBox.Show(this, message, "Render failed", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }

    private bool RenderWithRust(string layoutPath, string outputPath)
    {
        var rustExecutable = Environment.GetEnvironmentVariable(RustRendererExecutableEnvironmentVariable);
        if (string.IsNullOrWhiteSpace(rustExecutable))
        {
            rustExecutable = Path.Combine(AppContext.BaseDirectory, RustRendererDefaultRelativePath);
        }

        if (!File.Exists(rustExecutable))
        {
            MessageBox.Show(this, $"Rust renderer executable was not found at {rustExecutable}.", "Renderer missing", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        var result = ExternalProcessRunner.Run(
            rustExecutable,
            new[] { "--input", layoutPath, "--output", outputPath },
            workingDirectory: AppContext.BaseDirectory);

        if (result.ExitCode != 0)
        {
            var message = string.IsNullOrWhiteSpace(result.StandardError) ? result.StandardOutput : result.StandardError;
            MessageBox.Show(this, message, "Render failed", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }

    private static LayoutDocument BuildLayoutDocument(UiNode rootNode)
    {
        var nodes = new List<LayoutNode>();
        var nextY = 20;
        var canvasWidth = 0;
        var canvasHeight = 0;

        void Traverse(UiNode node, string? parentId, int depth)
        {
            var bounds = node.GetBounds();
            var size = node.GetSize();
            var dock = node.GetDock();
            var padding = node.GetPadding();
            var width = bounds?.Width ?? size?.Width ?? 160;
            var height = bounds?.Height ?? size?.Height ?? 48;
            var x = bounds?.X ?? 20 + (depth * 30);
            var y = bounds?.Y ?? nextY;

            if (!bounds.HasValue)
            {
                nextY += height + 20;
            }

            var nodeId = Guid.NewGuid().ToString("N");
            var computed = new LayoutRect
            {
                X = x,
                Y = y,
                Width = width,
                Height = height
            };

            LayoutRect? boundsRect = null;
            if (bounds.HasValue)
            {
                boundsRect = ToLayoutRect(bounds.Value);
            }

            LayoutThickness? paddingThickness = null;
            if (padding.HasValue)
            {
                paddingThickness = ToLayoutThickness(padding.Value);
            }

            nodes.Add(new LayoutNode
            {
                Id = nodeId,
                Name = node.Name,
                ParentId = parentId,
                Bounds = boundsRect,
                Computed = computed,
                Dock = dock?.ToString(),
                Padding = paddingThickness
            });

            canvasWidth = Math.Max(canvasWidth, x + width + 20);
            canvasHeight = Math.Max(canvasHeight, y + height + 20);

            foreach (var child in node.Children)
            {
                Traverse(child, nodeId, depth + 1);
            }
        }

        Traverse(rootNode, null, 0);

        return new LayoutDocument
        {
            SchemaVersion = LayoutSchemaVersion,
            Canvas = new LayoutCanvas
            {
                Width = Math.Max(canvasWidth, 800),
                Height = Math.Max(canvasHeight, 600)
            },
            Nodes = nodes
        };
    }

    private static LayoutRect ToLayoutRect(Rectangle rectangle)
    {
        return new LayoutRect
        {
            X = rectangle.X,
            Y = rectangle.Y,
            Width = rectangle.Width,
            Height = rectangle.Height
        };
    }

    private static LayoutThickness ToLayoutThickness(System.Windows.Forms.Padding padding)
    {
        return new LayoutThickness
        {
            Left = padding.Left,
            Top = padding.Top,
            Right = padding.Right,
            Bottom = padding.Bottom
        };
    }

    private static BitmapImage LoadImage(string path)
    {
        var image = new BitmapImage();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.UriSource = new Uri(path, UriKind.Absolute);
        image.EndInit();
        image.Freeze();
        return image;
    }

    private RendererBackend ResolveRendererBackend()
    {
        var value = Environment.GetEnvironmentVariable(RendererBackendEnvironmentVariable);
        if (string.IsNullOrWhiteSpace(value))
        {
            return RendererBackend.Python;
        }

        return value.Trim().Equals("rust", StringComparison.OrdinalIgnoreCase)
            ? RendererBackend.Rust
            : RendererBackend.Python;
    }

    private enum RendererBackend
    {
        Python,
        Rust
    }
}

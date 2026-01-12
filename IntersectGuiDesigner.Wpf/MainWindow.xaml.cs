using System.Collections.ObjectModel;
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
    public ObservableCollection<UiNodeViewModel> RootNodes { get; } = new();
    private UiNode? _rootNode;
    private readonly MainWindowViewModel _viewModel = new();

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
        RootNodes.Clear();
        RootNodes.Add(UiNodeViewModel.FromNode(rootNode));
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

        var dialog = new SaveFileDialog
        {
            Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*",
            Title = "Render Wireframe",
            FileName = $"{_rootNode.Name}.wireframe.png"
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        var layoutDocument = BuildLayoutDocument(_rootNode);
        var layoutPath = Path.ChangeExtension(dialog.FileName, ".json");
        var json = JsonConvert.SerializeObject(layoutDocument, Formatting.Indented);
        File.WriteAllText(layoutPath, json);

        var scriptPath = Path.Combine(AppContext.BaseDirectory, "Scripts", "render_wireframe.py");
        if (!File.Exists(scriptPath))
        {
            MessageBox.Show(this, $"Python renderer script was not found at {scriptPath}.", "Renderer missing", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var pythonExecutable = Environment.GetEnvironmentVariable("PYTHON_EXE");
        var result = PythonScriptRunner.Run(
            scriptPath,
            new[] { "--input", layoutPath, "--output", dialog.FileName },
            pythonExecutable: pythonExecutable);

        if (result.ExitCode != 0)
        {
            var message = string.IsNullOrWhiteSpace(result.StandardError) ? result.StandardOutput : result.StandardError;
            MessageBox.Show(this, message, "Render failed", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        WireframeImage.Source = LoadImage(dialog.FileName);
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

            nodes.Add(new LayoutNode
            {
                Id = nodeId,
                Name = node.Name,
                ParentId = parentId,
                Bounds = boundsRect,
                Computed = computed
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

    public sealed class UiNodeViewModel
    {
        public string Name { get; }
        public ObservableCollection<UiNodeViewModel> Children { get; }

        private UiNodeViewModel(string name, ObservableCollection<UiNodeViewModel> children)
        {
            Name = name;
            Children = children;
        }

        public static UiNodeViewModel FromNode(UiNode node)
        {
            var children = new ObservableCollection<UiNodeViewModel>();
            foreach (var child in node.Children)
            {
                children.Add(FromNode(child));
            }

            return new UiNodeViewModel(node.Name, children);
        }
        _viewModel.LoadFromRoot(rootNode);
    }
}

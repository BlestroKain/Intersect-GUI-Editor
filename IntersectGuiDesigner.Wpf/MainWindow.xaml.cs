using System.IO;
using System.Windows;
using IntersectGuiDesigner.Core;
using Microsoft.Win32;

namespace IntersectGuiDesigner.Wpf;

public partial class MainWindow : Window
{
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

        _viewModel.LoadFromRoot(rootNode);
    }
}

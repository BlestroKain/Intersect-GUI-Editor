using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using IntersectGuiDesigner.Core;
using Microsoft.Win32;

namespace IntersectGuiDesigner.Wpf;

public partial class MainWindow : Window
{
    public ObservableCollection<UiNodeViewModel> RootNodes { get; } = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
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

        RootNodes.Clear();
        RootNodes.Add(UiNodeViewModel.FromNode(rootNode));
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
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using IntersectGuiDesigner.Core;
using IntersectGuiDesigner.DesignerViewModels;

namespace IntersectGuiDesigner.Wpf;

public sealed class MainWindowViewModel : ViewModelBase
{
    private readonly UiTreeViewModel _tree = new();
    private readonly SelectionStateViewModel _selection = new();
    private readonly ObservableCollection<UiNodeViewModel> _wireframeNodes = new();

    public MainWindowViewModel()
    {
        _tree.PropertyChanged += TreeOnPropertyChanged;
    }

    public ObservableCollection<UiNodeViewModel> RootNodes => _tree.RootNodes;

    public ObservableCollection<UiNodeViewModel> WireframeNodes => _wireframeNodes;

    public UiNodeViewModel? SelectedNode
    {
        get => _selection.SelectedNode;
        set
        {
            _selection.SetSelectedNode(value);
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasSelection));
        }
    }

    public bool HasSelection => SelectedNode is not null;

    public void LoadFromRoot(UiNode? root)
    {
        _tree.LoadFromRoot(root);
        RebuildWireframeNodes();
        SelectedNode = RootNodes.Count > 0 ? RootNodes[0] : null;
    }

    private void TreeOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(UiTreeViewModel.RootNodes))
        {
            OnPropertyChanged(nameof(RootNodes));
        }
    }

    private void RebuildWireframeNodes()
    {
        _wireframeNodes.Clear();
        foreach (var root in RootNodes)
        {
            AddNodeAndChildren(root);
        }
    }

    private void AddNodeAndChildren(UiNodeViewModel node)
    {
        _wireframeNodes.Add(node);
        foreach (var child in node.Children)
        {
            AddNodeAndChildren(child);
        }
    }
}

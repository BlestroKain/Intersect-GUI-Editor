namespace IntersectGuiDesigner.DesignerViewModels;

public sealed class SelectionStateViewModel : ViewModelBase
{
    private UiNodeViewModel? _selectedNode;
    private bool _isSynchronizing;

    public UiNodeViewModel? SelectedNode
    {
        get => _selectedNode;
        private set => SetProperty(ref _selectedNode, value);
    }

    public bool IsSynchronizing
    {
        get => _isSynchronizing;
        private set => SetProperty(ref _isSynchronizing, value);
    }

    public void BeginSynchronization()
    {
        IsSynchronizing = true;
    }

    public void EndSynchronization()
    {
        IsSynchronizing = false;
    }

    public void SetSelectedNode(UiNodeViewModel? node)
    {
        SelectedNode = node;
    }
}

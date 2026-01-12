using System.Collections.ObjectModel;
using IntersectGuiDesigner.Core;

namespace IntersectGuiDesigner.DesignerViewModels;

public sealed class UiTreeViewModel : ViewModelBase
{
    private ObservableCollection<UiNodeViewModel> _rootNodes = new();

    public ObservableCollection<UiNodeViewModel> RootNodes
    {
        get => _rootNodes;
        private set => SetProperty(ref _rootNodes, value);
    }

    public void LoadFromRoot(UiNode? root)
    {
        if (root is null)
        {
            RootNodes = new ObservableCollection<UiNodeViewModel>();
            return;
        }

        RootNodes = new ObservableCollection<UiNodeViewModel> { new UiNodeViewModel(root) };
    }

    public UiNodeViewModel? FindByModel(UiNode? target)
    {
        if (target is null)
        {
            return null;
        }

        foreach (var root in RootNodes)
        {
            var match = root.FindByModel(target);
            if (match is not null)
            {
                return match;
            }
        }

        return null;
    }
}

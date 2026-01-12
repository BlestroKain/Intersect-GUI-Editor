using System.Windows;
using System.Windows.Controls;

namespace IntersectGuiDesigner.Wpf;

public static class TreeViewSelectionBehavior
{
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.RegisterAttached(
        "SelectedItem",
        typeof(object),
        typeof(TreeViewSelectionBehavior),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

    private static readonly DependencyProperty IsHookedProperty = DependencyProperty.RegisterAttached(
        "IsHooked",
        typeof(bool),
        typeof(TreeViewSelectionBehavior),
        new PropertyMetadata(false));

    public static object? GetSelectedItem(DependencyObject obj)
    {
        return obj.GetValue(SelectedItemProperty);
    }

    public static void SetSelectedItem(DependencyObject obj, object? value)
    {
        obj.SetValue(SelectedItemProperty, value);
    }

    private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TreeView treeView)
        {
            return;
        }

        if (!(bool)treeView.GetValue(IsHookedProperty))
        {
            treeView.SelectedItemChanged += TreeViewOnSelectedItemChanged;
            treeView.SetValue(IsHookedProperty, true);
        }

        if (e.NewValue is null)
        {
            return;
        }

        if (treeView.ItemContainerGenerator.ContainerFromItem(e.NewValue) is TreeViewItem directItem)
        {
            directItem.IsSelected = true;
            directItem.BringIntoView();
            return;
        }

        if (FindContainer(treeView, e.NewValue) is { } container)
        {
            container.IsSelected = true;
            container.BringIntoView();
        }
    }

    private static void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (sender is TreeView treeView)
        {
            treeView.SetCurrentValue(SelectedItemProperty, e.NewValue);
        }
    }

    private static TreeViewItem? FindContainer(ItemsControl parent, object target)
    {
        foreach (var item in parent.Items)
        {
            if (parent.ItemContainerGenerator.ContainerFromItem(item) is not TreeViewItem container)
            {
                continue;
            }

            if (Equals(item, target))
            {
                return container;
            }

            var childContainer = FindContainer(container, target);
            if (childContainer is not null)
            {
                return childContainer;
            }
        }

        return null;
    }
}

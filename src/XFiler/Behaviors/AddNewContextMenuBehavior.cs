using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace XFiler;

internal sealed class AddNewContextMenuBehavior : Behavior<MenuItem>
{
    #region Attached Properties

    public static readonly DependencyProperty RootItemProperty = DependencyProperty.RegisterAttached(
        "RootItem", typeof(bool), typeof(AddNewContextMenuBehavior),
        new PropertyMetadata(false));

    public static void SetRootItem(DependencyObject element, bool value)
        => element.SetValue(RootItemProperty, value);

    public static bool GetRootItem(DependencyObject element)
        => (bool) element.GetValue(RootItemProperty);

    #endregion

    #region Dependency Properties

    public static readonly DependencyProperty NativeContextMenuLoaderProperty = DependencyProperty.Register(
        nameof(NativeContextMenuLoader), typeof(INativeContextMenuLoader),
        typeof(AddNewContextMenuBehavior),
        new PropertyMetadata(default(INativeContextMenuLoader)));

    public static readonly DependencyProperty FileInfoModelProperty = DependencyProperty.Register(
        nameof(FileInfoModel), typeof(IFileSystemModel),
        typeof(AddNewContextMenuBehavior),
        new PropertyMetadata(default(IFileSystemModel)));
    
    #endregion

    #region Public Properties

    public INativeContextMenuLoader? NativeContextMenuLoader
    {
        get => (INativeContextMenuLoader) GetValue(NativeContextMenuLoaderProperty);
        set => SetValue(NativeContextMenuLoaderProperty, value);
    }

    public IFileSystemModel? FileInfoModel
    {
        get => (IFileSystemModel) GetValue(FileInfoModelProperty);
        set => SetValue(FileInfoModelProperty, value);
    }

    #endregion

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.Loaded += AssociatedObjectOnLoaded;
    }

    private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
    {
        AssociatedObject.Loaded -= AssociatedObjectOnLoaded; 
        
        if (NativeContextMenuLoader != null && FileInfoModel != null)
            LoadNativeContextMenuItems(NativeContextMenuLoader, FileInfoModel);
    }

    #region Private Methods

    private void LoadNativeContextMenuItems(INativeContextMenuLoader loader, IFileSystemModel parentInfo)
        {
        ClearOldItems();

        var rootItem = AssociatedObject.Items.OfType<DependencyObject>()
            .FirstOrDefault(GetRootItem);

        var index = AssociatedObject.Items.Count;

        if (rootItem != null)
            index = AssociatedObject.Items.IndexOf(rootItem) + 1;

        var models = loader.AddItems;

        foreach (var model in models)
            AssociatedObject.Items.Insert(index++, CreateRegistryMenuItem(loader, model, parentInfo));
    }

    private void ClearOldItems()
    {
        if (AssociatedObject.Items is { } items)
        {
            var oldItems = items.OfType<MenuItem>()
                .Where(i => i.DataContext is IAddNewContextMenuModel)
                .ToList();

            foreach (var oldItem in oldItems)
                AssociatedObject.Items.Remove(oldItem);
        }
    }

    private static MenuItem CreateRegistryMenuItem(INativeContextMenuLoader loader, IAddNewContextMenuModel model,
        IFileSystemModel parentModel)
    {
        MenuItem item = new()
        {
            Header = model.Name,
            DataContext = model,
            Command = loader.InvokeAddNewItemsCommand,
            CommandParameter = new Tuple<IAddNewContextMenuModel, IFileSystemModel>(model, parentModel),
            Icon = model.Icon,
        };

        return item;
    }

    #endregion
}
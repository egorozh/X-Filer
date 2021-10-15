using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Input;

namespace XFiler;

internal sealed class RegistryContextMenuBehavior : Behavior<ContextMenu>
{
    #region Attached Properties

    public static readonly DependencyProperty RootItemProperty = DependencyProperty.RegisterAttached(
        "RootItem", typeof(bool), typeof(RegistryContextMenuBehavior),
        new PropertyMetadata(false));

    public static void SetRootItem(DependencyObject element, bool value)
        => element.SetValue(RootItemProperty, value);

    public static bool GetRootItem(DependencyObject element)
        => (bool) element.GetValue(RootItemProperty);

    #endregion

    #region Dependency Properties

    public static readonly DependencyProperty ModelsProperty = DependencyProperty.Register(
        nameof(Models), typeof(IReadOnlyCollection<IRegistryContextMenuModel>),
        typeof(RegistryContextMenuBehavior),
        new PropertyMetadata(null, ModelsChangedCallback));

    private static void ModelsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RegistryContextMenuBehavior b)
            b.ModelsChanged();
    }

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command), typeof(ICommand), typeof(RegistryContextMenuBehavior),
        new PropertyMetadata(default(ICommand)));

    public static readonly DependencyProperty FileInfoModelProperty = DependencyProperty.Register(
        nameof(FileInfoModel), typeof(IFileSystemModel),
        typeof(RegistryContextMenuBehavior), new PropertyMetadata(default(IFileSystemModel)));

    #endregion

    #region Public Properties

    public IReadOnlyCollection<IRegistryContextMenuModel>? Models
    {
        get => (IReadOnlyCollection<IRegistryContextMenuModel>) GetValue(ModelsProperty);
        set => SetValue(ModelsProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand) GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public IFileSystemModel FileInfoModel
    {
        get => (IFileSystemModel) GetValue(FileInfoModelProperty);
        set => SetValue(FileInfoModelProperty, value);
    }

    #endregion

    #region Private Methods

    private void ModelsChanged()
    {
        if (Models == null)
            return;

        var rootItem = AssociatedObject.Items.OfType<MenuItem>()
            .FirstOrDefault(GetRootItem);

        var index = AssociatedObject.Items.Count;

        if (rootItem != null)
            index = AssociatedObject.Items.IndexOf(rootItem) + 1;

        foreach (var model in Models)
            AssociatedObject.Items.Insert(index++, CreateRegistryMenuItem(model));
    }

    private MenuItem CreateRegistryMenuItem(IRegistryContextMenuModel model) => new()
    {
        Header = model.Name,
        Command = Command,
        CommandParameter = new Tuple<IFileSystemModel, string?>(FileInfoModel, model.Command)
    };

    #endregion
}
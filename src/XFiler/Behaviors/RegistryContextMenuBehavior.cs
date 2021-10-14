using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace XFiler;

internal sealed class RegistryContextMenuBehavior : Behavior<ContextMenu>
{
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
        "Command", typeof(ICommand), typeof(RegistryContextMenuBehavior), new PropertyMetadata(default(ICommand)));

    public static readonly DependencyProperty FileInfoModelProperty = DependencyProperty.Register(
        "FileInfoModel", typeof(IFileSystemModel),
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
        if (Models != null)
        {
            foreach (var model in Models)
            {
                AssociatedObject.Items.Add(CreateRegistryMenuItem(model));
            }
        }
    }

    private MenuItem CreateRegistryMenuItem(IRegistryContextMenuModel model)
    {
        return new MenuItem()
        {
            Header = model.Name,
            Command = Command,
            CommandParameter = new Tuple<IFileSystemModel, string?>(FileInfoModel, model.Command)
        };
    }

    #endregion
}
namespace XFiler.MyComputer;

public sealed class FolderItemModel : BaseItemModel
{
    public FolderItemModel(Route route, IIconLoader iconLoader, DelegateCommand<Route> openCommand)
        : base(route, iconLoader, openCommand)
    {
    }
}
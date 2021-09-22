using Prism.Commands;

namespace XFiler.MyComputer
{
    public sealed class FolderItemModel : BaseItemModel
    {
        public FolderItemModel(XFilerRoute route, IIconLoader iconLoader, DelegateCommand<XFilerRoute> openCommand)
            : base(route, iconLoader, openCommand)
        {
        }
    }
}
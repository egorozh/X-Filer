using Prism.Commands;

namespace XFiler.SDK.MyComputer
{
    public class FolderItemModel : BaseItemModel
    {
        public FolderItemModel(XFilerRoute route, IIconLoader iconLoader, DelegateCommand<XFilerRoute> openCommand)
            : base(route, iconLoader, openCommand)
        {
        }
    }
}
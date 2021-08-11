using Prism.Commands;
using XFiler.SDK;

namespace XFiler.MyComputer
{
    public class FolderItemModel : BaseItemModel
    {
        public FolderItemModel(XFilerRoute route, IIconLoader iconLoader, DelegateCommand<XFilerRoute> openCommand)
            : base(route, iconLoader, openCommand)
        {
        }
    }
}
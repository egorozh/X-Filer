using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;

namespace XFiler.SDK
{
    public class MyComputerPageModel : BasePageModel
    {
        #region Private Fields

        #endregion

        #region Public Methods

        public ObservableCollection<FolderItemModel> Folders { get; }

        #endregion

        #region Commands

        public DelegateCommand<XFilerRoute> OpenCommand { get; }

        #endregion

        #region Constructor

        public MyComputerPageModel(IIconLoader iconLoader) : base(typeof(MyComputerPage))
        {
            OpenCommand = new DelegateCommand<XFilerRoute>(OnOpen);

            Folders = new ObservableCollection<FolderItemModel>(
                SpecialRoutes.GetFolders().Select(r => new FolderItemModel(r, iconLoader, OpenCommand)));
        }

        #endregion

        #region Public Methods

        public override void Dispose()
        {
            base.Dispose();

            foreach (var folder in Folders)
            {
                folder.Dispose();
            }
        }

        #endregion

        #region Private Methods

        private void OnOpen(XFilerRoute route)
        {
            GoTo(route);
        }

        #endregion
    }
}
using Autofac.Features.Indexed;
using System.IO;
using System.Windows;
using XFiler.SDK;

namespace XFiler.Base
{
    public class RegularTilesFilesPresenterFactory : BaseFilesPresenterFactory
    {
        private IIndex<string, IFilesPresenter> _presenterFactory;

        public RegularTilesFilesPresenterFactory(IIndex<string, IFilesPresenter> presenterFactory)
            : base("Крупные значки", CreateTemplate())
        {
            _presenterFactory = presenterFactory;
        }

        public override IFilesPresenter CreatePresenter(DirectoryInfo currentDirectory, IFilesGroup group)
        {
            var presenter = _presenterFactory["regularTile"];
            presenter.Init(currentDirectory, group);
            return presenter;
        }

        public override void Dispose()
        {
            base.Dispose();

            _presenterFactory = null!;
        }

        private static DataTemplate CreateTemplate() => new()
        {
            DataType = typeof(TileFilesPresenterViewModel),
            VisualTree = new FrameworkElementFactory(typeof(TileFilePresenter))
        };
    }
}
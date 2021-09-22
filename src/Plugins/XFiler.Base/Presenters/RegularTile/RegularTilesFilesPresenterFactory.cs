using Autofac.Features.Indexed;
using System.IO;
using System.Windows;
using System.Windows.Media;
using XFiler.SDK;

namespace XFiler.Base
{
    public class RegularTilesFilesPresenterFactory : BaseFilesPresenterFactory
    {
        private IIndex<string, IFilesPresenter> _presenterFactory;

        public RegularTilesFilesPresenterFactory(IIndex<string, IFilesPresenter> presenterFactory)
            : base("Крупные значки", CreateTemplate(), CreateIcon(), "2e60a960-5261-413c-b046-278f5753140b")
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

        private static ImageSource CreateIcon()
        {
            const string? data = "M8,4H5C4.447,4,4,4.447,4,5v3c0,0.552,0.447,1,1,1h3c0.553,0,1-0.448,1-1V5  C9,4.448,8.553,4,8,4z M15,4h-3c-0.553,0-1,0.447-1,1v3c0,0.552,0.447,1,1,1h3c0.553,0,1-0.448,1-1V5C16,4.448,15.553,4,15,4z M8,11  H5c-0.553,0-1,0.447-1,1v3c0,0.552,0.447,1,1,1h3c0.553,0,1-0.448,1-1v-3C9,11.448,8.553,11,8,11z M15,11h-3c-0.553,0-1,0.447-1,1v3  c0,0.552,0.447,1,1,1h3c0.553,0,1-0.448,1-1v-3C16,11.448,15.553,11,15,11z";

            return new DrawingImage(new GeometryDrawing(Brushes.White,
                new Pen(Brushes.White, 0), Geometry.Parse(data)));
        }
    }
}
using Autofac.Features.Indexed;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using XFiler.Base.Localization;
using XFiler.SDK;

namespace XFiler.Base
{
    public class LargeTilesFilesPresenterFactory : BaseFilesPresenterFactory
    {
        private IIndex<string, IFilesPresenter> _presenterFactory;

        public LargeTilesFilesPresenterFactory(IIndex<string, IFilesPresenter> presenterFactory)
            : base(Strings.Presenters_LargeTiles, CreateTemplate(), CreateIcon(),
                "0afc4046-3926-4eb5-ad10-44101eafa518")
        {
            _presenterFactory = presenterFactory;
        }
            
        public override IFilesPresenter CreatePresenter(DirectoryInfo currentDirectory, IFilesGroup group)
        {
            var presenter = _presenterFactory["largeTile"];
            presenter.Init(currentDirectory, group);
            return presenter;
        }

        public override void Dispose()
        {
            base.Dispose();

            _presenterFactory = null!;
        }

        private static DataTemplate CreateTemplate()
        {
            const string? xaml = @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                  xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" 
                                  xmlns:base=""clr-namespace:XFiler.Base;assembly=XFiler.Base""
                                  DataType=""{ x:Type base:SmallTileFilesPresenterViewModel}"">
                        <base:TileFilePresenter TileSize=""256.0""/>
                    </DataTemplate>";

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(xaml));

            return (DataTemplate)XamlReader.Load(ms);
        }

        private static ImageSource CreateIcon()
        {
            const string? data = "M1536 1120v-960c0 -159 -129 -288 -288 -288h-960c-159 0 -288 129 -288 288v960c0 159 129 288 288 288h960c159 0 288 -129 288 -288z";

            return new DrawingImage(new GeometryDrawing(Brushes.White,
                new Pen(Brushes.White, 0), Geometry.Parse(data)));
        }
    }
}
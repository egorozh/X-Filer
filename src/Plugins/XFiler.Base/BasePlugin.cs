using Autofac;
using XFiler.Base.Icons;
using XFiler.SDK;
using XFiler.SDK.Plugins;

namespace XFiler.Base
{
    public sealed class BasePlugin : IPlugin
    {
        public void Load(ContainerBuilder services)
        {
            services.RegisterType<GridFilesPresenterViewModel>().Keyed<IFilesPresenter>("grid");
            services.RegisterType<SmallTileFilesPresenterViewModel>().Keyed<IFilesPresenter>("smallTile");
            services.RegisterType<TileFilesPresenterViewModel>().Keyed<IFilesPresenter>("regularTile");

            
            services.RegisterType<GridFilesPresenterFactory>().As<IFilesPresenterFactory>();
            services.RegisterType<SmallTilesFilesPresenterFactory>().As<IFilesPresenterFactory>();
            services.RegisterType<RegularTilesFilesPresenterFactory>().As<IFilesPresenterFactory>();
            
            services.RegisterType<SvgIconProvider>().As<IIconProvider>().SingleInstance();
        }
    }
}
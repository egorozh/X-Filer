using Autofac;
using XFiler.SDK;

namespace XFiler.Base
{
    public class BasePlugin : Module
    {
        protected override void Load(ContainerBuilder services)
        {
            base.Load(services);

            services.RegisterType<GridFilesPresenterViewModel>().Keyed<IFilesPresenter>("grid");
            services.RegisterType<TileFilesPresenterViewModel>().Keyed<IFilesPresenter>("regularTile");

            services.RegisterType<RegularTilesFilesPresenterFactory>().As<IFilesPresenterFactory>();
            services.RegisterType<GridFilesPresenterFactory>().As<IFilesPresenterFactory>();
        }
    }
}
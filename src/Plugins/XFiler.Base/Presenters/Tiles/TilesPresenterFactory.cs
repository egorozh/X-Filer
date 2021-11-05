using Autofac.Features.Indexed;
using XFiler.Base.Localization;

namespace XFiler.Base;

internal class TilesPresenterFactory : BaseFilesPresenterFactory
{
    private IIndex<PresenterType, IFilesPresenter> _presenterFactory;

    public TilesPresenterFactory(IIndex<PresenterType, IFilesPresenter> presenterFactory)
        : base(Strings.Presenters_Tiles, CreateTemplate(), CreateIcon(),
            "f8588c78-18d0-4c39-a4ca-d637f7e46321")
    {
        _presenterFactory = presenterFactory;
    }

    public override IFilesPresenter CreatePresenter(DirectoryInfo currentDirectory, IFilesGroup @group,
        IFilesSorting filesSorting)
    {
        var presenter = _presenterFactory[PresenterType.Tiles];
        presenter.Init(currentDirectory, group, filesSorting);
        return presenter;
    }

    public override void Dispose()
    {
        base.Dispose();

        _presenterFactory = null!;
    }

    private static DataTemplate CreateTemplate() => new()
    {
        DataType = typeof(TilesPresenterModel),
        VisualTree = new FrameworkElementFactory(typeof(TileFilePresenter))
    };

    private static ImageSource CreateIcon()
    {
        const string? data =
            "M960 832v-128h-576v128h576zM1024 896h-704v-256h704v256zM0 896h256v-256h-256v256zM960 512v-128h-576v128h576zM1024 576h-704v-256h704v256zM0 576h256v-256h-256v256zM960 192v-128h-576v128h576zM1024 256h-704v-256h704v256zM0 256h256v-256h-256v256z";

        return new DrawingImage(new GeometryDrawing(Brushes.White,
            new Pen(Brushes.White, 0), Geometry.Parse(data)));
    }
}
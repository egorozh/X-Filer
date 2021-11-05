using Autofac.Features.Indexed;
using XFiler.Base.Localization;

namespace XFiler.Base;

internal class ContentPresenterFactory : BaseFilesPresenterFactory
{
    private IIndex<PresenterType, IFilesPresenter> _presenterFactory;

    public ContentPresenterFactory(IIndex<PresenterType, IFilesPresenter> presenterFactory)
        : base(Strings.Presenters_Content, CreateTemplate(), CreateIcon(),
            "267a5a31-e768-4289-a45c-795510f74741")
    {
        _presenterFactory = presenterFactory;
    }

    public override IFilesPresenter CreatePresenter(DirectoryInfo currentDirectory, IFilesGroup @group,
        IFilesSorting filesSorting)
    {
        var presenter = _presenterFactory[PresenterType.Content];
        presenter.Init(currentDirectory, @group, filesSorting);
        return presenter;
    }

    public override void Dispose()
    {
        base.Dispose();

        _presenterFactory = null!;
    }

    private static DataTemplate CreateTemplate() => new()
    {
        DataType = typeof(ContentPresenterViewModel),
        VisualTree = new FrameworkElementFactory(typeof(ContentFilePresenter))
    };

    private static ImageSource CreateIcon()
    {
        const string? data =
            "M2 14H8V20H2M16 8H10V10H16M2 10H8V4H2M10 4V6H22V4M10 20H16V18H10M10 16H22V14H10";

        return new DrawingImage(new GeometryDrawing(Brushes.White,
            new Pen(Brushes.White, 0), Geometry.Parse(data)));
    }
}
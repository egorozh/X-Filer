using Autofac.Features.Indexed;
using XFiler.Base.Localization;

namespace XFiler.Base;

internal class RegularIconsPresenterFactory : BaseFilesPresenterFactory
{
    private IIndex<PresenterType, IFilesPresenter> _presenterFactory;

    public RegularIconsPresenterFactory(IIndex<PresenterType, IFilesPresenter> presenterFactory)
        : base(Strings.Presenters_RegularIcons, CreateTemplate(), CreateIcon(), "2e60a960-5261-413c-b046-278f5753140b")
    {
        _presenterFactory = presenterFactory;
    }

    public override IFilesPresenter CreatePresenter(DirectoryInfo currentDirectory, IFilesGroup @group,
        IFilesSorting filesSorting)
    {
        var presenter = _presenterFactory[PresenterType.RegularIcons];
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
        DataType = typeof(RegularIconsPresenterViewModel),
        VisualTree = new FrameworkElementFactory(typeof(IconsPresenter))
    };

    private static ImageSource CreateIcon()
    {
        const string? data = "M8,4H5C4.447,4,4,4.447,4,5v3c0,0.552,0.447,1,1,1h3c0.553,0,1-0.448,1-1V5  C9,4.448,8.553,4,8,4z M15,4h-3c-0.553,0-1,0.447-1,1v3c0,0.552,0.447,1,1,1h3c0.553,0,1-0.448,1-1V5C16,4.448,15.553,4,15,4z M8,11  H5c-0.553,0-1,0.447-1,1v3c0,0.552,0.447,1,1,1h3c0.553,0,1-0.448,1-1v-3C9,11.448,8.553,11,8,11z M15,11h-3c-0.553,0-1,0.447-1,1v3  c0,0.552,0.447,1,1,1h3c0.553,0,1-0.448,1-1v-3C16,11.448,15.553,11,15,11z";

        return new DrawingImage(new GeometryDrawing(Brushes.White,
            new Pen(Brushes.White, 0), Geometry.Parse(data)));
    }
}
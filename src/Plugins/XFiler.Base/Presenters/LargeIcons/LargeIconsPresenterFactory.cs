using Autofac.Features.Indexed;
using XFiler.Base.Localization;

namespace XFiler.Base;

internal class LargeIconsPresenterFactory : BaseFilesPresenterFactory
{
    private IIndex<PresenterType, IFilesPresenter> _presenterFactory;

    public LargeIconsPresenterFactory(IIndex<PresenterType, IFilesPresenter> presenterFactory)
        : base(Strings.Presenters_LargeIcons, CreateTemplate(), CreateIcon(),
            "0afc4046-3926-4eb5-ad10-44101eafa518")
    {
        _presenterFactory = presenterFactory;
    }
            
    public override IFilesPresenter CreatePresenter(DirectoryInfo currentDirectory, IFilesGroup @group,
        IFilesSorting filesSorting)
    {
        var presenter = _presenterFactory[PresenterType.LargeIcons];
        presenter.Init(currentDirectory, @group, filesSorting);
        return presenter;
    }

    public override void Dispose()
    {
        base.Dispose();

        _presenterFactory = null!;
    }

    private static DataTemplate CreateTemplate()
    {
        var factory = new FrameworkElementFactory(typeof(IconsPresenter));
        factory.SetValue(IconsPresenter.TileSizeProperty, 256.0);
        return new DataTemplate
        {
            DataType = typeof(LargeIconsPresenterViewModel),
            VisualTree = factory
        };
    }

    private static ImageSource CreateIcon()
    {
        const string? data = "M1536 1120v-960c0 -159 -129 -288 -288 -288h-960c-159 0 -288 129 -288 288v960c0 159 129 288 288 288h960c159 0 288 -129 288 -288z";

        var gd = new GeometryDrawing
        {
            Geometry = Geometry.Parse(data),
            Brush = Brushes.White
        };

        //var binding = new Binding("Foreground")
        //{
        //    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor)
        //    {
        //        AncestorType = typeof(ListBox)
        //    }
        //};
            
        //BindingOperations.SetBinding(gd, GeometryDrawing.BrushProperty, binding);

        return new DrawingImage(gd);
    }
}
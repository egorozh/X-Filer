using Autofac.Features.Indexed;
using System.Windows.Markup;
using XFiler.Base.Localization;

namespace XFiler.Base;

internal class GridFilesPresenterFactory : BaseFilesPresenterFactory
{
    private IIndex<PresenterType, IFilesPresenter> _presenterFactory;

    public GridFilesPresenterFactory(IIndex<PresenterType, IFilesPresenter> presenterFactory)
        : base(Strings.Presenters_Table, CreateTemplate(), CreateIcon(), "9a5d97b9-628d-45fd-b36f-89936f3c9506")
    {
        _presenterFactory = presenterFactory;
    }
        
    public override IFilesPresenter CreatePresenter(DirectoryInfo currentDirectory, IFilesGroup @group,
        IFilesSorting filesSorting)
    {
        var presenter = _presenterFactory[PresenterType.Grid];
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
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(
            @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                  xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" 
                                  xmlns:base=""clr-namespace:XFiler.Base;assembly=XFiler.Base""
                                  DataType=""{ x:Type base:GridFilesPresenterViewModel}"">
                        <base:GridFilesPresenter />
                    </DataTemplate>"));

        return (DataTemplate)XamlReader.Load(ms);
    }

    private static ImageSource CreateIcon()
    {
        const string? data =
            "M0 262.5A37.5 37.5 0 0 0 37.5 300H262.5A37.5 37.5 0 0 0 300 262.5V37.5A37.5 37.5 0 0 0 262.5 0H37.5A37.5 37.5 0 0 0 0 37.5V262.5zM281.25 225H206.25V168.75H281.25V225zM281.25 150H206.25V93.75H281.25V150zM281.25 75H206.25V18.75H262.5A18.75 18.75 0 0 1 281.25 37.5V75zM187.5 18.75V75H112.5V18.75H187.5zM93.75 18.75V75H18.75V37.5A18.75 18.75 0 0 1 37.5 18.75H93.75zM18.75 93.75H93.75V150H18.75V93.75zM18.75 168.75H93.75V225H18.75V168.75zM112.5 225V168.75H187.5V225H112.5zM187.5 150H112.5V93.75H187.5V150z";

        return new DrawingImage(new GeometryDrawing(Brushes.White,
            new Pen(Brushes.White, 0), Geometry.Parse(data)));
    }
}
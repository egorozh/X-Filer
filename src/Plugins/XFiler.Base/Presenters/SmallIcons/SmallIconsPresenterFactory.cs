using Autofac.Features.Indexed;
using System.Windows.Markup;
using XFiler.Base.Localization;

namespace XFiler.Base;

internal class SmallIconsPresenterFactory : BaseFilesPresenterFactory
{
    private IIndex<PresenterType, IFilesPresenter> _presenterFactory;

    public SmallIconsPresenterFactory(IIndex<PresenterType, IFilesPresenter> presenterFactory)
        : base(Strings.Presenters_SmallIcons, CreateTemplate(), CreateIcon(),
            "ab0db79f-14a4-4818-adbf-f3441ec28f9c")
    {
        _presenterFactory = presenterFactory;
    }

    public override IFilesPresenter CreatePresenter(DirectoryInfo currentDirectory, IFilesGroup @group,
        IFilesSorting filesSorting)
    {
        var presenter = _presenterFactory[PresenterType.SmallIcons];
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
        const string? xaml = @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                  xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" 
                                  xmlns:base=""clr-namespace:XFiler.Base;assembly=XFiler.Base""
                                  DataType=""{ x:Type base:SmallIconsPresenterViewModel}"">
                        <base:IconsPresenter TileSize=""70.0""/>
                    </DataTemplate>";

        var ms = new MemoryStream(Encoding.UTF8.GetBytes(xaml));

        return (DataTemplate)XamlReader.Load(ms);
    }

    private static ImageSource CreateIcon()
    {
        const string? data =
            "M18.75 262.5A18.75 18.75 0 0 0 37.5 281.25H75A18.75 18.75 0 0 0 93.75 262.5V225A18.75 18.75 0 0 0 75 206.25H37.5A18.75 18.75 0 0 0 18.75 225V262.5zM112.5 262.5A18.75 18.75 0 0 0 131.25 281.25H168.75A18.75 18.75 0 0 0 187.5 262.5V225A18.75 18.75 0 0 0 168.75 206.25H131.25A18.75 18.75 0 0 0 112.5 225V262.5zM206.25 262.5A18.75 18.75 0 0 0 225 281.25H262.5A18.75 18.75 0 0 0 281.25 262.5V225A18.75 18.75 0 0 0 262.5 206.25H225A18.75 18.75 0 0 0 206.25 225V262.5zM18.75 168.75A18.75 18.75 0 0 0 37.5 187.5H75A18.75 18.75 0 0 0 93.75 168.75V131.25A18.75 18.75 0 0 0 75 112.5H37.5A18.75 18.75 0 0 0 18.75 131.25V168.75zM112.5 168.75A18.75 18.75 0 0 0 131.25 187.5H168.75A18.75 18.75 0 0 0 187.5 168.75V131.25A18.75 18.75 0 0 0 168.75 112.5H131.25A18.75 18.75 0 0 0 112.5 131.25V168.75zM206.25 168.75A18.75 18.75 0 0 0 225 187.5H262.5A18.75 18.75 0 0 0 281.25 168.75V131.25A18.75 18.75 0 0 0 262.5 112.5H225A18.75 18.75 0 0 0 206.25 131.25V168.75zM18.75 75A18.75 18.75 0 0 0 37.5 93.75H75A18.75 18.75 0 0 0 93.75 75V37.5A18.75 18.75 0 0 0 75 18.75H37.5A18.75 18.75 0 0 0 18.75 37.5V75zM112.5 75A18.75 18.75 0 0 0 131.25 93.75H168.75A18.75 18.75 0 0 0 187.5 75V37.5A18.75 18.75 0 0 0 168.75 18.75H131.25A18.75 18.75 0 0 0 112.5 37.5V75zM206.25 75A18.75 18.75 0 0 0 225 93.75H262.5A18.75 18.75 0 0 0 281.25 75V37.5A18.75 18.75 0 0 0 262.5 18.75H225A18.75 18.75 0 0 0 206.25 37.5V75z";

        return new DrawingImage(new GeometryDrawing(Brushes.White,
            new Pen(Brushes.White, 0), Geometry.Parse(data)));
    }
}
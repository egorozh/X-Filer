namespace XFiler.Base;

public partial class IconsPresenter
{
    public static readonly DependencyProperty TileSizeProperty = DependencyProperty.Register(
        nameof(TileSize), typeof(double),
        typeof(IconsPresenter),
        new PropertyMetadata(120.0));

    public double TileSize
    {
        get => (double)GetValue(TileSizeProperty);
        set => SetValue(TileSizeProperty, value);
    }

    public IconsPresenter()
    {
        InitializeComponent();
    }
}
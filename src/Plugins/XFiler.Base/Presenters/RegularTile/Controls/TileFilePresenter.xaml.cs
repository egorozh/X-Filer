using System.Windows;

namespace XFiler.Base
{
    public partial class TileFilePresenter
    {
        public static readonly DependencyProperty TileSizeProperty = DependencyProperty.Register(
            "TileSize", typeof(double),
            typeof(TileFilePresenter),
            new PropertyMetadata(120.0));

        public double TileSize
        {
            get => (double)GetValue(TileSizeProperty);
            set => SetValue(TileSizeProperty, value);
        }

        public TileFilePresenter()
        {
            InitializeComponent();
        }
    }
}
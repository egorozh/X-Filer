namespace XFiler.Controls;

internal class RectSelectLogic<T> : IDisposable where T : Control
{
    #region Private Fields

    private ItemsControl _itemsControl;
    private Canvas _canvas;
    private Action<T> _selectAction;
    private Action<T> _unSelectAction;
    private readonly System.Windows.Shapes.Rectangle _rectangleShape;
    private Point _initPos;
    private bool _isRectSelected;

    #endregion

    #region Constructor

    public RectSelectLogic(ItemsControl itemsControl,
        Canvas canvas,
        Action<T> selectAction,
        Action<T> unSelectAction)
    {
        _itemsControl = itemsControl;
        _canvas = canvas;
        _selectAction = selectAction;
        _unSelectAction = unSelectAction;

        _rectangleShape = new System.Windows.Shapes.Rectangle()
        {
            Fill = new SolidColorBrush(Color.FromArgb(80, 0, 220, 255)),
            Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 220, 255)),
            StrokeThickness = 2,
            Visibility = Visibility.Collapsed
        };

        _canvas.Children.Add(_rectangleShape);
    }

    #endregion

    #region Public Methods

    public void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        _initPos = e.GetPosition(_canvas);
        _isRectSelected = true;
        _rectangleShape.Visibility = Visibility.Visible;
        Mouse.Capture(_itemsControl);
    }

    public void OnMouseMove(MouseEventArgs e)
    {
        if (_isRectSelected)
        {
            var pos = e.GetPosition(_canvas);

            var width = Math.Abs(pos.X - _initPos.X);
            var height = Math.Abs(pos.Y - _initPos.Y);

            var left = Math.Min(pos.X, _initPos.X);
            var top = Math.Min(pos.Y, _initPos.Y);

            _rectangleShape.Width = width;
            _rectangleShape.Height = height;
            Canvas.SetLeft(_rectangleShape, left);
            Canvas.SetTop(_rectangleShape, top);

            SelectItems(new RectangleGeometry(new Rect(new Point(left, top), new Size(width, height))));
        }
    }

    public void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        _rectangleShape.Width = 0;
        _rectangleShape.Height = 0;

        _isRectSelected = false;
        _rectangleShape.Visibility = Visibility.Collapsed;
        Mouse.Capture(null);
    }

    public void Dispose()
    {
        _itemsControl = null!;
        _canvas = null!;
        _selectAction = null!;
        _unSelectAction = null!;
    }

    #endregion

    #region Private Methods

    private void SelectItems(RectangleGeometry rectangleGeometry)
    {
        foreach (var item in _itemsControl.Items)
        {
            var uiItem = (T)_itemsControl.ItemContainerGenerator.ContainerFromItem(item);

            if (uiItem == null)
                continue;

            var pos = uiItem.TranslatePoint(new Point(), _canvas);

            var padding = uiItem.Padding;

            pos = new(pos.X + padding.Left, pos.Y + padding.Top);

            Size itemSize = new(uiItem.ActualWidth - padding.Left - padding.Right,
                uiItem.ActualHeight - padding.Top - padding.Bottom);

            RectangleGeometry itemGeometry = new(new Rect(pos, itemSize));

            var detail = itemGeometry.FillContainsWithDetail(rectangleGeometry);

            if (detail == IntersectionDetail.FullyContains ||
                detail == IntersectionDetail.Intersects ||
                detail == IntersectionDetail.FullyInside)
            {
                _selectAction.Invoke(uiItem);
            }
            else
            {
                _unSelectAction.Invoke(uiItem);
            }
        }
    }

    #endregion
}
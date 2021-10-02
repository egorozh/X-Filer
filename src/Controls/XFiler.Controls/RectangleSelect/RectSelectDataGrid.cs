namespace XFiler.Controls;

public class RectSelectDataGrid : DataGrid
{
    #region Private Fields

    private RectSelectLogic<DataGridRow>? _selectLogic;

    #endregion

    #region Dependency Properties

    public static readonly DependencyProperty HasSelectItemsProperty = DependencyProperty.Register(
        "HasSelectItems", typeof(bool), typeof(RectSelectDataGrid),
        new PropertyMetadata(default(bool)));

    public static readonly DependencyProperty IsManyItemsSelectedProperty = DependencyProperty.Register(
        "IsManyItemsSelected", typeof(bool), typeof(RectSelectDataGrid), new PropertyMetadata(default(bool)));

    #endregion

    #region Public Properties

    public bool HasSelectItems
    {
        get => (bool)GetValue(HasSelectItemsProperty);
        set => SetValue(HasSelectItemsProperty, value);
    }

    public bool IsManyItemsSelected
    {
        get => (bool)GetValue(IsManyItemsSelectedProperty);
        set => SetValue(IsManyItemsSelectedProperty, value);
    }

    #endregion

    #region Static Constructor

    static RectSelectDataGrid()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RectSelectDataGrid),
            new FrameworkPropertyMetadata(typeof(RectSelectDataGrid)));
    }

    #endregion

    #region Public Methods

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild("DG_ScrollViewer") is ScrollViewer scrollViewer)
            scrollViewer.Loaded += ScrollViewerOnLoaded;
    }

    private void ScrollViewerOnLoaded(object sender, RoutedEventArgs e)
    {
        var scrollViewer = (ScrollViewer)sender;

        scrollViewer.Loaded -= ScrollViewerOnLoaded;

        if (scrollViewer.Template.FindName("PART_Canvas", scrollViewer) is Canvas canvas)
        {
            _selectLogic = new RectSelectLogic<DataGridRow>(this, canvas,
                i => i.IsSelected = true, i => i.IsSelected = false);

            Unloaded += RectSelectDataGrid_Unloaded;
        }
    }

    #endregion

    #region Protected Methods

    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
        base.OnSelectionChanged(e);

        HasSelectItems = SelectedItems.Count > 0;
        IsManyItemsSelected = SelectedItems.Count > 1;
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        this.Focus();

        _selectLogic?.OnMouseLeftButtonDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        _selectLogic?.OnMouseMove(e);
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);

        _selectLogic?.OnMouseLeftButtonUp(e);
    }

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseRightButtonDown(e);

        //_selectLogic?.OnMouseLeftButtonDown(e);
        //_selectLogic?.OnMouseLeftButtonUp(e);
    }

    #endregion

    #region Private Methods

    private void RectSelectDataGrid_Unloaded(object sender, RoutedEventArgs e)
    {
        Unloaded -= RectSelectDataGrid_Unloaded;
        _selectLogic?.Dispose();
        _selectLogic = null!;
    }

    #endregion
}
namespace XFiler.Base;

public partial class GridFilesPresenter
{
    public GridFilesPresenter()
    {
        InitializeComponent();

        this.Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        this.Loaded -= OnLoaded;

        RectSelectDataGrid.Focus();
    }
}
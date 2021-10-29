using System.Windows.Controls;
using System.Windows.Input;

namespace XFiler.Base;

public partial class GridFilesPresenter
{
    public GridFilesPresenter()
    {
        InitializeComponent();

        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;

        RectSelectDataGrid.Focus();
        FindResource("OpenCommand");
    }

    private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left && sender is Control control)
        {
            if (FindResource("OpenCommand") is ICommand command) 
                command.Execute(control.DataContext);
        }
    }
}
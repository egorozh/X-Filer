using System.Windows.Media;
using Dragablz;

namespace XFiler;

public sealed partial class MainWindow : IMainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        Closed += OnClosed;
    }

    public void NormalizeAndActivate()
    {
        if (WindowState == WindowState.Minimized)
            WindowState = WindowState.Normal;

        Activate();
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        Closed -= OnClosed;

        //var items = new List<TabablzControl>();

        //FindVisualChild(this, items);

        //foreach (var disposable in items.Select(i => i.DataContext).OfType<IDisposable>()) 
        //    disposable.Dispose();
    }

    private static void FindVisualChild<TItem>(DependencyObject obj, ICollection<TItem> items)
        where TItem : DependencyObject
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            var child = VisualTreeHelper.GetChild(obj, i);

            if (child is TItem item)
                items.Add(item);

            FindVisualChild(child, items);
        }
    }
}
using System.Windows.Controls;
using System.Windows.Data;

namespace XFiler.Base;

public class CustomSortBehaviour
{
    public static readonly DependencyProperty CustomSorterProperty =
        DependencyProperty.RegisterAttached("CustomSorter", typeof(INaturalStringComparer),
            typeof(CustomSortBehaviour));

    public static INaturalStringComparer? GetCustomSorter(DataGridColumn gridColumn) 
        => (INaturalStringComparer)gridColumn.GetValue(CustomSorterProperty);

    public static void SetCustomSorter(DataGridColumn gridColumn, INaturalStringComparer value)
        => gridColumn.SetValue(CustomSorterProperty, value);

    public static readonly DependencyProperty AllowCustomSortProperty =
        DependencyProperty.RegisterAttached("AllowCustomSort", typeof(bool),
            typeof(CustomSortBehaviour), new UIPropertyMetadata(false, OnAllowCustomSortChanged));

    public static bool GetAllowCustomSort(DataGrid grid) 
        => (bool)grid.GetValue(AllowCustomSortProperty);

    public static void SetAllowCustomSort(DataGrid grid, bool value) 
        => grid.SetValue(AllowCustomSortProperty, value);

    private static void OnAllowCustomSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not DataGrid existing)
            return;

        var oldAllow = (bool)e.OldValue;
        var newAllow = (bool)e.NewValue;

        if (!oldAllow && newAllow)
            existing.Sorting += HandleCustomSorting;
        else
            existing.Sorting -= HandleCustomSorting;
    }

    private static void HandleCustomSorting(object sender, DataGridSortingEventArgs e)
    {
        if (sender is not DataGrid dataGrid || !GetAllowCustomSort(dataGrid)) 
            return;

        if (dataGrid.ItemsSource is not ListCollectionView listColView)
            throw new Exception("The DataGrid's ItemsSource property must be of type, ListCollectionView");

        // Sanity check
        var sorter = GetCustomSorter(e.Column);
           
        if (sorter == null)
            return;

        // The guts.
        e.Handled = true;

        var direction = (e.Column.SortDirection != ListSortDirection.Ascending)
            ? ListSortDirection.Ascending
            : ListSortDirection.Descending;

        e.Column.SortDirection = sorter.SortDirection = direction;
        listColView.CustomSort = sorter;
    }
}
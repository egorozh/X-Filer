using System;
using System.ComponentModel;
using System.Windows;

namespace XFiler.SDK;

public class ComparerReference : Freezable, INaturalStringComparer
{
    #region Dependency Properties

    public static readonly DependencyProperty ComparerProperty = DependencyProperty.Register(
        nameof(Comparer), typeof(INaturalStringComparer), typeof(ComparerReference),
        new PropertyMetadata());
        
    #endregion
            
    #region Public Properties

    public INaturalStringComparer? Comparer
    {
        get => (INaturalStringComparer) GetValue(ComparerProperty);
        set => SetValue(ComparerProperty, value);
    }

    #endregion

    #region Freezable

    protected override Freezable CreateInstanceCore() => throw new NotImplementedException();

    #endregion

    public int Compare(object? x, object? y)
    {
        if (Comparer != null) 
            return Comparer.Compare(x, y);

        return 0;
    }

    public int Compare(string? x, string? y)
    {
        if (Comparer != null)
            return Comparer.Compare(x, y);

        return 0;
    }

    public ListSortDirection SortDirection
    {
        get
        {
            if (Comparer != null) 
                return Comparer.SortDirection;

            return ListSortDirection.Ascending;
        }
        set
        {
            if (Comparer != null) 
                Comparer.SortDirection = value;
        }
    }
}
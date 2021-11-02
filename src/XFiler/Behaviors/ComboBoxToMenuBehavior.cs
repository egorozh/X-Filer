using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;

namespace XFiler;

internal sealed class ComboBoxToMenuBehavior : Behavior<MenuItem>
{
    #region Private Fields

    private Dictionary<ICheckedItem, MenuItem> _dictionary = new();

    #endregion

    #region Dependency Properties

    public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
        nameof(Items), typeof(IReadOnlyList<ICheckedItem>), typeof(ComboBoxToMenuBehavior),
        new PropertyMetadata(default(IReadOnlyList<ICheckedItem>),
            ItemsChangedCallback));

    private static void ItemsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((ComboBoxToMenuBehavior)d).ItemsChanged((IReadOnlyList<ICheckedItem>)e.NewValue);
    }

    public static readonly DependencyProperty CurrentItemProperty = DependencyProperty.Register(
        nameof(CurrentItem), typeof(ICheckedItem), typeof(ComboBoxToMenuBehavior),
        new PropertyMetadata(default(ICheckedItem), CurrentItemChangedCallback));

    private static void CurrentItemChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((ComboBoxToMenuBehavior)d).CurrentItemChanged((ICheckedItem)e.NewValue);
    }

    #endregion

    #region Public Properties

    public IReadOnlyList<ICheckedItem> Items
    {
        get => (IReadOnlyList<ICheckedItem>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public ICheckedItem CurrentItem
    {
        get => (ICheckedItem)GetValue(CurrentItemProperty);
        set => SetValue(CurrentItemProperty, value);
    }

    #endregion

    #region Protected Methods

    protected override void OnDetaching()
    {
        base.OnDetaching();

        Unsubscribe();

        _dictionary = null!;
    }

    #endregion

    #region Private Methods

    private void ItemsChanged(IReadOnlyList<ICheckedItem>? items)
    {
        if (items == null)
            return;
            
        foreach (var item in items)
        {
            MenuItem menuItem = new()
            {
                Header = item.Name,
                IsCheckable = true
            };

            menuItem.Checked += MenuItemOnChecked;
            menuItem.Unchecked += MenuItemOnUnchecked;

            _dictionary.Add(item, menuItem);

            AssociatedObject.Items.Add(menuItem);
        }
    }

    private void CurrentItemChanged(ICheckedItem? current)
    {
        if (current == null)
            return;

        if (!_dictionary.ContainsKey(current))
            return;

        var menuItem = _dictionary[current];

        Unsubscribe();

        foreach (var item in AssociatedObject.Items)
            ((MenuItem)item).IsChecked = false;

        menuItem.IsChecked = true;

        Subscribe();
    }

    private void MenuItemOnChecked(object sender, RoutedEventArgs e)
    {
        var presenter = _dictionary.First(p => p.Value == sender).Key;

        if (CurrentItem == presenter)
            return;

        CurrentItem = presenter;
    }

    private void MenuItemOnUnchecked(object sender, RoutedEventArgs e)
    {
        var item = (MenuItem)sender;

        item.IsChecked = true;
    }

    private void Subscribe()
    {
        foreach (var item in _dictionary.Values)
        {
            item.Checked += MenuItemOnChecked;
            item.Unchecked += MenuItemOnUnchecked;
        }
    }

    private void Unsubscribe()
    {
        foreach (var item in _dictionary.Values)
        {
            item.Checked -= MenuItemOnChecked;
            item.Unchecked -= MenuItemOnUnchecked;
        }
    }

    #endregion
}
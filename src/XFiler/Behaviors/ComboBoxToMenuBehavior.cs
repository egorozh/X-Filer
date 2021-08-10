using Microsoft.Xaml.Behaviors;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using XFiler.SDK;

namespace XFiler
{
    internal class ComboBoxToMenuBehavior : Behavior<MenuItem>
    {
        #region Private Fields

        private Dictionary<IFilesPresenterFactory, MenuItem> _dictionary = new();

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items", typeof(IReadOnlyList<IFilesPresenterFactory>), typeof(ComboBoxToMenuBehavior),
            new PropertyMetadata(default(IReadOnlyList<IFilesPresenterFactory>),
                ItemsChangedCallback));

        private static void ItemsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ComboBoxToMenuBehavior)d).ItemsChanged((IReadOnlyList<IFilesPresenterFactory>)e.NewValue);
        }

        public static readonly DependencyProperty CurrentItemProperty = DependencyProperty.Register(
            "CurrentItem", typeof(IFilesPresenterFactory), typeof(ComboBoxToMenuBehavior),
            new PropertyMetadata(default(IFilesPresenterFactory), CurrentItemChangedCallback));

        private static void CurrentItemChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ComboBoxToMenuBehavior)d).CurrentItemChanged((IFilesPresenterFactory)e.NewValue);
        }

        #endregion

        #region Public Properties

        public IReadOnlyList<IFilesPresenterFactory> Items
        {
            get => (IReadOnlyList<IFilesPresenterFactory>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public IFilesPresenterFactory CurrentItem
        {
            get => (IFilesPresenterFactory)GetValue(CurrentItemProperty);
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

        private void ItemsChanged(IReadOnlyList<IFilesPresenterFactory>? items)
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

        private void CurrentItemChanged(IFilesPresenterFactory? current)
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
}
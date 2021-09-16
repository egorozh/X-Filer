using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace XFiler.Controls.MenuEx
{
    [TemplatePart(Name = "PART_Panel", Type = typeof(Panel))]
    public class MenuEx : Menu
    {
        #region Private Fields

        private Panel _panel = null!;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty MainItemsProperty = DependencyProperty.Register(
            nameof(MainItems), typeof(IEnumerable), typeof(MenuEx),
            new PropertyMetadata(default(IEnumerable), MainItemsChangedCallback));

        private static void MainItemsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MenuEx menu)
                menu.SliceItems();
        }

        public static readonly DependencyProperty HidenItemsProperty = DependencyProperty.Register(
            nameof(HidenItems), typeof(IEnumerable), typeof(MenuEx),
            new PropertyMetadata(default(IEnumerable)));

        public static readonly DependencyProperty HiddenAnyProperty = DependencyProperty.Register(
            nameof(HiddenAny), typeof(bool), typeof(MenuEx),
            new PropertyMetadata(default(bool)));

        #endregion

        #region Public Properties

        public IEnumerable? MainItems
        {
            get => (IEnumerable)GetValue(MainItemsProperty);
            set => SetValue(MainItemsProperty, value);
        }

        public IEnumerable HidenItems
        {
            get => (IEnumerable)GetValue(HidenItemsProperty);
            set => SetValue(HidenItemsProperty, value);
        }

        public bool HiddenAny
        {
            get => (bool)GetValue(HiddenAnyProperty);
            set => SetValue(HiddenAnyProperty, value);
        }

        #endregion

        #region Public Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _panel = GetTemplateChild("PART_Panel") as Panel
                     ?? throw new NotImplementedException();
        }

        #endregion
        
        #region Protected Methods

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            SliceItems();
        }

        #endregion

        #region Private Methods

        private void SliceItems()
        {
            if (MainItems == null)
                return;

            ItemsSource = MainItems;

            var layoutBounds = LayoutInformation.GetLayoutSlot(_panel);
            
            var hidenModels = new List<object>();

            foreach (var visualChild in _panel.GetChildren())
            {
                visualChild.UpdateLayout();

                var childBounds = LayoutInformation.GetLayoutSlot(visualChild);

                if (!layoutBounds.Contains(childBounds))
                    hidenModels.Add(visualChild.DataContext);
            }

            HiddenAny = hidenModels.Any();

            HidenItems = hidenModels;
            ItemsSource = MainItems.Cast<object>().Except(hidenModels).ToList();
        }

        #endregion
    }

    public static class MyVisualTreeHelpers
    {
        public static IEnumerable<FrameworkElement> GetChildren(this DependencyObject dependencyObject)
        {
            var numberOfChildren = VisualTreeHelper.GetChildrenCount(dependencyObject);
            
            return (from index in Enumerable.Range(0, numberOfChildren)
                select VisualTreeHelper.GetChild(dependencyObject, index)).Cast<FrameworkElement>();
        }
    }
}
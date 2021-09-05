using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace XFiler.Controls
{
    public class SearchControl : TextBox
    {
        #region Private Fields

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty IsSelectResultsProperty = DependencyProperty.Register(
            "IsSelectResults", typeof(bool), typeof(SearchControl), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty ButtonsContentProperty = DependencyProperty.Register(
            nameof(ButtonsContent), typeof(UIElement), typeof(SearchControl), new PropertyMetadata(default(UIElement)));

        public static readonly DependencyProperty SearchResultsProperty = DependencyProperty.Register(
            "SearchResults", typeof(IReadOnlyList<object>), typeof(SearchControl),
            new PropertyMetadata(default(IReadOnlyList<object>)));

        public static readonly DependencyProperty GetResultsHandlerProperty = DependencyProperty.Register(
            "GetResultsHandler", typeof(Func<string, IReadOnlyList<object>>), typeof(SearchControl),
            new PropertyMetadata(default(Func<string, IReadOnlyList<object>>)));

        public static readonly DependencyProperty CurrentResultProperty = DependencyProperty.Register(
            "CurrentResult", typeof(object), typeof(SearchControl), new PropertyMetadata(default(object)));


        public static readonly DependencyProperty SearchResultTemplateProperty = DependencyProperty.Register(
            "SearchResultTemplate", typeof(DataTemplate), typeof(SearchControl),
            new PropertyMetadata(default(DataTemplate)));

        #endregion

        #region Public Properties

        public bool IsSelectResults
        {
            get => (bool)GetValue(IsSelectResultsProperty);
            set => SetValue(IsSelectResultsProperty, value);
        }

        public UIElement ButtonsContent
        {
            get => (UIElement)GetValue(ButtonsContentProperty);
            set => SetValue(ButtonsContentProperty, value);
        }

        public IReadOnlyList<object>? SearchResults
        {
            get => (IReadOnlyList<object>)GetValue(SearchResultsProperty);
            set => SetValue(SearchResultsProperty, value);
        }

        public object? CurrentResult
        {
            get => GetValue(CurrentResultProperty);
            set => SetValue(CurrentResultProperty, value);
        }

        public Func<string, IReadOnlyList<object>> GetResultsHandler
        {
            get => (Func<string, IReadOnlyList<object>>)GetValue(GetResultsHandlerProperty);
            set => SetValue(GetResultsHandlerProperty, value);
        }

        public DataTemplate SearchResultTemplate
        {
            get => (DataTemplate)GetValue(SearchResultTemplateProperty);
            set => SetValue(SearchResultTemplateProperty, value);
        }

        #endregion

        #region Static Constructor

        static SearchControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchControl),
                new FrameworkPropertyMetadata(typeof(SearchControl)));
        }

        #endregion

        #region Private Methods

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (IsKeyboardFocused) 
                IsSelectResults = true;

            if (IsSelectResults)
            {
                SearchResults = GetResultsHandler?.Invoke(Text);
                CurrentResult = SearchResults?.FirstOrDefault();
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (!IsSelectResults)
                return;

            if (CurrentResult == null || SearchResults == null)
                return;

            var results = SearchResults.ToList();

            var index = results.IndexOf(CurrentResult);

            switch (e.Key)
            {
                case Key.Down:
                    CurrentResult = results[(index + 1) % results.Count];
                    break;

                case Key.Up:
                    var newIndex = index - 1;

                    if (newIndex == -1) newIndex = results.Count - 1;

                    CurrentResult = results[newIndex];

                    break;
            }
        }

        #endregion
    }
}
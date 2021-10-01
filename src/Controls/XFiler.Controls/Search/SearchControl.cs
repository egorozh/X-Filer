namespace XFiler.Controls;

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

    public static readonly DependencyProperty GoToCommandProperty = DependencyProperty.Register(
        "GoToCommand", typeof(ICommand), typeof(SearchControl), new PropertyMetadata(default(ICommand)));

    public static readonly DependencyProperty CurrentResultProperty = DependencyProperty.Register(
        "CurrentResult", typeof(object), typeof(SearchControl), new PropertyMetadata(default(object)));

    public static readonly DependencyProperty SearchResultTemplateProperty = DependencyProperty.Register(
        "SearchResultTemplate", typeof(DataTemplate), typeof(SearchControl),
        new PropertyMetadata(default(DataTemplate)));

    private ListBox _resultsListBox;

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

    public ICommand? GoToCommand
    {
        get => (ICommand)GetValue(GoToCommandProperty);
        set => SetValue(GoToCommandProperty, value);
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

    #region Constructor

    public SearchControl()
    {
        GotKeyboardFocus += TextBox_GotKeyboardFocus;
        //LostMouseCapture += TextBox_LostMouseCapture;
        LostKeyboardFocus += TextBox_LostKeyboardFocus;
    }

    #endregion

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _resultsListBox = GetTemplateChild("Part_ResultsListBox") as ListBox ??
                          throw new NotImplementedException("Part_ResultsListBox not founded in style");

        _resultsListBox.PreviewMouseLeftButtonDown += ResultsListBoxOnMouseLeftButtonDown;
    }

    #region Protected Methods

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

            case Key.Enter:

                GoToCommand?.Execute(CurrentResult);
                IsSelectResults = false;
                break;
        }
    }

    #endregion

    private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        // Fixes issue when clicking cut/copy/paste in context menu
        if (SelectionLength == 0)
            SelectAll();
    }

    private void ResultsListBoxOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (ItemsControl.ContainerFromElement(_resultsListBox,
            (DependencyObject)e.OriginalSource) is ListBoxItem item)
        {
            GoToCommand?.Execute(item.DataContext);
            IsSelectResults = false;
            e.Handled = true;
        }
    }

    private void TextBox_LostMouseCapture(object sender, MouseEventArgs e)
    {
        // If user highlights some text, don't override it
        if (SelectionLength == 0)
            SelectAll();

        // further clicks will not select all
        LostMouseCapture -= TextBox_LostMouseCapture;
    }

    private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        // once we've left the TextBox, return the select all behavior
        LostMouseCapture += TextBox_LostMouseCapture;
    }
}
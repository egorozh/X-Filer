namespace XFiler.Controls;

[TemplatePart(Name = "PART_Panel", Type = typeof(StackPanel))]
public class MenuEx : Menu
{
    #region Private Fields

    private StackPanel _panel = null!;

    #endregion

    #region Dependency Properties

    public static readonly DependencyProperty MainItemsProperty = DependencyProperty.Register(
        nameof(MainItems), typeof(IEnumerable), typeof(MenuEx),
        new PropertyMetadata(default(IEnumerable), MainItemsChangedCallback));

    private static void MainItemsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is MenuEx menu)
            menu.MainItemsChanged(e.OldValue, e.NewValue);
    }

    public static readonly DependencyProperty HidenItemsProperty = DependencyProperty.Register(
        nameof(HidenItems), typeof(IEnumerable), typeof(MenuEx),
        new PropertyMetadata(default(IEnumerable)));

    public static readonly DependencyProperty HiddenAnyProperty = DependencyProperty.Register(
        nameof(HiddenAny), typeof(bool), typeof(MenuEx),
        new PropertyMetadata(default(bool)));

    private bool _lock;

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

        _panel = GetTemplateChild("PART_Panel") as StackPanel
                 ?? throw new NotImplementedException();
    }

    #endregion

    #region Protected Methods

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
            
        if (!_lock)
            SliceItems();
    }

    #endregion

    #region Private Methods

    private void MainItemsChanged(object oldValue, object newValue)
    {
        if (oldValue is INotifyCollectionChanged oldVm)
            oldVm.CollectionChanged -= NotifyVmOnCollectionChanged;

        if (newValue is INotifyCollectionChanged newVm)
            newVm.CollectionChanged += NotifyVmOnCollectionChanged;

        SliceItems();
    }

    private void NotifyVmOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        SliceItems();
    }

    private void SliceItems()
    {
        if (MainItems == null)
            return;

        _lock = true;

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

        _lock = false;
    }

    #endregion
}

public static class MyVisualTreeHelpers
{
    public static IEnumerable<FrameworkElement> GetChildren(this DependencyObject dependencyObject)
    {
        var numberOfChildren = VisualTreeHelper.GetChildrenCount(dependencyObject);

        for (var index = 0; index < numberOfChildren; index++)
        {
            FrameworkElement? element = null;

            try
            {
                element = VisualTreeHelper.GetChild(dependencyObject, index) as FrameworkElement;
            }
            catch (Exception)
            {
                // ignored
            }

            if (element != null)
                yield return element;
        }
    }
}
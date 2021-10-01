namespace XFiler.Controls;

[TemplatePart(Name = PART_TextBlock, Type = typeof(TextBlock))]
[TemplatePart(Name = PART_EditTextBox, Type = typeof(TextBox))]
[TemplatePart(Name = PART_Popup, Type = typeof(Popup))]
public class EditBox : Control
{
    #region Constants

    private const string PART_TextBlock = "PART_TextBlock";
    private const string PART_EditTextBox = "PART_EditTextBox";
    private const string PART_Popup = "PART_Popup";

    #endregion

    #region Private Fields

    private TextBlock _textBlock = null!;
    private TextBox _textBox = null!;
    private Popup _popup = null!;

    private readonly object _lockObject = new();

    private bool _isSubscribed;

    private DateTime _lastClicked;

    private ItemsControl? _parentItemsControl;
    private Window? _rootWindow;

    private ScrollChangedEventHandler? _scrollHandler;
    private RoutedEventHandler? _mouseWheelHandler;

    #endregion

    #region Dependency properties

    /// <summary>
    /// Implements the backing store of the <see cref="DisplayTextForegroundBrush"/>
    /// dependency property which can be used to set the foreground color of the textblock
    /// portion in the control.
    /// </summary>
    public static readonly DependencyProperty DisplayTextForegroundBrushProperty =
        DependencyProperty.Register("DisplayTextForegroundBrush", typeof(SolidColorBrush),
            typeof(EditBox), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0))));

    /// <summary>
    /// TextProperty DependencyProperty should be used to indicate
    /// the string that should be edit in the <seealso cref="EditBox"/> control.
    /// </summary>
    private static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string),
            typeof(EditBox), new FrameworkPropertyMetadata(string.Empty,
                OnTextChangedCallback));

    /// <summary>
    /// This handler method is called when the dependency property <seealso cref="EditBox.TextProperty"/>
    /// is changed in the data source (the ViewModel). The event is raised to tell the view to update its display.
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void OnTextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        d.CoerceValue(TextProperty);
    }

    /// <summary>
    /// Backing storage of DisplayText dependency property should be used to indicate
    /// the string that should displayed when <seealso cref="EditBox"/>
    /// control is not in edit mode.
    /// </summary>
    private static readonly DependencyProperty DisplayTextProperty =
        DependencyProperty.Register("DisplayText",
            typeof(string),
            typeof(EditBox),
            new PropertyMetadata(string.Empty));

    /// <summary>
    /// IsEditingProperty DependencyProperty
    /// </summary>
    private static readonly DependencyProperty IsEditingProperty =
        DependencyProperty.Register(
            "IsEditing",
            typeof(bool),
            typeof(EditBox),
            new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Implement dependency property to determine whether editing data is allowed or not
    /// (control never enters editing mode if IsReadOnly is set to true [default is false])
    /// </summary>
    private static readonly DependencyProperty mIsReadOnlyProperty =
        DependencyProperty.Register("IsReadOnly", typeof(bool),
            typeof(EditBox), new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Send a Rename command request to the ViewModel if renaming has been executed
    /// 
    /// 1> Control entered Edit mode
    /// 2> String changed
    /// 3> Control left Edit Mode (with Enter or F2)
    /// </summary>
    public static readonly DependencyProperty RenameCommandProperty =
        DependencyProperty.Register("RenameCommand",
            typeof(ICommand), typeof(EditBox),
            new UIPropertyMetadata(null));

    // Using a DependencyProperty as the backing store for RenameCommandParameter.  This enables animation, styling, binding, etc...
    private static readonly DependencyProperty RenameCommandParameterProperty =
        DependencyProperty.Register("RenameCommandParameter",
            typeof(object),
            typeof(EditBox), new PropertyMetadata(null));

    #region Mouse Events to trigger renaming with timed mouse click gesture

    /// <summary>
    /// This property can be used to enable/disable mouse "double click"
    /// gesture to start the edit mode (edit mode may also be started via
    /// context menu or F2 key only).
    /// </summary>
    public static readonly DependencyProperty IsEditableOnDoubleClickProperty =
        DependencyProperty.Register("IsEditableOnDoubleClick", typeof(bool), typeof(EditBox),
            new PropertyMetadata(true));

    /// <summary>
    /// The maximum time between the last click and the current one that will allow the user to edit the text block.
    /// This will allow the user to still be able to "double click to close" the TreeViewItem.
    /// Default is 700 ms.
    /// </summary>
    public static readonly DependencyProperty MaximumClickTimeProperty =
        DependencyProperty.Register("MaximumClickTime", typeof(double),
            typeof(EditBox), new UIPropertyMetadata(700d));

    /// <summary>
    /// The minimum time between the last click and the current one that will allow the user to edit the text block.
    /// This will help prevent against accidental edits when they are double clicking.
    /// Default is 300 ms.
    /// </summary>
    public static readonly DependencyProperty MinimumClickTimeProperty =
        DependencyProperty.Register("MinimumClickTime",
            typeof(double), typeof(EditBox), new UIPropertyMetadata(300d));

    #endregion

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets/sets the foreground color of the textblock portion in the control.
    /// </summary>
    public SolidColorBrush DisplayTextForegroundBrush
    {
        get => (SolidColorBrush) GetValue(DisplayTextForegroundBrushProperty);
        set => SetValue(DisplayTextForegroundBrushProperty, value);
    }

    /// <summary>
    /// Gets the text value for editing in the
    /// text portion of the EditBox.
    /// </summary>
    public string Text
    {
        private get { return (string) GetValue(TextProperty); }

        set { SetValue(TextProperty, value); }
    }

    /// <summary>
    /// Gets the text to display.
    /// 
    /// The DisplayText dependency property should be used to indicate
    /// the string that should displayed when <seealso cref="EditBox"/>
    /// control is not in edit mode.
    /// </summary>
    public string DisplayText
    {
        private get { return (string) GetValue(DisplayTextProperty); }
        set { SetValue(DisplayTextProperty, value); }
    }

    /// <summary>
    /// Implement dependency property to determine whether editing data is allowed or not
    /// (control never enters efiting mode if IsReadOnly is set to true [default is false])
    /// </summary>
    public bool IsReadOnly
    {
        get => (bool) GetValue(mIsReadOnlyProperty);
        set => SetValue(mIsReadOnlyProperty, value);
    }

    /// <summary>
    /// Gets Editing mode which is true if the EditBox control
    /// is in editing mode, otherwise false.
    /// </summary>
    public bool IsEditing
    {
        get => (bool) GetValue(IsEditingProperty);

        private set => SetValue(IsEditingProperty, value);
    }

    /// <summary>
    /// Gets the command that is executed (if any is bound)
    /// to request a rename process via viemodel delegation.
    /// 
    /// The command parameter is a tuple containing the new name
    /// (as string) and the bound viewmodel on the datacontext
    /// of this control (as object). The CommandParameter is
    /// created by the control itself an needs no extra binding statement.
    /// </summary>
    public ICommand? RenameCommand
    {
        get => (ICommand) GetValue(RenameCommandProperty);
        set => SetValue(RenameCommandProperty, value);
    }

    /// <summary>
    /// Bind this parameter to supply additional information (such as item renamed)
    /// in rename command binding.
    /// </summary>
    public object? RenameCommandParameter
    {
        get => GetValue(RenameCommandParameterProperty);
        set => SetValue(RenameCommandParameterProperty, value);
    }

    #region Mouse Events to trigger renaming with timed mouse click gesture

    /// <summary>
    /// This property can be used to enable/disable maouse "double click"
    /// gesture to start the edit mode (edit mode may also be started via
    /// context menu or F2 key only).
    /// </summary>
    public bool IsEditableOnDoubleClick
    {
        get => (bool) GetValue(IsEditableOnDoubleClickProperty);
        set => SetValue(IsEditableOnDoubleClickProperty, value);
    }

    /// <summary>
    /// The minimum time between the last click and the current one that will allow the user to edit the text block.
    /// This will help prevent against accidental edits when they are double clicking.
    /// </summary>
    public double MinimumClickTime
    {
        get => (double) GetValue(MinimumClickTimeProperty);
        set => SetValue(MinimumClickTimeProperty, value);
    }

    /// <summary>
    /// The maximum time between the last click and the current one that will allow the user to edit the text block.
    /// This will allow the user to still be able to "double click to close" the TreeViewItem.
    /// </summary>
    public double MaximumClickTime
    {
        get => (double) GetValue(MaximumClickTimeProperty);
        set => SetValue(MaximumClickTimeProperty, value);
    }

    #endregion

    #endregion

    #region Static Constructor

    static EditBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(EditBox),
            new FrameworkPropertyMetadata(typeof(EditBox)));
    }

    #endregion

    #region Constructor

    public EditBox()
    {
        DataContextChanged += OnDataContextChanged;

        Unloaded += OnEditBox_Unloaded;
    }

    #endregion

    #region Public Methods

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _textBlock = GetTemplateChild(PART_TextBlock) as TextBlock ??
                     throw new NotImplementedException($"{PART_TextBlock} is missing");

        _textBox = GetTemplateChild(PART_EditTextBox) as TextBox ??
                   throw new NotImplementedException($"{PART_EditTextBox} is missing");

        _popup = GetTemplateChild(PART_Popup) as Popup ??
                 throw new NotImplementedException($"{PART_Popup} is missing");

        _textBlock.MouseLeftButtonDown += TextBlock_LeftMouseDown;

        // Doing this here instead of in XAML makes the XAML easier to overview and apply correctly
        // Text="{Binding Path=DisplayText, Mode=OneWay, UpdateSourceTrigger=PropertyChanged, RelativeSource={RelativeSource TemplatedParent}}"
        // Bind TextBox onto editBox control property Text
        Binding binding = new("DisplayText")
        {
            Source = this,
            Mode = BindingMode.OneWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };

        _textBlock.SetBinding(TextBlock.TextProperty, binding);
    }

    #endregion

    #region Private Methods

    #region EditBox Handlers

    /// <summary>
    /// Free notification resources when parent window is being closed.
    /// </summary>
    private void OnEditBox_Unloaded(object sender, EventArgs e)
    {
        if (DataContext is IEditBoxModel editBoxModel)
            editBoxModel.RequestEdit -= ViewModel_RequestEdit;

        DataContextChanged -= OnDataContextChanged;
        Unloaded -= OnEditBox_Unloaded;

        if (_isSubscribed)
        {
            _textBox.LayoutUpdated -= OnTextBoxLayoutUpdated;
            _textBox.KeyDown -= OnTextBoxKeyDown;
            _textBox.LostKeyboardFocus -= OnLostKeyboardFocus;
            _textBox.LostFocus -= OnLostFocus;

            if (_parentItemsControl != null)
            {
                if (_scrollHandler != null)
                    _parentItemsControl.RemoveHandler(ScrollViewer.ScrollChangedEvent, _scrollHandler);
                if (_mouseWheelHandler != null)
                    _parentItemsControl.RemoveHandler(MouseWheelEvent, _mouseWheelHandler);

                _parentItemsControl.MouseDown -= ParentItemsControlOnMouseDown;
                _parentItemsControl.SizeChanged -= ParentItemsControlOnSizeChanged;
            }

            if (_rootWindow != null)
                _rootWindow.LocationChanged -= RootWindowOnLocationChanged;
        }

        if (IsEditing)
        {
            Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideElement);
        }
    }

    /// <summary>
    /// Method is invoked when the datacontext is changed.
    /// This requires changing event hook-up on attached viewmodel to enable
    /// notification event conversion from viewmodel into view.
    /// </summary>
    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is IEditBoxModel oldModel)
            oldModel.RequestEdit -= ViewModel_RequestEdit;

        if (e.NewValue is IEditBoxModel newModel)
            newModel.RequestEdit += ViewModel_RequestEdit;
    }

    /// <summary>
    /// Method is invoked when the viewmodel tells the view: Start to edit the name of the item we represent.
    /// (eg: Start to rename a folder).
    /// </summary>
    private void ViewModel_RequestEdit(object? sender, EventArgs e)
    {
        if (IsEditing == false)
            OnSwitchToEditingMode();
    }

    /// <summary>
    /// Escape Edit Mode when the user clicks outside of the textbox
    /// https://stackoverflow.com/questions/6489032/wpf-remove-focus-when-clicking-outside-of-a-textbox#6489274
    /// 
    /// Thanks for helpful hints to Alaa Ben Fatma.
    /// </summary>
    private void OnMouseDownOutsideElement(object sender, MouseButtonEventArgs e)
    {
        e.Handled = true;
        OnSwitchToNormalMode();
    }

    #endregion

    #region TextBlock Handlers

    private void TextBlock_LeftMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (IsEditableOnDoubleClick == false)
            return;

        var timeBetweenClicks = (DateTime.Now - _lastClicked).TotalMilliseconds;
            
        _lastClicked = DateTime.Now;

        if (timeBetweenClicks > MinimumClickTime && timeBetweenClicks < MaximumClickTime)
        {
            OnSwitchToEditingMode();
            e.Handled = true;
        }
    }

    #endregion

    /// <summary>
    /// Displays the adorner textbox to let the user edit the text string.
    /// </summary>
    private void OnSwitchToEditingMode()
    {
        lock (_lockObject)
        {
            if (IsReadOnly == false && IsEditing == false)
            {
                if (!_isSubscribed)
                    SubscribeCancelEvents();

                _isSubscribed = true;

                Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideElement);

                _popup.IsOpen = true;
                _textBlock.Visibility = Visibility.Hidden;

                _textBox.Text = Text;

                IsEditing = true;
            }
        }

        _textBox.SelectAll();
    }

    /// <summary>
    /// Walk the visual tree to find the ItemsControl and 
    /// hook into some of its events. This is done to make
    /// sure that editing is cancelled whenever
    /// 
    ///   1> The parent control is scrolling its content
    /// 1.1> The MouseWheel is used
    ///   2> A user clicks outside the adorner control
    ///   3> The parent control changes its size
    /// 
    /// </summary>
    private void SubscribeCancelEvents()
    {
        // try to get the text box focused when layout finishes.
        _textBox.LayoutUpdated += OnTextBoxLayoutUpdated;

        CaptureMouse();

        _textBox.KeyDown += OnTextBoxKeyDown;
        _textBox.LostKeyboardFocus += OnLostKeyboardFocus;
        _textBox.LostFocus += OnLostFocus;

        if (GetDpObjectFromVisualTree(this, typeof(ItemsControl)) is ItemsControl parentItemsControl)
        {
            _parentItemsControl = parentItemsControl;

            // Handle events on parent control and determine whether to switch to Normal mode or stay in editing mode

            _scrollHandler = OnScrollViewerChanged;
            _parentItemsControl.AddHandler(ScrollViewer.ScrollChangedEvent, _scrollHandler);
            _mouseWheelHandler = OnParentItemsMouseWheel;
            _parentItemsControl.AddHandler(MouseWheelEvent, _mouseWheelHandler, true);

            _parentItemsControl.MouseDown += ParentItemsControlOnMouseDown;
            _parentItemsControl.SizeChanged += ParentItemsControlOnSizeChanged;
        }

        if (GetDpObjectFromVisualTree(this, typeof(Window)) is Window rootWindow)
        {
            _rootWindow = rootWindow;
            _rootWindow.LocationChanged += RootWindowOnLocationChanged;
        }
    }

    /// <summary>
    /// Sets IsEditing to false when the ViewItem that contains an EditBox changes its size
    /// </summary>
    private void OnSwitchToNormalMode(bool bCancelEdit = true)
    {
        lock (_lockObject)
        {
            if (IsEditing)
            {
                string sNewName = _textBox.Text;

                if (bCancelEdit == false)
                {
                    if (RenameCommand != null)
                    {
                        var tuple = new Tuple<string, object?>(sNewName, RenameCommandParameter);
                        RenameCommand.Execute(tuple);
                    }
                }
                else
                {
                    _textBox.Text = Text;
                }

                IsEditing = false;

                Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideElement);
                ReleaseMouseCapture();

                //_textBlock.Focus();
                _textBlock.Visibility = Visibility.Visible;
                _popup.IsOpen = false;
            }
        }
    }

    #region TextBox Handlers

    /// <summary>
    /// When Layout finish, if in editable mode, update focus status on TextBox.
    /// </summary>
    private void OnTextBoxLayoutUpdated(object? sender, EventArgs e)
    {
        if (_textBox.IsFocused == false)
            _textBox.Focus();
    }

    /// <summary>
    /// When an EditBox is in editing mode, pressing the ENTER or F2
    /// keys switches the EditBox to normal mode.
    /// </summary>
    private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
    {
        lock (_lockObject)
        {
            if (e.Key == Key.Escape)
            {
                OnSwitchToNormalMode();
                e.Handled = true;

                return;
            }

            // Cancel editing mode (editing string is OK'ed)
            if (IsEditing && e.Key is Key.Enter or Key.F2)
            {
                OnSwitchToNormalMode(false);
                e.Handled = true;
            }
        }
    }

    /// <summary>
    /// If an EditBox loses focus while it is in editing mode, 
    /// the EditBox mode switches to normal mode.
    /// </summary>
    private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        OnSwitchToNormalMode();
    }

    /// <summary>
    /// Ends the editing mode if textbox loses the focus
    /// </summary>
    private void OnLostFocus(object sender, RoutedEventArgs e)
    {
        OnSwitchToNormalMode();
    }

    #endregion

    #region ParentItemsControl Handlers

    /// <summary>
    /// If an EditBox is in editing mode and the content of a ListView is
    /// scrolled, then the EditBox switches to normal mode.
    /// </summary>
    private void OnScrollViewerChanged(object sender, ScrollChangedEventArgs args)
    {
        if (args.HorizontalOffset == 0 && args.VerticalOffset == 0)
            return;

        if (IsEditing && Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
            OnSwitchToNormalMode();
    }

    private void OnParentItemsMouseWheel(object sender, RoutedEventArgs e)
        => OnSwitchToNormalMode();

    private void ParentItemsControlOnMouseDown(object sender, MouseButtonEventArgs e)
        => OnSwitchToNormalMode();

    private void ParentItemsControlOnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (_parentItemsControl != null)
            _textBox.MaxWidth = _parentItemsControl.ActualWidth;

        OnSwitchToNormalMode();
    }

    #endregion

    #region Window Handlers

    private void RootWindowOnLocationChanged(object? sender, EventArgs e) => OnSwitchToNormalMode();

    #endregion

    #region Helpers

    /// <summary>
    /// Walk visual tree to find the first DependencyObject of a specific type.
    /// (This method works for finding a ScrollViewer within a TreeView).
    /// </summary>
    private static DependencyObject? GetDpObjectFromVisualTree(DependencyObject startObject, Type type)
    {
        // Walk the visual tree to get the parent(ItemsControl)
        // of this control
        DependencyObject parent = startObject;

        while (parent != null)
        {
            if (type.IsInstanceOfType(parent))
                break;
            parent = VisualTreeHelper.GetParent(parent);
        }

        return parent;
    }

    /// <summary>
    /// Walk visual tree to find the first DependencyObject of a specific type.
    /// (This method works for finding a ScrollViewer within a TreeView).
    /// Source: http://stackoverflow.com/questions/3963341/get-reference-to-my-wpf-listboxs-scrollviewer-in-c
    /// </summary>
    private static TItem? FindVisualChild<TItem>(DependencyObject obj) where TItem : DependencyObject
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            var child = VisualTreeHelper.GetChild(obj, i);

            if (child is TItem item)
                return item;

            var childOfChild = FindVisualChild<TItem>(child);

            if (childOfChild != null)
                return childOfChild;
        }

        return null;
    }

    #endregion

    #endregion
}
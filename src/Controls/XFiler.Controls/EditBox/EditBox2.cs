namespace XFiler.Controls;

[TemplatePart(Name = PART_TextBlock, Type = typeof(TextBlock))]
[TemplatePart(Name = PART_EditTextBox, Type = typeof(TextBox))]
[TemplatePart(Name = PART_Popup, Type = typeof(Popup))]
public class EditBox2 : Control
{
    #region Constants

    private const string PART_TextBlock = "PART_TextBlock";
    private const string PART_EditTextBox = "PART_EditTextBox";
    private const string PART_Popup = "PART_Popup";

    #endregion

    #region Static Constructor

    static EditBox2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(EditBox),
            new FrameworkPropertyMetadata(typeof(EditBox)));
    }

    #endregion

    #region Dependency properties

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(EditBox),
            new FrameworkPropertyMetadata(string.Empty));

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register(nameof(IsReadOnly), typeof(bool),
            typeof(EditBox), new FrameworkPropertyMetadata(false));

    public static readonly DependencyProperty IsEditableOnDoubleClickProperty =
        DependencyProperty.Register(nameof(IsEditableOnDoubleClick), typeof(bool), typeof(EditBox),
            new PropertyMetadata(true));

    public static readonly DependencyProperty MaximumClickTimeProperty =
        DependencyProperty.Register(nameof(MaximumClickTime), typeof(double),
            typeof(EditBox), new FrameworkPropertyMetadata(700d));

    public static readonly DependencyProperty MinimumClickTimeProperty =
        DependencyProperty.Register(nameof(MinimumClickTime), typeof(double), typeof(EditBox),
            new FrameworkPropertyMetadata(300d));

    #endregion

    #region Public Properties

    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsReadOnly
    {
        get => (bool) GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public bool IsEditableOnDoubleClick
    {
        get => (bool) GetValue(IsEditableOnDoubleClickProperty);
        set => SetValue(IsEditableOnDoubleClickProperty, value);
    }

    public double MinimumClickTime
    {
        get => (double) GetValue(MinimumClickTimeProperty);
        set => SetValue(MinimumClickTimeProperty, value);
    }

    public double MaximumClickTime
    {
        get => (double) GetValue(MaximumClickTimeProperty);
        set => SetValue(MaximumClickTimeProperty, value);
    }

    #endregion
}
using System.Windows;
using System.Windows.Controls;

namespace XFiler.SDK;

public class ToolBarControl : Control
{
    #region Static Constructor

    static ToolBarControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolBarControl),
            new FrameworkPropertyMetadata(typeof(ToolBarControl)));
    }

    #endregion
}
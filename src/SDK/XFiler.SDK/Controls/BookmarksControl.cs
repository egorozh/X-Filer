using System.Windows;
using System.Windows.Controls;

namespace XFiler.SDK;

public class BookmarksControl : Control
{
    #region Static Constructor

    static BookmarksControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(BookmarksControl),
            new FrameworkPropertyMetadata(typeof(BookmarksControl)));
    }

    #endregion
}
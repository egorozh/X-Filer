using System.Windows;
using System.Windows.Controls;

namespace XFiler.SDK;

public class PresenterProgressBar : ProgressBar
{
    #region Static Constructor

    static PresenterProgressBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PresenterProgressBar),
            new FrameworkPropertyMetadata(typeof(PresenterProgressBar)));
    }

    #endregion
}
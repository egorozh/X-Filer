using System;
using System.Windows;

namespace XFiler.SDK
{
    public class MyComputerPageModel : BaseViewModel, IPageModel
    {
        #region Private Fields

        #endregion

        #region Public Properties

        public DataTemplate Template { get; }

        #endregion

        #region Events

        public event EventHandler<HyperlinkEventArgs>? GoToUrl;

        #endregion

        #region Constructor

        public MyComputerPageModel()
        {
            Template = CreateTemplate();
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
        }

        #endregion

        #region Private Methods

        private static DataTemplate CreateTemplate() => new()
        {
            DataType = typeof(MyComputerPageModel),
            VisualTree = new FrameworkElementFactory(typeof(MyComputerPage))
        };

        #endregion
    }
}
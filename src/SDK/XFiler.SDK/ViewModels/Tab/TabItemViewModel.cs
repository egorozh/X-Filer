using System.Windows;

namespace XFiler.SDK
{
    public abstract class TabItemViewModel : BaseViewModel, ITabItem
    {
        #region Public Properties

        public DataTemplate Template { get; }

        public string Header { get; set; }

        public bool IsSelected { get; set; }

        public bool LogicalIndex { get; set; }

        #endregion

        #region Constructor

        protected TabItemViewModel(string header, DataTemplate template)
        {
            Header = header;
            Template = template;
        }

        #endregion

        #region Public Methods

        public virtual void Dispose()
        {
        }

        #endregion
    }
}
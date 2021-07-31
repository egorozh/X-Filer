namespace XFiler.SDK
{
    public abstract class TabItemViewModel : BaseViewModel, ITabItem
    {
        #region Public Properties

        public string Header { get; set; }

        public bool IsSelected { get; set; }

        public bool LogicalIndex { get; set; }

        #endregion

        #region Constructor

        protected TabItemViewModel(string header)
        {
            Header = header;
        }

        #endregion
    }
}
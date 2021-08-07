using Prism.Commands;
using System;
using System.IO;

namespace XFiler.SDK
{
    public class TabItemModel : BaseViewModel, ITabItemModel
    {
        #region Private Fields

        private IDirectoryHistory _history;
        private IPageFactory _pageFactory;
        private string _searchText;

        #endregion

        #region Public Properties

        public IPageModel Page { get; private set; }

        public XFilerUrl Url { get; private set; }

        public string Header { get; set; }

        public bool IsSelected { get; set; }

        public bool LogicalIndex { get; set; }

        public string SearchText
        {
            get => _searchText;
            set => SetSearchText(value);
        }

        #endregion

        #region Commands

        public DelegateCommand<string> AddBookmarkCommand { get; private set; }

        public DelegateCommand MoveBackCommand { get; }

        public DelegateCommand MoveForwardCommand { get; }

        #endregion

        #region Constructor

        public TabItemModel(
            IBookmarksManager bookmarksManager,
            IPageFactory pageFactory,
            XFilerUrl url)
        {
            _pageFactory = pageFactory;

            AddBookmarkCommand = bookmarksManager.AddBookmarkCommand;

            _history = new DirectoryHistory(url);

            MoveBackCommand = new DelegateCommand(OnMoveBack, OnCanMoveBack);
            MoveForwardCommand = new DelegateCommand(OnMoveForward, OnCanMoveForward);

            Url = url;
            Header = url.Header;
            _searchText = url.FullName;
            Page = _pageFactory.CreatePage(Url);

            Page.GoToUrl += PageOnGoToUrl;
            _history.HistoryChanged += History_HistoryChanged;
        }

        public TabItemModel(IBookmarksManager bookmarksManager, IPageFactory pageFactory,
            FileSystemInfo directoryInfo)
            : this(bookmarksManager, pageFactory, new XFilerUrl(directoryInfo))
        {
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            if (Page != null!)
            {
                Page.GoToUrl -= PageOnGoToUrl;
                Page.Dispose();
                Page = null!;
            }

            _history.HistoryChanged -= History_HistoryChanged;

            _history = null!;
            _pageFactory = null!;
            AddBookmarkCommand = null!;
        }

        public void Open(XFilerUrl url)
        {
            var page = _pageFactory.CreatePage(url);

            if (page != null)
            {
                _history.Add(url);
                Url = url;
                Header = url.Header;
                SearchText = url.FullName;

                if (Page != null!)
                    Page.GoToUrl -= PageOnGoToUrl;

                Page = page;
                Page.GoToUrl += PageOnGoToUrl;
            }
        }

        #endregion

        #region Commands Methods

        private bool OnCanMoveForward() => _history.CanMoveForward;

        private void OnMoveForward()
        {
            _history.MoveForward();

            UpdatePage(_history.Current.Url);
        }

        private bool OnCanMoveBack() => _history.CanMoveBack;

        private void OnMoveBack()
        {
            _history.MoveBack();

            UpdatePage(_history.Current.Url);
        }

        #endregion

        #region Private Methods

        private void History_HistoryChanged(object? sender, EventArgs e)
        {
            MoveBackCommand?.RaiseCanExecuteChanged();
            MoveForwardCommand?.RaiseCanExecuteChanged();
        }

        private void SetSearchText(string text)
        {
            _searchText = text;
        }

        private void PageOnGoToUrl(object? sender, HyperlinkEventArgs e)
        {
            Open(e.Url);
        }

        private void UpdatePage(XFilerUrl url)
        {
            Url = url;
            Header = url.Header;
            SearchText = url.FullName;

            if (Page != null!)
                Page.GoToUrl -= PageOnGoToUrl;

            var page = _pageFactory.CreatePage(Url);

            if (page != null)
            {
                Page = page;
                Page.GoToUrl += PageOnGoToUrl;
            }
        }

        #endregion
    }
}
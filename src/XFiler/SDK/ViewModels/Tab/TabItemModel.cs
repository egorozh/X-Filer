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

        public XFilerRoute Route { get; private set; }

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
            XFilerRoute route)
        {
            _pageFactory = pageFactory;

            AddBookmarkCommand = bookmarksManager.AddBookmarkCommand;

            _history = new DirectoryHistory(route);

            MoveBackCommand = new DelegateCommand(OnMoveBack, OnCanMoveBack);
            MoveForwardCommand = new DelegateCommand(OnMoveForward, OnCanMoveForward);

            Route = route;
            Header = route.Header;
            _searchText = route.FullName;
            Page = _pageFactory.CreatePage(Route);

            Page.GoToUrl += PageOnGoToUrl;
            _history.HistoryChanged += History_HistoryChanged;
        }

        public TabItemModel(IBookmarksManager bookmarksManager, IPageFactory pageFactory,
            DirectoryInfo directoryInfo)
            : this(bookmarksManager, pageFactory, new XFilerRoute(directoryInfo))
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

        public void Open(XFilerRoute route)
        {
            if (Route == route)
                return;

            var page = _pageFactory.CreatePage(route);

            if (page != null)
            {
                _history.Add(route);
                Route = route;
                Header = route.Header;
                SearchText = route.FullName;

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

            UpdatePage(_history.Current.Route);
        }

        private bool OnCanMoveBack() => _history.CanMoveBack;

        private void OnMoveBack()
        {
            _history.MoveBack();

            UpdatePage(_history.Current.Route);
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
            Open(e.Route);
        }

        private void UpdatePage(XFilerRoute route)
        {
            Route = route;
            Header = route.Header;
            SearchText = route.FullName;

            if (Page != null!)
                Page.GoToUrl -= PageOnGoToUrl;

            var page = _pageFactory.CreatePage(Route);

            if (page != null)
            {
                Page = page;
                Page.GoToUrl += PageOnGoToUrl;
            }
        }

        #endregion
    }
}
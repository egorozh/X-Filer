using XFiler.History;

namespace XFiler
{
    public sealed class TabItemModel : BaseViewModel, ITabItemModel
    {
        #region Private Fields

        private IDirectoryHistory _history;
        private IPageFactory _pageFactory;
        private ISearchHandler _searchHandler;
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

        public Func<string, IReadOnlyList<object>> GetResultsHandler { get; private set; }

        #endregion

        #region Commands

        public DelegateCommand<object> AddBookmarkCommand { get; private set; }

        public DelegateCommand MoveBackCommand { get; }

        public DelegateCommand MoveForwardCommand { get; }

        public DelegateCommand UpdateCommand { get; }

        public DelegateCommand<ResultsModel> GoToCommand { get; }

        #endregion

        #region Constructor

        public TabItemModel(
            IBookmarksManager bookmarksManager,
            IPageFactory pageFactory,
            ISearchHandler searchHandler,
            XFilerRoute route,
            IPageModel initPage)
        {
            _pageFactory = pageFactory;
            _searchHandler = searchHandler;

            AddBookmarkCommand = bookmarksManager.AddBookmarkCommand;

            _history = new DirectoryHistory(route);

            MoveBackCommand = new DelegateCommand(OnMoveBack, OnCanMoveBack);
            MoveForwardCommand = new DelegateCommand(OnMoveForward, OnCanMoveForward);
            UpdateCommand = new DelegateCommand(OnUpdate);

            GoToCommand = new DelegateCommand<ResultsModel>(OnGoTo);

            Route = route;
            Header = route.Header;
            _searchText = route.FullName;
            Page = initPage;

            GetResultsHandler = GetResultsFilter;

            Page.GoToUrl += PageOnGoToUrl;
            _history.HistoryChanged += History_HistoryChanged;
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
            _searchHandler = null!;
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
                {
                    Page.GoToUrl -= PageOnGoToUrl;
                    Page.Dispose();
                }


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

        private void OnUpdate()
        {
            UpdatePage(_history.Current.Route);
        }

        private void OnGoTo(ResultsModel result)
        {
            if (result is RouteModel routeModel)
            {
                Open(routeModel.Route);
            }
            else if (result is SearchModel searchModel)
            {
                Open(searchModel.Route);
            }
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
            {
                Page.GoToUrl -= PageOnGoToUrl;
                Page.Dispose();
            }

            var page = _pageFactory.CreatePage(Route);

            if (page != null)
            {
                Page = page;
                Page.GoToUrl += PageOnGoToUrl;
            }
        }

        private IReadOnlyList<object> GetResultsFilter(string arg)
            => _searchHandler.GetResultsFilter(arg, Route);

        #endregion
    }
}
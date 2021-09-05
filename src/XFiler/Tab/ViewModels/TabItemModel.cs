using Prism.Commands;
using System;
using System.Collections.Generic;
using XFiler.History;
using XFiler.SDK;

namespace XFiler
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

        public Func<string, IReadOnlyList<object>> GetResultsHandler { get; }

        #endregion

        #region Commands

        public DelegateCommand<IPageModel> AddBookmarkCommand { get; private set; }

        public DelegateCommand MoveBackCommand { get; }

        public DelegateCommand MoveForwardCommand { get; }

        public DelegateCommand UpdateCommand { get; }

        public DelegateCommand<ResultsModel> GoToCommand { get; }

        #endregion

        #region Constructor

        public TabItemModel(
            IBookmarksManager bookmarksManager,
            IPageFactory pageFactory,
            XFilerRoute route,
            IPageModel initPage)
        {
            _pageFactory = pageFactory;

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

        private IReadOnlyList<object> GetResultsFilter(string currentRoute)
        {
            var results = new List<object>();

            if (string.IsNullOrEmpty(currentRoute))
                return results;

            var route = XFilerRoute.FromPathEx(currentRoute);

            if (route != null && route.FullName != Route.FullName)
                results.Add(new RouteModel($"Перейти в {route.Header}", route));

            results.Add(new ResultsModel($"Поиск {currentRoute} по текущей директории"));
            results.Add(new ResultsModel($"Поиск {currentRoute} по всем директориям"));

            return results;
        }

        #endregion
    }

    public class ResultsModel : BaseViewModel
    {
        public string Text { get; }

        public ResultsModel(string text)
        {
            Text = text;
        }
    }

    public class RouteModel : ResultsModel
    {
        public XFilerRoute Route { get; }

        public RouteModel(string text, XFilerRoute route) : base(text)
        {
            Route = route;
        }
    }
}
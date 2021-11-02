using XFiler.History;

namespace XFiler;

internal sealed class TabItemModel : BaseViewModel, ITabItemModel
{
    #region Private Fields

    private IDirectoryHistory _history;
    private IPageFactory _pageFactory;
    private ISearchHandler _searchHandler;

    #endregion

    #region Public Properties

    public IPageModel Page { get; private set; } = null!;

    public Route Route { get; private set; } = null!;

    public string Header { get; set; } = null!;

    public bool IsSelected { get; set; }

    public bool LogicalIndex { get; set; }

    public string? SearchText { get; set; }

    public Func<string, IReadOnlyList<object>> GetResultsHandler { get; private set; }

    #endregion

    #region Commands

    public DelegateCommand<object> AddBookmarkCommand { get; private set; }

    public DelegateCommand MoveBackCommand { get; }

    public DelegateCommand MoveForwardCommand { get; }

    public DelegateCommand UpdateCommand { get; }
        
    public DelegateCommand GoHomeCommand { get; }

    public DelegateCommand<ResultsModel> GoToCommand { get; }

    #endregion

    #region Constructor

    public TabItemModel(
        IBookmarksManager bookmarksManager,
        IPageFactory pageFactory,
        ISearchHandler searchHandler,
        IDirectoryHistory history)
    {
        _pageFactory = pageFactory;
        _searchHandler = searchHandler;
        _history = history;

        AddBookmarkCommand = bookmarksManager.AddBookmarkCommand;

        MoveBackCommand = new DelegateCommand(OnMoveBack, OnCanMoveBack);
        MoveForwardCommand = new DelegateCommand(OnMoveForward, OnCanMoveForward);
        UpdateCommand = new DelegateCommand(OnUpdate);
        GoToCommand = new DelegateCommand<ResultsModel>(OnGoTo);
        GoHomeCommand = new DelegateCommand(GoHome);
        GetResultsHandler = GetResultsFilter;
    }
    
    #endregion

    #region Public Methods

    public void Dispose()
    {
        if (Page != null!)
        {
            Page.GoToUrl -= PageOnGoToUrl;
            Page.Dispose();
        }

        Page = null!;

        _history.HistoryChanged -= History_HistoryChanged;

        _history = null!;
        _pageFactory = null!;
        _searchHandler = null!;
        AddBookmarkCommand = null!;
        GetResultsHandler = null!;
    }

    public void Init(Route route)
    {
        _history.Init(route);
        _history.HistoryChanged += History_HistoryChanged;

        SetPage(route, false);
    }

    public void Open(Route route) => SetPage(route, true);

    #endregion

    #region Commands Methods

    private bool OnCanMoveForward() => _history.CanMoveForward;

    private void OnMoveForward()
    {
        _history.MoveForward();

        SetPage(_history.Current.Route, false);
    }

    private bool OnCanMoveBack() => _history.CanMoveBack;

    private void OnMoveBack()
    {
        _history.MoveBack();

        SetPage(_history.Current.Route, false);
    }

    private void OnUpdate()
    {
        SetPage(_history.Current.Route, false, true);
    }

    private void GoHome()
    {
        Open(SpecialRoutes.MyComputer);
    }

    private void OnGoTo(ResultsModel result)
    {
        if (result is RouteModel routeModel)
            Open(routeModel.Route);
        else if (result is SearchModel searchModel) 
            Open(searchModel.Route);
    }

    #endregion

    #region Private Methods

    private void History_HistoryChanged(object? sender, EventArgs e)
    {
        MoveBackCommand?.RaiseCanExecuteChanged();
        MoveForwardCommand?.RaiseCanExecuteChanged();
    }

    private void PageOnGoToUrl(object? sender, HyperlinkEventArgs e) => Open(e.Route);
    
    private IReadOnlyList<object> GetResultsFilter(string arg)
        => _searchHandler.GetResultsFilter(arg, Route);

    private void SetPage(Route route, bool addToHistory, bool update = false)
    {
        if (!update && Route == route)
            return;

        var page = _pageFactory.CreatePage(route);

        if (page is not InvalidatePage)
        {
            if (addToHistory)
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
}
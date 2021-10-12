namespace XFiler;

internal sealed class SearchHandler : ISearchHandler
{
    private readonly IResultModelFactory _resultModelFactory;

    public SearchHandler(IResultModelFactory resultModelFactory)
    {
        _resultModelFactory = resultModelFactory;
    }

    public IReadOnlyList<ResultsModel> GetResultsFilter(string query, Route currentRoute)
    {
        var results = new List<ResultsModel>();

        if (string.IsNullOrEmpty(query))
            return results;

        var route = Route.FromPath(query);

        if (route != null && route.FullName != currentRoute.FullName)
            results.Add(_resultModelFactory.CreateRouteModel(route));
        else
        {
            switch (currentRoute.Type)
            {
                case RouteType.Directory:
                    results.Add(_resultModelFactory.CreateSearchInDirectoryModel(query, currentRoute));
                    break;
                case RouteType.Drive or RouteType.SystemDrive:
                    results.Add(_resultModelFactory.CreateSearchInDriveModel(query, currentRoute));
                    break;
            }

            results.Add(_resultModelFactory.CreateSearchInAllDrivesModel(query));
        }


        return results;
    }
}
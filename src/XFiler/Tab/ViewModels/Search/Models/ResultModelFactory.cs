using XFiler.Resources.Localization;

namespace XFiler;

internal sealed class ResultModelFactory : IResultModelFactory
{
    public RouteModel CreateRouteModel(Route route) 
        => new(string.Format(Strings.SearchHandler_RouteText, route.Header), route);

    public SearchModel CreateSearchInDirectoryModel(string query,Route currentRoute) 
        => new(string.Format(Strings.SearchHandler_SearchDirectoryText, query,
                currentRoute.Header),
            query,currentRoute.FullName);

    public SearchModel CreateSearchInDriveModel(string query, Route currentRoute)
        => new(string.Format(Strings.SearchHandler_SearchDriveText, query,
                currentRoute.Header),
            query, currentRoute.FullName);

    public SearchModel CreateSearchInAllDrivesModel(string query) 
        => new(string.Format(Strings.SearchHandler_SearchAllDrivesText, query),
            query);
}
using XFiler.Resources.Localization;
using XFiler.SDK;

namespace XFiler
{
    internal class ResultModelFactory : IResultModelFactory
    {
        public RouteModel CreateRouteModel(XFilerRoute route) 
            => new(string.Format(Strings.SearchHandler_RouteText, route.Header), route);

        public SearchModel CreateSearchInDirectoryModel(string query,XFilerRoute currentRoute) 
            => new(string.Format(Strings.SearchHandler_SearchDirectoryText, query,
                currentRoute.Header),
                query,currentRoute.FullName);

        public SearchModel CreateSearchInDriveModel(string query, XFilerRoute currentRoute)
            => new(string.Format(Strings.SearchHandler_SearchDriveText, query,
                currentRoute.Header),
                query, currentRoute.FullName);

        public SearchModel CreateSearchInAllDrivesModel(string query) 
            => new(string.Format(Strings.SearchHandler_SearchAllDrivesText, query),
                query);
    }
}
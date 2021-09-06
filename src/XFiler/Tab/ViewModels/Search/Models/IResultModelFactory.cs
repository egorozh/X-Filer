using XFiler.SDK;

namespace XFiler
{
    internal interface IResultModelFactory
    {
        RouteModel CreateRouteModel(XFilerRoute route);
        SearchModel CreateSearchInDirectoryModel(string query, XFilerRoute currentRoute);
        SearchModel CreateSearchInDriveModel(string query, XFilerRoute currentRoute);
        SearchModel CreateSearchInAllDrivesModel(string query);
    }
}
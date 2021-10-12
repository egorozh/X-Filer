namespace XFiler;

internal interface IResultModelFactory
{
    RouteModel CreateRouteModel(Route route);
    SearchModel CreateSearchInDirectoryModel(string query, Route currentRoute);
    SearchModel CreateSearchInDriveModel(string query, Route currentRoute);
    SearchModel CreateSearchInAllDrivesModel(string query);
}
namespace XFiler;

public class RouteModel : ResultsModel
{
    public Route Route { get; }

    public RouteModel(string text, Route route) : base(text)
    {
        Route = route;
    }
}
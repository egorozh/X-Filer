using XFiler.SDK;

namespace XFiler
{
    public class RouteModel : ResultsModel
    {
        public XFilerRoute Route { get; }

        public RouteModel(string text, XFilerRoute route) : base(text)
        {
            Route = route;
        }
    }
}
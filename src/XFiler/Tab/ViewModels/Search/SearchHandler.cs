using System.Collections.Generic;
using XFiler.SDK;

namespace XFiler
{
    internal class SearchHandler : ISearchHandler
    {
        public IReadOnlyList<object> GetResultsFilter(string newRouteS, XFilerRoute route)
        {
            var results = new List<object>();

            if (string.IsNullOrEmpty(newRouteS))
                return results;

            var newRoute = XFilerRoute.FromPathEx(newRouteS);

            if (newRoute != null && newRoute.FullName != route.FullName)
                results.Add(new RouteModel($"Перейти в {newRoute.Header}", newRoute));

            results.Add(new ResultsModel($"Поиск {newRouteS} по текущей директории"));
            results.Add(new ResultsModel($"Поиск {newRouteS} по всем директориям"));

            return results;
        }
    }
}

using System.Collections.Generic;
using XFiler.SDK;

namespace XFiler
{
    public interface ISearchHandler 
    {
        IReadOnlyList<object> GetResultsFilter(string newRoute, XFilerRoute route);
    }
}
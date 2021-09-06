using System.Collections.Generic;
using XFiler.SDK;

namespace XFiler
{
    public interface ISearchHandler 
    {
        IReadOnlyList<ResultsModel> GetResultsFilter(string query, XFilerRoute currentRoute);
    }
}
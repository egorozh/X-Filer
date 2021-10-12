namespace XFiler;

public interface ISearchHandler 
{
    IReadOnlyList<ResultsModel> GetResultsFilter(string query, Route currentRoute);
}
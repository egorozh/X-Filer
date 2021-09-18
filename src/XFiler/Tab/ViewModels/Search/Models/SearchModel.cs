namespace XFiler
{
    public class SearchModel : ResultsModel
    {
        public SearchModel(string text, string query, string? targetDir = null) 
            : base(text)
        {
            Route = new XFilerRoute(query, targetDir);
        }

        public XFilerRoute Route { get; }
    }
}
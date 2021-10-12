using XFiler.Views;

namespace XFiler.ViewModels;

internal sealed class SearchPageModel : BasePageModel
{
    public void Init(Route route)
    {
        Init(typeof(SearchPage), route);
    }
}
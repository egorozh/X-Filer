using XFiler.Views;

namespace XFiler.ViewModels;

internal sealed class SearchPageModel : BasePageModel
{
    public void Init(XFilerRoute route)
    {
        Init(typeof(SearchPage), route);
    }
}
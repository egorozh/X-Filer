using System.Collections.Generic;

namespace XFiler.SDK
{
    public interface IMenuItemViewModel
    {
        IList<IMenuItemViewModel> Items { get; set; }

        XFilerRoute? Url { get; }
    }
}
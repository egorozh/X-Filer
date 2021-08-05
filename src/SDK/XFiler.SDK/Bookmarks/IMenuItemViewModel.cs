using System.Collections.Generic;

namespace XFiler.SDK
{
    public interface IMenuItemViewModel
    {
        IList<IMenuItemViewModel> Items { get; set; }

        string? Path { get; }
    }
}
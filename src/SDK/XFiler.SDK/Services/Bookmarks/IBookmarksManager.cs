using System.Collections.Generic;
using Prism.Commands;

namespace XFiler.SDK;

/// <summary>
/// Менеджер закладок
/// </summary>
public interface IBookmarksManager
{
    IReadOnlyCollection<IMenuItemViewModel> Bookmarks { get; }
    DelegateCommand<object> AddBookmarkCommand { get; }
}
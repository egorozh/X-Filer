using System.ComponentModel;
using System.Windows.Input;

namespace XFiler;

internal sealed class MenuItemViewModel : DisposableViewModel, IMenuItemViewModel
{
    public string? Path { get; set; }

    public string? Header { get; set; }

    public ICommand? Command { get; set; }

    public DelegateCommand<object> RenameCommand { get; private set; }

    public XFilerRoute? Route { get; }        
        
    public IList<IMenuItemViewModel> Items { get; set; }

    public IIconLoader IconLoader { get; private set; }

    public bool IsSelected { get; set; }

    public event EventHandler? IsSelectedChanged;
        
    public MenuItemViewModel(BookmarkItem bookmarkItem,
        ObservableCollection<IMenuItemViewModel> children,
        ICommand command, 
        IIconLoader iconLoader,
        IRenameService renameService)
    {
        Path = bookmarkItem.Path;

        Items = children;
        IconLoader = iconLoader;

        RenameCommand = renameService.RenameCommand;

        if (Path == null)
        {
            Header = bookmarkItem.BookmarkFolderName;
        }
        else
        {
            Command = command;
            Route = XFilerRoute.FromPath(Path);
            Header = Route.Header;
        }

        PropertyChanged += OnPropertyChanged;
    }

    protected override void Dispose(bool disposing)
    {
        if (!Disposed && disposing)
        {
            PropertyChanged -= OnPropertyChanged;

            RenameCommand = null!;
            Command = null!;
            IconLoader = null!;
        }

        base.Dispose(disposing);
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IsSelected))
        {
            IsSelectedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public BookmarkItem GetItem()
    {
        if (Route == null)
        {
            IList<BookmarkItem> items = new List<BookmarkItem>();

            foreach (var model in Items) 
                items.Add(model.GetItem());

            return new BookmarkItem()
            {
                Path = null,
                BookmarkFolderName = Header,
                Children = items
            };
        }

        return new BookmarkItem()
        {
            Path = Route.FullName
        };
    }
}
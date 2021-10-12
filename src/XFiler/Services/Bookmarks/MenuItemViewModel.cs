using System.ComponentModel;
using System.Windows.Input;

namespace XFiler;

internal sealed class MenuItemViewModel : DisposableViewModel, IMenuItemViewModel
{
    public string? Path { get; set; }

    public string? Header { get; set; }

    public ICommand? Command { get; set; }

    public DelegateCommand<object> RenameCommand { get; private set; }

    public Route? Route { get; private set; }

    public IList<IMenuItemViewModel> Items { get; set; } = null!;

    public IIconLoader IconLoader { get; private set; }

    public bool IsSelected { get; set; }

    public event EventHandler? IsSelectedChanged;

    public MenuItemViewModel(IIconLoader iconLoader,
        IRenameService renameService)
    {
        IconLoader = iconLoader;
        RenameCommand = renameService.RenameCommand;
    }

    public void Init(Route route, ObservableCollection<IMenuItemViewModel> children, ICommand command)
    {
        Route = route;
        Path = route.FullName;
        Header = route.Header;
        Items = children;
        Command = command;

        PropertyChanged += OnPropertyChanged;
    }

    public void Init(string folderHeader, ObservableCollection<IMenuItemViewModel> children)
    {
        Header = folderHeader;
        Items = children;

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
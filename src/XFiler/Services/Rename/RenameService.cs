namespace XFiler;

internal sealed class RenameService : IRenameService
{
    private readonly IFileOperations _fileOperations;
    private readonly ILogger _logger;

    public DelegateCommand<object> RenameCommand { get; }
    public DelegateCommand<object> StartRenameCommand { get; }

    public RenameService(IFileOperations fileOperations, ILogger logger)
    {
        _fileOperations = fileOperations;
        _logger = logger;
        RenameCommand = new DelegateCommand<object>(OnRename);
        StartRenameCommand = new DelegateCommand<object>(OnStartRename);
    }

    private void OnRename(object parameters)
    {
        if (parameters is not Tuple<string, object>(var newName, var parameter))
            return;

        if (parameter is IFileSystemModel fileSystemModel)
        {
            try
            {
                _fileOperations.Rename(fileSystemModel.Info, newName);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Rename Error");
            }
        }

        if (parameter is MenuItemViewModel menuModel)
        {
            if (string.IsNullOrEmpty(newName))
                return;

            menuModel.Header = newName;
        }
    }

    private void OnStartRename(object parameters)
    {
        if (parameters is FileEntityViewModel vm)
            vm.StartRename();
        else if (parameters is IEnumerable e)
        {
            var items = e.OfType<FileEntityViewModel>().ToList();

            if (items.Count == 1)
                items[0].StartRename();
        }
    }
}
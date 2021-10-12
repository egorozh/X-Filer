using Prism.Commands;

namespace XFiler.SDK;

public interface IRenameService
{
    DelegateCommand<object> RenameCommand { get; }
    DelegateCommand<object> StartRenameCommand { get; }
}
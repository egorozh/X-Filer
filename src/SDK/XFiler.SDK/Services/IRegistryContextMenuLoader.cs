using System.Collections.Generic;
using Prism.Commands;

namespace XFiler.SDK;

public interface IRegistryContextMenuLoader
{
    List<IRegistryContextMenuModel> AllEntityContextModels { get; }

    DelegateCommand<object> InvokeRegistryCommand { get; } 
}

public interface IRegistryContextMenuModel
{
    string Name { get; }
    string? IconPath { get; }
    string? Command { get; }
}
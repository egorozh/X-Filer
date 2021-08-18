using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using XFiler.SDK;

namespace XFiler
{
    internal class RenameService : IRenameService
    {
        public DelegateCommand<object> RenameCommand { get; }
        public DelegateCommand<object> StartRenameCommand { get; }

        public RenameService()
        {
            RenameCommand = new DelegateCommand<object>(OnRename);
            StartRenameCommand = new DelegateCommand<object>(OnStartRename);
        }

        private void OnRename(object model)
        {
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
}
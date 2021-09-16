using System;
using System.ComponentModel;

namespace XFiler.SDK
{
    public interface IFilesGroup : INotifyPropertyChanged, IDisposable, ICheckedItem
    {
        string GetGroup(IFileItem fileEntityViewModel);
    }

    public class FilesGroupOfType : DisposableViewModel, IFilesGroup
    {
        public string Name { get; } = "Тип";

        public string GetGroup(IFileItem item)
        {
            return item.Type;
        }
    }

    public class FilesGroupOfName : DisposableViewModel, IFilesGroup
    {
        public string Name { get; } = "Имя";

        public string GetGroup(IFileItem item)
        {
            return item.Name[..1].ToUpper();
        }
    }
}
namespace XFiler.SDK
{
    public class FilesGroupOfType : DisposableViewModel, IFilesGroup
    {
        public string Name { get; } = "Тип";

        public string GetGroup(IFileItem item)
        {
            return item.Type;
        }
    }
}
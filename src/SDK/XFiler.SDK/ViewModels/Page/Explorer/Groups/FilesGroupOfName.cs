namespace XFiler.SDK
{
    public class FilesGroupOfName : DisposableViewModel, IFilesGroup
    {
        public string Name { get; } = "Имя";

        public string GetGroup(IFileItem item)
        {
            return item.Name[..1].ToUpper();
        }
    }
}
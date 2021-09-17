namespace XFiler.SDK
{
    public class FilesGroupOfNone : DisposableViewModel, IFilesGroup
    {
        public string Name { get; } = "Нет";

        public string GetGroup(IFileItem item)
        {
            return null;
        }
    }
}
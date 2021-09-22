namespace XFiler.SDK
{
    public class FilesGroupOfType : DisposableViewModel, IFilesGroup
    {
        public string Name { get; } = "Тип";

        public string GetGroup(IFileItem item)
        {
            return item.Type;
        }

        public string Id { get; } = "ef45927b-b015-4686-a02b-935df331966f";
    }
}
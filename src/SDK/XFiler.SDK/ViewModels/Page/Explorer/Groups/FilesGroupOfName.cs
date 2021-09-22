namespace XFiler.SDK
{
    public class FilesGroupOfName : DisposableViewModel, IFilesGroup
    {
        public string Name { get; } = "Имя";

        public string GetGroup(IFileItem item)
        {
            return item.Name[..1].ToUpper();
        }

        public string Id { get; } = "79e9be04-2a5a-4775-b5c2-4421d07dd7e4";
    }
}
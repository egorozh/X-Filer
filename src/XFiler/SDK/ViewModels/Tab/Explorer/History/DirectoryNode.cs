namespace XFiler.SDK
{
    internal class DirectoryNode
    {
        public DirectoryNode PreviousNode { get; set; }
        public DirectoryNode NextNode { get; set; }

        public string DirectoryPath { get; }
        public string DirectoryPathName { get; }

        public DirectoryNode(string directoryPath, string directoryPathName)
        {
            DirectoryPath = directoryPath;
            DirectoryPathName = directoryPathName;
        }
    }
}
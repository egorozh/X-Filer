namespace XFiler.SDK
{
    internal class DirectoryNode
    {
        public XFilerUrl Url { get; }

        public DirectoryNode? PreviousNode { get; set; }
        public DirectoryNode? NextNode { get; set; }
        
        public DirectoryNode(XFilerUrl url)
        {
            Url = url;
        }
    }
}
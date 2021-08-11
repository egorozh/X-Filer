﻿namespace XFiler.SDK
{
    internal class DirectoryNode
    {
        public XFilerRoute Route { get; }

        public DirectoryNode? PreviousNode { get; set; }
        public DirectoryNode? NextNode { get; set; }
        
        public DirectoryNode(XFilerRoute route)
        {
            Route = route;
        }
    }
}
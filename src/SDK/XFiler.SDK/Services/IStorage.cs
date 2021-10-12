﻿namespace XFiler.SDK;

public interface IStorage
{
    string BaseDirectory { get; }
    
    string ExplorerWallpapersDirectory { get; }
    
    string LogDirectory { get; }

    string ConfigDirectory { get; }

    string DbDirectory { get; }
            
    string Bookmarks { get; }
}
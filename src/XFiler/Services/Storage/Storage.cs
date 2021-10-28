using System.IO;

namespace XFiler;

internal sealed class Storage : IStorage
{
    #region Public Properties

    public string BaseDirectory { get; }

    public string ExplorerWallpapersDirectory { get; }

    public string LogDirectory { get; }

    public string ConfigDirectory { get; }

    public string DbDirectory { get; }

    public string Bookmarks { get; }

    public string ContextMenuFolder { get; }

    public string ContextMenuTxtFile { get; }

    public string ContextMenuTxtFile2 { get; }

    #endregion

    #region Constructors

    private Storage(string directory)
    {
        BaseDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), directory);
        Directory.CreateDirectory(BaseDirectory);

        ExplorerWallpapersDirectory = Path.Combine(BaseDirectory, "Explorer", "Wallpapers");
        Directory.CreateDirectory(ExplorerWallpapersDirectory);

        LogDirectory = Path.Combine(BaseDirectory, "Logs");
        Directory.CreateDirectory(LogDirectory);

        ConfigDirectory = Path.Combine(BaseDirectory, "Config");
        Directory.CreateDirectory(ConfigDirectory);

        DbDirectory = Path.Combine(BaseDirectory, "Data");
        Directory.CreateDirectory(DbDirectory);

        Bookmarks = Path.Combine(BaseDirectory, "bookmarks.json");

        ContextMenuFolder = Path.Combine(BaseDirectory, "ContextMenu");
        Directory.CreateDirectory(ContextMenuFolder);

        ContextMenuTxtFile = Path.Combine(ContextMenuFolder, "cm.txt");
        ContextMenuTxtFile2 = Path.Combine(ContextMenuFolder, "cm2.txt");

        File.WriteAllText(ContextMenuTxtFile, "Egg");
        File.WriteAllText(ContextMenuTxtFile2, "Egg2");
    }

    public Storage() : this("X-Filer")
    {
    }

    #endregion
}
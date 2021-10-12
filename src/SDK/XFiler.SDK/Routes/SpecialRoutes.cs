using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Reflection;
using XFiler.SDK.Localization;

namespace XFiler.SDK;

public static class SpecialRoutes
{
    #region Private Fields

    private static readonly Dictionary<string, Route> Routes;

    #endregion

    #region Public Properties

    public static Route MyComputer { get; }

    public static Route Settings { get; }

    public static Route BookmarksDispatcher { get; }
        
    public static Route Desktop { get; }

    public static Route Downloads { get; }

    public static Route MyDocuments { get; }

    public static Route MyPictures { get; }

    public static Route MyMusic { get; }

    public static Route MyVideos { get; }

    public static Route RecycleBin { get; }

    #endregion

    #region Static Constructor

    static SpecialRoutes()
    {
        MyComputer = new Route(Strings.Routes_MyComputer, "xfiler://mycomputer", RouteType.MyComputer);

        Settings = new Route(Strings.Routes_Settings, "xfiler://settings", RouteType.Settings);

        BookmarksDispatcher =
            new Route(Strings.Routes_BookmarksDispatcher, "xfiler://bookmarks", RouteType.BookmarksDispatcher);

        Desktop = new Route(Strings.Routes_Desktop,
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RouteType.Desktop);

        Downloads = new Route(Strings.Routes_Downloads,
            GetDownloadFolderPath(), RouteType.Downloads);

        MyDocuments = new Route(Strings.Routes_MyDocuments,
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), RouteType.MyDocuments);

        MyPictures = new Route(Strings.Routes_MyPictures,
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), RouteType.MyPictures);

        MyMusic = new Route(Strings.Routes_MyMusic,
            Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), RouteType.MyMusic);

        MyVideos = new Route(Strings.Routes_MyVideos,
            Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), RouteType.MyVideos);

        RecycleBin = new Route(Strings.Routes_RecycleBin,
            @"C:\$Recycle.Bin", RouteType.RecycleBin);

        var type = typeof(SpecialRoutes);

        var routesProps = type.GetProperties(BindingFlags.Public | BindingFlags.Static);

        Routes = new Dictionary<string, Route>();

        foreach (var prop in routesProps)
        {
            if (prop.GetValue(null) is Route value) 
                Routes.Add(value.FullName, value);
        }
    }

    #endregion

    #region Public Methods

    public static Route? GetSpecialUrl(string fullName) => Routes.ContainsKey(fullName)
        ? Routes[fullName]
        : null;

    public static IReadOnlyList<Route> GetFolders() => new List<Route>
    {
        Desktop, Downloads,
        MyDocuments, MyPictures,
        MyMusic, MyVideos,
        //RecycleBin
    };

    #endregion

    private static string GetDownloadFolderPath() => Registry
        .GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders",
            "{374DE290-123F-4565-9164-39C4925E467B}", string.Empty)
        ?.ToString() ?? string.Empty;
}
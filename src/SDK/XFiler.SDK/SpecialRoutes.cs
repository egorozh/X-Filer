using System;
using System.Collections.Generic;
using Microsoft.Win32;
using XFiler.SDK.Localization;

namespace XFiler.SDK
{
    public static class SpecialRoutes
    {
        #region Private Fields

        private static readonly Dictionary<string, XFilerRoute> Routes;

        #endregion

        #region Public Properties

        public static XFilerRoute MyComputer { get; }

        public static XFilerRoute Settings { get; }

        public static XFilerRoute Desktop { get; }

        public static XFilerRoute Downloads { get; }

        public static XFilerRoute MyDocuments { get; }

        public static XFilerRoute MyPictures { get; }

        public static XFilerRoute MyMusic { get; }

        public static XFilerRoute MyVideos { get; }

        #endregion

        #region Static Constructor

        static SpecialRoutes()
        {
            MyComputer = new(Strings.Routes_MyComputer, "xfiler://mycomputer", RouteType.MyComputer);

            Settings = new(Strings.Routes_Settings, "xfiler://settings", RouteType.Settings);

            Desktop = new(Strings.Routes_Desktop,
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RouteType.Desktop);

            Downloads = new(Strings.Routes_Downloads,
                GetDownloadFolderPath(), RouteType.Downloads);

            MyDocuments = new(Strings.Routes_MyDocuments,
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), RouteType.MyDocuments);

            MyPictures = new(Strings.Routes_MyPictures,
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), RouteType.MyPictures);

            MyMusic = new(Strings.Routes_MyMusic,
                Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), RouteType.MyMusic);

            MyVideos = new(Strings.Routes_MyVideos,
                Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), RouteType.MyVideos);

            Routes = new Dictionary<string, XFilerRoute>
            {
                { MyComputer.FullName, MyComputer },
                { Settings.FullName, Settings },
                { Desktop.FullName, Desktop },
                { Downloads.FullName, Downloads },
                { MyDocuments.FullName, MyDocuments },
                { MyPictures.FullName, MyPictures },
                { MyMusic.FullName, MyMusic },
                { MyVideos.FullName, MyVideos }
            };
        }

        #endregion

        #region Public Methods

        public static XFilerRoute? GetSpecialUrl(string fullName) => Routes.ContainsKey(fullName)
            ? Routes[fullName]
            : null;

        public static IReadOnlyList<XFilerRoute> GetFolders() => new List<XFilerRoute>
        {
            Desktop, Downloads,
            MyDocuments, MyPictures,
            MyMusic, MyVideos
        };

        #endregion

        private static string? GetDownloadFolderPath() => Registry
            .GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders",
                "{374DE290-123F-4565-9164-39C4925E467B}", string.Empty)
            ?.ToString();
    }
}
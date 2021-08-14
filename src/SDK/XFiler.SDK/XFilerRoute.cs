using System;
using System.IO;
using System.Net;
using XFiler.SDK.Localization;

namespace XFiler.SDK
{
    public sealed record XFilerRoute
    {
        #region Public Properties

        public string Header { get; }

        public string FullName { get; }

        public RouteType Type { get; }

        #endregion

        #region Constructors

        public XFilerRoute(string header, string fullName, RouteType type)
        {
            Header = header;
            FullName = fullName;
            Type = type;
        }

        public XFilerRoute(DirectoryInfo directory)
        {
            Header = directory.Name;
            FullName = directory.FullName;
            Type = RouteType.Directory;
        }

        public XFilerRoute(FileInfo file)
        {
            Header = file.Name;
            FullName = file.FullName;
            Type = RouteType.File;
        }

        public XFilerRoute(DriveInfo driveInfo)
        {
            Header = GetName(driveInfo);
            FullName = driveInfo.RootDirectory.FullName;

            Type = FullName == "C:\\"
                ? RouteType.SystemDrive
                : RouteType.Drive;
        }

        #endregion

        #region Public Methods

        public static XFilerRoute FromPath(string path)
        {
            var special = SpecialRoutes.GetSpecialUrl(path);

            if (special != null)
                return special;

            var fileSystem = IsFileSystemRoute(path);

            if (fileSystem != null)
                return fileSystem;

            var webLink = IsWebLinkRoute(path);

            if (webLink != null)
                return webLink;

            return SpecialRoutes.MyComputer;
        }

        #endregion

        #region Private Methods

        private static XFilerRoute? IsWebLinkRoute(string url)
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;

                request.Method = "HEAD";

                var response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.Close();

                    return new XFilerRoute(url, url, RouteType.WebLink);
                }
            }
            catch (Exception e)
            {
                // ignored
            }

            return null;
        }

        private static XFilerRoute? IsFileSystemRoute(string path)
        {
            var info = path.ToInfo();

            if (info == null)
                return null;

            return info switch
            {
                FileInfo fileInfo => new XFilerRoute(fileInfo),
                DirectoryInfo { Parent: null } => new XFilerRoute(new DriveInfo(path)),
                DirectoryInfo dir => new XFilerRoute(dir),
                _ => null
            };
        }

        private static string GetName(DriveInfo driveInfo)
        {
            switch (driveInfo.DriveType)
            {
                case DriveType.Unknown:
                    break;
                case DriveType.NoRootDirectory:
                    break;
                case DriveType.Removable:
                    return $"{Strings.DriveType_Usb} ({driveInfo.Name})";
                case DriveType.Fixed:
                    return $"{Strings.DriveType_Fixed} ({driveInfo.Name})";
                case DriveType.Network:
                    break;
                case DriveType.CDRom:
                    break;
                case DriveType.Ram:
                    break;
                default:
                    return driveInfo.Name;
            }

            return driveInfo.Name;
        }

        #endregion
    }
}
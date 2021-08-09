using System;
using System.IO;
using System.Net;
using XFiler.SDK.Localization;

namespace XFiler.SDK
{
    public sealed record XFilerRoute
    {
        public string Header { get; }

        public string FullName { get; }

        public RouteType Type { get; }

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
            Type = RouteType.Directory;
        }
        
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
            try
            {
                var attr = File.GetAttributes(path);

                return attr.HasFlag(FileAttributes.Directory)
                    ? new XFilerRoute(new DirectoryInfo(path))
                    : new XFilerRoute(new FileInfo(path));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private string GetName(DriveInfo driveInfo)
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

    }
}
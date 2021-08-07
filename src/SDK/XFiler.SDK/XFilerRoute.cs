using System;
using System.IO;
using System.Net;

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

        public static XFilerRoute FromPath(string path)
        {
            var special = SpecialUrls.GetSpecialUrl(path);

            if (special != null)
                return special;

            var fileSystem = IsFileSystemRoute(path);

            if (fileSystem != null)
                return fileSystem;

            var webLink = IsWebLinkRoute(path);

            if (webLink != null)
                return webLink;


            return SpecialUrls.MyComputer;
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
    }
}
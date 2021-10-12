using System;
using System.IO;
using System.Net;
using XFiler.SDK.Localization;

namespace XFiler.SDK;

public record Route
{   
    #region Public Properties

    public string Header { get; }

    public string FullName { get; }

    public RouteType Type { get; }

    public string? Query { get; }

    public string? TargetSearchDirectory { get; }

    #endregion

    #region Constructors

    public Route(string header, string fullName, RouteType type)
    {
        Header = header;
        FullName = fullName;
        Type = type;
    }
        
    public Route(DriveInfo driveInfo)
    {
        Header = GetName(driveInfo);
        FullName = driveInfo.RootDirectory.FullName;

        Type = FullName == "C:\\"
            ? RouteType.SystemDrive
            : RouteType.Drive;
    }

    public Route(string query, string? targetSearchDirectory)
    {
        Query = query;
        TargetSearchDirectory = targetSearchDirectory;

        Header = "Поиск";
        FullName = query;

        Type = RouteType.Search;
    }

    #endregion

    #region Public Methods
    
    public static Route? FromPath(string path)
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

        return null;
    }

    #endregion

    #region Private Methods

    private static Route? IsWebLinkRoute(string url)
    {
        try
        {
            if (WebRequest.Create(url) is HttpWebRequest request)
            {
                request.Method = "HEAD";

                if (request.GetResponse() is HttpWebResponse 
                    { StatusCode: HttpStatusCode.OK } response)
                {
                    response.Close();

                    return new Route(url, url, RouteType.WebLink);
                }
            }
        }
        catch (Exception)
        {
            // ignored
        }

        return null;
    }

    private static Route? IsFileSystemRoute(string path)
    {
        var info = path.ToInfo();

        if (info == null)
            return null;

        return info switch
        {
            FileInfo fileInfo => new FileRoute(fileInfo),
            DirectoryInfo { Parent: null } => new Route(new DriveInfo(path)),
            DirectoryInfo dir => new DirectoryRoute(dir),
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
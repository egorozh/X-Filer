using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using XFiler.MyComputer;
using XFiler.Resources.Localization;
using XFiler.SDK;
using XFiler.ViewModels;

namespace XFiler
{
    internal class PageFactory : IPageFactory
    {
        private readonly Func<IReadOnlyList<IFilesPresenterFactory>> _filesPresenters;
        private readonly IClipboardService _clipboardService;
        private readonly IIconLoader _iconLoader;

        public PageFactory(
            Func<IReadOnlyList<IFilesPresenterFactory>> filesPresenters,
            IClipboardService clipboardService,
            IIconLoader iconLoader)
        {
            _filesPresenters = filesPresenters;
            _clipboardService = clipboardService;
            _iconLoader = iconLoader;
        }

        public IPageModel? CreatePage(XFilerRoute route)
        {
            switch (route.Type)
            {
                case RouteType.File:
                    OpenFile(route.FullName);
                    return null;
                case RouteType.WebLink:
                    OpenFile(route.FullName);
                    return null;
                default:
                    return route.Type switch
                    {
                        RouteType.Directory => CreateExplorerPage(route),
                        RouteType.Desktop => CreateExplorerPage(route),
                        RouteType.Downloads => CreateExplorerPage(route),
                        RouteType.MyDocuments => CreateExplorerPage(route),
                        RouteType.MyMusic => CreateExplorerPage(route),
                        RouteType.MyPictures => CreateExplorerPage(route),
                        RouteType.MyVideos => CreateExplorerPage(route),
                        RouteType.SystemDrive => CreateExplorerPage(route),
                        RouteType.Drive => CreateExplorerPage(route),
                        RouteType.RecycleBin => CreateExplorerPage(route),
                        RouteType.MyComputer => new MyComputerPageModel(_iconLoader),
                        RouteType.Settings => new SettingsPageModel(),
                        _ => new SearchPageModel(route)
                    };
            }
        }

        private ExplorerPageModel? CreateExplorerPage(XFilerRoute route)
        {
            var dir = new DirectoryInfo(route.FullName);

            try
            {
                var access = dir.EnumerateFileSystemInfos();
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show($"{Strings.PageFactory_NotAccessText} \"{dir.FullName}\"",
                    Strings.PageFactory_NotAccessCaption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return null;
            }

            return new ExplorerPageModel(_filesPresenters.Invoke(), _clipboardService, dir);
        }


        private static void OpenFile(string path) => new Process
        {
            StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            }
        }.Start();
    }
}
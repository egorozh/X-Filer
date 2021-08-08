using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace XFiler.SDK
{
    internal class PageFactory : IPageFactory
    {
        private readonly Func<IReadOnlyList<IFilesPresenterFactory>> _filesPresenters;

        public PageFactory(Func<IReadOnlyList<IFilesPresenterFactory>> filesPresenters)
        {
            _filesPresenters = filesPresenters;
        }

        public IPageModel? CreatePage(XFilerRoute route)
        {
            switch (route.Type)
            {
                case RouteType.File:
                    OpenFile(route.FullName);
                    return null;
                case RouteType.Directory:
                    return new ExplorerPageModel(_filesPresenters.Invoke(), new DirectoryInfo(route.FullName));
                case RouteType.Special:
                    if (route == SpecialRoutes.MyComputer)
                        return new MyComputerPageModel();

                    if (route == SpecialRoutes.Settings)
                        return new SettingsPageModel();
                    break;
                case RouteType.WebLink:
                    return new BrowserPageModel(route.FullName);
            }

            return new SearchPageModel(route);
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
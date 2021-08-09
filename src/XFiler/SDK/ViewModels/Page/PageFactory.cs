using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using XFiler.SDK.MyComputer;

namespace XFiler.SDK
{
    internal class PageFactory : IPageFactory
    {
        private readonly Func<IReadOnlyList<IFilesPresenterFactory>> _filesPresenters;
        private readonly IIconLoader _iconLoader;

        public PageFactory(
            Func<IReadOnlyList<IFilesPresenterFactory>> filesPresenters,
            IIconLoader iconLoader)
        {
            _filesPresenters = filesPresenters;
            _iconLoader = iconLoader;
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
                        return new MyComputerPageModel(_iconLoader);

                    if (route == SpecialRoutes.Settings)
                        return new SettingsPageModel();

                    return new ExplorerPageModel(_filesPresenters.Invoke(), new DirectoryInfo(route.FullName));

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
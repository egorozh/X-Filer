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

        public IPageModel? CreatePage(XFilerUrl url)
        {
            if (url == SpecialUrls.MyComputer)
                return new MyComputerPageModel();

            if (url == SpecialUrls.Settings)
                return new SettingsPageModel();

            try
            {
                string path = url.FullName;

                var attr = File.GetAttributes(path);

                if (attr.HasFlag(FileAttributes.Directory))
                    return new ExplorerPageModel(_filesPresenters.Invoke(), new DirectoryInfo(path));
                else
                {
                    OpenFile(path);
                    return null;
                }
            }
            catch (Exception e)
            {
            }

            return new SearchPageModel(url);
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
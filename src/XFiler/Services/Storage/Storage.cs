﻿using System;
using System.IO;
using XFiler.SDK;

namespace XFiler
{
    public class Storage : IStorage
    {
        #region Public Properties

        public string BaseDirectory { get; }

        public string LogDirectory { get; }

        public string Bookmarks { get; }

        #endregion

        #region Constructors

        private Storage(string directory)
        {
            BaseDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), directory);
            Directory.CreateDirectory(BaseDirectory);

            LogDirectory = Path.Combine(BaseDirectory, "Logs");
            Directory.CreateDirectory(LogDirectory);

            Bookmarks = Path.Combine(BaseDirectory, "bookmarks.json");
        }

        public Storage() : this("X-Filer")
        {
        }

        #endregion
    }
}
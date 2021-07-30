using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XFiler.SDK
{
    internal class ExtensionToImageFileConverter
    {
        #region Private Fields

        private readonly Dictionary<string, FileInfo> _icons;

        #endregion

        #region Constructor

        public ExtensionToImageFileConverter()
        {
            var applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var iconsDirectory = new DirectoryInfo(Path.Combine(applicationDirectory, "Resources", "Icons"));

            _icons = iconsDirectory
                .GetFiles()
                .ToDictionary(fi => Path.GetFileNameWithoutExtension(fi.Name).ToUpper());
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Получение полного пути до иконки для заданного расширения
        /// </summary>
        /// <param name="extension">Расширение в формате без точки сначала</param>
        /// <returns></returns>
        public FileInfo GetImagePath(string extension)
        {
            if (_icons.ContainsKey(extension.ToUpper()))
                return _icons[extension.ToUpper()];

            return _icons[IconName.Blank.ToUpper()];
        }

        #endregion
    }
}
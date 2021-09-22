using System.IO;

namespace XFiler
{
    public sealed class Storage : IStorage
    {
        #region Public Properties

        public string BaseDirectory { get; }

        public string LogDirectory { get; }

        public string DbDirectory { get; }

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

            DbDirectory = Path.Combine(BaseDirectory, "Data");
            Directory.CreateDirectory(DbDirectory);

            Bookmarks = Path.Combine(BaseDirectory, "bookmarks.json");
        }

        public Storage() : this("X-Filer")
        {
        }

        #endregion
    }
}
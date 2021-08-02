using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace VirtualisationTest
{
    public partial class MainWindow
    {
        public ObservableCollection<Entity> Entities { get; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            Entities = new ObservableCollection<Entity>();

            Load();
        }

        private void Load()
        {
            var path = @"C:\Windows\System32";

            var dirInfo = new DirectoryInfo(path);

            foreach (var directory in dirInfo.EnumerateDirectories())
            {
                Entities.Add(new Entity(directory.Name));
            }

            foreach (var file in dirInfo.EnumerateFiles())
            {
                Entities.Add(new Entity(file.Name));
            }

        }
    }

    public class Entity : INotifyPropertyChanged
    {
        public string Name { get; }

        public Entity(string name)
        {
            Name = name;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
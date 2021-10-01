using System.Threading.Tasks;

namespace XFiler.SDK;

public interface IStartupOptionsFileManager
{
    IStartupOptions InitOptions();
    Task Save();
}
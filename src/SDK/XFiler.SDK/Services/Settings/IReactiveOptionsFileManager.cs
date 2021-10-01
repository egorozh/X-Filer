using System.Threading.Tasks;

namespace XFiler.SDK;

public interface IReactiveOptionsFileManager
{
    IReactiveOptions InitOptions();

    Task Save();
}
using System.IO;
using System.Text.Json;

namespace XFiler;

internal class ReactiveOptionsFileManager : IReactiveOptionsFileManager
{
    private readonly ILogger _logger;
    private readonly string _configPath;
    private IReactiveOptions _options = null!;

    public ReactiveOptionsFileManager(IStorage storage, ILogger logger)
    {
        _logger = logger;
        _configPath = Path.Combine(storage.ConfigDirectory, "reactive.config");
    }

    public IReactiveOptions GetOptions() => _options = Open();

    public void Save()
    {
        try
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };

            JsonSerializer.Serialize(_options, options);
        }
        catch (Exception e)
        {
            _logger.Error(e, "ReactiveOptionsSave");
        }
    }

    private IReactiveOptions Open()
    {
        try
        {
            if (File.Exists(_configPath))
            {
                return JsonSerializer.Deserialize<IReactiveOptions>(_configPath)
                       ?? new ReactiveOptions();
            }
        }
        catch (Exception e)
        {
            _logger.Error(e, "ReactiveOptionsOpen");
        }

        return new ReactiveOptions();
    }
}
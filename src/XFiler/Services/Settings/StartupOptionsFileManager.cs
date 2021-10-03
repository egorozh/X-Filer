using System.IO;
using System.Text.Json;

namespace XFiler;

internal class StartupOptionsFileManager : IStartupOptionsFileManager
{
    private readonly ILogger _logger;
    private readonly string _configPath;
    private IStartupOptions _options = null!;

    public StartupOptionsFileManager(IStorage storage, ILogger logger)
    {
        _logger = logger;
        _configPath = Path.Combine(storage.ConfigDirectory, "startup.config");
    }

    public IStartupOptions InitOptions() => _options = Open();

    public async Task Save()
    {
        try
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };

            await using MemoryStream stream = new();
            await JsonSerializer.SerializeAsync(stream, _options, options);

            await File.WriteAllBytesAsync(_configPath, stream.GetBuffer());
        }
        catch (Exception e)
        {
            _logger.Error(e, "StartupOptionsSave");
        }
    }

    private IStartupOptions Open()
    {
        try
        {
            if (File.Exists(_configPath))
            {
                return JsonSerializer.Deserialize<StartupOptions>(File.ReadAllText(_configPath))
                       ?? new StartupOptions();
            }
        }
        catch (Exception e)
        {
            _logger.Error(e, "StartupOptionsOpen");
        }

        return new StartupOptions();
    }
}
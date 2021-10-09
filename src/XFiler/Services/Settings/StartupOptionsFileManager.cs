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
        _configPath = Path.Combine(storage.ConfigDirectory, "startup.json");
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

            await using var fs = new FileStream(_configPath, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fs, _options, options);
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
                JsonSerializerOptions options = new()
                {
                    WriteIndented = true
                };

                var json = File.ReadAllText(_configPath);

                return JsonSerializer.Deserialize<StartupOptions>(json, options)
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
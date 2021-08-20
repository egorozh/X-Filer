namespace XFiler.SDK
{
    public interface ISettingsReader
    {
        T? GetOption<T>(string optionName);
    }
}   
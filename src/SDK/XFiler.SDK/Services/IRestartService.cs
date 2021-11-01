namespace XFiler.SDK;

public interface IRestartService
{
    void RestartApplication();

    const string RestartKey = "/restart";
}
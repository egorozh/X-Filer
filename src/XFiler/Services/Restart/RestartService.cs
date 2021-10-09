using System.Diagnostics;

namespace XFiler;

internal class RestartService :IRestartService
{
    public void RestartApplication()
    {
        Application.Current.Shutdown();

        // Restart the app passing "/restart [processId]" as cmd line args
        Process.Start(Environment.ProcessPath, IRestartService.RestartKey + Process.GetCurrentProcess().Id);
    }
}
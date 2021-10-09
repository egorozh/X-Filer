using System.Diagnostics;

namespace XFiler
{
    internal class RestartService :IRestartService
    {
        public void RestartApplication()
        {
            //Process.Start(Application.ExecutablePath); 

            //this.Close();

           
            Application.Current.Shutdown();

            // Restart the app passing "/restart [processId]" as cmd line args
            Process.Start(Environment.ProcessPath, "/restart" + Process.GetCurrentProcess().Id);
        }
    }
}

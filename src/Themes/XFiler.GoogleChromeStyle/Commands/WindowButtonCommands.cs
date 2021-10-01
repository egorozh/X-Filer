using System.Windows;
using Prism.Commands;

namespace XFiler.GoogleChromeStyle;

internal static class WindowButtonCommands
{
    public static DelegateCommand<Window> CloseCommand { get; }
    public static DelegateCommand<Window> CollapseCommand { get; }
    public static DelegateCommand<Window> ExpandCommand { get; }

    static WindowButtonCommands()
    {
        CloseCommand = new DelegateCommand<Window>(OnClose);
        CollapseCommand = new DelegateCommand<Window>(OnCollapse);
        ExpandCommand = new DelegateCommand<Window>(OnExpand);
    }
        
    private static void OnClose(Window window) => window.Close();

    private static void OnCollapse(Window window) => window.WindowState = WindowState.Minimized;

    private static void OnExpand(Window window) =>
        window.WindowState = window.WindowState switch
        {
            WindowState.Normal => WindowState.Maximized,
            WindowState.Maximized => WindowState.Normal,
            _ => window.WindowState
        };
}
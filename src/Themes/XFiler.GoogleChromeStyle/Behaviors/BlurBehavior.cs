using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace XFiler.GoogleChromeStyle;

internal class BlurBehavior : AttachableForStyleBehavior<Window, BlurBehavior>
{
    protected override void OnAttached()
    {
        AssociatedObject.Loaded += (s, e) => { EnableBlur(); };
    }

    [DllImport("user32.dll")]
    internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

    private uint _blurOpacity;

    public double BlurOpacity
    {
        get => _blurOpacity;
        set
        {
            _blurOpacity = (uint) value;
            EnableBlur();
        }
    }

    private uint _blurBackgroundColor = 0x990000; /* BGR color format */

    internal void EnableBlur()
    {
        var accent = new AccentPolicy();
        var accentStructSize = Marshal.SizeOf(accent);
        accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;
        accent.GradientColor = (_blurOpacity << 24) | (_blurBackgroundColor & 0xFFFFFF);

        var accentPtr = Marshal.AllocHGlobal(accentStructSize);
        Marshal.StructureToPtr(accent, accentPtr, false);

        var data = new WindowCompositionAttributeData
        {
            Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
            SizeOfData = accentStructSize,
            Data = accentPtr
        };

        var windowHelper = new WindowInteropHelper(AssociatedObject);
        SetWindowCompositionAttribute(windowHelper.Handle, ref data);

        Marshal.FreeHGlobal(accentPtr);
    }
}

[StructLayout(LayoutKind.Sequential)]
internal struct WindowCompositionAttributeData
{
    public WindowCompositionAttribute Attribute;
    public IntPtr Data;
    public int SizeOfData;
}

internal enum WindowCompositionAttribute
{
    // ...
    WCA_ACCENT_POLICY = 19
    // ...
}

internal enum AccentState
{
    ACCENT_DISABLED = 0,
    ACCENT_ENABLE_GRADIENT = 1,
    ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
    ACCENT_ENABLE_BLURBEHIND = 3,
    ACCENT_INVALID_STATE = 4
}

[StructLayout(LayoutKind.Sequential)]
internal struct AccentPolicy
{
    public AccentState AccentState;
    public uint AccentFlags;
    public uint GradientColor;
    public uint AnimationId;
}
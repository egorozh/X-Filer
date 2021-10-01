using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Markup;

namespace XFiler.SDK;

public class ExtendedMouseBinding : MouseBinding
{
    [ValueSerializer(typeof(MouseGestureValueSerializer)), TypeConverter(typeof(ExtendedMouseGestureConverter))]
    public override InputGesture Gesture
    {
        get => base.Gesture;
        set => base.Gesture = value;
    }
}

public class ExtendedMouseGestureConverter : MouseGestureConverter
{
    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
        object source)
    {
        switch (source.ToString())
        {
            case "XButton1":
                return new ExtendedMouseGesture(MouseButton.XButton1);
            case "XButton2":
                return new ExtendedMouseGesture(MouseButton.XButton2);
        }

        return base.ConvertFrom(context, culture, source);
    }
}

public class ExtendedMouseGesture : MouseGesture
{
    private readonly MouseButton _mouseButton;

    public ExtendedMouseGesture(MouseButton mouseButton)
    {
        _mouseButton = mouseButton;
    }

    public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
    {
        var device = inputEventArgs.Device as MouseDevice;

        if (device != null)
        {
            switch (_mouseButton)
            {
                case MouseButton.XButton1:
                    if (device.XButton1 == MouseButtonState.Pressed) return true;
                    break;
                case MouseButton.XButton2:
                    if (device.XButton2 == MouseButtonState.Pressed) return true;
                    break;
            }
        }

        return false;
    }
}
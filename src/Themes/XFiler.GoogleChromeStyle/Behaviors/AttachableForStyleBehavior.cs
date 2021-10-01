using Microsoft.Xaml.Behaviors;
using System.Linq;
using System.Windows;

namespace XFiler.GoogleChromeStyle;

public class AttachableForStyleBehavior<TComponent, TBehavior> : Behavior<TComponent>
    where TComponent : DependencyObject
    where TBehavior : AttachableForStyleBehavior<TComponent, TBehavior>, new()
{
    #region Attached Property

    public static DependencyProperty IsEnabledForStyleProperty =
        DependencyProperty.RegisterAttached(nameof(IsEnabledForStyle),
            typeof(bool),
            typeof(AttachableForStyleBehavior<TComponent, TBehavior>),
            new FrameworkPropertyMetadata(false, OnIsEnabledForStyleChanged));

    public bool IsEnabledForStyle
    {
        get => (bool) GetValue(IsEnabledForStyleProperty);
        set => SetValue(IsEnabledForStyleProperty, value);
    }

    #endregion

    #region Private Methods

    private static void OnIsEnabledForStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behaviors = Interaction.GetBehaviors(d);

        var existingBehavior = behaviors.FirstOrDefault(b => b.GetType() ==
                                                             typeof(TBehavior)) as TBehavior;

        if ((bool) e.NewValue == false && existingBehavior != null)
        {
            behaviors.Remove(existingBehavior);
        }

        else if ((bool) e.NewValue == true && existingBehavior == null)
        {
            behaviors.Add(new TBehavior());
        }
    }

    #endregion
}
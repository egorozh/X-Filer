namespace Windows.FileOperations;

internal static class SRExt
{
    internal static bool UsingResourceKeys() => false;

    internal static string Format(string resourceFormat, params object[] args)
    {
        if (args == null)
            return resourceFormat;
        return UsingResourceKeys() ? resourceFormat + string.Join(", ", args) : string.Format(resourceFormat, args);
    }

    internal static string Format(string resourceFormat, object p1)
    {
        if (!UsingResourceKeys())
            return string.Format(resourceFormat, RuntimeHelpers.GetObjectValue(p1));
        return string.Join(", ", resourceFormat, p1);
    }

    internal static string Format(string resourceFormat, object p1, object p2)
    {
        if (!UsingResourceKeys())
            return string.Format(resourceFormat, RuntimeHelpers.GetObjectValue(p1), RuntimeHelpers.GetObjectValue(p2));
        return string.Join(", ", resourceFormat, p1, p2);
    }

    internal static string Format(string resourceFormat, object p1, object p2, object p3)
    {
        if (!UsingResourceKeys())
            return string.Format(resourceFormat, RuntimeHelpers.GetObjectValue(p1), RuntimeHelpers.GetObjectValue(p2), RuntimeHelpers.GetObjectValue(p3));
        return string.Join(", ", resourceFormat, p1, p2, p3);
    }
}
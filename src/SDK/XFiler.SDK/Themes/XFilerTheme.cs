using System;
using System.Windows;

namespace XFiler.SDK.Themes;

public abstract class XFilerTheme : DependencyObject, ITheme
{
    private readonly Lazy<string> _id;
    private readonly Lazy<Uri> _resource;

    public string Id => _id.Value;

    public Uri ResourceUri => _resource.Value;

    protected XFilerTheme()
    {
        _id = new Lazy<string>(GetId);
        _resource = new Lazy<Uri>(GetResourceUri);
    }

    public abstract Uri GetResourceUri();

    public abstract string GetId();
}
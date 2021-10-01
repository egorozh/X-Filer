using System;

namespace XFiler.SDK;

public abstract class Feature : IFeature
{
    private readonly Lazy<string> _id;

    public string Id => _id.Value;

    protected Feature()
    {
        _id = new Lazy<string>(GetId);
    }

    public abstract string GetId();
}
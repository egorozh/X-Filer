namespace XFiler;

public abstract class ResultsModel : BaseViewModel
{
    public string Text { get; }

    protected ResultsModel(string text)
    {
        Text = text;
    }
}
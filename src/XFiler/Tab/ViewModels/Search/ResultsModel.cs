using XFiler.SDK;

namespace XFiler
{
    public class ResultsModel : BaseViewModel
    {
        public string Text { get; }

        public ResultsModel(string text)
        {
            Text = text;
        }
    }
}
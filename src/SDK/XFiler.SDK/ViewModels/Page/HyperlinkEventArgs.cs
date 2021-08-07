namespace XFiler.SDK
{
    public class HyperlinkEventArgs
    {
        public HyperlinkEventArgs(XFilerUrl url, bool isOpenInNewTab = false)
        {
            Url = url;
            IsOpenInNewTab = isOpenInNewTab;
        }

        public bool IsOpenInNewTab { get;  }

        public XFilerUrl Url { get;  }
    }
}
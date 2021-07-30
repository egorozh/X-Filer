namespace XFiler.SDK
{
    public interface ITabsViewModel
    {
        void OnOpenNewTab(FileEntityViewModel fileEntityViewModel, bool isSelectNewTab = false);
    }   
}
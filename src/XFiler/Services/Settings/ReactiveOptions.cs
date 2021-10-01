namespace XFiler;

internal class ReactiveOptions : BaseViewModel, IReactiveOptions
{
    public bool ShowSystemFiles { get; set; } = false;

    public bool ShowHiddenFiles { get; set; } = true;
        
    public string DefaultPresenterId { get; set; } = "9a5d97b9-628d-45fd-b36f-89936f3c9506"; // default grid presenter

    public bool AlwaysOpenDirectoryInDefaultPresenter { get; set; } = false;


    public string CurrentThemeId { get; set; } = "e394f339-5907-4c5f-9113-6e49368b3d22";
}
namespace XFiler.SDK
{
    public interface IFilesPresenterFactory
    {
        IFilesPresenter CreatePresenter(PresenterType presenterType, string currentDirectory);
    }
}
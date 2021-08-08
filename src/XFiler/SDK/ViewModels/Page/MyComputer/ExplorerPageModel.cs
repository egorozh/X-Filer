namespace XFiler.SDK
{
    public class MyComputerPageModel : BasePageModel
    {
        #region Private Fields

        #endregion
        
        #region Constructor

        public MyComputerPageModel() : base(typeof(MyComputerPage))
        {//if (CurrentDirectoryPathName == IXFilerApp.RootName)
            //{
            //    list.AddRange(new (object, EntityType)[]
            //    {
            //        (Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), EntityType.SpecialFolder),
            //        (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            //            EntityType.SpecialFolder),
            //        (Environment.GetFolderPath(Environment.SpecialFolder.Desktop), EntityType.SpecialFolder),
            //        (Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), EntityType.SpecialFolder),
            //        (Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), EntityType.SpecialFolder)
            //    });

            //    list.AddRange(Directory.GetLogicalDrives().Select(p => ((object)p, EntityType.Drive)));
            //}
            //else
            //{
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
    }
}
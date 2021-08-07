namespace XFiler.SDK
{
    public static class SpecialUrls
    {
        public static XFilerUrl MyComputer = new("Мой компьютер", "xfiler://mycomputer");

        public static XFilerUrl Settings = new("Параметры", "xfiler://settings");


        public static XFilerUrl? GetSpecialUrl(string fullName)
        {
            if (MyComputer.FullName == fullName)
                return MyComputer;
            if (Settings.FullName == fullName)
                return Settings;    

            return null;
        }
    }
}
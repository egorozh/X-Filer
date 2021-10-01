namespace Windows.FileOperations;

internal static class NativeTypes
{
    public const int LCMAP_TRADITIONAL_CHINESE = 67108864;
    public const int LCMAP_SIMPLIFIED_CHINESE = 33554432;
    public const int LCMAP_UPPERCASE = 512;
    public const int LCMAP_LOWERCASE = 256;
    public const int LCMAP_FULLWIDTH = 8388608;
    public const int LCMAP_HALFWIDTH = 4194304;
    public const int LCMAP_KATAKANA = 2097152;
    public const int LCMAP_HIRAGANA = 1048576;
    public const int ERROR_FILE_NOT_FOUND = 2;
    public const int ERROR_PATH_NOT_FOUND = 3;
    public const int ERROR_ACCESS_DENIED = 5;
    public const int ERROR_ALREADY_EXISTS = 183;
    public const int ERROR_FILENAME_EXCED_RANGE = 206;
    public const int ERROR_INVALID_DRIVE = 15;
    public const int ERROR_INVALID_PARAMETER = 87;
    public const int ERROR_SHARING_VIOLATION = 32;
    public const int ERROR_FILE_EXISTS = 80;
    public const int ERROR_OPERATION_ABORTED = 995;
    public const int ERROR_CANCELLED = 1223;
        
    [StructLayout(LayoutKind.Sequential)]
    internal sealed class SystemTime
    {
        public short wYear;
        public short wMonth;
        public short wDayOfWeek;
        public short wDay;
        public short wHour;
        public short wMinute;
        public short wSecond;
        public short wMilliseconds;

        internal SystemTime()
        {
        }
    }

    [Flags]
    public enum MoveFileExFlags
    {
        MOVEFILE_REPLACE_EXISTING = 1,
        MOVEFILE_COPY_ALLOWED = 2,
        MOVEFILE_DELAY_UNTIL_REBOOT = 4,
        MOVEFILE_WRITE_THROUGH = 8,
    }
}
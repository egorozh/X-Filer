namespace Windows.FileOperations;

internal static class ExceptionUtils
{
    private const string VbDefaultErrorId = "ID95";

    private const string ResourceMsgDefault =
        "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found.";

    private static string GetFallbackMessage(string name, params object[] args) => name;

    internal static string GetResourceString2(string resourceKey)
    {
        string str;
        try
        {
            str = (SR.ResourceManager.GetString(resourceKey) ?? SR.ResourceManager.GetString(VbDefaultErrorId)) ??
                  GetFallbackMessage(resourceKey);
        }
        catch (StackOverflowException ex)
        {
            throw ex;
        }
        catch (OutOfMemoryException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            str = ResourceMsgDefault;
        }

        return str;
    }

    public static string GetResourceString(string resourceKey, params string[] args)
    {
        Debug.Assert(resourceKey != string.Empty, "ResourceKey is missing");
        Debug.Assert(args != null, "No Args");
        string unformattedString = null;
        string formattedString = null;
        try
        {
            unformattedString = GetResourceString2(resourceKey);
            formattedString = string.Format(Thread.CurrentThread.CurrentCulture, unformattedString, args);
        }
        catch (StackOverflowException ex)
        {
            throw ex;
        }
        catch (OutOfMemoryException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            Debug.Fail("Unable to get and format string for ResourceKey: " + resourceKey);
        }
        finally
        {
            Debug.Assert(!string.IsNullOrEmpty(unformattedString),
                "Unable to get string for ResourceKey: " + resourceKey);
            Debug.Assert(!string.IsNullOrEmpty(formattedString),
                "Unable to format string for ResourceKey: " + resourceKey);
        }

        return !string.IsNullOrEmpty(formattedString) ? formattedString : unformattedString;
    }

    internal static Exception VbMakeException(int hr)
    {
        string sMsg = hr <= 0 || hr > ushort.MaxValue ? "" : GetResourceString(hr.ToString());
        return VbMakeExceptionEx(hr, sMsg);
    }

    internal static Exception VbMakeExceptionEx(int number, string sMsg)
    {
        bool vbDefinedError = false;
        Exception exception = BuildException(number, sMsg, ref vbDefinedError);
            
        return exception;
    }

    internal static Exception BuildException(
        int number,
        string description,
        ref bool vbDefinedError)
    {
        vbDefinedError = true;
        switch (number)
        {
            case -2147467261:
                return new AccessViolationException();
            case 0:
                vbDefinedError = false;
                return new Exception(description);
            case 3:
            case 20:
            case 94:
            case 100:
                return new InvalidOperationException(description);
            case 5:
            case 446:
            case 448:
            case 449:
                return new ArgumentException(description);
            case 6:
                return new OverflowException(description);
            case 7:
            case 14:
                return new OutOfMemoryException(description);
            case 9:
                return new IndexOutOfRangeException(description);
            case 11:
                return new DivideByZeroException(description);
            case 13:
                return new InvalidCastException(description);
            case 28:
                return new StackOverflowException(description);
            case 48:
                return new TypeLoadException(description);
            case 52:
            case 54:
            case 55:
            case 57:
            case 58:
            case 59:
            case 61:
            case 63:
            case 67:
            case 68:
            case 70:
            case 71:
            case 74:
            case 75:
                return new IOException(description);
            case 53:
                return new FileNotFoundException(description);
            case 62:
                return new EndOfStreamException(description);
            case 76:
            case 432:
                return new FileNotFoundException(description);
            case 91:
                return new NullReferenceException(description);
            case 422:
                return new MissingFieldException(description);
            case 429:
            case 462:
                return new Exception(description);
            case 438:
                return new MissingMemberException(description);
            default:
                vbDefinedError = false;
                return new Exception(description);
        }
    }

    internal static ArgumentException GetArgumentExceptionWithArgName(
        string argumentName,
        string resourceId,
        params string[] placeHolders)
    {
        return new ArgumentException(SRExt.Format(resourceId, placeHolders), argumentName);
    }

    internal static ArgumentNullException GetArgumentNullException(
        string argumentName)
    {
        return new ArgumentNullException(argumentName, SR.General_ArgumentNullException);
    }

    internal static ArgumentNullException GetArgumentNullException(
        string argumentName,
        string resourceId,
        params string[] placeHolders)
    {
        return new ArgumentNullException(argumentName, SRExt.Format(resourceId, placeHolders));
    }

    internal static DirectoryNotFoundException GetDirectoryNotFoundException(
        string resourceId,
        params string[] placeHolders)
    {
        return new DirectoryNotFoundException(SRExt.Format(resourceId, placeHolders));
    }

    internal static FileNotFoundException GetFileNotFoundException(
        string fileName,
        string resourceId,
        params string[] placeHolders)
    {
        return new FileNotFoundException(SRExt.Format(resourceId, placeHolders), fileName);
    }

    internal static InvalidOperationException GetInvalidOperationException(
        string resourceId,
        params string[] placeHolders)
    {
        return new InvalidOperationException(SRExt.Format(resourceId, placeHolders));
    }

    internal static IOException GetIoException(
        string resourceId,
        params string[] placeHolders)
    {
        return new IOException(SRExt.Format(resourceId, placeHolders));
    }
}
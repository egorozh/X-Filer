' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.

Option Strict Off

Imports Resources

Friend Enum vbErrors
    None = 0
    ReturnWOGoSub = 3
    IllegalFuncCall = 5
    Overflow = 6
    OutOfMemory = 7
    OutOfBounds = 9
    ArrayLocked = 10
    DivByZero = 11
    TypeMismatch = 13
    OutOfStrSpace = 14
    ExprTooComplex = 16
    CantContinue = 17
    UserInterrupt = 18
    ResumeWOErr = 20
    OutOfStack = 28
    UNDONE = 29
    UndefinedProc = 35
    TooManyClients = 47
    DLLLoadErr = 48
    DLLBadCallingConv = 49
    InternalError = 51
    BadFileNameOrNumber = 52
    FileNotFound = 53
    BadFileMode = 54
    FileAlreadyOpen = 55
    IOError = 57
    FileAlreadyExists = 58
    BadRecordLen = 59
    DiskFull = 61
    EndOfFile = 62
    BadRecordNum = 63
    TooManyFiles = 67
    DevUnavailable = 68
    PermissionDenied = 70
    DiskNotReady = 71
    DifferentDrive = 74
    PathFileAccess = 75
    PathNotFound = 76
    ObjNotSet = 91
    IllegalFor = 92
    BadPatStr = 93
    CantUseNull = 94
    UserDefined = 95
    AdviseLimit = 96
    BadCallToFriendFunction = 97
    CantPassPrivateObject = 98
    DLLCallException = 99
    DoesntImplementICollection = 100
    Abort = 287
    InvalidFileFormat = 321
    CantCreateTmpFile = 322
    InvalidResourceFormat = 325
    InvalidPropertyValue = 380
    InvalidPropertyArrayIndex = 381
    SetNotSupportedAtRuntime = 382
    SetNotSupported = 383
    NeedPropertyArrayIndex = 385
    SetNotPermitted = 387
    GetNotSupportedAtRuntime = 393
    GetNotSupported = 394
    PropertyNotFound = 422
    NoSuchControlOrProperty = 423
    NotObject = 424
    CantCreateObject = 429
    OLENotSupported = 430
    OLEFileNotFound = 432
    OLENoPropOrMethod = 438
    OLEAutomationError = 440
    LostTLB = 442
    OLENoDefault = 443
    ActionNotSupported = 445
    NamedArgsNotSupported = 446
    LocaleSettingNotSupported = 447
    NamedParamNotFound = 448
    ParameterNotOptional = 449
    FuncArityMismatch = 450
    NotEnum = 451
    InvalidOrdinal = 452
    InvalidDllFunctionName = 453
    CodeResourceNotFound = 454
    CodeResourceLockError = 455
    DuplicateKey = 457
    InvalidTypeLibVariable = 458
    ObjDoesNotSupportEvents = 459
    InvalidClipboardFormat = 460
    IdentNotMember = 461
    ServerNotFound = 462
    ObjNotRegistered = 463
    InvalidPicture = 481
    PrinterError = 482
    CantSaveFileToTemp = 735
    SearchTextNotFound = 744
    ReplacementsTooLong = 746

    NotYetImplemented = 32768
    FileNotFoundWithName = 40243
    CantFindDllEntryPoint = 59201

    SeekErr = 32771
    ReadFault = 32772
    WriteFault = 32773
    BadFunctionId = 32774
    FileLockViolation = 32775
    ShareRequired = 32789
    BufferTooSmall = 32790
    InvDataRead = 32792
    UnsupFormat = 32793
    RegistryAccess = 32796
    LibNotRegistered = 32797
    Usage = 32799
    UndefinedType = 32807
    QualifiedNameDisallowed = 32808
    InvalidState = 32809
    WrongTypeKind = 32810
    ElementNotFound = 32811
    AmbiguousName = 32812
    ModNameConflict = 32813
    UnknownLcid = 32814
    BadModuleKind = 35005
    NoContainingLib = 35009
    BadTypeId = 35010
    BadLibId = 35011
    Eof = 35012
    SizeTooBig = 35013
    ExpectedFuncNotModule = 35015
    ExpectedFuncNotRecord = 35016
    ExpectedFuncNotProject = 35017
    ExpectedFuncNotVar = 35018
    ExpectedTypeNotProj = 35019
    UnsuitableFuncPropMatch = 35020
    BrokenLibRef = 35021
    UnsupportedTypeLibFeature = 35022
    ModuleAsType = 35024
    InvalidTypeInfoKind = 35025
    InvalidTypeLibFunction = 35026
    OperationNotAllowedInDll = 40035
    CompileError = 40036
    CantEvalWatch = 40037
    MissingVbaTypeLib = 40038
    UserReset = 40040
    MissingEndBrack = 40041
    IncorrectTypeChar = 40042
    InvalidNumLit = 40043
    IllegalChar = 40044
    IdTooLong = 40045
    StatementTooComplex = 40046
    ExpectedTokens = 40047
    InconsistentPropFuncs = 40067
    CircularType = 40068
    AccessViolation = &H80004003 'This is E_POINTER.  This is what VB6 returns from err.Number when calling into a .NET assembly that throws an AccessViolation
    LastTrappable = ReplacementsTooLong
End Enum

' Implements error utilities for Basic
Friend NotInheritable Class ExceptionUtils

    ' Prevent creation.
    Private Sub New()
    End Sub
    Private Const VBDefaultErrorID As String = "ID95"
    Private Const ResourceMsgDefault As String = "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found."
    Private Shared Function GetFallbackMessage(ByVal name As String, ByVal ParamArray args() As Object) As String
        'last-ditch effort; just give back name
        Return name
    End Function
    Friend Shared Function GetResourceString2(ByVal ResourceKey As String) As String

        Dim s As String = Nothing

        Try
            s = SR.ResourceManager.GetString(ResourceKey)
            ' this may be unknown error, so try getting default message
            If s Is Nothing Then
                s = SR.ResourceManager.GetString(VBDefaultErrorID)
            End If

            'if we have found nothing, get a fallback message.
            If s Is Nothing Then
                s = GetFallbackMessage(ResourceKey)
            End If
        Catch ex As StackOverflowException
            Throw ex
        Catch ex As OutOfMemoryException
            Throw ex
        Catch
            s = ResourceMsgDefault
        End Try

        Return s
    End Function

    ''' <summary>
    ''' Retrieves a resource string and formats it by replacing placeholders with parameters.
    ''' </summary>
    ''' <param name="ResourceKey">The resource string identifier</param>
    ''' <param name="Args">An array of parameters used to replace placeholders</param>
    ''' <returns>The resource string if found or an error message string</returns>
    Public Shared Function GetResourceString(ByVal ResourceKey As String, ByVal ParamArray Args() As String) As String

        System.Diagnostics.Debug.Assert(Not ResourceKey = "", "ResourceKey is missing")
        System.Diagnostics.Debug.Assert(Not Args Is Nothing, "No Args")

        Dim UnformattedString As String = Nothing
        Dim FormattedString As String = Nothing
        Try
            'Get unformatted string which may have place holders ie "Hello, {0}. How is {1}?"
            UnformattedString = GetResourceString2(ResourceKey)

            'Replace placeholders with items from the passed in array
            FormattedString = String.Format(System.Threading.Thread.CurrentThread.CurrentCulture, UnformattedString, Args)

            'Rethrow hosting exceptions
        Catch ex As StackOverflowException
            Throw ex
        Catch ex As OutOfMemoryException
            Throw ex

        Catch ex As Exception
            System.Diagnostics.Debug.Fail("Unable to get and format string for ResourceKey: " & ResourceKey)
        Finally
            System.Diagnostics.Debug.Assert(Not UnformattedString = "", "Unable to get string for ResourceKey: " & ResourceKey)
            System.Diagnostics.Debug.Assert(Not FormattedString = "", "Unable to format string for ResourceKey: " & ResourceKey)
        End Try

        'Return the string if we have one otherwise return a default error message
        If Not FormattedString = "" Then
            Return FormattedString
        Else
            Return UnformattedString 'will contain an error string from the attempt to load via the GetResourceString() overload we call internally
        End If
    End Function

    Friend Shared Function VbMakeException(ByVal hr As Integer) As System.Exception
        Dim sMsg As String

        If hr > 0 AndAlso hr <= &HFFFFI Then
            sMsg = GetResourceString(CType(hr, vbErrors))
        Else
            sMsg = ""
        End If
        VbMakeException = VbMakeExceptionEx(hr, sMsg)
    End Function

    Friend Shared Function VbMakeExceptionEx(ByVal number As Integer, ByVal sMsg As String) As System.Exception
        Dim vBDefinedError As Boolean

        VbMakeExceptionEx = BuildException(number, sMsg, vBDefinedError)

        If vBDefinedError Then
        End If

    End Function

    Friend Shared Function BuildException(ByVal Number As Integer, ByVal Description As String, ByRef VBDefinedError As Boolean) As System.Exception

        VBDefinedError = True

        Select Case Number

            Case vbErrors.None

            Case vbErrors.ReturnWOGoSub,
                vbErrors.ResumeWOErr,
                vbErrors.CantUseNull,
                vbErrors.DoesntImplementICollection
                Return New InvalidOperationException(Description)

            Case vbErrors.IllegalFuncCall,
                vbErrors.NamedParamNotFound,
                vbErrors.NamedArgsNotSupported,
                vbErrors.ParameterNotOptional
                Return New ArgumentException(Description)

            Case vbErrors.OLENoPropOrMethod
                Return New MissingMemberException(Description)

            Case vbErrors.Overflow
                Return New OverflowException(Description)

            Case vbErrors.OutOfMemory, vbErrors.OutOfStrSpace
                Return New OutOfMemoryException(Description)

            Case vbErrors.OutOfBounds
                Return New IndexOutOfRangeException(Description)

            Case vbErrors.DivByZero
                Return New DivideByZeroException(Description)

            Case vbErrors.TypeMismatch
                Return New InvalidCastException(Description)

            Case vbErrors.OutOfStack
                Return New StackOverflowException(Description)

            Case vbErrors.DLLLoadErr
                Return New TypeLoadException(Description)

            Case vbErrors.FileNotFound
                Return New IO.FileNotFoundException(Description)

            Case vbErrors.EndOfFile
                Return New IO.EndOfStreamException(Description)

            Case vbErrors.IOError,
                vbErrors.BadFileNameOrNumber,
                vbErrors.BadFileMode,
                vbErrors.FileAlreadyOpen,
                vbErrors.FileAlreadyExists,
                vbErrors.BadRecordLen,
                vbErrors.DiskFull,
                vbErrors.BadRecordNum,
                vbErrors.TooManyFiles,
                vbErrors.DevUnavailable,
                vbErrors.PermissionDenied,
                vbErrors.DiskNotReady,
                vbErrors.DifferentDrive,
                vbErrors.PathFileAccess
                Return New IO.IOException(Description)

            Case vbErrors.PathNotFound,
                vbErrors.OLEFileNotFound
                Return New IO.FileNotFoundException(Description)

            Case vbErrors.ObjNotSet
                Return New NullReferenceException(Description)

            Case vbErrors.PropertyNotFound
                Return New MissingFieldException(Description)

            Case vbErrors.CantCreateObject,
                vbErrors.ServerNotFound
                Return New Exception(Description)

            Case vbErrors.AccessViolation
                Return New AccessViolationException() 'We never want a custom description here.  Use the localized message that comes for free inside the exception

            Case Else
                'Fall below to default
                VBDefinedError = False
                Return New Exception(Description)
        End Select

        VBDefinedError = False
        Return New Exception(Description)

    End Function

    ''' <summary>
    ''' Return a new instance of ArgumentException with the message from resource file and the Exception.ArgumentName property set.
    ''' </summary>
    ''' <param name="ArgumentName">The name of the argument (parameter). Not localized.</param>
    ''' <param name="ResourceID">The resource ID. Use CompilerServices.ResID.xxx</param>
    ''' <param name="PlaceHolders">Strings that will replace place holders in the resource string, if any.</param>
    ''' <returns>A new instance of ArgumentException.</returns>
    ''' <remarks>This is the preferred way to construct an argument exception.</remarks>
    Friend Shared Function GetArgumentExceptionWithArgName(ByVal ArgumentName As String,
                                                           ByVal ResourceID As String, ByVal ParamArray PlaceHolders() As String) As ArgumentException

        Return New ArgumentException(SRExt.Format(ResourceID, PlaceHolders), ArgumentName)
    End Function

    ''' <summary>
    ''' Return a new instance of ArgumentNullException with message: "Argument cannot be Nothing."
    ''' </summary>
    ''' <param name="ArgumentName">The name of the argument (parameter). Not localized.</param>
    ''' <returns>A new instance of ArgumentNullException.</returns>
    Friend Shared Function GetArgumentNullException(ByVal ArgumentName As String) As ArgumentNullException

        Return New ArgumentNullException(ArgumentName, SR.General_ArgumentNullException)
    End Function

    ''' <summary>
    ''' Return a new instance of ArgumentNullException with the message from resource file.
    ''' </summary>
    ''' <param name="ArgumentName">The name of the argument (parameter). Not localized.</param>
    ''' <param name="ResourceID">The resource ID. Use CompilerServices.ResID.xxx</param>
    ''' <param name="PlaceHolders">Strings that will replace place holders in the resource string, if any.</param>
    ''' <returns>A new instance of ArgumentNullException.</returns>
    Friend Shared Function GetArgumentNullException(ByVal ArgumentName As String,
                                                    ByVal ResourceID As String, ByVal ParamArray PlaceHolders() As String) As ArgumentNullException

        Return New ArgumentNullException(ArgumentName, SRExt.Format(ResourceID, PlaceHolders))
    End Function

    ''' <summary>
    ''' Return a new instance of IO.DirectoryNotFoundException with the message from resource file.
    ''' </summary>
    ''' <param name="ResourceID">The resource ID. Use CompilerServices.ResID.xxx</param>
    ''' <param name="PlaceHolders">Strings that will replace place holders in the resource string, if any.</param>
    ''' <returns>A new instance of IO.DirectoryNotFoundException.</returns>
    Friend Shared Function GetDirectoryNotFoundException(
                                                         ByVal ResourceID As String, ByVal ParamArray PlaceHolders() As String) As IO.DirectoryNotFoundException

        Return New IO.DirectoryNotFoundException(SRExt.Format(ResourceID, PlaceHolders))
    End Function

    ''' <summary>
    ''' Return a new instance of IO.FileNotFoundException with the message from resource file.
    ''' </summary>
    ''' <param name="FileName">The file name (path) of the not found file.</param>
    ''' <param name="ResourceID">The resource ID. Use CompilerServices.ResID.xxx</param>
    ''' <param name="PlaceHolders">Strings that will replace place holders in the resource string, if any.</param>
    ''' <returns>A new instance of IO.FileNotFoundException.</returns>
    Friend Shared Function GetFileNotFoundException(ByVal FileName As String,
                                                    ByVal ResourceID As String, ByVal ParamArray PlaceHolders() As String) As IO.FileNotFoundException

        Return New IO.FileNotFoundException(SRExt.Format(ResourceID, PlaceHolders), FileName)
    End Function

    ''' <summary>
    ''' Return a new instance of InvalidOperationException with the message from resource file.
    ''' </summary>
    ''' <param name="ResourceID">The resource ID. Use CompilerServices.ResID.xxx</param>
    ''' <param name="PlaceHolders">Strings that will replace place holders in the resource string, if any.</param>
    ''' <returns>A new instance of InvalidOperationException.</returns>
    Friend Shared Function GetInvalidOperationException(
                                                        ByVal ResourceID As String, ByVal ParamArray PlaceHolders() As String) As InvalidOperationException

        Return New InvalidOperationException(SRExt.Format(ResourceID, PlaceHolders))
    End Function

    ''' <summary>
    ''' Return a new instance of IO.IOException with the message from resource file.
    ''' </summary>
    ''' <param name="ResourceID">The resource ID. Use CompilerServices.ResID.xxx</param>
    ''' <param name="PlaceHolders">Strings that will replace place holders in the resource string, if any.</param>
    ''' <returns>A new instance of IO.IOException.</returns>
    Friend Shared Function GetIOException(ByVal ResourceID As String, ByVal ParamArray PlaceHolders() As String) As IO.IOException

        Return New IO.IOException(SRExt.Format(ResourceID, PlaceHolders))
    End Function
End Class
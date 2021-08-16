Imports System.Runtime.CompilerServices

Namespace System

    Friend Class SRExt
        Friend Shared Function UsingResourceKeys() As Boolean
            Return False
        End Function

        Friend Shared Function Format(ByVal resourceFormat As String, ParamArray args() As Object) As String
            If args IsNot Nothing Then
                If (UsingResourceKeys()) Then
                    Return resourceFormat + String.Join(", ", args)
                End If
                Return String.Format(resourceFormat, args)
            End If
            Return resourceFormat
        End Function


        Friend Shared Function Format(ByVal resourceFormat As String, p1 As Object) As String
            If (UsingResourceKeys()) Then
                Return String.Join(", ", resourceFormat, p1)
            End If

            Return String.Format(resourceFormat, p1)
        End Function


        Friend Shared Function Format(ByVal resourceFormat As String, p1 As Object, p2 As Object) As String
            If (UsingResourceKeys()) Then
                Return String.Join(", ", resourceFormat, p1, p2)
            End If

            Return String.Format(resourceFormat, p1, p2)
        End Function

        Friend Shared Function Format(ByVal resourceFormat As String, p1 As Object, p2 As Object, p3 As Object) As String
            If (UsingResourceKeys()) Then
                Return String.Join(", ", resourceFormat, p1, p2, p3)
            End If
            Return String.Format(resourceFormat, p1, p2, p3)
        End Function
    End Class
End Namespace
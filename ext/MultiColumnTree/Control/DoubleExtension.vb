Imports System.Runtime.CompilerServices

''' <seealso cref="https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net"/>
Public Module DoubleExtension
    Public Function BytesToString(ByVal d As Double) As String
        Dim suf() As String = {"B", "KB", "MB", "GB", "TB", "PB", "EB"}
        If d = 0 Then
            Return "0" + suf(0)
        End If

        Dim place As Integer = Convert.ToInt32(Math.Floor(Math.Log(d, 1024)))
        Dim num As Double = Math.Round(d / Math.Pow(1024, place), 1)
        Return (Math.Sign(d) * num).ToString() + suf(place)
    End Function
End Module

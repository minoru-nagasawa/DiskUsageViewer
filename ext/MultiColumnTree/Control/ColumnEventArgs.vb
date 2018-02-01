Public Class ColumnEventArgs
    Inherits EventArgs
    Dim _column As ColumnHeader
    Public Sub New(ByVal column As ColumnHeader)
        MyBase.New()
        _column = column
    End Sub
    Public ReadOnly Property Column() As ColumnHeader
        Get
            Return _column
        End Get
    End Property
End Class
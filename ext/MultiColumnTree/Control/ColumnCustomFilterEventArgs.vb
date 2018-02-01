Public Class ColumnCustomFilterEventArgs
    Inherits EventArgs
    Dim _column As ColumnHeader
    Dim _cancelFilter As Boolean = False
    Public Sub New(ByVal column As ColumnHeader)
        MyBase.New()
        _column = column
    End Sub
    Public Property CancelFilter() As Boolean
        Get
            Return _cancelFilter
        End Get
        Set(ByVal value As Boolean)
            _cancelFilter = value
        End Set
    End Property
    Public ReadOnly Property Column() As ColumnHeader
        Get
            Return _column
        End Get
    End Property
End Class
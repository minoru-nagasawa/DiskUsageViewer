Imports System.Drawing
Public Class ColumnBackgroundPaintEventArgs
    Dim _column As ColumnHeader
    Dim _columnIndex As Integer
    Dim _graphics As Graphics
    Dim _rectangle As Rectangle
    Public Sub New(ByVal column As ColumnHeader, ByVal index As Integer, ByVal graphics As Graphics, _
        ByVal rectangle As Rectangle)
        _column = column
        _columnIndex = index
        _graphics = graphics
        _rectangle = rectangle
    End Sub
    Public ReadOnly Property Column() As ColumnHeader
        Get
            Return _column
        End Get
    End Property
    Public ReadOnly Property ColumnIndex() As Integer
        Get
            Return _columnIndex
        End Get
    End Property
    Public ReadOnly Property Graphics() As Graphics
        Get
            Return _graphics
        End Get
    End Property
    Public ReadOnly Property Rectangle() As Rectangle
        Get
            Return _rectangle
        End Get
    End Property
End Class
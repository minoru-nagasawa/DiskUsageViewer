Public Class TreeNodeMouseEventArgs
    Inherits EventArgs
    Dim _node As TreeNode
    Dim _action As TreeNodeAction
    Dim _e As Windows.Forms.MouseEventArgs
    Public Sub New(ByVal node As TreeNode, ByVal action As TreeNodeAction, ByVal e As Windows.Forms.MouseEventArgs)
        MyBase.New()
        _node = node
        _action = action
        _e = e
    End Sub
    Public ReadOnly Property Node() As TreeNode
        Get
            Return _node
        End Get
    End Property
    Public ReadOnly Property Action() As TreeNodeAction
        Get
            Return _action
        End Get
    End Property
    Public ReadOnly Property MouseEvent() As Windows.Forms.MouseEventArgs
        Get
            Return _e
        End Get
    End Property
End Class
Public Class TreeNodeEventArgs
    Inherits EventArgs
    Dim _node As TreeNode
    Dim _action As TreeNodeAction = TreeNodeAction.Unknown
    Dim _cancel As Boolean = False
    Public Sub New(ByVal node As TreeNode, ByVal action As TreeNodeAction)
        MyBase.New()
        _node = node
        _action = action
    End Sub
    Public Property Cancel() As Boolean
        Get
            Return _cancel
        End Get
        Set(ByVal value As Boolean)
            _cancel = value
        End Set
    End Property
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
End Class
Public Enum TreeNodeAction
    Collapse
    Expand
    MouseHover
    MouseLeave
    LabelEdit
    MouseDown
    MouseUp
    Checked
    Unknown
End Enum
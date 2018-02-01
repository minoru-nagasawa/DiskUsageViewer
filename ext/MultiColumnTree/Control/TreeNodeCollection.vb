Imports System.ComponentModel
Imports System.Drawing
''' <summary>
''' Represent a collection of TreeNode objects in a ListView.
''' </summary>
Public NotInheritable Class TreeNodeCollection
    Inherits CollectionBase
    Friend _parent As TreeNode
    Friend _owner As MultiColumnTree
    Public Sub New(ByVal parent As TreeNode, ByVal owner As MultiColumnTree)
        MyBase.New()
        _owner = owner
        _parent = parent
    End Sub
#Region "Friend Events"
    Friend Event Clearing(ByVal sender As Object, ByVal e As CollectionEventArgs)
    Friend Event AfterClear(ByVal sender As Object, ByVal e As CollectionEventArgs)
    Friend Event Inserting(ByVal sender As Object, ByVal e As CollectionEventArgs)
    Friend Event AfterInsert(ByVal sender As Object, ByVal e As CollectionEventArgs)
    Friend Event Removing(ByVal sender As Object, ByVal e As CollectionEventArgs)
    Friend Event AfterRemove(ByVal sender As Object, ByVal e As CollectionEventArgs)
    Friend Event Setting(ByVal sender As Object, ByVal e As CollectionEventArgs)
    Friend Event AfterSet(ByVal sender As Object, ByVal e As CollectionEventArgs)
#End Region
#Region "Public Properties"
    Default Public ReadOnly Property Item(ByVal index As Integer) As TreeNode
        Get
            If index < 0 Or index >= List.Count Then
                Return Nothing
            Else
                Return DirectCast(List.Item(index), TreeNode)
            End If
        End Get
    End Property
    Public ReadOnly Property IndexOf(ByVal item As TreeNode) As Integer
        Get
            Return Me.List.IndexOf(item)
        End Get
    End Property
#End Region
#Region "Public Methods"
    Public Overloads Function Add(ByVal item As TreeNode) As TreeNode
        item._owner = _owner
        item._parent = _parent
        item.setParentNOwner()
        Dim index As Integer = List.Add(item)
        Return DirectCast(List.Item(index), TreeNode)
    End Function
    Public Overloads Function Add(ByVal text As String) As TreeNode
        Dim anItem As TreeNode
        anItem = New TreeNode(text)
        Return Me.Add(anItem)
    End Function
    Public Overloads Function Add(ByVal text As String, ByVal nodes As TreeNodeCollection) As TreeNode
        Dim anItem As TreeNode
        anItem = New TreeNode(text, nodes)
        Return Me.Add(anItem)
    End Function
    Public Overloads Function Add(ByVal text As String, ByVal img As Image, ByVal expImg As Image) As TreeNode
        Dim anItem As TreeNode
        anItem = New TreeNode(text, img, expImg)
        Return Me.Add(anItem)
    End Function
    Public Overloads Function Add(ByVal text As String, ByVal img As Image, ByVal expImg As Image, ByVal nodes As TreeNodeCollection) As TreeNode
        Dim anItem As TreeNode
        anItem = New TreeNode(text, img, expImg, nodes)
        Return Me.Add(anItem)
    End Function
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
    Public Overloads Sub AddRange(ByVal nodes As TreeNodeCollection)
        For Each tn As TreeNode In nodes
            Me.Add(tn)
        Next
    End Sub
    Public Sub Insert(ByVal index As Integer, ByVal node As TreeNode)
        node._owner = _owner
        node._parent = _parent
        node.setParentNOwner()
        List.Insert(index, node)
    End Sub
    Public Sub Remove(ByVal node As TreeNode)
        List.Remove(node)
    End Sub
    Public Function Contains(ByVal node As TreeNode) As Boolean
        Return List.Contains(node)
    End Function
    Public Function ContainsKey(ByVal key As String) As Boolean
        For Each tn As TreeNode In List
            If tn.Name = key Then Return True
        Next
        Return False
    End Function
    Public Function Find(ByVal key As String, Optional ByVal searchAllChildren As Boolean = False) As List(Of TreeNode)
        Dim result As List(Of TreeNode) = New List(Of TreeNode)
        For Each tn As TreeNode In List
            If tn.Name = key Then result.Add(tn)
            If searchAllChildren Then
                If tn.Nodes.Count > 0 Then
                    result.AddRange(tn.Nodes.Find(key, searchAllChildren))
                End If
            End If
        Next
        Return result
    End Function
#End Region
#Region "Protected Overriden Methods"
    Protected Overrides Sub OnValidate(ByVal value As Object)
        If Not GetType(TreeNode).IsAssignableFrom(value.GetType) Then
            Throw New ArgumentException("Value must AiControl.TreeNode", "value")
        End If
    End Sub
    Protected Overrides Sub OnClear()
        RaiseEvent Clearing(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnClear))
    End Sub
    Protected Overrides Sub OnClearComplete()
        RaiseEvent AfterClear(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnClearComplete))
    End Sub
    Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As Object)
        If _owner.IsDesignMode Then
            Dim aNode As TreeNode = DirectCast(value, TreeNode)
            aNode.NodeFont = _owner.Font
        End If
        RaiseEvent Inserting(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnInsert, index, value))
    End Sub
    Protected Overrides Sub OnInsertComplete(ByVal index As Integer, ByVal value As Object)
        RaiseEvent AfterInsert(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnInsertComplete, index, value))
    End Sub
    Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As Object)
        RaiseEvent Removing(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnRemove, index, value))
    End Sub
    Protected Overrides Sub OnRemoveComplete(ByVal index As Integer, ByVal value As Object)
        RaiseEvent AfterRemove(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnRemoveComplete, index, value))
    End Sub
    Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As Object, ByVal newValue As Object)
        RaiseEvent Setting(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnSet, index, oldValue, newValue))
    End Sub
    Protected Overrides Sub OnSetComplete(ByVal index As Integer, ByVal oldValue As Object, ByVal newValue As Object)
        RaiseEvent AfterSet(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnSet, index, oldValue, newValue))
    End Sub
#End Region
End Class
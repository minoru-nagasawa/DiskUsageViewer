Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing
''' <summary>
''' Class to represent an Node in the MultiColumnTree.
''' </summary>
<DefaultProperty("Text")> _
Public Class TreeNode
#Region "Friend Events."
    ' Child related events.
    ''' <summary>
    ''' Occurs when a child node is added to a TreeNode object.
    ''' </summary>
    Friend Event NodeAdded(ByVal sender As Object, ByVal e As CollectionEventArgs)
    ''' <summary>
    ''' Occurs when a child node is removed from a TreeNode object.
    ''' </summary>
    Friend Event NodeRemoved(ByVal sender As Object, ByVal e As CollectionEventArgs)
    ''' <summary>
    ''' Occurs when clearing the children of a TreeNode.
    ''' </summary>
    Friend Event NodesOnClear(ByVal sender As Object, ByVal e As CollectionEventArgs)
#End Region
#Region "Public Classes."
    ''' <summary>
    ''' Class to represent a SubItem of an Item in the MultiColumnTree.
    ''' </summary>
    <DefaultProperty("Value")> _
    Public Class TreeNodeSubItem
#Region "Declaration"
        Friend _owner As TreeNode = Nothing
        Friend _font As Font = Nothing
        Dim _value As Object = Nothing
        Dim _tag As Object = Nothing
        Dim _backColor As Drawing.Color = Drawing.Color.Transparent
        Dim _color As Drawing.Color = Drawing.Color.Black
        Dim _name As String = "TreeNodeSubItem"
        Dim _printValueOnBar As Boolean = False
#End Region
#Region "Constructor"
        <Description("Create an instance of TreeNodeSubItem.")> _
        Public Sub New()
            _owner = New TreeNode
            _font = _owner.NodeFont
        End Sub
        <Description("Create an instance of the TreeNodeSubItem with specified TreeNode as its owner.")> _
        Public Sub New(ByVal owner As TreeNode)
            _owner = owner
            _font = _owner.NodeFont
        End Sub
#End Region
#Region "Public Properties"
        ' Designs
        <DefaultValue("TreeNodeSubItem"), Category("Design"), _
            Description("Determine the name of the TreeNodeSubItem Object.")> _
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property
        <DefaultValue(""), Category("Design"), TypeConverter(GetType(StringConverter)), _
            Description("Determine an Object data associated with the TreeNodeSubItem.")> _
        Public Property Tag() As Object
            Get
                Return _tag
            End Get
            Set(ByVal value As Object)
                _tag = value
            End Set
        End Property
        ' Appearances
        <DefaultValue(""), Category("Appearance"), TypeConverter(GetType(StringConverter)), _
            Description("Determine the value of the TreeNodeSubItem.")> _
        Public Property Value() As Object
            Get
                Return _value
            End Get
            Set(ByVal value As Object)
                If _value IsNot value Then
                    _value = value
                    _owner._owner._subitemValueChanged(Me)
                End If
            End Set
        End Property
        <Category("Appearance"), _
            Description("Determine a Font object to draw the value of the TreeNodeSubItem.")> _
        Public Property Font() As Drawing.Font
            Get
                If _font Is Nothing Then
                    Return _owner.NodeFont
                Else
                    Return _font
                End If
            End Get
            Set(ByVal value As Drawing.Font)
                If _font IsNot value Then
                    _font = value
                    _owner._owner._subitemFontChanged(Me)
                End If
            End Set
        End Property
        <DefaultValue(GetType(Drawing.Color), "Transparent"), Category("Appearance"), _
            Description("Determine a color used for TreeNodeSubItem background.")> _
        Public Property BackColor() As Drawing.Color
            Get
                Return _backColor
            End Get
            Set(ByVal value As Drawing.Color)
                If _backColor <> value Then
                    _backColor = value
                    _owner._owner._subitemBackColorChanged(Me)
                End If
            End Set
        End Property
        <DefaultValue(GetType(Drawing.Color), "Black"), Category("Appearance"), _
            Description("Determine the color used to draw the value of the TreeNodeSubItem.")> _
        Public Property Color() As Drawing.Color
            Get
                Return _color
            End Get
            Set(ByVal value As Drawing.Color)
                If _color <> value Then
                    _color = value
                    _owner._owner._subitemColorChanged(Me)
                End If
            End Set
        End Property
        <Browsable(False)> _
        Public ReadOnly Property TreeNode() As TreeNode
            Get
                Return _owner
            End Get
        End Property
        <Browsable(False)> _
        Public ReadOnly Property MultiColumnTree() As MultiColumnTree
            Get
                Return _owner._owner
            End Get
        End Property
        ' Behavior
        <Category("Behavior"), DefaultValue(False), _
            Description("Draw subitem value when column format is Bar")> _
        Public Property PrintValueOnBar() As Boolean
            Get
                Return _printValueOnBar
            End Get
            Set(ByVal value As Boolean)
                If _printValueOnBar <> value Then
                    _printValueOnBar = value
                    _owner._owner._subitemPrintValueOnBarChanged(Me)
                End If
            End Set
        End Property
#End Region
    End Class
    ''' <summary>
    ''' Class to represent a Collection of TreeNodeSubItem object.
    ''' </summary>
    Public Class TreeNodeSubItemCollection
        Inherits CollectionBase
        Dim _owner As TreeNode
        <Description("Create an instance of TreeNodeSubItemCollection with a TreeNode as its owner.")> _
        Public Sub New(ByVal owner As TreeNode)
            MyBase.New()
            _owner = owner
        End Sub
#Region "Public Events"
        Public Event Clearing(ByVal sender As Object, ByVal e As CollectionEventArgs)
        Public Event AfterClear(ByVal sender As Object, ByVal e As CollectionEventArgs)
        Public Event Inserting(ByVal sender As Object, ByVal e As CollectionEventArgs)
        Public Event AfterInsert(ByVal sender As Object, ByVal e As CollectionEventArgs)
        Public Event Removing(ByVal sender As Object, ByVal e As CollectionEventArgs)
        Public Event AfterRemove(ByVal sender As Object, ByVal e As CollectionEventArgs)
        Public Event Setting(ByVal sender As Object, ByVal e As CollectionEventArgs)
        Public Event AfterSet(ByVal sender As Object, ByVal e As CollectionEventArgs)
#End Region
#Region "Public Properties"
        <Description("Gets a TreeNodeSubItem object in the collection specified by its index.")> _
        Default Public ReadOnly Property Item(ByVal index As Integer) As TreeNodeSubItem
            Get
                Select Case index
                    Case 0
                        Dim aSubItem As TreeNodeSubItem = New TreeNodeSubItem
                        aSubItem.Value = _owner._text
                        aSubItem.Font = _owner.NodeFont
                        aSubItem.Color = _owner.Color
                        aSubItem.BackColor = _owner.BackColor
                        Return aSubItem
                    Case Is >= 1, Is <= List.Count
                        Return DirectCast(List.Item(index - 1), TreeNodeSubItem)
                End Select
                Return Nothing
            End Get
        End Property
        <Description("Gets the index of a TreeNodeSubItem object in the collection.")> _
        Public ReadOnly Property IndexOf(ByVal item As TreeNodeSubItem) As Integer
            Get
                Return Me.List.IndexOf(item)
            End Get
        End Property
#End Region
#Region "Public Methods"
        <Description("Add a TreeNodeSubItem object to the collection.")> _
        Public Overloads Function Add(ByVal item As TreeNodeSubItem) As TreeNodeSubItem
            item._owner = _owner
            Dim index As Integer = List.Add(item)
            Return DirectCast(List.Item(index), TreeNodeSubItem)
        End Function
        <Description("Add a TreeNodeSubItem object to the collection by providing its value.")> _
        Public Overloads Function Add(ByVal value As Object) As TreeNodeSubItem
            Dim anItem As TreeNodeSubItem = New TreeNodeSubItem(_owner)
            anItem.Value = value
            Return Me.Add(anItem)
        End Function
        <Description("Add a TreeNodeSubItem collection to the collection.")> _
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Public Overloads Sub AddRange(ByVal items As TreeNodeSubItemCollection)
            For Each subItem As TreeNodeSubItem In items
                Me.Add(subItem)
            Next
        End Sub
        <Description("Insert a TreeNodeSubItem object to the collection at specified index.")> _
        Public Sub Insert(ByVal index As Integer, ByVal item As TreeNodeSubItem)
            item._owner = _owner
            List.Insert(index, item)
        End Sub
        <Description("Remove a TreeNodeSubItem object from the collection.")> _
        Public Sub Remove(ByVal item As TreeNodeSubItem)
            If List.Contains(item) Then List.Remove(item)
        End Sub
        <Description("Determine whether a TreeNodeSubItem object exist in the collection.")> _
        Public Function Contains(ByVal item As TreeNodeSubItem) As Boolean
            Return List.Contains(item)
        End Function
#End Region
#Region "Private Methods"
        Private Function containsName(ByVal name As String) As Boolean
            For Each tnsi As TreeNodeSubItem In List
                If String.Compare(tnsi.Name, name, True) = 0 Then Return True
            Next
            Return False
        End Function
#End Region
#Region "Protected Overriden Methods"
        <Description("Performs additional custom processes when validating a value.")> _
        Protected Overrides Sub OnValidate(ByVal value As Object)
            If Not GetType(TreeNodeSubItem).IsAssignableFrom(value.GetType) Then
                Throw New ArgumentException("Value must TreeNodeSubItem", "value")
            End If
        End Sub
        Protected Overrides Sub OnClear()
            RaiseEvent Clearing(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnClear))
        End Sub
        Protected Overrides Sub OnClearComplete()
            RaiseEvent AfterClear(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnClearComplete))
        End Sub
        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As Object)
            If _owner._owner.IsDesignMode Then
                Dim aSubItem As TreeNodeSubItem = DirectCast(value, TreeNodeSubItem)
                'Dim i As Integer = 0
                'Dim find As Boolean = False
                'While i < List.Count And Not find
                '    find = containsName("TreeNodeSubItem" & CStr(i))
                '    If Not find Then i += 1
                'End While
                'aSubItem.Name = "TreeNodeSubItem" & CStr(i)
                aSubItem.Font = _owner._owner.Font
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
#End Region
#Region "Declaration"
    Friend _owner As MultiColumnTree = Nothing
    Friend _font As Font = Nothing
    Friend _parent As TreeNode = Nothing
    Dim _text As String = "TreeNode"
    Dim _name As String = "TreeNode"
    Dim _color As Drawing.Color = Drawing.Color.Black
    Dim _backColor As Drawing.Color = Drawing.Color.Transparent
    Dim _checked As Boolean = False
    Dim _image As Drawing.Image = Nothing
    Dim _expandedImage As Drawing.Image = Nothing
    Dim _tag As Object = Nothing
    Dim _tooltip As String = ""
    Dim _tooltipTitle As String = ""
    Dim _tooltipImage As Image = Nothing
    Dim _useNodeStyleForSubItems As Boolean = True
    Dim _isExpanded As Boolean = False
    Dim _checkState As System.Windows.Forms.CheckState = Windows.Forms.CheckState.Unchecked
    Dim WithEvents _subItems As TreeNodeSubItemCollection
    Dim WithEvents _nodes As TreeNodeCollection
#End Region
#Region "Constructor"
    <Description("Create an instance of TreeNode.")> _
    Public Sub New()
        _owner = New MultiColumnTree
        _subItems = New TreeNodeSubItemCollection(Me)
        _font = _owner.Font
        _nodes = New TreeNodeCollection(Me, _owner)
    End Sub
    <Description("Create an Instance of TreeNode with specified MultiColumnTree as its owner.")> _
    Public Sub New(ByVal owner As MultiColumnTree)
        _owner = owner
        _subItems = New TreeNodeSubItemCollection(Me)
        _font = _owner.Font
        _nodes = New TreeNodeCollection(Me, _owner)
    End Sub
    <Description("Initializes a new instance of the TreeNode class with the specified label text.")> _
    Public Sub New(ByVal text As String)
        _text = text
        _owner = New MultiColumnTree
        _parent = Nothing
        _nodes = New TreeNodeCollection(Me, _owner)
        _subItems = New TreeNodeSubItemCollection(Me)
    End Sub
    <Description("Initializes a new instance of the TreeNode class with the specified label text and child tree nodes.")> _
    Public Sub New(ByVal text As String, ByVal nodes As TreeNodeCollection)
        _text = text
        _owner = New MultiColumnTree
        _parent = Nothing
        _nodes = nodes
        _subItems = New TreeNodeSubItemCollection(Me)
    End Sub
    <Description("Initializes a new instance of the TreeNode class with the specified label text and images to display when the tree node is in a expanded and collapsed state.")> _
    Public Sub New(ByVal text As String, ByVal img As Image, ByVal expImg As Image)
        _text = text
        _image = img
        _expandedImage = expImg
        _owner = New MultiColumnTree
        _parent = Nothing
        _nodes = New TreeNodeCollection(Me, _owner)
        _subItems = New TreeNodeSubItemCollection(Me)
    End Sub
    <Description("Initializes a new instance of the TreeNode class with the specified label text, child tree nodes, and images to display when the tree node is in a expanded and collapsed state.")> _
    Public Sub New(ByVal text As String, ByVal img As Image, ByVal expImg As Image, ByVal nodes As TreeNodeCollection)
        _text = text
        _image = img
        _expandedImage = expImg
        _owner = New MultiColumnTree
        _parent = Nothing
        _nodes = nodes
        _subItems = New TreeNodeSubItemCollection(Me)
    End Sub
#End Region
#Region "Public Properties"
    ' Appearance
    <DefaultValue("TreeNode"), Category("Appearance"), _
        Description("Determine the text displayed in a TreeNode.")> _
    Public Property Text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            If _text <> value Then
                _text = value
                _owner._nodeTextChanged(Me)
            End If
        End Set
    End Property
    <Category("Appearance"), Description("Determine the font used to draw the Text of a TreeNode.")> _
    Public Property NodeFont() As Font
        Get
            If _font Is Nothing Then
                Return _owner.Font
            Else
                Return _font
            End If
        End Get
        Set(ByVal value As Font)
            If _font IsNot value Then
                _font = value
                _owner._nodeFontChanged(Me)
            End If
        End Set
    End Property
    <DefaultValue(GetType(Drawing.Color), "Transparent"), Category("Appearance"), _
        Description("Determine a color used for TreeNode background.")> _
    Public Property BackColor() As Drawing.Color
        Get
            Return _backColor
        End Get
        Set(ByVal value As Drawing.Color)
            If _backColor <> value Then
                _backColor = value
                _owner._nodeBackColorChanged(Me)
            End If
        End Set
    End Property
    <DefaultValue(GetType(Drawing.Color), "Black"), Category("Appearance"), _
        Description("Determine a color used to draw the Text of a TreeNode.")> _
    Public Property Color() As Drawing.Color
        Get
            Return _color
        End Get
        Set(ByVal value As Drawing.Color)
            If _color <> value Then
                _color = value
                _owner._nodeColorChanged(Me)
            End If
        End Set
    End Property
    <DefaultValue(GetType(Image), "Nothing"), Category("Appearance"), _
        Description("Determine an image to be displayed when the TreeNode is Collapsed.")> _
    Public Property Image() As Image
        Get
            Return _image
        End Get
        Set(ByVal value As Drawing.Image)
            If _image IsNot value Then
                _image = value
                _owner._nodeImageChanged(Me)
            End If
        End Set
    End Property
    <DefaultValue(GetType(Image), "Nothing"), Category("Appearance"), _
        Description("Determine an image to be displayed when the TreeNode is Expanded.")> _
    Public Property ExpandedImage() As Image
        Get
            Return _expandedImage
        End Get
        Set(ByVal value As Image)
            If _expandedImage IsNot value Then
                _expandedImage = value
                _owner._nodeExpandedImageChanged(Me)
            End If
        End Set
    End Property
    <DefaultValue(False), Category("Appearance"), _
        Description("Determine whether a TreeNode object is checked.")> _
    Public Property Checked() As Boolean
        Get
            Return _checked
        End Get
        Set(ByVal value As Boolean)
            If _checked <> value Then
                Dim e As TreeNodeEventArgs = New TreeNodeEventArgs(Me, TreeNodeAction.Checked)
                _owner._nodeBeforeCheck(Me, e)
                If Not e.Cancel Then
                    _checked = value
                    If _checked Then
                        _checkState = Windows.Forms.CheckState.Checked
                    Else
                        _checkState = Windows.Forms.CheckState.Unchecked
                    End If
                    _owner._nodeChecked(Me)
                End If
            End If
        End Set
    End Property
    <DefaultValue(True), Category("Appearance"), _
        Description("Determine whether item style is used for all subitems.")> _
    Public Property UseNodeStyleForSubItems() As Boolean
        Get
            Return _useNodeStyleForSubItems
        End Get
        Set(ByVal value As Boolean)
            If _useNodeStyleForSubItems <> value Then
                _useNodeStyleForSubItems = value
                _owner._nodeUseNodeStyleForSubItemsChanged(Me)
            End If
        End Set
    End Property
    ' Design
    <DefaultValue("TreeNode"), Category("Design"), _
        Description("Determine the name of a TreeNode object.")> _
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property
    <DefaultValue(""), EditorAttribute(GetType(System.ComponentModel.Design.MultilineStringEditor), _
        GetType(System.Drawing.Design.UITypeEditor)), Category("Design"), _
        Description("Determine the contents of the ToolTip should be displayed when mouse hover the TreeNode.")> _
    Public Property ToolTip() As String
        Get
            Return _tooltip
        End Get
        Set(ByVal value As String)
            _tooltip = value
        End Set
    End Property
    <DefaultValue(""), Category("Design"), _
        Description("Determine the title of the ToolTip should be displayed when mouse hover the TreeNode.")> _
    Public Property ToolTipTitle() As String
        Get
            Return _tooltipTitle
        End Get
        Set(ByVal value As String)
            _tooltipTitle = value
        End Set
    End Property
    <DefaultValue(GetType(Image), "Nothing"), Category("Design"), _
        Description("Determine the image of the ToolTip should be displayed when mouse hover the TreeNode.")> _
    Public Property ToolTipImage() As Image
        Get
            Return _tooltipImage
        End Get
        Set(ByVal value As Image)
            _tooltipImage = value
        End Set
    End Property
    ' Data
    <DefaultValue(""), Category("Data"), TypeConverter(GetType(StringConverter)), _
        Description("Determine an object data associated with TreeNode object.")> _
    Public Property Tag() As Object
        Get
            Return _tag
        End Get
        Set(ByVal value As Object)
            _tag = value
        End Set
    End Property
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Gets the collection of TreeNode objects assigned to the current tree node.")> _
    Public ReadOnly Property Nodes() As TreeNodeCollection
        Get
            Return _nodes
        End Get
    End Property
    <Category("Data"), _
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
        Description("Gets the collection of TreeNodeSubItem objects assigned to the current TreeNode.")> _
    Public ReadOnly Property SubItems() As TreeNodeSubItemCollection
        Get
            Return _subItems
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property MultiColumnTree() As MultiColumnTree
        Get
            Return _owner
        End Get
    End Property
    <Browsable(False), Description("Gets the path from the root tree node to the current tree node.")> _
    Public ReadOnly Property FullPath() As String
        Get
            If _parent IsNot Nothing Then
                Return _parent.FullPath & _owner.PathSeparator & _text
            Else
                Return _text
            End If
        End Get
    End Property
    <Browsable(False), Description("Gets the parent tree node of the current tree node.")> _
    Public ReadOnly Property Parent() As TreeNode
        Get
            Return _parent
        End Get
    End Property
    <Browsable(False), Description("Gets the zero-based depth of the tree node in the TreeView control.")> _
    Public ReadOnly Property Level() As Integer
        Get
            Dim _level As Integer = 0
            Dim pNode As TreeNode = _parent
            While pNode IsNot Nothing
                _level = _level + 1
                pNode = pNode._parent
            End While
            Return _level
        End Get
    End Property
    <Browsable(False), Description("Gets a value indicating whether the tree node is in the expanded state.")> _
    Public ReadOnly Property IsExpanded() As Boolean
        Get
            Return _isExpanded
        End Get
    End Property
    Public ReadOnly Property CheckState() As System.Windows.Forms.CheckState
        Get
            Return _checkState
        End Get
    End Property
#End Region
#Region "Event Handlers"
    Private Sub _subItems_AfterClear(ByVal sender As Object, ByVal e As CollectionEventArgs) Handles _subItems.AfterClear
        _owner._nodeSubItemsChanged(Me)
    End Sub
    Private Sub _subItems_AfterInsert(ByVal sender As Object, ByVal e As CollectionEventArgs) Handles _subItems.AfterInsert
        _owner._nodeSubItemsChanged(Me)
    End Sub
    Private Sub _subItems_AfterRemove(ByVal sender As Object, ByVal e As CollectionEventArgs) Handles _subItems.AfterRemove
        _owner._nodeSubItemsChanged(Me)
    End Sub
    Private Sub _subItems_AfterSet(ByVal sender As Object, ByVal e As CollectionEventArgs) Handles _subItems.AfterSet
        _owner._nodeSubItemsChanged(Me)
    End Sub
    Private Sub _nodes_AfterInsert(ByVal sender As Object, ByVal e As CollectionEventArgs) Handles _nodes.AfterInsert
        RaiseEvent NodeAdded(Me, e)
    End Sub
    Private Sub _nodes_AfterRemove(ByVal sender As Object, ByVal e As CollectionEventArgs) Handles _nodes.AfterRemove
        RaiseEvent NodeRemoved(Me, e)
    End Sub
    Private Sub _nodes_Clearing(ByVal sender As Object, ByVal e As CollectionEventArgs) Handles _nodes.Clearing
        RaiseEvent NodesOnClear(Me, e)
    End Sub
#End Region
#Region "Public Methods"
    ''' <summary>
    ''' Collapses the tree node.
    ''' </summary>
    <Description("Collapses the tree node.")> _
    Public Sub collapse()
        If _isExpanded Then
            Dim e As TreeNodeEventArgs = New TreeNodeEventArgs(Me, TreeNodeAction.Collapse)
            _owner._nodeBeforeCollapse(Me, e)
            If Not e.Cancel Then
                _isExpanded = False
                _owner._nodeCollapsed(Me)
            End If
        End If
    End Sub
    ''' <summary>
    ''' Collapses the TreeNode and optionally collapses its children.
    ''' </summary>
    <Description("Collapses the TreeNode and optionally collapses its children.")> _
    Public Sub collapse(ByVal ignoreChildren As Boolean)
        If _isExpanded Then
            Dim e As TreeNodeEventArgs = New TreeNodeEventArgs(Me, TreeNodeAction.Collapse)
            _owner._nodeBeforeCollapse(Me, e)
            If Not e.Cancel Then
                _isExpanded = False
                _owner._nodeCollapsed(Me, ignoreChildren)
            Else
                If Not ignoreChildren Then
                    For Each tn As TreeNode In _nodes
                        tn.collapse(ignoreChildren)
                    Next
                End If
            End If
        ElseIf Not ignoreChildren Then
            For Each tn As TreeNode In _nodes
                tn.collapse(ignoreChildren)
            Next
        End If
    End Sub
    ''' <summary>
    ''' Expands the tree node.
    ''' </summary>
    <Description("Expands the tree node.")> _
    Public Sub expand()
        If Not _isExpanded Then
            Dim e As TreeNodeEventArgs = New TreeNodeEventArgs(Me, TreeNodeAction.Expand)
            _owner._nodeBeforeExpand(Me, e)
            If Not e.Cancel Then
                _isExpanded = True
                _owner._nodeExpanded(Me)
            End If
        End If
    End Sub
    ''' <summary>
    ''' Expands all the child tree nodes.
    ''' </summary>
    <Description("Expands all the child tree nodes.")> _
    Public Sub expandAll()
        If Not _isExpanded Then
            Dim e As TreeNodeEventArgs = New TreeNodeEventArgs(Me, TreeNodeAction.Expand)
            _owner._nodeBeforeExpand(Me, e)
            If Not e.Cancel Then
                _isExpanded = True
                _owner._nodeExpanded(Me)
            Else
                For Each tn As TreeNode In _nodes
                    tn.expandAll()
                Next
            End If
        Else
            For Each tn As TreeNode In _nodes
                tn.expandAll()
            Next
        End If
    End Sub
    ''' <summary>
    ''' Returns the number of child tree nodes.
    ''' </summary>
    Public Function getNodeCount(ByVal includeSubTrees As Boolean) As Integer
        Dim result As Integer = _nodes.Count
        If includeSubTrees Then
            For Each tn As TreeNode In _nodes
                result += tn.getNodeCount(includeSubTrees)
            Next
        End If
        Return result
    End Function
    ''' <summary>
    ''' Removes the current tree node from the MultiColumnTree control.
    ''' </summary>
    Public Sub remove()
        If _parent IsNot Nothing Then
            _parent._nodes.Remove(Me)
        Else
            _owner.Nodes.Remove(Me)
        End If
    End Sub
#End Region
#Region "Friend Methods"
    Friend Sub _expand()
        If Not _isExpanded Then
            _isExpanded = True
            _owner._invokeNodeExpanded(Me)
        End If
    End Sub
    Friend Sub _collapse()
        If _isExpanded Then
            _isExpanded = False
            _owner._invokeNodeCollapsed(Me)
        End If
    End Sub
    Friend Sub setCheckState(ByVal state As Windows.Forms.CheckState)
        If _checkState <> state Then
            _checkState = state
            If _checkState = Windows.Forms.CheckState.Checked Then
                If Not _checked Then
                    _checked = True
                    _owner._invokeNodeChecked(Me)
                End If
            ElseIf _checkState = Windows.Forms.CheckState.Unchecked Then
                If _checked Then
                    _checked = False
                    _owner._invokeNodeChecked(Me)
                End If
            End If
        End If
    End Sub
    Friend Sub setChecked(ByVal value As Boolean)
        If _checked <> value Then
            _checked = value
            If _checked Then
                _checkState = Windows.Forms.CheckState.Checked
            Else
                _checkState = Windows.Forms.CheckState.Unchecked
            End If
            _owner._invokeNodeChecked(Me)
        End If
    End Sub
    Friend Sub setParentNOwner()
        _nodes._owner = _owner
        _nodes._parent = Me
        For Each cn As TreeNode In _nodes
            cn._owner = _owner
            cn._parent = Me
            cn.setParentNOwner()
        Next
    End Sub
#End Region
End Class
' Created by : Burhanudin Ashari (red_moon@CodeProject) @ July 22, 2010.
' Modified by : Burhanudin Ashari (red_moon@CodeProject) @ August 30 - September 05, 2010.
' I'm not live my life for the code, but I'll live and enhance it through the code.
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Globalization
''' <summary>
''' Represent a control to displays a collection of TreeNode.
''' </summary>
''' <remarks>
''' This control allows you to display a list of nodes with node text, and optionally, additional information of an item (subitem), and images displayed in when collapsed or expanded.
''' </remarks>
Public Class MultiColumnTree
    Inherits Windows.Forms.Control
#Region "Private Constants."
    ''' <summary>
    ''' Margin size of each subitem from its bounding rectangle.
    ''' </summary>
    Const _subItemMargin As Integer = 3
    ''' <summary>
    ''' Minimum value of the Indent property.
    ''' </summary>
    Const _mininumIndent As Integer = 15
#End Region
#Region "Public Classes."
    ''' <summary>
    ''' Represent a collection of ColumnHeader objects in a MultiColumnTree.
    ''' </summary>
    <Description("Represent a collection of ColumnHeader objects.")> _
    Public Class ColumnHeaderCollection
        Inherits CollectionBase
        Friend _owner As MultiColumnTree
#Region "Constructor"
        Public Sub New(ByVal owner As MultiColumnTree)
            MyBase.New()
            _owner = owner
        End Sub
#End Region
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
        <Description("Gets a ColumnHeader object in the collection specified by its index.")> _
        Default Public ReadOnly Property Item(ByVal index As Integer) As ColumnHeader
            Get
                If index >= 0 And index < List.Count Then Return DirectCast(List.Item(index), ColumnHeader)
                Return Nothing
            End Get
        End Property
        <Description("Gets the index of a ColumnHeader object in the collection.")> _
        Public ReadOnly Property IndexOf(ByVal item As ColumnHeader) As Integer
            Get
                Return Me.List.IndexOf(item)
            End Get
        End Property
#End Region
#Region "Public Methods"
        <Description("Add a ColumnHeader object to the collection.")> _
        Public Overloads Function Add(ByVal header As ColumnHeader) As ColumnHeader
            If Not Me.Contains(header) Then ' Avoid adding the same item multiple times.
                header._owner = _owner
                AddHandler header.EnableFilteringChanged, AddressOf columnVisibilityChanged
                AddHandler header.EnableFrozenChanged, AddressOf columnVisibilityChanged
                AddHandler header.EnableHiddenChanged, AddressOf columnVisibilityChanged
                AddHandler header.EnableSortingChanged, AddressOf columnVisibilityChanged
                AddHandler header.FrozenChanged, AddressOf columnVisibilityChanged
                AddHandler header.ImageChanged, AddressOf columnVisibilityChanged
                AddHandler header.SizeTypeChanged, AddressOf columnVisibilityChanged
                AddHandler header.SortOrderChanged, AddressOf columnSortOrderChanged
                AddHandler header.TextAlignChanged, AddressOf columnAppearanceChanged
                AddHandler header.TextChanged, AddressOf columnAppearanceChanged
                AddHandler header.VisibleChanged, AddressOf columnVisibilityChanged
                AddHandler header.WidthChanged, AddressOf columnVisibilityChanged
                AddHandler header.MaximumValueChanged, AddressOf columnItemsRelatedValueChanged
                AddHandler header.MinimumValueChanged, AddressOf columnItemsRelatedValueChanged
                AddHandler header.FormatChanged, AddressOf columnItemsRelatedValueChanged
                AddHandler header.ColumnAlignChanged, AddressOf columnItemsRelatedValueChanged
                AddHandler header.CustomFormatChanged, AddressOf columnCustomFormatChanged
                Dim index As Integer = List.Add(header)
                Return DirectCast(List.Item(index), ColumnHeader)
            End If
            Return header
        End Function
        <Description("Add a ColumnHeader object to the collection by providing its text.")> _
        Public Overloads Function Add(ByVal text As String) As ColumnHeader
            Dim aHeader As ColumnHeader = New ColumnHeader(_owner)
            aHeader.Text = text
            Return Me.Add(aHeader)
        End Function
        <Description("Add a ColumnHeader collection to the collection.")> _
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Public Overloads Sub AddRange(ByVal columns As ColumnHeaderCollection)
            For Each column As ColumnHeader In columns
                Me.Add(column)
            Next
        End Sub
        <Description("Insert a ColumnHeader object to the collection at specified index.")> _
        Public Sub Insert(ByVal index As Integer, ByVal header As ColumnHeader)
            header._owner = _owner
            AddHandler header.EnableFilteringChanged, AddressOf columnVisibilityChanged
            AddHandler header.EnableFrozenChanged, AddressOf columnVisibilityChanged
            AddHandler header.EnableHiddenChanged, AddressOf columnVisibilityChanged
            AddHandler header.EnableSortingChanged, AddressOf columnVisibilityChanged
            AddHandler header.FrozenChanged, AddressOf columnVisibilityChanged
            AddHandler header.ImageChanged, AddressOf columnVisibilityChanged
            AddHandler header.SizeTypeChanged, AddressOf columnVisibilityChanged
            AddHandler header.SortOrderChanged, AddressOf columnSortOrderChanged
            AddHandler header.TextAlignChanged, AddressOf columnAppearanceChanged
            AddHandler header.TextChanged, AddressOf columnAppearanceChanged
            AddHandler header.VisibleChanged, AddressOf columnVisibilityChanged
            AddHandler header.WidthChanged, AddressOf columnVisibilityChanged
            AddHandler header.MaximumValueChanged, AddressOf columnItemsRelatedValueChanged
            AddHandler header.MinimumValueChanged, AddressOf columnItemsRelatedValueChanged
            AddHandler header.FormatChanged, AddressOf columnItemsRelatedValueChanged
            AddHandler header.ColumnAlignChanged, AddressOf columnItemsRelatedValueChanged
            AddHandler header.CustomFormatChanged, AddressOf columnCustomFormatChanged
            List.Insert(index, header)
        End Sub
        <Description("Remove a ColumnHeader object from the collection.")> _
        Public Sub Remove(ByVal header As ColumnHeader)
            If Not List.Contains(header) Then Return
            RemoveHandler header.EnableFilteringChanged, AddressOf columnVisibilityChanged
            RemoveHandler header.EnableFrozenChanged, AddressOf columnVisibilityChanged
            RemoveHandler header.EnableHiddenChanged, AddressOf columnVisibilityChanged
            RemoveHandler header.EnableSortingChanged, AddressOf columnVisibilityChanged
            RemoveHandler header.FrozenChanged, AddressOf columnVisibilityChanged
            RemoveHandler header.ImageChanged, AddressOf columnVisibilityChanged
            RemoveHandler header.SizeTypeChanged, AddressOf columnVisibilityChanged
            RemoveHandler header.SortOrderChanged, AddressOf columnSortOrderChanged
            RemoveHandler header.TextAlignChanged, AddressOf columnAppearanceChanged
            RemoveHandler header.TextChanged, AddressOf columnAppearanceChanged
            RemoveHandler header.VisibleChanged, AddressOf columnVisibilityChanged
            RemoveHandler header.WidthChanged, AddressOf columnVisibilityChanged
            RemoveHandler header.MaximumValueChanged, AddressOf columnItemsRelatedValueChanged
            RemoveHandler header.MinimumValueChanged, AddressOf columnItemsRelatedValueChanged
            RemoveHandler header.FormatChanged, AddressOf columnItemsRelatedValueChanged
            RemoveHandler header.ColumnAlignChanged, AddressOf columnItemsRelatedValueChanged
            RemoveHandler header.CustomFormatChanged, AddressOf columnCustomFormatChanged
            _owner._columnControl.removeHost(header)
            _owner._columnControl.Invalidate()
            List.Remove(header)
        End Sub
        <Description("Determine whether a ColumnHeader object exist in the collection.")> _
        Public Function Contains(ByVal header As ColumnHeader) As Boolean
            Return List.Contains(header)
        End Function
#End Region
#Region "Private Methods"
        Private Function containsName(ByVal name As String) As Boolean
            For Each ch As ColumnHeader In List
                If String.Compare(ch.Name, name, True) = 0 Then Return True
            Next
            Return False
        End Function
#End Region
#Region "Protected Overriden Methods"
        <Description("Performs additional custom processes when validating a value.")> _
        Protected Overrides Sub OnValidate(ByVal value As Object)
            If Not GetType(ColumnHeader).IsAssignableFrom(value.GetType) Then
                Throw New ArgumentException("Value must ColumnHeader", "value")
            End If
        End Sub
        Protected Overrides Sub OnClear()
            For Each header As ColumnHeader In List
                RemoveHandler header.EnableFilteringChanged, AddressOf columnVisibilityChanged
                RemoveHandler header.EnableFrozenChanged, AddressOf columnVisibilityChanged
                RemoveHandler header.EnableHiddenChanged, AddressOf columnVisibilityChanged
                RemoveHandler header.EnableSortingChanged, AddressOf columnVisibilityChanged
                RemoveHandler header.FrozenChanged, AddressOf columnVisibilityChanged
                RemoveHandler header.ImageChanged, AddressOf columnVisibilityChanged
                RemoveHandler header.SizeTypeChanged, AddressOf columnVisibilityChanged
                RemoveHandler header.SortOrderChanged, AddressOf columnSortOrderChanged
                RemoveHandler header.TextAlignChanged, AddressOf columnAppearanceChanged
                RemoveHandler header.TextChanged, AddressOf columnAppearanceChanged
                RemoveHandler header.VisibleChanged, AddressOf columnVisibilityChanged
                RemoveHandler header.WidthChanged, AddressOf columnVisibilityChanged
                RemoveHandler header.MaximumValueChanged, AddressOf columnItemsRelatedValueChanged
                RemoveHandler header.MinimumValueChanged, AddressOf columnItemsRelatedValueChanged
                RemoveHandler header.FormatChanged, AddressOf columnItemsRelatedValueChanged
                RemoveHandler header.ColumnAlignChanged, AddressOf columnItemsRelatedValueChanged
                RemoveHandler header.CustomFormatChanged, AddressOf columnCustomFormatChanged
            Next
            RaiseEvent Clearing(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnClear))
        End Sub
        Protected Overrides Sub OnClearComplete()
            _owner._columnControl.clearHosts()
            _owner._columnControl.Invalidate()
            RaiseEvent AfterClear(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnClearComplete))
        End Sub
        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As Object)
            RaiseEvent Inserting(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnInsert, index, value))
        End Sub
        Protected Overrides Sub OnInsertComplete(ByVal index As Integer, ByVal value As Object)
            _owner._columnControl.addHost(value)
            _owner._columnControl.Invalidate()
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
#Region "Column event handlers"
        Private Sub columnAppearanceChanged(ByVal sender As Object, ByVal e As EventArgs)
            _owner._columnControl.Invalidate()
        End Sub
        Private Sub columnVisibilityChanged(ByVal sender As Object, ByVal e As EventArgs)
            _owner._columnControl.relocateHosts(IIf(_owner._hScroll.Visible, -_owner._hScroll.Value, 0))
            _owner._columnControl.Invalidate()
            _owner.measureAll()
            _owner.relocateAll()
            _owner.Invalidate()
        End Sub
        Private Sub columnSortOrderChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim ch As ColumnHeader = sender
            If ch.EnableSorting Then
                If ch.SortOrder <> SortOrder.None Then
                    For Each otherCH As ColumnHeader In List
                        If otherCH IsNot ch Then otherCH.SortOrder = SortOrder.None
                    Next
                    _owner._columnRef = _owner._columns.IndexOf(ch)
                    _owner.sortAll()
                    _owner.relocateAll()
                    _owner.Invalidate()
                End If
            End If
        End Sub
        Private Sub columnItemsRelatedValueChanged(ByVal sender As Object, ByVal e As EventArgs)
            _owner.Invalidate()
        End Sub
        Private Sub columnCustomFormatChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim ch As ColumnHeader = DirectCast(sender, ColumnHeader)
            If ch.Format = ColumnFormat.Custom Or ch.Format = ColumnFormat.CustomDateTime Then
                _owner.Invalidate()
            End If
        End Sub
#End Region
    End Class
    ''' <summary>
    ''' Represent a collection of Checked TreeNode objects in a MultiColumnTree.
    ''' </summary>
    <Description("Represent a collection of Checked TreeNode objects in a MultiColumnTree.")> _
    Public Class CheckedTreeNodeCollection
        Inherits CollectionBase
        Friend _owner As MultiColumnTree
#Region "Constructor"
        Public Sub New(ByVal owner As MultiColumnTree)
            MyBase.New()
            _owner = owner
        End Sub
#End Region
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
        <Description("Gets a TreeNode object in the collection specified by its index.")> _
        Default Public ReadOnly Property Item(ByVal index As Integer) As TreeNode
            Get
                If index >= 0 And index < List.Count Then Return DirectCast(List.Item(index), TreeNode)
                Return Nothing
            End Get
        End Property
        <Description("Gets the index of a TreeNode object in the collection.")> _
        Public ReadOnly Property IndexOf(ByVal node As TreeNode) As Integer
            Get
                Return Me.List.IndexOf(node)
            End Get
        End Property
#End Region
#Region "Public Methods"
        <Description("Add a TreeNode object to the collection.")> _
        Friend Overloads Function Add(ByVal node As TreeNode) As TreeNode
            If Not Me.Contains(node) Then ' Avoid adding the same item multiple times.
                Dim index As Integer = List.Add(node)
                Return DirectCast(List.Item(index), TreeNode)
            End If
            Return node
        End Function
        <Description("Add a TreeNode collection to the collection.")> _
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Friend Overloads Sub AddRange(ByVal nodes As TreeNodeCollection)
            For Each node As TreeNode In nodes
                Me.Add(node)
            Next
        End Sub
        <Description("Remove a TreeNode object from the collection.")> _
        Public Sub Remove(ByVal node As TreeNode)
            If List.Contains(node) Then
                If Not node.Checked Then node.Checked = False
                List.Remove(node)
            End If
        End Sub
        <Description("Determine whether a TreeNode object exist in the collection.")> _
        Public Function Contains(ByVal node As TreeNode) As Boolean
            Return List.Contains(node)
        End Function
#End Region
#Region "Protected Overriden Methods"
        <Description("Performs additional custom processes when validating a value.")> _
        Protected Overrides Sub OnValidate(ByVal value As Object)
            If Not GetType(TreeNode).IsAssignableFrom(value.GetType) Then
                Throw New ArgumentException("Value must TreeNode", "value")
            End If
        End Sub
        Protected Overrides Sub OnClear()
            For Each tn As TreeNode In List
                tn.Checked = False
            Next
            RaiseEvent Clearing(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnClear))
        End Sub
        Protected Overrides Sub OnClearComplete()
            RaiseEvent AfterClear(Me, New CollectionEventArgs(CollectionEventArgs.EventType.OnClearComplete))
        End Sub
        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As Object)
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
#Region "Public Events."
    ''' <summary>
    ''' Occurs before tree node check box is checked.
    ''' </summary>
    Public Event BeforeCheck(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
    ''' <summary>
    ''' Occurs afetr tree node check box is checked.
    ''' </summary>
    Public Event AfterCheck(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
    ''' <summary>
    ''' Occurs before tree node is collapsed.
    ''' </summary>
    Public Event BeforeCollapse(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
    ''' <summary>
    ''' Occurs after tree node is collapsed.
    ''' </summary>
    Public Event AfterCollapse(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
    ''' <summary>
    ''' Occurs before tree node is expanded.
    ''' </summary>
    Public Event BeforeExpand(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
    ''' <summary>
    ''' Occurs after tree node is expanded.
    ''' </summary>
    Public Event AfterExpand(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
    ''' <summary>
    ''' Occurs before tree node label text is edited.
    ''' </summary>
    Public Event BeforeLabelEdit(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
    ''' <summary>
    ''' Occurs after tree node label text is edited.
    ''' </summary>
    Public Event AfterLabelEdit(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
    ''' <summary>
    ''' Occurs before tree node is selected.
    ''' </summary>
    Public Event BeforeSelect(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
    ''' <summary>
    ''' Occurs after tree node is selected.
    ''' </summary>
    Public Event AfterSelect(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
    ''' <summary>
    ''' Occurs when mouse pointer is hover a tree node.
    ''' </summary>
    Public Event NodeMouseHover(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
    ''' <summary>
    ''' Occurs when mouse pointer is leaving a tree node.
    ''' </summary>
    Public Event NodeMouseLeave(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
    ''' <summary>
    ''' Occurs when mouse button is pressed over a tree node.
    ''' </summary>
    Public Event NodeMouseDown(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs)
    ''' <summary>
    ''' Occurs when mouse button is released over a tree node.
    ''' </summary>
    Public Event NodeMouseUp(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs)
    ''' <summary>
    ''' Occurs when column header has been reordered.
    ''' </summary>
    Public Event ColumnOrderChanged(ByVal sender As Object, ByVal e As ColumnEventArgs)
    ''' <summary>
    ''' Occurs when filter parameter of a column has been changed.
    ''' </summary>
    Public Event ColumnFilterChanged(ByVal sender As Object, ByVal e As ColumnEventArgs)
    ''' <summary>
    ''' Occurs when custom filter of a column is choosen.
    ''' </summary>
    Public Event ColumnCustomFilter(ByVal sender As Object, ByVal e As ColumnCustomFilterEventArgs)
    ''' <summary>
    ''' Occurs when the width of a column has been changed.
    ''' </summary>
    Public Event ColumnSizeChanged(ByVal sender As Object, ByVal e As ColumnEventArgs)
    ''' <summary>
    ''' Occurs when background of a column in MultiColumnTree need to paint.
    ''' </summary>
    Public Event ColumnBackgroundPaint(ByVal sender As Object, ByVal e As ColumnBackgroundPaintEventArgs)
    ''' <summary>
    ''' Occurs when selected node has been changed.
    ''' </summary>
    Public Event SelectedNodeChanged(ByVal sender As Object, ByVal e As EventArgs)
#End Region
#Region "Members."
    ' Components
    Dim WithEvents _nodes As TreeNodeCollection
    Dim WithEvents _checkedNodes As CheckedTreeNodeCollection
    Dim WithEvents _columns As ColumnHeaderCollection
    Dim WithEvents _vScroll As VScrollBar = New VScrollBar
    Dim WithEvents _hScroll As HScrollBar = New HScrollBar
    ' Properties
    Dim _checkBoxes As Boolean = False
    Dim _showColumnOptions As Boolean = True
    Dim _allowMultiline As Boolean = False
    Dim _ci As CultureInfo = Renderer.Drawing.en_us_ci
    Dim _fullRowSelect As Boolean = False
    Dim _labelEdit As Boolean = False
    Dim _nodeToolTip As Boolean = True
    Dim _pathSeparator As String = "\"
    Dim _showRootLines As Boolean = False
    Dim _showNodeLines As Boolean = False
    Dim _showImages As Boolean = True
    Dim _indent As Integer = 20
    ' Internal use
    Dim _columnRef As Integer = -1                          ' Index of a ColumnHeader that perform sort operation.
    Dim _internalThread As Boolean = False                  ' Indicating whether an event is called from internal process, to avoid multiple calls of an operation on an event.
    Dim _clientArea As Rectangle                            ' An area to draw TreeNode and ListViewGroup.
    Dim _linePenBlend As ColorBlend                         ' A color blend to draw line separator of each column in ListView.
    Dim _needToolTip As Boolean = False                     ' Indicating when a tooltip need to be shown.
    Dim _currentEditedHost As TreeNodeHost = Nothing        ' A TreeNodeHost object that performs label editing operation.
    Dim WithEvents _tooltip As ToolTip                      ' ToolTip object to show Item tooltip.
    Dim WithEvents _columnControl As ColumnHeaderControl    ' Custom Control to handle all column header operations.
    Dim WithEvents _txtEditor As TextBoxLabelEditor         ' TextBox control to perform label editng operation.
    ' For text measuring purposes
    Dim _gBmp As Bitmap = New Bitmap(1, 1)              ' A bitmap to create a Graphics object.
    Dim _gObj As Graphics = Graphics.FromImage(_gBmp)   ' A Graphics object to measure text, to support text formating in measurement.
    ' TreeNode Host
    Dim _nodeHosts As List(Of TreeNodeHost) = New List(Of TreeNodeHost)         ' A list of TreeNodeHost.  All node host is stored here.
    Dim _selectedHost As TreeNodeHost = Nothing                                 ' Host of selected item.
    ' ToolTip
    Dim _currentToolTip As String = ""                  ' A tooltip text needed to be shown.
    Dim _currentToolTipTitle As String = ""             ' A tooltip title needed to be shown.
    Dim _currentToolTipImage As Image = Nothing         ' A tooltip image needed to be shown.
    Dim _currentToolTipRect As Rectangle                ' An area that must be avoided by the tooltip.
    Dim _tooltipCaller As TreeNodeHost = Nothing        ' A TreeNodeHost that need the tooltip to be shown.
#End Region
#Region "Core Engines.  Classes to handle all objects, key and mouse event handlers, and as a visual representation of an object associated with ListView."
    ''' <summary>
    ''' Class to hosts all columns and handles all of the operations.
    ''' </summary>
    Private Class ColumnHeaderControl
        Inherits Windows.Forms.Control
        ''' <summary>
        ''' Host for each ColumnHeader in ColumnHeaderControl.
        ''' </summary>
        Private Class ColumnHost
            Dim _column As ColumnHeader
            Dim _owner As ColumnHeaderControl
            Dim _rect As Rectangle
            Dim _selected As Boolean = False
            Dim _onHover As Boolean = False
            Dim _onHoverSplit As Boolean = False
            Dim _onMouseDown As Boolean = False
            Dim _onMouseDownSplit As Boolean = False
            Dim _filterHandler As ColumnFilterHandle
            Public Sub New(ByVal column As ColumnHeader, ByVal owner As ColumnHeaderControl)
                _column = column
                _owner = owner
                _filterHandler = New ColumnFilterHandle(_column)
            End Sub
            ''' <summary>
            ''' Draw ColumnHeader object in the ColumnHeaderControl.
            ''' </summary>
            ''' <param name="g">Graphics object to draw the column.</param>
            Public Sub draw(ByVal g As Graphics)
                Dim txtColRect As Rectangle = _rect
                Dim txtColFormat As StringFormat = New StringFormat
                Dim imgColRect As Rectangle = _rect
                Dim sortSignLeft As Integer = _rect.Right - 8
                If _column.Image IsNot Nothing Then
                    imgColRect.X = txtColRect.X
                    imgColRect.Size = Renderer.Drawing.scaleImage(_column.Image, 16)
                    imgColRect.Y = (_rect.Height - imgColRect.Height) / 2
                    txtColRect.X = txtColRect.X + 18
                    txtColRect.Width = txtColRect.Width - 18
                End If
                If _column.EnableFiltering Then
                    txtColRect.Width = txtColRect.Width - 11
                    sortSignLeft = _rect.Right - 18
                End If
                If _column.SortOrder <> SortOrder.None And _column.EnableSorting Then
                    txtColRect.Width = txtColRect.Width - 9
                End If
                txtColFormat.LineAlignment = StringAlignment.Center
                Select Case _column.TextAlign
                    Case HorizontalAlignment.Center
                        txtColFormat.Alignment = StringAlignment.Center
                    Case HorizontalAlignment.Left
                        txtColFormat.Alignment = StringAlignment.Near
                    Case HorizontalAlignment.Right
                        txtColFormat.Alignment = StringAlignment.Far
                End Select
                txtColFormat.FormatFlags = txtColFormat.FormatFlags Or StringFormatFlags.NoWrap
                txtColFormat.Trimming = StringTrimming.EllipsisCharacter
                Dim bgBrush As LinearGradientBrush = New LinearGradientBrush(_rect, _
                        Color.Black, Color.White, LinearGradientMode.Vertical)
                bgBrush.InterpolationColors = Renderer.Column.NormalBlend
                g.FillRectangle(bgBrush, _rect)
                bgBrush.Dispose()
                bgBrush = Nothing
                If _owner.Enabled Then
                    Dim splitBrush As LinearGradientBrush = Nothing
                    Dim splitRect As Rectangle = New Rectangle(_rect.Right - 10, _
                        _rect.Y, 10, _rect.Height)
                    Dim linePen As Pen = Renderer.Column.NormalBorderPen
                    If _onHover Then
                        bgBrush = New LinearGradientBrush(_rect, _
                            Color.Black, Color.White, LinearGradientMode.Vertical)
                        If _column.EnableFiltering Then
                            splitBrush = New LinearGradientBrush(splitRect, _
                                Color.Black, Color.White, LinearGradientMode.Vertical)
                            If _onMouseDownSplit Then
                                splitBrush.InterpolationColors = Renderer.Column.PressedBlend
                            Else
                                If _onHoverSplit Then
                                    splitBrush.InterpolationColors = Renderer.Column.HLitedDropDownBlend
                                Else
                                    splitBrush.InterpolationColors = Renderer.Column.HLitedBlend
                                End If
                            End If
                        End If
                        If _onMouseDown Then
                            bgBrush.InterpolationColors = Renderer.Column.PressedBlend
                        Else
                            bgBrush.InterpolationColors = Renderer.Column.HLitedBlend
                        End If
                    Else
                        If _selected Then
                            bgBrush = New LinearGradientBrush(_rect, _
                                Color.Black, Color.White, LinearGradientMode.Vertical)
                            bgBrush.InterpolationColors = Renderer.Column.SelectedBlend
                        End If
                    End If
                    If bgBrush IsNot Nothing Then
                        linePen = Renderer.Column.ActiveBorderPen
                        g.FillRectangle(bgBrush, _rect)
                        bgBrush.Dispose()
                    End If
                    If splitBrush IsNot Nothing Then
                        g.FillRectangle(splitBrush, splitRect)
                        splitBrush.Dispose()
                    End If
                    If _column.EnableFiltering Then
                        g.DrawLine(linePen, _rect.Right - 10, _
                            _rect.Y + 1, _rect.Right - 10, _rect.Bottom - 2)
                        Renderer.Drawing.drawTriangle(g, _rect.Right - 8, _rect.Y + CInt((_rect.Height - 6) / 2), _
                            Color.FromArgb(21, 66, 139), Color.White, Renderer.Drawing.TriangleDirection.Down)
                    End If
                Else
                    If _column.EnableFiltering Then
                        g.DrawLine(Pens.Gray, _rect.Right - 10, _
                            _rect.Y + 1, _rect.Right - 10, _rect.Bottom - 2)
                        Renderer.Drawing.drawTriangle(g, _rect.Right - 8, _rect.Y + CInt((_rect.Height - 6) / 2), _
                            Color.Gray, Color.White, Renderer.Drawing.TriangleDirection.Down)
                    End If
                End If
                If _column.Image IsNot Nothing Then
                    If _owner.Enabled Then
                        g.DrawImage(_column.Image, imgColRect)
                    Else
                        Renderer.Drawing.grayscaledImage(_column.Image, imgColRect, g)
                    End If
                End If
                g.DrawString(_column.Text, _owner.Font, _
                    IIf(_owner.Enabled, Renderer.Column.TextBrush, _
                    Renderer.Drawing.DisabledTextBrush), txtColRect, txtColFormat)
                If _column.EnableSorting Then
                    Select Case _column.SortOrder
                        Case SortOrder.Ascending
                            Renderer.Drawing.drawTriangle(g, sortSignLeft, _rect.Y + CInt((_rect.Height - 6) / 2), _
                                IIf(_owner.Enabled, Color.FromArgb(21, 66, 139), Color.Gray), Color.White, _
                                Renderer.Drawing.TriangleDirection.Up)
                        Case SortOrder.Descending
                            Renderer.Drawing.drawTriangle(g, sortSignLeft, _rect.Y + CInt((_rect.Height - 6) / 2), _
                                IIf(_owner.Enabled, Color.FromArgb(21, 66, 139), Color.Gray), Color.White, _
                                Renderer.Drawing.TriangleDirection.Down)
                    End Select
                End If
                If _onMouseDown Then Renderer.Column.drawPressedShadow(g, _rect)
            End Sub
            ''' <summary>
            ''' Draw ColumnHeader object when moved.
            ''' </summary>
            ''' <param name="g">Graphics object to draw the column.</param>
            ''' <param name="rect">Area where the column must be drawn.</param>
            ''' <param name="canDrop">Determine whether the column can be dropped on a location.</param>
            Public Sub drawMoved(ByVal g As Graphics, ByVal rect As Rectangle, _
                Optional ByVal canDrop As Boolean = True)
                Dim txtColRect As Rectangle = rect
                Dim txtColFormat As StringFormat = New StringFormat
                Dim txtBrush As SolidBrush = New SolidBrush(Color.FromArgb(191, 127, 127, 127))
                Dim borderPen As Pen = New Pen(Color.FromArgb(191, 127, 127, 127), 2)
                If _column.Image IsNot Nothing Then
                    txtColRect.X = txtColRect.X + 18
                    txtColRect.Width = txtColRect.Width - 18
                End If
                If _column.EnableFiltering Then
                    txtColRect.Width = txtColRect.Width - 11
                End If
                If _column.SortOrder <> SortOrder.None And _column.EnableSorting Then
                    txtColRect.Width = txtColRect.Width - 9
                End If
                txtColFormat.LineAlignment = StringAlignment.Center
                Select Case _column.TextAlign
                    Case HorizontalAlignment.Center
                        txtColFormat.Alignment = StringAlignment.Center
                    Case HorizontalAlignment.Left
                        txtColFormat.Alignment = StringAlignment.Near
                    Case HorizontalAlignment.Right
                        txtColFormat.Alignment = StringAlignment.Far
                End Select
                txtColFormat.FormatFlags = txtColFormat.FormatFlags Or StringFormatFlags.NoWrap
                txtColFormat.Trimming = StringTrimming.EllipsisCharacter
                g.DrawString(_column.Text, _owner.Font, txtBrush, txtColRect, txtColFormat)
                g.DrawRectangle(borderPen, rect)
                If Not canDrop Then
                    Dim rectSign As Rectangle = New Rectangle(rect.X + 2, rect.Y + 2, rect.Height - 5, rect.Height - 5)
                    g.FillEllipse(New SolidBrush(Color.FromArgb(191, 255, 0, 0)), rectSign)
                    g.DrawEllipse(New Pen(Color.FromArgb(191, 0, 0, 0)), rectSign)
                    g.DrawLine(New Pen(Color.White, 3), rectSign.X + 2, rectSign.Y + CInt(rectSign.Height / 2), rectSign.Right - 3, rectSign.Y + CInt(rectSign.Height / 2))
                End If
            End Sub
            ''' <summary>
            ''' Gets a value indicating a column header need a tooltip to be shown.
            ''' </summary>
            ''' <returns>True if the column need tooltip.</returns>
            Private Function needToolTip() As Boolean
                Dim txtColRect As Rectangle = _rect
                If _column.Image IsNot Nothing Then
                    txtColRect.X = txtColRect.X + 18
                    txtColRect.Width = txtColRect.Width - 18
                End If
                If _column.EnableFiltering Then
                    txtColRect.Width = txtColRect.Width - 11
                End If
                If _column.SortOrder <> SortOrder.None And _column.EnableSorting Then
                    txtColRect.Width = txtColRect.Width - 9
                End If
                Return TextRenderer.MeasureText(_column.Text, _owner.Font).Width > txtColRect.Width
            End Function
            ''' <summary>
            ''' Test whether mouse pointer is moved over the column.
            ''' </summary>
            ''' <returns>True if the state is changed and need to change the appearance.</returns>
            Public Function mouseMove(ByVal e As MouseEventArgs) As Boolean
                If _owner._owner._showColumnOptions Then
                    If _owner._optRect.Contains(e.Location) Then
                        If _onHover Or _onHoverSplit Then
                            _onHover = False
                            _onHoverSplit = False
                            Return True
                        End If
                        Return False
                    End If
                End If
                If e.X <= _owner._frozenRight And Not _column.Frozen Then
                    If _onHover Or _onHoverSplit Then
                        _onHover = False
                        _onHoverSplit = False
                        Return True
                    End If
                    Return False
                End If
                Dim stateChanged As Boolean = False
                Dim rectSplit As Rectangle = New Rectangle(0, 0, 0, 0)
                If _column.EnableFiltering Then rectSplit = New Rectangle(_rect.Right - 10, _rect.Y, 10, _rect.Height)
                If _rect.Contains(e.Location) Then
                    If rectSplit.Contains(e.Location) Then
                        If Not _onHoverSplit Then
                            _onHoverSplit = True
                            stateChanged = True
                        End If
                    Else
                        If _onHoverSplit Then
                            _onHoverSplit = False
                            stateChanged = True
                        End If
                    End If
                    If Not _onHover Then
                        _onHover = True
                        _owner._currentToolTipRect = _rect
                        If Renderer.ToolTip.containsToolTip(_column.ToolTipTitle, _
                            _column.ToolTip, _column.ToolTipImage) Then
                            _owner._currentToolTip = _column.ToolTip
                            _owner._currentToolTipTitle = _column.ToolTipTitle
                            _owner._currentToolTipImage = _column.ToolTipImage
                        Else
                            If needToolTip() Then _owner._currentToolTip = _column.Text
                        End If
                        stateChanged = True
                    End If
                Else
                    If _onHover Or _onHoverSplit Then
                        _onHover = False
                        _onHoverSplit = False
                        stateChanged = True
                    End If
                End If
                Return stateChanged
            End Function
            ''' <summary>
            ''' Test whether the mouse left button is pressed over the column, whether it pressed on the column, or on the column' split.
            ''' </summary>
            Public Function mouseDown() As Boolean
                If _onHover Or _onHoverSplit Then
                    If _owner._selectedHost IsNot Nothing Then _owner._selectedHost._selected = False
                    _selected = True
                    _owner._selectedHost = Me
                    If _onHoverSplit Then
                        _onMouseDownSplit = True
                        _onMouseDown = False
                        Dim rectSplit As Rectangle = New Rectangle(0, 0, 0, 0)
                        rectSplit = New Rectangle(_rect.Right - 10, _rect.Y, 10, _rect.Height)
                        _owner.showFilterPopup(New FilterChooser(_filterHandler, _owner._toolStrip, _
                            Renderer.ToolTip.TextFont, _owner._owner._ci), rectSplit)
                    Else
                        _onMouseDown = True
                        _onMouseDownSplit = False
                        Return True
                    End If
                Else
                    If _onMouseDown Or _onMouseDownSplit Then
                        _onMouseDown = False
                        _onMouseDownSplit = False
                        Return True
                    End If
                End If
                Return False
            End Function
            ''' <summary>
            ''' Test whether the mouse left button is released over the column.
            ''' </summary>
            Public Function mouseUp() As Boolean
                If _onMouseDown Or _onMouseDownSplit Then
                    _onMouseDown = False
                    _onMouseDownSplit = False
                    Return True
                End If
                Return False
            End Function
            ''' <summary>
            ''' Test whether the mouse leaving the column.
            ''' </summary>
            Public Function mouseLeave() As Boolean
                If _onHover Or _onHoverSplit Or _onMouseDown Or _onMouseDownSplit Then
                    _onHover = False
                    _onHoverSplit = False
                    _onMouseDown = False
                    _onMouseDownSplit = False
                    Return True
                End If
                Return False
            End Function
            ''' <summary>
            ''' Add a TreeNode or its subitem to the filter parameter, based on the column and the associated subitems.
            ''' </summary>
            ''' <param name="item">TreeNode object to be added.</param>
            Public Sub addItemToFilter(ByVal item As TreeNode)
                If _column.EnableFiltering Then
                    Dim columnIndex As Integer = _owner._owner._columns.IndexOf(_column)
                    If item.SubItems(columnIndex) IsNot Nothing Then
                        _filterHandler.addFilter(item.SubItems(columnIndex).Value)
                    End If
                End If
            End Sub
            ''' <summary>
            ''' Clear all filter parameter on the column.
            ''' </summary>
            Public Sub clearFilter()
                _filterHandler.Items.Clear()
            End Sub
            ''' <summary>
            ''' Determine whether the column has been selected.
            ''' </summary>
            Public Property Selected() As Boolean
                Get
                    Return _selected
                End Get
                Set(ByVal value As Boolean)
                    _selected = value
                End Set
            End Property
            ''' <summary>
            ''' The bounding rectangle of the column in the ColumnHeaderControl.
            ''' </summary>
            Public Property Bounds() As Rectangle
                Get
                    Return _rect
                End Get
                Set(ByVal value As Rectangle)
                    _rect = value
                End Set
            End Property
            ''' <summary>
            ''' Determine the width of the host based on the column's attributes.
            ''' </summary>
            Public Property Width() As Integer
                Get
                    Return _rect.Width
                End Get
                Set(ByVal value As Integer)
                    If _rect.Width <> value Then
                        If value >= MinResize Then
                            _rect.Width = value
                        Else
                            _rect.Width = MinResize
                        End If
                    End If
                End Set
            End Property
            ''' <summary>
            ''' Determine the height of the host based on the ColumnHeaderControl's height.
            ''' </summary>
            Public Property Height() As Integer
                Get
                    Return _rect.Height
                End Get
                Set(ByVal value As Integer)
                    _rect.Height = value
                End Set
            End Property
            ''' <summary>
            ''' Determine x location of the host in the ColumnHeaderControl.
            ''' </summary>
            Public Property X() As Integer
                Get
                    Return _rect.X
                End Get
                Set(ByVal value As Integer)
                    _rect.X = value
                End Set
            End Property
            ''' <summary>
            ''' Gets the rightmost location of the host.
            ''' </summary>
            Public ReadOnly Property Right() As Integer
                Get
                    Return _rect.Right
                End Get
            End Property
            ''' <summary>
            ''' Gets the leftmost location of the host.
            ''' </summary>
            Public ReadOnly Property Left() As Integer
                Get
                    Return _rect.Left
                End Get
            End Property
            ''' <summary>
            ''' Gets a value indicating the column has been pressed.
            ''' </summary>
            Public ReadOnly Property OnMouseDown() As Boolean
                Get
                    Return _onMouseDown
                End Get
            End Property
            ''' <summary>
            ''' Gets a value indicating the split column has been pressed.
            ''' </summary>
            Public ReadOnly Property OnMouseDownSplit() As Boolean
                Get
                    Return _onMouseDownSplit
                End Get
            End Property
            ''' <summary>
            ''' Gets a value indicating the mouse pointer moved over the column.
            ''' </summary>
            Public ReadOnly Property OnHover() As Boolean
                Get
                    Return _onHover
                End Get
            End Property
            ''' <summary>
            ''' Gets a value indicating the mouse pointer moved over the column's split.
            ''' </summary>
            Public ReadOnly Property OnHoverSplit() As Boolean
                Get
                    Return _onHoverSplit
                End Get
            End Property
            ''' <summary>
            ''' Gets a ColumnHeader object contained within the host.
            ''' </summary>
            Public ReadOnly Property Column() As ColumnHeader
                Get
                    Return _column
                End Get
            End Property
            ''' <summary>
            ''' Minimum size allowed for this column.
            ''' </summary>
            Public ReadOnly Property MinResize() As Integer
                Get
                    Dim minWidth As Integer = 10
                    If _column.Image IsNot Nothing Then minWidth = minWidth + 18
                    If _column.EnableFiltering And _column.SortOrder <> SortOrder.None Then minWidth = minWidth + 10
                    If _column.EnableFiltering Then minWidth = minWidth + 10
                    If minWidth < 25 Then minWidth = 25
                    Return minWidth
                End Get
            End Property
            ''' <summary>
            ''' Maximum width allowed for this column.
            ''' </summary>
            Public ReadOnly Property MaxResize() As Integer
                Get
                    Dim maxWidth As Integer = _owner.Width - (_rect.Left + 5)
                    If _owner._owner._vScroll.Visible Then
                        maxWidth = maxWidth - _owner._owner._vScroll.Width
                    Else
                        If _owner._owner._showColumnOptions Then maxWidth = maxWidth - 10
                    End If
                    Return maxWidth
                End Get
            End Property
            ''' <summary>
            ''' Gets the filter handle contained in the host.
            ''' </summary>
            Public ReadOnly Property FilterHandler() As ColumnFilterHandle
                Get
                    Return _filterHandler
                End Get
            End Property
        End Class
        ''' <summary>
        ''' Control to display header options in the popup window.
        ''' </summary>
        ''' <remarks>This control contains two sections, first for column visibility, and second for column frozen.</remarks>
        Private Class OptionControl
            Inherits Windows.Forms.Control
            ''' <summary>
            ''' Host to control item options operations.
            ''' </summary>
            Private Class ItemHost
                Dim _rect As Rectangle
                Dim _column As ColumnHeader
                Dim _checked As Boolean = False
                Dim _displayFor As Integer = 0
                Dim _onHover As Boolean = False
                Dim _owner As OptionControl
                Public Sub New(ByVal column As ColumnHeader, ByVal displayFor As Integer, ByVal owner As OptionControl)
                    _column = column
                    _displayFor = displayFor
                    _owner = owner
                    If _displayFor = 0 Then
                        _checked = _column.Visible
                    Else
                        _checked = _column.Frozen
                    End If
                End Sub
                ''' <summary>
                ''' Determine x location of the host.
                ''' </summary>
                Public Property X() As Integer
                    Get
                        Return _rect.X
                    End Get
                    Set(ByVal value As Integer)
                        _rect.X = value
                    End Set
                End Property
                ''' <summary>
                ''' Determine y location of the host.
                ''' </summary>
                Public Property Y() As Integer
                    Get
                        Return _rect.Y
                    End Get
                    Set(ByVal value As Integer)
                        _rect.Y = value
                    End Set
                End Property
                ''' <summary>
                ''' Determine the size of the host.
                ''' </summary>
                Public Property Size() As Size
                    Get
                        Return _rect.Size
                    End Get
                    Set(ByVal value As Size)
                        _rect.Size = value
                    End Set
                End Property
                ''' <summary>
                ''' Determine the bounding rectangle of the host.
                ''' </summary>
                Public Property Bounds() As Rectangle
                    Get
                        Return _rect
                    End Get
                    Set(ByVal value As Rectangle)
                        _rect = value
                    End Set
                End Property
                ''' <summary>
                ''' Determine checked state of the host.
                ''' </summary>
                Public Property Checked() As Boolean
                    Get
                        Return _checked
                    End Get
                    Set(ByVal value As Boolean)
                        _checked = value
                    End Set
                End Property
                ''' <summary>
                ''' Gets a ColumnHeader object contrined within the host.
                ''' </summary>
                Public ReadOnly Property Column() As ColumnHeader
                    Get
                        Return _column
                    End Get
                End Property
                ''' <summary>
                ''' Gets left location of the host.
                ''' </summary>
                Public ReadOnly Property Left() As Integer
                    Get
                        Return _rect.X
                    End Get
                End Property
                ''' <summary>
                ''' Gets top location of the host.
                ''' </summary>
                Public ReadOnly Property Top() As Integer
                    Get
                        Return _rect.Y
                    End Get
                End Property
                ''' <summary>
                ''' Gets the rightmost location of the host.
                ''' </summary>
                Public ReadOnly Property Right() As Integer
                    Get
                        Return _rect.Right
                    End Get
                End Property
                ''' <summary>
                ''' Gets the bottommost location of the host.
                ''' </summary>
                Public ReadOnly Property Bottom() As Integer
                    Get
                        Return _rect.Bottom
                    End Get
                End Property
                ''' <summary>
                ''' Gets a display functions of the host.
                ''' </summary>
                ''' <remarks>Returns 0 for column visibility, and 1 for column frozen.</remarks>
                Public ReadOnly Property DisplayFor() As Integer
                    Get
                        Return _displayFor
                    End Get
                End Property
                ''' <summary>
                ''' Test whether the mouse pointer is moved over the host.
                ''' </summary>
                Public Function mouseMove(ByVal e As MouseEventArgs) As Boolean
                    If _rect.Contains(e.Location) Then
                        If Not _onHover Then
                            _onHover = True
                            _owner._hoverHost = Me
                            Return True
                        End If
                    Else
                        If _onHover Then
                            _owner._hoverHost = Nothing
                            _onHover = False
                            Return True
                        End If
                    End If
                    Return False
                End Function
                ''' <summary>
                ''' Test whether the mouse left button is pressed over the host.
                ''' </summary>
                Public Function mouseDown() As Boolean
                    If _onHover Then
                        _checked = Not _checked
                        Return True
                    End If
                    Return False
                End Function
                ''' <summary>
                ''' Test whether the mouse pointer is leave the host.
                ''' </summary>
                Public Function mouseLeave() As Boolean
                    If _onHover Then
                        _onHover = False
                        _owner._hoverHost = Nothing
                        Return True
                    End If
                    Return False
                End Function
                ''' <summary>
                ''' Draw the on the specified graphics object.
                ''' </summary>
                ''' <param name="g">Graphics object where the host must be drawn.</param>
                Public Sub draw(ByVal g As Graphics)
                    Dim chkRect As Rectangle
                    Dim txtRect As Rectangle
                    chkRect = New Rectangle(_rect.X, _rect.Y, 22, _rect.Height)
                    txtRect = New Rectangle(_rect.X + 22, _rect.Y, _rect.Width - 22, _rect.Height)
                    If _onHover Then
                        Renderer.Button.draw(g, _rect, , 2, , , , True)
                    End If
                    If _checked Then
                        Renderer.CheckBox.drawCheck(g, chkRect, CheckState.Checked)
                    End If
                    g.DrawString(_column.Text, _owner.Font, Renderer.Drawing.NormalTextBrush, _
                        txtRect, _owner.txtFormat)
                End Sub
            End Class
            Dim txtFormat As StringFormat = New StringFormat
            ' Visibility
            Dim _txtVisibilityRect As Rectangle = New Rectangle(0, 0, 0, 0)
            Dim _chkVisibilityRect As Rectangle = New Rectangle(0, 0, 0, 0)
            Dim _chkVisibilityState As CheckState
            Dim _chkVisibilityHover As Boolean = False
            Dim _itemsVisibility As List(Of ItemHost) = New List(Of ItemHost)
            Dim _vscVisibility As VScrollBar = New VScrollBar
            ' Freeze
            Dim _txtFreezeRect As Rectangle = New Rectangle(0, 0, 0, 0)
            Dim _chkFreezeRect As Rectangle = New Rectangle(0, 0, 0, 0)
            Dim _chkFreezeState As CheckState
            Dim _chkFreezeHover As Boolean = False
            Dim _itemsFreeze As List(Of ItemHost) = New List(Of ItemHost)
            Dim _vscFreeze As VScrollBar = New VScrollBar
            ' Button OK
            Dim _btnRect As Rectangle
            Dim _btnHover As Boolean = False
            ' Owner
            Dim _owner As ColumnHeaderControl
            ' Hover host
            Dim _hoverHost As ItemHost = Nothing
            Public Sub New(ByVal owner As ColumnHeaderControl, ByVal font As Font)
                _owner = owner
                Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
                Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
                Me.SetStyle(ControlStyles.ResizeRedraw, True)
                Me.Font = font
                ' Setting up ...
                Dim maxWidth As Integer = 0
                Dim itemWidth As Integer = 0
                Dim iHost As ItemHost
                ' Populating hosts and gather maximum width
                For Each ch As ColumnHost In _owner._columnHosts
                    itemWidth = 0
                    If ch.Column.EnableHidden Then
                        iHost = New ItemHost(ch.Column, 0, Me)
                        _itemsVisibility.Add(iHost)
                        itemWidth = TextRenderer.MeasureText(ch.Column.Text, Me.Font).Width + 5
                    End If
                    If ch.Column.EnableFrozen Then
                        iHost = New ItemHost(ch.Column, 1, Me)
                        _itemsFreeze.Add(iHost)
                        itemWidth = TextRenderer.MeasureText(ch.Column.Text, Me.Font).Width + 5
                    End If
                    If maxWidth < itemWidth Then maxWidth = itemWidth
                Next
                maxWidth = maxWidth + 22
                _vscVisibility.Visible = False
                _vscFreeze.Visible = False
                If _itemsVisibility.Count + _itemsFreeze.Count > 20 Then
                    _vscVisibility.Visible = _itemsVisibility.Count > 10
                    If _vscVisibility.Visible Then _vscVisibility.Maximum = _itemsVisibility.Count - 10
                    _vscFreeze.Visible = _itemsFreeze.Count > 10
                    If _vscFreeze.Visible Then _vscFreeze.Maximum = _itemsFreeze.Count - 10
                End If
                If _vscVisibility.Visible Or _vscFreeze.Visible Then
                    If maxWidth + _vscVisibility.Width + 4 < 110 Then maxWidth = 110 - (_vscVisibility.Width + 4)
                Else
                    If maxWidth < 110 Then maxWidth = 110
                End If
                _vscVisibility.Left = maxWidth + 4
                _vscFreeze.Left = maxWidth + 4
                ' Hosts size and x location
                For Each ih As ItemHost In _itemsVisibility
                    ih.Size = New Size(maxWidth, 20)
                    ih.X = 2
                Next
                For Each ih As ItemHost In _itemsFreeze
                    ih.Size = New Size(maxWidth, 20)
                    ih.X = 2
                Next
                ' y location
                Dim y As Integer = 0
                If _itemsVisibility.Count > 0 Then
                    Dim i As Integer, max As Integer
                    _chkVisibilityRect = New Rectangle(0, 0, 22, 22)
                    _txtVisibilityRect = New Rectangle(22, 0, TextRenderer.MeasureText("Visible", Me.Font).Width + 5, 22)
                    y = 23
                    _vscVisibility.Top = y
                    i = 0
                    max = IIf(_vscVisibility.Visible, 10, _itemsVisibility.Count)
                    While i < max
                        _itemsVisibility(i).Y = y
                        y = _itemsVisibility(i).Bottom
                        i = i + 1
                    End While
                    _vscVisibility.Height = max * 20
                    y = y + 1
                End If
                If _itemsFreeze.Count > 0 Then
                    Dim i As Integer, max As Integer
                    _chkFreezeRect = New Rectangle(0, y, 22, 22)
                    _txtFreezeRect = New Rectangle(22, y, TextRenderer.MeasureText("Freeze", Me.Font).Width + 5, 22)
                    y = y + 23
                    _vscFreeze.Top = y
                    i = 0
                    max = IIf(_vscFreeze.Visible, 10, _itemsFreeze.Count)
                    While i < max
                        _itemsFreeze(i).Y = y
                        y = _itemsFreeze(i).Bottom
                        i = i + 1
                    End While
                    _vscFreeze.Height = max * 20
                    y = y + 1
                End If
                If _vscVisibility.Visible Or _vscFreeze.Visible Then
                    Me.Width = _vscVisibility.Right
                Else
                    Me.Width = maxWidth + 4
                End If
                Me.Height = y + 28
                Me.Controls.Add(_vscVisibility)
                Me.Controls.Add(_vscFreeze)
                _btnRect = New Rectangle(Me.Width - 80, y + 3, 75, 21)
                checkCheckedState()
                AddHandler _vscVisibility.ValueChanged, AddressOf vScroll_ValueChanged
                AddHandler _vscFreeze.ValueChanged, AddressOf vScroll_ValueChanged
                txtFormat.Alignment = StringAlignment.Near
                txtFormat.LineAlignment = StringAlignment.Center
                Me.Invalidate()
            End Sub
            Private Sub vScroll_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
                Dim vsc As VScrollBar = DirectCast(sender, VScrollBar)
                Dim y As Integer = vsc.Top
                Dim i As Integer = 0
                If sender Is _vscVisibility Then
                    While i < 10
                        _itemsVisibility(i + vsc.Value).Y = y
                        y = _itemsVisibility(i + vsc.Value).Bottom
                        i = i + 1
                    End While
                Else
                    While i < 10
                        _itemsFreeze(i + vsc.Value).Y = y
                        y = _itemsFreeze(i + vsc.Value).Bottom
                        i = i + 1
                    End While
                End If
                Me.Invalidate()
            End Sub
            ''' <summary>
            ''' Change the check state of each section.
            ''' </summary>
            Private Sub checkCheckedState()
                Dim chkCount As Integer = 0
                For Each ih As ItemHost In _itemsVisibility
                    If ih.Checked Then chkCount = chkCount + 1
                Next
                If chkCount = 0 Then
                    _chkVisibilityState = CheckState.Unchecked
                ElseIf chkCount = _itemsVisibility.Count Then
                    _chkVisibilityState = CheckState.Checked
                Else
                    _chkVisibilityState = CheckState.Indeterminate
                End If
                chkCount = 0
                For Each ih As ItemHost In _itemsFreeze
                    If ih.Checked Then chkCount = chkCount + 1
                Next
                If chkCount = 0 Then
                    _chkFreezeState = CheckState.Unchecked
                ElseIf chkCount = _itemsFreeze.Count Then
                    _chkFreezeState = CheckState.Checked
                Else
                    _chkFreezeState = CheckState.Indeterminate
                End If
            End Sub
            Private Sub OptionControl_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
                If e.Button = Windows.Forms.MouseButtons.Left Then
                    Dim changed As Boolean = False
                    Dim i As Integer
                    If _chkVisibilityHover Then
                        If _chkVisibilityState = CheckState.Indeterminate Or _
                            _chkVisibilityState = CheckState.Unchecked Then
                            _chkVisibilityState = CheckState.Checked
                            For Each ih As ItemHost In _itemsVisibility
                                ih.Checked = True
                            Next
                        Else
                            _chkVisibilityState = CheckState.Unchecked
                            For Each ih As ItemHost In _itemsVisibility
                                ih.Checked = False
                            Next
                        End If
                        changed = True
                    End If
                    If Not changed Then
                        If _vscVisibility.Visible Then
                            i = 0
                            While i < 10
                                changed = changed Or _itemsVisibility(i + _vscVisibility.Visible).mouseDown
                                i = i + 1
                            End While
                        Else
                            For Each ih As ItemHost In _itemsVisibility
                                changed = changed Or ih.mouseDown
                            Next
                        End If
                    End If
                    If Not changed Then
                        If _chkFreezeHover Then
                            If _chkFreezeState = CheckState.Indeterminate Or _
                                _chkFreezeState = CheckState.Unchecked Then
                                _chkFreezeState = CheckState.Checked
                                For Each ih As ItemHost In _itemsFreeze
                                    ih.Checked = True
                                Next
                            Else
                                _chkFreezeState = CheckState.Unchecked
                                For Each ih As ItemHost In _itemsFreeze
                                    ih.Checked = False
                                Next
                            End If
                            changed = True
                        End If
                    End If
                    If Not changed Then
                        If _vscFreeze.Visible Then
                            i = 0
                            While i < 10
                                changed = changed Or _itemsFreeze(i + _vscFreeze.Visible).mouseDown
                                i = i + 1
                            End While
                        Else
                            For Each ih As ItemHost In _itemsFreeze
                                changed = changed Or ih.mouseDown
                            Next
                        End If
                    End If
                    checkCheckedState()
                    If Not changed Then
                        If _btnHover Then
                            ' Applying all changes and close the popup window.
                            For Each ih As ItemHost In _itemsVisibility
                                ih.Column.Visible = ih.Checked
                            Next
                            For Each ih As ItemHost In _itemsFreeze
                                ih.Column.Frozen = ih.Checked
                            Next
                            _owner._toolStrip.Close()
                            Return
                        End If
                    End If
                    If changed Then Me.Invalidate()
                End If
            End Sub
            Private Sub OptionControl_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
                Dim changed As Boolean = False
                If _chkVisibilityHover Then
                    _chkVisibilityHover = False
                    changed = True
                End If
                If _chkFreezeHover Then
                    _chkFreezeHover = False
                    changed = True
                End If
                If _btnHover Then
                    _btnHover = False
                    changed = True
                End If
                For Each ih As ItemHost In _itemsVisibility
                    changed = changed Or ih.mouseLeave
                Next
                For Each ih As ItemHost In _itemsFreeze
                    changed = changed Or ih.mouseLeave
                Next
                If changed Then Me.Invalidate()
            End Sub
            Private Sub OptionControl_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
                Dim changed As Boolean = False
                Dim i As Integer
                If _chkVisibilityRect.Contains(e.Location) Then
                    _chkVisibilityHover = True
                    changed = True
                Else
                    If _chkVisibilityHover Then
                        _chkVisibilityHover = False
                        changed = True
                    End If
                End If
                If _chkFreezeRect.Contains(e.Location) Then
                    _chkFreezeHover = True
                    changed = True
                Else
                    If _chkFreezeHover Then
                        _chkFreezeHover = False
                        changed = True
                    End If
                End If
                If _btnRect.Contains(e.Location) Then
                    _btnHover = True
                    changed = True
                Else
                    If _btnHover Then
                        _btnHover = False
                        changed = True
                    End If
                End If
                If _vscVisibility.Visible Then
                    i = 0
                    While i < 10
                        changed = changed Or _itemsVisibility(i + _vscVisibility.Visible).mouseMove(e)
                        i = i + 1
                    End While
                Else
                    For Each ih As ItemHost In _itemsVisibility
                        changed = changed Or ih.mouseMove(e)
                    Next
                End If
                If _vscFreeze.Visible Then
                    i = 0
                    While i < 10
                        changed = changed Or _itemsFreeze(i + _vscFreeze.Visible).mouseMove(e)
                        i = i + 1
                    End While
                Else
                    For Each ih As ItemHost In _itemsFreeze
                        changed = changed Or ih.mouseMove(e)
                    Next
                End If
                If changed Then Me.Invalidate()
            End Sub
            Private Sub OptionControl_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
                If _hoverHost Is Nothing Then Return
                If _hoverHost.DisplayFor = 0 Then
                    If _vscVisibility.Visible Then
                        If e.Delta < 0 Then
                            If _vscVisibility.Value > 0 Then _vscVisibility.Value -= 1
                        Else
                            If _vscVisibility.Value < _vscVisibility.Maximum Then _vscVisibility.Value += 1
                        End If
                    End If
                Else
                    If _vscFreeze.Visible Then
                        If e.Delta < 0 Then
                            If _vscFreeze.Value > 0 Then _vscFreeze.Value -= 1
                        Else
                            If _vscFreeze.Value < _vscFreeze.Maximum Then _vscFreeze.Value += 1
                        End If
                    End If
                End If
            End Sub
            Private Sub OptionControl_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
                Dim btnFormat As StringFormat = New StringFormat
                Dim i As Integer = 0
                btnFormat.Alignment = StringAlignment.Center
                btnFormat.LineAlignment = StringAlignment.Center
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
                e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                ' Check area
                e.Graphics.Clear(Renderer.Popup.BackgroundBrush.Color)
                e.Graphics.FillRectangle(Renderer.Popup.PlacementBrush, _
                    New Rectangle(0, 0, 22, Me.Height))
                e.Graphics.DrawLine(Renderer.Popup.SeparatorPen, 22, 0, 22, Me.Height)
                If _itemsVisibility.Count > 0 Then
                    e.Graphics.FillRectangle(Renderer.Popup.SeparatorBrush, _
                        New Rectangle(0, _chkVisibilityRect.Y, Me.Width, _chkVisibilityRect.Height))
                    e.Graphics.DrawLine(Renderer.Popup.SeparatorPen, 0, _chkVisibilityRect.Bottom, Me.Width, _chkVisibilityRect.Bottom)
                    Renderer.CheckBox.drawCheckBox(e.Graphics, _chkVisibilityRect, _
                        _chkVisibilityState, , , _chkVisibilityHover)
                    e.Graphics.DrawString("Visible", Me.Font, Renderer.Drawing.NormalTextBrush, _
                        _txtVisibilityRect, txtFormat)
                    If _vscVisibility.Visible Then
                        While i < 10
                            _itemsVisibility(i + _vscVisibility.Value).draw(e.Graphics)
                            i = i + 1
                        End While
                    Else
                        For Each ih As ItemHost In _itemsVisibility
                            ih.draw(e.Graphics)
                        Next
                    End If
                End If
                If _itemsFreeze.Count > 0 Then
                    e.Graphics.FillRectangle(Renderer.Popup.SeparatorBrush, _
                        New Rectangle(0, _chkFreezeRect.Y, Me.Width, _chkFreezeRect.Height))
                    e.Graphics.DrawLine(Renderer.Popup.SeparatorPen, 0, _chkFreezeRect.Bottom, Me.Width, _chkFreezeRect.Bottom)
                    Renderer.CheckBox.drawCheckBox(e.Graphics, _chkFreezeRect, _
                        _chkFreezeState, , , _chkFreezeHover)
                    e.Graphics.DrawString("Freeze", Me.Font, Renderer.Drawing.NormalTextBrush, _
                        _txtFreezeRect, txtFormat)
                    If _vscFreeze.Visible Then
                        i = 0
                        While i < 10
                            _itemsFreeze(i + _vscFreeze.Value).draw(e.Graphics)
                            i = i + 1
                        End While
                    Else
                        For Each ih As ItemHost In _itemsFreeze
                            ih.draw(e.Graphics)
                        Next
                    End If
                End If
                e.Graphics.FillRectangle(Renderer.Popup.BackgroundBrush, _
                    New Rectangle(0, _btnRect.Y - 2, Me.Width, Me.Height - (_btnRect.Y - 2)))
                e.Graphics.DrawLine(Renderer.Popup.SeparatorPen, 0, _
                    _btnRect.Y - 2, Me.Width, _btnRect.Y - 2)
                Renderer.Button.draw(e.Graphics, _btnRect, , , , , , _btnHover)
                e.Graphics.DrawString("OK", Me.Font, Renderer.Drawing.NormalTextBrush, _btnRect, btnFormat)
                btnFormat.Dispose()
            End Sub
        End Class
        ' CheckBox control
        Dim _chkRect As Rectangle
        Dim _chkHover As Boolean = False
        Dim _chkState As Windows.Forms.CheckState = Windows.Forms.CheckState.Unchecked
        ' Internal use
        Dim _owner As MultiColumnTree
        Dim _columnHosts As List(Of ColumnHost)
        Dim _selectedHost As ColumnHost = Nothing
        Dim WithEvents _toolStrip As ToolStripDropDown
        Dim WithEvents _toolTip As ToolTip
        Dim _frozenRight As Integer = 0
        Dim _shownChooser As FilterChooser = Nothing
        ' ToolTip
        Dim _currentToolTip As String = ""
        Dim _currentToolTipTitle As String = ""
        Dim _currentToolTipImage As Image = Nothing
        Dim _currentToolTipRect As Rectangle
        ' Column resize
        Dim _onColumnResize As Boolean = False
        Dim _resizeHost As ColumnHost = Nothing
        Dim _resizeStartX As Integer = -1
        Dim _resizeCurrentX As Integer = -1
        ' Column reorder
        Dim _reorderHost As ColumnHost = Nothing
        Dim _reorderTarget As ColumnHost = Nothing
        Dim _reorderStart As Point, _reorderCurrent As Point
        Dim _onColumnReorder As Boolean = False
        ' Column option button
        Dim _optRect As Rectangle
        Dim _optHover As Boolean = False
        Dim _optOnDown As Boolean = False
        ' Resume painting
        Dim _resumePainting As Boolean = True
        <Description("Occurs when the column order has been changed.")> _
        Public Event ColumnOrderChanged(ByVal sender As Object, ByVal e As ColumnEventArgs)
        <Description("Occurs when the column filter has been changed.")> _
        Public Event AfterColumnFilter(ByVal sender As Object, ByVal e As ColumnEventArgs)
        <Description("Occurs when the column custom filter is choosen.")> _
        Public Event AfterColumnCustomFilter(ByVal sender As Object, ByVal e As ColumnEventArgs)
        <Description("Occurs when the CheckBox checked status has been changed.")> _
        Public Event CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        <Description("Occurs when the ColumnHeader width has been changed.")> _
        Public Event AfterColumnResize(ByVal sender As Object, ByVal e As ColumnEventArgs)
        Public Sub New(ByVal owner As MultiColumnTree, ByVal font As Font)
            _owner = owner
            _columnHosts = New List(Of ColumnHost)
            Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            Me.SetStyle(ControlStyles.ResizeRedraw, True)
            Me.Dock = DockStyle.Top
            Me.Font = font
            Me.Height = Me.Font.Height + 8
            _chkRect = New Rectangle(3, CInt((Me.Height - 16) / 2), 14, 14)
            _toolTip = New ToolTip(Me)
            _toolTip.AnimationSpeed = 20
            _toolStrip = New ToolStripDropDown(Me)
            _toolStrip.SizingGrip = ToolStripDropDown.SizingGripMode.BottomRight
        End Sub
        ''' <summary>
        ''' Create a new host for new ColumnHeader an added it to the collection.
        ''' </summary>
        ''' <param name="aColumn">A ColumnHeader object to be added.</param>
        Public Sub addHost(ByVal aColumn As ColumnHeader)
            Dim aHost As ColumnHost = New ColumnHost(aColumn, Me)
            _columnHosts.Add(aHost)
            aColumn._displayIndex = _columnHosts.IndexOf(aHost)
            relocateHosts()
        End Sub
        ''' <summary>
        ''' Remove a host from collection based on the column contained.
        ''' </summary>
        ''' <param name="aColumn">A ColumnHeader object contained within the host.</param>
        Public Sub removeHost(ByVal aColumn As ColumnHeader)
            Dim i As Integer = 0
            If _selectedHost.Column Is aColumn Then _selectedHost = Nothing
            While i < _columnHosts.Count
                If _columnHosts(i).Column Is aColumn Then
                    _columnHosts.RemoveAt(i)
                Else
                    i = i + 1
                End If
            End While
            For Each ch As ColumnHost In _columnHosts
                ch.Column._displayIndex = _columnHosts.IndexOf(ch)
            Next
            relocateHosts()
        End Sub
        ''' <summary>
        ''' Clear all existing hosts.
        ''' </summary>
        Public Sub clearHosts()
            _columnHosts.Clear()
            _selectedHost = Nothing
        End Sub
        ''' <summary>
        ''' Initialize the bounding rectangle for each column host.
        ''' </summary>
        ''' <param name="startX">Optional, determine the starting x location of the host of unfrozen columns..</param>
        Public Sub relocateHosts(Optional ByVal startX As Integer = 0)
            Dim availWidth As Integer = Me.Width - (_owner._vScroll.Width + 5)
            Dim spaceLeft As Integer = 1
            Dim frozenWidth As Integer = 0
            If _owner._checkBoxes Then
                spaceLeft += 22
                availWidth -= 22
            End If
            If availWidth > 0 Then ' No need to create if no space available.
                ' Create rectangle for frozen columns first.
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column.Frozen And ch.Column.Visible Then
                        Select Case ch.Column.SizeType
                            Case ColumnSizeType.Fixed
                                ch.Width = ch.Column.Width
                            Case ColumnSizeType.Auto
                                ch.Width = _owner.getSubItemMaxWidth(ch.Column)
                            Case ColumnSizeType.Percentage
                                ch.Width = ch.Column.Width * availWidth / 100
                        End Select
                        ch.Height = Me.Height
                        frozenWidth = ch.Width + 2
                    End If
                Next
                availWidth -= frozenWidth
                ' Create reactangle for the rest of the visible columns.
                Dim fillSizeCount As Integer = 0
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column.Visible And Not ch.Column.Frozen Then
                        Select Case ch.Column.SizeType
                            Case ColumnSizeType.Fixed
                                ch.Width = ch.Column.Width
                            Case ColumnSizeType.Auto
                                ch.Width = _owner.getSubItemMaxWidth(ch.Column)
                            Case ColumnSizeType.Fill
                                fillSizeCount = fillSizeCount + 1
                            Case ColumnSizeType.Percentage
                                ch.Width = ch.Column.Width * availWidth / 100
                        End Select
                        ch.Height = Me.Height
                        If ch.Column.SizeType <> ColumnSizeType.Fill Then availWidth = availWidth - (ch.Width + 2)
                    End If
                Next
                If fillSizeCount > 0 Then
                    For Each ch As ColumnHost In _columnHosts
                        If ch.Column.Visible And ch.Column.SizeType = ColumnSizeType.Fill Then _
                            ch.Width = (availWidth + (2 * fillSizeCount)) / fillSizeCount
                    Next
                End If
            End If
            _optRect = New Rectangle(Me.Right - 10, 0, 10, Me.Height)
            moveColumns(startX)
        End Sub
        ''' <summary>
        ''' Move x location of each host of unfrozen columns.
        ''' </summary>
        Public Sub moveColumns(ByVal xScroll As Integer)
            Dim startX As Integer = 1
            If _owner._checkBoxes Then
                startX += 22
            End If
            ' Move frezed columns
            For Each ch As ColumnHost In _columnHosts
                If ch.Column.Frozen And ch.Column.Visible Then
                    ch.X = startX
                    startX = ch.Right + 2
                End If
            Next
            _frozenRight = startX
            ' Move the rest of the columns
            startX = _frozenRight
            startX = startX + xScroll
            For Each ch As ColumnHost In _columnHosts
                If ch.Column.Visible And Not ch.Column.Frozen Then
                    ch.X = startX
                    startX = ch.Right + 2
                End If
            Next
        End Sub
        ''' <summary>
        ''' Determine the check state of the checkbox displayed in the ColumnHeaderControl.
        ''' </summary>
        ''' <remarks>This checkbox represent the Checked state of all the items in ListView.</remarks>
        Public Property CheckState() As Windows.Forms.CheckState
            Get
                Return _chkState
            End Get
            Set(ByVal value As Windows.Forms.CheckState)
                If _chkState <> value Then
                    _chkState = value
                    If _owner._checkBoxes Then Me.Invalidate()
                End If
            End Set
        End Property
        ''' <summary>
        ''' Gets a value indicating column resize operation is performed.
        ''' </summary>
        <Description("Gets a value indicating column resize operation is performed.")> _
        Public ReadOnly Property OnColumnResize() As Boolean
            Get
                Return _onColumnResize
            End Get
        End Property
        ''' <summary>
        ''' Gets current column resize position.
        ''' </summary>
        <Description("Gets current column resize position.")> _
        Public ReadOnly Property ResizeCurrentX() As Integer
            Get
                Return _resizeCurrentX
            End Get
        End Property
        ''' <summary>
        ''' Gets total width of the visible columns.
        ''' </summary>
        <Description("Gets total width of the visible columns.")> _
        Public ReadOnly Property ColumnsWidth() As Integer
            Get
                Dim result As Integer = 0
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column.Visible Then result = result + ch.Width + 2
                Next
                Return result
            End Get
        End Property
        ''' <summary>
        ''' Gets a ColumnHeader specified by its displayed index, and Frozen property is ignored.
        ''' </summary>
        ''' <param name="index">Displayed index of a ColumnHeader object.</param>
        ''' <remarks>The displayed index and the actual display of the columns can be different.
        ''' The frozen columns will be shown first.</remarks>
        <Description("Gets a ColumnHeader specified by its displayed index, and Frozen property is ignored.  " & _
            "However, Frozen columns is displayed first than the others.")> _
        Public ReadOnly Property ColumnAt(ByVal index As Integer) As ColumnHeader
            Get
                If index >= 0 And index < _columnHosts.Count Then Return _columnHosts(index).Column
                Return Nothing
            End Get
        End Property
        ''' <summary>
        ''' Gets the bounding rectangle of a ColumnHeader.
        ''' </summary>
        ''' <param name="column">A ColumnHeader object whose the bounding rectangle you want to return.</param>
        <Description("Gets the bounding rectangle of a ColumnHeader.")> _
        Public ReadOnly Property ColumnRectangle(ByVal column As ColumnHeader) As Rectangle
            Get
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column Is column Then Return ch.Bounds
                Next
                Return New Rectangle(0, 0, 0, 0)
            End Get
        End Property
        ''' <summary>
        ''' Gets total width of all unfrozen and visible columns.
        ''' </summary>
        <Description("Gets total width of all unfrozen and visible columns.")> _
        Public ReadOnly Property UnFrozenWidth() As Integer
            Get
                Dim result As Integer = 0
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column.Visible And Not ch.Column.Frozen Then result = result + ch.Width + 2
                Next
                Return result
            End Get
        End Property
        ''' <summary>
        ''' Gets total width of all frozen and visible columns, and reserved area.
        ''' </summary>
        <Description("Gets total width of all frozen and visible columns, and reserved area.")> _
        Public ReadOnly Property FrozenWidth() As Integer
            Get
                Dim result As Integer = 0
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column.Visible And ch.Column.Frozen Then result = result + ch.Width + 2
                Next
                Return result
            End Get
        End Property
        ''' <summary>
        ''' Gets all frozen and visible columns in the collection, sorted by its display index.
        ''' </summary>
        <Description("Gets all frozen and visible columns in the collection, sorted by its display index.")> _
        Public ReadOnly Property FrozenColumns() As List(Of ColumnHeader)
            Get
                Dim result As List(Of ColumnHeader) = New List(Of ColumnHeader)
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column.Visible And ch.Column.Frozen Then result.Add(ch.Column)
                Next
                Return result
            End Get
        End Property
        ''' <summary>
        ''' Gets all unfrozen and visible columns in the collection, sorted by its display index.
        ''' </summary>
        <Description("Gets all unfrozen and visible columns in the collection, sorted by its display index.")> _
        Public ReadOnly Property UnFrozenColumns() As List(Of ColumnHeader)
            Get
                Dim result As List(Of ColumnHeader) = New List(Of ColumnHeader)
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column.Visible And Not ch.Column.Frozen Then result.Add(ch.Column)
                Next
                Return result
            End Get
        End Property
        ''' <summary>
        ''' Gets an area used in the ColumnHeaderControl to display all visible columns.
        ''' </summary>
        <Description("Gets an area used in the ColumnHeaderControl to display all visible columns.")> _
        Public ReadOnly Property DisplayedRectangle() As Rectangle
            Get
                Dim leftMost As Integer = 0
                Dim rightMost As Integer = 0
                Dim first As Boolean = True
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column.Visible Then
                        If first Then
                            leftMost = ch.Bounds.X
                            first = False
                        Else
                            If leftMost > ch.Bounds.X Then leftMost = ch.Bounds.X
                        End If
                        If rightMost < ch.Bounds.Right + 2 Then rightMost = ch.Bounds.Right + 2
                    End If
                Next
                Return New Rectangle(leftMost, 0, rightMost - leftMost, Me.Height)
            End Get
        End Property
        ''' <summary>
        ''' Gets an area used in the ColumnHeaderControl to display all frozen and visible columns.
        ''' </summary>
        <Description("Gets an area used in the ColumnHeaderControl to display all frozen and visible columns.")> _
        Public ReadOnly Property FrozenRectangle() As Rectangle
            Get
                Dim leftMost As Integer = -1
                Dim rightMost As Integer = 0
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column.Visible And ch.Column.Frozen Then
                        If leftMost = -1 Then
                            leftMost = ch.Bounds.X
                        Else
                            If leftMost > ch.Bounds.X Then leftMost = ch.Bounds.X
                        End If
                        If rightMost < ch.Bounds.Right + 2 Then rightMost = ch.Bounds.Right + 2
                    End If
                Next
                Return New Rectangle(leftMost, 0, rightMost - leftMost, Me.Height)
            End Get
        End Property
        ''' <summary>
        ''' Gets an area used in the ColumnHeaderControl to display all visible and unfrozen columns.
        ''' </summary>
        <Description("Gets an area used in the ColumnHeaderControl to display all visible and unfrozen columns.")> _
        Public ReadOnly Property UnFrozenRectangle() As Rectangle
            Get
                Dim leftMost As Integer = 0
                Dim rightMost As Integer = 0
                Dim first As Boolean = True
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column.Visible And Not ch.Column.Frozen Then
                        If first Then
                            leftMost = ch.Bounds.X
                            first = False
                        Else
                            If leftMost > ch.Bounds.X Then leftMost = ch.Bounds.X
                        End If
                        If rightMost < ch.Bounds.Right + 2 Then rightMost = ch.Bounds.Right + 2
                    End If
                Next
                Return New Rectangle(leftMost, 0, rightMost - leftMost, Me.Height)
            End Get
        End Property
        ''' <summary>
        ''' Gets a list of ColumnFilterHandle objects of all ColumnHeader, sorted by ColumnHeader in ListView columns.
        ''' </summary>
        Public ReadOnly Property FilterHandlers() As List(Of ColumnFilterHandle)
            Get
                Dim result As List(Of ColumnFilterHandle) = New List(Of ColumnFilterHandle)
                For Each col As ColumnHeader In _owner._columns
                    For Each ch As ColumnHost In _columnHosts
                        If ch.Column Is col Then
                            result.Add(ch.FilterHandler)
                            Exit For
                        End If
                    Next
                Next
                Return result
            End Get
        End Property
        ''' <summary>
        ''' Gets a ColumnFilterHandle object specified by its ColumnHeader.
        ''' </summary>
        <Description("Gets a ColumnFilterHandle object specified by its ColumnHeader.")> _
        Public ReadOnly Property FilterHandler(ByVal column As ColumnHeader) As ColumnFilterHandle
            Get
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column Is column Then Return ch.FilterHandler
                Next
                Return Nothing
            End Get
        End Property
        ' Filter column related
        ''' <summary>
        ''' Reload filter parameters of a ColumnFilterHandle on specified ColumnHeader using a list of all parameter.
        ''' </summary>
        Public Sub reloadFilter(ByVal columnIndex As Integer, ByVal objs As List(Of Object))
            For Each ch As ColumnHost In _columnHosts
                If _owner.Columns.IndexOf(ch.Column) = columnIndex Then
                    ch.FilterHandler.reloadFilter(objs)
                    Exit For
                End If
            Next
        End Sub
        ''' <summary>
        ''' Add a filter parameter to a ColumnFilterHandle on specified ColumnHeader.
        ''' </summary>
        Public Sub addFilter(ByVal columnIndex As Integer, ByVal obj As Object)
            For Each ch As ColumnHost In _columnHosts
                If _owner.Columns.IndexOf(ch.Column) = columnIndex Then
                    ch.FilterHandler.addFilter(obj)
                    Exit For
                End If
            Next
        End Sub
        ''' <summary>
        ''' Clear all filter parameters on all columns.
        ''' </summary>
        Public Sub clearFilters()
            For Each ch As ColumnHost In _columnHosts
                ch.FilterHandler.Items.Clear()
            Next
        End Sub
        ''' <summary>
        ''' Gets displayed index of the specified column.
        ''' </summary>
        Public Function getDisplayedIndex(ByVal column As ColumnHeader) As Integer
            If column Is Nothing Then Return -1
            Dim index As Integer = 0
            ' Search on frozen columns first.
            For Each colHost As ColumnHost In _columnHosts
                If colHost.Column.Visible And colHost.Column.Frozen Then
                    If colHost.Column Is column Then Return index
                    index += 1
                End If
            Next
            ' Search on unfrozen columns.
            For Each colHost As ColumnHost In _columnHosts
                If colHost.Column.Visible And Not colHost.Column.Frozen Then
                    If colHost.Column Is column Then Return index
                    index += 1
                End If
            Next
            Return -1
        End Function
        ''' <summary>
        ''' Swap the position of two ColumnHost in collection.
        ''' </summary>
        Private Sub swapHost(ByVal host1 As ColumnHost, ByVal host2 As ColumnHost)
            Dim tmpHost As ColumnHost = host1
            Dim tmpX As Integer
            host1 = host2
            host2 = tmpHost
            tmpX = host1.X
            host1.X = host2.X
            host2.X = tmpX
            host1.Column._displayIndex = _columnHosts.IndexOf(host1)
            host2.Column._displayIndex = _columnHosts.IndexOf(host2)
        End Sub
        ''' <summary>
        ''' Show filter options window in a popup window.
        ''' </summary>
        Private Sub showFilterPopup(ByVal chooser As FilterChooser, ByVal dropdownRect As Rectangle)
            Dim anHost As ToolStripControlHost
            Dim scrLoc As Point
            chooser.Width = 200
            anHost = New ToolStripControlHost(chooser)
            anHost.Padding = New Padding(0)
            _toolStrip.Items.Clear()
            _toolStrip.Items.Add(anHost)
            scrLoc = Me.PointToScreen(New Point(dropdownRect.Right - 200, Me.Height + 2))
            If scrLoc.X < 0 Then scrLoc = Me.PointToScreen(New Point(dropdownRect.X, Me.Height + 2))
            If scrLoc.Y + chooser.Height + 5 > Screen.PrimaryScreen.WorkingArea.Height Then scrLoc.Y = scrLoc.Y - (chooser.Height + 5 + Me.Height)
            _resumePainting = False
            ' Painting the column control before the dropdown window is shown.
            Dim pe As PaintEventArgs
            pe = New PaintEventArgs(Me.CreateGraphics, New Rectangle(0, 0, Me.Width, Me.Height))
            Me.InvokePaint(Me, pe)
            pe.Dispose()
            _shownChooser = chooser
            _toolStrip.Show(scrLoc)
        End Sub
        Private Sub ColumnHeaderControl_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
            _toolTip.hide()
            If Me.Cursor = Cursors.VSplit Then
                If _resizeHost IsNot Nothing Then
                    Dim maxWidth As Integer = _owner.getSubItemMaxWidth(_resizeHost.Column)
                    If maxWidth < _resizeHost.MinResize Then maxWidth = _resizeHost.MinResize
                    _resizeHost.Column.SizeType = ColumnSizeType.Fixed
                    _resizeHost.Column.Width = maxWidth
                End If
            End If
        End Sub
        Private Sub ColumnHeaderControl_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.EnabledChanged
            Me.Invalidate()
        End Sub
        Private Sub ColumnHeaderControl_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
            _toolTip.hide()
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If _optHover Then
                    _optOnDown = True
                    Me.Invalidate()
                    _resumePainting = False
                    ' Show columns options
                    Dim optCtrl As OptionControl = New OptionControl(Me, Renderer.ToolTip.TextFont)
                    Dim anHost As ToolStripControlHost = New ToolStripControlHost(optCtrl)
                    Dim scrPoint As Point = Me.PointToScreen(New Point(_optRect.X, _optRect.Bottom + 1))
                    If scrPoint.X + optCtrl.Width + 6 > Screen.PrimaryScreen.WorkingArea.Width Then _
                        scrPoint.X = scrPoint.X - (optCtrl.Width - 4)
                    If scrPoint.Y + optCtrl.Height + 6 > Screen.PrimaryScreen.WorkingArea.Height Then _
                        scrPoint.Y = scrPoint.Y - (optCtrl.Height + Me.Height + 8)
                    _toolStrip.Items.Clear()
                    _toolStrip.Items.Add(anHost)
                    _toolStrip.Show(scrPoint)
                    Return
                End If
                If _chkHover Then
                    If _chkState = Windows.Forms.CheckState.Indeterminate Or _chkState = Windows.Forms.CheckState.Unchecked Then
                        _chkState = Windows.Forms.CheckState.Checked
                    Else
                        _chkState = Windows.Forms.CheckState.Unchecked
                    End If
                    If _resumePainting Then Me.Invalidate()
                    RaiseEvent CheckedChanged(Me, New EventArgs)
                    Return
                End If
                If Me.Cursor = Cursors.VSplit Then
                    ' Start column resizing operation
                    _onColumnResize = True
                    _resizeStartX = e.X
                    _resizeCurrentX = e.X
                    If _resumePainting Then _owner.Invalidate(True)
                    Return
                End If
                If _reorderHost IsNot Nothing Then
                    ' Start column reordering operation
                    _onColumnReorder = True
                    _reorderStart = e.Location
                    _reorderCurrent = e.Location
                    _reorderHost.mouseDown()
                    If _resumePainting Then Me.Invalidate()
                    Return
                End If
                Dim changed As Boolean = False
                For Each columnHost As ColumnHost In _columnHosts
                    If columnHost.Column.Visible Then changed = changed Or columnHost.mouseDown
                Next
            End If
        End Sub
        Private Sub ColumnHeaderControl_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
            Dim changed As Boolean = False
            _toolTip.hide()
            If _optHover Then
                _optHover = False
                changed = True
            End If
            If _chkHover Then
                _chkHover = False
                changed = True
            End If
            For Each ch As ColumnHost In _columnHosts
                If ch.Column.Visible Then changed = changed Or ch.mouseLeave
            Next
            If Not _onColumnResize Then
                Me.Cursor = Cursors.Default
                _resizeHost = Nothing
            End If
            If changed And _resumePainting Then Me.Invalidate()
        End Sub
        Private Sub ColumnHeaderControl_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
            If e.Button = Windows.Forms.MouseButtons.None Then
                Dim changed As Boolean = False
                Me.Cursor = Cursors.Default
                _resizeHost = Nothing
                _reorderHost = Nothing
                If _owner._showColumnOptions Then
                    If _optRect.Contains(e.Location) Then
                        If Not _optHover Then
                            _optHover = True
                            _currentToolTip = "Show column options."
                            _currentToolTipRect = _optRect
                            changed = True
                        End If
                    Else
                        If _optHover Then
                            _optHover = False
                            changed = True
                        End If
                    End If
                End If
                If _owner._checkBoxes Then
                    If _chkRect.Contains(e.Location) Then
                        If Not _chkHover Then
                            _chkHover = True
                            If _chkState = CheckState.Unchecked Or _chkState = CheckState.Indeterminate Then
                                _currentToolTip = "Check all items."
                            Else
                                _currentToolTip = "Uncheck all items."
                            End If
                            _currentToolTipRect = _chkRect
                            changed = True
                        End If
                    Else
                        If _chkHover Then
                            _chkHover = False
                            changed = True
                        End If
                    End If
                End If
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column.Visible Then changed = changed Or ch.mouseMove(e)
                    If ch.OnHover And Not ch.OnHoverSplit Then
                        If Not ch.Column.Frozen Then _reorderHost = ch
                    End If
                Next
                _reorderTarget = _reorderHost
                Dim rectColResize As Rectangle
                rectColResize.Size = New Size(2, Me.Height)
                rectColResize.Y = 0
                _onColumnResize = False
                For Each ch As ColumnHost In _columnHosts
                    If ch.Column.Visible Then
                        rectColResize.X = ch.Right
                        If rectColResize.Contains(e.Location) Then
                            Me.Cursor = Cursors.VSplit
                            _resizeHost = ch
                            Exit For
                        End If
                    End If
                Next
                If changed And Renderer.ToolTip.containsToolTip(_currentToolTipTitle, _
                    _currentToolTip, _currentToolTipImage) Then
                    _toolTip.show(Me, _currentToolTipRect)
                Else
                    If changed Then _toolTip.hide()
                End If
                If changed And _resumePainting Then Me.Invalidate()
            ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
                If Me.Cursor = Cursors.VSplit Then
                    ' Performing resize operation.
                    Dim changed As Boolean = False
                    For Each ch As ColumnHost In _columnHosts
                        If ch.Column.Visible Then changed = changed Or ch.mouseMove(e)
                    Next
                    Dim dx As Integer = e.X - _resizeStartX
                    Dim lastX As Integer = _resizeCurrentX
                    If _resizeHost.Width + dx > _resizeHost.MinResize And _
                        _resizeHost.Width + dx < _resizeHost.MaxResize Then
                        _resizeCurrentX = e.X
                    Else
                        If _resizeHost.Width + dx < _resizeHost.MinResize Then
                            _resizeCurrentX = _resizeStartX - (_resizeHost.Width - _resizeHost.MinResize)
                        Else
                            _resizeCurrentX = _resizeStartX + (_resizeHost.MaxResize - _resizeHost.Width)
                        End If
                    End If
                    changed = changed Or (lastX - _resizeCurrentX <> 0)
                    If _resumePainting And changed Then _owner.Invalidate(True)
                ElseIf _onColumnReorder Then
                    ' Performing column reorder operation.
                    _reorderTarget = Nothing
                    Dim rBound As Rectangle
                    For Each ch As ColumnHost In _columnHosts
                        If ch.Column.Visible Then ch.mouseMove(e)
                        rBound = ch.Bounds
                        rBound.Width = rBound.Width + 2
                        If rBound.Contains(e.Location) Then _reorderTarget = ch
                    Next
                    If _reorderTarget Is Nothing Then
                        Dim mPoint As Point = New Point(e.X, 10)
                        Dim leftHost As ColumnHost = Nothing
                        Dim rightHost As ColumnHost = Nothing
                        For Each ch As ColumnHost In _columnHosts
                            If ch.Column.Visible Then
                                If Not ch.Column.Frozen Then
                                    If leftHost Is Nothing Then leftHost = ch
                                    rightHost = ch
                                End If
                                rBound = ch.Bounds
                                rBound.Width = rBound.Width + 2
                                If rBound.Contains(mPoint) Then
                                    _reorderTarget = ch
                                    Exit For
                                End If
                            End If
                        Next
                        If _reorderTarget Is Nothing Then
                            If e.X < leftHost.X Then _reorderTarget = leftHost
                            If e.X > rightHost.Right Then _reorderTarget = rightHost
                        End If
                    End If
                    _reorderCurrent = e.Location
                    If _resumePainting Then Me.Invalidate()
                End If
            End If
        End Sub
        Private Sub ColumnHeaderControl_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
            If _onColumnResize Then
                ' End of resize operation
                Dim dx As Integer = _resizeCurrentX - _resizeStartX
                _resizeHost.Column.Width = _resizeHost.Bounds.Width + dx
                _resizeHost.Column.SizeType = ColumnSizeType.Fixed
                Me.Cursor = Cursors.Default
                _onColumnResize = False
                If _resumePainting Then _owner.Invalidate(True)
                RaiseEvent AfterColumnResize(Me, New ColumnEventArgs(_resizeHost.Column))
                _resizeHost = Nothing
                Return
            End If
            If _onColumnReorder Then
                ' End of reorder operation
                Dim colOrderChanged As Boolean = False
                _onColumnReorder = False
                If _reorderStart <> _reorderCurrent Then
                    If _reorderHost IsNot _reorderTarget Then
                        If Not _reorderTarget.Column.Frozen Then
                            swapHost(_reorderHost, _reorderTarget)
                            colOrderChanged = True
                        End If
                    End If
                Else
                    If _reorderHost.Column.EnableSorting Then
                        Select Case _reorderHost.Column.SortOrder
                            Case SortOrder.Descending, SortOrder.None
                                _reorderHost.Column.SortOrder = SortOrder.Ascending
                            Case SortOrder.Ascending
                                _reorderHost.Column.SortOrder = SortOrder.Descending
                        End Select
                    End If
                End If
                Me.Invalidate()
                If colOrderChanged Then RaiseEvent ColumnOrderChanged(Me, New ColumnEventArgs(_reorderHost.Column))
                _reorderHost = Nothing
                _reorderTarget = Nothing
                Return
            End If
            Dim changed As Boolean = False
            For Each ch As ColumnHost In _columnHosts
                changed = changed Or ch.mouseUp
            Next
            If changed And _resumePainting Then Me.Invalidate()
        End Sub
        Private Sub ColumnHeaderControl_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
            Dim xSeparator As Integer = 0
            Dim rectHeader As Rectangle = New Rectangle(0, 0, Me.Width, Me.Height)
            Dim bgBrush As LinearGradientBrush = New LinearGradientBrush(rectHeader, _
                Color.Black, Color.White, LinearGradientMode.Vertical)
            bgBrush.InterpolationColors = Renderer.Column.NormalBlend
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
            e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
            e.Graphics.FillRectangle(bgBrush, rectHeader)
            bgBrush.Dispose()
            ' Draw unfrozen columns first
            For Each ch As ColumnHost In _columnHosts
                If ch.Column.Visible And Not ch.Column.Frozen Then
                    xSeparator = ch.Right
                    ch.draw(e.Graphics)
                    e.Graphics.DrawLine(Renderer.Column.NormalBorderPen, xSeparator, 4, xSeparator, Me.Height - 5)
                    e.Graphics.DrawLine(Pens.White, xSeparator + 1, 4, xSeparator + 1, Me.Height - 5)
                End If
            Next
            ' Draw frozen columns
            For Each ch As ColumnHost In _columnHosts
                If ch.Column.Visible And ch.Column.Frozen Then
                    xSeparator = ch.Right
                    ch.draw(e.Graphics)
                    e.Graphics.DrawLine(Renderer.Column.NormalBorderPen, xSeparator, 4, xSeparator, Me.Height - 5)
                    e.Graphics.DrawLine(Pens.White, xSeparator + 1, 4, xSeparator + 1, Me.Height - 5)
                End If
            Next
            If _owner._showColumnOptions Then
                Dim borderPen As Pen = Renderer.Column.NormalBorderPen
                Dim bgOpt As LinearGradientBrush = New LinearGradientBrush(_optRect, _
                    Color.Black, Color.White, LinearGradientMode.Vertical)
                If _optHover Then
                    If _optOnDown Then
                        bgOpt.InterpolationColors = Renderer.Column.PressedBlend
                    Else
                        bgOpt.InterpolationColors = Renderer.Column.HLitedBlend
                    End If
                    borderPen = Renderer.Column.ActiveBorderPen
                Else
                    bgOpt.InterpolationColors = Renderer.Column.NormalBlend
                End If
                e.Graphics.FillRectangle(bgOpt, _optRect)
                bgOpt.Dispose()
                e.Graphics.DrawLine(borderPen, _optRect.X, 0, _optRect.X, Me.Height)
                e.Graphics.DrawLine(Pens.White, _optRect.X + 1, 0, _optRect.X + 1, Me.Height)
                Renderer.Drawing.drawTriangle(e.Graphics, _optRect, _
                    IIf(Me.Enabled, Color.FromArgb(21, 66, 139), Drawing.Color.Gray), _
                    Color.White, Renderer.Drawing.TriangleDirection.Down)
                borderPen.Dispose()
            End If
            If _owner._checkBoxes Then
                Dim reservedWidth As Integer = 0
                If _owner._checkBoxes Then reservedWidth += 22
                Dim chkArea As Rectangle = New Rectangle(0, 0, reservedWidth, Me.Height)
                Dim borderPen As Pen = Renderer.Column.NormalBorderPen
                Dim bgChk As LinearGradientBrush = New LinearGradientBrush(chkArea, _
                    Color.Black, Color.White, LinearGradientMode.Vertical)
                bgChk.InterpolationColors = Renderer.Column.NormalBlend
                e.Graphics.FillRectangle(bgChk, chkArea)
                bgChk.Dispose()
                e.Graphics.DrawLine(borderPen, chkArea.Right - 1, 0, chkArea.Right - 1, Me.Height)
                e.Graphics.DrawLine(Pens.White, chkArea.Right, 0, chkArea.Right, Me.Height)
                Renderer.CheckBox.drawCheckBox(e.Graphics, _chkRect, _chkState, , _
                    Me.Enabled, _chkHover)
                borderPen.Dispose()
            End If
            If _onColumnResize Then
                e.Graphics.DrawLine(Pens.Black, _resizeCurrentX, 0, _resizeCurrentX, Me.Height)
            End If
            If _onColumnReorder Then
                Dim rectMark As Rectangle = _reorderHost.Bounds
                rectMark.X = rectMark.X + (_reorderCurrent.X - _reorderStart.X)
                _reorderHost.drawMoved(e.Graphics, rectMark, _
                    IIf(_reorderTarget.Column.Frozen, False, True))
            End If
            e.Graphics.DrawRectangle(Renderer.Column.NormalBorderPen, 0, 0, Me.Width - 1, Me.Height - 1)
        End Sub
        Private Sub ColumnHeaderControl_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
            _optRect = New Rectangle(Me.Right - 10, 0, 10, Me.Height)
            relocateHosts()
        End Sub
        Private Sub _toolStrip_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles _toolStrip.Closed
            _optOnDown = False
            _resumePainting = True
            Me.Invalidate()
            If _shownChooser IsNot Nothing Then
                If _shownChooser.Result <> FilterChooserResult.Cancel Then
                    If _shownChooser.Result = FilterChooserResult.OK Then
                        RaiseEvent AfterColumnFilter(Me, New ColumnEventArgs(_shownChooser.FilterHandle.Column))
                    Else
                        RaiseEvent AfterColumnCustomFilter(Me, New ColumnEventArgs(_shownChooser.FilterHandle.Column))
                    End If
                End If
                _shownChooser.Dispose()
            End If
        End Sub
        Private Sub _toolTip_Draw(ByVal sender As Object, ByVal e As DrawEventArgs) Handles _tooltip.Draw
            Renderer.ToolTip.drawToolTip(_currentToolTipTitle, _currentToolTip, _
                _currentToolTipImage, e.Graphics, e.Rectangle)
            _currentToolTipTitle = ""
            _currentToolTip = ""
            _currentToolTipImage = Nothing
        End Sub
        Private Sub _toolTip_Popup(ByVal sender As Object, ByVal e As PopupEventArgs) Handles _tooltip.Popup
            e.Size = Renderer.ToolTip.measureSize(_currentToolTipTitle, _currentToolTip, _currentToolTipImage)
        End Sub
    End Class
    ''' <summary>
    ''' TextBox for label editing.
    ''' </summary>
    Private Class TextBoxLabelEditor
        Inherits TextBox
        Public Sub New()
            MyBase.New()
        End Sub
        Protected Overrides Function IsInputKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean
            Select Case keyData
                Case Keys.Escape
                    Return True
                Case Keys.Return
                    If Not Multiline Then Return True
                Case Keys.Control Or Keys.Return
                    If Multiline Then Return True
            End Select
            Return MyBase.IsInputKey(keyData)
        End Function
    End Class
    ''' <summary>
    ''' Class to host a TreeNode object and handles all of its operations, a visual representation on a MultiColumnTree.
    ''' </summary>
    <Description("Class to host a TreeNode object and handles all of its operations.")> _
    Private Class TreeNodeHost
        ''' <summary>
        ''' Class to host a TreeNodeSubItem, a visual representation of a TreeNodeSubItem.
        ''' </summary>
        <Description("Class to host a TreeNodeSubItem.")> _
        Private Class TreeNodeSubItemHost
            Dim _subItem As TreeNode.TreeNodeSubItem
            Dim _owner As TreeNodeHost
            Dim _displaySize As Size
            Dim _location As Point
            Dim _originalSize As SizeF
            Dim _displayedRect As Rectangle
            Dim _checkBoxRect As Rectangle = New Rectangle(0, 0, 14, 14)
            Dim _onHover As Boolean = False
            Dim _onHoverCheck As Boolean = False
            Dim _isDisplayed As Boolean = True
            Public Sub New(ByVal owner As TreeNodeHost, ByVal subitem As TreeNode.TreeNodeSubItem)
                _owner = owner
                _subItem = subitem
            End Sub
            ''' <summary>
            ''' Gets the display string of a TreeNodeSubItem value.
            ''' </summary>
            <Description("Gets the display string of a TreeNodeSubItem value.")> _
            Public Function getSubItemString() As String
                Dim index As Integer = _owner._node.SubItems.IndexOf(_subItem) + 1
                If index = 0 Then Return _owner._node.Text
                Return _owner._owner.getValueString(_subItem.Value, index)
            End Function
            ''' <summary>
            ''' Measure the original size of the TreeNodeSubItem value.
            ''' </summary>
            <Description("Measure the original size of the TreeNodeSubItem value.")> _
            Public Sub measureOriginal()
                Dim strSubitem As String = getSubItemString()
                Dim index As Integer = _owner._node.SubItems.IndexOf(_subItem) + 1
                Dim aColumn As ColumnHeader = _owner._owner._columns(index)
                If aColumn IsNot Nothing Then
                    Dim font As Font
                    If _owner._node.UseNodeStyleForSubItems Then
                        font = _owner._node.NodeFont
                    Else
                        font = _subItem.Font
                    End If
                    _originalSize = _owner._owner._gObj.MeasureString(strSubitem, font)
                    If _originalSize.Width = 0 Then _originalSize.Width = 5
                    If _originalSize.Height = 0 Then _originalSize.Height = font.Height
                    _originalSize.Width += _subItemMargin * 2
                    _originalSize.Width += 1
                    _originalSize.Height += _subItemMargin * 2
                    If index = 0 Then
                        If _owner._owner._showImages Then _originalSize.Width += 20
                        If _owner._owner._checkBoxes Then _originalSize.Width += 20
                    End If
                Else
                    _originalSize = New SizeF(0, 0)
                End If
            End Sub
            ''' <summary>
            ''' Measure the displayed size of the TreeNodeSubItem value.
            ''' </summary>
            <Description("Measure the displayed size of the TreeNodeSubItem value.")> _
            Public Sub measureDisplay()
                Dim strSubitem As String = getSubItemString()
                Dim index As Integer = _owner._node.SubItems.IndexOf(_subItem) + 1
                Dim aColumn As ColumnHeader = _owner._owner._columns(index)
                If aColumn IsNot Nothing Then
                    Dim colRect As Rectangle = _owner._owner._columnControl.ColumnRectangle(aColumn)
                    If _owner._owner._columnControl.getDisplayedIndex(aColumn) = 0 And _owner._owner._checkBoxes Then
                        colRect.X -= 22
                        colRect.Width += 22
                    End If
                    If index = 0 Then
                        _displaySize = New Size(_originalSize.Width, _originalSize.Height)
                        _displayedRect.X = colRect.X + (_owner._node.Level * _owner._owner._indent) + 12
                        _checkBoxRect.X = _displayedRect.X + 3
                        _owner._dropDownRect.X = _displayedRect.X - 12
                    Else
                        If colRect.Width <= _originalSize.Width Then
                            _displaySize = New Size(colRect.Width, _originalSize.Height)
                        Else
                            If aColumn.ColumnAlign <> HorizontalAlignment.Left Or _
                                aColumn.Format = ColumnFormat.Bar Then
                                _displaySize = New Size(colRect.Width, _originalSize.Height)
                            Else
                                _displaySize = New Size(_originalSize.Width, _originalSize.Height)
                            End If
                        End If
                        _displayedRect.X = colRect.X
                    End If
                    _displaySize.Height += 1
                    If Not _owner._owner._allowMultiline Then
                        If strSubitem.IndexOf(vbCr) > -1 Then
                            Dim font As Font = _subItem.Font
                            If _owner._node.UseNodeStyleForSubItems Then font = _owner.Node.NodeFont
                            _displaySize.Height = font.Height + (2 * _subItemMargin)
                        End If
                    End If
                    _displayedRect.Width = _displaySize.Width
                    _displayedRect.Height = _displaySize.Height
                    _isDisplayed = aColumn.Visible
                Else
                    _displaySize = New Size(0, 0)
                    _isDisplayed = False
                End If
            End Sub
            ''' <summary>
            ''' Move x location of displayed rectangle based on the related column.
            ''' </summary>
            <Description("Move x location of displayed rectangle based on the related column.")> _
            Public Sub moveX()
                Dim index As Integer = _owner._node.SubItems.IndexOf(_subItem) + 1
                Dim aColumn As ColumnHeader = _owner._owner._columns(index)
                If aColumn IsNot Nothing Then
                    Dim colRect As Rectangle = _owner._owner._columnControl.ColumnRectangle(aColumn)
                    If _owner._owner._columnControl.getDisplayedIndex(aColumn) = 0 And _owner._owner._checkBoxes Then
                        colRect.X -= 22
                        colRect.Width += 22
                    End If
                    If index > 0 Then
                        _displayedRect.X = colRect.X
                    Else
                        _displayedRect.X = colRect.X + (_owner._node.Level * _owner._owner._indent) + 12
                        _checkBoxRect.X = _displayedRect.X + 3
                        _owner._dropDownRect.X = _displayedRect.X - 12
                    End If
                End If
            End Sub
            ''' <summary>
            ''' Determine whether mouse pointer is hover on this TreeNodeSubItem.
            ''' </summary>
            <Description("Determine whether mouse pointer is hover on this TreeNodeSubItem.")> _
            Public Property OnHover() As Boolean
                Get
                    Return _onHover
                End Get
                Set(ByVal value As Boolean)
                    _onHover = value
                End Set
            End Property
            ''' <summary>
            ''' Determine whether mouse pointer is hover on the checkbox of this TreeNodeSubItem.
            ''' </summary>
            Public Property OnHoverCheck() As Boolean
                Get
                    Return _onHoverCheck
                End Get
                Set(ByVal value As Boolean)
                    _onHoverCheck = value
                End Set
            End Property
            ''' <summary>
            ''' Determine y location of the host.
            ''' </summary>
            Public Property Y() As Integer
                Get
                    Return _displayedRect.Y
                End Get
                Set(ByVal value As Integer)
                    _displayedRect.Y = value
                    _checkBoxRect.Y = value + ((_owner._size.Height - 14) / 2)
                End Set
            End Property
            ''' <summary>
            ''' Gets the original size of TreeNodeSubItem.
            ''' </summary>
            <Description("Gets the original size of TreeNodeSubItem.")> _
            Public ReadOnly Property OriginalSize() As SizeF
                Get
                    Return _originalSize
                End Get
            End Property
            ''' <summary>
            ''' Gets the displayed size of TreeNodeSubItem.
            ''' </summary>
            <Description("Gets the displayed size of TreeNodeSubItem.")> _
            Public ReadOnly Property DisplayedSize() As Size
                Get
                    Return _displaySize
                End Get
            End Property
            ''' <summary>
            ''' Gets the displayed rectangle of TreeNodeSubItem.
            ''' </summary>
            <Description("Gets the displayed rectangle of TreeNodeSubItem.")> _
            Public ReadOnly Property Bounds() As Rectangle
                Get
                    Return _displayedRect
                End Get
            End Property
            ''' <summary>
            ''' Draw TreeNodeSubItem object on specified graphics object, and optionally draw its background.
            ''' </summary>
            <Description("Draw TreeNodeSubItem object on specified graphics object, and optionally draw its background.")> _
            Public Sub draw(ByVal g As Graphics, Optional ByVal drawBackground As Boolean = False, Optional ByVal selected As Boolean = False)
                Dim strSubitem As String = getSubItemString()
                Dim index As Integer = _owner._node.SubItems.IndexOf(_subItem) + 1
                Dim aColumn As ColumnHeader = _owner._owner._columns(index)
                If aColumn Is Nothing Then Return
                If Not aColumn.Visible Then Return
                Dim rect As Rectangle = _owner._owner._columnControl.ColumnRectangle(aColumn)
                If _owner._owner._columnControl.getDisplayedIndex(aColumn) = 0 And _owner._owner._checkBoxes Then
                    rect.X -= 22
                    rect.Width += 22
                End If
                rect.Y = _displayedRect.Y
                rect.Height = _displayedRect.Height
                If rect.X > _owner._owner._clientArea.Right Or rect.Y > _owner._owner._clientArea.Bottom Then Return
                If rect.Right < _owner._owner._clientArea.X Or rect.Bottom < _owner._owner._clientArea.Y Then Return
                If index = 0 Then g.SetClip(rect)
                If drawBackground Then
                    If _onHover Or selected Then
                        Dim hoverBrush As LinearGradientBrush = New LinearGradientBrush(_displayedRect, _
                            Drawing.Color.Black, Drawing.Color.White, LinearGradientMode.Vertical)
                        Dim hoverPath As GraphicsPath = Renderer.Drawing.roundedRectangle(_displayedRect, _
                            2, 2, 2, 2)
                        Dim borderPen As Pen
                        If selected Then
                            If _onHover Then
                                borderPen = Renderer.ListItem.SelectedHLiteBorderPen
                                hoverBrush.InterpolationColors = Renderer.ListItem.SelectedHLiteBlend
                            Else
                                If _owner._owner.Focused Then
                                    borderPen = Renderer.ListItem.SelectedBorderPen
                                    hoverBrush.InterpolationColors = Renderer.ListItem.SelectedBlend
                                Else
                                    borderPen = Renderer.ListItem.SelectedBlurBorderPen
                                    hoverBrush.InterpolationColors = Renderer.ListItem.SelectedBlurBlend
                                End If
                            End If
                        Else
                            borderPen = Renderer.ListItem.HLitedBorderPen
                            hoverBrush.InterpolationColors = Renderer.ListItem.HLitedBlend
                        End If
                        g.FillPath(hoverBrush, hoverPath)
                        g.DrawPath(borderPen, hoverPath)
                        borderPen.Dispose()
                        hoverPath.Dispose()
                        hoverBrush.Dispose()
                    Else
                        If _owner._node.UseNodeStyleForSubItems Then
                            g.FillRectangle(New SolidBrush(_owner._node.BackColor), rect)
                        Else
                            g.FillRectangle(New SolidBrush(_subItem.BackColor), rect)
                        End If
                    End If
                End If
                Dim color As Drawing.Color = IIf(_owner._owner.Enabled, IIf(_owner._node.UseNodeStyleForSubItems, _owner._node.Color, _subItem.Color), Drawing.Color.Gray)
                Dim font As Font = IIf(_owner._node.UseNodeStyleForSubItems, _owner._node.NodeFont, _subItem.Font)
                Dim strFormat As StringFormat = New StringFormat
                Dim columnFormat As ColumnFormat = IIf(index = 0, Control.ColumnFormat.None, aColumn.Format)
                Dim rectValue As Rectangle = _displayedRect
                If index = 0 Then
                    If _owner._owner._checkBoxes Then
                        rectValue.X += 20
                        rectValue.Width -= 20
                        If _owner._onHover Or _owner._node.CheckState <> CheckState.Unchecked Or _owner._selected Then _
                            Renderer.CheckBox.drawCheckBox(g, _checkBoxRect, _owner._node.CheckState, , _
                                _owner._owner.Enabled, _onHoverCheck)
                    End If
                    If _owner._owner._showImages Then
                        Dim img As Image = Nothing
                        If _owner._node.IsExpanded Then img = _owner._node.ExpandedImage
                        If img Is Nothing Then img = _owner._node.Image
                        If img IsNot Nothing Then
                            Dim rectImg As Rectangle = New Rectangle(rectValue.Location, New Size(20, rectValue.Height))
                            rectImg = Renderer.Drawing.getImageRectangle(img, rectImg, 16)
                            If _owner._owner.Enabled Then
                                g.DrawImage(img, rectImg)
                            Else
                                Renderer.Drawing.grayscaledImage(img, rectImg, g)
                            End If
                        End If
                        rectValue.X += 20
                        rectValue.Width -= 20
                    End If
                    If _owner.getVisibleChildCount > 0 Then
                        If _owner._owner.Enabled Then
                            Renderer.Drawing.drawTriangle(g, _owner._dropDownRect, _
                                IIf(_owner._onHoverDropDown, Drawing.Color.Gold, Drawing.Color.Black), Drawing.Color.White, _
                                IIf(_owner._node.IsExpanded, Renderer.Drawing.TriangleDirection.DownRight, Renderer.Drawing.TriangleDirection.Right))
                        Else
                            Renderer.Drawing.drawTriangle(g, _owner._dropDownRect, _
                                Drawing.Color.Gray, Drawing.Color.White, _
                                IIf(_owner._node.IsExpanded, Renderer.Drawing.TriangleDirection.DownRight, Renderer.Drawing.TriangleDirection.Right))
                        End If
                    End If
                End If
                rectValue.X += _subItemMargin
                rectValue.Width -= _subItemMargin * 2
                strFormat.Trimming = StringTrimming.EllipsisCharacter
                strFormat.LineAlignment = StringAlignment.Center
                Select Case aColumn.ColumnAlign
                    Case HorizontalAlignment.Center
                        strFormat.Alignment = StringAlignment.Center
                    Case HorizontalAlignment.Left
                        strFormat.Alignment = StringAlignment.Near
                    Case HorizontalAlignment.Right
                        strFormat.Alignment = StringAlignment.Far
                End Select
                strFormat.FormatFlags = strFormat.FormatFlags Or StringFormatFlags.NoWrap
                If Not _owner._owner._allowMultiline Then _
                    strFormat.FormatFlags = strFormat.FormatFlags Or StringFormatFlags.LineLimit
                If columnFormat = Control.ColumnFormat.Bar Then _
                    _owner.drawBar(g, New Rectangle(rectValue.X, rectValue.Y + 2, rectValue.Width, rectValue.Height - 4), _subItem, aColumn)
                If columnFormat <> Control.ColumnFormat.Bar Or _subItem.PrintValueOnBar Then _
                    g.DrawString(strSubitem, font, New SolidBrush(color), rectValue, strFormat)
                strFormat.Dispose()
                If index = 0 Then g.ResetClip()
            End Sub
            ' Mouse event
            ''' <summary>
            ''' Test whether the mouse pointer moves over the host.
            ''' </summary>
            <Description("Test whether the mouse pointer moves over the host.")> _
            Public Function mouseMove(ByVal e As MouseEventArgs) As Boolean
                If Not _isDisplayed Then Return False
                If _displayedRect.Contains(e.Location) And _
                    e.X > _owner._owner._clientArea.X And e.Y > _owner._owner._clientArea.Y Then
                    Dim stateChanged As Boolean = False
                    If Not _onHover Then
                        _onHover = True
                        stateChanged = True
                    End If
                    If _owner._owner._checkBoxes Then
                        If _owner._subItemHosts.IndexOf(Me) = 0 Then
                            If _checkBoxRect.Contains(e.Location) Then
                                If Not _onHoverCheck Then
                                    _onHoverCheck = True
                                    stateChanged = True
                                End If
                            Else
                                If _onHoverCheck Then
                                    _onHoverCheck = False
                                    stateChanged = True
                                End If
                            End If
                        End If
                    End If
                    Return stateChanged
                Else
                    If _onHover Or _onHoverCheck Then
                        _onHover = False
                        _onHoverCheck = False
                        Return True
                    End If
                End If
                Return False
            End Function
            ''' <summary>
            ''' Test whether the mouse pointer leaving the host.
            ''' </summary>
            Public Function mouseLeave() As Boolean
                If _onHover Or _onHoverCheck Then
                    _onHover = False
                    _onHoverCheck = False
                    Return True
                End If
                Return False
            End Function
            ' Tooltip
            ''' <summary>
            ''' Determine whether a tooltip need to be shown on a TreeNodeSubItem object.
            ''' </summary>
            Public Function needToolTip() As Boolean
                Dim index As Integer = _owner._node.SubItems.IndexOf(_subItem) + 1
                If index > 0 Then Return False
                Dim aColumn As ColumnHeader = _owner._owner._columns(index)
                If aColumn IsNot Nothing Then
                    Dim colRect As Rectangle = _owner._owner._columnControl.ColumnRectangle(aColumn)
                    If _owner._owner._columnControl.getDisplayedIndex(aColumn) = 0 And _owner._owner._checkBoxes Then
                        colRect.X -= 22
                        colRect.Width += 22
                    End If
                    Dim subItemRect As Rectangle = Bounds
                    Return subItemRect.Right > colRect.Right Or Math.Floor(_originalSize.Height) > _displaySize.Height
                End If
                Return False
            End Function
            ''' <summary>
            ''' Gets an area where tooltip must be avoid.
            ''' </summary>
            Public Function toolTipRect() As Rectangle
                Dim rect As Rectangle = _displayedRect
                If _owner._owner._showImages Then
                    rect.X += 20
                    rect.Width -= 20
                End If
                If _owner._owner._checkBoxes Then
                    rect.X += 20
                    rect.Width -= 20
                End If
                Return rect
            End Function
        End Class
        Dim _owner As MultiColumnTree
        Dim _node As TreeNode
        Dim _location As Point
        Dim _size As Size
        Dim _onMouseDown As Boolean = False
        Dim _selected As Boolean = False
        Dim _onHover As Boolean = False
        Dim _visible As Boolean = True
        ' Checkbox
        Dim _onHoverDropDown As Boolean = False
        Dim _dropDownRect As Rectangle = New Rectangle(0, 0, 10, 10)
        ' SubItem hosts
        Dim _subItemHosts As List(Of TreeNodeSubItemHost) = New List(Of TreeNodeSubItemHost)
        ' Child nodes
        Dim _childSize As Size
        Dim _childHosts As List(Of TreeNodeHost) = New List(Of TreeNodeHost)
        Dim _parentHost As TreeNodeHost = Nothing
        Public Sub New(ByVal node As TreeNode, ByVal owner As MultiColumnTree, ByVal parentHost As TreeNodeHost)
            _owner = owner
            _node = node
            _parentHost = parentHost
            ' Adding item text and all its subitems to filter parameters
            Dim i As Integer = 0
            While i <= _node.SubItems.Count And i < _owner._columns.Count
                _owner._columnControl.addFilter(i, _node.SubItems(i).Value)
                i += 1
            End While
            If _node.Checked Then _owner._checkedNodes.Add(_node)
            refreshSubItem()
            refreshChildrenHosts()
            AddHandler _node.NodeAdded, AddressOf node_NodeAdded
            AddHandler _node.NodeRemoved, AddressOf node_NodeRemoved
            AddHandler _node.NodesOnClear, AddressOf node_NodeRemoved
        End Sub
        ' Child nodes
        ''' <summary>
        ''' Refresh TreeNodeHost contained in this host.
        ''' </summary>
        Public Sub refreshChildrenHosts()
            _childHosts.Clear()
            For Each tn As TreeNode In _node.Nodes
                Dim tnHost As TreeNodeHost = New TreeNodeHost(tn, _owner, Me)
                _childHosts.Add(tnHost)
            Next
        End Sub
        ''' <summary>
        ''' Relocate all TreeNodeHost object contained in this host.
        ''' </summary>
        Public Sub relocateChildrenHosts()
            Dim y As Integer = _location.Y + _size.Height
            For Each tnHost As TreeNodeHost In _childHosts
                tnHost.Y = y
                If tnHost.Visible Then
                    If tnHost._node.IsExpanded Then tnHost.relocateChildrenHosts()
                    y = tnHost.Bottom
                End If
            Next
        End Sub
        ''' <summary>
        ''' Sort the children host contained in this host.
        ''' </summary>
        Public Sub sortChildren(Optional ByVal relocateChild As Boolean = True)
            _childHosts.Sort(AddressOf _owner.nodeHostComparer)
            For Each tnHost As TreeNodeHost In _childHosts
                tnHost.sortChildren(relocateChild)
            Next
            If relocateChild Then relocateChildrenHosts()
        End Sub
        ''' <summary>
        ''' Filter the children host contained in this host, based on specified filter parameters.
        ''' </summary>
        Public Sub filterChildren(ByVal handlers As List(Of ColumnFilterHandle), Optional ByVal relocateChild As Boolean = True)
            For Each tnHost As TreeNodeHost In _childHosts
                tnHost.Visible = _owner.filterNode(tnHost.Node, handlers)
                If tnHost.Visible Then tnHost.filterChildren(handlers, relocateChild)
            Next
            If relocateChild Then
                measureChildren()
                relocateChildrenHosts()
            End If
        End Sub
        ''' <summary>
        ''' Filter the children host contained in this host.
        ''' </summary>
        Public Sub filterChildren(Optional ByVal relocateChild As Boolean = True)
            Dim handlers As List(Of ColumnFilterHandle) = _owner._columnControl.FilterHandlers
            filterChildren(handlers, relocateChild)
        End Sub
        ''' <summary>
        ''' Removes the event handlers of the node and all of the children.
        ''' </summary>
        Public Sub removeHandlers()
            RemoveHandler _node.NodeAdded, AddressOf node_NodeAdded
            RemoveHandler _node.NodeRemoved, AddressOf node_NodeRemoved
            RemoveHandler _node.NodesOnClear, AddressOf node_NodesOnClear
            If _node.Checked Then _owner._checkedNodes.Remove(_node)
            For Each tnHost As TreeNodeHost In _childHosts
                tnHost.removeHandlers()
            Next
        End Sub
        ''' <summary>
        ''' Collect all available TreeNodeSubItem values contained in node.
        ''' </summary>
        Public Sub collectSubItemValue(ByVal index As Integer, ByVal values As List(Of Object))
            If index >= 0 And index <= _node.SubItems.Count Then
                Dim subValue As Object = _node.SubItems(index).Value
                If subValue IsNot Nothing Then
                    If Not values.Contains(subValue) Then values.Add(subValue)
                End If
            End If
            For Each tnHost As TreeNodeHost In _childHosts
                tnHost.collectSubItemValue(index, values)
            Next
        End Sub
        ''' <summary>
        ''' Gets a TreeNodeHost object that contains specified node.
        ''' </summary>
        Public Function getHost(ByVal node As TreeNode) As TreeNodeHost
            If node Is _node Then Return Me
            Dim foundHost As TreeNodeHost = Nothing
            For Each tnHost As TreeNodeHost In _childHosts
                foundHost = tnHost.getHost(node)
                If foundHost IsNot Nothing Then Return foundHost
            Next
            Return Nothing
        End Function
        ''' <summary>
        ''' Gets the number of visible child of a TreeNode.
        ''' </summary>
        Public Function getVisibleChildCount() As Integer
            Dim visibleCount As Integer = 0
            For Each tnHost As TreeNodeHost In _childHosts
                If tnHost.Visible Then visibleCount += 1
            Next
            Return visibleCount
        End Function
        ''' <summary>
        ''' Gets previous TreeNodeHost child of a TreeNodeHost, starting from specified TreeNodeHost.
        ''' </summary>
        Public Function getPrevHost(ByVal fromHost As TreeNodeHost) As TreeNodeHost
            Dim hostIndex As Integer = _childHosts.IndexOf(fromHost)
            If hostIndex = 0 Then
                Return Me
            Else
                hostIndex -= 1
                While hostIndex >= 0
                    If _childHosts(hostIndex).Visible Then Return _childHosts(hostIndex)
                    hostIndex += 1
                End While
            End If
            Return Nothing
        End Function
        ''' <summary>
        ''' Gets the next TreeNodeHost child of a TreeNodeHost, starting from specified TreeNodeHost.
        ''' </summary>
        Public Function getNextHost(ByVal from As TreeNodeHost) As TreeNodeHost
            If getVisibleChildCount() = 0 Then Return Nothing
            Dim hostIndex As Integer = _childHosts.IndexOf(from)
            If from Is Me Then hostIndex = 0
            If hostIndex = -1 Then Return Nothing
            While hostIndex < _childHosts.Count
                If _childHosts(hostIndex).Visible Then Return _childHosts(hostIndex)
                hostIndex += 1
            End While
            Return Nothing
        End Function
        ''' <summary>
        ''' Gets the next TreeNodeHost child of a TreeNodeHost, starting from specified TreeNodeHost, and NodeText is started with specified character.
        ''' </summary>
        Public Function getNextHost(ByVal from As TreeNodeHost, ByVal startsWith As String, _
            Optional ByVal startNextNode As Boolean = True) As TreeNodeHost
            If getVisibleChildCount() = 0 Then Return Nothing
            Dim hostIndex As Integer = _childHosts.IndexOf(from)
            If from Is Me Then
                hostIndex = 0
                If Not startNextNode Then
                    ' Checking own node
                    If _node.Text.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase) Then Return Me
                    If getVisibleChildCount() = 0 Or Not _node.IsExpanded Then Return Nothing
                End If
            End If
            While hostIndex < _childHosts.Count
                If _childHosts(hostIndex).Visible Then
                    If _childHosts(hostIndex)._node.Text.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase) Then
                        Return _childHosts(hostIndex)
                    Else
                        If _childHosts(hostIndex).getVisibleChildCount > 0 And _childHosts(hostIndex)._node.IsExpanded Then
                            Dim result As TreeNodeHost = Nothing
                            result = _childHosts(hostIndex).getNextHost(_childHosts(hostIndex), startsWith)
                            If result IsNot Nothing Then Return result
                        End If
                    End If
                End If
                hostIndex += 1
            End While
            Return Nothing
        End Function
        ''' <summary>
        ''' Gets the last visible of the children host of a TreeNodeHost.
        ''' </summary>
        Public Function getLastVisibleNode() As TreeNodeHost
            If _node.IsExpanded And getVisibleChildCount() > 0 Then
                Dim i As Integer = _childHosts.Count - 1
                While i >= 0
                    If _childHosts(i).Visible Then Return _childHosts(i).getLastVisibleNode
                    i -= 1
                End While
            End If
            Return Me
        End Function
        ' Parent node and host.
        ''' <summary>
        ''' Gets the top level of the parent.
        ''' </summary>
        Public Function getTopParentHost() As TreeNodeHost
            If _parentHost Is Nothing Then Return Me
            Dim pHost As TreeNodeHost = _parentHost
            While pHost IsNot Nothing
                If pHost.ParentHost Is Nothing Then Return pHost
                pHost = pHost.ParentHost
            End While
            Return Nothing
        End Function
        ''' <summary>
        ''' Gets a value indicating whether node containing within the host is descendant from specified node.
        ''' </summary>
        Public Function isDescendantFrom(ByVal node As TreeNode) As Boolean
            Dim pNode As TreeNode = _node.Parent
            While pNode IsNot Nothing
                If pNode Is node Then Return True
                pNode = pNode.Parent
            End While
            Return False
        End Function
        ''' <summary>
        ''' Gets a TreeNodeHost that contains one of the parental node.
        ''' </summary>
        Public Function getParentHost(ByVal node As TreeNode) As TreeNodeHost
            Dim pHost As TreeNodeHost = Nothing
            While pHost IsNot Nothing
                If pHost.Node Is node Then Return pHost
                pHost = pHost.ParentHost
            End While
            Return Nothing
        End Function
        ''' <summary>
        ''' Expand all parent in node hierarchy.
        ''' </summary>
        Public Sub expandAllParent()
            Dim pNode As TreeNode = _node.Parent
            While pNode IsNot Nothing
                pNode.expand()
                pNode = pNode.Parent
            End While
        End Sub
        ' Sub items
        ''' <summary>
        ''' Refresh TreeNodeSubItemHost contained in this host.
        ''' </summary>
        <Description("Refresh TreeNodeSubItemHost contained in this host.")> _
        Public Sub refreshSubItem()
            _subItemHosts.Clear()
            Dim i As Integer = 0
            While i <= _node.SubItems.Count
                Dim aHost As TreeNodeSubItemHost = New TreeNodeSubItemHost(Me, _node.SubItems(i))
                aHost.measureOriginal()
                _subItemHosts.Add(aHost)
                i = i + 1
            End While
        End Sub
        ''' <summary>
        ''' Relocate all available TreeNodeSubItemHost.
        ''' </summary>
        Public Sub relocateSubItems()
            For Each lvsiHost As TreeNodeSubItemHost In _subItemHosts
                lvsiHost.moveX()
            Next
        End Sub
        ' Drawing
        ''' <summary>
        ''' Draw bar chart of a TreeNodeSubItem with associated column header.
        ''' </summary>
        <Description("Draw bar chart of a TreeNodeSubItem with associated column header.")> _
        Private Sub drawBar(ByVal g As Graphics, ByVal rect As Rectangle, _
            ByVal subItem As TreeNode.TreeNodeSubItem, ByVal column As ColumnHeader)
            If column.MaximumValue > column.MinimumValue Then
                ' Preparing bar area
                Dim rectBar As Rectangle = rect
                Dim pathBar As GraphicsPath
                rectBar.Y = rectBar.Y + 1
                rectBar.Height = rectBar.Height - 3
                pathBar = Renderer.Drawing.roundedRectangle(rectBar, 2, 2, 2, 2)
                ' Convert subitem's value to Double
                Try
                    Dim subItemValue As Double = CDbl(subItem.Value)
                    Dim columnRange As Double = column.MaximumValue - column.MinimumValue
                    Dim rectValue As Rectangle = rectBar
                    Dim valueRange As Double = subItemValue - column.MinimumValue
                    If valueRange >= 0 And valueRange <= columnRange Then
                        rectValue.Width = Math.Ceiling(valueRange * rectBar.Width / columnRange)
                        Dim valueRegion As Region = New Region(pathBar)
                        valueRegion.Intersect(rectValue)
                        Dim valueGlowBrush As LinearGradientBrush = New LinearGradientBrush( _
                            rectValue, Color.Black, Color.White, LinearGradientMode.Vertical)
                        valueGlowBrush.InterpolationColors = Renderer.Drawing.BarGlow
                        If _owner.Enabled Then
                            g.FillRegion(New SolidBrush(subItem.Color), valueRegion)
                        Else
                            g.FillRegion(Brushes.Gray, valueRegion)
                        End If
                        g.FillRegion(valueGlowBrush, valueRegion)
                        valueRegion.Dispose()
                        valueGlowBrush.Dispose()
                    End If
                Catch ex As Exception
                End Try
                g.DrawPath(Pens.Black, pathBar)
                pathBar.Dispose()
            End If
        End Sub
        ''' <summary>
        ''' Draw TreeNodeSubItems that associated with all unfrozen columns.
        ''' </summary>
        <Description("Draw ListViewSubItems that associated with all unfrozen columns.")> _
        Public Sub drawUnFrozen(ByVal g As Graphics, ByVal frozenCount As Integer)
            If Not _visible Then Return
            If _location.X > _owner.Width Or _location.Y > _owner.Height Then Return
            If Bottom < 0 Or Right < 0 Then Return
            If _owner._fullRowSelect Then
                If _selected Or _onHover Then
                    Dim unfrozenRect As Rectangle = _owner._columnControl.UnFrozenRectangle
                    unfrozenRect.Y = _location.Y
                    unfrozenRect.Height = _size.Height
                    Dim unfrozenPath As GraphicsPath = Renderer.Drawing.roundedRectangle(unfrozenRect, _
                        IIf(frozenCount > 0, 0, 2), 2, IIf(frozenCount > 0, 0, 2), 2)
                    Dim unfrozenBrush As LinearGradientBrush = New LinearGradientBrush( _
                        unfrozenRect, Color.Black, Color.White, LinearGradientMode.Vertical)
                    Dim unfrozenPen As Pen = Nothing
                    If _onMouseDown Then
                        unfrozenBrush.InterpolationColors = Renderer.ListItem.PressedBlend
                        unfrozenPen = Renderer.ListItem.PressedBorderPen
                    Else
                        If _selected Then
                            If _onHover Then
                                unfrozenBrush.InterpolationColors = Renderer.ListItem.SelectedHLiteBlend
                                unfrozenPen = Renderer.ListItem.SelectedHLiteBorderPen
                            Else
                                If _owner.Focused Then
                                    unfrozenBrush.InterpolationColors = Renderer.ListItem.SelectedBlend
                                    unfrozenPen = Renderer.ListItem.SelectedBorderPen
                                Else
                                    unfrozenBrush.InterpolationColors = Renderer.ListItem.SelectedBlurBlend
                                    unfrozenPen = Renderer.ListItem.SelectedBlurBorderPen
                                End If
                            End If
                        Else
                            unfrozenBrush.InterpolationColors = Renderer.ListItem.HLitedBlend
                            unfrozenPen = Renderer.ListItem.HLitedBorderPen
                        End If
                    End If
                    g.FillPath(unfrozenBrush, unfrozenPath)
                    g.DrawPath(unfrozenPen, unfrozenPath)
                    unfrozenBrush.Dispose()
                    unfrozenPen.Dispose()
                End If
            End If
            Dim first As Boolean = frozenCount = 0
            For Each ch As ColumnHeader In _owner._columnControl.UnFrozenColumns
                Dim colIndex As Integer = _owner._columns.IndexOf(ch)
                If colIndex >= 0 And colIndex < _subItemHosts.Count Then
                    _subItemHosts(colIndex).draw(g, Not _owner._fullRowSelect, first And _selected)
                    first = False
                End If
            Next
            If getVisibleChildCount() > 0 And _node.IsExpanded Then
                For Each tnHost As TreeNodeHost In _childHosts
                    tnHost.drawUnFrozen(g, frozenCount)
                Next
            End If
        End Sub
        ''' <summary>
        ''' Draw TreeNodeSubItems that associated with all frozen columns.
        ''' </summary>
        <Description("Draw ListViewSubItems that associated with all frozen columns.")> _
        Public Sub drawFrozen(ByVal g As Graphics, ByVal unfrozenCount As Integer)
            If Not _visible Then Return
            If _location.X > _owner._clientArea.Right Or _location.Y > _owner._clientArea.Bottom Then Return
            If Me.Bottom < 0 Or Me.Right < 0 Then Return
            If _owner._fullRowSelect Then
                If _selected Or _onHover Then
                    Dim frozenRect As Rectangle = _owner._columnControl.FrozenRectangle
                    frozenRect.Y = _location.Y
                    frozenRect.Height = _size.Height
                    frozenRect.Width -= 1
                    Dim frozenPath As GraphicsPath = Renderer.Drawing.roundedRectangle(frozenRect, _
                        2, IIf(unfrozenCount > 0, 0, 2), 2, IIf(unfrozenCount > 0, 0, 2))
                    Dim frozenBrush As LinearGradientBrush = New LinearGradientBrush( _
                        frozenRect, Color.Black, Color.White, LinearGradientMode.Vertical)
                    Dim frozenPen As Pen = Nothing
                    If _onMouseDown Then
                        frozenBrush.InterpolationColors = Renderer.ListItem.PressedBlend
                        frozenPen = Renderer.ListItem.PressedBorderPen
                    Else
                        If _selected Then
                            If _onHover Then
                                frozenBrush.InterpolationColors = Renderer.ListItem.SelectedHLiteBlend
                                frozenPen = Renderer.ListItem.SelectedHLiteBorderPen
                            Else
                                If _owner.Focused Then
                                    frozenBrush.InterpolationColors = Renderer.ListItem.SelectedBlend
                                    frozenPen = Renderer.ListItem.SelectedBorderPen
                                Else
                                    frozenBrush.InterpolationColors = Renderer.ListItem.SelectedBlurBlend
                                    frozenPen = Renderer.ListItem.SelectedBlurBorderPen
                                End If
                            End If
                        Else
                            frozenBrush.InterpolationColors = Renderer.ListItem.HLitedBlend
                            frozenPen = Renderer.ListItem.HLitedBorderPen
                        End If
                    End If
                    g.FillPath(frozenBrush, frozenPath)
                    g.DrawPath(frozenPen, frozenPath)
                    frozenBrush.Dispose()
                    frozenPen.Dispose()
                End If
            End If
            Dim first As Boolean = True
            For Each ch As ColumnHeader In _owner._columnControl.FrozenColumns
                Dim colIndex As Integer = _owner._columns.IndexOf(ch)
                If colIndex >= 0 And colIndex < _subItemHosts.Count Then
                    _subItemHosts(colIndex).draw(g, Not _owner._fullRowSelect, first And _selected)
                    first = False
                End If
            Next
            If getVisibleChildCount() > 0 And _node.IsExpanded Then
                For Each tnHost As TreeNodeHost In _childHosts
                    tnHost.drawFrozen(g, unfrozenCount)
                Next
            End If
        End Sub
        ' Item measurement
        ''' <summary>
        ''' Measure the size of the TreeNodeHost in defferent views.
        ''' </summary>
        <Description("Measure the size of the TreeNodeHost in defferent views.")> _
        Public Sub measureSize()
            Dim height As Integer = 0
            For Each siHost As TreeNodeSubItemHost In _subItemHosts
                siHost.measureOriginal()
                siHost.measureDisplay()
                If height < siHost.Bounds.Height Then height = siHost.Bounds.Height
            Next
            Dim colsRect As Rectangle = _owner._columnControl.DisplayedRectangle
            _location.X = colsRect.X
            _size.Width = colsRect.Width
            _size.Height = height
            _dropDownRect.Y = _location.Y + ((height - 10) / 2)
        End Sub
        ''' <summary>
        ''' Measure the size used as children's area of the host.
        ''' </summary>
        Public Sub measureChildren()
            _childSize.Width = _size.Width
            If _node.IsExpanded Then
                Dim childrenHeight As Integer = 0
                For Each tnHost As TreeNodeHost In _childHosts
                    tnHost.measureSize()
                    tnHost.measureChildren()
                    If tnHost.Visible Then childrenHeight += tnHost.Bounds.Height
                Next
                _childSize.Height = childrenHeight
            Else
                _childSize.Height = 0
            End If
        End Sub
        ''' <summary>
        ''' Gets the bounding rectangle of a TreeNodeSubItem host specified by its index.
        ''' </summary>
        ''' <param name="index">Index of a TreeNodeSubItem object in TreeNode.SubItems collection.</param>
        <Description("Gets the bounding rectangle of a TreeNodeSubItem host specified by its index.")> _
        Public Function getSubItemRectangle(ByVal index As Integer) As Rectangle
            If index >= 0 And index < _subItemHosts.Count Then
                Return _subItemHosts(index).Bounds
            End If
            Return New Rectangle(0, 0, 0, 0)
        End Function
        ''' <summary>
        ''' Gets the display string of a TreeNodeSubItem object specified by its index.
        ''' </summary>
        ''' <param name="index">Index of a TreeNodeSubItem object in TreeNode.SubItems collection.</param>
        <Description("Gets the display string of a TreeNodeSubItem object specified by its index.")> _
        Public Function getSubItemString(ByVal index As Integer) As String
            If index >= 0 And index < _subItemHosts.Count Then
                Return _subItemHosts(index).getSubItemString
            End If
            Return ""
        End Function
        ''' <summary>
        ''' Gets the size of displayed string from a TreeNodeSubItem object specified by its index.
        ''' </summary>
        ''' <param name="index">Index of a TreeNodeSubItem object in TreeNode.SubItems collection.</param>
        <Description("Gets the size of displayed string from a TreeNodeSubItem object specified by its index.")> _
        Public Function getSubItemSize(ByVal index As Integer) As SizeF
            Dim subSize As SizeF = New SizeF(0, 0)
            If index >= 0 And index < _subItemHosts.Count Then
                If index = 0 Then
                    subSize = _subItemHosts(0).OriginalSize
                    subSize.Width += 12 + (_node.Level * _owner._indent)
                Else
                    subSize = _subItemHosts(index).OriginalSize
                End If
                If _node.IsExpanded Then
                    Dim childSize As SizeF
                    For Each tnHost As TreeNodeHost In _childHosts
                        If tnHost.Visible Then
                            childSize = tnHost.getSubItemSize(index)
                            If subSize.Width < childSize.Width Then subSize.Width = childSize.Width
                        End If
                    Next
                End If
            End If
            Return subSize
        End Function
        ''' <summary>
        ''' Gets the size of the node's text, with maximum width and height allowed.
        ''' </summary>
        ''' <param name="maxWidth">Maximum width allowed of the node's text size.</param>
        ''' <param name="maxHeight">Maximum height allowed of the node's text size.</param>
        <Description("Gets the size of the node's text, with maximum width and height allowed.")> _
        Public Function getTextSize(ByVal maxWidth As Integer, ByVal maxHeight As Integer) As SizeF
            Dim result As SizeF
            Dim strFormat As StringFormat = New StringFormat
            strFormat.LineAlignment = StringAlignment.Center
            strFormat.Alignment = StringAlignment.Center
            If Not _owner._allowMultiline Then _
                strFormat.FormatFlags = strFormat.FormatFlags Or StringFormatFlags.LineLimit
            result = _owner._gObj.MeasureString(_node.Text, _node.NodeFont, maxWidth, strFormat)
            If _owner._showImages Then result.Width += 20
            If result.Height > maxHeight Then result.Height = maxHeight
            Return result
        End Function
        ''' <summary>
        ''' Gets the bounding rectangle of the TreeNode text.
        ''' </summary>
        Public Function getTextRectangle() As Rectangle
            Dim txtRect As Rectangle = New Rectangle(0, 0, 0, 0)
            txtRect.Size = _subItemHosts(0).OriginalSize.ToSize
            If txtRect.Height = 0 Then
                txtRect.Height = _node.NodeFont.Height
                txtRect.Width = 10
            End If
            txtRect.Y = _location.Y
            txtRect.X = _subItemHosts(0).Bounds.X + IIf(_owner._showImages, 20, 0)
            Return txtRect
        End Function
        ' Properties
        ''' <summary>
        ''' Determine x location of the TreeNodeHost.
        ''' </summary>
        Public Property X() As Integer
            Get
                Return _location.X
            End Get
            Set(ByVal value As Integer)
                _location.X = value
                relocateSubItems()
                For Each tnHost As TreeNodeHost In _childHosts
                    tnHost.X = _location.X
                Next
            End Set
        End Property
        ''' <summary>
        ''' Determine y location of the TreeNodeHost.
        ''' </summary>
        Public Property Y() As Integer
            Get
                Return _location.Y
            End Get
            Set(ByVal value As Integer)
                Dim dy As Integer = value - Location.Y
                _location.Y = value
                _dropDownRect.Y = _location.Y + ((_size.Height - 10) / 2)
                For Each siHost As TreeNodeSubItemHost In _subItemHosts
                    siHost.Y = _location.Y
                Next
                For Each tnHost As TreeNodeHost In _childHosts
                    tnHost.Y += dy
                Next
            End Set
        End Property
        ''' <summary>
        ''' Determine the location of the TreeNodeHost.
        ''' </summary>
        <Description("Determine the locatoin of the host in ListView." & _
            "This location can be determined by ListView itself, or by the GroupHost where this item is attached.")> _
        Public Property Location() As Point
            Get
                Return _location
            End Get
            Set(ByVal value As Point)
                Dim dy As Integer = _location.Y - value.Y
                _location = value
                _dropDownRect.Y = _location.Y + ((_size.Height - 10) / 2)
                For Each siHost As TreeNodeSubItemHost In _subItemHosts
                    siHost.Y = _location.Y
                Next
                For Each tnHost As TreeNodeHost In _childHosts
                    tnHost.Y += dy
                Next
            End Set
        End Property
        ''' <summary>
        ''' Determine the visibility of the TreeNodeHost.  The visibility of a TreeNodeHost is determined by filtering operation.
        ''' </summary>
        <Description("Determine the visibility of the host.")> _
        Public Property Visible() As Boolean
            Get
                Return _visible
            End Get
            Set(ByVal value As Boolean)
                _visible = value
                If Not _visible Then
                    _onMouseDown = False
                    _onHover = False
                    _onHoverDropDown = False
                End If
            End Set
        End Property
        ''' <summary>
        ''' Determine whether the TreeNodeSubItemHost is selected.
        ''' </summary>
        Public Property Selected() As Boolean
            Get
                Return _selected
            End Get
            Set(ByVal value As Boolean)
                _selected = value
            End Set
        End Property
        ''' <summary>
        ''' Gets a value indicating the TreeNodeHost is visible in the client area of ListView control.
        ''' </summary>
        Public ReadOnly Property IsVisible() As Boolean
            Get
                If Not _visible Then Return False
                If _parentHost IsNot Nothing Then
                    If Not _parentHost.IsVisible Then Return False
                    If Not _parentHost.Node.IsExpanded Then Return False
                End If
                If _location.X > _owner._clientArea.Right Or _location.Y > _owner._clientArea.Bottom Then Return False
                Dim rect As Rectangle = Bounds
                If rect.Right < _owner._clientArea.X Or rect.Bottom < _owner._clientArea.Y Then Return False
                Return True
            End Get
        End Property
        ''' <summary>
        ''' Gets the rightmost location of the TreeNodeHost.
        ''' </summary>
        Public ReadOnly Property Right() As Integer
            Get
                Return _location.X + _size.Width
            End Get
        End Property
        ''' <summary>
        ''' Gets the bottommost location of the TreeNodeHost.
        ''' </summary>
        Public ReadOnly Property Bottom() As Integer
            Get
                Return _location.Y + _size.Height + _childSize.Height
            End Get
        End Property
        ''' <summary>
        ''' Gets the bounding rectangle of the TreeNodeHost.
        ''' </summary>
        <Description("Gets the bounding rectangle of the TreeNodeHost.  It can be different when the item is selected.")> _
        Public ReadOnly Property Bounds() As Rectangle
            Get
                Dim rectArea As Rectangle
                rectArea = New Rectangle(_location, _size)
                rectArea.Height += _childSize.Height
                Return rectArea
            End Get
        End Property
        ''' <summary>
        ''' Gets the TreeNode object contained within TreeNodeHost.
        ''' </summary>
        <Description("Gets TreeNode object contained in the TreeNodeHost.")> _
        Public ReadOnly Property Node() As TreeNode
            Get
                Return _node
            End Get
        End Property
        ''' <summary>
        ''' Gets a value indicating the mouse is pressed on the TreeNodeHost.
        ''' </summary>
        <Description("Gets a value indicating the host is pressed.")> _
        Public ReadOnly Property OnMouseDown() As Boolean
            Get
                Return _onMouseDown
            End Get
        End Property
        ''' <summary>
        ''' Gets a value indicating the mouse pointer is moved over the TreeNodeHost.
        ''' </summary>
        <Description("Gets a value indicating the mouse pointer is moved over the host.")> _
        Public ReadOnly Property OnHover() As Boolean
            Get
                Return _onHover
            End Get
        End Property
        ''' <summary>
        ''' Gets the parent of the host.
        ''' </summary>
        Public ReadOnly Property ParentHost() As TreeNodeHost
            Get
                Return _parentHost
            End Get
        End Property
        ' Mouse event handlers
        ''' <summary>
        ''' Test whether the mouse pointer moves over the host.
        ''' </summary>
        <Description("Test whether the mouse pointer moves over the host.")> _
        Public Function mouseMove(ByVal e As MouseEventArgs) As Boolean
            If Not _visible Then Return False
            If _location.X > _owner._clientArea.Right Or _location.Y > _owner._clientArea.Bottom Then Return False
            If Bottom < _owner._clientArea.Y Or Right < _owner._clientArea.X Then Return False
            Dim stateChanged As Boolean = False
            Dim subitemHover As TreeNodeSubItemHost = Nothing
            Dim columns As List(Of ColumnHeader)
            Dim colIndex As Integer, hoverIndex As Integer = -1
            ' Test on unfrozen columns first
            columns = _owner._columnControl.UnFrozenColumns
            For Each ch As ColumnHeader In columns
                colIndex = _owner._columns.IndexOf(ch)
                If colIndex >= 0 And colIndex < _subItemHosts.Count Then
                    stateChanged = stateChanged Or _subItemHosts(colIndex).mouseMove(e)
                    If _subItemHosts(colIndex).OnHover Then
                        If subitemHover IsNot Nothing Then subitemHover.OnHover = False
                        subitemHover = _subItemHosts(colIndex)
                        hoverIndex = colIndex
                    End If
                End If
            Next
            ' Test on frozen columns.
            columns = _owner._columnControl.FrozenColumns
            If columns.Count > 0 Then
                Dim rectFrozen As Rectangle = _owner._columnControl.FrozenRectangle
                rectFrozen.Y = 0
                rectFrozen.Height = _owner.Height
                If rectFrozen.Contains(e.Location) Then
                    If subitemHover IsNot Nothing Then subitemHover.OnHover = False
                    subitemHover = Nothing
                    hoverIndex = -1
                End If
            End If
            For Each ch As ColumnHeader In columns
                colIndex = _owner._columns.IndexOf(ch)
                If colIndex >= 0 And colIndex < _subItemHosts.Count Then
                    stateChanged = stateChanged Or _subItemHosts(colIndex).mouseMove(e)
                    If _subItemHosts(colIndex).OnHover Then
                        If subitemHover IsNot Nothing Then subitemHover.OnHover = False
                        subitemHover = _subItemHosts(colIndex)
                        hoverIndex = colIndex
                    End If
                End If
            Next
            If _owner._fullRowSelect Then
                Dim itemRect As Rectangle = New Rectangle(_location, _size)
                If itemRect.Contains(e.Location) And e.X > _owner._clientArea.X _
                    And e.Y > _owner._clientArea.Y Then
                    If getVisibleChildCount() > 0 Then
                        If _dropDownRect.Contains(e.Location) Then
                            If Not _onHoverDropDown Then
                                _onHoverDropDown = True
                                stateChanged = True
                            End If
                        Else
                            If _onHoverDropDown Then
                                _onHoverDropDown = False
                                stateChanged = True
                            End If
                        End If
                    End If
                    If Not _onHover Then
                        _owner.invokeNodeMouseHover(_node)
                        _onHover = True
                        If _owner._nodeToolTip Then
                            If Renderer.ToolTip.containsToolTip(_node.ToolTipTitle, _
                                _node.ToolTip, _node.ToolTipImage) Then
                                _owner._needToolTip = True
                                _owner._currentToolTip = _node.ToolTip
                                _owner._currentToolTipTitle = _node.ToolTipTitle
                                _owner._currentToolTipImage = _node.ToolTipImage
                                _owner._currentToolTipRect = itemRect
                                _owner._tooltipCaller = Me
                            End If
                        Else
                            ' Check if mouse hover on node text.
                            If subitemHover IsNot Nothing Then
                                If subitemHover.needToolTip And hoverIndex = 0 Then
                                    _owner._needToolTip = True
                                    _owner._currentToolTip = subitemHover.getSubItemString
                                    _owner._currentToolTipRect = subitemHover.toolTipRect
                                    _owner._tooltipCaller = Me
                                End If
                            End If
                        End If
                        stateChanged = True
                    End If
                Else
                    If _onHover Then
                        _onHover = False
                        If _owner._tooltipCaller Is Me Then _owner._tooltip.hide()
                        stateChanged = True
                    End If
                End If
            Else
                If subitemHover IsNot Nothing And hoverIndex = 0 Then
                    If Not _onHover Then
                        _owner.invokeNodeMouseHover(_node)
                        If _owner._nodeToolTip Then
                            If Renderer.ToolTip.containsToolTip(_node.ToolTipTitle, _
                                _node.ToolTip, _node.ToolTipImage) Then
                                _owner._needToolTip = True
                                _owner._currentToolTip = _node.ToolTip
                                _owner._currentToolTipTitle = _node.ToolTipTitle
                                _owner._currentToolTipImage = _node.ToolTipImage
                                _owner._currentToolTipRect = subitemHover.toolTipRect
                                _owner._tooltipCaller = Me
                            End If
                        Else
                            If subitemHover.needToolTip Then
                                _owner._needToolTip = True
                                _owner._currentToolTip = subitemHover.getSubItemString
                                _owner._currentToolTipRect = subitemHover.toolTipRect
                                _owner._tooltipCaller = Me
                            End If
                        End If
                    End If
                    _onHover = True
                Else
                    If _owner._tooltipCaller Is Me Then _owner._tooltip.hide()
                    _onHover = False
                End If
                If Not _onHover Then
                    If getVisibleChildCount() > 0 Then
                        If _dropDownRect.Contains(e.Location) Then
                            If Not _onHoverDropDown Then
                                _onHoverDropDown = True
                                stateChanged = True
                            End If
                        Else
                            If _onHoverDropDown Then
                                _onHoverDropDown = False
                                stateChanged = True
                            End If
                        End If
                    End If
                End If
            End If
            If getVisibleChildCount() > 0 And _node.IsExpanded Then
                For Each tnHost As TreeNodeHost In _childHosts
                    If tnHost.Visible Then stateChanged = stateChanged Or tnHost.mouseMove(e)
                Next
            End If
            Return stateChanged
        End Function
        ''' <summary>
        ''' Test whether the mouse is pressed over the host.
        ''' </summary>
        <Description("Test whether the mouse is pressed over the host.")> _
        Public Function mouseDown(ByVal e As MouseEventArgs) As Boolean
            Dim stateChanged As Boolean = False
            If _onHover Then
                If _owner._fullRowSelect Then
                    If _onHoverDropDown And e.Button = Windows.Forms.MouseButtons.Left Then
                        If _node.IsExpanded Then
                            _node.collapse()
                        Else
                            _node.expand()
                        End If
                        stateChanged = True
                    Else
                        If _subItemHosts(0).OnHoverCheck Then
                            _node.Checked = Not _node.Checked
                        Else
                            If _owner._selectedHost IsNot Me Then
                                Dim bsEvent As TreeNodeEventArgs = New TreeNodeEventArgs(_node, TreeNodeAction.Unknown)
                                _owner.invokeNodeBeforeSelect(_node, bsEvent)
                                If Not bsEvent.Cancel Then
                                    _owner.setSelectedHost(Me)
                                    _node.expand()
                                End If
                                _owner.invokeNodeMouseDown(_node, e)
                                If e.Button = Windows.Forms.MouseButtons.Left Then _node.expand()
                            End If
                            If Not _onMouseDown Then
                                _onMouseDown = True
                                stateChanged = True
                            End If
                        End If
                    End If
                Else
                    If _subItemHosts(0).OnHoverCheck Then
                        _node.Checked = Not _node.Checked
                    Else
                        If _owner._selectedHost IsNot Me Then
                            Dim bsEvent As TreeNodeEventArgs = New TreeNodeEventArgs(_node, TreeNodeAction.Unknown)
                            _owner.invokeNodeBeforeSelect(_node, bsEvent)
                            If Not bsEvent.Cancel Then
                                _owner.setSelectedHost(Me)
                                _node.expand()
                            End If
                            _owner.invokeNodeMouseDown(_node, e)
                            If e.Button = Windows.Forms.MouseButtons.Left Then _node.expand()
                        End If
                        If Not _onMouseDown Then
                            _onMouseDown = True
                            stateChanged = True
                        End If
                    End If
                End If
            Else
                If _onMouseDown Then
                    _onMouseDown = False
                    stateChanged = True
                End If
            End If
            If _onHoverDropDown And e.Button = Windows.Forms.MouseButtons.Left Then
                If _node.IsExpanded Then
                    _node.collapse()
                Else
                    _node.expand()
                End If
                stateChanged = True
            End If
            If getVisibleChildCount() > 0 And _node.IsExpanded Then
                For Each tnHost As TreeNodeHost In _childHosts
                    If tnHost.Visible Then stateChanged = stateChanged Or tnHost.mouseDown(e)
                Next
            End If
            Return stateChanged
        End Function
        ''' <summary>
        ''' Test whether the mouse is released over the host.
        ''' </summary>
        <Description("Test whether the mouse is released over the host.")> _
        Public Function mouseUp(ByVal e As MouseEventArgs) As Boolean
            Dim stateChanged As Boolean = False
            If _onMouseDown Then
                _onMouseDown = False
                _owner.invokeNodeMouseUp(_node, e)
                stateChanged = True
            End If
            If getVisibleChildCount() > 0 And _node.IsExpanded Then
                For Each tnHost As TreeNodeHost In _childHosts
                    If tnHost.Visible Then stateChanged = stateChanged Or tnHost.mouseUp(e)
                Next
            End If
            Return stateChanged
        End Function
        ''' <summary>
        ''' Test whether the mouse pointer is leaving the host.
        ''' </summary>
        Public Function mouseLeave() As Boolean
            Dim stateChanged As Boolean = False
            For Each tnsiHost As TreeNodeSubItemHost In _subItemHosts
                stateChanged = stateChanged Or tnsiHost.mouseLeave
            Next
            If _onHover Or _onHoverDropDown Then
                _onHover = False
                _onHoverDropDown = False
                stateChanged = True
                _owner.invokeNodeMouseLeave(_node)
            End If
            If getVisibleChildCount() > 0 And _node.IsExpanded Then
                For Each tnHost As TreeNodeHost In _childHosts
                    If tnHost.Visible Then stateChanged = stateChanged Or tnHost.mouseLeave
                Next
            End If
            Return stateChanged
        End Function
        ' Node event handler
        ''' <summary>
        ''' Performs additional action when a TreeNode is added to the node.
        ''' </summary>
        Private Sub node_NodeAdded(ByVal sender As Object, ByVal e As CollectionEventArgs)
            Dim newNode As TreeNode = DirectCast(e.Item, TreeNode)
            Dim newHost As TreeNodeHost = New TreeNodeHost(newNode, _owner, Me)
            newHost.Visible = _owner.filterNode(newNode)
            _childHosts.Add(newHost)
            sortChildren(False)
            If _node.IsExpanded Then
                _owner.measureAll()
                _owner.relocateAll()
                _owner.Invalidate()
            End If
        End Sub
        ''' <summary>
        ''' Performs additional action when a TreeNode is removed from the node.
        ''' </summary>
        Private Sub node_NodeRemoved(ByVal sender As Object, ByVal e As CollectionEventArgs)
            Dim remNode As TreeNode = DirectCast(e.Item, TreeNode)
            Dim remHost As TreeNodeHost = Nothing
            For Each tnHost As TreeNodeHost In _childHosts
                If tnHost.Node Is remNode Then
                    remHost = tnHost
                    Exit For
                End If
            Next
            If remHost Is Nothing Then Return
            remHost.removeHandlers()
            _childHosts.Remove(remHost)
            sortChildren(False)
            If _node.IsExpanded Then
                _owner.measureAll()
                _owner.relocateAll()
                _owner.Invalidate()
            End If
        End Sub
        ''' <summary>
        ''' Performs additional action the children of the node has been cleared.
        ''' </summary>
        Private Sub node_NodesOnClear(ByVal sender As Object, ByVal e As CollectionEventArgs)
            For Each tnHost As TreeNodeHost In _childHosts
                tnHost.removeHandlers()
            Next
            _childHosts.Clear()
            If _node.IsExpanded Then
                _owner.measureAll()
                _owner.relocateAll()
                _owner.Invalidate()
            End If
        End Sub
    End Class
#End Region
#Region "Private Routines."
    ''' <summary>
    ''' Gets the maximum width of subitem in all of the items specified by its index.
    ''' </summary>
    <Description("Gets the maximum width of subitem in all of the items specified by its index.")> _
    Private Function getSubItemMaxWidth(ByVal index As Integer) As Integer
        Dim maxWidth As Integer = 0
        Dim iSize As SizeF
        For Each tnHost As TreeNodeHost In _nodeHosts
            If tnHost.Visible Then
                iSize = tnHost.getSubItemSize(index)
                If maxWidth < iSize.Width Then maxWidth = Math.Ceiling(iSize.Width)
            End If
        Next
        Return maxWidth
    End Function
    ''' <summary>
    ''' Gets the maximum width of subitem in all of the items specified by its column.
    ''' </summary>
    <Description("Gets the maximum width of subitem in all of the items specified by its column.")> _
    Private Function getSubItemMaxWidth(ByVal column As ColumnHeader) As Integer
        Return getSubItemMaxWidth(_columns.IndexOf(column))
    End Function
    ''' <summary>
    ''' Compares two TreeNodeHosts for equivalence.  Depending on the column sort order.
    ''' </summary>
    <Description("Compares two TreeNodeHosts for equivalence.  Depending on the column sort order.")> _
    Private Function nodeHostComparer(ByVal host1 As TreeNodeHost, ByVal host2 As TreeNodeHost) As Integer
        If _columnRef = -1 Then Return 0
        ' Check whether both hosts is visible
        If host1.Visible And host2.Visible Then
            Dim colsToCompare As List(Of Integer) = New List(Of Integer)
            Dim i As Integer, result As Integer = 0, currentColumn As Integer
            ' Create a sequence of column indexes to perform comparison
            ' Add sorted column index to the first sequence
            colsToCompare.Add(_columnRef)
            i = 0
            While i < _columns.Count
                If i <> _columnRef Then colsToCompare.Add(i)
                i = i + 1
            End While
            i = 0
            While i < colsToCompare.Count And result = 0
                currentColumn = colsToCompare(i)
                Try
                    Dim vNode1 As Object
                    Dim vNode2 As Object
                    Dim objCompare As Comparer = New Comparer(_ci)
                    vNode1 = host1.Node.SubItems(currentColumn).Value
                    vNode2 = host2.Node.SubItems(currentColumn).Value
                    If _columns.Item(currentColumn).SortOrder = SortOrder.Ascending Then
                        result = objCompare.Compare(vNode1, vNode2)
                    Else
                        result = -objCompare.Compare(vNode1, vNode2)
                    End If
                Catch ex As Exception
                    result = 0
                End Try
                i = i + 1
            End While
            Return result
        Else
            If host1.Visible Then
                If _columns(_columnRef).SortOrder = SortOrder.Ascending Then
                    Return 1
                Else
                    Return -1
                End If
            ElseIf host2.Visible Then
                If _columns(_columnRef).SortOrder = SortOrder.Ascending Then
                    Return -1
                Else
                    Return 1
                End If
            End If
            Return 0
        End If
    End Function
    ''' <summary>
    ''' Filter a TreeNode based on existing filter parameters on each columns.
    ''' </summary>
    <Description("Filter a TreeNode based on existing filter parameters on each columns.")> _
    Private Function filterNode(ByVal node As TreeNode) As Boolean
        Dim handlers As List(Of ColumnFilterHandle) = _columnControl.FilterHandlers
        Dim i As Integer = 0
        Dim result As Boolean = True
        While i <= node.SubItems.Count And i < handlers.Count
            result = result And handlers(i).filterValue(node.SubItems(i).Value)
            i = i + 1
        End While
        Return result
    End Function
    ''' <summary>
    ''' Filter a TreeNode based on specified filter parameters.
    ''' </summary>
    <Description("Filter a TreeNode based on specified filter parameters.")> _
    Private Function filterNode(ByVal node As TreeNode, ByVal handlers As List(Of ColumnFilterHandle)) As Boolean
        Dim i As Integer = 0
        Dim result As Boolean = True
        While i <= node.SubItems.Count And i < handlers.Count
            result = result And handlers(i).filterValue(node.SubItems(i).Value)
            i = i + 1
        End While
        Return result
    End Function
    ''' <summary>
    ''' Change the check state of the checkbox that appears at column header.
    ''' </summary>
    <Description("Change the check state of the checkbox that appears at column header.")> _
    Private Sub changeColumnCheckState()
        Dim checkedCount As Integer = 0
        Dim indeterminateCount As Integer = 0
        For Each tnHost As TreeNodeHost In _nodeHosts
            If tnHost.Node.CheckState = CheckState.Checked Then
                checkedCount += 1
            ElseIf tnHost.Node.CheckState = CheckState.Indeterminate Then
                indeterminateCount += 1
            End If
        Next
        If checkedCount = _nodeHosts.Count Then
            _columnControl.CheckState = CheckState.Checked
        Else
            If checkedCount = 0 Then
                If indeterminateCount = 0 Then
                    _columnControl.CheckState = CheckState.Unchecked
                Else
                    _columnControl.CheckState = CheckState.Indeterminate
                End If
            Else
                _columnControl.CheckState = CheckState.Indeterminate
            End If
        End If
    End Sub
    ''' <summary>
    ''' Sort all existing hosts.
    ''' </summary>
    <Description("Sort all existing hosts.")> _
    Private Sub sortAll()
        If _columnRef = -1 Then Return
        _nodeHosts.Sort(AddressOf nodeHostComparer)
        For Each tnHost As TreeNodeHost In _nodeHosts
            tnHost.sortChildren(False)
        Next
    End Sub
    ''' <summary>
    ''' Measure all existing hosts, and the client area used to display all hosts.
    ''' </summary>
    Private Sub measureAll()
        If _nodeHosts.Count = 0 Then
            _clientArea = New Rectangle(1, _columnControl.Bottom + 1, Me.Width - (_vScroll.Width + 2), Me.Height - (_columnControl.Bottom + 2))
            _hScroll.Left = 1
            _hScroll.Top = Me.Height - (_hScroll.Height + 1)
            _hScroll.Width = Me.Width - 2
            Dim columnsWidth As Integer = _columnControl.ColumnsWidth + IIf(_checkBoxes, 22, 0)
            If columnsWidth > Me.Width - (_vScroll.Width + _clientArea.X) Then
                _hScroll.Visible = True
                _hScroll.SmallChange = _clientArea.Width / 20
                _hScroll.LargeChange = _clientArea.Width / 10
                _hScroll.Maximum = (columnsWidth - _clientArea.Width) + _hScroll.LargeChange
            Else
                _hScroll.Visible = False
            End If
            Return
        End If
        Dim itemsHeight As Integer = 0
        Dim itemsWidth As Integer = _columnControl.ColumnsWidth
        For Each tnHost As TreeNodeHost In _nodeHosts
            tnHost.measureSize()
            tnHost.measureChildren()
        Next
        For Each tnHost As TreeNodeHost In _nodeHosts
            If tnHost.Visible Then
                itemsHeight += tnHost.Bounds.Height + 1
            End If
        Next
        _clientArea.X = 1
        _clientArea.Y = _columnControl.Bottom + 1
        Dim heightUsed As Integer = _clientArea.Y
        If Me.Width > 0 Then ' For minimize
            _clientArea.Width = Me.Width - (_vScroll.Width + _clientArea.X)
            If itemsWidth > Me.Width - (_vScroll.Width + _clientArea.X) Then
                _hScroll.Visible = True
                _hScroll.SmallChange = _clientArea.Width / 20
                _hScroll.LargeChange = _clientArea.Width / 10
                _hScroll.Maximum = (itemsWidth - _clientArea.Width) + _hScroll.LargeChange
                heightUsed += _hScroll.Height
            Else
                _hScroll.Visible = False
            End If
        End If
        If Me.Height > 0 Then ' For minimize
            _clientArea.Height = Me.Height - heightUsed
            If itemsHeight > _clientArea.Height Then
                _vScroll.Visible = True
                _vScroll.SmallChange = _clientArea.Height / 20
                _vScroll.LargeChange = _clientArea.Height / 10
                _vScroll.Maximum = (itemsHeight - _clientArea.Height) + _vScroll.LargeChange
            Else
                _vScroll.Visible = False
            End If
        End If
        _vScroll.Top = _clientArea.Y
        _vScroll.Left = Me.Width - (_vScroll.Width + 1)
        _vScroll.Height = _clientArea.Height
        _hScroll.Left = 1
        _hScroll.Top = Me.Height - (_hScroll.Height + 1)
        _hScroll.Width = IIf(_vScroll.Visible, Me.Width - (_vScroll.Width + 2), Me.Width - 2)
    End Sub
    ''' <summary>
    ''' Relocate all existing hosts and scrollbars.
    ''' </summary>
    Private Sub relocateAll()
        If _nodeHosts.Count = 0 Then Return
        Dim x As Integer = _clientArea.X
        Dim y As Integer = _clientArea.Y
        If _hScroll.Visible Then x -= _hScroll.Value
        If _vScroll.Visible Then y -= _vScroll.Value
        For Each tnHost As TreeNodeHost In _nodeHosts
            tnHost.Y = y
            tnHost.relocateSubItems()
            tnHost.relocateChildrenHosts()
            If tnHost.Visible Then y = tnHost.Bounds.Bottom + 1
        Next
    End Sub
    ''' <summary>
    ''' Sets the selected TreeNodeHost to a specified TreeNodeHost.
    ''' </summary>
    Private Sub setSelectedHost(ByVal host As TreeNodeHost)
        Dim selectedHostChanged As Boolean = False
        If _selectedHost Is Nothing Then selectedHostChanged = True
        If _selectedHost IsNot host Then selectedHostChanged = True
        If _selectedHost IsNot Nothing Then _selectedHost.Selected = False
        _selectedHost = host
        If _selectedHost IsNot Nothing Then _selectedHost.Selected = True
        If selectedHostChanged Then
            If _selectedHost IsNot Nothing Then RaiseEvent AfterSelect(Me, New TreeNodeEventArgs(_selectedHost.Node, TreeNodeAction.Unknown))
            RaiseEvent SelectedNodeChanged(Me, New EventArgs)
        End If
    End Sub
    ''' <summary>
    ''' Raise the NodeMouseHover event.
    ''' </summary>
    Private Sub invokeNodeMouseHover(ByVal node As TreeNode)
        RaiseEvent NodeMouseHover(Me, New TreeNodeEventArgs(node, TreeNodeAction.MouseHover))
    End Sub
    ''' <summary>
    ''' Raise the NodeMouseLeave event.
    ''' </summary>
    Private Sub invokeNodeMouseLeave(ByVal node As TreeNode)
        RaiseEvent NodeMouseLeave(Me, New TreeNodeEventArgs(node, TreeNodeAction.MouseLeave))
    End Sub
    ''' <summary>
    ''' Raise the NodeMouseDown event.
    ''' </summary>
    Private Sub invokeNodeMouseDown(ByVal node As TreeNode, ByVal e As MouseEventArgs)
        RaiseEvent NodeMouseDown(Me, New TreeNodeMouseEventArgs(node, TreeNodeAction.MouseDown, e))
    End Sub
    ''' <summary>
    ''' Raise the NodeMouseUp event.
    ''' </summary>
    Private Sub invokeNodeMouseUp(ByVal node As TreeNode, ByVal e As MouseEventArgs)
        RaiseEvent NodeMouseUp(Me, New TreeNodeMouseEventArgs(node, TreeNodeAction.MouseDown, e))
    End Sub
    ''' <summary>
    ''' Raise the BeforeSelect event.
    ''' </summary>
    Private Sub invokeNodeBeforeSelect(ByVal node As TreeNode, ByVal e As TreeNodeEventArgs)
        RaiseEvent BeforeSelect(Me, e)
    End Sub
    ''' <summary>
    ''' Gets a string represent the value of a TreeNodeSubItem.
    ''' </summary>
    Private Function getValueString(ByVal value As Object, ByVal index As Integer) As String
        If index < 0 Or index >= _columns.Count Then Return ""
        Dim result As String = ""
        If value Is Nothing Then Return ""
        Dim aColumn As ColumnHeader = _columns(index)
        If aColumn IsNot Nothing Then
            Select Case aColumn.Format
                Case ColumnFormat.None
                    result = value.ToString
                Case ColumnFormat.Password
                    result = "12345"
                Case ColumnFormat.Bar, ColumnFormat.Currency, _
                    ColumnFormat.Custom, ColumnFormat.Exponential, _
                    ColumnFormat.FixedPoint, ColumnFormat.General, _
                    ColumnFormat.HexaDecimal, ColumnFormat.Number, _
                    ColumnFormat.Percent, ColumnFormat.RoundTrip
                    ' Convert to double
                    Try
                        Dim dblValue As Double = CDbl(value)
                        Select Case aColumn.Format
                            Case ColumnFormat.Bar, ColumnFormat.Custom
                                result = dblValue.ToString(aColumn.CustomFormat, _ci)
                            Case ColumnFormat.Currency
                                result = dblValue.ToString("C", _ci)
                            Case ColumnFormat.Exponential
                                result = dblValue.ToString("E", _ci)
                            Case ColumnFormat.FixedPoint
                                result = dblValue.ToString("F", _ci)
                            Case ColumnFormat.General
                                result = dblValue.ToString("G", _ci)
                            Case ColumnFormat.HexaDecimal
                                result = dblValue.ToString("X", _ci)
                            Case ColumnFormat.Number
                                result = dblValue.ToString("N", _ci)
                            Case ColumnFormat.Percent
                                result = dblValue.ToString("P", _ci)
                            Case ColumnFormat.RoundTrip
                                result = dblValue.ToString("R", _ci)
                        End Select
                    Catch ex As Exception
                    End Try
                Case ColumnFormat.DecimalNumber
                    ' Convert to integer
                    Try
                        Dim intValue As Integer = CInt(value)
                        result = intValue.ToString("D", _ci)
                    Catch ex As Exception
                    End Try
                Case Else
                    ' Convert to datetime
                    Try
                        Dim dtValue As Date = CDate(value)
                        Select Case aColumn.Format
                            Case ColumnFormat.CustomDateTime
                                result = dtValue.ToString(aColumn.CustomFormat, _ci)
                            Case ColumnFormat.ShortDate
                                result = dtValue.ToString("d", _ci)
                            Case ColumnFormat.LongDate
                                result = dtValue.ToString("D", _ci)
                            Case ColumnFormat.FullDateShortTime
                                result = dtValue.ToString("f", _ci)
                            Case ColumnFormat.FullDateLongTime
                                result = dtValue.ToString("F", _ci)
                            Case ColumnFormat.GeneralDateShortTime
                                result = dtValue.ToString("g", _ci)
                            Case ColumnFormat.GeneralDateLongTime
                                result = dtValue.ToString("G", _ci)
                            Case ColumnFormat.RoundTripDateTime
                                result = dtValue.ToString("O", _ci)
                            Case ColumnFormat.RFC1123
                                result = dtValue.ToString("R", _ci)
                            Case ColumnFormat.SortableDateTime
                                result = dtValue.ToString("s", _ci)
                            Case ColumnFormat.ShortTime
                                result = dtValue.ToString("t", _ci)
                            Case ColumnFormat.LongTime
                                result = dtValue.ToString("T", _ci)
                            Case ColumnFormat.UniversalSortableDateTime
                                result = dtValue.ToString("u", _ci)
                            Case ColumnFormat.UniversalFullDateTime
                                result = dtValue.ToString("U", _ci)
                        End Select
                    Catch ex As Exception
                    End Try
            End Select
        End If
        Return result
    End Function
    ''' <summary>
    ''' Show a TextBox for label editing on a specified TreeNodeHost.
    ''' </summary>
    Private Sub showTextBoxEditor(ByVal aHost As TreeNodeHost)
        If aHost Is Nothing Then Return
        If _columns.Count = 0 Then Return
        If Not _columns(0).Visible Then Return
        _currentEditedHost = aHost
        If _allowMultiline Then
            _txtEditor.Multiline = True
            _txtEditor.ScrollBars = ScrollBars.Both
        Else
            _txtEditor.Multiline = False
            _txtEditor.ScrollBars = ScrollBars.None
        End If
        Dim txtRect As Rectangle = aHost.getTextRectangle
        _txtEditor.Location = txtRect.Location
        _txtEditor.Size = txtRect.Size
        _txtEditor.Text = aHost.Node.Text
        _txtEditor.Visible = True
        _txtEditor.SelectAll()
        _txtEditor.Focus()
    End Sub
    ''' <summary>
    ''' Collapse all existing children in a node.
    ''' </summary>
    Private Sub collapseAllChild(ByVal node As TreeNode)
        Dim changeInternalThread As Boolean = False ' This variable is used to avoid repeat call to method measureAll, relocateAll, and Invalidate.
        If Not _internalThread Then
            _internalThread = True
            changeInternalThread = True
        End If
        For Each tn As TreeNode In node.Nodes
            tn._collapse()
            If tn.Nodes.Count > 0 Then collapseAllChild(tn)
        Next
        If changeInternalThread Then _internalThread = False
    End Sub
    ''' <summary>
    ''' Expand all existing children in a node.
    ''' </summary>
    Private Sub expandAllChild(ByVal node As TreeNode)
        Dim changeInternalThread As Boolean = False ' This variable is used to avoid repeat call to method measureAll, relocateAll, and Invalidate.
        If Not _internalThread Then
            _internalThread = True
            changeInternalThread = True
        End If
        For Each tn As TreeNode In node.Nodes
            tn._expand()
            If tn.Nodes.Count > 0 Then expandAllChild(tn)
        Next
        If changeInternalThread Then _internalThread = False
    End Sub
    ''' <summary>
    ''' Check the CheckState of a TreeNode.
    ''' </summary>
    Private Sub checkNodeCheckedState(ByVal node As TreeNode)
        Dim checkedCount As Integer = 0
        Dim indeterminateCount As Integer = 0
        For Each tn As TreeNode In node.Nodes
            If tn.CheckState = CheckState.Checked Then checkedCount += 1
            If tn.CheckState = CheckState.Indeterminate Then indeterminateCount += 1
        Next
        If checkedCount = 0 And indeterminateCount = 0 Then
            node.setCheckState(CheckState.Unchecked)
        ElseIf checkedCount = node.Nodes.Count Then
            node.setCheckState(CheckState.Checked)
        Else
            node.setCheckState(CheckState.Indeterminate)
        End If
        If node.Parent IsNot Nothing Then checkNodeCheckedState(node.Parent)
    End Sub
    ''' <summary>
    ''' Change the checked property on children node of a TreeNode.
    ''' </summary>
    Private Sub changeChildrenChecked(ByVal node As TreeNode)
        For Each tn As TreeNode In node.Nodes
            tn.setChecked(node.Checked)
            If tn.Nodes.Count > 0 Then changeChildrenChecked(tn)
        Next
    End Sub
    ''' <summary>
    ''' Gets previous visible node host from current selected node host.
    ''' </summary>
    Private Function selectPrevNodeHost() As TreeNodeHost
        If _nodeHosts.Count = 0 Then Return Nothing
        If _selectedHost Is Nothing Then Return Nothing
        If _selectedHost.ParentHost IsNot Nothing Then
            Return _selectedHost.ParentHost.getPrevHost(_selectedHost)
        Else
            Dim hostIndex As Integer = _nodeHosts.IndexOf(_selectedHost) - 1
            While hostIndex >= 0
                If _nodeHosts(hostIndex).Visible Then Return _nodeHosts(hostIndex).getLastVisibleNode
                hostIndex -= 1
            End While
        End If
        Return Nothing
    End Function
    ''' <summary>
    ''' Gets next visible node host from current selected node host.
    ''' </summary>
    Private Function selectNextNodeHost() As TreeNodeHost
        If _nodeHosts.Count = 0 Then Return Nothing
        If _selectedHost Is Nothing Then Return Nothing
        If _selectedHost.getVisibleChildCount > 0 And _selectedHost.Node.IsExpanded Then Return _selectedHost.getNextHost(_selectedHost)
        Dim fromHost As TreeNodeHost = _selectedHost
        If _selectedHost.ParentHost IsNot Nothing Then
            Dim result As TreeNodeHost = Nothing
            Dim pHost As TreeNodeHost = _selectedHost.ParentHost
            While result Is Nothing And pHost IsNot Nothing
                result = pHost.getNextHost(fromHost)
                If result IsNot Nothing Then Return result
                fromHost = pHost
                pHost = pHost.ParentHost
            End While
        End If
        Dim i As Integer = _nodeHosts.IndexOf(fromHost) + 1
        While i < _nodeHosts.Count
            If _nodeHosts(i).Visible Then Return _nodeHosts(i)
            i += 1
        End While
        Return Nothing
    End Function
    ''' <summary>
    ''' Gets next visible node host from current selected node host, specified by starting charachter of the node's text.
    ''' </summary>
    Private Function selectNextNodeHost(ByVal startsWith As String) As TreeNodeHost
        If _nodeHosts.Count = 0 Then Return Nothing
        Dim result As TreeNodeHost = Nothing
        Dim i As Integer
        If _selectedHost IsNot Nothing Then
            If _selectedHost.getVisibleChildCount > 0 And _selectedHost.Node.IsExpanded Then
                result = _selectedHost.getNextHost(_selectedHost, startsWith)
                If result IsNot Nothing Then Return result
            End If
            Dim fromHost As TreeNodeHost = _selectedHost
            If _selectedHost.ParentHost IsNot Nothing Then
                Dim pHost As TreeNodeHost = _selectedHost.ParentHost
                While result Is Nothing And pHost IsNot Nothing
                    result = pHost.getNextHost(fromHost, startsWith)
                    If result IsNot Nothing Then Return result
                    fromHost = pHost
                    pHost = pHost.ParentHost
                End While
            End If
            i = _nodeHosts.IndexOf(fromHost) + 1
            While i < _nodeHosts.Count
                If _nodeHosts(i).Visible Then
                    result = _nodeHosts(i).getNextHost(_nodeHosts(i), startsWith, False)
                    If result IsNot Nothing Then Return result
                End If
                i += 1
            End While
        End If
        ' No node host found, starting from the first node.
        i = 0
        While i < _nodeHosts.Count
            If _nodeHosts(i).Visible Then
                result = _nodeHosts(i).getNextHost(_nodeHosts(i), startsWith, False)
                If result IsNot Nothing Then Return result
            End If
            i += 1
        End While
        Return Nothing
    End Function
    ''' <summary>
    ''' Gets the first visible node host.
    ''' </summary>
    Private Function selectFirstNodeHost() As TreeNodeHost
        Dim i As Integer = 0
        While i < _nodeHosts.Count
            If _nodeHosts(i).Visible Then Return _nodeHosts(i)
            i += 1
        End While
        Return Nothing
    End Function
    ''' <summary>
    ''' Gets the last visible node host.
    ''' </summary>
    Private Function selectLastNodeHost() As TreeNodeHost
        Dim i As Integer = _nodeHosts.Count - 1
        While i >= 0
            If _nodeHosts(i).Visible Then Return _nodeHosts(i).getLastVisibleNode
            i -= 1
        End While
        Return Nothing
    End Function
    ''' <summary>
    ''' Ensures that the specified node is visible within the control, scrolling the contents of the control if necessary.
    ''' </summary>
    Private Sub ensureVisible(ByVal nodeHost As TreeNodeHost)
        If nodeHost Is Nothing Then Return
        If Not nodeHost.Visible Then Return
        Dim dx As Integer = 0
        Dim dy As Integer = 0
        If nodeHost.X < _clientArea.X Or nodeHost.Right > _clientArea.Right Then
            If nodeHost.X < _clientArea.X Then
                dx = nodeHost.X - _clientArea.X
            Else
                dx = nodeHost.Right - _clientArea.Right
            End If
        End If
        If nodeHost.Y < _clientArea.Y Or nodeHost.Bottom > _clientArea.Bottom Then
            If nodeHost.Y < _clientArea.Y Then
                dy = nodeHost.Y - _clientArea.Y
            Else
                dy = nodeHost.Bottom - _clientArea.Bottom
            End If
        End If
        If _vScroll.Visible Or _hScroll.Visible Then
            _vScroll.Value += dy
            _hScroll.Value += dx
        End If
    End Sub
    ''' <summary>
    ''' Gets a TreeNodeHost for specified TreeNode object.
    ''' </summary>
    Private Function getNodeHost(ByVal node As TreeNode) As TreeNodeHost
        If node Is Nothing Then Return Nothing
        If _nodeHosts.Count = 0 Then Return Nothing
        For Each tnHost As TreeNodeHost In _nodeHosts
            If tnHost.Visible Then
                Dim result As TreeNodeHost = tnHost.getHost(node)
                If result IsNot Nothing Then
                    If result.Visible Then
                        Return result
                    Else
                        Return Nothing
                    End If
                End If
            End If
        Next
        Return Nothing
    End Function
#End Region
#Region "Friend Routines."
    ' Friend property, to provide design mode to the other classes.
    Friend ReadOnly Property IsDesignMode() As Boolean
        Get
            Return DesignMode
        End Get
    End Property
    ' TreeNode related.
    Friend Sub _nodeTextChanged(ByVal node As TreeNode)
        If _columns.Count = 0 Then Return
        If _columns(0).SizeType = ColumnSizeType.Auto Then _columnControl.relocateHosts(IIf(_hScroll.Visible, -_hScroll.Value, 0))
        ' Rebuilding filter
        Dim nodeHost As TreeNodeHost = Nothing
        Dim foundHost As TreeNodeHost = Nothing
        Dim objs As List(Of Object) = New List(Of Object)
        For Each tnHost As TreeNodeHost In _nodeHosts
            foundHost = tnHost.getHost(node)
            If foundHost IsNot Nothing Then nodeHost = foundHost
            tnHost.collectSubItemValue(0, objs)
        Next
        If nodeHost Is Nothing Then Return
        _columnControl.reloadFilter(0, objs)
        nodeHost.Visible = filterNode(node)
        sortAll()
        measureAll()
        relocateAll()
        Me.Invalidate()
    End Sub
    Friend Sub _nodeFontChanged(ByVal node As TreeNode)
        If _columns.Count = 0 Then Return
        If _columns(0).SizeType = ColumnSizeType.Auto Then _columnControl.relocateHosts(IIf(_hScroll.Visible, -_hScroll.Value, 0))
        Dim foundHost As TreeNodeHost = Nothing
        For Each tnHost As TreeNodeHost In _nodeHosts
            foundHost = tnHost.getHost(node)
            If foundHost IsNot Nothing Then Exit For
        Next
        If foundHost Is Nothing Then Return
        measureAll()
        relocateAll()
        Me.Invalidate()
    End Sub
    Friend Sub _nodeBackColorChanged(ByVal node As TreeNode)
        Dim foundHost As TreeNodeHost = Nothing
        For Each tnHost As TreeNodeHost In _nodeHosts
            foundHost = tnHost.getHost(node)
            If foundHost IsNot Nothing Then
                If foundHost.IsVisible Then Me.Invalidate()
                Return
            End If
        Next
    End Sub
    Friend Sub _nodeColorChanged(ByVal node As TreeNode)
        Dim foundHost As TreeNodeHost = Nothing
        For Each tnHost As TreeNodeHost In _nodeHosts
            foundHost = tnHost.getHost(node)
            If foundHost IsNot Nothing Then
                If foundHost.IsVisible Then Me.Invalidate()
                Return
            End If
        Next
    End Sub
    Friend Sub _nodeImageChanged(ByVal node As TreeNode)
        If Not _showImages Then Return
        Dim foundHost As TreeNodeHost = Nothing
        For Each tnHost As TreeNodeHost In _nodeHosts
            foundHost = tnHost.getHost(node)
            If foundHost IsNot Nothing Then
                If foundHost.IsVisible Then Me.Invalidate()
                Return
            End If
        Next
    End Sub
    Friend Sub _nodeExpandedImageChanged(ByVal node As TreeNode)
        If Not _showImages Then Return
        Dim foundHost As TreeNodeHost = Nothing
        For Each tnHost As TreeNodeHost In _nodeHosts
            foundHost = tnHost.getHost(node)
            If foundHost IsNot Nothing Then
                If foundHost.IsVisible Then Me.Invalidate()
                Return
            End If
        Next
    End Sub
    Friend Sub _nodeUseNodeStyleForSubItemsChanged(ByVal node As TreeNode)
        _nodeFontChanged(node)
    End Sub
    Friend Sub _nodeSubItemsChanged(ByVal node As TreeNode)
        Dim foundHost As TreeNodeHost = Nothing
        For Each tnHost As TreeNodeHost In _nodeHosts
            foundHost = tnHost.getHost(node)
            If foundHost IsNot Nothing Then Exit For
        Next
        If foundHost Is Nothing Then Return
        foundHost.refreshSubItem()
        sortAll()
        _columnControl.relocateHosts(IIf(_hScroll.Visible, -_hScroll.Value, 0))
        measureAll()
        relocateAll()
    End Sub
    ' TreeNode events
    Friend Sub _nodeBeforeCheck(ByVal node As TreeNode, ByVal e As TreeNodeEventArgs)
        RaiseEvent BeforeCheck(Me, e)
    End Sub
    <Description("Fired when checked property of TreeNode has been changed.")> _
    Friend Sub _nodeChecked(ByVal node As TreeNode)
        If node.Checked Then
            _checkedNodes.Add(node)
        Else
            _checkedNodes.Remove(node)
        End If
        RaiseEvent AfterCheck(Me, New TreeNodeEventArgs(node, TreeNodeAction.Checked))
        If node.Parent IsNot Nothing Then checkNodeCheckedState(node.Parent)
        If node.Nodes.Count > 0 Then changeChildrenChecked(node)
        Dim foundHost As TreeNodeHost = Nothing
        For Each tnHost As TreeNodeHost In _nodeHosts
            foundHost = tnHost.getHost(node)
            If foundHost IsNot Nothing Then Exit For
        Next
        If foundHost Is Nothing Then Return
        If Not _internalThread Then
            changeColumnCheckState()
            Me.Invalidate(True)
        End If
    End Sub
    Friend Sub _invokeNodeChecked(ByVal node As TreeNode)
        If node.Checked Then
            _checkedNodes.Add(node)
        Else
            _checkedNodes.Remove(node)
        End If
        RaiseEvent AfterCheck(Me, New TreeNodeEventArgs(node, TreeNodeAction.Checked))
    End Sub
    Friend Sub _nodeBeforeCollapse(ByVal node As TreeNode, ByVal e As TreeNodeEventArgs)
        RaiseEvent BeforeCollapse(Me, e)
    End Sub
    Friend Sub _nodeCollapsed(ByVal node As TreeNode, Optional ByVal ignoreChildren As Boolean = True)
        RaiseEvent AfterCollapse(Me, New TreeNodeEventArgs(node, TreeNodeAction.Collapse))
        If Not ignoreChildren Then collapseAllChild(node)
        If _internalThread Then Return
        measureAll()
        relocateAll()
        If _selectedHost IsNot Nothing Then
            If _selectedHost.isDescendantFrom(node) Then
                setSelectedHost(_selectedHost.getParentHost(node))
                ensureVisible(_selectedHost)
            End If
        End If
        Me.Invalidate()
    End Sub
    Friend Sub _invokeNodeCollapsed(ByVal node As TreeNode)
        RaiseEvent AfterCollapse(Me, New TreeNodeEventArgs(node, TreeNodeAction.Collapse))
    End Sub
    Friend Sub _nodeBeforeExpand(ByVal node As TreeNode, ByVal e As TreeNodeEventArgs)
        RaiseEvent BeforeExpand(Me, e)
    End Sub
    Friend Sub _nodeExpanded(ByVal node As TreeNode, Optional ByVal ignoreChildren As Boolean = True)
        RaiseEvent AfterExpand(Me, New TreeNodeEventArgs(node, TreeNodeAction.Expand))
        If Not ignoreChildren Then expandAllChild(node)
        If _internalThread Then Return
        measureAll()
        relocateAll()
        Me.Invalidate()
    End Sub
    Friend Sub _invokeNodeExpanded(ByVal node As TreeNode)
        RaiseEvent AfterExpand(Me, New TreeNodeEventArgs(node, TreeNodeAction.Expand))
    End Sub
    ' TreeNodeSubItem related.
    Friend Sub _subitemValueChanged(ByVal subitem As TreeNode.TreeNodeSubItem)
        Dim subItemIndex As Integer = subitem.TreeNode.SubItems.IndexOf(subitem) + 1
        If _columns.Count = 0 Or subItemIndex >= _columns.Count Then Return
        If _columns(subItemIndex).SizeType = ColumnSizeType.Auto Then _columnControl.relocateHosts(IIf(_hScroll.Visible, -_hScroll.Value, 0))
        ' Rebuilding filter
        Dim nodeHost As TreeNodeHost = Nothing
        Dim foundHost As TreeNodeHost = Nothing
        Dim objs As List(Of Object) = New List(Of Object)
        For Each tnHost As TreeNodeHost In _nodeHosts
            foundHost = tnHost.getHost(subitem.TreeNode)
            If foundHost IsNot Nothing Then nodeHost = foundHost
            tnHost.collectSubItemValue(subItemIndex, objs)
        Next
        If nodeHost Is Nothing Then Return
        _columnControl.reloadFilter(subItemIndex, objs)
        nodeHost.Visible = filterNode(subitem.TreeNode)
        sortAll()
        measureAll()
        relocateAll()
        Me.Invalidate()
    End Sub
    Friend Sub _subitemFontChanged(ByVal subitem As TreeNode.TreeNodeSubItem)
        If subitem.TreeNode.UseNodeStyleForSubItems Then Return
        Dim reloadAll As Boolean = False
        Dim subItemIndex As Integer = subitem.TreeNode.SubItems.IndexOf(subitem) + 1
        If _columns.Count = 0 Or subItemIndex >= _columns.Count Then Return
        If _columns(subItemIndex).SizeType = ColumnSizeType.Auto Then
            _columnControl.relocateHosts(IIf(_hScroll.Visible, -_hScroll.Value, 0))
            reloadAll = True
        End If
        Dim foundHost As TreeNodeHost = Nothing
        For Each tnHost As TreeNodeHost In _nodeHosts
            foundHost = tnHost.getHost(subitem.TreeNode)
            If foundHost IsNot Nothing Then Exit For
        Next
        If foundHost Is Nothing Then Return
        measureAll()
        relocateAll()
        Me.Invalidate()
    End Sub
    Friend Sub _subitemBackColorChanged(ByVal subitem As TreeNode.TreeNodeSubItem)
        If subitem.TreeNode.UseNodeStyleForSubItems Then Return
        _nodeBackColorChanged(subitem.TreeNode)
    End Sub
    Friend Sub _subitemColorChanged(ByVal subitem As TreeNode.TreeNodeSubItem)
        If subitem.TreeNode.UseNodeStyleForSubItems Then Return
        _nodeColorChanged(subitem.TreeNode)
    End Sub
    Friend Sub _subitemPrintValueOnBarChanged(ByVal subitem As TreeNode.TreeNodeSubItem)
        _nodeColorChanged(subitem.TreeNode)
    End Sub
#End Region
#Region "Public Properties."
    <EditorBrowsable(EditorBrowsableState.Never), Browsable(False)> _
    Public Shadows Property Padding() As Padding
        Get
            Return MyBase.Padding
        End Get
        Set(ByVal value As Padding)
            MyBase.Padding = value
        End Set
    End Property
    <EditorBrowsable(EditorBrowsableState.Never), Browsable(False)> _
    Public Shadows Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
        End Set
    End Property
    <DefaultValue(GetType(Drawing.Color), "White")> _
    Public Shadows Property BackColor() As Drawing.Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As Drawing.Color)
            If MyBase.BackColor <> value Then
                MyBase.BackColor = value
                Me.Invalidate()
            End If
        End Set
    End Property
    ' Behavior
    <Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
        Description("Gets the collection of ColumnHeader that are assigned to the MultiColumnTree control")> _
    Public ReadOnly Property Columns() As ColumnHeaderCollection
        Get
            Return _columns
        End Get
    End Property
    <Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
        Description("Gets the collection of TreeNode that are assigned to the MultiColumnTree control")> _
    Public ReadOnly Property Nodes() As TreeNodeCollection
        Get
            Return _nodes
        End Get
    End Property
    <Category("Behavior"), Browsable(False), _
        Description("Gets the collection of checked TreeNode that are assigned to the MultiColumnTree control")> _
    Public ReadOnly Property CheckedNodes() As CheckedTreeNodeCollection
        Get
            Return _checkedNodes
        End Get
    End Property
    <Category("Behavior"), DefaultValue(False), _
        Description("Determine whether the user can edit the labels of nodes in the control.")> _
    Public Property LabelEdit() As Boolean
        Get
            Return _labelEdit
        End Get
        Set(ByVal value As Boolean)
            _labelEdit = value
        End Set
    End Property
    <Category("Behavior"), Description("Determine culture info used to display values.")> _
    Public Property Culture() As CultureInfo
        Get
            Return _ci
        End Get
        Set(ByVal value As CultureInfo)
            If _ci IsNot value Then
                If value Is Nothing Then
                    _ci = Renderer.Drawing.en_us_ci
                Else
                    _ci = value
                End If
                _columnControl.relocateHosts(IIf(_hScroll.Visible, -_hScroll.Value, 0))
                measureAll()
                relocateAll()
                Me.Invalidate(True)
            End If
        End Set
    End Property
    <Category("Behavior"), DefaultValue(True), _
        Description("")> _
    Public Property NodeToolTip() As Boolean
        Get
            Return _nodeToolTip
        End Get
        Set(ByVal value As Boolean)
            _nodeToolTip = value
        End Set
    End Property
    ' Appearance
    <Category("Appearance"), DefaultValue(False), _
        Description("Determine a check box appears next to each node in the control.")> _
    Public Property CheckBoxes() As Boolean
        Get
            Return _checkBoxes
        End Get
        Set(ByVal value As Boolean)
            If _checkBoxes <> value Then
                _checkBoxes = value
                _columnControl.relocateHosts(IIf(_hScroll.Visible, -_hScroll.Value, 0))
                measureAll()
                relocateAll()
                Me.Invalidate(True)
            End If
        End Set
    End Property
    <Category("Appearance"), DefaultValue(True), _
        Description("Determine a dropdown options is shown in the column header.")> _
    Public Property ShowColumnOptions() As Boolean
        Get
            Return _showColumnOptions
        End Get
        Set(ByVal value As Boolean)
            If _showColumnOptions <> value Then
                _showColumnOptions = value
                _columnControl.relocateHosts(IIf(_hScroll.Visible, -_hScroll.Value, 0))
            End If
        End Set
    End Property
    <Category("Appearance"), DefaultValue(False), _
        Description("Determine whether clicking an item selects all its subitems.")> _
    Public Property FullRowSelect() As Boolean
        Get
            Return _fullRowSelect
        End Get
        Set(ByVal value As Boolean)
            If _fullRowSelect <> value Then
                _fullRowSelect = value
                Me.Invalidate()
            End If
        End Set
    End Property
    <Category("Appearance"), DefaultValue(False), _
        Description("Determine whether multiline node's text and subitem's value is allowed.")> _
    Public Property AllowMultiline() As Boolean
        Get
            Return _allowMultiline
        End Get
        Set(ByVal value As Boolean)
            If _allowMultiline <> value Then
                _allowMultiline = value
                measureAll()
                relocateAll()
                Me.Invalidate()
            End If
        End Set
    End Property
    <Category("Appearance"), DefaultValue("\"), _
        Description("Determine the delimiter string that the tree node path uses.")> _
    Public Property PathSeparator() As String
        Get
            Return _pathSeparator
        End Get
        Set(ByVal value As String)
            _pathSeparator = value
        End Set
    End Property
    <Category("Appearance"), DefaultValue(True), _
        Description("Determine whether Images are shown on each TreeNode.")> _
    Public Property ShowImages() As Boolean
        Get
            Return _showImages
        End Get
        Set(ByVal value As Boolean)
            If _showImages <> value Then
                _showImages = value
                measureAll()
                relocateAll()
                Me.Invalidate()
            End If
        End Set
    End Property
    <Category("Appearance"), DefaultValue(20), _
        Description("Determine the distance to indent each of the child tree node levels.")> _
    Public Property Indent() As Integer
        Get
            Return _showImages
        End Get
        Set(ByVal value As Integer)
            If _indent <> value And value > _mininumIndent Then
                _indent = value
                measureAll()
                relocateAll()
                Me.Invalidate()
            End If
        End Set
    End Property
    <Browsable(False), _
        Description("Gets or sets the tree node that is currently selected in the tree view control.")> _
    Public Property SelectedNode() As TreeNode
        Get
            If _selectedHost Is Nothing Then
                Return Nothing
            Else
                Return _selectedHost.Node
            End If
        End Get
        Set(ByVal value As TreeNode)
            If _selectedHost IsNot Nothing Then
                If _selectedHost.Node Is value Then Return
            End If
            Dim foundHost As TreeNodeHost = getNodeHost(value)
            If foundHost IsNot Nothing Then
                Dim beforeSelect As TreeNodeEventArgs = New TreeNodeEventArgs(foundHost.Node, TreeNodeAction.Unknown)
                RaiseEvent BeforeSelect(Me, beforeSelect)
                If beforeSelect.Cancel Then Return
                foundHost.expandAllParent()
                setSelectedHost(foundHost)
                ensureVisible(_selectedHost)
                Me.Invalidate()
            Else
                If value Is Nothing Then
                    setSelectedHost(Nothing)
                    Me.Invalidate()
                End If
            End If
        End Set
    End Property
#End Region
#Region "Constructor."
    Public Sub New()
        ' Initialize control styles.
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        Me.SetStyle(ControlStyles.Selectable, True)
        MyBase.BackColor = Color.White
        MyBase.Padding = New Padding(1)
        Me.SuspendLayout()
        ' Setting color blend for column separator
        Dim _linePenColors(0 To 2) As Color
        Dim _linePenPos(0 To 2) As Single
        _linePenColors(0) = Color.FromArgb(0, 158, 187, 221)
        _linePenColors(1) = Color.FromArgb(158, 187, 221)
        _linePenColors(2) = Color.FromArgb(0, 158, 187, 221)
        _linePenPos(0) = 0.0F
        _linePenPos(1) = 0.5F
        _linePenPos(2) = 1.0F
        _linePenBlend = New ColorBlend
        _linePenBlend.Colors = _linePenColors
        _linePenBlend.Positions = _linePenPos
        ' Initialize all objects
        _columns = New ColumnHeaderCollection(Me)
        _columnControl = New ColumnHeaderControl(Me, Renderer.ToolTip.TextFont)
        _nodes = New TreeNodeCollection(Nothing, Me)
        _checkedNodes = New CheckedTreeNodeCollection(Me)
        _txtEditor = New TextBoxLabelEditor
        _txtEditor.Visible = False
        ' Setting up graphics object for text measurement.
        _gObj.SmoothingMode = SmoothingMode.AntiAlias
        _gObj.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        ' Setting up tooltip
        _tooltip = New ToolTip(Me)
        _tooltip.AnimationSpeed = 20
        ' Adding ColumnControl, TextBoxLabelEditor, and ScrollBars
        Me.Controls.Add(_txtEditor)
        Me.Controls.Add(_columnControl)
        _vScroll.Top = _columnControl.Bottom + 1
        _vScroll.Left = _clientArea.Right - _vScroll.Width
        _vScroll.Visible = False
        _hScroll.Left = 1
        _hScroll.Top = _clientArea.Bottom - _hScroll.Height
        _hScroll.Visible = False
        Me.Controls.Add(_vScroll)
        Me.Controls.Add(_hScroll)
        MyBase.Size = New Size(100, 100)
        ' Initialize client area
        measureAll()
        Me.ResumeLayout()
    End Sub
    Protected Overrides Function IsInputKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Select Case keyData
            Case Keys.Up, Keys.Down, Keys.Space, Keys.PageUp, Keys.PageDown, Keys.Home, Keys.End, Keys.F2, Keys.Left, Keys.Right
                Return True
            Case Else
                Return MyBase.IsInputKey(keyData)
        End Select
    End Function
#End Region
#Region "Public Routines."
    ''' <summary>
    ''' Collapses all the tree nodes.
    ''' </summary>
    Public Sub collapseAll()
        _internalThread = True
        For Each tn As TreeNode In _nodes
            tn.collapse(False)
        Next
        _internalThread = False
        measureAll()
        relocateAll()
        Me.Invalidate()
    End Sub
    ''' <summary>
    ''' Expands all the tree nodes.
    ''' </summary>
    Public Sub expandAll()
        _internalThread = True
        For Each tn As TreeNode In _nodes
            tn.expandAll()
        Next
        _internalThread = False
        measureAll()
        relocateAll()
        Me.Invalidate()
    End Sub
    ''' <summary>
    ''' Retrieves the number of tree nodes, optionally including those in all subtrees, assigned to the tree view control.
    ''' </summary>
    Public Function getNodesCount(ByVal includeSubTrees As Boolean) As Integer
        Dim result As Integer = _nodes.Count
        If includeSubTrees Then
            For Each tn As TreeNode In _nodes
                result += tn.getNodeCount(includeSubTrees)
            Next
        End If
        Return result
    End Function
    ''' <summary>
    ''' Retrieves the bounding rectangle of a TreeNode in MultiColumnTree control.
    ''' </summary>
    Public Function getNodeRectangle(ByVal node As TreeNode) As Rectangle
        Dim result As Rectangle = New Rectangle(0, 0, 0, 0)
        If node IsNot Nothing Then
            Dim tnHost As TreeNodeHost = getNodeHost(node)
            If tnHost IsNot Nothing Then result = tnHost.Bounds
        End If
        Return result
    End Function
#End Region
#Region "Event Handlers."
    Private Sub _nodes_AfterClear(ByVal sender As Object, ByVal e As CollectionEventArgs) Handles _nodes.AfterClear
        setSelectedHost(Nothing)
        For Each tnHost As TreeNodeHost In _nodeHosts
            tnHost.removeHandlers()
        Next
        _nodeHosts.Clear()
        _columnControl.clearFilters()
        _vScroll.Visible = False
        _clientArea = New Rectangle(1, _columnControl.Bottom + 1, Me.Width - 2, Me.Height - (_columnControl.Bottom + 1 + IIf(_hScroll.Visible, _hScroll.Height + 1, 0)))
        Me.Invalidate(True)
    End Sub
    Private Sub _nodes_AfterInsert(ByVal sender As Object, ByVal e As CollectionEventArgs) Handles _nodes.AfterInsert
        Dim newNode As TreeNode = DirectCast(e.Item, TreeNode)
        Dim newHost As TreeNodeHost = New TreeNodeHost(newNode, Me, Nothing)
        _nodeHosts.Add(newHost)
        newHost.Visible = filterNode(newNode)
        sortAll()
        _columnControl.relocateHosts(IIf(_hScroll.Visible, -_hScroll.Value, 0))
        measureAll()
        relocateAll()
        Me.Invalidate(True)
    End Sub
    Private Sub _nodes_AfterRemove(ByVal sender As Object, ByVal e As CollectionEventArgs) Handles _nodes.AfterRemove
        Dim remNode As TreeNode = DirectCast(e.Item, TreeNode)
        Dim remHost As TreeNodeHost = Nothing
        For Each tnHost As TreeNodeHost In _nodeHosts
            If tnHost.Node Is remNode Then
                remHost = tnHost
                Exit For
            End If
        Next
        If remHost Is Nothing Then Return
        remHost.removeHandlers()
        If remHost Is _selectedHost Then setSelectedHost(Nothing)
        _columnControl.relocateHosts(IIf(_hScroll.Visible, -_hScroll.Value, 0))
        measureAll()
        relocateAll()
        Me.Invalidate(True)
    End Sub
    Private Sub _hScroll_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _hScroll.ValueChanged
        If Not _hScroll.Visible Then Return
        _columnControl.moveColumns(-_hScroll.Value)
        For Each tnHost As TreeNodeHost In _nodeHosts
            tnHost.X = _clientArea.X - _hScroll.Value
        Next
        Me.Invalidate(True)
    End Sub
    Private Sub _vScroll_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _vScroll.ValueChanged
        If Not _vScroll.Visible Then Return
        If _nodeHosts.Count = 0 Then Return
        Dim lastY As Integer = _nodeHosts(0).Y
        Dim dy As Integer = (_clientArea.Y - _vScroll.Value) - lastY
        For Each tnHost As TreeNodeHost In _nodeHosts
            tnHost.Y += dy
        Next
        If _txtEditor.Visible Then _txtEditor.Top += dy
        Me.Invalidate()
    End Sub
    Private Sub _columnControl_AfterColumnCustomFilter(ByVal sender As Object, ByVal e As ColumnEventArgs) Handles _columnControl.AfterColumnCustomFilter
        Dim cEvent As ColumnCustomFilterEventArgs = New ColumnCustomFilterEventArgs(e.Column)
        RaiseEvent ColumnCustomFilter(Me, cEvent)
        If Not cEvent.CancelFilter Then
            Dim handlers As List(Of ColumnFilterHandle) = _columnControl.FilterHandlers
            For Each tnHost As TreeNodeHost In _nodeHosts
                tnHost.Visible = filterNode(tnHost.Node, handlers)
                If tnHost.Visible Then tnHost.filterChildren(handlers, False)
            Next
            If _selectedHost IsNot Nothing Then
                If Not _selectedHost.Visible Then setSelectedHost(Nothing)
            End If
            measureAll()
            relocateAll()
            Me.Invalidate()
            RaiseEvent ColumnFilterChanged(Me, e)
        End If
    End Sub
    Private Sub _columnControl_AfterColumnFilter(ByVal sender As Object, ByVal e As ColumnEventArgs) Handles _columnControl.AfterColumnFilter
        Dim handlers As List(Of ColumnFilterHandle) = _columnControl.FilterHandlers
        For Each tnHost As TreeNodeHost In _nodeHosts
            tnHost.Visible = filterNode(tnHost.Node, handlers)
            If tnHost.Visible Then tnHost.filterChildren(handlers, False)
        Next
        If _selectedHost IsNot Nothing Then
            If Not _selectedHost.Visible Then setSelectedHost(Nothing)
        End If
        measureAll()
        relocateAll()
        Me.Invalidate()
        RaiseEvent ColumnFilterChanged(Me, e)
    End Sub
    Private Sub _columnControl_AfterColumnResize(ByVal sender As Object, ByVal e As ColumnEventArgs) Handles _columnControl.AfterColumnResize
        measureAll()
        _columnControl.moveColumns(IIf(_hScroll.Visible, -_hScroll.Value, 0))
        relocateAll()
        Me.Invalidate()
        RaiseEvent ColumnSizeChanged(Me, e)
    End Sub
    Private Sub _columnControl_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _columnControl.CheckedChanged
        _internalThread = True
        For Each tn As TreeNode In _nodes
            tn.Checked = _columnControl.CheckState = CheckState.Checked
            'changeChildrenChecked(tn)
        Next
        Me.Invalidate()
        _internalThread = False
    End Sub
    Private Sub _columnControl_ColumnOrderChanged(ByVal sender As Object, ByVal e As ColumnEventArgs) Handles _columnControl.ColumnOrderChanged
        measureAll()
        relocateAll()
        Me.Invalidate()
        RaiseEvent ColumnOrderChanged(Me, e)
    End Sub
    Private Sub _columns_AfterClear(ByVal sender As Object, ByVal e As CollectionEventArgs) Handles _columns.AfterClear
        _hScroll.Visible = False
    End Sub
    Private Sub _columns_AfterInsert(ByVal sender As Object, ByVal e As CollectionEventArgs) Handles _columns.AfterInsert
        measureAll()
        relocateAll()
        Me.Invalidate()
    End Sub
    Private Sub _columns_AfterRemove(ByVal sender As Object, ByVal e As CollectionEventArgs) Handles _columns.AfterRemove
        measureAll()
        relocateAll()
        Me.Invalidate()
    End Sub
    Private Sub _tooltip_Draw(ByVal sender As Object, ByVal e As DrawEventArgs) Handles _tooltip.Draw
        Renderer.ToolTip.drawToolTip(_currentToolTipTitle, _currentToolTip, _
                _currentToolTipImage, e.Graphics, e.Rectangle)
        _currentToolTipTitle = ""
        _currentToolTip = ""
        _currentToolTipImage = Nothing
    End Sub
    Private Sub _tooltip_Popup(ByVal sender As Object, ByVal e As PopupEventArgs) Handles _tooltip.Popup
        e.Size = Renderer.ToolTip.measureSize(_currentToolTipTitle, _currentToolTip, _currentToolTipImage)
    End Sub
    Private Sub _txtEditor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles _txtEditor.KeyDown
        Select Case e.KeyData
            Case Keys.Escape
                Me.Focus()
            Case Keys.Return
                If Not _txtEditor.Multiline Then
                    If _currentEditedHost IsNot Nothing Then
                        If _currentEditedHost.Node.Text <> _txtEditor.Text Then
                            _currentEditedHost.Node.Text = _txtEditor.Text
                            RaiseEvent AfterLabelEdit(Me, New TreeNodeEventArgs(_currentEditedHost.Node, TreeNodeAction.LabelEdit))
                        End If
                    End If
                    Me.Focus()
                End If
            Case Keys.Control Or Keys.Return
                If _txtEditor.Multiline Then
                    If _currentEditedHost IsNot Nothing Then
                        If _currentEditedHost.Node.Text <> _txtEditor.Text Then
                            _currentEditedHost.Node.Text = _txtEditor.Text
                            RaiseEvent AfterLabelEdit(Me, New TreeNodeEventArgs(_currentEditedHost.Node, TreeNodeAction.LabelEdit))
                        End If
                    End If
                    Me.Focus()
                End If
        End Select
    End Sub
    Private Sub _txtEditor_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _txtEditor.LostFocus
        _txtEditor.Visible = False
    End Sub
    Private Sub MultiColumnTree_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        If _gObj IsNot Nothing Then _gObj.Dispose()
        If _gBmp IsNot Nothing Then _gBmp.Dispose()
    End Sub
    Private Sub MultiColumnTree_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.EnabledChanged
        Me.Invalidate()
    End Sub
    Private Sub MultiColumnTree_FontChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.FontChanged
        measureAll()
        relocateAll()
        Me.Invalidate()
    End Sub
    Private Sub MultiColumnTree_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        Me.Invalidate()
    End Sub
    Private Sub MultiColumnTree_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Up
                Dim upperHost As TreeNodeHost = Nothing
                If _selectedHost IsNot Nothing Then
                    upperHost = selectPrevNodeHost()
                Else
                    upperHost = selectFirstNodeHost()
                End If
                If upperHost IsNot Nothing Then
                    Dim bfrSelect As TreeNodeEventArgs = New TreeNodeEventArgs(upperHost.Node, TreeNodeAction.Unknown)
                    RaiseEvent BeforeSelect(Me, bfrSelect)
                    If bfrSelect.Cancel Then Return
                    setSelectedHost(upperHost)
                    ensureVisible(_selectedHost)
                    Me.Invalidate()
                End If
            Case Keys.Down
                Dim lowerHost As TreeNodeHost = Nothing
                If _selectedHost IsNot Nothing Then
                    lowerHost = selectNextNodeHost()
                Else
                    lowerHost = selectFirstNodeHost()
                End If
                If lowerHost IsNot Nothing Then
                    Dim bfrSelect As TreeNodeEventArgs = New TreeNodeEventArgs(lowerHost.Node, TreeNodeAction.Unknown)
                    RaiseEvent BeforeSelect(Me, bfrSelect)
                    If bfrSelect.Cancel Then Return
                    setSelectedHost(lowerHost)
                    ensureVisible(_selectedHost)
                    Me.Invalidate()
                End If
            Case Keys.Left
                If _selectedHost IsNot Nothing Then
                    If _selectedHost.getVisibleChildCount > 0 And _selectedHost.Node.IsExpanded Then
                        _selectedHost.Node.collapse()
                    Else
                        If _selectedHost.ParentHost IsNot Nothing Then
                            Dim bfrSelect As TreeNodeEventArgs = New TreeNodeEventArgs(_selectedHost.ParentHost.Node, TreeNodeAction.Unknown)
                            RaiseEvent BeforeSelect(Me, bfrSelect)
                            If bfrSelect.Cancel Then Return
                            setSelectedHost(_selectedHost.ParentHost)
                            ensureVisible(_selectedHost)
                            Me.Invalidate()
                        End If
                    End If
                Else
                    Dim firstHost As TreeNodeHost = selectFirstNodeHost()
                    If firstHost IsNot Nothing Then
                        Dim bfrSelect As TreeNodeEventArgs = New TreeNodeEventArgs(firstHost.Node, TreeNodeAction.Unknown)
                        RaiseEvent BeforeSelect(Me, bfrSelect)
                        If bfrSelect.Cancel Then Return
                        setSelectedHost(firstHost)
                        If _selectedHost IsNot Nothing Then
                            ensureVisible(_selectedHost)
                            Me.Invalidate()
                        End If
                    End If
                End If
            Case Keys.Right
                If _selectedHost IsNot Nothing Then
                    If _selectedHost.getVisibleChildCount > 0 And Not _selectedHost.Node.IsExpanded Then
                        _selectedHost.Node.expand()
                    Else
                        If _selectedHost.getVisibleChildCount > 0 Then
                            Dim nextHost As TreeNodeHost = _selectedHost.getNextHost(_selectedHost)
                            Dim bfrSelect As TreeNodeEventArgs = New TreeNodeEventArgs(nextHost.Node, TreeNodeAction.Unknown)
                            RaiseEvent BeforeSelect(Me, bfrSelect)
                            If bfrSelect.Cancel Then Return
                            setSelectedHost(nextHost)
                            ensureVisible(_selectedHost)
                            Me.Invalidate()
                        End If
                    End If
                Else
                    Dim firstHost As TreeNodeHost = selectFirstNodeHost()
                    If firstHost IsNot Nothing Then
                        Dim bfrSelect As TreeNodeEventArgs = New TreeNodeEventArgs(firstHost.Node, TreeNodeAction.Unknown)
                        RaiseEvent BeforeSelect(Me, bfrSelect)
                        If bfrSelect.Cancel Then Return
                        setSelectedHost(firstHost)
                        If _selectedHost IsNot Nothing Then
                            ensureVisible(_selectedHost)
                            Me.Invalidate()
                        End If
                    End If
                End If
            Case Keys.Home
                Dim firstHost As TreeNodeHost = selectFirstNodeHost()
                If firstHost IsNot Nothing Then
                    Dim bfrSelect As TreeNodeEventArgs = New TreeNodeEventArgs(firstHost.Node, TreeNodeAction.Unknown)
                    RaiseEvent BeforeSelect(Me, bfrSelect)
                    If bfrSelect.Cancel Then Return
                    setSelectedHost(firstHost)
                    If _selectedHost IsNot Nothing Then
                        ensureVisible(_selectedHost)
                        Me.Invalidate()
                    End If
                End If
            Case Keys.End
                Dim lastHost As TreeNodeHost = selectFirstNodeHost()
                If lastHost IsNot Nothing Then
                    Dim bfrSelect As TreeNodeEventArgs = New TreeNodeEventArgs(lastHost.Node, TreeNodeAction.Unknown)
                    RaiseEvent BeforeSelect(Me, bfrSelect)
                    If bfrSelect.Cancel Then Return
                    setSelectedHost(lastHost)
                    If _selectedHost IsNot Nothing Then
                        ensureVisible(_selectedHost)
                        Me.Invalidate()
                    End If
                End If
            Case Keys.F2
                If Not _labelEdit Then Return
                If _selectedHost Is Nothing Then Return
                Dim lblEditEvent As TreeNodeEventArgs = New TreeNodeEventArgs(_selectedHost.Node, TreeNodeAction.LabelEdit)
                RaiseEvent BeforeLabelEdit(Me, lblEditEvent)
                If lblEditEvent.Cancel Then Return
                ensureVisible(_selectedHost)
                Me.Invalidate()
                showTextBoxEditor(_selectedHost)
        End Select
    End Sub
    Private Sub MultiColumnTree_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Dim nextHost As TreeNodeHost = selectNextNodeHost(e.KeyChar.ToString)
        If nextHost IsNot Nothing And nextHost IsNot _selectedHost Then
            Dim bfrSelect As TreeNodeEventArgs = New TreeNodeEventArgs(nextHost.Node, TreeNodeAction.Unknown)
            RaiseEvent BeforeSelect(Me, bfrSelect)
            If bfrSelect.Cancel Then Return
            setSelectedHost(nextHost)
            ensureVisible(_selectedHost)
            Me.Invalidate()
        End If
    End Sub
    Private Sub MultiColumnTree_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        Me.Invalidate()
    End Sub
    Private Sub MultiColumnTree_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        _tooltip.hide()
        Me.Focus()
        Dim stateChanged As Boolean = False
        For Each tnHost As TreeNodeHost In _nodeHosts
            If tnHost.Visible Then stateChanged = stateChanged Or tnHost.mouseDown(e)
        Next
        If stateChanged Then Me.Invalidate()
    End Sub
    Private Sub MultiColumnTree_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        _tooltip.hide()
        Dim stateChanged As Boolean = False
        For Each tnHost As TreeNodeHost In _nodeHosts
            If tnHost.Visible Then stateChanged = stateChanged Or tnHost.mouseLeave
        Next
        If stateChanged Then Me.Invalidate()
    End Sub
    Private Sub MultiColumnTree_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        Dim stateChanged As Boolean = False
        For Each tnHost As TreeNodeHost In _nodeHosts
            If tnHost.Visible Then stateChanged = stateChanged Or tnHost.mouseMove(e)
        Next
        If stateChanged Then Me.Invalidate()
        If _needToolTip Then
            _tooltip.show(Me, _currentToolTipRect)
            _needToolTip = False
        End If
    End Sub
    Private Sub MultiColumnTree_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        Dim stateChanged As Boolean = False
        For Each tnHost As TreeNodeHost In _nodeHosts
            If tnHost.Visible Then stateChanged = stateChanged Or tnHost.mouseUp(e)
        Next
        If stateChanged Then Me.Invalidate()
    End Sub
    Private Sub MultiColumnTree_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        Dim _next As Boolean = True
        If e.Delta > 0 Then
            If _vScroll.Visible And _vScroll.Value > 0 Then
                If _vScroll.Value >= _vScroll.SmallChange Then
                    _vScroll.Value -= _vScroll.SmallChange
                Else
                    _vScroll.Value = 0
                End If
                Return
            End If
            If _hScroll.Visible And _hScroll.Value > 0 Then
                If _hScroll.Value >= _hScroll.SmallChange Then
                    _hScroll.Value -= _hScroll.SmallChange
                Else
                    _hScroll.Value = 0
                End If
            End If
        ElseIf e.Delta < 0 Then
            If _vScroll.Visible And _vScroll.Value < _vScroll.Maximum Then
                If _vScroll.Value <= _vScroll.Maximum - _vScroll.SmallChange Then
                    _vScroll.Value += _vScroll.SmallChange
                Else
                    _vScroll.Value = _vScroll.Maximum
                End If
                Return
            End If
            If _hScroll.Visible And _hScroll.Value < _hScroll.Maximum Then
                If _hScroll.Value <= _hScroll.Maximum - _hScroll.SmallChange Then
                    _hScroll.Value += _hScroll.SmallChange
                Else
                    _hScroll.Value = _hScroll.Maximum
                End If
            End If
        End If
    End Sub
    Private Sub MultiColumnTree_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        e.Graphics.Clear(Color.White)
        ' Create separator pen for each column.
        Dim lineBrush As LinearGradientBrush = New LinearGradientBrush(_clientArea, _
            Color.Black, Color.White, LinearGradientMode.Vertical)
        lineBrush.InterpolationColors = _linePenBlend
        Dim linePen As Pen = New Pen(lineBrush)
        ' Paint unfrozen columns
        Dim unfrozenCols As List(Of ColumnHeader) = _columnControl.UnFrozenColumns
        Dim frozenCols As List(Of ColumnHeader) = _columnControl.FrozenColumns
        Dim colRect As Rectangle
        For Each ch As ColumnHeader In unfrozenCols
            colRect = _columnControl.ColumnRectangle(ch)
            colRect.Y = _clientArea.Y
            colRect.Height = _clientArea.Height
            Dim pe As ColumnBackgroundPaintEventArgs = _
                New ColumnBackgroundPaintEventArgs(ch, _columns.IndexOf(ch), e.Graphics, colRect)
            RaiseEvent ColumnBackgroundPaint(Me, pe)
            e.Graphics.DrawLine(linePen, colRect.Right, colRect.Y, colRect.Right, colRect.Bottom)
        Next
        For Each tnHost As TreeNodeHost In _nodeHosts
            tnHost.drawUnFrozen(e.Graphics, frozenCols.Count)
        Next
        If frozenCols.Count > 0 Then
            colRect = _columnControl.FrozenRectangle
            colRect.Y = 0
            colRect.Height = Me.Height
            colRect.Width += colRect.X
            colRect.X = 0
            e.Graphics.FillRectangle(Brushes.White, colRect)
            For Each ch As ColumnHeader In frozenCols
                colRect = _columnControl.ColumnRectangle(ch)
                colRect.Y = _clientArea.Y
                colRect.Height = _clientArea.Height
                Dim pe As ColumnBackgroundPaintEventArgs = _
                    New ColumnBackgroundPaintEventArgs(ch, _columns.IndexOf(ch), e.Graphics, colRect)
                RaiseEvent ColumnBackgroundPaint(Me, pe)
                e.Graphics.DrawLine(linePen, colRect.Right, colRect.Y, colRect.Right, colRect.Bottom)
            Next
            For Each tnHost As TreeNodeHost In _nodeHosts
                tnHost.drawFrozen(e.Graphics, unfrozenCols.Count)
            Next
        End If
        linePen.Dispose()
        lineBrush.Dispose()
        If _hScroll.Visible And _vScroll.Visible Then
            Dim aRect As Rectangle = New Rectangle(_vScroll.Left - 1, _hScroll.Top - 1, _vScroll.Width + 1, _hScroll.Height + 1)
            e.Graphics.FillRectangle(Brushes.Gainsboro, aRect)
        End If
        e.Graphics.DrawRectangle(Pens.LightGray, 0, 0, Me.Width - 1, Me.Height - 1)
        If _columnControl.OnColumnResize Then _
            e.Graphics.DrawLine(Pens.Black, _columnControl.ResizeCurrentX, _
                0, _columnControl.ResizeCurrentX, Me.Height)
    End Sub
    Private Sub MultiColumnTree_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        _columnControl.relocateHosts()
        measureAll()
        _columnControl.moveColumns(IIf(_hScroll.Visible, -_hScroll.Value, 0))
        relocateAll()
        Me.Invalidate(True)
    End Sub
#End Region
End Class
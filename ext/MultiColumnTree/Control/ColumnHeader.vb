Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Globalization
''' <summary>
''' A class to represent column header of a ListView or MultiColumnTree control.
''' </summary>
''' <remarks>
''' Each column can have specific format to display value of the related subitem, but column 0 formating will always be ignored.
''' Text alignment for column header and text alignment for displayed value of the related subitem can be different, 
''' for column header determined by TextAlign property of ColumnHeader class, and for displayed value determined by ColumnAlign property of ColumnHeader class.
''' Each column can have sort and filter capabilities, specified by EnableSorting and SortOrder for sort capability, and EnableFiltering for filter capability.
''' Each column can be hidden or freezed, specified by EnableHidden and Visible for visibility, and EnableFreezed and Freezed.
''' Freezed column will be shown first, and top of the unfreezed column.
''' Freezed column cannot be moved, or as a movement target of the other column, but still can be resized.
''' An additional option placed at the right most of the column header, to change visibility and freezed for each column on runtime.
''' </remarks>
Public Class ColumnHeader
#Region "Declaration"
    Dim _sortOrder As SortOrder = SortOrder.None
    Dim _format As ColumnFormat = ColumnFormat.None
    Dim _sizeType As ColumnSizeType = ColumnSizeType.Fixed
    Dim _width As Integer = 75
    Dim _text As String = "Column"
    Dim _textAlign As HorizontalAlignment = HorizontalAlignment.Left
    Dim _columnAlign As HorizontalAlignment = HorizontalAlignment.Left
    Dim _customFormat As String = "#"
    Dim _name As String = "ColumnHeader"
    Dim _image As Image = Nothing
    Dim _tag As Object = Nothing
    Dim _tooltip As String = ""
    Dim _tooltiptitle As String = ""
    Dim _tooltipimage As Image = Nothing
    Dim _enableSorting As Boolean = False
    Dim _enableFiltering As Boolean = False
    Dim _enableFrozen As Boolean = False
    Dim _enableHidden As Boolean = False
    Dim _visible As Boolean = True
    Dim _frozen As Boolean = False
    Dim _maxValue As Double = 0
    Dim _minValue As Double = 0
    Dim _enableCustomFilter As Boolean = False
    Dim _customFilter As CustomFilterFunction = Nothing
    Friend _owner As Object
    Friend _displayIndex As Integer = 0
#End Region
#Region "Constructor"
    Public Sub New()
        _owner = Nothing
    End Sub
    'Public Sub New(ByVal owner As ListView)
    '    _owner = owner
    'End Sub
    Public Sub New(ByVal owner As MultiColumnTree)
        _owner = owner
    End Sub
#End Region
#Region "Friend Events"
    Friend Event EnableSortingChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event EnableFilteringChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event EnableFrozenChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event EnableHiddenChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event SortOrderChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event FormatChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event CustomFormatChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event ColumnAlignChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event WidthChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event SizeTypeChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event TextChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event ImageChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event VisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event FrozenChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event TextAlignChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event MaximumValueChanged(ByVal sender As Object, ByVal e As EventArgs)
    Friend Event MinimumValueChanged(ByVal sender As Object, ByVal e As EventArgs)
#End Region
#Region "Public Properties"
    ' Behaviors
    <DefaultValue(False), Category("Behavior"), _
        Description("Determine whether the Column can perform sort operation.")> _
    Public Property EnableSorting() As Boolean
        Get
            Return _enableSorting
        End Get
        Set(ByVal value As Boolean)
            If _enableSorting <> value Then
                _enableSorting = value
                RaiseEvent EnableSortingChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue(False), Category("Behavior"), _
        Description("Determine whether the Column can perform filter operation.")> _
    Public Property EnableFiltering() As Boolean
        Get
            Return _enableFiltering
        End Get
        Set(ByVal value As Boolean)
            If _enableFiltering <> value Then
                _enableFiltering = value
                RaiseEvent EnableFilteringChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue(False), Category("Behavior"), _
        Description("Determine whether the Column can be freezed.")> _
    Public Property EnableFrozen() As Boolean
        Get
            Return _enableFrozen
        End Get
        Set(ByVal value As Boolean)
            If value And _sizeType = ColumnSizeType.Fill Then Return
            If _enableFrozen <> value Then
                _enableFrozen = value
                RaiseEvent EnableFrozenChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue(False), Category("Behavior"), _
        Description("Determine whether the Column can be hidden.")> _
    Public Property EnableHidden() As Boolean
        Get
            Return _enableHidden
        End Get
        Set(ByVal value As Boolean)
            If _enableHidden <> value Then
                _enableHidden = value
                RaiseEvent EnableHiddenChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue(GetType(SortOrder), "None"), Category("Behavior"), _
        Description("Specifies a how the items in the ListView will be sorted based on this Column.")> _
    Public Property SortOrder() As SortOrder
        Get
            Return _sortOrder
        End Get
        Set(ByVal value As SortOrder)
            If _sortOrder <> value Then
                _sortOrder = value
                RaiseEvent SortOrderChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue(GetType(ColumnFormat), "None"), Category("Behavior"), _
        Description("Specifies how the value or the SubItem in a ListViewItem will be formatted.")> _
    Public Property Format() As ColumnFormat
        Get
            Return _format
        End Get
        Set(ByVal value As ColumnFormat)
            If _format <> value Then
                _format = value
                RaiseEvent FormatChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue("#"), Category("Behavior"), _
        Description("Specifies how the value or the SubItem in a ListViewItem will be formatted using this format.")> _
    Public Property CustomFormat() As String
        Get
            Return _customFormat
        End Get
        Set(ByVal value As String)
            If _customFormat <> value Then
                _customFormat = value
                RaiseEvent CustomFormatChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue(GetType(HorizontalAlignment), "Left"), Category("Behavior"), _
        Description("Specifies how a SubItem on a ListViewItem will be aligned.")> _
    Public Property ColumnAlign() As HorizontalAlignment
        Get
            Return _columnAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            If _columnAlign <> value Then
                _columnAlign = value
                RaiseEvent ColumnAlignChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <Category("Behavior"), DefaultValue(False), _
        Description("")> _
    Public Property EnableCustomFilter() As Boolean
        Get
            Return _enableCustomFilter
        End Get
        Set(ByVal value As Boolean)
            _enableCustomFilter = value
        End Set
    End Property
    ' Appearances
    <DefaultValue(75), Category("Appearance"), Description("Determine width of the Column.")> _
    Public Property Width() As Integer
        Get
            Return _width
        End Get
        Set(ByVal value As Integer)
            If _width <> value Then
                If _sizeType = ColumnSizeType.Fixed And value < 25 Then Return
                _width = value
                RaiseEvent WidthChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue(GetType(ColumnSizeType), "Fixed"), Category("Appearance"), _
        Description("Determine how the Column width should be calculated.")> _
    Public Property SizeType() As ColumnSizeType
        Get
            Return _sizeType
        End Get
        Set(ByVal value As ColumnSizeType)
            If _sizeType <> value Then
                If _width < 25 And value = ColumnSizeType.Fixed Then Return
                _sizeType = value
                If _sizeType = ColumnSizeType.Fill Then
                    _enableFrozen = False
                    _frozen = False
                End If
                RaiseEvent SizeTypeChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue("Column"), Category("Appearance"), _
        Description("Determine the text displayed in the Column header.")> _
    Public Property Text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            If _text <> value Then
                _text = value
                RaiseEvent TextChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue(GetType(Image), "Nothing"), Category("Appearance"), _
        Description("Determine the image displayed in the Column header.")> _
    Public Property Image() As Image
        Get
            Return _image
        End Get
        Set(ByVal value As Image)
            If _image IsNot value Then
                _image = value
                RaiseEvent ImageChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue(True), Category("Appearance"), _
        Description("Determine whether the Column is displayed in the ListView control.")> _
    Public Property Visible() As Boolean
        Get
            Return _visible
        End Get
        Set(ByVal value As Boolean)
            If _visible <> value Then
                _visible = value
                RaiseEvent VisibleChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue(False), Category("Appearance"), _
        Description("Determine wherther the Column should be freeze in ListView control.")> _
    Public Property Frozen() As Boolean
        Get
            Return _frozen
        End Get
        Set(ByVal value As Boolean)
            If _enableFrozen And _sizeType <> ColumnSizeType.Fill Then
                If _frozen <> value Then
                    _frozen = value
                    RaiseEvent FrozenChanged(Me, New EventArgs)
                End If
            End If
        End Set
    End Property
    <DefaultValue(GetType(HorizontalAlignment), "Left"), Category("Appearance"), _
        Description("Determine how the text of the Column should be aligned.")> _
    Public Property TextAlign() As HorizontalAlignment
        Get
            Return _textAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            If _textAlign <> value Then
                _textAlign = value
                RaiseEvent TextAlignChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    ' Designs
    <DefaultValue("ColumnHeader"), Category("Design"), _
        Description("Determine the name of the Column.")> _
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
        Description("Determine the contents of the ToolTip should be displayed when mouse hover the Column.")> _
    Public Property ToolTip() As String
        Get
            Return _tooltip
        End Get
        Set(ByVal value As String)
            _tooltip = value
        End Set
    End Property
    <DefaultValue(""), Category("Design"), _
        Description("Determine the title of the ToolTip should be displayed when mouse hover the Column.")> _
    Public Property ToolTipTitle() As String
        Get
            Return _tooltiptitle
        End Get
        Set(ByVal value As String)
            _tooltiptitle = value
        End Set
    End Property
    <DefaultValue(GetType(Drawing.Image), "Nothing"), Category("Design"), _
        Description("Determine the image of the ToolTip should be displayed when mouse hover the Column.")> _
    Public Property ToolTipImage() As Drawing.Image
        Get
            Return _tooltipimage
        End Get
        Set(ByVal value As Drawing.Image)
            _tooltipimage = value
        End Set
    End Property
    <Browsable(False)> _
    Public ReadOnly Property Owner() As Object
        Get
            Return _owner
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property DisplayIndex() As Integer
        Get
            Return _displayIndex
        End Get
    End Property
    ' Data
    <DefaultValue(""), Category("Data"), _
        Description("Determine an Object data associated with the Column."), _
        TypeConverter(GetType(StringConverter))> _
    Public Property Tag() As Object
        Get
            Return _tag
        End Get
        Set(ByVal value As Object)
            _tag = value
        End Set
    End Property
    <DefaultValue(0), Category("Data"), _
        Description("Determine the maximum value for the column.  This property is used when the Format property is set to Bar.")> _
    Public Property MaximumValue() As Double
        Get
            Return _maxValue
        End Get
        Set(ByVal value As Double)
            If _maxValue <> value Then
                _maxValue = value
                RaiseEvent MaximumValueChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue(0), Category("Data"), _
        Description("Determine the minimum value for the column.  This property is used when the Format property is set to Bar.")> _
    Public Property MinimumValue() As Double
        Get
            Return _minValue
        End Get
        Set(ByVal value As Double)
            If _minValue <> value Then
                _minValue = value
                RaiseEvent MinimumValueChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <Category("Data"), Browsable(False), _
        Description("Provide a function used to filter a value when using custom filtering operation.")> _
    Public Property CustomFilter() As CustomFilterFunction
        Get
            Return _customFilter
        End Get
        Set(ByVal value As CustomFilterFunction)
            _customFilter = value
        End Set
    End Property
#End Region
#Region "Public Delegate Methods"
    Public Delegate Function CustomFilterFunction(ByVal value As Object) As Boolean
#End Region
End Class
<Description("Predefined format to display the value of a list item.")> _
Public Enum ColumnFormat
    ' Date Time data type format.
    ' Custom format.
    CustomDateTime
    ' Standard date time format.
    ShortDate
    LongDate
    FullDateShortTime
    FullDateLongTime
    GeneralDateShortTime
    GeneralDateLongTime
    RoundTripDateTime
    RFC1123
    SortableDateTime
    ShortTime
    LongTime
    UniversalSortableDateTime
    UniversalFullDateTime
    ' Numeric data type format.
    ' Custom format.
    Bar
    Custom
    ' Standard numeric format.
    Currency
    DecimalNumber
    Exponential
    FixedPoint
    General
    Number
    Percent
    RoundTrip
    HexaDecimal
    ' Other data type, including string data type.
    None
    Password
End Enum
<Description("Define how a column width on a list will be calculated.")> _
Public Enum ColumnSizeType
    Auto
    Fixed
    Percentage
    Fill
End Enum
#Region "Classes used for filtering purposes only, inside Ai.Control namespace."
Friend Enum FilterChooserResult
    OK
    Cancel
    Custom
End Enum
<Description("Determine how the comparison method for filtering operation should be performed.")> _
Friend Enum FilterMode
    ByRange
    ByValue
End Enum
''' <summary>
''' Enumeration to determine how the value must be compared with the filter parameters.
''' </summary>
''' <remarks>For the string type, if ContainsAny, ContainsAll, or NotContain is specified, the delimiter used to separate the words is comma and space.</remarks>
<Description("Enumeration to determine how the value must be compared with the filter parameters.")> _
Public Enum FilterRangeMode
    ' Numeric and DateTime data type.
    Between = 0
    Outside = 1
    ' String data type.
    StartsWith = 0
    EndsWith = 1
    ContainsAll = 2
    ContainsAny = 3
    NotContain = 4
    ' Image only data type, for future implementation.
    Equal = 3
End Enum
<Description("Class to represent a filter item of a ColumnHeader.")> _
Friend Class ColumnFilterItem
    Implements IComparable
    Dim _owner As ColumnFilterHandle
    Dim _value As Object
    Dim _selected As Boolean = True
    Public Sub New(ByVal owner As ColumnFilterHandle)
        _owner = owner
        _value = Nothing
    End Sub
    Public Sub New(ByVal owner As ColumnFilterHandle, ByVal value As Object)
        _owner = owner
        _value = value
    End Sub
    Public Property Selected() As Boolean
        Get
            Return _selected
        End Get
        Set(ByVal value As Boolean)
            _selected = value
        End Set
    End Property
    Public Property Value() As Object
        Get
            Return _value
        End Get
        Set(ByVal value As Object)
            _value = value
        End Set
    End Property
    Public ReadOnly Property Owner() As ColumnFilterHandle
        Get
            Return _owner
        End Get
    End Property
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Dim i2 As ColumnFilterItem = DirectCast(obj, ColumnFilterItem)
        Select Case _owner.DisplayFormat
            Case ColumnFormat.Bar, ColumnFormat.Currency, _
                ColumnFormat.Custom, ColumnFormat.Exponential, _
                ColumnFormat.FixedPoint, ColumnFormat.General, _
                ColumnFormat.HexaDecimal, ColumnFormat.Number, _
                ColumnFormat.Percent, ColumnFormat.RoundTrip
                Dim d1 As Double = _value
                Dim d2 As Double = i2._value
                Return d1.CompareTo(d2)
            Case ColumnFormat.DecimalNumber
                Dim d1 As Integer = _value
                Dim d2 As Integer = i2._value
                Return d1.CompareTo(d2)
            Case ColumnFormat.None, ColumnFormat.Password
                Return StrComp(_value, i2._value, CompareMethod.Text)
            Case Else
                Dim d1 As Date = _value
                Dim d2 As Date = i2._value
                Return Date.Compare(d1, d2)
        End Select
    End Function
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If TypeOf (obj) Is ColumnFilterItem Then
            Dim i2 As ColumnFilterItem = DirectCast(obj, ColumnFilterItem)
            Select Case _owner.DisplayFormat
                Case ColumnFormat.Bar, ColumnFormat.Currency, _
                ColumnFormat.Custom, ColumnFormat.Exponential, _
                ColumnFormat.FixedPoint, ColumnFormat.General, _
                ColumnFormat.HexaDecimal, ColumnFormat.Number, _
                ColumnFormat.Percent, ColumnFormat.RoundTrip
                    Dim d1 As Double = _value
                    Dim d2 As Double = i2._value
                    Return d1.CompareTo(d2) = 0
                Case ColumnFormat.DecimalNumber
                    Dim d1 As Integer = _value
                    Dim d2 As Integer = i2._value
                    Return d1.CompareTo(d2) = 0
                Case ColumnFormat.None, ColumnFormat.Password
                    Return StrComp(_value, i2._value, CompareMethod.Text) = 0
                Case Else
                    Dim d1 As Date = _value
                    Dim d2 As Date = i2._value
                    Return Date.Compare(d1, d2) = 0
            End Select
        End If
        Return False
    End Function
End Class
<Description("Class to handle filtering operaton for a ColumnHeader.")> _
Friend Class ColumnFilterHandle
    Dim _column As ColumnHeader
    Dim _items As List(Of ColumnFilterItem) = New List(Of ColumnFilterItem)
    Dim _minValue As Object = ""
    Dim _maxValue As Object = ""
    Dim _useCustomFilter As Boolean = False
    Dim _filterMode As FilterMode = FilterMode.ByValue
    Dim _rangeMode As FilterRangeMode = FilterRangeMode.Between
    Public Sub New(ByVal column As ColumnHeader)
        _column = column
    End Sub
    Public Property MaxValueSelected() As Object
        Get
            Return _maxValue
        End Get
        Set(ByVal value As Object)
            _maxValue = value
        End Set
    End Property
    Public Property MinValueSelected() As Object
        Get
            Return _minValue
        End Get
        Set(ByVal value As Object)
            _minValue = value
        End Set
    End Property
    Public Property FilterMode() As FilterMode
        Get
            Return _filterMode
        End Get
        Set(ByVal value As FilterMode)
            _filterMode = value
        End Set
    End Property
    Public Property RangeMode() As FilterRangeMode
        Get
            Return _rangeMode
        End Get
        Set(ByVal value As FilterRangeMode)
            _rangeMode = value
        End Set
    End Property
    Public Property UseCustomFilter() As Boolean
        Get
            Return _useCustomFilter
        End Get
        Set(ByVal value As Boolean)
            _useCustomFilter = value
        End Set
    End Property
    Public ReadOnly Property MinValue() As Object
        Get
            If _items.Count > 0 Then
                Return _items(0).Value
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property MaxValue() As Object
        Get
            If _items.Count > 0 Then
                Return _items(_items.Count - 1).Value
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property Column() As ColumnHeader
        Get
            Return _column
        End Get
    End Property
    Public ReadOnly Property Items() As List(Of ColumnFilterItem)
        Get
            Return _items
        End Get
    End Property
    Public ReadOnly Property DisplayFormat() As ColumnFormat
        Get
            Return _column.Format
        End Get
    End Property
    Public Sub addFilter(ByVal value As Object)
        If value IsNot Nothing Then
            Dim aCFI As ColumnFilterItem = New ColumnFilterItem(Me)
            Dim find As Boolean = False
            aCFI.Value = value
            For Each cfi As ColumnFilterItem In _items
                If cfi.CompareTo(aCFI) = 0 Then
                    find = True
                    Exit For
                End If
            Next
            If Not find Then
                aCFI.Selected = True
                _items.Add(aCFI)
                _items.Sort()
            End If
        End If
    End Sub
    Public Sub reloadFilter(ByVal objs As List(Of Object))
        ' Create a temporary list, to remove unused filter item.
        Dim _tmpItems As List(Of ColumnFilterItem) = New List(Of ColumnFilterItem)
        For Each obj As Object In objs
            Dim anItem As ColumnFilterItem = New ColumnFilterItem(Me, obj)
            _tmpItems.Add(anItem)
        Next
        ' Remove an filter item that not exist in the new list
        Dim i As Integer = 0
        While i < _items.Count
            If Not _tmpItems.Contains(_items(i)) Then
                _items.RemoveAt(i)
            Else
                i = i + 1
            End If
        End While
        _tmpItems.Clear()
        ' Adding new filter
        For Each obj As Object In objs
            addFilter(obj)
        Next
    End Sub
    Public Function filterValue(ByVal itemValue As Object) As Boolean
        If Not _column.EnableFiltering Then Return True
        If _column.EnableCustomFilter And _useCustomFilter Then
            If _column.CustomFilter Is Nothing Then Return True
            Return _column.CustomFilter.Invoke(itemValue)
        End If
        If itemValue IsNot Nothing Then
            Select Case _column.Format
                Case ColumnFormat.None, ColumnFormat.Password
                    Dim strValue As String = itemValue
                    If _filterMode = FilterMode.ByRange Then
                        Select Case _rangeMode
                            Case FilterRangeMode.StartsWith
                                Return strValue.StartsWith(_minValue)
                            Case FilterRangeMode.EndsWith
                                Return strValue.EndsWith(_minValue)
                            Case FilterRangeMode.ContainsAny, FilterRangeMode.ContainsAll
                                Dim words As String = _minValue
                                words = words.Replace(",", " ")
                                Dim strWords() As String = Split(words, " ")
                                If strWords.Length > 0 Then
                                    Dim result As Boolean = _rangeMode = FilterRangeMode.ContainsAll
                                    For Each s As String In strWords
                                        If _rangeMode = FilterRangeMode.ContainsAll Then
                                            result = result And strValue.IndexOf(s, StringComparison.OrdinalIgnoreCase) > -1
                                        Else
                                            result = result Or strValue.IndexOf(s, StringComparison.OrdinalIgnoreCase) > -1
                                        End If
                                    Next
                                    Return result
                                End If
                                Return True
                            Case FilterRangeMode.NotContain
                                Dim words As String = _minValue
                                words = words.Replace(",", " ")
                                Dim strWords() As String = Split(words, " ")
                                If strWords.Length > 0 Then
                                    For Each s As String In strWords
                                        If strValue.IndexOf(s, StringComparison.OrdinalIgnoreCase) > -1 Then
                                            Return False
                                        End If
                                    Next
                                    Return True
                                End If
                                Return True
                        End Select
                    Else
                        For Each cfi As ColumnFilterItem In _items
                            If cfi.Selected Then
                                If String.Compare(strValue, cfi.Value, True) = 0 Then Return True
                            End If
                        Next
                        Return False
                    End If
                Case ColumnFormat.Bar, ColumnFormat.Currency, _
                    ColumnFormat.Custom, ColumnFormat.Exponential, _
                    ColumnFormat.FixedPoint, ColumnFormat.General, _
                    ColumnFormat.HexaDecimal, ColumnFormat.Number, _
                    ColumnFormat.Percent, ColumnFormat.RoundTrip
                    Dim dblValue As Double = CDbl(itemValue)
                    If _filterMode = FilterMode.ByRange Then
                        Dim dblMin As Double = CDbl(_minValue)
                        Dim dblMax As Double = CDbl(_maxValue)
                        If _rangeMode = FilterRangeMode.Between Then
                            Return dblValue >= dblMin And dblValue <= dblMax
                        Else
                            Return dblValue < dblMin Or dblValue > dblMax
                        End If
                    Else
                        For Each cfi As ColumnFilterItem In _items
                            If cfi.Selected Then
                                If CDbl(cfi.Value) = dblValue Then Return True
                            End If
                        Next
                        Return False
                    End If
                Case ColumnFormat.DecimalNumber
                    Dim intValue As Integer = CInt(itemValue)
                    If _filterMode = FilterMode.ByRange Then
                        Dim intMin As Integer = CInt(_minValue)
                        Dim intMax As Integer = CInt(_maxValue)
                        If _rangeMode = FilterRangeMode.Between Then
                            Return intValue >= intMin And intValue <= intMax
                        Else
                            Return intValue < intMin Or intValue > intMax
                        End If
                    Else
                        For Each cfi As ColumnFilterItem In _items
                            If cfi.Selected Then
                                If CDbl(cfi.Value) = intValue Then Return True
                            End If
                        Next
                        Return False
                    End If
                Case Else
                    Dim dValue As Date = itemValue
                    Dim minDate As Date = _minValue
                    Dim maxDate As Date = _maxValue
                    If _filterMode = FilterMode.ByRange Then
                        If _rangeMode = FilterRangeMode.Between Then
                            Return dValue >= minDate And dValue <= maxDate
                        Else
                            Return dValue < minDate Or dValue > maxDate
                        End If
                    Else
                        For Each cfi As ColumnFilterItem In _items
                            If cfi.Selected Then
                                Dim aDate As Date = cfi.Value
                                If aDate = dValue Then Return True
                            End If
                        Next
                        Return False
                    End If
            End Select
        End If
        Return True
    End Function
    Public Function getFilterItem(ByVal value As Object) As ColumnFilterItem
        For Each cfi As ColumnFilterItem In _items
            If cfi.Equals(value) Then Return cfi
        Next
        Return Nothing
    End Function
End Class
<Description("Control to display filter options on the popup window.")> _
Friend Class FilterChooser
    Inherits System.Windows.Forms.Control
    Dim _chkRect As Rectangle
    Dim _chkState As CheckState = CheckState.Unchecked
    Dim _hoverChk As Boolean = False
    Dim _toolStrip As ToolStripDropDown
    Dim _itemHosts As List(Of ItemHost) = New List(Of ItemHost)
    Dim _startIndex As Integer = 0, _endIndex As Integer = -1
    Dim _clientBound As Rectangle
    Dim _result As FilterChooserResult = FilterChooserResult.Cancel
    Dim _hoverItem As ItemHost = Nothing
    Dim _filterHandle As ColumnFilterHandle
    Dim _top As Integer = 0
    Dim _ci As CultureInfo
    Dim _customHost As ItemHost = Nothing
    Dim _ctrlRanges As List(Of System.Windows.Forms.Control) = New List(Of System.Windows.Forms.Control)
    Dim WithEvents _vscroll As VScrollBar = Nothing
    Dim WithEvents _rdbRange As RadioButton = Nothing
    Dim WithEvents _rdbValue As RadioButton = Nothing
    Dim WithEvents _btnOK As Button = Nothing
    Dim WithEvents _btnCancel As Button = Nothing
    <Description("Class to control FilterItem operations.")> _
    Private Class ItemHost
        Dim _item As ColumnFilterItem = Nothing
        Dim _parent As FilterChooser
        Dim _checked As Boolean = False
        Public Sub New(ByVal item As ColumnFilterItem, ByVal parent As FilterChooser)
            _item = item
            _parent = parent
            If _item IsNot Nothing Then _checked = _item.Selected
        End Sub
        Public Sub drawItem(ByVal g As Graphics, ByVal rect As Rectangle, _
            Optional ByVal hLited As Boolean = False, Optional ByVal enabled As Boolean = True)
            Dim txtRect As Rectangle = New Rectangle(rect.X + 20, rect.Y, rect.Width - 20, rect.Height)
            Dim txtFormat As StringFormat
            If _item Is Nothing Then txtRect = rect
            txtFormat = New StringFormat
            txtFormat.LineAlignment = StringAlignment.Center
            txtFormat.Alignment = StringAlignment.Near
            txtFormat.FormatFlags = StringFormatFlags.NoWrap
            txtFormat.Trimming = StringTrimming.EllipsisCharacter
            If hLited Then
                Renderer.Button.draw(g, rect, , 2, , , , True)
            End If
            If _checked Then
                Dim chkPoints(0 To 2) As Point
                Dim chkPen As Pen
                chkPoints(0).X = rect.X + 6
                chkPoints(1).X = rect.X + (20 * 0.4)
                chkPoints(2).X = 20 - 6
                chkPoints(0).Y = rect.Y + CInt(rect.Height / 2)
                chkPoints(1).Y = rect.Y + CInt(rect.Height / 2) + 4
                chkPoints(2).Y = rect.Y + CInt(rect.Height / 2) - 4
                If enabled Then
                    chkPen = New Pen(Color.Black, 2)
                Else
                    chkPen = New Pen(Color.Gray, 2)
                End If
                g.DrawLines(chkPen, chkPoints)
                chkPen.Dispose()
            End If
            If _item IsNot Nothing Then
                g.DrawString(getValueString(_item.Value), _parent.Font, IIf(enabled, Brushes.Black, Brushes.Gray), txtRect, txtFormat)
            Else
                g.DrawString("Custom Filter ...", _parent.Font, IIf(enabled, Brushes.Black, Brushes.Gray), txtRect, txtFormat)
            End If
            txtFormat.Dispose()
        End Sub
        Public Property Checked() As Boolean
            Get
                Return _checked
            End Get
            Set(ByVal value As Boolean)
                _checked = value
            End Set
        End Property
        Public ReadOnly Property Item() As ColumnFilterItem
            Get
                Return _item
            End Get
        End Property
        Private Function getValueString(ByVal value As Object) As String
            Dim result As String = ""
            Select Case _parent._filterHandle.DisplayFormat
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
                        Select Case _parent._filterHandle.DisplayFormat
                            Case ColumnFormat.Bar, ColumnFormat.Custom
                                result = dblValue.ToString(_parent._filterHandle.Column.CustomFormat, _parent._ci)
                            Case ColumnFormat.Currency
                                result = dblValue.ToString("C", _parent._ci)
                            Case ColumnFormat.Exponential
                                result = dblValue.ToString("E", _parent._ci)
                            Case ColumnFormat.FixedPoint
                                result = dblValue.ToString("F", _parent._ci)
                            Case ColumnFormat.General
                                result = dblValue.ToString("G", _parent._ci)
                            Case ColumnFormat.HexaDecimal
                                result = dblValue.ToString("X", _parent._ci)
                            Case ColumnFormat.Number
                                result = dblValue.ToString("N", _parent._ci)
                            Case ColumnFormat.Percent
                                result = dblValue.ToString("P", _parent._ci)
                            Case ColumnFormat.RoundTrip
                                result = dblValue.ToString("R", _parent._ci)
                        End Select
                    Catch ex As Exception
                    End Try
                Case ColumnFormat.DecimalNumber
                    ' Convert to integer
                    Try
                        Dim intValue As Integer = CInt(value)
                        result = intValue.ToString("D", _parent._ci)
                    Catch ex As Exception
                    End Try
                Case Else
                    ' Convert to datetime
                    Try
                        Dim dtValue As Date = CDate(value)
                        Select Case _parent._filterHandle.DisplayFormat
                            Case ColumnFormat.CustomDateTime
                                result = dtValue.ToString(_parent._filterHandle.Column.CustomFormat, _parent._ci)
                            Case ColumnFormat.ShortDate
                                result = dtValue.ToString("d", _parent._ci)
                            Case ColumnFormat.LongDate
                                result = dtValue.ToString("D", _parent._ci)
                            Case ColumnFormat.FullDateShortTime
                                result = dtValue.ToString("f", _parent._ci)
                            Case ColumnFormat.FullDateLongTime
                                result = dtValue.ToString("F", _parent._ci)
                            Case ColumnFormat.GeneralDateShortTime
                                result = dtValue.ToString("g", _parent._ci)
                            Case ColumnFormat.GeneralDateLongTime
                                result = dtValue.ToString("G", _parent._ci)
                            Case ColumnFormat.RoundTripDateTime
                                result = dtValue.ToString("O", _parent._ci)
                            Case ColumnFormat.RFC1123
                                result = dtValue.ToString("R", _parent._ci)
                            Case ColumnFormat.SortableDateTime
                                result = dtValue.ToString("s", _parent._ci)
                            Case ColumnFormat.ShortTime
                                result = dtValue.ToString("t", _parent._ci)
                            Case ColumnFormat.LongTime
                                result = dtValue.ToString("T", _parent._ci)
                            Case ColumnFormat.UniversalSortableDateTime
                                result = dtValue.ToString("u", _parent._ci)
                            Case ColumnFormat.UniversalFullDateTime
                                result = dtValue.ToString("U", _parent._ci)
                        End Select
                    Catch ex As Exception
                    End Try
            End Select
            Return result
        End Function
    End Class
    Public Sub New(ByVal handle As ColumnFilterHandle, ByVal toolStrip As ToolStripDropDown, ByVal font As Font, ByVal ci As CultureInfo)
        _toolStrip = toolStrip
        _filterHandle = handle
        _ci = ci
        _filterHandle.UseCustomFilter = False
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        Me.SetStyle(ControlStyles.Selectable, True)
        Me.BackColor = Renderer.Popup.BackgroundBrush.Color
        Me.Font = font
        If _filterHandle.Items.Count > 2 Then
            _rdbRange = New RadioButton
            _rdbRange.Top = 2
            _rdbRange.Left = 2
            Me.Controls.Add(_rdbRange)
            Select Case _filterHandle.DisplayFormat
                Case ColumnFormat.None, ColumnFormat.Password
                    _rdbRange.Text = "Words"
                    _top = _rdbRange.Bottom + 2
                    Dim lbWord As Label, lbMode As Label
                    Dim txt As TextBox = New TextBox
                    Dim cmb As ComboBox = New ComboBox
                    lbWord = New Label
                    lbWord.Left = 5
                    lbWord.Top = _top
                    Me.Controls.Add(lbWord)
                    lbWord.Text = "Words"
                    _ctrlRanges.Add(lbWord)
                    _top = lbWord.Bottom + 2
                    txt.Text = CStr(_filterHandle.MinValueSelected)
                    txt.Top = _top
                    txt.Left = 5
                    Me.Controls.Add(txt)
                    _ctrlRanges.Add(txt)
                    _top = txt.Bottom + 2
                    lbMode = New Label
                    lbMode.Left = 5
                    lbMode.Top = _top
                    Me.Controls.Add(lbMode)
                    lbMode.Text = "Search by"
                    _ctrlRanges.Add(lbMode)
                    _top = lbMode.Bottom + 2
                    cmb.Items.Add("Prefix")
                    cmb.Items.Add("Suffix")
                    cmb.Items.Add("All Words (AND)")
                    cmb.Items.Add("Any Words (OR)")
                    cmb.Items.Add("Not Contain")
                    cmb.DropDownStyle = ComboBoxStyle.DropDownList
                    cmb.Top = _top
                    cmb.Left = 5
                    Me.Controls.Add(cmb)
                    _ctrlRanges.Add(cmb)
                    Try
                        cmb.SelectedIndex = _filterHandle.RangeMode
                    Catch ex As Exception
                        cmb.SelectedIndex = 0
                    End Try
                    _top = cmb.Bottom + 5
                Case ColumnFormat.Bar, ColumnFormat.Currency, _
                    ColumnFormat.Custom, ColumnFormat.Exponential, _
                    ColumnFormat.FixedPoint, ColumnFormat.General, _
                    ColumnFormat.HexaDecimal, ColumnFormat.Number, _
                    ColumnFormat.Percent, ColumnFormat.RoundTrip, ColumnFormat.DecimalNumber
                    _rdbRange.Text = "Value range"
                    _top = _rdbRange.Bottom + 2
                    Dim nud1 As NumericUpDown, nud2 As NumericUpDown
                    Dim lbFrom As Label, lbTo As Label
                    Dim cmb As ComboBox = New ComboBox
                    lbFrom = New Label
                    lbFrom.Left = 5
                    lbFrom.Top = _top
                    Me.Controls.Add(lbFrom)
                    lbFrom.Text = "From"
                    _ctrlRanges.Add(lbFrom)
                    _top = lbFrom.Bottom + 2
                    nud1 = New NumericUpDown
                    nud1.Tag = "Lowest"
                    If _filterHandle.DisplayFormat = ColumnFormat.DecimalNumber Then
                        nud1.DecimalPlaces = 0
                        nud1.Minimum = CInt(_filterHandle.MinValue)
                        nud1.Maximum = CInt(_filterHandle.MaxValue)
                        Try
                            nud1.Value = CInt(_filterHandle.MinValueSelected)
                        Catch ex As Exception
                            nud1.Value = nud1.Minimum
                        End Try
                    Else
                        nud1.DecimalPlaces = 2
                        nud1.Minimum = CDbl(_filterHandle.MinValue)
                        nud1.Maximum = CDbl(_filterHandle.MaxValue)
                        Try
                            nud1.Value = CDbl(_filterHandle.MinValueSelected)
                        Catch ex As Exception
                            nud1.Value = nud1.Minimum
                        End Try
                    End If
                    nud1.Top = _top
                    nud1.Left = 5
                    nud1.TextAlign = HorizontalAlignment.Right
                    AddHandler nud1.ValueChanged, AddressOf nud_ValueChanged
                    Me.Controls.Add(nud1)
                    _top = nud1.Bottom + 2
                    _ctrlRanges.Add(nud1)
                    lbTo = New Label
                    lbTo.Top = _top
                    lbTo.Left = 5
                    Me.Controls.Add(lbTo)
                    lbTo.Text = "To"
                    _ctrlRanges.Add(lbTo)
                    _top = lbTo.Bottom + 2
                    nud2 = New NumericUpDown
                    nud2.Tag = "Highest"
                    nud2.DecimalPlaces = IIf(_filterHandle.DisplayFormat = ColumnFormat.DecimalNumber, 0, 2)
                    nud2.Minimum = nud1.Minimum
                    nud2.Maximum = nud1.Maximum
                    If _filterHandle.DisplayFormat = ColumnFormat.DecimalNumber Then
                        Try
                            nud2.Value = CInt(_filterHandle.MaxValueSelected)
                        Catch ex As Exception
                            nud2.Value = nud2.Maximum
                        End Try
                    Else
                        Try
                            nud2.Value = CDbl(_filterHandle.MaxValueSelected)
                        Catch ex As Exception
                            nud2.Value = nud2.Maximum
                        End Try
                    End If
                    nud2.Top = _top
                    nud2.Left = 5
                    nud2.TextAlign = HorizontalAlignment.Right
                    AddHandler nud2.ValueChanged, AddressOf nud_ValueChanged
                    Me.Controls.Add(nud2)
                    _ctrlRanges.Add(nud2)
                    _top = nud2.Bottom + 5
                    cmb.Items.Add("Inside Range")
                    cmb.Items.Add("Outside Range")
                    cmb.DropDownStyle = ComboBoxStyle.DropDownList
                    cmb.Top = _top
                    cmb.Left = 5
                    Me.Controls.Add(cmb)
                    _ctrlRanges.Add(cmb)
                    Try
                        cmb.SelectedIndex = _filterHandle.RangeMode
                    Catch ex As Exception
                        cmb.SelectedIndex = 0
                    End Try
                    _top = cmb.Bottom + 5
                Case Else
                    _rdbRange.Text = "Date / Time range"
                    _top = _rdbRange.Bottom + 2
                    Dim dp1 As DateTimePicker, dp2 As DateTimePicker
                    Dim lbFrom As Label, lbTo As Label
                    Dim cmb As ComboBox = New ComboBox
                    Dim dtfi As DateTimeFormatInfo = _ci.DateTimeFormat
                    lbFrom = New Label
                    lbFrom.Left = 5
                    lbFrom.Top = _top
                    Me.Controls.Add(lbFrom)
                    lbFrom.Text = "From"
                    _ctrlRanges.Add(lbFrom)
                    _top = lbFrom.Bottom + 2
                    dp1 = New DateTimePicker
                    dp1.Tag = "Lowest"
                    dp1.Format = DateTimePickerFormat.Custom
                    Select Case _filterHandle.DisplayFormat
                        Case ColumnFormat.CustomDateTime
                            dp1.CustomFormat = _filterHandle.Column.CustomFormat
                        Case ColumnFormat.ShortDate
                            dp1.CustomFormat = dtfi.ShortDatePattern
                        Case ColumnFormat.LongDate
                            dp1.CustomFormat = dtfi.LongDatePattern
                        Case ColumnFormat.FullDateShortTime
                            dp1.CustomFormat = dtfi.LongDatePattern & " " & dtfi.ShortTimePattern
                        Case ColumnFormat.FullDateLongTime
                            dp1.CustomFormat = dtfi.FullDateTimePattern
                        Case ColumnFormat.GeneralDateShortTime
                            dp1.CustomFormat = dtfi.ShortDatePattern & " " & dtfi.ShortTimePattern
                        Case ColumnFormat.GeneralDateLongTime
                            dp1.CustomFormat = dtfi.ShortDatePattern & " " & dtfi.LongTimePattern
                        Case ColumnFormat.RoundTripDateTime
                            dp1.CustomFormat = "yyyy-MM-dd T HH:mm:ss"
                        Case ColumnFormat.RFC1123
                            dp1.CustomFormat = dtfi.RFC1123Pattern
                        Case ColumnFormat.SortableDateTime
                            dp1.CustomFormat = dtfi.SortableDateTimePattern
                        Case ColumnFormat.ShortTime
                            dp1.CustomFormat = dtfi.ShortTimePattern
                        Case ColumnFormat.LongTime
                            dp1.CustomFormat = dtfi.LongTimePattern
                        Case ColumnFormat.UniversalSortableDateTime
                            dp1.CustomFormat = dtfi.UniversalSortableDateTimePattern
                        Case ColumnFormat.UniversalFullDateTime
                            dp1.CustomFormat = dtfi.FullDateTimePattern
                    End Select
                    dp1.MinDate = CDate(_filterHandle.MinValue)
                    dp1.MaxDate = CDate(_filterHandle.MaxValue)
                    Try
                        dp1.Value = CDate(_filterHandle.MinValueSelected)
                    Catch ex As Exception
                        dp1.Value = dp1.MinDate
                    End Try
                    dp1.Top = _top
                    dp1.Left = 5
                    dp1.ShowUpDown = True
                    AddHandler dp1.ValueChanged, AddressOf dp_ValueChanged
                    Me.Controls.Add(dp1)
                    _top = dp1.Bottom + 2
                    _ctrlRanges.Add(dp1)
                    lbTo = New Label
                    lbTo.Top = _top
                    lbTo.Left = 5
                    Me.Controls.Add(lbTo)
                    lbTo.Text = "To"
                    _ctrlRanges.Add(lbTo)
                    _top = lbTo.Bottom + 2
                    dp2 = New DateTimePicker
                    dp2.Tag = "Highest"
                    dp2.Format = DateTimePickerFormat.Custom
                    dp2.CustomFormat = dp1.CustomFormat
                    dp2.MinDate = dp1.MinDate
                    dp2.MaxDate = dp1.MaxDate
                    Try
                        dp2.Value = CDate(_filterHandle.MaxValueSelected)
                    Catch ex As Exception
                        dp2.Value = dp2.MaxDate
                    End Try
                    dp2.Top = _top
                    dp2.Left = 5
                    dp2.ShowUpDown = True
                    AddHandler dp2.ValueChanged, AddressOf dp_ValueChanged
                    Me.Controls.Add(dp2)
                    _ctrlRanges.Add(dp2)
                    _top = dp2.Bottom + 5
                    cmb.Items.Add("Inside Range")
                    cmb.Items.Add("Outside Range")
                    cmb.DropDownStyle = ComboBoxStyle.DropDownList
                    cmb.Top = _top
                    cmb.Left = 5
                    Me.Controls.Add(cmb)
                    _ctrlRanges.Add(cmb)
                    Try
                        cmb.SelectedIndex = _filterHandle.RangeMode
                    Catch ex As Exception
                        cmb.SelectedIndex = 0
                    End Try
                    _top = cmb.Bottom + 5
            End Select
            _rdbValue = New RadioButton
            _rdbValue.Top = _top
            _rdbValue.Left = 2
            Me.Controls.Add(_rdbValue)
            _rdbValue.Text = "Values"
            _top = _rdbValue.Bottom + 2
            If _filterHandle.FilterMode = FilterMode.ByRange Then
                _rdbRange.Checked = True
            Else
                _rdbValue.Checked = True
            End If
        Else
            _top = 2
        End If
        _chkRect = New Rectangle(0, _top, 22, 22)
        _top = _top + 20
        _vscroll = New VScrollBar
        _vscroll.LargeChange = 1
        Me.Controls.Add(_vscroll)
        createHosts(True)
    End Sub
    Public Sub createHosts(Optional ByVal measureHeight As Boolean = False)
        Dim i As Integer
        Dim anHost As ItemHost
        Dim iHeight As Integer = Me.Font.Height + 8
        _itemHosts.Clear()
        i = 0
        While i < _filterHandle.Items.Count
            anHost = New ItemHost(_filterHandle.Items.Item(i), Me)
            _itemHosts.Add(anHost)
            i = i + 1
        End While
        getEndIndex()
        If measureHeight Then
            If 7 < _filterHandle.Items.Count Then
                Me.Height = (7 * iHeight) + (_top + 1)
            Else
                Me.Height = (_filterHandle.Items.Count * iHeight) + (_top + 1)
            End If
        End If
        If _startIndex > 0 Or _endIndex < _itemHosts.Count - 1 Then
            _vscroll.Visible = True
            _vscroll.Maximum = _filterHandle.Items.Count - ((_endIndex - _startIndex) + 1)
            _clientBound = New Rectangle(1, _top, Me.Width - (_vscroll.Width + 2), _vscroll.Height)
        Else
            _vscroll.Visible = False
            _clientBound = New Rectangle(1, _top, Me.Width - 2, _vscroll.Height)
        End If
        If _filterHandle.Items.Count > 0 Then
            _btnOK = New Button
            _btnOK.Text = "OK"
            _btnOK.Top = _clientBound.Bottom + 3
            Me.Controls.Add(_btnOK)
            _ctrlRanges.Add(_btnOK)
            _btnCancel = New Button
            _btnCancel.Text = "Cancel"
            _btnCancel.Top = _clientBound.Bottom + 3
            Me.Controls.Add(_btnCancel)
            _ctrlRanges.Add(_btnCancel)
            If _filterHandle.Column.EnableCustomFilter Then
                _customHost = New ItemHost(Nothing, Me)
                Me.Height += (2 * _btnOK.Height) + 9
                Me.MinimumSize = New Size(200, _top + (2 * _btnOK.Height) + 39)
            Else
                Me.Height += _btnOK.Height + 6
                Me.MinimumSize = New Size(200, _top + _btnOK.Height + 36)
            End If
        End If
        checkCheckedState()
    End Sub
    Public ReadOnly Property FilterHandle() As ColumnFilterHandle
        Get
            Return _filterHandle
        End Get
    End Property
    Public ReadOnly Property Result() As FilterChooserResult
        Get
            Return _result
        End Get
    End Property
    Private Sub checkCheckedState()
        Dim chkCount As Integer
        chkCount = 0
        For Each iHost As ItemHost In _itemHosts
            If iHost.Checked Then chkCount += 1
        Next
        If chkCount = 0 Then
            _chkState = CheckState.Unchecked
        Else
            If chkCount = _itemHosts.Count Then
                _chkState = CheckState.Checked
            Else
                _chkState = CheckState.Indeterminate
            End If
        End If
    End Sub
    Private Sub getEndIndex()
        Dim i As Integer
        Dim quit As Boolean
        Dim visArea As Integer = 0
        Dim iHeight As Integer = Me.Font.Height + 8
        visArea = 0
        i = _startIndex
        quit = i >= _itemHosts.Count
        While Not quit
            visArea = visArea + iHeight
            If i < _itemHosts.Count - 1 Then quit = visArea + iHeight > _clientBound.Height
            i = i + 1
            If Not quit Then quit = i >= _itemHosts.Count
        End While
        _endIndex = i - 1
        If _endIndex = _itemHosts.Count - 1 Then
            ' Try to move starting index
            i = _startIndex - 1
            quit = i < 0
            While Not quit
                If visArea + iHeight < _clientBound.Height Then
                    quit = visArea + iHeight > _clientBound.Height
                    _startIndex = i
                    If i > 0 Then
                        i = i - 1
                        visArea = visArea + iHeight
                    Else
                        quit = True
                    End If
                Else
                    quit = True
                End If
            End While
        End If
    End Sub
    Private Sub FilterChooser_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyData
            Case Keys.Escape
                _result = FilterChooserResult.Cancel
                _toolStrip.Close()
            Case Keys.Return
                If _btnOK IsNot Nothing Then
                    If _btnOK.Enabled Then
                        _btnOK_Click(Me, New EventArgs)
                        Return
                    End If
                End If
                _toolStrip.Close()
            Case Else
                If _rdbRange IsNot Nothing Then
                    If _rdbRange.Checked Then Return
                End If
                Select Case e.KeyData
                    Case Keys.Space
                        If Me._hoverItem IsNot Nothing Then
                            Me._hoverItem.Checked = Not Me._hoverItem.Checked
                            checkCheckedState()
                            Me.Invalidate()
                        End If
                    Case Keys.Up, Keys.Down
                        Dim currentIndex As Integer
                        If e.KeyData = Keys.Up Then
                            If Me._hoverItem IsNot Nothing Then
                                currentIndex = _itemHosts.IndexOf(Me._hoverItem) - 1
                                If currentIndex < 0 Then Return
                                Me._hoverItem = _itemHosts(currentIndex)
                            Else
                                Me._hoverItem = _itemHosts(_itemHosts.Count - 1)
                                currentIndex = _itemHosts.Count - 1
                            End If
                        Else
                            If Me._hoverItem IsNot Nothing Then
                                currentIndex = _itemHosts.IndexOf(Me._hoverItem) + 1
                                If currentIndex >= _itemHosts.Count Then Return
                                Me._hoverItem = _itemHosts(currentIndex)
                            Else
                                Me._hoverItem = _itemHosts(0)
                                currentIndex = 0
                            End If
                        End If
                        If _vscroll.Visible Then
                            If currentIndex >= _startIndex And currentIndex <= _endIndex Then
                                Me.Invalidate()
                            Else
                                If currentIndex < _startIndex Then
                                    _vscroll.Value = currentIndex
                                Else
                                    _vscroll.Value = _startIndex + (currentIndex - _endIndex)
                                End If
                            End If
                        Else
                            Me.Invalidate()
                        End If
                End Select
        End Select
    End Sub
    Private Sub FilterChooser_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If _customHost IsNot Nothing Then
            If _hoverItem Is _customHost Then
                _result = FilterChooserResult.Custom
                _filterHandle.UseCustomFilter = True
                _toolStrip.Close()
                Return
            End If
        End If
        If _rdbValue Is Nothing Then Return
        If _rdbValue.Checked Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If Me._hoverItem IsNot Nothing Then
                    Me._hoverItem.Checked = Not Me._hoverItem.Checked
                    checkCheckedState()
                    Me.Invalidate()
                End If
                If _hoverChk Then
                    Select Case _chkState
                        Case CheckState.Checked
                            _chkState = CheckState.Unchecked
                            For Each fi As ColumnFilterItem In _filterHandle.Items
                                fi.Selected = False
                            Next
                        Case CheckState.Unchecked, CheckState.Indeterminate
                            _chkState = CheckState.Checked
                            For Each fi As ColumnFilterItem In _filterHandle.Items
                                fi.Selected = True
                            Next
                    End Select
                    Me.Invalidate()
                End If
            End If
        End If
    End Sub
    Private Sub FilterChooser_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        Dim changed As Boolean = False
        If Me._hoverItem IsNot Nothing Then
            Me._hoverItem = Nothing
            changed = True
        End If
        If _hoverChk Then
            _hoverChk = False
            changed = True
        End If
        If changed Then
            Me.Invalidate()
        End If
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub FilterChooser_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        'If _rdbValue Is Nothing Then Exit Sub
        'If _rdbValue.Checked Then
        'End If
        Dim i As Integer, lastY As Integer
        Dim aRect As Rectangle
        Dim find As Boolean = False, changed As Boolean = False
        Dim iHeight As Integer = Me.Font.Height + 8
        lastY = _clientBound.Top
        i = _startIndex
        While i <= _endIndex And Not find
            aRect = New Rectangle(_clientBound.Left, lastY, _clientBound.Width, iHeight)
            If aRect.Contains(e.X, e.Y) Then
                find = True
                If Me._hoverItem IsNot _itemHosts.Item(i) Then
                    Me._hoverItem = _itemHosts.Item(i)
                    changed = True
                End If
            End If
            lastY = lastY + iHeight
            i = i + 1
        End While
        If _customHost IsNot Nothing Then
            aRect = New Rectangle(_clientBound.X, _btnOK.Bottom + 3, Me.Width - 3, iHeight)
            If aRect.Contains(e.X, e.Y) Then
                find = True
                If Me._hoverItem IsNot _customHost Then
                    Me._hoverItem = _customHost
                    changed = True
                End If
            End If
        End If
        If Not find Then
            If Me._hoverItem IsNot Nothing Then
                Me._hoverItem = Nothing
                changed = True
            End If
            If _chkRect.Contains(e.X, e.Y) Then
                If Not _hoverChk Then
                    _hoverChk = True
                    changed = True
                End If
            Else
                If _hoverChk Then
                    _hoverChk = False
                    changed = True
                End If
            End If
        Else
            If _hoverChk Then
                _hoverChk = False
                changed = True
            End If
        End If
        If changed Then
            Me.Invalidate()
        End If
    End Sub
    Private Sub FilterChooser_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        If _rdbValue Is Nothing Then Exit Sub
        If _rdbValue.Checked Then
            If _vscroll.Visible Then
                If e.Delta > 0 Then
                    If _startIndex > 0 Then
                        _vscroll.Value -= 1
                    End If
                Else
                    If _endIndex < _filterHandle.Items.Count - 1 Then
                        _vscroll.Value += 1
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub FilterChooser_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim i As Integer, lastY As Integer
        Dim aRect As Rectangle, txtRect As Rectangle
        Dim txtFormat As StringFormat
        Dim _splitRect As Rectangle = New Rectangle(0, Me.Height - 10, Me.Width, 10)
        Dim iHeight As Integer = Me.Font.Height + 8
        Dim itemsEnabled As Boolean = True
        If _rdbValue IsNot Nothing Then itemsEnabled = _rdbValue.Checked
        txtFormat = New StringFormat
        txtFormat.FormatFlags = StringFormatFlags.NoWrap
        txtFormat.LineAlignment = StringAlignment.Center
        txtFormat.Alignment = StringAlignment.Near
        txtRect = New Rectangle(20, _top - 20, Me.Width - 23, 20)
        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        e.Graphics.Clear(Me.BackColor)
        e.Graphics.FillRectangle(Renderer.Popup.PlacementBrush, _
            New Rectangle(0, _top - 20, 20, Me.Height))
        e.Graphics.DrawLine(Renderer.Popup.SeparatorPen, 20, _top, 20, Me.Height)
        lastY = _clientBound.Top
        i = _startIndex
        While i <= _endIndex
            aRect = New Rectangle(_clientBound.Left, lastY, _clientBound.Width, iHeight)
            _itemHosts.Item(i).drawItem(e.Graphics, aRect, _itemHosts.Item(i) Is _hoverItem, itemsEnabled)
            lastY = lastY + iHeight
            i = i + 1
        End While
        e.Graphics.FillRectangle(Renderer.Popup.SeparatorBrush, _
            New Rectangle(0, _top - 20, Me.Width, 20))
        e.Graphics.DrawLine(Renderer.Popup.SeparatorPen, 0, _top, Me.Width, _top)
        If itemsEnabled Then
            e.Graphics.DrawString(_filterHandle.Column.Text, Me.Font, Brushes.Black, _
                txtRect, txtFormat)
        Else
            e.Graphics.DrawString(_filterHandle.Column.Text, Me.Font, Brushes.Gray, _
                txtRect, txtFormat)
        End If
        aRect = New Rectangle(0, _btnOK.Top - 3, Me.Width, Me.Height - (_btnOK.Top - 3))
        e.Graphics.FillRectangle(Renderer.Popup.BackgroundBrush, aRect)
        e.Graphics.DrawLine(Renderer.Popup.SeparatorPen, 0, _btnOK.Top - 3, Me.Width, _btnOK.Top - 3)
        If _customHost IsNot Nothing Then
            aRect = New Rectangle(_clientBound.X, _btnOK.Bottom + 3, Me.Width - 3, iHeight)
            _customHost.drawItem(e.Graphics, aRect, _hoverItem Is _customHost, True)
        End If
        Renderer.CheckBox.drawCheckBox(e.Graphics, _chkRect, _chkState, , itemsEnabled, _hoverChk)
        txtFormat.Dispose()
    End Sub
    Private Sub FilterChooser_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        _vscroll.Top = _top + 1
        If _btnOK Is Nothing Then
            _vscroll.Height = Me.Height - (_top + 1)
        Else
            If _customHost Is Nothing Then
                _btnOK.Top = Me.Height - (_btnOK.Height + 3)
                _btnCancel.Top = _btnOK.Top
            Else
                _btnOK.Top = Me.Height - ((_btnOK.Height * 2) + 6)
                _btnCancel.Top = _btnOK.Top
            End If
            _vscroll.Height = (_btnOK.Top - 3) - (_top + 1)
        End If
        _vscroll.Left = Me.Width - _vscroll.Width
        _clientBound.Height = _vscroll.Height
        getEndIndex()
        If _startIndex > 0 Or _endIndex < _itemHosts.Count - 1 Then
            _vscroll.Visible = True
            _vscroll.Maximum = _filterHandle.Items.Count - ((_endIndex - _startIndex) + 1)
            _clientBound = New Rectangle(1, _top, Me.Width - (_vscroll.Width + 2), _vscroll.Height)
        Else
            _vscroll.Visible = False
            _clientBound = New Rectangle(1, _top, Me.Width - 2, _vscroll.Height)
        End If
        If _ctrlRanges.Count > 0 Then
            For Each c As System.Windows.Forms.Control In _ctrlRanges
                If TypeOf (c) Is Button Then
                    If c Is _btnCancel Then
                        c.Left = Me.Width - (c.Width + 5)
                    Else
                        c.Left = Me.Width - ((c.Width * 2) + 10)
                    End If
                ElseIf TypeOf (c) Is TextBox Then
                    c.Width = Me.Width - 10
                ElseIf TypeOf (c) Is ComboBox Then
                    c.Width = Me.Width - 10
                ElseIf TypeOf (c) Is NumericUpDown Then
                    c.Width = Me.Width - 10
                ElseIf TypeOf (c) Is DateTimePicker Then
                    c.Width = Me.Width - 10
                End If
            Next
        End If
    End Sub
    Private Sub _vscroll_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _vscroll.ValueChanged
        _startIndex = _vscroll.Value
        getEndIndex()
        Me.Invalidate()
    End Sub
    Private Sub _rdbRange_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _rdbRange.CheckedChanged
        For Each c As System.Windows.Forms.Control In _ctrlRanges
            c.Enabled = _rdbRange.Checked
        Next
        If _vscroll IsNot Nothing Then _vscroll.Enabled = _rdbValue.Checked
        Me.Invalidate()
    End Sub
    Private Sub _rdbValue_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _rdbValue.CheckedChanged
        For Each c As System.Windows.Forms.Control In _ctrlRanges
            c.Enabled = _rdbRange.Checked
        Next
        If _vscroll IsNot Nothing Then _vscroll.Enabled = _rdbValue.Checked
        Me.Invalidate()
    End Sub
    Private Sub _btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnOK.Click
        _result = FilterChooserResult.OK
        If _rdbRange Is Nothing Then
            _filterHandle.FilterMode = FilterMode.ByValue
            For Each iHost As ItemHost In _itemHosts
                iHost.Item.Selected = iHost.Checked
            Next
            _toolStrip.Close()
            Return
        End If
        If _rdbRange.Checked Then
            _filterHandle.FilterMode = FilterMode.ByRange
            Select Case _filterHandle.DisplayFormat
                Case ColumnFormat.None, ColumnFormat.Password
                    For Each c As System.Windows.Forms.Control In _ctrlRanges
                        If TypeOf (c) Is TextBox Then
                            Dim txt As TextBox = DirectCast(c, TextBox)
                            _filterHandle.MinValueSelected = txt.Text
                        ElseIf TypeOf (c) Is ComboBox Then
                            Dim cmb As ComboBox = DirectCast(c, ComboBox)
                            _filterHandle.RangeMode = cmb.SelectedIndex
                        End If
                    Next
                Case ColumnFormat.Bar, ColumnFormat.Currency, _
                    ColumnFormat.Custom, ColumnFormat.Exponential, _
                    ColumnFormat.FixedPoint, ColumnFormat.General, _
                    ColumnFormat.HexaDecimal, ColumnFormat.Number, _
                    ColumnFormat.Percent, ColumnFormat.RoundTrip, ColumnFormat.DecimalNumber
                    For Each c As System.Windows.Forms.Control In _ctrlRanges
                        If TypeOf (c) Is NumericUpDown Then
                            Dim nud As NumericUpDown = DirectCast(c, NumericUpDown)
                            If nud.Tag = "Lowest" Then
                                _filterHandle.MinValueSelected = nud.Value
                            Else
                                _filterHandle.MaxValueSelected = nud.Value
                            End If
                        ElseIf TypeOf (c) Is ComboBox Then
                            Dim cmb As ComboBox = DirectCast(c, ComboBox)
                            _filterHandle.RangeMode = cmb.SelectedIndex
                        End If
                    Next
                Case Else
                    For Each c As System.Windows.Forms.Control In _ctrlRanges
                        If TypeOf (c) Is DateTimePicker Then
                            Dim dp As DateTimePicker = DirectCast(c, DateTimePicker)
                            If dp.Tag = "Lowest" Then
                                _filterHandle.MinValueSelected = dp.Value
                            Else
                                _filterHandle.MaxValueSelected = dp.Value
                            End If
                        ElseIf TypeOf (c) Is ComboBox Then
                            Dim cmb As ComboBox = DirectCast(c, ComboBox)
                            _filterHandle.RangeMode = cmb.SelectedIndex
                        End If
                    Next
            End Select
            _toolStrip.Close()
        Else
            _filterHandle.FilterMode = FilterMode.ByValue
            For Each iHost As ItemHost In _itemHosts
                iHost.Item.Selected = iHost.Checked
            Next
            _toolStrip.Close()
        End If
    End Sub
    Private Sub nud_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim nud1 As NumericUpDown = DirectCast(sender, NumericUpDown)
        Dim nud2 As NumericUpDown = Nothing
        For Each c As System.Windows.Forms.Control In _ctrlRanges
            If TypeOf (c) Is NumericUpDown Then
                If nud1.Tag = "Lowest" Then
                    If c.Tag = "Highest" Then
                        nud2 = DirectCast(c, NumericUpDown)
                        Exit For
                    End If
                Else
                    If c.Tag = "Lowest" Then
                        nud2 = DirectCast(c, NumericUpDown)
                        Exit For
                    End If
                End If
            End If
        Next
        If nud2 IsNot Nothing Then
            If nud1.Tag = "Lowest" Then
                If nud2.Value < nud1.Value Then nud2.Value = nud1.Value
            Else
                If nud2.Value > nud1.Value Then nud2.Value = nud1.Value
            End If
        End If
    End Sub
    Private Sub dp_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim dp1 As DateTimePicker = DirectCast(sender, DateTimePicker)
        Dim dp2 As DateTimePicker = Nothing
        For Each c As System.Windows.Forms.Control In _ctrlRanges
            If TypeOf (c) Is NumericUpDown Then
                If dp1.Tag = "Lowest" Then
                    If c.Tag = "Highest" Then
                        dp2 = DirectCast(c, DateTimePicker)
                        Exit For
                    End If
                Else
                    If c.Tag = "Lowest" Then
                        dp2 = DirectCast(c, DateTimePicker)
                        Exit For
                    End If
                End If
            End If
        Next
        If dp2 IsNot Nothing Then
            If dp1.Tag = "Lowest" Then
                If dp2.Value < dp1.Value Then dp2.Value = dp1.Value
            Else
                If dp2.Value > dp1.Value Then dp2.Value = dp1.Value
            End If
        End If
    End Sub
    Protected Overrides Function IsInputKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Select Case keyData
            Case Keys.Up, Keys.Down, Keys.Space, Keys.Return, Keys.Escape
                Return True
        End Select
        Return MyBase.IsInputKey(keyData)
    End Function
    Private Sub _btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnCancel.Click
        _result = FilterChooserResult.Cancel
        _toolStrip.Close()
    End Sub
End Class
#End Region
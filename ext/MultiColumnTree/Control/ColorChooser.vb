Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.ComponentModel
''' <summary>
''' Control for selecting a color.
''' Provides :
''' 1. Predefinded colors and 17 history of last used colors, the last history will be shown first.
''' 2. Creating a custom color from its components (A, RGB, or HSB).
''' 3. Directly show ColorDialog window when the button(not the split) is clicked.
''' 4. Automatically add the last selected color to the color histories.
''' </summary>
<DefaultEvent("SelectedColorChanged"), DefaultProperty("SelectedColor")> _
Public Class ColorChooser
    Inherits System.Windows.Forms.Control
#Region "Public Classes"
    Public Class ColorHistories
        Inherits CollectionBase
        Dim _owner As ColorChooser
#Region "Public Events"
        Public Event AfterInsert(ByVal sender As Object, ByVal e As EventArgs)
        Public Event AfterRemove(ByVal sender As Object, ByVal e As EventArgs)
        Public Event AfterClear(ByVal sender As Object, ByVal e As EventArgs)
        Public Event AfterSet(ByVal sender As Object, ByVal e As EventArgs)
#End Region
#Region "Constructor"
        Public Sub New(ByVal owner As ColorChooser)
            _owner = owner
        End Sub
#End Region
#Region "Public Properties"
        Default Public ReadOnly Property Item(ByVal index As Integer) As Drawing.Color
            Get
                If index < 0 Or index >= List.Count Then
                    Return Nothing
                Else
                    Return DirectCast(List.Item(index), Drawing.Color)
                End If
            End Get
        End Property
        Public ReadOnly Property IndexOf(ByVal color As Drawing.Color) As Integer
            Get
                Dim i As Integer = 0
                While i < List.Count
                    Dim clr As Drawing.Color = DirectCast(List(i), Drawing.Color)
                    If Renderer.Drawing.compareColor(color, clr) Then Return i
                    i = i + 1
                End While
                Return -1
            End Get
        End Property
#End Region
#Region "Public Methods"
        Public Overloads Function Add(ByVal color As Drawing.Color) As Drawing.Color
            Dim index As Integer = IndexOf(color)
            If index > -1 Then
                ' There is same color exist, remove it.
                List.RemoveAt(index)
            End If
            index = List.Add(color)
            ' Check if there are more than 17 colors.
            If List.Count > 17 Then
                ' Remove from first index.
                While List.Count > 17
                    List.RemoveAt(0)
                End While
                index = IndexOf(color)
            End If
            Return DirectCast(List.Item(index), Drawing.Color)
        End Function
        Public Overloads Function Add(ByVal alpha As Integer, ByVal red As Integer, _
            ByVal green As Integer, ByVal blue As Integer) As Color
            If alpha < 0 Or alpha > 255 Or red < 0 Or red > 255 Or green < 0 Or green > 255 Or _
                blue < 0 Or blue > 255 Then Return Color.Transparent
            Dim newColor As Drawing.Color = Drawing.Color.FromArgb(alpha, red, green, blue)
            Return Me.Add(newColor)
        End Function
        Public Overloads Function Add(ByVal red As Integer, ByVal green As Integer, _
            ByVal blue As Integer) As Color
            If red < 0 Or red > 255 Or green < 0 Or green > 255 Or _
                blue < 0 Or blue > 255 Then Return Color.Transparent
            Dim newColor As Drawing.Color = Drawing.Color.FromArgb(red, green, blue)
            Return Me.Add(newColor)
        End Function
        Public Overloads Function Add(ByVal alpha As Integer, ByVal hue As Integer, _
            ByVal saturation As Single, ByVal brightness As Single) As Color
            If alpha < 0 Or alpha > 255 Or hue < 0 Or hue > 360 Or _
                saturation < 0.0F Or saturation > 1.0F Or _
                brightness < 0.0F Or brightness > 1.0F Then Return Color.Transparent
            Dim newColor As Drawing.Color = Renderer.Drawing.colorFromAHSB(alpha, hue, saturation, brightness)
            Return Me.Add(newColor)
        End Function
        Public Overloads Function Add(ByVal hue As Integer, _
            ByVal saturation As Single, ByVal brightness As Single) As Color
            If hue < 0 Or hue > 360 Or _
                saturation < 0.0F Or saturation > 1.0F Or _
                brightness < 0.0F Or brightness > 1.0F Then Return Color.Transparent
            Dim newColor As Drawing.Color = Renderer.Drawing.colorFromAHSB(255, hue, saturation, brightness)
            Return Me.Add(newColor)
        End Function
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Public Overloads Sub AddRange(ByVal colors As ColorHistories)
            For Each clr As Drawing.Color In colors
                Me.Add(clr)
            Next
        End Sub
        Public Sub Insert(ByVal index As Integer, ByVal color As Drawing.Color)
            If IndexOf(color) = -1 Then
                List.Insert(index, color)
            End If
        End Sub
        Public Sub Remove(ByVal color As Drawing.Color)
            List.Remove(color)
        End Sub
        Public Function Contains(ByVal color As Drawing.Color) As Boolean
            Dim i As Integer = 0
            While i < List.Count
                Dim clr As Drawing.Color = DirectCast(List(i), Drawing.Color)
                If Renderer.Drawing.compareColor(clr, color) Then Return True
            End While
            Return False
        End Function
#End Region
#Region "Protected Overriden Methods"
        Protected Overrides Sub OnValidate(ByVal value As Object)
            If Not GetType(Color).IsAssignableFrom(value.GetType) Then
                Throw New ArgumentException("Value must System.Drawing.Color", "value")
            End If
        End Sub
        Protected Overrides Sub OnInsertComplete(ByVal index As Integer, ByVal value As Object)
            RaiseEvent AfterInsert(Me, New EventArgs)
        End Sub
        Protected Overrides Sub OnRemoveComplete(ByVal index As Integer, ByVal value As Object)
            RaiseEvent AfterRemove(Me, New EventArgs)
        End Sub
        Protected Overrides Sub OnClearComplete()
            RaiseEvent AfterClear(Me, New EventArgs)
        End Sub
        Protected Overrides Sub OnSetComplete(ByVal index As Integer, ByVal oldValue As Object, ByVal newValue As Object)
            RaiseEvent AfterSet(Me, New EventArgs)
        End Sub
#End Region
    End Class
#End Region
#Region "Private Classes."
    ''' <summary>
    ''' Define a component of a color.
    ''' </summary>
    Private Enum ComponentName
        Alpha = 0
        Red = 1
        Green = 2
        Blue = 3
        Hue = 4
        Saturation = 5
        Brightness = 6
    End Enum
    ''' <summary>
    ''' Class for picking a component value of a color.
    ''' </summary>
    Private Class ComponentSlider
        Dim _component As ComponentName = ComponentName.Alpha
        Dim _value As Single = 0
        Dim _location As Point = New Point(0, 0)
        Dim _width As Integer = 100
        Dim _height As Integer = 20
        Dim _enabled As Boolean = True
        Dim _max As Single = 255
        Dim _customFormat As String = "0"
        Dim _slideRect As Rectangle = New Rectangle(-5, 5, 10, 10)
        Dim _hoverSlide As Boolean = False
        Dim _interceptMouseDown As Boolean = False
        Dim _boxPoint As Point = New Point(0, 0)
        Dim _changed As Boolean = False ' Determine whether the object need to repaint.
        Dim _rectColor As Rectangle = New Rectangle(_location.X, _location.Y, _width, 10)
        Public Event ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
        Public Sub New()
        End Sub
        Public Property Component() As ComponentName
            Get
                Return _component
            End Get
            Set(ByVal value As ComponentName)
                If _component <> value Then
                    _component = value
                    Select Case _component
                        Case ComponentName.Alpha, ComponentName.Red, ComponentName.Green, ComponentName.Blue
                            _max = 255
                        Case ComponentName.Hue
                            _max = 360.0F
                        Case ComponentName.Saturation, ComponentName.Brightness
                            _max = 1.0F
                    End Select
                    _slideRect.X = _location.X + ((_width * _value / _max) - 5)
                    If _value > _max Then
                        _value = _max
                    End If
                    _changed = True
                    RaiseEvent ValueChanged(Me, New EventArgs)
                End If
            End Set
        End Property
        Public Property Value() As Single
            Get
                Return _value
            End Get
            Set(ByVal value As Single)
                If _value <> value Then
                    If value >= 0 And value <= _max Then
                        _value = value
                        _slideRect.X = _location.X + ((_width * _value / _max) - 5)
                        _changed = True
                        RaiseEvent ValueChanged(Me, New EventArgs)
                    End If
                End If
            End Set
        End Property
        Public Property Location() As Point
            Get
                Return _location
            End Get
            Set(ByVal value As Point)
                If _location <> value Then
                    _location = value
                    _slideRect.X = _location.X + ((_width * _value / _max) - 5)
                    _slideRect.Y = _location.Y + 10
                    _rectColor = New Rectangle(_location.X, _location.Y, _width, 10)
                    _changed = True
                    RaiseEvent ValueChanged(Me, New EventArgs)
                End If
            End Set
        End Property
        Public Property Enabled() As Boolean
            Get
                Return _enabled
            End Get
            Set(ByVal value As Boolean)
                If _enabled <> value Then
                    _enabled = value
                    If Not _enabled Then
                        _hoverSlide = False
                        _interceptMouseDown = False
                        _boxPoint = New Point(0, 0)
                    End If
                    _changed = True
                    RaiseEvent ValueChanged(Me, New EventArgs)
                End If
            End Set
        End Property
        Public Property Width() As Integer
            Get
                Return _width
            End Get
            Set(ByVal value As Integer)
                If value > 20 Then
                    _width = value
                    _slideRect.X = _location.X + ((_width * _value / _max) - 5)
                    _rectColor = New Rectangle(_location.X, _location.Y, _width, 10)
                    _changed = True
                    RaiseEvent ValueChanged(Me, New EventArgs)
                End If
            End Set
        End Property
        Public Property Changed() As Boolean
            Get
                Return _changed
            End Get
            Set(ByVal value As Boolean)
                _changed = value
            End Set
        End Property
        Public Property CustomFormat() As String
            Get
                Return _customFormat
            End Get
            Set(ByVal value As String)
                _customFormat = value
            End Set
        End Property
        Public ReadOnly Property Right() As Integer
            Get
                Return _location.X + _width + 5
            End Get
        End Property
        Public ReadOnly Property Bottom() As Integer
            Get
                Return _location.Y + _height
            End Get
        End Property
        Public ReadOnly Property Height() As Integer
            Get
                Return _height
            End Get
        End Property
        Public ReadOnly Property InterceptMouseDown() As Boolean
            Get
                Return _interceptMouseDown
            End Get
        End Property
        Public ReadOnly Property Bounds() As Rectangle
            Get
                Return New Rectangle(_location.X, _location.Y, _width, _height)
            End Get
        End Property
        Private Sub drawSlider(ByVal g As Graphics)
            Dim sliderBrush As LinearGradientBrush = New LinearGradientBrush(_slideRect, _
                Color.Black, Color.White, LinearGradientMode.Vertical)
            Dim borderPen As Pen
            Dim tPoints(0 To 3) As Point
            If _enabled Then
                If _hoverSlide Then
                    sliderBrush.InterpolationColors = Ai.Renderer.Button.HLitedBlend
                    borderPen = Ai.Renderer.Button.HLitedBorderPen
                Else
                    sliderBrush.InterpolationColors = Ai.Renderer.Button.NormalBlend
                    borderPen = Ai.Renderer.Button.NormalBorderPen
                End If
            Else
                sliderBrush.InterpolationColors = Ai.Renderer.Button.DisabledBlend
                borderPen = Ai.Renderer.Button.DisabledBorderPen
            End If
            With _slideRect
                tPoints(0) = New Point(.X + 5, .Y)
                tPoints(1) = New Point(.X + 9, .Y + 7)
                tPoints(2) = New Point(.X, .Y + 7)
                tPoints(3) = tPoints(0)
            End With
            g.FillPolygon(sliderBrush, tPoints)
            g.DrawPolygon(borderPen, tPoints)
            sliderBrush.Dispose()
            borderPen.Dispose()
        End Sub
        Public Sub draw(ByVal g As Graphics, Optional ByVal focused As Boolean = False)
            If _enabled Then
                ' Setting up color bar
                Dim fColor As Color, eColor As Color
                Select Case _component
                    Case ComponentName.Alpha
                        fColor = Color.Black
                        eColor = Color.White
                    Case ComponentName.Blue
                        fColor = Color.Black
                        eColor = Color.FromArgb(0, 0, 255)
                    Case ComponentName.Red
                        fColor = Color.Black
                        eColor = Color.FromArgb(255, 0, 0)
                    Case ComponentName.Green
                        fColor = Color.Black
                        eColor = Color.FromArgb(0, 255, 0)
                    Case ComponentName.Brightness
                        fColor = Ai.Renderer.Drawing.colorFromAHSB(255, 0, 0, 0)
                        eColor = Ai.Renderer.Drawing.colorFromAHSB(255, 0, 0, 1.0F)
                    Case ComponentName.Hue
                        fColor = Ai.Renderer.Drawing.colorFromAHSB(255, 0, 0, 0)
                        eColor = Ai.Renderer.Drawing.colorFromAHSB(255, 360.0F, 0, 0)
                    Case ComponentName.Saturation
                        fColor = Ai.Renderer.Drawing.colorFromAHSB(255, 0, 0, 0)
                        eColor = Ai.Renderer.Drawing.colorFromAHSB(255, 0, 1.0F, 0)
                End Select
                Dim bgBrush As LinearGradientBrush = New LinearGradientBrush(_rectColor, _
                    fColor, eColor, LinearGradientMode.Horizontal)
                g.FillRectangle(bgBrush, _rectColor)
                g.DrawRectangle(Pens.Black, _rectColor)
                bgBrush.Dispose()
            Else
                g.FillRectangle(Brushes.Gray, _rectColor)
                g.DrawRectangle(Pens.DimGray, _rectColor)
            End If
            drawSlider(g)
            If focused Then
                Dim aPen As Pen
                Dim rectFocus As Rectangle = New Rectangle(_location.X, _location.Y, _width, _height)
                aPen = New Pen(Color.Black, 1)
                aPen.DashStyle = DashStyle.Dot
                aPen.DashOffset = 0.1F
                g.DrawRectangle(aPen, rectFocus)
                aPen.Dispose()
            End If
        End Sub
        Public Sub mouseMove(ByVal p As Point)
            If _enabled Then
                If Not _interceptMouseDown Then
                    If _slideRect.Contains(p) Then
                        If Not _hoverSlide Then
                            _hoverSlide = True
                            _changed = True
                        End If
                    Else
                        If _rectColor.Contains(p) Then
                            _boxPoint = p
                        Else
                            _boxPoint = New Point(0, 0)
                        End If
                        If _hoverSlide Then
                            _hoverSlide = False
                            _changed = True
                        End If
                    End If
                Else
                    Dim _newValue As Single = (p.X - _location.X) * _max / _width
                    If _newValue < 0 Then _newValue = 0
                    If _newValue > _max Then _newValue = _max
                    Value = _newValue
                End If
            End If
        End Sub
        Public Sub mouseDown()
            If _enabled Then
                If _hoverSlide Then
                    _interceptMouseDown = True
                Else
                    If _boxPoint.X > 0 Or _boxPoint.Y > 0 Then
                        Dim _newValue As Single = (_boxPoint.X - _location.X) * _max / _width
                        If _newValue < 0 Then _newValue = 0
                        If _newValue > _max Then _newValue = _max
                        Value = _newValue
                    End If
                End If
            End If
        End Sub
        Public Sub mouseLeave()
            If _enabled Then
                _boxPoint = New Point(0, 0)
                If _hoverSlide Then
                    _hoverSlide = False
                    _changed = True
                End If
            End If
        End Sub
        Public Sub mouseUp()
            _interceptMouseDown = False
        End Sub
        Public Sub increaseValue()
            Dim newValue As Single
            Select Case _component
                Case ComponentName.Alpha, ComponentName.Red, ComponentName.Green, ComponentName.Blue, ComponentName.Hue
                    newValue = _value + 1
                Case Else
                    newValue = _value + 0.01F
            End Select
            If newValue > _max Then newValue = _max
            Value = newValue
        End Sub
        Public Sub decreaseValue()
            Dim newValue As Single
            Select Case _component
                Case ComponentName.Alpha, ComponentName.Red, ComponentName.Green, ComponentName.Blue, ComponentName.Hue
                    newValue = _value - 1
                Case Else
                    newValue = _value - 0.01F
            End Select
            If newValue < 0 Then newValue = 0
            Value = newValue
        End Sub
        Public Overrides Function ToString() As String
            Return _value.ToString(_customFormat, Ai.Renderer.Drawing.en_us_ci)
        End Function
    End Class
    ''' <summary>
    ''' Control to display popup window to select existing colors or create it from each components.
    ''' </summary>
    Private Class ColorPopup
        Inherits System.Windows.Forms.Control
        Dim _owner As ColorChooser
        Dim _rectPredefined As Rectangle
        Dim _rectComponent As Rectangle
        Dim _hoverIndex As Integer = -1
        Dim _selectedIndex As Integer = 0
        Dim _predefined As PredefinedColorControl
        Dim _component As ComponentColorControl
        ''' <summary>
        ''' Control to display history and predefined colors.
        ''' </summary>
        Private Class PredefinedColorControl
            Inherits System.Windows.Forms.Control
            Private Const ColorMargin As Integer = 4
            Private Const HostHeight As Integer = 20
            Dim _owner As ColorPopup
            Dim _hosts As List(Of ColorHost) = New List(Of ColorHost)
            Dim _hoverHost As ColorHost = Nothing
            Dim _focusedHost As ColorHost = Nothing
            Dim _tooltipHost As ColorHost = Nothing
            Dim WithEvents _colorTooltip As ToolTip
            Private Class ColorHost
                Dim _owner As PredefinedColorControl
                Dim _color As Drawing.Color = Drawing.Color.Black
                Dim _rect As Rectangle
                Dim _selected As Boolean = False
                Public Sub New(ByVal owner As PredefinedColorControl)
                    _owner = owner
                End Sub
                Public Property Rectangle() As Rectangle
                    Get
                        Return _rect
                    End Get
                    Set(ByVal value As Rectangle)
                        _rect = value
                    End Set
                End Property
                Public Property Selected() As Boolean
                    Get
                        Return _selected
                    End Get
                    Set(ByVal value As Boolean)
                        _selected = value
                    End Set
                End Property
                Public Property Color() As Drawing.Color
                    Get
                        Return _color
                    End Get
                    Set(ByVal value As Drawing.Color)
                        _color = value
                    End Set
                End Property
                Public ReadOnly Property TooltipText() As String
                    Get
                        Return "Alpha : " & CStr(_color.A) & vbCrLf & _
                            "R : " & CStr(_color.R) & vbCrLf & _
                            "G : " & CStr(_color.G) & vbCrLf & _
                            "B : " & CStr(_color.B) & vbCrLf
                    End Get
                End Property
                Public Sub draw(ByVal g As Graphics, _
                    Optional ByVal focused As Boolean = False, Optional ByVal hLited As Boolean = False)
                    If _selected Or hLited Or focused Then
                        Ai.Renderer.Button.draw(g, _rect, , 2, True, , _selected, hLited, focused)
                    End If
                    Dim rectColor As Rectangle = New Rectangle(_rect.X + ColorMargin, _
                        _rect.Y + ColorMargin, _rect.Width - (2 * ColorMargin), _rect.Height - (2 * ColorMargin))
                    g.FillRectangle(New SolidBrush(_color), rectColor)
                    g.DrawRectangle(Pens.Black, rectColor)
                End Sub
            End Class
            Public Sub New(ByVal owner As ColorPopup)
                Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
                Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
                Me.SetStyle(ControlStyles.ResizeRedraw, True)
                Me.SetStyle(ControlStyles.Selectable, True)
                _owner = owner
                recreateColor()
                MyBase.Width = (7 * HostHeight) + 94
                MyBase.Height = 55 + (17 * HostHeight)
                Me.SetStyle(ControlStyles.FixedHeight, True)
                Me.SetStyle(ControlStyles.FixedWidth, True)
                _colorTooltip = New ToolTip(Me)
                _colorTooltip.AnimationSpeed = 20
                _colorTooltip.EnableAutoClose = False
            End Sub
            Public Sub setSelectedColor()
                ' Setting up selected color
                recreateColor()
                For Each ch As ColorHost In _hosts
                    If ch.Color = _owner._owner._selectedColor Then
                        ch.Selected = True
                    Else
                        ch.Selected = False
                    End If
                Next
                Me.Invalidate()
            End Sub
            Public Sub fireKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
                PredefinedColorControl_KeyDown(Me, e)
            End Sub
            Private Sub recreateColor()
                ' Setting up hosts for 17 maximum colors history.
                Dim i As Integer, j As Integer
                Dim x As Integer, y As Integer
                Dim aHost As ColorHost
                _hosts.Clear()
                i = 0
                x = 3
                y = 22
                While i < 17 And i < _owner._owner._mruColors.Count
                    aHost = New ColorHost(Me)
                    aHost.Color = _owner._owner._mruColors(_owner._owner._mruColors.Count - (i + 1))
                    aHost.Rectangle = New Rectangle(x, y, 70, HostHeight)
                    _hosts.Add(aHost)
                    i = i + 1
                    y = y + HostHeight + 2
                End While
                ' Setting up predefined colors.
                aHost = New ColorHost(Me)
                aHost.Color = Drawing.Color.Black
                aHost.Rectangle = New Rectangle(75, 22, (7 * HostHeight) + 12, HostHeight)
                _hosts.Add(aHost)
                x = 75
                j = 0
                While j < 7
                    y = 22 + HostHeight + 2
                    i = 15
                    While i < 256
                        aHost = New ColorHost(Me)
                        aHost.Rectangle = New Rectangle(x, y, HostHeight, HostHeight)
                        Select Case j
                            Case 0
                                aHost.Color = Drawing.Color.FromArgb(255, i, 0, 0)
                            Case 1
                                aHost.Color = Drawing.Color.FromArgb(255, 0, i, 0)
                            Case 2
                                aHost.Color = Drawing.Color.FromArgb(255, 0, 0, i)
                            Case 3
                                aHost.Color = Drawing.Color.FromArgb(255, 0, i, i)
                            Case 4
                                aHost.Color = Drawing.Color.FromArgb(255, i, 0, i)
                            Case 5
                                aHost.Color = Drawing.Color.FromArgb(255, i, i, 0)
                            Case 6
                                aHost.Color = Drawing.Color.FromArgb(255, i, i, i)
                        End Select
                        _hosts.Add(aHost)
                        i = i + 16
                        y = y + HostHeight + 2
                    End While
                    j = j + 1
                    x = x + HostHeight + 2
                End While
            End Sub
            Private Sub PredefinedColorControl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
                Dim changed As Boolean = False
                Select Case e.KeyData
                    Case Keys.Up
                        If _focusedHost Is Nothing Then
                            _focusedHost = _hosts(_hosts.Count - 1)
                        Else
                            Dim focusedIndex = _hosts.IndexOf(_focusedHost)
                            If focusedIndex = 0 Then
                                _focusedHost = _hosts(_hosts.Count - 1)
                            Else
                                _focusedHost = _hosts(focusedIndex - 1)
                            End If
                        End If
                        _tooltipHost = _focusedHost
                        _colorTooltip.show(Me, New Rectangle(-Left, -Top, _owner.Width, _owner.Height))
                        changed = True
                    Case Keys.Down
                        If _focusedHost Is Nothing Then
                            _focusedHost = _hosts(0)
                        Else
                            Dim focusedIndex = _hosts.IndexOf(_focusedHost)
                            If focusedIndex = _hosts.Count - 1 Then
                                _focusedHost = _hosts(0)
                            Else
                                _focusedHost = _hosts(focusedIndex + 1)
                            End If
                        End If
                        _tooltipHost = _focusedHost
                        _colorTooltip.show(Me, New Rectangle(-Left, -Top, _owner.Width, _owner.Height))
                        changed = True
                    Case Keys.Left
                        If _focusedHost IsNot Nothing Then
                            Dim find As Boolean = False
                            Dim i As Integer = _hosts.IndexOf(_focusedHost) - 1
                            While i >= 0 And Not find
                                If _hosts(i).Rectangle.Y = _focusedHost.Rectangle.Y Then
                                    find = True
                                Else
                                    i = i - 1
                                End If
                            End While
                            If find Then
                                _focusedHost = _hosts(i)
                                _tooltipHost = _focusedHost
                                _colorTooltip.show(Me, New Rectangle(-Left, -Top, _owner.Width, _owner.Height))
                                changed = True
                            End If
                        Else
                            _focusedHost = _hosts(_hosts.Count - 1)
                            _tooltipHost = _focusedHost
                            _colorTooltip.show(Me, New Rectangle(-Left, -Top, _owner.Width, _owner.Height))
                            changed = True
                        End If
                    Case Keys.Right
                        If _focusedHost IsNot Nothing Then
                            Dim find As Boolean = False
                            Dim i As Integer = _hosts.IndexOf(_focusedHost) + 1
                            While i < _hosts.Count And Not find
                                If _hosts(i).Rectangle.Y = _focusedHost.Rectangle.Y Then
                                    find = True
                                Else
                                    i = i + 1
                                End If
                            End While
                            If find Then
                                _focusedHost = _hosts(i)
                                _tooltipHost = _focusedHost
                                _colorTooltip.show(Me, New Rectangle(-Left, -Top, _owner.Width, _owner.Height))
                                changed = True
                            End If
                        Else
                            _focusedHost = _hosts(0)
                            _tooltipHost = _focusedHost
                            _colorTooltip.show(Me, New Rectangle(-Left, -Top, _owner.Width, _owner.Height))
                            changed = True
                        End If
                    Case Keys.Return
                        _colorTooltip.hide()
                        _owner._owner._mustRaiseEvent = Not Renderer.Drawing.compareColor(_owner._owner._selectedColor, _focusedHost.Color)
                        _owner._owner._selectedColor = _focusedHost.Color
                        ' Close popup.
                        _owner._owner._tdown.Hide()
                    Case Keys.Tab
                        _colorTooltip.hide()
                        ' Switch to component control.
                        _owner.switchControl()
                    Case Keys.Escape
                        _colorTooltip.hide()
                        ' Close popup.
                        _owner._owner._tdown.Hide()
                End Select
                If changed Then Me.Invalidate()
            End Sub
            Private Sub PredefinedColorControl_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
                If e.Button = Windows.Forms.MouseButtons.Left Then
                    If _hoverHost IsNot Nothing Then
                        _colorTooltip.hide()
                        ' Highlited color is selected and popup closed.
                        _owner._owner._mustRaiseEvent = Not Renderer.Drawing.compareColor(_owner._owner._selectedColor, _hoverHost.Color)
                        _owner._owner._selectedColor = _hoverHost.Color
                        ' Close popup.
                        _owner._owner._tdown.Hide()
                    End If
                End If
            End Sub
            Private Sub PredefinedColorControl_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
                If _tooltipHost Is _hoverHost Then
                    _tooltipHost = Nothing
                    _colorTooltip.hide()
                End If
                If _hoverHost IsNot Nothing Then
                    _hoverHost = Nothing
                    Me.Invalidate()
                End If
            End Sub
            Private Sub PredefinedColorControl_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
                Dim find As Boolean = False
                Dim changed As Boolean = False
                Dim i As Integer = 0
                While i < _hosts.Count And Not find
                    If _hosts(i).Rectangle.Contains(e.Location) Then
                        If _hoverHost IsNot _hosts(i) Then
                            _hoverHost = _hosts(i)
                            _tooltipHost = _hoverHost
                            _colorTooltip.show(Me, New Rectangle(-Left, -Top, _owner.Width, _owner.Height))
                            changed = True
                        End If
                        find = True
                    End If
                    i = i + 1
                End While
                If Not find Then
                    If _hoverHost IsNot Nothing Then
                        _colorTooltip.hide()
                        _hoverHost = Nothing
                        changed = True
                    End If
                End If
                If changed Then Me.Invalidate()
            End Sub
            Private Sub PredefinedColorControl_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
                e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                e.Graphics.Clear(Drawing.Color.FromArgb(250, 250, 250))
                e.Graphics.DrawString("MRU", Ai.Renderer.ToolTip.TextFont, Brushes.Black, 3, 5)
                e.Graphics.DrawString("Predefined", Ai.Renderer.ToolTip.TextFont, Brushes.Black, 75, 5)
                For Each ch As ColorHost In _hosts
                    ch.draw(e.Graphics, ch Is _focusedHost, ch Is _hoverHost)
                Next
                e.Graphics.DrawRectangle(Ai.Renderer.Button.NormalBorderPen, New Rectangle(0, 0, Me.Width - 1, Me.Height - 1))
            End Sub
            Private Sub _colorTooltip_Draw(ByVal sender As Object, ByVal e As DrawEventArgs) Handles _colorTooltip.Draw
                If _tooltipHost IsNot Nothing Then _
                    Ai.Renderer.ToolTip.drawToolTip("", _tooltipHost.TooltipText, Nothing, e.Graphics, e.Rectangle)
            End Sub
            Private Sub _colorTooltip_Popup(ByVal sender As Object, ByVal e As PopupEventArgs) Handles _colorTooltip.Popup
                If _tooltipHost IsNot Nothing Then e.Size = Ai.Renderer.ToolTip.measureSize("", _tooltipHost.TooltipText, Nothing)
            End Sub
        End Class
        ''' <summary>
        ''' Control to create custom color based on its components (A, RGB, HSB).
        ''' </summary>
        Private Class ComponentColorControl
            Inherits System.Windows.Forms.Control
            Private Const sliderLeft As Integer = 25
            Private Const sliderWidth As Integer = 150
            Dim _owner As ColorPopup
            Dim _sliders As List(Of ComponentSlider)
            Dim _buttonRectangle As Rectangle
            Dim _pickerRectangle As Rectangle
            Dim _pickerPreview As Rectangle
            Dim _hoverPicker As Boolean = False
            Dim _pickerFocused As Boolean = False
            Dim _hoverButton As Boolean = False
            Dim _onPicking As Boolean = False
            Dim _currentColor As Drawing.Color = Drawing.Color.Black
            Dim _suspendEvent As Boolean = False
            Dim _focusedSlider As ComponentSlider = Nothing
            Dim _lastPoint As Point
            Dim _pickedColor As Color
            Dim WithEvents tmrCapture As Timer = New Timer
            Public Sub New(ByVal owner As ColorPopup)
                Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
                Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
                Me.SetStyle(ControlStyles.ResizeRedraw, True)
                Me.SetStyle(ControlStyles.Selectable, True)
                _owner = owner
                _sliders = New List(Of ComponentSlider)
                ' Setting up sliders
                Dim aSlider As ComponentSlider
                Dim lastY As Integer = 10
                aSlider = New ComponentSlider
                aSlider.Component = ComponentName.Alpha
                aSlider.CustomFormat = "0"
                aSlider.Width = sliderWidth
                aSlider.Location = New Point(sliderLeft, lastY)
                _sliders.Add(aSlider)
                lastY = lastY + 45
                aSlider = New ComponentSlider
                aSlider.Component = ComponentName.Red
                aSlider.CustomFormat = "0"
                aSlider.Width = sliderWidth
                aSlider.Location = New Point(sliderLeft, lastY)
                _sliders.Add(aSlider)
                lastY = lastY + 25
                aSlider = New ComponentSlider
                aSlider.Component = ComponentName.Green
                aSlider.CustomFormat = "0"
                aSlider.Width = sliderWidth
                aSlider.Location = New Point(sliderLeft, lastY)
                _sliders.Add(aSlider)
                lastY = lastY + 25
                aSlider = New ComponentSlider
                aSlider.Component = ComponentName.Blue
                aSlider.CustomFormat = "0"
                aSlider.Width = sliderWidth
                aSlider.Location = New Point(sliderLeft, lastY)
                _sliders.Add(aSlider)
                lastY = lastY + 45
                aSlider = New ComponentSlider
                aSlider.Component = ComponentName.Hue
                aSlider.CustomFormat = "0.00"
                aSlider.Width = sliderWidth
                aSlider.Location = New Point(sliderLeft, lastY)
                _sliders.Add(aSlider)
                lastY = lastY + 25
                aSlider = New ComponentSlider
                aSlider.Component = ComponentName.Saturation
                aSlider.CustomFormat = "0.00"
                aSlider.Width = sliderWidth
                aSlider.Location = New Point(sliderLeft, lastY)
                _sliders.Add(aSlider)
                lastY = lastY + 25
                aSlider = New ComponentSlider
                aSlider.Component = ComponentName.Brightness
                aSlider.CustomFormat = "0.00"
                aSlider.Width = sliderWidth
                aSlider.Location = New Point(sliderLeft, lastY)
                _sliders.Add(aSlider)
                lastY = lastY + 25
                _pickerRectangle = New Rectangle(3, lastY, 228, 30)
                lastY = lastY + 35
                _pickerPreview = New Rectangle(3, lastY, 228, 30)
                lastY = lastY + 47
                _buttonRectangle = New Rectangle(sliderLeft, lastY, sliderWidth, 22)
                ' Setting up ValueChanged event handler for ComponentSliders
                For Each cs As ComponentSlider In _sliders
                    AddHandler cs.ValueChanged, AddressOf slider_ValueChanged
                Next
                MyBase.Width = 234
                MyBase.Height = 55 + (17 * 20)
                Me.SetStyle(ControlStyles.FixedHeight, True)
                Me.SetStyle(ControlStyles.FixedWidth, True)
                tmrCapture.Enabled = False
            End Sub
            Public Sub setSelectedColor()
                _suspendEvent = True
                _currentColor = _owner._owner._selectedColor
                For Each cs As ComponentSlider In _sliders
                    Select Case cs.Component
                        Case ComponentName.Alpha
                            cs.Value = _owner._owner._selectedColor.A
                            cs.Enabled = _owner._owner._alphaEnabled
                        Case ComponentName.Red
                            cs.Value = _owner._owner._selectedColor.R
                        Case ComponentName.Green
                            cs.Value = _owner._owner._selectedColor.G
                        Case ComponentName.Blue
                            cs.Value = _owner._owner._selectedColor.B
                        Case ComponentName.Hue
                            cs.Value = _owner._owner._selectedColor.GetHue
                        Case ComponentName.Saturation
                            cs.Value = _owner._owner._selectedColor.GetSaturation
                        Case ComponentName.Brightness
                            cs.Value = _owner._owner._selectedColor.GetBrightness
                    End Select
                Next
                _pickerFocused = False
                _onPicking = False
                _focusedSlider = Nothing
                Me.Invalidate()
                _suspendEvent = False
            End Sub
            Public Sub fireKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
                ComponentColorControl_KeyDown(Me, e)
            End Sub
            Private Sub slider_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
                If Not _suspendEvent Then
                    _suspendEvent = True ' Avoiding more events fired.
                    Dim slider As ComponentSlider = DirectCast(sender, ComponentSlider)
                    ' Change current color
                    Select Case slider.Component
                        Case ComponentName.Alpha
                            _currentColor = Drawing.Color.FromArgb(slider.Value, _currentColor.R, _currentColor.G, _currentColor.B)
                        Case ComponentName.Red
                            _currentColor = Drawing.Color.FromArgb(_currentColor.A, slider.Value, _currentColor.G, _currentColor.B)
                        Case ComponentName.Green
                            _currentColor = Drawing.Color.FromArgb(_currentColor.A, _currentColor.R, slider.Value, _currentColor.B)
                        Case ComponentName.Blue
                            _currentColor = Drawing.Color.FromArgb(_currentColor.A, _currentColor.R, _currentColor.G, slider.Value)
                        Case ComponentName.Hue
                            _currentColor = Ai.Renderer.Drawing.colorFromAHSB(_currentColor.A, slider.Value, _
                                _currentColor.GetSaturation, _currentColor.GetBrightness)
                        Case ComponentName.Saturation
                            _currentColor = Ai.Renderer.Drawing.colorFromAHSB(_currentColor.A, _currentColor.GetHue, _
                                slider.Value, _currentColor.GetBrightness)
                        Case ComponentName.Brightness
                            _currentColor = Ai.Renderer.Drawing.colorFromAHSB(_currentColor.A, _currentColor.GetHue, _
                                _currentColor.GetSaturation, slider.Value)
                    End Select
                    ' Change the other slider's value
                    Select Case slider.Component
                        Case ComponentName.Red, ComponentName.Green, ComponentName.Blue
                            For Each cs As ComponentSlider In _sliders
                                Select Case cs.Component
                                    Case ComponentName.Hue
                                        cs.Value = _currentColor.GetHue
                                    Case ComponentName.Saturation
                                        cs.Value = _currentColor.GetSaturation
                                    Case ComponentName.Brightness
                                        cs.Value = _currentColor.GetBrightness
                                End Select
                            Next
                        Case ComponentName.Hue, ComponentName.Saturation, ComponentName.Brightness
                            For Each cs As ComponentSlider In _sliders
                                Select Case cs.Component
                                    Case ComponentName.Red
                                        cs.Value = _currentColor.R
                                    Case ComponentName.Green
                                        cs.Value = _currentColor.G
                                    Case ComponentName.Blue
                                        cs.Value = _currentColor.B
                                End Select
                            Next
                    End Select
                    _suspendEvent = False
                    Me.Invalidate()
                End If
            End Sub
            Private Sub ComponentColorControl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
                If Not _onPicking Then
                    Select Case e.KeyData
                        Case Keys.Down
                            Dim nextIndex As Integer
                            _pickerFocused = False
                            If _focusedSlider Is Nothing Then
                                nextIndex = 0
                            Else
                                nextIndex = _sliders.IndexOf(_focusedSlider) + 1
                            End If
                            If nextIndex < _sliders.Count Then
                                If Not _sliders(nextIndex).Enabled Then nextIndex = nextIndex + 1
                            End If
                            If nextIndex < _sliders.Count Then
                                _focusedSlider = _sliders(nextIndex)
                                _pickerFocused = False
                            Else
                                _pickerFocused = True
                                _focusedSlider = Nothing
                            End If
                            Me.Invalidate()
                        Case Keys.Up
                            If _focusedSlider Is Nothing And Not _pickerFocused Then
                                _pickerFocused = True
                            Else
                                If _pickerFocused Then
                                    _focusedSlider = _sliders(_sliders.Count - 1)
                                    _pickerFocused = False
                                Else
                                    Dim prevIndex As Integer = _sliders.IndexOf(_focusedSlider) - 1
                                    If prevIndex < 0 Then
                                        _pickerFocused = True
                                        _focusedSlider = Nothing
                                    Else
                                        If prevIndex = 0 And Not _sliders(0).Enabled Then
                                            _pickerFocused = True
                                            _focusedSlider = Nothing
                                        Else
                                            _focusedSlider = _sliders(prevIndex)
                                            _pickerFocused = False
                                        End If
                                    End If
                                End If
                            End If
                            Me.Invalidate()
                        Case Keys.Space
                            If _pickerFocused Then
                                _onPicking = True
                                tmrCapture.Enabled = True
                                Me.Invalidate()
                            End If
                        Case Keys.Left
                            If _focusedSlider IsNot Nothing Then _focusedSlider.decreaseValue()
                        Case Keys.Right
                            If _focusedSlider IsNot Nothing Then _focusedSlider.increaseValue()
                        Case Keys.Return
                            _owner._owner._mustRaiseEvent = Not Renderer.Drawing.compareColor(_owner._owner._selectedColor, _currentColor)
                            _owner._owner._selectedColor = _currentColor
                            ' Close popup.
                            _owner._owner._tdown.Hide()
                        Case Keys.Tab
                            ' Switch to predefined control.
                            _owner.switchControl()
                        Case Keys.Escape
                            ' Close popup.
                            _owner._owner._tdown.Hide()
                    End Select
                Else
                    Select Case e.KeyData
                        Case Keys.Space
                            _onPicking = False
                            tmrCapture.Enabled = False
                            Me.Invalidate()
                        Case Keys.Return
                            _suspendEvent = True
                            _currentColor = _pickedColor
                            For Each cs As ComponentSlider In _sliders
                                Select Case cs.Component
                                    Case ComponentName.Red
                                        cs.Value = _currentColor.R
                                    Case ComponentName.Green
                                        cs.Value = _currentColor.G
                                    Case ComponentName.Blue
                                        cs.Value = _currentColor.B
                                    Case ComponentName.Hue
                                        cs.Value = _currentColor.GetHue
                                    Case ComponentName.Saturation
                                        cs.Value = _currentColor.GetSaturation
                                    Case ComponentName.Brightness
                                        cs.Value = _currentColor.GetBrightness
                                End Select
                            Next
                            Me.Invalidate()
                            _suspendEvent = False
                    End Select
                End If
            End Sub
            Private Sub ComponentColorControl_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
                If Not _onPicking Then
                    If e.Button = Windows.Forms.MouseButtons.Left Then
                        If _hoverButton Then
                            _owner._owner._mustRaiseEvent = Not Renderer.Drawing.compareColor(_owner._owner._selectedColor, _currentColor)
                            _owner._owner._selectedColor = _currentColor
                            ' Close popup.
                            _owner._owner._tdown.Hide()
                        Else
                            If _hoverPicker Then
                                _focusedSlider = Nothing
                                _pickerFocused = True
                                _onPicking = True
                                tmrCapture.Enabled = True
                            Else
                                Dim pCursor As Point = System.Windows.Forms.Control.MousePosition
                                Dim changed As Boolean = False
                                pCursor = Me.PointToClient(pCursor)
                                For Each cs As ComponentSlider In _sliders
                                    cs.mouseDown()
                                    If cs.Enabled Then
                                        If cs.Bounds.Contains(pCursor) Then
                                            If _focusedSlider IsNot cs Then
                                                _focusedSlider = cs
                                                changed = True
                                            End If
                                        End If
                                    End If
                                    changed = changed Or cs.Changed
                                Next
                                If changed Then Me.Invalidate()
                            End If
                        End If
                    End If
                End If
            End Sub
            Private Sub ComponentColorControl_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
                Dim changed As Boolean = False
                For Each cs As ComponentSlider In _sliders
                    cs.mouseLeave()
                    changed = changed Or cs.Changed
                Next
                If _hoverButton Then
                    _hoverButton = False
                    changed = True
                End If
                If _hoverPicker Then
                    _hoverPicker = False
                    changed = True
                End If
                If changed Then Me.Invalidate()
            End Sub
            Private Sub ComponentColorControl_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
                If Not _onPicking Then
                    Dim changed As Boolean = False
                    For Each cs As ComponentSlider In _sliders
                        cs.mouseMove(e.Location)
                        changed = changed Or cs.Changed
                    Next
                    Dim mouseIntercepted As Boolean = False
                    For Each cs As ComponentSlider In _sliders
                        mouseIntercepted = mouseIntercepted Or cs.InterceptMouseDown
                    Next
                    If _buttonRectangle.Contains(e.Location) Then
                        If Not mouseIntercepted Then
                            If Not _hoverButton Then
                                _hoverButton = True
                                changed = True
                            End If
                        End If
                    Else
                        If _hoverButton Then
                            _hoverButton = False
                            changed = True
                        End If
                    End If
                    If _pickerRectangle.Contains(e.Location) Then
                        If Not mouseIntercepted Then
                            If Not _hoverPicker Then
                                _hoverPicker = True
                                changed = True
                            End If
                        End If
                    Else
                        If _hoverPicker Then
                            _hoverPicker = False
                            changed = True
                        End If
                    End If
                    If changed Then Me.Invalidate()
                End If
            End Sub
            Private Sub ComponentColorControl_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
                Dim changed As Boolean = False
                For Each cs As ComponentSlider In _sliders
                    cs.mouseUp()
                    changed = changed Or cs.Changed
                Next
                If changed Then Me.Invalidate()
            End Sub
            Private Sub ComponentColorControl_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
                Dim nameFormat As StringFormat = New StringFormat
                Dim valueFormat As StringFormat = New StringFormat
                Dim nameRect As Rectangle, valueRect As Rectangle
                Dim i As Integer
                Dim rectColor As Rectangle = New Rectangle(_buttonRectangle.X + 4, _buttonRectangle.Y + 4, _buttonRectangle.Width - 8, _buttonRectangle.Height - 8)
                nameRect = New Rectangle(3, 0, 20, 30)
                valueRect = New Rectangle(172, 0, 45, 30)
                nameFormat.Alignment = StringAlignment.Near
                valueFormat.Alignment = StringAlignment.Far
                nameFormat.LineAlignment = StringAlignment.Center
                valueFormat.LineAlignment = StringAlignment.Center
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
                e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                e.Graphics.Clear(Color.FromArgb(250, 250, 250))
                i = 0
                While i < _sliders.Count
                    nameRect.Y = _sliders(i).Location.Y - 7
                    valueRect.Y = _sliders(i).Location.Y - 7
                    e.Graphics.DrawString(_sliders(i).Component.ToString.Substring(0, 1), _
                        Ai.Renderer.ToolTip.TextFont, Brushes.Black, nameRect, nameFormat)
                    e.Graphics.DrawString(_sliders(i).ToString, _
                        Ai.Renderer.ToolTip.TextFont, Brushes.Black, valueRect, valueFormat)
                    _sliders(i).draw(e.Graphics, _sliders(i) Is _focusedSlider)
                    If i = 0 Then
                        Dim groupRect As Rectangle = New Rectangle(3, _sliders(i).Bottom, Me.Width - 6, 25)
                        e.Graphics.DrawString("RGB Components", Ai.Renderer.ToolTip.TextFont, _
                            Brushes.Black, groupRect, nameFormat)
                    ElseIf i = 3 Then
                        Dim groupRect As Rectangle = New Rectangle(3, _sliders(i).Bottom, Me.Width - 6, 25)
                        e.Graphics.DrawString("HSB Components", Ai.Renderer.ToolTip.TextFont, _
                            Brushes.Black, groupRect, nameFormat)
                    End If
                    _sliders(i).Changed = False
                    i = i + 1
                End While
                ' Checkbox for color picker
                Dim rectChk As Rectangle = New Rectangle(_pickerRectangle.X, _pickerRectangle.Y, _
                    20, _pickerRectangle.Height)
                Dim rectText As Rectangle = New Rectangle(_pickerRectangle.X + 21, _pickerRectangle.Y, _
                    _pickerRectangle.Width - 25, _pickerRectangle.Height)
                Renderer.CheckBox.drawCheckBox(e.Graphics, rectChk, _
                    IIf(_onPicking, CheckState.Checked, CheckState.Unchecked), , , _hoverPicker)
                If Not _onPicking Then
                    e.Graphics.DrawString("Pick color on screen.", _
                        Renderer.ToolTip.TextFont, Brushes.Black, rectText, nameFormat)
                Else
                    e.Graphics.DrawString("Press enter to select current color, space to disable.", _
                        Renderer.ToolTip.TextFont, Brushes.Black, rectText, nameFormat)
                End If
                If _pickerFocused Then
                    Dim aPen As Pen
                    aPen = New Pen(Color.Black, 1)
                    aPen.DashStyle = DashStyle.Dot
                    aPen.DashOffset = 0.1F
                    e.Graphics.DrawRectangle(aPen, rectText)
                    aPen.Dispose()
                End If
                If _onPicking Then
                    ' Picking a color from screen.
                    Dim pCursor As Point = System.Windows.Forms.Control.MousePosition
                    Dim previewLocation As Point = New Point(_pickerPreview.X + 50, _pickerPreview.Y)
                    Dim colorRect As Rectangle = New Rectangle(_pickerPreview.X + 85, _pickerPreview.Y, 30, 30)
                    Dim aBmp As Bitmap = New Bitmap(30, 30)
                    Dim gBmp As Graphics = Graphics.FromImage(aBmp)
                    gBmp.CopyFromScreen(pCursor.X - 15, pCursor.Y - 15, 0, 0, New Size(30, 30))
                    gBmp.Dispose()
                    e.Graphics.DrawImage(aBmp, previewLocation)
                    e.Graphics.DrawRectangle(Pens.Black, New Rectangle(previewLocation.X, previewLocation.Y, 30, 30))
                    e.Graphics.DrawRectangle(Pens.Black, New Rectangle(previewLocation.X + 14, previewLocation.Y + 14, 4, 4))
                    e.Graphics.FillRectangle(New SolidBrush(aBmp.GetPixel(15, 15)), colorRect)
                    _pickedColor = aBmp.GetPixel(15, 15)
                    e.Graphics.DrawRectangle(Pens.Black, colorRect)
                    aBmp.Dispose()
                    e.Graphics.DrawString("Preview :", Renderer.ToolTip.TextFont, Brushes.Black, _
                        _pickerPreview, nameFormat)
                    colorRect.X = colorRect.Right + 5
                    colorRect.Width = Me.Width - (colorRect.Left + 5)
                    colorRect.Height = 40
                    e.Graphics.DrawString("R : " & CStr(_pickedColor.R) & vbCrLf & _
                        "G : " & CStr(_pickedColor.G) & vbCrLf & "B : " & CStr(_pickedColor.B), _
                        Renderer.ToolTip.TextFont, Brushes.Black, colorRect, nameFormat)
                    ' End picking
                End If
                Ai.Renderer.Button.draw(e.Graphics, _buttonRectangle, , 2, , , , _hoverButton)
                e.Graphics.FillRectangle(New SolidBrush(_currentColor), rectColor)
                e.Graphics.DrawRectangle(Pens.Black, rectColor)
                e.Graphics.DrawRectangle(Ai.Renderer.Button.NormalBorderPen, New Rectangle(0, 0, Me.Width - 1, Me.Height - 1))
                nameFormat.Dispose()
                valueFormat.Dispose()
            End Sub
            Private Sub ComponentColorControl_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
                If Me.Visible Then
                    If _onPicking Then
                        _lastPoint = Windows.Forms.Control.MousePosition
                        tmrCapture.Enabled = True
                    End If
                Else
                    _lastPoint = New Point(0, 0)
                    tmrCapture.Enabled = False
                End If
            End Sub
            Private Sub tmrCapture_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrCapture.Tick
                Dim currentPoint As Point = Windows.Forms.Control.MousePosition
                If currentPoint <> _lastPoint Then
                    _lastPoint = currentPoint
                    Me.Invalidate()
                End If
            End Sub
        End Class
        Public Sub New(ByVal owner As ColorChooser)
            Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            Me.SetStyle(ControlStyles.ResizeRedraw, True)
            Me.SetStyle(ControlStyles.Selectable, True)
            _owner = owner
            _rectPredefined = New Rectangle(3, 3, 100, 22)
            _rectComponent = New Rectangle(105, 3, 100, 22)
            _predefined = New PredefinedColorControl(Me)
            _component = New ComponentColorControl(Me)
            _predefined.Left = 3
            _predefined.Top = 27
            _component.Left = 3
            _component.Top = 27
            _component.Visible = False
            Me.Controls.Add(_predefined)
            Me.Controls.Add(_component)
            Me.Width = 243
            Me.Height = 425
            Me.SetStyle(ControlStyles.FixedHeight, True)
            Me.SetStyle(ControlStyles.FixedWidth, True)
        End Sub
        Protected Overrides Function IsInputKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean
            Select Case keyData
                Case Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.Return, Keys.Tab, Keys.Escape, Keys.Space
                    Return True
                Case Else
                    Return MyBase.IsInputKey(keyData)
            End Select
        End Function
        Public Sub setSelectedColor()
            _predefined.setSelectedColor()
            _component.setSelectedColor()
        End Sub
        Private Sub switchControl()
            If _selectedIndex = 0 Then
                _selectedIndex = 1
                _predefined.Visible = False
                _component.Visible = True
            Else
                _selectedIndex = 0
                _predefined.Visible = True
                _component.Visible = False
            End If
            Me.Invalidate()
        End Sub
        Private Sub ColorPopup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
            If _selectedIndex = 0 Then
                _predefined.fireKeyDown(e)
            Else
                _component.fireKeyDown(e)
            End If
        End Sub
        Private Sub ColorPopup_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If _hoverIndex <> _selectedIndex Then
                    switchControl()
                End If
            End If
        End Sub
        Private Sub ColorPopup_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
            If _hoverIndex <> -1 Then
                _hoverIndex = -1
                Me.Invalidate()
            End If
        End Sub
        Private Sub ColorPopup_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
            Dim changed As Boolean = False
            If _rectPredefined.Contains(e.Location) Then
                If _hoverIndex <> 0 Then
                    _hoverIndex = 0
                    changed = True
                End If
            Else
                If _rectComponent.Contains(e.Location) Then
                    If _hoverIndex <> 1 Then
                        _hoverIndex = 1
                        changed = True
                    End If
                Else
                    If _hoverIndex <> -1 Then
                        _hoverIndex = -1
                        changed = True
                    End If
                End If
            End If
            If changed Then Me.Invalidate()
        End Sub
        Private Sub ColorPopup_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
            Dim strFormat As StringFormat = New StringFormat
            strFormat.Alignment = StringAlignment.Center
            strFormat.LineAlignment = StringAlignment.Center
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
            e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            e.Graphics.Clear(Me.BackColor)
            If _selectedIndex = 0 Or _hoverIndex = 0 Then
                Renderer.Button.draw(e.Graphics, _rectPredefined, , 2, True, , _selectedIndex = 0, _hoverIndex = 0)
            End If
            If _selectedIndex = 1 Or _hoverIndex = 1 Then
                Renderer.Button.draw(e.Graphics, _rectComponent, , 2, True, , _selectedIndex = 1, _hoverIndex = 1)
            End If
            e.Graphics.DrawString("Select", Renderer.ToolTip.TextFont, Brushes.Black, _rectPredefined, strFormat)
            e.Graphics.DrawString("Create", Renderer.ToolTip.TextFont, Brushes.Black, _rectComponent, strFormat)
            strFormat.Dispose()
        End Sub
    End Class
#End Region
#Region "Declarations"
    Dim WithEvents _tdown As ToolStripDropDown
    Dim _mruColors As ColorHistories
    Dim _selectedColor As Drawing.Color = Drawing.Color.Black
    Dim _mustRaiseEvent As Boolean = False
    Dim _alphaEnabled As Boolean = True
    Dim _rectSub As Rectangle
    Dim _hoverSub As Boolean = False
    Dim _onMouse As Boolean = False
    Dim _pressed As Boolean = False
    Dim _resumePainting As Boolean = True
    Dim cPopup As ColorPopup
    Dim _theme As Ai.Renderer.Drawing.ColorTheme = Ai.Renderer.Drawing.ColorTheme.Blue
#End Region
    Public Event SelectedColorChanged(ByVal sender As Object, ByVal e As EventArgs)
    Private Sub raiseMyEvent()
        _mustRaiseEvent = False
        _mruColors.Add(_selectedColor)
        RaiseEvent SelectedColorChanged(Me, New EventArgs)
    End Sub
    Public Sub New()
        MyBase.New()
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        Me.SetStyle(ControlStyles.Selectable, True)
        _mruColors = New ColorHistories(Me)
        Dim pHost As ToolStripControlHost
        _tdown = New ToolStripDropDown(Me)
        _rectSub = New Rectangle(Me.Width - 12, 0, 12, Me.Height)
        cPopup = New ColorPopup(Me)
        pHost = New ToolStripControlHost(cPopup)
        _tdown.Items.Clear()
        _tdown.Items.Add(pHost)
    End Sub
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Gets the collection of System.Drawing.Color objects assigned to the current tree node.")> _
    Public ReadOnly Property Histories() As ColorHistories
        Get
            Return _mruColors
        End Get
    End Property
    <DefaultValue(True)> _
    Public Property AlphaEnabled() As Boolean
        Get
            Return _alphaEnabled
        End Get
        Set(ByVal value As Boolean)
            _alphaEnabled = value
        End Set
    End Property
    <Browsable(False)> _
    Public Shadows Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
        End Set
    End Property
    <DefaultValue(GetType(Color), "0,0,0")> _
    Public Property SelectedColor() As Drawing.Color
        Get
            Return _selectedColor
        End Get
        Set(ByVal value As Drawing.Color)
            If _selectedColor <> value Then
                _selectedColor = value
                _mruColors.Add(_selectedColor)
                If _resumePainting Then
                    Me.Invalidate()
                End If
                RaiseEvent SelectedColorChanged(Me, New EventArgs)
            End If
        End Set
    End Property
    <DefaultValue(GetType(Ai.Renderer.Drawing.ColorTheme), "Blue")> _
    Public Property Theme() As Ai.Renderer.Drawing.ColorTheme
        Get
            Return _theme
        End Get
        Set(ByVal value As Ai.Renderer.Drawing.ColorTheme)
            If Not GetType(Ai.Renderer.Drawing.ColorTheme).IsAssignableFrom(value.GetType) Then
                Throw New ArgumentException("Value must be one of ColorTheme enumeration.", "value")
            Else
                If _theme <> value Then
                    _theme = value
                    'Me.Invalidate()
                End If
            End If
        End Set
    End Property
    Private Sub ColorChooser_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.EnabledChanged
        Me.Invalidate()
    End Sub
    Private Sub ColorChooser_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        If _resumePainting Then Me.Invalidate()
    End Sub
    Private Sub ColorChooser_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyData
            Case Keys.Return
                Dim clrDlg As ColorDialog = New ColorDialog
                Dim colorChanged As Boolean = False
                clrDlg.Color = _selectedColor
                If clrDlg.ShowDialog = DialogResult.OK Then
                    _selectedColor = clrDlg.Color
                    _mruColors.Add(_selectedColor)
                    colorChanged = True
                End If
                clrDlg.Dispose()
                Me.Invalidate()
                If colorChanged Then RaiseEvent SelectedColorChanged(Me, New EventArgs)
            Case Keys.Apps
                Dim scrPoint As Point = New Point(0, Me.Height)
                _resumePainting = False
                cPopup.setSelectedColor()
                _tdown.Show(Me, scrPoint.X, scrPoint.Y)
        End Select
    End Sub
    Private Sub ColorChooser_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        If _resumePainting Then Me.Invalidate()
    End Sub
    Private Sub ColorChooser_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If Not _pressed Then
                _pressed = True
                Dim pe As PaintEventArgs
                pe = New PaintEventArgs(Me.CreateGraphics, New Rectangle(0, 0, Me.Width, Me.Height))
                Me.InvokePaint(Me, pe)
                pe.Dispose()
            End If
            If _hoverSub Then
                Dim scrPoint As Point = New Point(0, Me.Height)
                _resumePainting = False
                cPopup.setSelectedColor()
                _tdown.Show(Me, scrPoint.X, scrPoint.Y)
            Else
                Dim clrDlg As ColorDialog = New ColorDialog
                Dim colorChanged As Boolean = False
                clrDlg.Color = _selectedColor
                If clrDlg.ShowDialog = DialogResult.OK Then
                    _selectedColor = clrDlg.Color
                    _mruColors.Add(_selectedColor)
                    colorChanged = True
                End If
                clrDlg.Dispose()
                Me.Invalidate()
                If colorChanged Then RaiseEvent SelectedColorChanged(Me, New EventArgs)
            End If
        End If
    End Sub
    Private Sub ColorChooser_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter
        _onMouse = True
        If _resumePainting Then
            Me.Invalidate()
        End If
    End Sub
    Private Sub ColorChooser_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        _onMouse = False
        _pressed = False
        _hoverSub = False
        If _resumePainting Then
            Me.Invalidate()
        End If
    End Sub
    Private Sub ColorChooser_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If _rectSub.Contains(e.X, e.Y) Then
            If Not _hoverSub Then
                _hoverSub = True
                If _resumePainting Then
                    Me.Invalidate()
                End If
            End If
        Else
            If _hoverSub Then
                _hoverSub = False
                If _resumePainting Then
                    Me.Invalidate()
                End If
            End If
        End If
    End Sub
    Private Sub ColorChooser_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        If _pressed And Not _hoverSub Then
            Dim cDialog As ColorDialog
            cDialog = New ColorDialog
            cDialog.Color = _selectedColor
            If cDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                _selectedColor = cDialog.Color
                Me.Invalidate()
            End If
            cDialog.Dispose()
        End If
    End Sub
    Private Sub ColorShooser_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim _rectColor As Rectangle, rect As Rectangle
        Dim pressedLocation As Ai.Renderer.Button.SplitEffectLocation = Renderer.Button.SplitEffectLocation.None
        Dim hoverLocation As Ai.Renderer.Button.SplitEffectLocation = Renderer.Button.SplitEffectLocation.None
        If Me.Enabled Then
            If _pressed Then
                If _hoverSub Then
                    pressedLocation = Renderer.Button.SplitEffectLocation.Split
                Else
                    pressedLocation = Renderer.Button.SplitEffectLocation.Button
                End If
            Else
                If _onMouse Then
                    If _hoverSub Then
                        hoverLocation = Renderer.Button.SplitEffectLocation.Split
                    Else
                        hoverLocation = Renderer.Button.SplitEffectLocation.Button
                    End If
                End If
            End If
        End If
        rect = New Rectangle(1, 1, Me.Width - 2, Me.Height - 2)
        _rectColor = New Rectangle(4, 4, Me.Width - 17, Me.Height - 8)
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        e.Graphics.Clear(Me.BackColor)
        Ai.Renderer.Button.drawSplit(e.Graphics, New Rectangle(0, 0, Me.Width, Me.Height), _
            Renderer.Button.SplitLocation.Right, 10, _theme, 2, Me.Enabled, _
            pressedLocation, False, hoverLocation, Me.Focused)
        e.Graphics.FillRectangle(New SolidBrush(_selectedColor), _rectColor)
        e.Graphics.DrawRectangle(Pens.Black, _rectColor)
        If Me.Enabled Then
            Renderer.Drawing.drawTriangle(e.Graphics, _rectSub, Color.FromArgb(21, 66, 139), Color.White, Renderer.Drawing.TriangleDirection.Down)
        Else
            Renderer.Drawing.drawTriangle(e.Graphics, _rectSub, Color.Gray, Color.White, Renderer.Drawing.TriangleDirection.Down)
        End If
    End Sub
    Private Sub ColorChooser_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        _rectSub = New Rectangle(Me.Width - 12, 0, 12, Me.Height)
    End Sub
    Private Sub _tdown_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles _tdown.Closed
        _resumePainting = True
        Me.Invalidate()
        If _mustRaiseEvent Then
            raiseMyEvent()
        End If
    End Sub
    Protected Overrides Function IsInputKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Select Case keyData
            Case Keys.Return, Keys.Apps
                Return True
            Case Else
                Return MyBase.IsInputKey(keyData)
        End Select
    End Function
End Class
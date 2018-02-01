Imports System.Drawing
Imports System.Drawing.Drawing2D
''' <summary>
''' Class for rendering list item.
''' </summary>
Public Class ListItem
#Region "Color Blend"
    ''' <summary>
    ''' Represent a color blend for selected item in a list that lost it focus input.
    ''' </summary>
    ''' <param name="theme">Them used to paint.</param>
    ''' <returns>ColorBlend.</returns>
    Public Shared ReadOnly Property SelectedBlurBlend(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As ColorBlend
        Get
            Dim colors(0 To 1) As Color
            Dim pos(0 To 1) As Single
            Dim blend As ColorBlend = New ColorBlend
            pos(0) = 0.0F
            pos(1) = 1.0F
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    colors(0) = Color.FromArgb(248, 248, 248)
                    colors(1) = Color.FromArgb(229, 229, 229)
            End Select
            blend.Colors = colors
            blend.Positions = pos
            Return blend
        End Get
    End Property
    ''' <summary>
    ''' Represent a color blend for selected item in focused list.
    ''' </summary>
    ''' <param name="theme">Them used to paint.</param>
    ''' <returns>ColorBlend.</returns>
    Public Shared ReadOnly Property SelectedBlend(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As ColorBlend
        Get
            Dim colors(0 To 1) As Color
            Dim pos(0 To 1) As Single
            Dim blend As ColorBlend = New ColorBlend
            pos(0) = 0.0F
            pos(1) = 1.0F
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    colors(0) = Color.FromArgb(241, 248, 253)
                    colors(1) = Color.FromArgb(213, 239, 252)
            End Select
            blend.Colors = colors
            blend.Positions = pos
            Return blend
        End Get
    End Property
    ''' <summary>
    ''' Represent a color blend for selected and highlighted item.
    ''' </summary>
    ''' <param name="theme">Them used to paint.</param>
    ''' <returns>ColorBlend.</returns>
    Public Shared ReadOnly Property SelectedHLiteBlend(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As ColorBlend
        Get
            Dim colors(0 To 1) As Color
            Dim pos(0 To 1) As Single
            Dim blend As ColorBlend = New ColorBlend
            pos(0) = 0.0F
            pos(1) = 1.0F
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    colors(0) = Color.FromArgb(232, 246, 253)
                    colors(1) = Color.FromArgb(196, 232, 250)
            End Select
            blend.Colors = colors
            blend.Positions = pos
            Return blend
        End Get
    End Property
    ''' <summary>
    ''' Represent a color blend for highlighted item.
    ''' </summary>
    ''' <param name="theme">Them used to paint.</param>
    ''' <returns>ColorBlend.</returns>
    Public Shared ReadOnly Property HLitedBlend(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As ColorBlend
        Get
            Dim colors(0 To 1) As Color
            Dim pos(0 To 1) As Single
            Dim blend As ColorBlend = New ColorBlend
            pos(0) = 0.0F
            pos(1) = 1.0F
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    colors(0) = Color.FromArgb(245, 250, 253)
                    colors(1) = Color.FromArgb(232, 245, 253)
            End Select
            blend.Colors = colors
            blend.Positions = pos
            Return blend
        End Get
    End Property
    ''' <summary>
    ''' Represent a color blend for pressed item.
    ''' </summary>
    ''' <param name="theme">Them used to paint.</param>
    ''' <returns>ColorBlend.</returns>
    Public Shared ReadOnly Property PressedBlend(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As ColorBlend
        Get
            Dim colors(0 To 1) As Color
            Dim pos(0 To 1) As Single
            Dim blend As ColorBlend = New ColorBlend
            pos(0) = 0.0F
            pos(1) = 1.0F
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    colors(0) = Color.FromArgb(160, 189, 227)
                    colors(1) = Color.FromArgb(255, 255, 255)
            End Select
            blend.Colors = colors
            blend.Positions = pos
            Return blend
        End Get
    End Property
#End Region
#Region "Border Pen"
    ''' <summary>
    ''' Represent a border pen for selected item in a list that lost it focus input.
    ''' </summary>
    ''' <param name="theme">Theme used to paint.</param>
    ''' <returns>Pen.</returns>
    Public Shared ReadOnly Property SelectedBlurBorderPen(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As Pen
        Get
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    Return New Pen(Color.FromArgb(217, 217, 217))
            End Select
            Return Pens.Black
        End Get
    End Property
    ''' <summary>
    ''' Represent a border pen for selected item in a focused list.
    ''' </summary>
    ''' <param name="theme">Theme used to paint.</param>
    ''' <returns>Pen.</returns>
    Public Shared ReadOnly Property SelectedBorderPen(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As Pen
        Get
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    Return New Pen(Color.FromArgb(177, 217, 229))
            End Select
            Return Pens.Black
        End Get
    End Property
    ''' <summary>
    ''' Represent a border pen for highlighted item in a list.
    ''' </summary>
    ''' <param name="theme">Theme used to paint.</param>
    ''' <returns>Pen.</returns>
    Public Shared ReadOnly Property HLitedBorderPen(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As Pen
        Get
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    Return New Pen(Color.FromArgb(185, 215, 252))
            End Select
            Return Pens.Black
        End Get
    End Property
    ''' <summary>
    ''' Represent a border pen for selected and highlighted item in a list.
    ''' </summary>
    ''' <param name="theme">Theme used to paint.</param>
    ''' <returns>Pen.</returns>
    Public Shared ReadOnly Property SelectedHLiteBorderPen(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As Pen
        Get
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    Return New Pen(Color.FromArgb(132, 172, 221))
            End Select
            Return Pens.Black
        End Get
    End Property
    ''' <summary>
    ''' Represent a border pen for pressed item in a list.
    ''' </summary>
    ''' <param name="theme">Theme used to paint.</param>
    ''' <returns>Pen.</returns>
    Public Shared ReadOnly Property PressedBorderPen(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As Pen
        Get
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    Return New Pen(Color.FromArgb(104, 140, 175))
            End Select
            Return Pens.Black
        End Get
    End Property
#End Region
#Region "Draw"
    ''' <summary>
    ''' Draw an item background in the list.
    ''' </summary>
    ''' <param name="g">Graphics object where the item background to be drawn.</param>
    ''' <param name="rect">Bounding rectangle of the item.</param>
    ''' <param name="theme">Theme used to paint.</param>
    ''' <param name="rounded">Rounded range of each rectangle's corner.</param>
    ''' <param name="enabled">Determine whether the list is enabled.</param>
    ''' <param name="focused">Determine whether the list has input focus.</param>
    ''' <param name="pressed">Determine whether the item is on the pressed state.</param>
    ''' <param name="selected">Determine whether the item is selected.</param>
    ''' <param name="hLited">Determine whether the item is highlighted.</param>
    Public Shared Sub draw(ByVal g As Graphics, ByVal rect As Rectangle, _
        Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue, _
        Optional ByVal rounded As Integer = 2, Optional ByVal enabled As Boolean = True, _
        Optional ByVal focused As Boolean = True, Optional ByVal pressed As Boolean = False, _
        Optional ByVal selected As Boolean = False, Optional ByVal hLited As Boolean = False)
        Dim itemBrush As LinearGradientBrush = New LinearGradientBrush(rect, Color.Black, _
            Color.White, LinearGradientMode.Vertical)
        Dim itemPath As GraphicsPath = Drawing.roundedRectangle(rect, rounded, rounded, rounded, rounded)
        Dim itemBorder As Pen = Nothing
        If enabled Then
            If pressed Then
                itemBrush.InterpolationColors = PressedBlend(theme)
                itemBorder = PressedBorderPen(theme)
            Else
                If selected Then
                    If hLited Then
                        itemBrush.InterpolationColors = SelectedHLiteBlend(theme)
                        itemBorder = SelectedHLiteBorderPen(theme)
                    Else
                        If focused Then
                            itemBrush.InterpolationColors = SelectedBlend(theme)
                            itemBorder = SelectedBorderPen(theme)
                        Else
                            itemBrush.InterpolationColors = SelectedBlurBlend(theme)
                            itemBorder = SelectedBlurBorderPen(theme)
                        End If
                    End If
                Else
                    If hLited Then
                        itemBrush.InterpolationColors = HLitedBlend(theme)
                        itemBorder = HLitedBorderPen(theme)
                    End If
                End If
            End If
        Else
            If selected Then
                Dim colors(0 To 1) As Color
                colors(0) = Color.LightGray
                colors(1) = Color.LightGray
                itemBorder = Pens.Gray
                itemBrush.LinearColors = colors
            End If
        End If
        If itemBorder IsNot Nothing Then
            g.FillPath(itemBrush, itemPath)
            g.DrawPath(itemBorder, itemPath)
            itemBorder.Dispose()
        End If
        itemBrush.Dispose()
        itemPath.Dispose()
    End Sub
#End Region
End Class
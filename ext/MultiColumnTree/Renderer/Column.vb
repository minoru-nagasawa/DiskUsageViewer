Imports System.Drawing
Imports System.Drawing.Drawing2D
''' <summary>
''' Class for rendering column header.
''' </summary>
Public Class Column
#Region "Color Blend"
    ''' <summary>
    ''' Represent a color blend for a normal column.
    ''' </summary>
    ''' <param name="theme">Theme used to paint.</param>
    ''' <return>ColorBlend</return>
    Public Shared ReadOnly Property NormalBlend(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As ColorBlend
        Get
            Dim colors(0 To 3) As Color
            Dim pos(0 To 3) As Single
            Dim blend As ColorBlend = New ColorBlend
            pos(0) = 0.0F
            pos(1) = 0.4F
            pos(2) = 0.4F
            pos(3) = 1.0F
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    colors(0) = Color.FromArgb(255, 255, 255)
                    colors(1) = Color.FromArgb(255, 255, 255)
                    colors(2) = Color.FromArgb(247, 248, 250)
                    colors(3) = Color.FromArgb(241, 242, 244)
            End Select
            blend.Colors = colors
            blend.Positions = pos
            Return blend
        End Get
    End Property
    ''' <summary>
    ''' Represent a color blend for a selected column.
    ''' </summary>
    ''' <param name="theme">Theme used to paint.</param>
    ''' <return>ColorBlend</return>
    Public Shared ReadOnly Property SelectedBlend(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As ColorBlend
        Get
            Dim colors(0 To 3) As Color
            Dim pos(0 To 3) As Single
            Dim blend As ColorBlend = New ColorBlend
            pos(0) = 0.0F
            pos(1) = 0.4F
            pos(2) = 0.4F
            pos(3) = 1.0F
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    colors(0) = Color.FromArgb(242, 249, 252)
                    colors(1) = Color.FromArgb(242, 249, 252)
                    colors(2) = Color.FromArgb(225, 241, 249)
                    colors(3) = Color.FromArgb(216, 236, 246)
            End Select
            blend.Colors = colors
            blend.Positions = pos
            Return blend
        End Get
    End Property
    ''' <summary>
    ''' Represent a color blend for a highlited column.
    ''' </summary>
    ''' <param name="theme">Theme used to paint.</param>
    ''' <return>ColorBlend</return>
    Public Shared ReadOnly Property HLitedBlend(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As ColorBlend
        Get
            Dim colors(0 To 3) As Color
            Dim pos(0 To 3) As Single
            Dim blend As ColorBlend = New ColorBlend
            pos(0) = 0.0F
            pos(1) = 0.4F
            pos(2) = 0.4F
            pos(3) = 1.0F
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    colors(0) = Color.FromArgb(227, 247, 255)
                    colors(1) = Color.FromArgb(227, 247, 255)
                    colors(2) = Color.FromArgb(189, 237, 255)
                    colors(3) = Color.FromArgb(183, 231, 251)
            End Select
            blend.Colors = colors
            blend.Positions = pos
            Return blend
        End Get
    End Property
    ''' <summary>
    ''' Represent a color blend for a highlited column's dropdown.
    ''' </summary>
    ''' <param name="theme">Theme used to paint.</param>
    ''' <return>ColorBlend</return>
    Public Shared ReadOnly Property HLitedDropDownBlend(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As ColorBlend
        Get
            Dim colors(0 To 3) As Color
            Dim pos(0 To 3) As Single
            Dim blend As ColorBlend = New ColorBlend
            pos(0) = 0.0F
            pos(1) = 0.4F
            pos(2) = 0.4F
            pos(3) = 1.0F
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    colors(0) = Color.FromArgb(205, 242, 255)
                    colors(1) = Color.FromArgb(205, 242, 255)
                    colors(2) = Color.FromArgb(140, 224, 255)
                    colors(3) = Color.FromArgb(136, 217, 251)
            End Select
            blend.Colors = colors
            blend.Positions = pos
            Return blend
        End Get
    End Property
    ''' <summary>
    ''' Represent a color blend for a pressed column.
    ''' </summary>
    ''' <param name="theme">Theme used to paint.</param>
    ''' <return>ColorBlend</return>
    Public Shared ReadOnly Property PressedBlend(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As ColorBlend
        Get
            Dim colors(0 To 3) As Color
            Dim pos(0 To 3) As Single
            Dim blend As ColorBlend = New ColorBlend
            pos(0) = 0.0F
            pos(1) = 0.4F
            pos(2) = 0.4F
            pos(3) = 1.0F
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    colors(0) = Color.FromArgb(188, 228, 249)
                    colors(1) = Color.FromArgb(188, 228, 249)
                    colors(2) = Color.FromArgb(141, 214, 247)
                    colors(3) = Color.FromArgb(138, 209, 245)
            End Select
            blend.Colors = colors
            blend.Positions = pos
            Return blend
        End Get
    End Property
#End Region
#Region "Border Pen"
    ''' <summary>
    ''' Represent a pen for a normal column border.
    ''' </summary>
    ''' <param name="theme">Theme used to paint.</param>
    ''' <returns>Pen</returns>
    Public Shared ReadOnly Property NormalBorderPen(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As Pen
        Get
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    Return New Pen(Color.FromArgb(213, 213, 213))
            End Select
            Return Pens.Black
        End Get
    End Property
    ''' <summary>
    ''' Represent a pen for an active column border.
    ''' </summary>
    ''' <param name="theme">Theme used to paint.</param>
    ''' <returns>Pen</returns>
    Public Shared ReadOnly Property ActiveBorderPen(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) _
        As Pen
        Get
            Select Case theme
                Case Drawing.ColorTheme.Blue
                    Return New Pen(Color.FromArgb(147, 201, 227))
            End Select
            Return Pens.Black
        End Get
    End Property
#End Region
#Region "Text Brushes"
    Public Shared ReadOnly Property TextBrush(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) As SolidBrush
        Get
            Select Case theme
                Case Drawing.ColorTheme.Blue, Drawing.ColorTheme.BlackBlue
                    Return New SolidBrush(Color.FromArgb(62, 106, 170))
            End Select
            Return Nothing
        End Get
    End Property
    Public Shared ReadOnly Property DisabledTextBrush(Optional ByVal theme As Drawing.ColorTheme = Drawing.ColorTheme.Blue) As SolidBrush
        Get
            Select Case theme
                Case Drawing.ColorTheme.Blue, Drawing.ColorTheme.BlackBlue
                    Return New SolidBrush(Color.FromArgb(118, 118, 118))
            End Select
            Return Nothing
        End Get
    End Property
#End Region
#Region "Drawing"
    ''' <summary>
    ''' Draw a shadow effect for a pressed column, with specified graphics object, rectangle, and step.
    ''' </summary>
    Public Shared Sub drawPressedShadow(ByVal g As Graphics, ByVal rect As Rectangle, _
        Optional ByVal stepCount As Integer = 3)
        If stepCount <= 0 Then Return
        If rect.Width = 0 Or rect.Height = 0 Then Return
        Dim alpha As Integer = 4 + (stepCount * 8)
        Dim i As Integer = 0
        Dim shadowPoints(0 To 3) As Point
        With rect
            shadowPoints(0).X = .X
            shadowPoints(0).Y = .Bottom - 1
            shadowPoints(1).X = .X
            shadowPoints(1).Y = .Y
            shadowPoints(2).X = .Right - 1
            shadowPoints(2).Y = .Y
            shadowPoints(3).X = .Right - 1
            shadowPoints(3).Y = .Bottom - 1
        End With
        While i < stepCount
            g.DrawLine(New Pen(Color.FromArgb(alpha, 0, 0, 0)), shadowPoints(0), shadowPoints(1))
            g.DrawLine(New Pen(Color.FromArgb(alpha, 0, 0, 0)), shadowPoints(1), shadowPoints(2))
            g.DrawLine(New Pen(Color.FromArgb(alpha, 0, 0, 0)), shadowPoints(2), shadowPoints(3))
            alpha -= 8
            i += 1
            shadowPoints(0).X += 1
            shadowPoints(1).X += 1
            shadowPoints(1).Y += 1
            shadowPoints(2).X -= 1
            shadowPoints(2).Y += 1
            shadowPoints(3).X -= 1
        End While
    End Sub
#End Region
End Class
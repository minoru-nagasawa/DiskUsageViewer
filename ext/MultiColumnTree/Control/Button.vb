Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Public Class Button
    Inherits System.Windows.Forms.Button
    Dim _onMouse As Boolean = False
    Dim _pressed As Boolean = False
    Dim _theme As Ai.Renderer.Drawing.ColorTheme = Ai.Renderer.Drawing.ColorTheme.Blue
    Public Sub New()
        MyBase.New()
        ' Initialization
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub
    Private Sub Button_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.EnabledChanged
        Me.Invalidate()
    End Sub
    Private Sub Button_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        Me.Invalidate()
    End Sub
    Private Sub Button_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        Me.Invalidate()
    End Sub
    Private Sub Button_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            _pressed = True
            Me.Invalidate()
        End If
    End Sub
    Private Sub Button_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter
        _onMouse = True
        Me.Invalidate()
    End Sub
    Private Sub Button_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        _onMouse = False
        _pressed = False
        Me.Invalidate()
    End Sub
    Private Sub Button_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        If _pressed Then
            _pressed = False
            Me.Invalidate()
        End If
    End Sub
    Private Sub Button_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim rect As Rectangle
        Dim txtFormat As StringFormat
        txtFormat = New StringFormat
        txtFormat.LineAlignment = StringAlignment.Center
        txtFormat.HotkeyPrefix = Drawing.Text.HotkeyPrefix.Show
        Select Case Me.TextAlign
            Case ContentAlignment.BottomCenter, ContentAlignment.BottomLeft, ContentAlignment.BottomRight
                txtFormat.LineAlignment = StringAlignment.Far
            Case ContentAlignment.MiddleCenter, ContentAlignment.MiddleLeft, ContentAlignment.MiddleRight
                txtFormat.LineAlignment = StringAlignment.Center
            Case ContentAlignment.TopCenter, ContentAlignment.TopLeft, ContentAlignment.TopRight
                txtFormat.LineAlignment = StringAlignment.Near
        End Select
        txtFormat.Alignment = StringAlignment.Center
        Select Case Me.TextAlign
            Case ContentAlignment.BottomLeft, ContentAlignment.MiddleLeft, ContentAlignment.TopLeft
                txtFormat.Alignment = StringAlignment.Near
            Case ContentAlignment.BottomCenter, ContentAlignment.MiddleCenter, ContentAlignment.TopCenter
                txtFormat.Alignment = StringAlignment.Center
            Case ContentAlignment.BottomRight, ContentAlignment.MiddleRight, ContentAlignment.TopRight
                txtFormat.Alignment = StringAlignment.Far
        End Select
        rect = New Rectangle(1, 1, Me.Width - 2, Me.Height - 2)
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        e.Graphics.Clear(Me.BackColor)
        Ai.Renderer.Button.draw(e.Graphics, rect, Theme, 2, Me.Enabled, _pressed, False, _onMouse, Me.Focused)
        If Me.Enabled Then
            e.Graphics.DrawString(Me.Text, Me.Font, Ai.Renderer.Drawing.NormalTextBrush(_theme), rect, txtFormat)
        Else
            e.Graphics.DrawString(Me.Text, Me.Font, Ai.Renderer.Drawing.DisabledTextBrush(_theme), rect, txtFormat)
        End If
        If MyBase.Image IsNot Nothing Then
            Dim imgRect As Rectangle = Ai.Renderer.Drawing.getImageRectangle( _
                MyBase.Image, rect, _
                IIf(rect.Width > rect.Height, rect.Height - 4, rect.Width - 4))
            If Me.Enabled Then
                e.Graphics.DrawImage(MyBase.Image, imgRect)
            Else
                Ai.Renderer.Drawing.grayscaledImage(MyBase.Image, imgRect, e.Graphics)
            End If
        End If
        txtFormat.Dispose()
    End Sub
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
                    Me.Invalidate()
                End If
            End If
        End Set
    End Property
    Public Overloads Property Image() As Image
        Get
            Return MyBase.Image
        End Get
        Set(ByVal value As Image)
            MyBase.Image = value
            Me.Invalidate()
        End Set
    End Property
    Private Sub Button_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.TextChanged
        Me.Invalidate()
    End Sub
End Class
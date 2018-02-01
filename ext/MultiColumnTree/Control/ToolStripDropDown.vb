Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.Drawing
Imports System.Windows.Forms
Public Class ToolStripDropDown
    Inherits System.Windows.Forms.ToolStripDropDown
    Public Enum SizingGripMode
        BottomRight
        Bottom
        None
    End Enum
    Dim _showSizingGrip As SizingGripMode = SizingGripMode.None
    Dim _startPoint As Point
    Dim _gripRect As Rectangle
    Dim _resizing As Boolean = False
    Dim _opened As Boolean = False
    Dim _owner As System.Windows.Forms.Control = Nothing
    Private Const WM_NCLBUTTONDOWN = &HA1
    Private Const WM_NCCALCSIZE = &H83
    Private Const HTBOTTOMRIGHT As Integer = 17
    Private Const HTBOTTOM = 15
    Private Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (ByVal hwnd As IntPtr, _
        ByVal wMsg As Integer, ByVal wParam As Integer, _
        ByVal lParam As Integer) As Integer
    Private Declare Sub ReleaseCapture Lib "user32.dll" ()
    Public Sub New()
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, False)
        Me.DropShadowEnabled = False
        Me.Padding = New Padding(2)
    End Sub
    Public Sub New(ByVal owner As System.Windows.Forms.Control)
        _owner = owner
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, False)
        Me.DropShadowEnabled = False
        Me.Padding = New Padding(2)
    End Sub
    Public Function getForm() As Form
        If _owner IsNot Nothing Then
            If TypeOf (_owner) Is ToolStripDropDown Then
                Dim own As ToolStripDropDown = _owner
                Return own.getForm
            Else
                Return _owner.FindForm
            End If
        Else
            Return Nothing
        End If
    End Function
    <DefaultValue(False)> _
    Public Property SizingGrip() As SizingGripMode
        Get
            Return _showSizingGrip
        End Get
        Set(ByVal value As SizingGripMode)
            If _showSizingGrip <> value Then
                _showSizingGrip = value
                If _showSizingGrip <> SizingGripMode.None Then
                    Me.Padding = New Padding(2, 2, 2, 10)
                Else
                    Me.Padding = New Padding(2)
                End If
                Me.Invalidate()
            End If
        End Set
    End Property
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim aRect As Rectangle
        Dim oPath As System.Drawing.Drawing2D.GraphicsPath
        Dim _splitBrush As LinearGradientBrush
        Dim _splitRect As Rectangle = New Rectangle(0, Me.Height - 10, Me.Width, 10)
        aRect = New Rectangle(0, 0, Me.Width - 1, Me.Height - 1)
        oPath = Ai.Renderer.Drawing.roundedRectangle(aRect, 2, 2, 2, 2)
        oPath.CloseFigure()
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        e.Graphics.FillPath(New SolidBrush(Color.FromArgb(250, 250, 250)), oPath)
        If _showSizingGrip <> SizingGripMode.None Then
            _splitBrush = New System.Drawing.Drawing2D.LinearGradientBrush(_splitRect, _
                Color.Black, Color.White, Drawing2D.LinearGradientMode.Vertical)
            _splitBrush.InterpolationColors = Ai.Renderer.Drawing.SizingGripBlend
            e.Graphics.FillRectangle(_splitBrush, _splitRect)
            If _showSizingGrip = SizingGripMode.BottomRight Then
                Ai.Renderer.Drawing.drawGrip(Me.Width - 12, Me.Height - 12, e.Graphics)
            Else
                Ai.Renderer.Drawing.drawVGrip(_splitRect, e.Graphics)
            End If
            e.Graphics.DrawLine(Ai.Renderer.Drawing.GripBorderPen, 0, _splitRect.Y, Me.Width, _splitRect.Y)
            _splitBrush.Dispose()
        End If
        e.Graphics.DrawPath(New Pen(Color.FromArgb(134, 134, 134)), oPath)
        oPath.Dispose()
        aRect = New Rectangle(0, 0, Me.Width, Me.Height)
        oPath = Ai.Renderer.Drawing.roundedRectangle(aRect, 2, 2, 2, 2)
        Me.Region = New Region(oPath)
        oPath.Dispose()
    End Sub
    Private Sub ToolStripDropDown_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles Me.Closed
        _opened = False
    End Sub
    Private Sub ToolStripDropDown_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If _showSizingGrip <> SizingGripMode.None And e.Button = Windows.Forms.MouseButtons.Left Then
            If Me.Cursor = Cursors.SizeNWSE Then
                _resizing = True
                _startPoint = Me.PointToScreen(e.Location)
                ReleaseCapture()
                SendMessage(Me.Handle, WM_NCLBUTTONDOWN, HTBOTTOMRIGHT, 0)
            ElseIf Me.Cursor = Cursors.SizeNS Then
                _resizing = True
                _startPoint = Me.PointToScreen(e.Location)
                ReleaseCapture()
                SendMessage(Me.Handle, WM_NCLBUTTONDOWN, HTBOTTOM, 0)
            End If
        End If
    End Sub
    Private Sub ToolStripDropDown_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If _showSizingGrip <> SizingGripMode.None Then
            If _showSizingGrip = SizingGripMode.BottomRight Then
                If _gripRect.Contains(e.X, e.Y) Then
                    Me.Cursor = Cursors.SizeNWSE
                Else
                    Me.Cursor = Cursors.Default
                End If
            Else
                Dim _splitRect As Rectangle = New Rectangle(0, Me.Height - 10, Me.Width, 10)
                If _splitRect.Contains(e.X, e.Y) Then
                    Me.Cursor = Cursors.SizeNS
                Else
                    Me.Cursor = Cursors.Default
                End If
            End If
        End If
    End Sub
    Private Sub ToolStripDropDown_Opened(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Opened
        Dim pt As Point = New Point(0, 0)
        Dim aHost As ToolStripControlHost
        pt = Me.PointToScreen(pt)
        If TypeOf (Me.Items(0)) Is ToolStripControlHost Then
            aHost = Me.Items(0)
            aHost.Control.Focus()
        End If
        _opened = True
    End Sub
    Private Sub ToolStripDropDown_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        _gripRect = New Rectangle(Me.Width - 10, Me.Height - 10, 10, 10)
        If _opened Then
            Dim pt As Point = New Point(0, 0)
            pt = Me.PointToScreen(pt)
        End If
    End Sub
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If _resizing Then
            If m.Msg = WM_NCCALCSIZE Then
                Dim dx As Integer = System.Windows.Forms.Control.MousePosition.X - _startPoint.X
                Dim dy As Integer = System.Windows.Forms.Control.MousePosition.Y - _startPoint.Y
                _resizing = False
                If Me.Items(0) IsNot Nothing Then
                    Dim aHost As ToolStripControlHost = Me.Items(0)
                    If _showSizingGrip = SizingGripMode.BottomRight Then
                        aHost.Control.Width = aHost.Control.Width + dx
                    End If
                    aHost.Control.Height = aHost.Control.Height + dy
                End If
            Else
                MyBase.WndProc(m)
            End If
        Else
            MyBase.WndProc(m)
        End If
    End Sub
End Class
Imports System.Drawing.Drawing2D
Public Class Form1
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub
    Private Sub assignSubItem(ByVal node As Ai.Control.TreeNode)
        Dim colIndex As Integer = 1
        node.Image = PictureBox1.Image
        node.ExpandedImage = PictureBox2.Image
        While colIndex < tree.Columns.Count
            If colIndex = 2 Then
                Dim randomValue As Double = Rnd() * 100
                node.SubItems.Add(randomValue)
                Select Case randomValue
                    Case Is < 25
                        node.SubItems(colIndex).Color = Color.Green
                    Case Is > 75
                        node.SubItems(colIndex).Color = Color.Red
                    Case Else
                        node.SubItems(colIndex).Color = Color.Blue
                End Select
            Else
                node.SubItems.Add("SubItem " & CStr(node.Level) & CStr(colIndex))
            End If
            colIndex += 1
        End While
    End Sub
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        tree.CheckBoxes = CheckBox1.Checked
    End Sub
    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        tree.AllowMultiline = CheckBox2.Checked
    End Sub
    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        tree.FullRowSelect = CheckBox3.Checked
    End Sub
    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        tree.ShowImages = CheckBox4.Checked
    End Sub
    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox5.CheckedChanged
        tree.LabelEdit = CheckBox5.Checked
    End Sub
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim tn As Ai.Control.TreeNode, lv1Child As Ai.Control.TreeNode, lv2Child As Ai.Control.TreeNode
        Dim i As Integer, j As Integer, k As Integer
        i = 0
        While i < 3
            tn = New Ai.Control.TreeNode
            If i = 1 Then
                ' Testing multiline text
                tn.Text = "Node " & CStr(i) & vbCrLf & "Second line."
            Else
                tn.Text = "Node " & CStr(i)
            End If
            assignSubItem(tn)
            ' Assign 3 level 1 child nodes.
            j = 0
            While j < 3
                lv1Child = New Ai.Control.TreeNode
                lv1Child.Text = "Child " & CStr(i) & CStr(j)
                assignSubItem(lv1Child)
                ' Assign 5 level 2 child nodes.
                k = 0
                While k < 5
                    lv2Child = New Ai.Control.TreeNode
                    lv2Child.Text = "Child " & CStr(i) & CStr(j) & CStr(k)
                    If i = 1 And j = 1 And k = 3 Then
                        ' Create custom tooltip on following node
                        lv2Child.ToolTipTitle = lv2Child.Text
                        lv2Child.ToolTip = "This is tooltip information on a node."
                    End If
                    assignSubItem(lv2Child)
                    lv1Child.Nodes.Add(lv2Child)
                    k += 1
                End While
                tn.Nodes.Add(lv1Child)
                j += 1
            End While
            tree.Nodes.Add(tn)
            i += 1
        End While
    End Sub
    Private Sub tree_ColumnBackgroundPaint(ByVal sender As Object, ByVal e As Ai.Control.ColumnBackgroundPaintEventArgs) Handles tree.ColumnBackgroundPaint
        ' perform custom background paint on column 1
        If tree.Columns.IndexOf(e.Column) = 1 Then
            Dim customBrush As LinearGradientBrush = New LinearGradientBrush(e.Rectangle, _
                Color.White, Color.SkyBlue, LinearGradientMode.Horizontal)
            e.Graphics.FillRectangle(customBrush, e.Rectangle)
            customBrush.Dispose()
        End If
    End Sub
    Private Sub tree_ColumnCustomFilter(ByVal sender As Object, ByVal e As Ai.Control.ColumnCustomFilterEventArgs) Handles tree.ColumnCustomFilter
        ' Perform your custom filter dialog here.
        ' If you want to cancel the operation, set e.CancelFilter to true.
        ' Add your custom filtering like this.
        e.Column.CustomFilter = AddressOf column1CustomFilter
    End Sub
    Private Function column1CustomFilter(ByVal value As Object) As Boolean
        ' This is just a sample, to perform custom filtering operation on specified column.
        ' Return true if the value is meet your custom filter parameter, false otherwise.
        Return True
    End Function
End Class
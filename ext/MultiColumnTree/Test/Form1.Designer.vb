<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim ColumnHeader1 As Ai.Control.ColumnHeader = New Ai.Control.ColumnHeader
        Dim ColumnHeader2 As Ai.Control.ColumnHeader = New Ai.Control.ColumnHeader
        Dim ColumnHeader3 As Ai.Control.ColumnHeader = New Ai.Control.ColumnHeader
        Dim ColumnHeader4 As Ai.Control.ColumnHeader = New Ai.Control.ColumnHeader
        Dim ColumnHeader5 As Ai.Control.ColumnHeader = New Ai.Control.ColumnHeader
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.Button1 = New System.Windows.Forms.Button
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.tree = New Ai.Control.MultiColumnTree
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.CheckBox5 = New System.Windows.Forms.CheckBox
        Me.CheckBox4 = New System.Windows.Forms.CheckBox
        Me.CheckBox3 = New System.Windows.Forms.CheckBox
        Me.CheckBox2 = New System.Windows.Forms.CheckBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(410, 352)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Button1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(402, 326)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(8, 6)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.tree)
        Me.TabPage2.Controls.Add(Me.Panel1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(402, 326)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'tree
        '
        ColumnHeader1.CustomFilter = Nothing
        ColumnHeader1.EnableFiltering = True
        ColumnHeader1.EnableFrozen = True
        ColumnHeader1.EnableSorting = True
        ColumnHeader1.MaximumValue = 0
        ColumnHeader1.MinimumValue = 0
        ColumnHeader1.Tag = Nothing
        ColumnHeader1.Text = "Column 0"
        ColumnHeader2.CustomFilter = Nothing
        ColumnHeader2.EnableCustomFilter = True
        ColumnHeader2.EnableFiltering = True
        ColumnHeader2.EnableHidden = True
        ColumnHeader2.EnableSorting = True
        ColumnHeader2.MaximumValue = 0
        ColumnHeader2.MinimumValue = 0
        ColumnHeader2.Tag = Nothing
        ColumnHeader2.Text = "Column 1"
        ColumnHeader3.CustomFilter = Nothing
        ColumnHeader3.EnableFrozen = True
        ColumnHeader3.EnableSorting = True
        ColumnHeader3.Format = Ai.Control.ColumnFormat.Bar
        ColumnHeader3.MaximumValue = 100
        ColumnHeader3.MinimumValue = 0
        ColumnHeader3.Tag = Nothing
        ColumnHeader3.Text = "Column 2"
        ColumnHeader4.CustomFilter = Nothing
        ColumnHeader4.EnableHidden = True
        ColumnHeader4.EnableSorting = True
        ColumnHeader4.MaximumValue = 0
        ColumnHeader4.MinimumValue = 0
        ColumnHeader4.Tag = Nothing
        ColumnHeader4.Text = "Column 3"
        ColumnHeader5.CustomFilter = Nothing
        ColumnHeader5.MaximumValue = 0
        ColumnHeader5.MinimumValue = 0
        ColumnHeader5.Tag = Nothing
        ColumnHeader5.Text = "Column 4"
        Me.tree.Columns.Add(ColumnHeader1)
        Me.tree.Columns.Add(ColumnHeader2)
        Me.tree.Columns.Add(ColumnHeader3)
        Me.tree.Columns.Add(ColumnHeader4)
        Me.tree.Columns.Add(ColumnHeader5)
        Me.tree.Culture = New System.Globalization.CultureInfo("en-US")
        Me.tree.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tree.Indent = -1
        Me.tree.Location = New System.Drawing.Point(3, 3)
        Me.tree.Name = "tree"
        Me.tree.Padding = New System.Windows.Forms.Padding(1)
        Me.tree.SelectedNode = Nothing
        Me.tree.Size = New System.Drawing.Size(396, 192)
        Me.tree.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.PictureBox2)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.CheckBox5)
        Me.Panel1.Controls.Add(Me.CheckBox4)
        Me.Panel1.Controls.Add(Me.CheckBox3)
        Me.Panel1.Controls.Add(Me.CheckBox2)
        Me.Panel1.Controls.Add(Me.CheckBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(3, 195)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(396, 128)
        Me.Panel1.TabIndex = 2
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(316, 49)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(23, 24)
        Me.PictureBox2.TabIndex = 6
        Me.PictureBox2.TabStop = False
        Me.PictureBox2.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(301, 26)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(23, 24)
        Me.PictureBox1.TabIndex = 6
        Me.PictureBox1.TabStop = False
        Me.PictureBox1.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 103)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Behaviour"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Appearances"
        '
        'CheckBox5
        '
        Me.CheckBox5.AutoSize = True
        Me.CheckBox5.Location = New System.Drawing.Point(150, 102)
        Me.CheckBox5.Name = "CheckBox5"
        Me.CheckBox5.Size = New System.Drawing.Size(76, 17)
        Me.CheckBox5.TabIndex = 4
        Me.CheckBox5.Text = "Label edit"
        Me.CheckBox5.UseVisualStyleBackColor = True
        '
        'CheckBox4
        '
        Me.CheckBox4.AutoSize = True
        Me.CheckBox4.Checked = True
        Me.CheckBox4.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox4.Location = New System.Drawing.Point(150, 79)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.Size = New System.Drawing.Size(94, 17)
        Me.CheckBox4.TabIndex = 3
        Me.CheckBox4.Text = "Show images"
        Me.CheckBox4.UseVisualStyleBackColor = True
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.Location = New System.Drawing.Point(150, 56)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(100, 17)
        Me.CheckBox3.TabIndex = 2
        Me.CheckBox3.Text = "Full row select"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(150, 33)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(104, 17)
        Me.CheckBox2.TabIndex = 1
        Me.CheckBox2.Text = "Allow Multiline"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(150, 10)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(87, 17)
        Me.CheckBox1.TabIndex = 0
        Me.CheckBox1.Text = "Checkboxes"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(410, 352)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents tree As Ai.Control.MultiColumnTree
    Private WithEvents Panel1 As System.Windows.Forms.Panel
    Private WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Private WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Private WithEvents Label2 As System.Windows.Forms.Label
    Private WithEvents Label1 As System.Windows.Forms.Label
    Private WithEvents CheckBox5 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox4 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Private WithEvents Button1 As System.Windows.Forms.Button
    Private WithEvents TabControl1 As System.Windows.Forms.TabControl
    Private WithEvents TabPage1 As System.Windows.Forms.TabPage
    Private WithEvents TabPage2 As System.Windows.Forms.TabPage

End Class

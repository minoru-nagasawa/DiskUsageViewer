namespace DiskUsageViewer
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Ai.Control.ColumnHeader columnHeader11 = new Ai.Control.ColumnHeader();
            Ai.Control.ColumnHeader columnHeader12 = new Ai.Control.ColumnHeader();
            Ai.Control.ColumnHeader columnHeader13 = new Ai.Control.ColumnHeader();
            Ai.Control.ColumnHeader columnHeader14 = new Ai.Control.ColumnHeader();
            Ai.Control.ColumnHeader columnHeader15 = new Ai.Control.ColumnHeader();
            this.label2 = new System.Windows.Forms.Label();
            this.c_btnBrowse = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.c_statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.c_btnRun = new System.Windows.Forms.Button();
            this.c_textRootFolder = new System.Windows.Forms.TextBox();
            this.c_treeView = new Ai.Control.MultiColumnTree();
            this.c_timerStatusUpdate = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.c_menuLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.c_menuTool = new System.Windows.Forms.ToolStripMenuItem();
            this.c_menuHumanReadable = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Root Folder";
            // 
            // c_btnBrowse
            // 
            this.c_btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.c_btnBrowse.Location = new System.Drawing.Point(516, 28);
            this.c_btnBrowse.Name = "c_btnBrowse";
            this.c_btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.c_btnBrowse.TabIndex = 2;
            this.c_btnBrowse.Text = "Browse";
            this.c_btnBrowse.UseVisualStyleBackColor = true;
            this.c_btnBrowse.Click += new System.EventHandler(this.c_btnBrowse_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.c_statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 495);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(603, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // c_statusLabel
            // 
            this.c_statusLabel.Name = "c_statusLabel";
            this.c_statusLabel.Size = new System.Drawing.Size(118, 17);
            this.c_statusLabel.Text = "toolStripStatusLabel1";
            // 
            // c_btnRun
            // 
            this.c_btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.c_btnRun.Location = new System.Drawing.Point(516, 469);
            this.c_btnRun.Name = "c_btnRun";
            this.c_btnRun.Size = new System.Drawing.Size(75, 23);
            this.c_btnRun.TabIndex = 2;
            this.c_btnRun.Text = "Scan";
            this.c_btnRun.UseVisualStyleBackColor = true;
            this.c_btnRun.Click += new System.EventHandler(this.c_btnRun_Click);
            // 
            // c_textRootFolder
            // 
            this.c_textRootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.c_textRootFolder.Location = new System.Drawing.Point(83, 30);
            this.c_textRootFolder.Name = "c_textRootFolder";
            this.c_textRootFolder.Size = new System.Drawing.Size(427, 19);
            this.c_textRootFolder.TabIndex = 9;
            // 
            // c_treeView
            // 
            this.c_treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            columnHeader11.CustomFilter = null;
            columnHeader11.MaximumValue = 0D;
            columnHeader11.MinimumValue = 0D;
            columnHeader11.Name = "Column1";
            columnHeader11.Tag = null;
            columnHeader11.Text = "Name";
            columnHeader11.Width = 200;
            columnHeader12.ColumnAlign = System.Windows.Forms.HorizontalAlignment.Right;
            columnHeader12.CustomFilter = null;
            columnHeader12.EnableSorting = true;
            columnHeader12.MaximumValue = 0D;
            columnHeader12.MinimumValue = 0D;
            columnHeader12.Name = "Column2";
            columnHeader12.SortOrder = System.Windows.Forms.SortOrder.Descending;
            columnHeader12.Tag = null;
            columnHeader12.Text = "Size";
            columnHeader12.Width = 100;
            columnHeader13.ColumnAlign = System.Windows.Forms.HorizontalAlignment.Right;
            columnHeader13.CustomFilter = null;
            columnHeader13.CustomFormat = "0.00";
            columnHeader13.Format = Ai.Control.ColumnFormat.Custom;
            columnHeader13.MaximumValue = 100D;
            columnHeader13.MinimumValue = 0D;
            columnHeader13.Name = "Column3";
            columnHeader13.Tag = null;
            columnHeader13.Text = "%";
            columnHeader13.Width = 50;
            columnHeader14.CustomFilter = null;
            columnHeader14.Format = Ai.Control.ColumnFormat.Bar;
            columnHeader14.MaximumValue = 100D;
            columnHeader14.MinimumValue = 0D;
            columnHeader14.Name = "Column4";
            columnHeader14.Tag = null;
            columnHeader14.Text = "";
            columnHeader15.CustomFilter = null;
            columnHeader15.EnableSorting = true;
            columnHeader15.MaximumValue = 0D;
            columnHeader15.MinimumValue = 0D;
            columnHeader15.Name = "Column5";
            columnHeader15.Tag = null;
            columnHeader15.Text = "Date";
            columnHeader15.Width = 120;
            this.c_treeView.Columns.Add(columnHeader11);
            this.c_treeView.Columns.Add(columnHeader12);
            this.c_treeView.Columns.Add(columnHeader13);
            this.c_treeView.Columns.Add(columnHeader14);
            this.c_treeView.Columns.Add(columnHeader15);
            this.c_treeView.Culture = new System.Globalization.CultureInfo("en-US");
            this.c_treeView.Indent = -1;
            this.c_treeView.Location = new System.Drawing.Point(14, 55);
            this.c_treeView.Name = "c_treeView";
            this.c_treeView.Padding = new System.Windows.Forms.Padding(1);
            this.c_treeView.SelectedNode = null;
            this.c_treeView.Size = new System.Drawing.Size(577, 408);
            this.c_treeView.TabIndex = 10;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.c_menuTool});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(603, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.c_menuLoad});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(51, 20);
            this.toolStripMenuItem1.Text = "File(&F)";
            // 
            // c_menuLoad
            // 
            this.c_menuLoad.Name = "c_menuLoad";
            this.c_menuLoad.Size = new System.Drawing.Size(152, 22);
            this.c_menuLoad.Text = "Load(&L)";
            this.c_menuLoad.Click += new System.EventHandler(this.c_menuLoad_Click);
            // 
            // c_menuTool
            // 
            this.c_menuTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.c_menuHumanReadable});
            this.c_menuTool.Name = "c_menuTool";
            this.c_menuTool.Size = new System.Drawing.Size(55, 20);
            this.c_menuTool.Text = "Tool(&T)";
            // 
            // c_menuHumanReadable
            // 
            this.c_menuHumanReadable.Checked = true;
            this.c_menuHumanReadable.CheckOnClick = true;
            this.c_menuHumanReadable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.c_menuHumanReadable.Name = "c_menuHumanReadable";
            this.c_menuHumanReadable.Size = new System.Drawing.Size(193, 22);
            this.c_menuHumanReadable.Text = "Show human-readable";
            this.c_menuHumanReadable.Click += new System.EventHandler(this.c_menuHumanReadable_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 517);
            this.Controls.Add(this.c_treeView);
            this.Controls.Add(this.c_textRootFolder);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.c_btnRun);
            this.Controls.Add(this.c_btnBrowse);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "DiskUsageViewer";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button c_btnBrowse;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button c_btnRun;
        private System.Windows.Forms.TextBox c_textRootFolder;
        private Ai.Control.MultiColumnTree c_treeView;
        private System.Windows.Forms.Timer c_timerStatusUpdate;
        private System.Windows.Forms.ToolStripStatusLabel c_statusLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem c_menuLoad;
        private System.Windows.Forms.ToolStripMenuItem c_menuTool;
        private System.Windows.Forms.ToolStripMenuItem c_menuHumanReadable;
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiskUsageViewer
{
    public partial class MainForm : Form
    {
        class ScanData
        {
            public long Id;
            public long Size;
            public DateTime DateTime;
            public ScanData(long a_id, long a_size, DateTime a_datetime)
            {
                Id = a_id;
                Size = a_size;
                DateTime = a_datetime;
            }
        }
        private TreeModelAi m_model;
        private string m_currentFile;

        public MainForm()
        {
            InitializeComponent();
            c_statusLabel.Text = "";
            c_timerStatusUpdate.Interval = 100;
            c_timerStatusUpdate.Tick += (s, e) =>
            {
                c_statusLabel.Text = m_currentFile;
            };

            c_treeView.BeforeExpand += (s, e) =>
            {
                if (m_model != null)
                {
                    if (m_model.IsInitialized(e.Node))
                    {
                        return;
                    }

                    m_model.InitializeNode(e.Node.Nodes, e.Node);
                }
            };

            c_treeView.Columns[1].Format = Ai.Control.ColumnFormat.HumanReadable;
        }

        private void c_btnBrowse_Click(object sender, EventArgs e)
        {
            var dialog = new dnGREP.FileFolderDialog();
            dialog.SelectedPath = getExistDirectory(c_textRootFolder.Text);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dialog.SelectedPath))
                {
                    c_textRootFolder.Text = Path.GetDirectoryName(dialog.SelectedPath);
                }
                else
                {
                    c_textRootFolder.Text = dialog.SelectedPath;
                }
            }
        }

        private string getExistDirectory(string a_path)
        {
            // Remove invalid path chars
            foreach (char c in Path.GetInvalidPathChars())
            {
                a_path = a_path.Replace(c.ToString(), "");
            }

            // Check the upper folder until it finds an existing folder
            while (!string.IsNullOrEmpty(a_path) && !Directory.Exists(a_path))
            {
                a_path = Path.GetDirectoryName(a_path);
            }

            // If a folder is found return it
            if (Directory.Exists(a_path))
            {
                return a_path;
            }

            // Return the desktop if you can not find it
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        private async void c_btnRun_Click(object sender, EventArgs e)
        {
            string rootFolder = c_textRootFolder.Text;
            if (!Directory.Exists(rootFolder))
            {
                return;
            }

            // Change scan button until finish
            c_btnRun.Text = "Scanning...";
            c_btnRun.Enabled = false;

            // Start updating status bar 
            m_currentFile = "";
            c_timerStatusUpdate.Start();

            // Scan
            DateTime now = DateTime.Now;
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            var dbPath  = $"{exePath}{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.db";
            await Task.Run(() =>
            {
                if (rootFolder.Last() != '\\')
                {
                    rootFolder += "\\";
                }
                scanFolderMain(dbPath, rootFolder);
            });

            // Stop updating status bar 
            c_timerStatusUpdate.Stop();

            // Set result
            c_treeView.Nodes.Clear();
            m_model = new TreeModelAi(dbPath);
            m_model.InitializeNode(c_treeView.Nodes, null);

            // Revert scan button
            c_btnRun.Text = "Scan";
            c_btnRun.Enabled = true;

            c_statusLabel.Text = $"Finish {(DateTime.Now - now).TotalSeconds}sec";
        }
        private void scanFolderMain(string a_dbPath, string a_rootFolder)
        {
            // Create Table
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            var connStr = $"DATA Source={a_dbPath}";
            using (var connection = new SQLiteConnection(connStr))
            {
                using (var context = new ApplicationDbContext(connection))
                {
                    var dummyItem = new ItemPoco()
                    {
                        Id = 1,
                        ParentId = null,
                        Name = "Dummy",
                        Size = 0,
                        DateTime = new DateTime()
                    };
                    context.Items.Add(dummyItem);
                    context.Items.Remove(dummyItem);
                    context.SaveChanges();
                }
            }

            // Scan
            using (var connection = new SQLiteConnection(connStr))
            {
                connection.Open();

                using (var command     = new SQLiteCommand(connection))
                using (var transaction = connection.BeginTransaction())
                {
                    var res = scanFolderSub(command, transaction, 1, a_rootFolder, null);
                    transaction.Commit();
                }
            }
        }

        private ScanData scanFolderSub(SQLiteCommand a_command, SQLiteTransaction a_transaction, long a_id, string a_folder, long? a_folderId)
        {
            // 自分をDBに追加してIDを取得
            var finfo   = new FileInfo(a_folder);
            var dirname = finfo.Directory?.Name + "\\" ?? a_folder;
            var folderItem = new ItemPoco()
            {
                Id = a_id++,
                ParentId = a_folderId,
                Name = dirname,
                Size = 0,
                DateTime = new DateTime()
            };
            a_command.CommandText = folderItem.CreateInsertCommand();
            a_command.ExecuteNonQuery();

            // 子ファイルを追加
            long sizeSum = 0;
            DateTime lastDateTime = new DateTime();
            try
            {
                var files = Directory.GetFiles(a_folder);
                foreach (var f in files)
                {
                    var fileInfo = new FileInfo(Path.Combine(a_folder, f));
                    var fileItem = new ItemPoco()
                    {
                        Id = a_id++,
                        ParentId = folderItem.Id,
                        Name = fileInfo.Name,
                        Size = fileInfo.Length,
                        DateTime = fileInfo.LastWriteTime
                    };
                    a_command.CommandText = fileItem.CreateInsertCommand();
                    a_command.ExecuteNonQuery();

                    sizeSum += fileInfo.Length;
                    if (lastDateTime < fileInfo.LastWriteTime)
                    {
                        lastDateTime = fileInfo.LastWriteTime;
                    }

                    // Set check folder for status bar.
                    m_currentFile = fileInfo.FullName;
                }
            }
            catch (Exception)
            {
            }

            // 子ディレクトリを追加
            try
            {
                var dirs = Directory.GetDirectories(a_folder);
                foreach (var d in dirs)
                {
                    var r = scanFolderSub(a_command, a_transaction, a_id, d + "\\", folderItem.Id);
                    a_id = r.Id;
                    sizeSum += r.Size;
                    if (lastDateTime < r.DateTime)
                    {
                        lastDateTime = r.DateTime;
                    }
                }
            }
            catch (Exception)
            {
            }

            // サイズを設定
            folderItem.Size = sizeSum;
            folderItem.DateTime = lastDateTime;
            a_command.CommandText = folderItem.CreateUpdateCommand();
            a_command.ExecuteNonQuery();

            return new ScanData(a_id, sizeSum, lastDateTime);
        }

        private void c_menuLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            ofd.FileName = "*.db";
            ofd.Filter = "DB File(*.db)|*.db";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                c_treeView.Nodes.Clear();
                m_model = new TreeModelAi(ofd.FileName);
                m_model.InitializeNode(c_treeView.Nodes, null);
            }
        }

        private void c_menuHumanReadable_Click(object sender, EventArgs e)
        {
            if (c_menuHumanReadable.Checked)
            {
                c_treeView.Columns[1].Format = Ai.Control.ColumnFormat.HumanReadable;
            }
            else
            {
                c_treeView.Columns[1].Format = Ai.Control.ColumnFormat.None;
            }
        }
    }
}

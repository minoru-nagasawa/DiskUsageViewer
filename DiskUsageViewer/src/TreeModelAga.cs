using System;
using System.Collections.Generic;
using System.Text;
using Aga.Controls.Tree;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Threading;
using System.Collections;
using System.Data.SQLite;
using System.Linq;

namespace DiskUsageViewer
{
	public class TreeModelAga: ITreeModel
    {
        private string m_dbPath;

        public TreeModelAga(string a_dbPath)
		{
            m_dbPath = a_dbPath;
        }

        public IEnumerable GetChildren(TreePath a_treePath)
        {
            IEnumerable<ItemPoco> retVal = new List<ItemPoco>();
            if (a_treePath.IsEmpty())
            {
                if (File.Exists(m_dbPath))
                {
                    using (var connection = new SQLiteConnection($"DATA Source={m_dbPath}"))
                    {
                        using (var context = new ApplicationDbContext(connection))
                        {
                            retVal = context.Items.Where(x => x.ParentId == null).ToArray();
                        }
                    }
                }
            }
            else
            {
                if (File.Exists(m_dbPath))
                {
                    using (var connection = new SQLiteConnection($"DATA Source={m_dbPath}"))
                    {
                        using (var context = new ApplicationDbContext(connection))
                        {
                            var item = a_treePath.LastNode as ItemPoco;
                            if (item != null)
                            {
                                retVal = context.Items.Where(x => x.ParentId == item.Id).ToArray();
                            }
                        }
                    }
                }
            }

            return retVal;
        }

        public bool IsLeaf(TreePath a_treePath)
        {
            var item = a_treePath.LastNode as ItemPoco;
            if (item == null)
            {
                return true;
            }

            return item.Name.Last() != '\\';
        }

        public event EventHandler<TreeModelEventArgs> NodesChanged;
		public event EventHandler<TreeModelEventArgs> NodesInserted;
		public event EventHandler<TreeModelEventArgs> NodesRemoved;
		public event EventHandler<TreePathEventArgs> StructureChanged;
    }
}

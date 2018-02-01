using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Threading;
using System.Collections;
using System.Data.SQLite;
using System.Linq;
using Ai.Control;

namespace DiskUsageViewer
{
	public class TreeModelAi
    {
        private const int IndexItem = 5;
        private const int IndexInitializedFlag = 6;
        private string m_dbPath;
        private long m_totalSize;

        public TreeModelAi(string a_dbPath)
		{
            m_dbPath = a_dbPath;
            m_totalSize = 0;
            foreach (var v in getChildren(null))
            {
                m_totalSize += v.Size;
            }
        }

        public bool IsInitialized(TreeNode a_node)
        {
            return (bool)a_node.SubItems[IndexInitializedFlag].Value == true;
        }

        public void InitializeNode(TreeNodeCollection a_target, TreeNode a_node)
        {
            // Remove dummy data
            while (a_target.Count != 0)
            {
                // e.Node.Nodes.Clear() does not work properly 
                a_target.RemoveAt(0);
            }

            // Set initialized flag
            if (a_node != null)
            {
                a_node.SubItems[IndexInitializedFlag].Value = true;
            }

            // Add children
            foreach (var v in getChildren(a_node))
            {
                var node = new Ai.Control.TreeNode(v.Name);
                node.SubItems.Add(v.Size);     // [1] is Size
                double percent = 100.0 * v.Size / m_totalSize;
                node.SubItems.Add(percent);    // [2] is %
                node.SubItems.Add(percent).Color = Color.Green;    // [3] is bar for Size
                node.SubItems.Add(v.DateTime); // [4] is DateTime
                node.SubItems.Add(v);          // [5] is ItemPoco
                node.SubItems.Add(false);      // [6] is initialized flag.
                if (isLeaf(node))
                {
                    // http://www.small-icons.com/packs/16x16-free-toolbar-icons.htm
                    node.Image = Properties.Resources.IconFile;
                }
                else
                {
                    node.Image = Properties.Resources.IconDirectory;
                    node.Nodes.Add(""); // It is dummy data for Tree Expand Mark. (>)
                }
                
                a_target.Add(node);
            }
        }

        private IEnumerable<ItemPoco> getChildren(TreeNode a_node)
        {
            IEnumerable<ItemPoco> retVal = new List<ItemPoco>();
            if (a_node == null)
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
                            if (a_node.SubItems.Count != 0)
                            {
                                var item = a_node.SubItems[IndexItem].Value as ItemPoco;
                                if (item != null)
                                {
                                    retVal = context.Items.Where(x => x.ParentId == item.Id).ToArray();
                                }
                            }
                        }
                    }
                }
            }

            return retVal;
        }

        private bool isLeaf(TreeNode a_node)
        {
            if (a_node.SubItems.Count != 0)
            {
                var item = a_node.SubItems[IndexItem].Value as ItemPoco;
                if (item != null)
                {
                    return item.Name.Last() != '\\';
                }
            }
            return true;
        }
    }
}

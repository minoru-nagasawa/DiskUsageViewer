using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskUsageViewer
{
    public class AutoCommitSQLiteTransaction: IDisposable
    {
        private bool m_disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool a_disposing)
        {
            if (m_disposed)
            {
                return;
            }

            if (a_disposing)
            {
                m_transaction.Commit();
                m_transaction.Dispose();
            }
            m_disposed = true;
        }

        private SQLiteConnection  m_connection;
        private SQLiteTransaction m_transaction;
        private long m_period;
        public AutoCommitSQLiteTransaction(SQLiteConnection a_connection, long a_period)
        {
            m_connection  = a_connection;
            m_transaction = m_connection.BeginTransaction();
            m_period = a_period;
        }

        public void CommitIfNeeds(long a_count)
        {
            if ((a_count % m_period) == 0)
            {
                m_transaction.Commit();
                m_transaction.Dispose();
                m_transaction = m_connection.BeginTransaction();
            }
        }
    }
}

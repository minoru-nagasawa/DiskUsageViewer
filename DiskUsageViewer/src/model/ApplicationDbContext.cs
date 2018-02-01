using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskUsageViewer
{
    class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbConnection a_connection) : base(a_connection, true)
        {
        }

        public DbSet<ItemPoco> Items { get; set; }

        protected override void OnModelCreating(DbModelBuilder a_builder)
        {
            //var model = a_builder.Build(Database.Connection);
            //IDatabaseCreator sqliteDatabaseCreator = new SqliteDatabaseCreator();
            //sqliteDatabaseCreator.Create(Database, model);

            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<ApplicationDbContext>(a_builder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}

using TaskManager.Settings;
using ThinkingHome.Migrator;

namespace TaskManager.Mirgations
{
    public class DbMigrator
    {
        private PostgresSettings settings;

        public DbMigrator(PostgresSettings settings)
        {
            this.settings = settings;
        }

        public void StartMigration()
        {
            var provider = "Postgres";
            var assembly = typeof(DbMigration).Assembly;
            using (var migrator = new Migrator(provider, settings.ConnectionString, assembly))
            {
                migrator.Migrate();
            }
        }
    }
}

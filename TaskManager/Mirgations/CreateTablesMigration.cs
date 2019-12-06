using System.Data;
using ThinkingHome.Migrator.Framework;

namespace TaskManager.Mirgations
{
    [Migration(1)]
    public class CreateTablesMigration : Migration
    {
        public override void Apply()
        {
            Database.AddTable("tasks",
                new Column("id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
                new Column("parent_id", DbType.Int32),
                new Column("start_date", DbType.DateTime),
                new Column("level", DbType.Int32),
                new Column("name", DbType.String),
                new Column("description", DbType.String),
                new Column("performers", DbType.String),
                new Column("registration_date", DbType.DateTime),
                new Column("status", DbType.Int32),
                new Column("labour_input", DbType.Int64),
                new Column("lead_time", DbType.Int64),
                new Column("end_date", DbType.DateTime));

            Database.AddTable("users",
                new Column("id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
                new Column("email", DbType.String, ColumnProperty.PrimaryKey),
                new Column("password", DbType.String),
                new Column("username", DbType.String));

            Database.AddTable("tasks_performers",
                new Column("id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
                new Column("user_id", DbType.Int32),
                new Column("task_id", DbType.Int32));
        }

        public override void Revert()
        {
            Database.RemoveTable("tasks_performers");
            Database.RemoveTable("tasks");
            Database.RemoveTable("users");
        }
    }
}

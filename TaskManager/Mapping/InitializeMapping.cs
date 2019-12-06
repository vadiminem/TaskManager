namespace TaskManager.Mapping
{
    public class InitializeMapping
    {
        public void Initialize()
        {
            DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.PostgreSqlDialect();
            DapperExtensions.DapperExtensions.SetMappingAssemblies(new[]
            {
                typeof(TasksPerformersMap).Assembly,
                typeof(TasksMap).Assembly,
                typeof(UsersMap).Assembly
            });
        }
    }
}

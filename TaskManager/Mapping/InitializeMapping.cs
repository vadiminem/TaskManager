using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Mapping
{
    public class InitializeMapping
    {

        public void Initialize()
        {
            DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.PostgreSqlDialect();
            DapperExtensions.DapperExtensions.SetMappingAssemblies(new[]
            {
                typeof(TasksPerformersMap).Assembly
            });
        }
    }
}

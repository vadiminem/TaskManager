using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class TasksContext : DbContext
    {
        public DbSet<DbTask> Tasks { get; set; }
        public TasksContext(DbContextOptions<TasksContext> options)
            :base(options)
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
    }
}

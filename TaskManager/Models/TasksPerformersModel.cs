using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class TasksPerformersModel
    {
        public int Id { get; set; }
        public DbTask Task { get; set; }
        public User User { get; set; }
    }
}

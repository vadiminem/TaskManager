using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Mapping
{
    public class TasksPerformersMap : ClassMapper<TasksPerformersModel>
    {
        public TasksPerformersMap()
        {
            Table("tasks_performers");
            Map(x => x.Id).Column("id");
            Map(x => x.Task.Id).Column("task_id");
            Map(x => x.User.Id).Column("user_id");
        }
    }
}

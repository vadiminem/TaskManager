using DapperExtensions.Mapper;
using TaskManager.Models;

namespace TaskManager.Mapping
{
    public class TasksPerformersMap : ClassMapper<TasksPerformersModel>
    {
        public TasksPerformersMap()
        {
            Table("tasks_performers");
            Map(x => x.Id).Column("id").Key(KeyType.Identity);
            Map(x => x.TaskId).Column("task_id");
            Map(x => x.UserId).Column("user_id");
        }
    }
}

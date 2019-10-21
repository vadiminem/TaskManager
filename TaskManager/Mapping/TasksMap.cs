using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Mapping
{
    public class TasksMap : ClassMapper<DbTask>
    {
        public TasksMap()
        {
            Table("tasks");
            Map(x => x.Id).Column("id").Key(KeyType.Identity);
            Map(x => x.ParentId).Column("parent_id");
            Map(x => x.StartDate).Column("start_date");
            Map(x => x.Level).Column("level");
            Map(x => x.Name).Column("name");
            Map(x => x.Description).Column("description");
            Map(x => x.Performers).Column("performers");
            Map(x => x.RegistrationDate).Column("registration_date");
            Map(x => x.Status).Column("status");
            Map(x => x.LabourInput).Column("labour_input");
            Map(x => x.LeadTime).Column("lead_time");
            Map(x => x.EndDate).Column("end_date");
        }
    }
}

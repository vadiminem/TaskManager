using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class Task
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Performers { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Status Status { get; set; }
        public long LabourInput { get; set; }
        public long LeadTime { get; set; }
        public DateTime EndDate { get; set; }
    }
}

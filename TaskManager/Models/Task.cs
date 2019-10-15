using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class Task
    {
        //[Required(ErrorMessage = "Введите название задачи")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Введите описание задачи")]
        public string Description { get; set; }
        //[Required(ErrorMessage = "Введите исполнители задачи")]
        public string Performers { get; set; }
        //[Required(ErrorMessage = "Введите время начала задачи")]
        public DateTime RegistrationDate { get; set; }
        public Status Status { get; set; }
        public long LabourInput { get; set; }
        public long LeadTime { get; set; }
        //[Required(ErrorMessage = "Введите время завершения задачи")]
        public DateTime EndDate { get; set; }
    }
}

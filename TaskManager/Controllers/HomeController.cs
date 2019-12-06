using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IStorageRepository repository;
        public HomeController(IStorageRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTask(int id)
        {
            var task = repository.FindTask(id);
            if (task != null)
            {
                return View(task);
            }
            else
                return RedirectToAction("Index"); //Здесь надо вывести ошибку
        }

        [HttpGet]
        public IActionResult GetTasks()
        {
            var user = repository.FindUserByUsername(User.Identity.Name);
            var result = repository.GetTasksForUser(user);
            if (result != null)
                return Json(result.Select(t => new { t.Id, t.Name, t.ParentId, t.Level }));

            return null;
        }

        [HttpGet]
        public IActionResult Create(int? id)
        {
            ViewData["ParentId"] = id ?? -1;
            ViewData["User"] = User.Identity.Name;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DbTask task)
        {
            if (ModelState.IsValid)
            {
                int parentLevel = 0;
                if (task.ParentId != -1)
                {
                    var parent = repository.FindTask(task.ParentId);
                    parentLevel = parent.Level;
                }
                task.LabourInput = (task.EndDate - task.RegistrationDate).Ticks;
                task.Level = parentLevel + 1;
                var generatedId = repository.InsertTask(task);
                var taskPerformers = task.Performers.Split(',');
                try
                {
                    foreach (var p in taskPerformers)
                    {
                        repository.InsertTaskPerformers(new TasksPerformersModel { TaskId = generatedId, UserId = repository.FindUserByUsername(p.Trim()).Id });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }
                return RedirectToAction("GetTask", "Home", new { id = generatedId, layout = true });
            }
            else
                return View(task);
        }

        public async Task<IActionResult> ChangeStatus(int id, Status status)
        {
            var task = repository.FindTask(id);
            if (task.Status != Status.Completed)
            {
                if (status == Status.Completed)
                {
                    if (task.Status == Status.InProgress)
                    {
                        var tasks = repository.GetTasks().Where(t => t.ParentId == task.Id);
                        if (tasks.Where(t => t.Status == Status.Completed).Count() == tasks.Count())
                        {
                            task.Status = Status.Completed;
                            task.LeadTime += (DateTime.Now - task.StartDate).Ticks;
                        }
                        else
                        {
                            // Отправить сообщение об ошибке.
                        }
                    }
                }
                else if (task.Status == Status.Assigned || task.Status == Status.Paused)
                {
                    task.Status = Status.InProgress;
                    task.StartDate = DateTime.Now;
                }
                else if (task.Status == Status.InProgress)
                {
                    task.Status = Status.Paused;
                    task.LeadTime += (DateTime.Now - task.StartDate).Ticks; // проверить
                }
            }
            repository.UpdateTask(task);
            return RedirectToAction("GetTask", new { id = task.Id, layout = true});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = repository.FindTask(id);

            return View("Create", task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DbTask task)
        {
            var oldTask = repository.FindTask(task.Id);
            oldTask.Name = task.Name;
            oldTask.Description = task.Description;
            oldTask.Performers = task.Performers;
            oldTask.RegistrationDate = task.RegistrationDate;
            oldTask.EndDate = task.EndDate;
            return RedirectToAction("GetTask", "Home", new { id = task.Id, layout = true });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var task = repository.FindTask(id);
            var inhTasks = repository.GetTasks().Where(t => t.ParentId == task.Id);
            foreach (var t in inhTasks)
            {
                t.ParentId = task.ParentId;
            }
            repository.RemoveTask(task.Id);
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

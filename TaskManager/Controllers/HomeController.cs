using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private TasksContext db;
        public HomeController(TasksContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTask(int id, bool layout = false)
        {
            if (layout)
                ViewData["Layout"] = "true";
            else
                ViewData["Layout"] = "false";
            var task = await db.Tasks.FindAsync(id);
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
            return Json(db.Tasks.OrderBy(t => t.ParentId)
                .ThenByDescending(t => t.RegistrationDate)
                .Select(t => new { t.Id, t.Name, t.ParentId }));
        }

        [HttpGet]
        public IActionResult Create(int? id)
        {
            ViewData["ParentId"] = id ?? -1;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DbTask task)
        {
            task.LabourInput = (task.EndDate - task.RegistrationDate).Ticks;
            db.Tasks.Add(task);
            await db.SaveChangesAsync();
            return RedirectToAction("GetTask", "Home", new { id = db.Tasks.Last().Id, layout = true });
        }

        public async Task<IActionResult> ChangeStatus(int id, Status status)
        {
            var task = await db.Tasks.FindAsync(id);
            if (task.Status != Status.Completed)
            {
                if (status == Status.Completed)
                {
                    if (task.Status == Status.InProgress)
                    {
                        task.Status = Status.Completed;
                        task.LeadTime += (DateTime.Now - task.StartDate).Ticks; // проверить
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
                await db.SaveChangesAsync();
            }
            return RedirectToAction("GetTask", "Home", new { id = task.Id, layout = false });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await db.Tasks.FindAsync(id);

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DbTask task)
        {
            var oldTask = await db.Tasks.FindAsync(task.Id);
            oldTask.Name = task.Name;
            oldTask.Description = task.Description;
            oldTask.Performers = task.Performers;
            oldTask.RegistrationDate = task.RegistrationDate;
            oldTask.EndDate = task.EndDate;
            await db.SaveChangesAsync();
            return RedirectToAction("GetTask", "Home", new { id = task.Id, layout = true });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var task = await db.Tasks.FindAsync(id);
            var inhTasks = db.Tasks.Where(t => t.ParentId == task.Id);
            foreach (var t in inhTasks)
            {
                t.ParentId = task.ParentId;
            }
            db.Tasks.Remove(task);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

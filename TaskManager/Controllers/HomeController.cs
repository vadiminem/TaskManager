using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;

namespace TaskManager.Controllers
{
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

        public async Task<IActionResult> GetTask(int id)
        {
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
            return Json(db.Tasks.OrderBy(t => t.ParentId).ThenByDescending(t => t.RegistrationDate).Select(t => new { t.Id, t.Name, t.ParentId }));
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
            task.LabourInput = task.EndDate - task.RegistrationDate;
            db.Tasks.Add(task);
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

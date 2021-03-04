using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Enums;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TasksController : Controller
    {
        private ToDoContext _db;

        public TasksController(ToDoContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Task> tasks = _db.Tasks.ToList();
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Task task)
        {
            if (task != null)
            {
                _db.Tasks.Add(task);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id != null)
            {
                Task task = _db.Tasks.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    return View(task);
                }
                return NotFound();
            }
            return NotFound();
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            Task task = _db.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null && task.Status != Status.Открытая)
            {
                _db.Entry(task).State = EntityState.Deleted;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Task task = _db.Tasks.FirstOrDefault(t => t.Id == id);
            return View(task);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Enums;
using ToDoList.Models;
using ToDoList.ViewModels;

namespace ToDoList.Controllers
{
    public class TasksController : Controller
    {
        private ToDoContext _db;

        public TasksController(ToDoContext db)
        {
            _db = db;
        }

        public IActionResult Index(string header, string createFrom, string createTo, 
            string priority, string status, string descriptionPart, 
            SortState sortOrder = SortState.HeaderAsc)
        {
            
            List<Task> tasks = _db.Tasks.ToList();
            IQueryable<Task> tasksQ = tasks.AsQueryable(); 

            ViewBag.HeaderSort = sortOrder == SortState.HeaderAsc ? SortState.HeaderDesc : SortState.HeaderAsc;
            ViewBag.PrioritySort = sortOrder == SortState.PriorityAsc ? SortState.PriorityDesc : SortState.PriorityAsc;
            ViewBag.StatusSort = sortOrder == SortState.StatusAsc ? SortState.StatusDesc : SortState.StatusAsc;
            ViewBag.CreateSort = sortOrder == SortState.CreateAsc ? SortState.CreateDesc : SortState.CreateAsc;

            switch (sortOrder)
            {
                case SortState.HeaderDesc: tasksQ = tasksQ.OrderByDescending(t => t.Header); break;
                case SortState.PriorityAsc: tasksQ = tasksQ.OrderBy(t => t.Priority); break;
                case SortState.PriorityDesc: tasksQ = tasksQ.OrderByDescending(t => t.Priority); break;
                case SortState.StatusAsc: tasksQ = tasksQ.OrderBy(t => t.Status); break;
                case SortState.StatusDesc: tasksQ = tasksQ.OrderByDescending(t => t.Status); break;
                case SortState.CreateAsc: tasksQ = tasksQ.OrderBy(t => t.CreationAt); break;
                case SortState.CreateDesc: tasksQ = tasksQ.OrderByDescending(t => t.CreationAt); break;
                default: tasksQ = tasksQ.OrderBy(t => t.Header); break;
            }

            tasks = tasksQ.ToList();
            
            FilterViewModel model = new FilterViewModel
            {
                Tasks = tasks.ToList(),
                Header = header,
                CreationFrom = Convert.ToDateTime(createFrom),
                CreationTo = Convert.ToDateTime(createTo),
                Priority = priority == "1" ? Priority.Высокий 
                    : (priority == "2" ? Priority.Средний 
                        : (priority == "3" ? Priority.Низкий : Priority.Все)),
                Status = status == "1" ? Status.Новая
                    : (status == "2" ? Status.Открытая
                        : (status == "3" ? Status.Закрытая : Status.Все))
            };
            if(descriptionPart != null)
            {
                model.Tasks = model.Tasks.FindAll(task => task.Description.Contains(descriptionPart));
            }
            model.Tasks = Filtration(model);
            return View(model);
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            List<string> priorities = Enum.GetNames(typeof(Priority)).ToList();
            ViewBag.Priorities = priorities;
            return View();
        }

        
        [HttpPost]
        public IActionResult Create(Task task)
        {
            if (task != null && ModelState.IsValid)
            {
                task.Status = Status.Новая;
                _db.Tasks.Add(task);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id != 0)
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
        
        public IActionResult ChangeStatus(int id, string status)
        {
            if (id != 0)
            {
                Task task = _db.Tasks.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    switch (status)
                    {
                        case"Open":
                            if (task.Status == Status.Закрытая || task.Status == Status.Открытая)
                            {
                                return RedirectToAction("Index");
                            }

                            task.Status = Status.Открытая;
                            break;
                        case"Close":
                            if (task.Status == Status.Закрытая || task.Status == Status.Новая)
                            {
                                return RedirectToAction("Index");
                            }

                            task.Status = Status.Закрытая;
                            task.ExpirationAt = DateTime.Now;
                            break;
                    }

                    _db.Tasks.Update((task));
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        
        [NonAction]
        public List<Task> Filtration(FilterViewModel model)
        {
            if (model.Header != null)
            {
                model.Tasks = model.Tasks.Where(t => t.Header == model.Header).ToList();
            }

            if (model.CreationFrom.Date != DateTime.Parse("01.01.0001"))
            {
                model.Tasks = model.Tasks.Where(t => t.CreationAt.Date >= Convert.ToDateTime(model.CreationFrom.Date))
                    .ToList();
            }

            if (model.CreationTo.Date != DateTime.Parse("01.01.0001"))
            {
                model.Tasks = model.Tasks.Where(t => t.CreationAt <= Convert.ToDateTime(model.CreationTo.Date))
                    .ToList();
            }

            if(model.Priority != Priority.Все)
            {
                model.Tasks = model.Tasks.Where(t => t.Priority == model.Priority).ToList();
            }

            if(model.Status != Status.Все)  
            {
                model.Tasks = model.Tasks.Where(t => t.Status == model.Status).ToList();
            }

            if (model.Tasks.Count <= 0)
            {
                return new List<Task>();
            }
            return model.Tasks;
        }
    }
}
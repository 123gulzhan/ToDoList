using Microsoft.AspNetCore.Mvc;
using ToDoList.Enums;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ValidationController : Controller
    {
        private ToDoContext _db;

        public ValidationController(ToDoContext db)
        {
            _db = db;
        }

        public bool CheckPriority(string priority)
        {
            return (priority == Priority.Высокий.ToString()
                    || priority == Priority.Средний.ToString()
                    || priority == Priority.Низкий.ToString());

        }
    }
}
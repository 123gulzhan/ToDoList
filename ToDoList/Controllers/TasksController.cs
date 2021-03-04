using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers
{
    public class TasksController : Controller
    {
        
        
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}
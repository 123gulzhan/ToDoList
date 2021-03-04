using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models
{
    public class ToDoContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
            
        }
    }
}
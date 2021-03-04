using System;
using ToDoList.Enums;

namespace ToDoList.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string ExecutorName { get; set; }
        public string Description { get; set; }
        public DateTime CreationAt { get; set; } = DateTime.Now;
        public DateTime? ExpirationAr { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
    }
}
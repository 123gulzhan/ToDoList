using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
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
        public DateTime? ExpirationAt { get; set; }
        public Status Status { get; set; }
        [Required]
        [Remote("CheckPriority", "Validation", ErrorMessage = "Приоритет не выбран")]
        public Priority Priority { get; set; }
    }
}
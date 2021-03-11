using System;
using System.Collections.Generic;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ToDoList.Enums;
using ToDoList.Models;

namespace ToDoList.ViewModels
{
    public class FilterViewModel
    {
        public List<Task> Tasks { get; set; }
        public SortOrder SortOrder { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime CreationFrom { get; set; }
        public DateTime CreationTo { get; set; }
        public string Header { get; set; }
    }
}
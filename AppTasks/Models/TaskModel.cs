using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppTasks.Models
{
    [Table("Task")]
    public class TaskModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }
        
        public bool Done { get; set; }

        public string ImageUrl { get; set; }

        public string ImageBase64 { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SummerQuestion4.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public virtual ICollection<ProjectTask> Tasks { get; set; }
    }
}
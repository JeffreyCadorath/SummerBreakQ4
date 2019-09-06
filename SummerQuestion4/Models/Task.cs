using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SummerQuestion4.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ApplicationUser> Developer { get; set; }
        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }
        public int PercentageCompleted { get; set; }
        public Priority Priority { get; set; }
        public DateTime Duedate { get; set; }
        public bool Completed { get; set; }


    }

    public enum Priority
    {
        High,
        Low
    }
}
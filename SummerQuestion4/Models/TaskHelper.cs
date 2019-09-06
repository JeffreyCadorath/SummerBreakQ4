using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SummerQuestion4.Models
{
    public class TaskHelper
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> usersManager;
        private RoleManager<IdentityRole> rolesManager;

        public TaskHelper()
        {
            usersManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            rolesManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        }

        public bool AssignDev(int taskId, string userId)
        {
            var task = db.Tasks.Find(taskId);
            var user = usersManager.FindById(userId);

            if (task == null || user == null)
            {
                return false;
            }
            else
            {
                user.Tasks.Add(task);
                db.SaveChanges();
                return true;
            }
        }

        public bool RemoveDev(int taskId, string userId)
        {
            var task = db.Tasks.Find(taskId);
            var user = usersManager.FindById(userId);

            if(task == null || user == null)
            {
                return false;
            }
            else
            {
                user.Tasks.Remove(task);
                db.SaveChanges();
                return true;
            }
        }

        public bool updateCompletionStatus(int taskId, int CompletionPercentage, bool completed)
        {
            var task = db.Tasks.Find(taskId);
            if(task == null || CompletionPercentage == 0)
            {
                task.Completed = completed;
                return false;
            }
            else
            {
                task.PercentageCompleted = CompletionPercentage;
                if(task.PercentageCompleted >= 100)
                {
                    task.PercentageCompleted = 100;
                    task.Completed = true;
                }
                if (task.Completed == true)
                {
                    task.PercentageCompleted = 100;
                }
                db.SaveChanges();
                return true;
            }
        }

        public ICollection<ProjectTask> orderByPriority()
        {
            var order = db.Tasks.OrderByDescending(x => x.Priority).ToList();
            return order;
        }

        public ICollection<Project> orderByProjectPriority()
        {
            var order = db.Projects.OrderByDescending(x => x.Priority).ToList();
            return order;
        }
    }
}
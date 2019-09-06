using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SummerQuestion4.Models
{
    public class UserHelper
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> usersManager;
        private RoleManager<IdentityRole> rolesManager;

        public UserHelper()
        {
            usersManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            rolesManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        }

        public ICollection<ApplicationUser> GetAllDevelopers()
        {
            var DevId = rolesManager.FindByName("Developer").Id;
            var Developers = db.Users.Where(x => x.Roles.Any(d => d.RoleId == DevId)).ToList();
            return Developers;
        }

        public bool AddRole(string roleName)
        {
            if (rolesManager.RoleExists(roleName))
            {
                return false;
            }
            else
            {
                var newRole = new IdentityRole(roleName);
                rolesManager.Create(newRole);
                db.SaveChanges();
                return true;
            }
        }

        public bool AddUserToRole(string roleName, string userId)
        {
            var user = usersManager.FindById(userId);

            if(rolesManager.RoleExists(roleName) 
                && user != null 
                && !usersManager.IsInRole(userId, roleName))
            {
                usersManager.AddToRole(userId, roleName);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveUserRole(string roleName, string userId)
        {
            var user = usersManager.FindById(userId);
            if(user == null)
            {
                return false;
            }
            else
            {
                usersManager.RemoveFromRole(userId, roleName);
                db.SaveChanges();
                return true;
            }
        }

        public bool AddToTask(int taskId, string userId)
        {
            var user = usersManager.FindById(userId);
            var task = db.Tasks.Find(taskId);

            if(user == null || task == null)
            {
                return false;
            }
            else
            {
                task.Developer.Add(user);
                user.Tasks.Add(task);
                db.SaveChanges();
                return true;
            }
        }

        public bool RemoveFromTask(int taskId, string userId)
        {
            var user = usersManager.FindById(userId);
            var task = db.Tasks.Find(taskId);

            if (user == null || task == null)
            {
                return false;
            }
            else
            {
                task.Developer.Remove(user);
                user.Tasks.Remove(task);
                db.SaveChanges();
                return true;
            }
        }
    }
}
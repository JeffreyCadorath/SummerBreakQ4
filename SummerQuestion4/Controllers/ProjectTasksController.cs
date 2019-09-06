using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SummerQuestion4.Models;

namespace SummerQuestion4.Controllers
{
    [Authorize(Roles ="Project Manager")]
    public class ProjectTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserHelper userHelper;
        TaskHelper taskHelper;
        public ProjectTasksController()
        {
            userHelper = new UserHelper();
            taskHelper = new TaskHelper();
        }

        [Authorize(Roles = "Project Manager")]
        public ActionResult AssignTask()
        {
            var tasks = db.Tasks.ToList();
            ViewBag.taskId = new SelectList(tasks, "Id", "Name");

            var developers = userHelper.GetAllDevelopers();
            ViewBag.developerId = new SelectList(developers, "Id", "Email");

            return View();
        }

        [HttpPost]
        public ActionResult AssignTask(int taskId, string userId)
        {
            var task = db.Tasks.Find(taskId);
            var developer = db.Users.Find(userId);

            if (task != null && developer != null)
            {
                taskHelper.AssignDev(taskId, userId);
                return RedirectToAction("ProjectDisplay", "Projects");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [Authorize(Roles ="Developer")]
        public ActionResult ShowMyTasks()
        {
            if (User.IsInRole("Developer"))
            {
                var AppUser = db.Users.Find(User.Identity.GetUserId());
                var myTasks = AppUser.Tasks.OrderBy(x => x.Priority).ToList();
                return View(myTasks);
            }
            else
            {
                ViewBag.Message = "You don't have permission to view this page";
                return View();
            }
        }

        public ActionResult UpdateCompletion(int taskId)
        {
            if (User.IsInRole("Developer"))
            {
                var task = db.Tasks.Find(taskId);
                return View(task);
            }
            else
            {
                ViewBag.message = "You don't have permission to view this page";
                return View();
            }

        }

        [HttpPost]
        public ActionResult UpdateCompletion(int taskId, bool? completed, int percentageCompleted)
        {
            bool isComplete = false;
            if (completed == null)
            {
                isComplete = false;
            }
            else if (percentageCompleted > 100)
            {
                isComplete = true;
            }
            taskHelper.updateCompletionStatus(taskId, percentageCompleted, isComplete);
            return RedirectToAction("MyTasks");
        }

        // GET: ProjectTasks
        public ActionResult Index()
        {
            var tasks = db.Tasks.Include(p => p.Project);
            return View(tasks.ToList());
        }

        // GET: ProjectTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.Tasks.Find(id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // GET: ProjectTasks/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            return View();
        }

        // POST: ProjectTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ProjectId,PercentageCompleted,Priority,Duedate,Completed")] ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(projectTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectTask.ProjectId);
            return View(projectTask);
        }

        // GET: ProjectTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.Tasks.Find(id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectTask.ProjectId);
            return View(projectTask);
        }

        // POST: ProjectTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ProjectId,PercentageCompleted,Priority,Duedate,Completed")] ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectTask.ProjectId);
            return View(projectTask);
        }

        // GET: ProjectTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.Tasks.Find(id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // POST: ProjectTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectTask projectTask = db.Tasks.Find(id);
            db.Tasks.Remove(projectTask);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

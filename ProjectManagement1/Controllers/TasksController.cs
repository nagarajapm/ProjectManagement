using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectManagement.DAL;

namespace ProjectManagement.Controllers
{
    [RoutePrefix("api/tasks")]

    public class TasksController : ApiController
    {
        private ProjectManagementContext db = new ProjectManagementContext();

        // GET: api/Tasks
        public IQueryable<Task> GetTasks()
        {
            return db.Tasks;
        }
        [Route("GetParentTasks")]
        public IQueryable<ParentTask> GetParentTasks()
        {
            return db.ParentTasks;
        }

        // GET: api/Tasks/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult GetTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/Tasks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTask(int id, Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Task_ID)
            {
                return BadRequest();
            }

            db.Entry(task).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tasks
        [ResponseType(typeof(Task))]
        public IHttpActionResult PostTask(Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (task.Priority > 0)
            {
                db.Tasks.Add(task);
                db.SaveChanges();

            }
            else
            {
               
                db.ParentTasks.Add(new ParentTask() { Parent_Task = task.TaskName });
                db.SaveChanges();
               // ParentTask ParentTask = db.ParentTasks.Find(task.TaskName);
                task.Parent_ID = db.ParentTasks.Local[0].Parent_ID;
                db.Tasks.Add(task);
                db.SaveChanges();
            }


            if (task.User_ID != null)
            {
                int userid = (int)task.User_ID;
                User user = db.Users.Find(userid);
                user.Task_ID = task.Task_ID;
                user.Project_ID = task.Project_ID;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
           
             
            return CreatedAtRoute("DefaultApi", new { id = task.Task_ID }, task);
        }

        // DELETE: api/Tasks/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult DeleteTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            //db.Tasks.Remove(task);
            task.Status = "Completed";
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.Task_ID == id) > 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace productive.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTasks()
        {
            using (TasksEntities dc = new TasksEntities())
            {
                var tasks = dc.Tasks.ToList();
                return new JsonResult { Data = tasks, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        [HttpPost]
        public JsonResult SaveTask(Task task)
        {
            var success = false;
            using (TasksEntities dc = new TasksEntities())
            {
                if (task.Category == null)
                {
                    task.Category = " ";
                }
                if (task.EventID > 0)
                {
                    //update all values in a task multipurpose for all save functions specify active or complete on js side 
                    var newTask = dc.Tasks.Where(a => a.EventID == task.EventID).FirstOrDefault();
                    if (newTask != null)
                    {
                        newTask.Name = task.Name;
                        newTask.startDate = task.startDate;
                        newTask.endDate = task.endDate;
                        newTask.Description = task.Description;
                        newTask.Category = task.Category;
                        newTask.assigned = task.assigned;
                        newTask.completed = task.completed;
                        newTask.ThemeColor = task.ThemeColor;

                    }

                }
                else
                {
                    Task newTask1= new Task();
                    DateTime? dateOrNull = task.startDate;
                  
                    if (dateOrNull != null)
                    {
                         task.startDate= dateOrNull.Value;
                    }
                    DateTime? dateOrNull2= task.endDate;

                    if (dateOrNull2!= null)
                    {
                        task.endDate = dateOrNull2.Value;
                    }
                    newTask1.Name = task.Name;
                    newTask1.startDate = dateOrNull.Value;
                    newTask1.endDate = dateOrNull2.Value;
                    newTask1.Description = task.Description;
                    newTask1.Category = task.Category;
                    newTask1.assigned = task.assigned;
                    newTask1.createdDate = DateTime.Now;
                    newTask1.completed = task.completed;
                    newTask1.ThemeColor = task.ThemeColor;
                    dc.Tasks.Add(newTask1);
                }
                dc.SaveChanges();
                success = true;
            }
            return new JsonResult { Data = new { success = success } };
        }

        [HttpPost]
        public JsonResult DeleteTask(int eventID)
        {
            var success = false;
            using (TasksEntities dc = new TasksEntities())
            {
                var oldTask = dc.Tasks.Where(a => a.EventID == eventID).FirstOrDefault();
                if (oldTask != null)
                {
                    dc.Tasks.Remove(oldTask);
                    dc.SaveChanges();
                    success = true;

                }
                return new JsonResult { Data = new { success = success } };
            }
        }
    }
}
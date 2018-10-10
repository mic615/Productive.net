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
    }
}
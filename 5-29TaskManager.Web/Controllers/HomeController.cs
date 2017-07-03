using _5_29TaskManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.Data;

namespace _5_29TaskManager.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TaskManagerRepository repo = new TaskManagerRepository(Properties.Settings.Default.ConStr);
            UserManager manager = new UserManager(Properties.Settings.Default.ConStr);
            IndexVM vm = new IndexVM
            {
                Tasks = repo.GetAllTasks(),
                User = manager.GetUser(User.Identity.Name)
            };
            return View(vm);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskManager.Data;

namespace _5_29TaskManager.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signin(string emailAddress, string password)
        {
            var manager = new UserManager(Properties.Settings.Default.ConStr);
            var user = manager.Login(emailAddress, password);
            if (user == null)
            {
                return Redirect("/account/signin");
            }

            FormsAuthentication.SetAuthCookie(emailAddress, true);
            return Redirect("/");
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(string firstName, string lastName, string email, string password)
        {
            var manager = new UserManager(Properties.Settings.Default.ConStr);
            manager.AddUser(firstName, lastName, email, password);
            FormsAuthentication.SetAuthCookie(email, true);
            return Redirect("/");
        }

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public ActionResult GetUserId()
        {
            UserManager manager = new UserManager(Properties.Settings.Default.ConStr);
            User user = manager.GetUser(User.Identity.Name);
            return Json(user.Id, JsonRequestBehavior.AllowGet);
        }
    }
}
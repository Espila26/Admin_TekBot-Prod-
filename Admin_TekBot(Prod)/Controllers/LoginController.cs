using Admin_TekBot_Prod_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM_DEV.Controllers
{
    public class LoginController : Controller
    {

        private BotKnowledgeDB_Entities db = new BotKnowledgeDB_Entities();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Cuenta(string mail, string User_Password)
        {

            if (!String.IsNullOrEmpty(mail) && !String.IsNullOrEmpty(User_Password))
            {

                var users = from e in db.Users select e;

                users = users.Where(s => (s.mail.Equals(mail)));

                if (users.Count() > 0)
                {
                  if (users.First().User_Password.Equals(User_Password))
                    {
                        Session["user"] = users.First();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            TempData["Error"] = "Failed Login";
            return RedirectToAction("Login", "Login");
        }

        public ActionResult CerrarSession()
        {
            Session.RemoveAll();
            return RedirectToAction("Login", "Login");
        }
    }
}
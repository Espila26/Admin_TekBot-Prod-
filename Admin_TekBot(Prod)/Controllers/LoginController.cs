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

        public ActionResult Cuenta(string NOMBRE_USUARIO, string CONTRASEÑA)
        {

            if (!String.IsNullOrEmpty(NOMBRE_USUARIO) && !String.IsNullOrEmpty(CONTRASEÑA))
            {

                var users = from e in db.Users
                               select e;

                users = users.Where(s => (s.UserName.Equals(NOMBRE_USUARIO)));

                if (users.Count() > 0)
                {
                    if (users.First().User_Password.Equals(CONTRASEÑA))
                    {
                        Session["user"] = users.First();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult CerrarSession()
        {
            Session.RemoveAll();
            return RedirectToAction("Login", "Login");
        }
    }
}
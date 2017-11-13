using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Admin_TekBot_Prod_.Models;

namespace Admin_TekBot_Prod_.Controllers
{
    public class UsersController : Controller
    {
        private BotKnowledgeDB_Entities db = new BotKnowledgeDB_Entities();

        // GET: Users
        public ActionResult Index(string searchString)
        {
            var users = from e in db.Users select e;
            if (!String.IsNullOrEmpty(searchString))
            {
                //Muestra los empleados por el estado que el usuario definió previamente
                if (searchString.Equals("Inactive") || searchString.Equals("Active"))
                {
                    users = users.Where(s => s.Users_Status.Equals(searchString));
                }

                else if (searchString.Equals("All"))
                {
                    users = users.Where(s => s.Users_Status.Contains("tiv"));
                }

                else if (searchString.Equals("Select"))
                {
                    TempData["Error"] = "Please Select the Filter!";
                    return RedirectToAction("Index");
                }

                //Muestra los empleados que coincidan con el nombre, apellidos o cedula que el usuario desea ver.
                else
                {
                    users = users.Where(s => s.UserName.Contains(searchString) || s.Users_positions.Contains(searchString));
                }

                //si no existe registros que coicidan con el criterio de busqueda, se muestra el mensaje de error.
                if (users.Count() == 0)
                {
                    TempData["Error"] = "No Results!";
                    return RedirectToAction("Index");

                }

            }

            return View(users);
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int var_id = Int32.Parse(id);
            var var_user = db.Users.Where(i => i.User_Number == var_id);
            User user = (User)var_user.First();

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mail,UserName,TeamId,Last_Review,Users_Status,Users_positions,Users_vacations,User_Number")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", user.TeamId);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int var_id = Int32.Parse(id);
            var var_user = db.Users.Where(i => i.User_Number == var_id);
            User user = (User)var_user.First();
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", user.TeamId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mail,UserName,TeamId,Last_Review,Users_Status,Users_positions,Users_vacations,User_Number")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", user.TeamId);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int var_id = Int32.Parse(id);
            var var_user = db.Users.Where(i => i.User_Number == var_id);
            User user = (User)var_user.First();

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

        [HttpPost]
        public ActionResult formAction(string[] childChkbox)
        {
            if (childChkbox == null)
            {
                TempData["Error"] = "Please Select an User!";
                return RedirectToAction("Index");
            }
            else
            {
                if (Request.Form["Details"] != null)
                {
                    if (childChkbox.Count() == 1)
                    {
                        return RedirectToAction("Details", "Users", new { id = childChkbox.First() });
                    }
                    else
                    {
                        TempData["Error"] = "You can only select an user!";
                        return RedirectToAction("Index");
                    }
                }
                else if (Request.Form["Edit"] != null)
                {

                    if (childChkbox.Count() == 1)
                    {
                        return RedirectToAction("Edit", "Users", new { id = childChkbox.First() });
                    }
                    else
                    {
                        TempData["Error"] = "You can only select an user!";
                        return RedirectToAction("Index");
                    }
                }
                else if (Request.Form["Inactive"] != null)
                {
                    foreach (var i in childChkbox)
                    {
                        var users = db.Users.Find(Int32.Parse(i));
                        users.Users_Status = "Inactive";
                        db.SaveChanges();
                    }
                    TempData["Success"] = "The user have been disable!";
                    return RedirectToAction("Index");
                }

                else if (Request.Form["Active"] != null)
                {
                    foreach (var i in childChkbox)
                    {
                        var users = db.Users.Find(Int32.Parse(i));
                        users.Users_Status = "Active";
                        db.SaveChanges();
                    }
                    TempData["Success"] = "The user have been enable!";
                    return RedirectToAction("Index");
                }
                return View();
            }
        }
    }
}

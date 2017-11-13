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
    public class TeamsController : Controller
    {
        private BotKnowledgeDB_Entities db = new BotKnowledgeDB_Entities();

        // GET: Teams
        public ActionResult Index(string searchString)
        {
            var teams = from e in db.Teams select e;
            if (!String.IsNullOrEmpty(searchString))
            {
                //Muestra los empleados por el estado que el usuario definió previamente
                if (searchString.Equals("Inactive") || searchString.Equals("Active"))
                {
                    teams = teams.Where(s => s.TeamName.Equals(searchString));
                }

                //Muestra los empleados que coincidan con el nombre, apellidos o cedula que el usuario desea ver.
                else
                {
                    teams = teams.Where(s => s.TeamName.Contains(searchString));
                }

                //si no existe registros que coicidan con el criterio de busqueda, se muestra el mensaje de error.
                if (teams.Count() == 0)
                {
                    TempData["Error"] = "No Results!";
                    return RedirectToAction("Index");

                }

            }
            return View(teams);
        }

        // GET: Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeamId,TeamName")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(team);
        }

        // GET: Teams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamId,TeamName")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
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
                TempData["Error"] = "Please Select an Team!";
                return RedirectToAction("Index");
            }
            else
            {
                if (Request.Form["Details"] != null)
                {
                    if (childChkbox.Count() == 1)
                    {
                        return RedirectToAction("Details", "Teams", new { id = childChkbox.First() });
                    }
                    else
                    {
                        TempData["Error"] = "You can only select an team!";
                        return RedirectToAction("Index");
                    }
                }
                else if (Request.Form["Edit"] != null)
                {

                    if (childChkbox.Count() == 1)
                    {
                        return RedirectToAction("Edit", "Teams", new { id = childChkbox.First() });
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

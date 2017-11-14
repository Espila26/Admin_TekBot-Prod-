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
    public class CasesController : Controller
    {
        private BotKnowledgeDB_Entities db = new BotKnowledgeDB_Entities();

        // GET: Cases
        public ActionResult Index(string searchString)
        {
            var Cases = from e in db.Cases select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                //Muestra los empleados por el estado que el usuario definió previamente
                if (searchString.Equals("Closed") || searchString.Equals("Notified") || searchString.Equals("Acknowledged") || searchString.Equals("Transfered"))
                {
                    Cases = Cases.Where(s => s.Case_Status.Equals(searchString));
                }

                else if (searchString.Equals("Select"))
                {
                    TempData["Error"] = "Please Select the Filter!";
                    return RedirectToAction("Index");
                }

                //Muestra los empleados que coincidan con el nombre, apellidos o cedula que el usuario desea ver.
                else
                {
                    Cases = Cases.Where(s => s.Case_number.Contains(searchString) || s.Engineer_name.Contains(searchString));
                }

                //si no existe registros que coicidan con el criterio de busqueda, se muestra el mensaje de error.
                if (Cases.Count() == 0)
                {
                    TempData["Error"] = "No Results!";
                    return RedirectToAction("Index");

                }

            }

            return View(Cases);
        }

        // GET: Cases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // GET: Cases/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CaseId,Engineer_name,Team,Case_number,Case_type,Case_Status,Case_severity,Full_Date,Day_Number,Month_Number,Week_Year,Year_Number,Day_Name,Month_Name,Comments,AssignedBy,Full_Date_Close,Create_Date,Close_Date")] Case @case)
        {
            if (ModelState.IsValid)
            {
                db.Cases.Add(@case);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@case);
        }

        // GET: Cases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // POST: Cases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CaseId,Engineer_name,Team,Case_number,Case_type,Case_Status,Case_severity,Full_Date,Day_Number,Month_Number,Week_Year,Year_Number,Day_Name,Month_Name,Comments,AssignedBy,Full_Date_Close,Create_Date,Close_Date")] Case @case)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@case).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@case);
        }

        // GET: Cases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // POST: Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Case @case = db.Cases.Find(id);
            db.Cases.Remove(@case);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Taekwondo.Models;

namespace Taekwondo.Controllers
{
    public class ParentsController : Controller
    {
        private Taekwondo_DBEntities db = new Taekwondo_DBEntities();

        // GET: Parents
        public ActionResult Index()
        {
            var parents = db.Parents.Include(p => p.Student);
            return View(parents.ToList());
        }

        // GET: Parents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // GET: Parents/Create
        public ActionResult Create()
        {
            if (Session["StudentID"].ToString() != "")
                ViewBag.sid = Session["StudentID"];
            if (Session["StudentName"].ToString() != "")
                ViewBag.sname = Session["StudentName"];
            ViewBag.StudentID = db.Students.Select(s => new SelectListItem
            {
                Text = s.StudentID + "-" + s.FirstName + " " + s.LastName,
                Value = s.StudentID.ToString()
            });
            return View();
        }

        // POST: Parents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ParentD,ParentType,FName,LName,Email,Phone,Street,City,ZipCode,StudentID,IsStudent")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                db.Parents.Add(parent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentID = db.Students.Select(s => new SelectListItem
            {
                Text = s.StudentID + "-" + s.FirstName + " " + s.LastName,
                Value = s.StudentID.ToString()
            });
            return View(parent);
        }

        // GET: Parents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }

            ViewBag.StudentID = db.Students.Select(s => new SelectListItem
            {
                Text = s.StudentID + "-" + s.FirstName + " " + s.LastName,
                Value = s.StudentID.ToString()
            });
            return View(parent);
        }

        // POST: Parents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ParentD,ParentType,FName,LName,Email,Phone,Street,City,ZipCode,StudentID,IsStudent")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentID = db.Students.Select(s => new SelectListItem
            {
                Text = s.StudentID + "-" + s.FirstName + " " + s.LastName,
                Value = s.StudentID.ToString()
            });
            return View(parent);
        }

        // GET: Parents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // POST: Parents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Parent parent = db.Parents.Find(id);
            db.Parents.Remove(parent);
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
    }
}

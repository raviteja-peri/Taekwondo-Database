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
    public class EnrollmentsController : Controller
    {
        private Taekwondo_DBEntities db = new Taekwondo_DBEntities();

        // GET: Enrollments
        public ActionResult Index(int? searchID)
        {
            
            var enrollments = db.Enrollments.Include(e => e.Class).Include(e => e.Student);
            if(searchID!=null)
            {
                enrollments = enrollments.Where(e => e.ClassID == searchID);
                return View(enrollments.ToList());
            }
            return View(enrollments.ToList());
        }

        // GET: Enrollments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create()
        {
            ViewBag.ClassID = db.Classes.Select(c => new SelectListItem
            {
                Text = c.ClassID + "-" + c.Day,
                Value = c.ClassID.ToString()
            });
            ViewBag.StudentID = db.Students.Select(s => new SelectListItem
            {
                Text = s.StudentID + "-" + s.FirstName + " " + s.LastName,
                Value = s.StudentID.ToString()
            });
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,ClassID,DateEnrolled")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassID = db.Classes.Select(c => new SelectListItem
            {
                Text = c.ClassID + "-" + c.Day,
                Value = c.ClassID.ToString()
            });
            ViewBag.StudentID = db.Students.Select(s => new SelectListItem
            {
                Text = s.StudentID + "-" + s.FirstName + " " + s.LastName,
                Value = s.StudentID.ToString()
            });
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "Day", enrollment.ClassID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,ClassID,DateEnrolled")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "Day", enrollment.ClassID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
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

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
    public class AttendancesController : Controller
    {
        private Taekwondo_DBEntities db = new Taekwondo_DBEntities();

        // GET: Attendances
        public ActionResult Index(int? searchString,DateTime? searchDate)
        {
            var attendances = db.Attendances.Include(a => a.Class).Include(a => a.Student);
            if(searchString!=null)
            {
                attendances = attendances.Where(a => a.StudentID == searchString);
                return View(attendances.ToList());
            }
            if(searchDate!=null)
            {
                attendances = attendances.Where(a => a.Date==searchDate);
                return View(attendances.ToList());
            }
            return View(attendances.ToList());
        }

        // GET: Attendances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // GET: Attendances/Create
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

        // POST: Attendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Date,Status,StudentID,ClassID")] Attendance attendance)
        {
            ModelState.Remove("ClassID");
            if (ModelState.IsValid)
            {
                db.Attendances.Add(attendance);
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
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "Day", attendance.ClassID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", attendance.StudentID);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Date,Status,StudentID,ClassID")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "Day", attendance.ClassID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", attendance.StudentID);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attendance attendance = db.Attendances.Find(id);
            db.Attendances.Remove(attendance);
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

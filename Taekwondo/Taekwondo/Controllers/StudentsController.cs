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
    public class StudentsController : Controller
    {
        private Taekwondo_DBEntities db = new Taekwondo_DBEntities();

        // GET: Students
        public ActionResult Index(string sortOrder, string searchString, int? searchID)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            var students = db.Students.Include(s => s.Rank);
            students = students.Where(s => s.Status.Equals("Active"));
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }
            if(searchID!=null)
            {
                students = students.Where(s => s.StudentID == searchID);
                return View(students.ToList());
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "id_desc":
                    students = students.OrderByDescending(s => s.StudentID);
                    break;
                default:
                    students = students.OrderBy(s => s.StudentID);
                    break;
            }
            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.RankID = db.Ranks.Select(r => new SelectListItem
            {
                Text = r.RankID + "-" + r.BeltColor,
                Value = r.RankID.ToString()
            });
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,FirstName,LastName,DateofBirth,DateofJoining,MobileNumber,Email,Status,LivingwithParent,RankID,Street,City,ZipCode")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                Session["StudentID"] = student.StudentID;
                Session["StudentName"] = student.FirstName + student.LastName;
                ViewBag.sid = student.StudentID;
                if (student.LivingwithParent=="Yes")
                {
                    return Redirect("~/Parents/Create");
                }
                return RedirectToAction("Index");
            }

            ViewBag.RankID = db.Ranks.Select(r => new SelectListItem
            {
                Text = r.RankID + "-" +r.BeltColor,
                Value = r.RankID.ToString()
            });
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.RankID = db.Ranks.Select(r => new SelectListItem
            {
                Text = r.RankID + "-" + r.BeltColor,
                Value = r.RankID.ToString()
            });
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,FirstName,LastName,DateofBirth,DateofJoining,MobileNumber,Email,Status,LivingwithParent,RankID,Street,City,ZipCode")] Student student)
        {
            ModelState.Remove("StudentID");
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RankID = db.Ranks.Select(r => new SelectListItem
            {
                Text = r.RankID + "-" + r.BeltColor,
                Value = r.RankID.ToString()
            });
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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

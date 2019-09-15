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
    public class PaymentsController : Controller
    {
        private Taekwondo_DBEntities db = new Taekwondo_DBEntities();

        // GET: Payments
        public ActionResult Index(DateTime? searchFromDate,DateTime? searchToDate)
        {
            var payments = db.Payments.Include(p => p.Class).Include(p => p.Product).Include(p => p.Student);
            if (searchFromDate != null && searchToDate != null)
            {
                payments = payments.Where(p => p.PaymentDate >= searchFromDate && p.PaymentDate <= searchToDate);
                return View(payments.ToList());
            }
            return View(payments.ToList());
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
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
            ViewBag.ProductID = db.Products.Select(p => new SelectListItem
            {
                Text = p.ProductID + "-" + p.ProductType,
                Value = p.ProductID.ToString()
            });

            return View();
        }
        public ActionResult GetProductAmount(int productID)
        {
            //logic to find country code goes here
            Product p = db.Products.Find(productID);
            double amount = p.SellingPrice;
            return Content(amount.ToString());
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReferenceID,ProductID,Amount,PaymentDate,ClassID,StudentID")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
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
            ViewBag.ProductID = db.Products.Select(p => new SelectListItem
            {
                Text = p.ProductID + "-" + p.ProductType,
                Value = p.ProductID.ToString()
            });
            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassID", payment.ClassID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductID", payment.ProductID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentID", payment.StudentID);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReferenceID,ProductID,Amount,PaymentDate,ClassID,StudentID")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassID", payment.ClassID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductID", payment.ProductID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentID", payment.StudentID);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
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

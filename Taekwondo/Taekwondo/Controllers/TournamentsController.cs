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
    public class TournamentsController : Controller
    {
        private Taekwondo_DBEntities db = new Taekwondo_DBEntities();

        // GET: Tournaments
        public ActionResult Index(string searchString)
        {
            var tournaments = db.Tournaments.Include(t => t.Class).Include(t => t.Rank).Include(t => t.Student);
            if(!String.IsNullOrEmpty(searchString))
            {
                tournaments = tournaments.Where(t => t.Rank.BeltColor.Contains(searchString));
                return View(tournaments.ToList());
            }
            return View(tournaments.ToList());
        }

        // GET: Tournaments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // GET: Tournaments/Create
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
            ViewBag.RankID = db.Ranks.Select(r => new SelectListItem
            {
                Text = r.RankID + "-" + r.BeltColor,
                Value = r.RankID.ToString()
            });
            return View();
        }

        // POST: Tournaments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,RankID,TournamentDate,Location,ClassID")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                db.Tournaments.Add(tournament);
                Student profile = db.Students.Find(tournament.StudentID);
                if(profile!=null)
                {
                    profile.RankID = tournament.RankID;
                }
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
            ViewBag.RankID = db.Ranks.Select(r => new SelectListItem
            {
                Text = r.RankID + "-" + r.BeltColor,
                Value = r.RankID.ToString()
            });
            return View(tournament);
        }

        // GET: Tournaments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "Day", tournament.ClassID);
            ViewBag.RankID = new SelectList(db.Ranks, "RankID", "BeltColor", tournament.RankID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", tournament.StudentID);
            return View(tournament);
        }

        // POST: Tournaments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,RankID,TournamentDate,Location,ClassID")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournament).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "Day", tournament.ClassID);
            ViewBag.RankID = new SelectList(db.Ranks, "RankID", "BeltColor", tournament.RankID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", tournament.StudentID);
            return View(tournament);
        }

        // GET: Tournaments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tournament tournament = db.Tournaments.Find(id);
            db.Tournaments.Remove(tournament);
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

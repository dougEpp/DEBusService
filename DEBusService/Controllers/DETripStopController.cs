using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DEBusService.Models;

namespace DEBusService.Controllers
{
    public class DETripStopController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        // GET: DETripStop
        public ActionResult Index()
        {
            var tripStops = db.tripStops.Include(t => t.busStop).Include(t => t.trip);
            return View(tripStops.ToList());
        }

        // GET: DETripStop/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tripStop tripStop = db.tripStops.Find(id);
            if (tripStop == null)
            {
                return HttpNotFound();
            }
            return View(tripStop);
        }

        // GET: DETripStop/Create
        public ActionResult Create()
        {
            ViewBag.busStopNumber = new SelectList(db.busStops, "busStopNumber", "location");
            ViewBag.tripId = new SelectList(db.trips, "tripId", "comments");
            return View();
        }

        // POST: DETripStop/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tripStopId,tripId,busStopNumber,tripStopTime,comments")] tripStop tripStop)
        {
            if (ModelState.IsValid)
            {
                db.tripStops.Add(tripStop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.busStopNumber = new SelectList(db.busStops, "busStopNumber", "location", tripStop.busStopNumber);
            ViewBag.tripId = new SelectList(db.trips, "tripId", "comments", tripStop.tripId);
            return View(tripStop);
        }

        // GET: DETripStop/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tripStop tripStop = db.tripStops.Find(id);
            if (tripStop == null)
            {
                return HttpNotFound();
            }
            ViewBag.busStopNumber = new SelectList(db.busStops, "busStopNumber", "location", tripStop.busStopNumber);
            ViewBag.tripId = new SelectList(db.trips, "tripId", "comments", tripStop.tripId);
            return View(tripStop);
        }

        // POST: DETripStop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tripStopId,tripId,busStopNumber,tripStopTime,comments")] tripStop tripStop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tripStop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.busStopNumber = new SelectList(db.busStops, "busStopNumber", "location", tripStop.busStopNumber);
            ViewBag.tripId = new SelectList(db.trips, "tripId", "comments", tripStop.tripId);
            return View(tripStop);
        }

        // GET: DETripStop/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tripStop tripStop = db.tripStops.Find(id);
            if (tripStop == null)
            {
                return HttpNotFound();
            }
            return View(tripStop);
        }

        // POST: DETripStop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tripStop tripStop = db.tripStops.Find(id);
            db.tripStops.Remove(tripStop);
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

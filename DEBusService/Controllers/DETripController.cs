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
    public class DETripController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        // GET: DETrip
        public ActionResult Index()
        {
            var trips = db.trips.Include(t => t.bus).Include(t => t.driver).Include(t => t.routeSchedule);
            return View(trips.ToList());
        }

        // GET: DETrip/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip trip = db.trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // GET: DETrip/Create
        public ActionResult Create()
        {
            ViewBag.busId = new SelectList(db.buses, "busId", "status");
            ViewBag.driverId = new SelectList(db.drivers, "driverId", "firstName");
            ViewBag.routeScheduleId = new SelectList(db.routeSchedules, "routeScheduleId", "busRouteCode");
            return View();
        }

        // POST: DETrip/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tripId,routeScheduleId,tripDate,driverId,busId,comments")] trip trip)
        {
            if (ModelState.IsValid)
            {
                db.trips.Add(trip);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.busId = new SelectList(db.buses, "busId", "status", trip.busId);
            ViewBag.driverId = new SelectList(db.drivers, "driverId", "firstName", trip.driverId);
            ViewBag.routeScheduleId = new SelectList(db.routeSchedules, "routeScheduleId", "busRouteCode", trip.routeScheduleId);
            return View(trip);
        }

        // GET: DETrip/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip trip = db.trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            ViewBag.busId = new SelectList(db.buses, "busId", "status", trip.busId);
            ViewBag.driverId = new SelectList(db.drivers, "driverId", "firstName", trip.driverId);
            ViewBag.routeScheduleId = new SelectList(db.routeSchedules, "routeScheduleId", "busRouteCode", trip.routeScheduleId);
            return View(trip);
        }

        // POST: DETrip/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tripId,routeScheduleId,tripDate,driverId,busId,comments")] trip trip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.busId = new SelectList(db.buses, "busId", "status", trip.busId);
            ViewBag.driverId = new SelectList(db.drivers, "driverId", "firstName", trip.driverId);
            ViewBag.routeScheduleId = new SelectList(db.routeSchedules, "routeScheduleId", "busRouteCode", trip.routeScheduleId);
            return View(trip);
        }

        // GET: DETrip/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trip trip = db.trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // POST: DETrip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            trip trip = db.trips.Find(id);
            db.trips.Remove(trip);
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

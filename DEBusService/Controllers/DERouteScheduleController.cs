//This is the controller to create, read, update and delete bus stops
//Revision History:
//  2015/09/16 Created, Doug Epp
//  2015/09/29 Added RouteStopSchedule action and view
//  2015/09/30 Finished RouteStopSchedule code

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
    public class DERouteScheduleController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        // GET: RouteSchedule
        public ActionResult Index()
        {
            var routeSchedules = db.routeSchedules.Include(r => r.busRoute);
            return View(routeSchedules.ToList());
        }

        // GET: RouteSchedule/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            routeSchedule routeSchedule = db.routeSchedules.Find(id);
            if (routeSchedule == null)
            {
                return HttpNotFound();
            }
            return View(routeSchedule);
        }

        // GET: RouteSchedule/Create
        public ActionResult Create()
        {
            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName");
            return View();
        }

        // POST: RouteSchedule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "routeScheduleId,busRouteCode,startTime,isWeekDay,comments")] routeSchedule routeSchedule)
        {
            if (ModelState.IsValid)
            {
                db.routeSchedules.Add(routeSchedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName", routeSchedule.busRouteCode);
            return View(routeSchedule);
        }

        // GET: RouteSchedule/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            routeSchedule routeSchedule = db.routeSchedules.Find(id);
            if (routeSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName", routeSchedule.busRouteCode);
            return View(routeSchedule);
        }

        // POST: RouteSchedule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "routeScheduleId,busRouteCode,startTime,isWeekDay,comments")] routeSchedule routeSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(routeSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName", routeSchedule.busRouteCode);
            return View(routeSchedule);
        }

        // GET: RouteSchedule/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            routeSchedule routeSchedule = db.routeSchedules.Find(id);
            if (routeSchedule == null)
            {
                return HttpNotFound();
            }
            return View(routeSchedule);
        }

        // POST: RouteSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            routeSchedule routeSchedule = db.routeSchedules.Find(id);
            db.routeSchedules.Remove(routeSchedule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Shows the user all scheduled stops for the selected route and stop
        /// </summary>
        /// <param name="id">the id of the selected routeStop </param>
        /// <returns>the list of routeSchedules for the selected route and stop</returns>
        public ActionResult RouteStopSchedule(int? id)
        {
            try
            {
                if (id == null)//no routeStop has been passed to the action
                {
                    throw new Exception("Please select a bus stop and route.");
                }

                routeStop routeStop = db.routeStops.Find(id);
                if (routeStop == null)//the selected routeStop doesn't exist in the database
                {
                    throw new Exception("Please select a valid bus stop and route.");
                }

                //find all routeSchedules for the selected route
                var routeSchedules = db.routeSchedules.Where(s => s.busRouteCode == routeStop.busRouteCode);
                if (routeSchedules.ToList().Count == 0)//there are no schedules in the database for the selected route
                {
                    throw new Exception("There are no schedules associated with that route.");
                }


                //get the offsetminutes for the selected route stop
                double minutes = (double)routeStop.offsetMinutes;
                TimeSpan offSetMinutes = TimeSpan.FromMinutes(minutes);
                ViewBag.OffsetMinutes = offSetMinutes;

                ViewBag.StopLocation = routeStop.busStop.location;
                return View(routeSchedules);
            }
            catch (Exception ex)
            {
                //if something went wrong, send user back to list of bus stops with the appropriate error message
                TempData["message"] = ex.Message;
                return RedirectToAction("Index", "DEBusStop");
            }
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

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

        /// <summary>
        /// Shows a list of all route schedules
        /// </summary>
        /// <returns>A list of all route schedules</returns>
        public ActionResult Index()
        {
            var routeSchedules = db.routeSchedules.Include(r => r.busRoute);
            return View(routeSchedules.ToList());
        }

        /// <summary>
        /// Shows details for the selected route schedule
        /// </summary>
        /// <param name="id">the id of the selected route schedule</param>
        /// <returns>Details for the selected route schedule</returns>
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

        /// <summary>
        /// Allows the user to create a new route schedule
        /// </summary>
        /// <returns>A form for the user to create a new route schedule</returns>
        public ActionResult Create()
        {
            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName");
            return View();
        }

        /// <summary>
        /// Binds and saves the new route schedule to the database
        /// </summary>
        /// <param name="routeSchedule">The newly created routeSchedule object</param>
        /// <returns>a redirect to the index action</returns>
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

        /// <summary>
        /// Allows the user to edit a selected route schedule
        /// </summary>
        /// <param name="id">The id of the route schedule to be edited</param>
        /// <returns>a form for the user to edit the selected route schedule</returns>
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

        /// <summary>
        /// Binds and saves the newly edited route schedule to the database
        /// </summary>
        /// <param name="routeSchedule">the edited routeSchedule object</param>
        /// <returns>a redirect to the index action</returns>
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

        /// <summary>
        /// Allows the user to delete a selected route schedule
        /// </summary>
        /// <param name="id">the id of the route schedule to be deleted</param>
        /// <returns>the route schedule to be deleted</returns>
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

        /// <summary>
        /// Deletes the selected record from the database
        /// </summary>
        /// <param name="id">The id of the selected route schedule</param>
        /// <returns>the index view, minus the newly deleted route schedule</returns>
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
                var routeSchedules = db.routeSchedules
                    .Where(s => s.busRouteCode == routeStop.busRouteCode)
                    .OrderBy(s=>s.startTime);
                if (routeSchedules.ToList().Count == 0)//there are no schedules in the database for the selected route
                {
                    throw new Exception("There are no schedules associated with that route.");
                }

                //get the offsetminutes for the selected route stop
                double minutes = (double)routeStop.offsetMinutes;
                TimeSpan offSetMinutes = TimeSpan.FromMinutes(minutes);
                ViewBag.OffsetMinutes = offSetMinutes;

                ViewBag.BusStop = routeStop.busStop;
                return View(routeSchedules);
            }
            catch (Exception ex)
            {
                //if something went wrong, send user back to list of bus stops with the appropriate error message
                TempData["message"] = ex.Message;
            }
            return RedirectToAction("Index", "DEBusStop");
        }
        /// <summary>
        /// Cleans up memory resources and connections for this session
        /// </summary>
        /// <param name="disposing">Event handler for disposing of memory</param>
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

//This controller allows users to create, read, update and delete route stops
//Revision History:
//  2015/09/16 Created, Doug Epp

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
    public class DERouteStopController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        /// <summary>
        /// Provides a list of bus route stops, given the id of the bus route
        /// </summary>
        /// <param name="id">the code for the selected bus route</param>
        /// <returns>A list of stops for the selected route</returns>
        public ActionResult Index(int? id)
        {
            var routeStops = db.routeStops.Include(r => r.busRoute).Include(r => r.busStop);

            try
            {
                if (id != null)
                {
                    Session["busRouteCode"] = id.ToString();
                }
                else if (Session["busRouteCode"] != null)
                {
                    id = int.Parse(Session["busRouteCode"].ToString());
                }
                else
                {
                    throw new Exception("Please select a bus route.");
                }

                routeStops = routeStops.Where(r => r.busRouteCode.Equals(id.ToString()));

                ViewBag.RouteName = id + " - " + db.busRoutes.Find(id.ToString()).routeName;
                
                //Experimental; checking how many times a route stops at a given stop
                var rs = routeStops.ToList();
                List<int> numTimes = new List<int>();
                for (int i = 0; i < rs.Count; i++)
                {
                    numTimes.Add(0);
                    foreach (var item in routeStops)
                    {
                        if (item.busStopNumber == rs[i].busStopNumber)
                        {
                        numTimes[i]++;

                        }
                    }
                }
                ViewBag.rs = rs;
                ViewBag.NumTimes = numTimes;

                return View(routeStops.OrderBy(r => r.offsetMinutes).ToList());
            }
            catch (NullReferenceException)
            {
                TempData["message"] = "Please select a valid bus route code.";
                return RedirectToAction("Index", "DEBusRoute");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return RedirectToAction("Index", "DEBusRoute");
            }
        }

        /// <summary>
        /// Provides the details for the selected route stop
        /// </summary>
        /// <param name="id">The id of the selected stop</param>
        /// <returns>The details for the selected stop</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            routeStop routeStop = db.routeStops.Find(id);
            if (routeStop == null)
            {
                return HttpNotFound();
            }
            ViewBag.RouteName = routeStop.busRoute.busRouteCode + " - " + routeStop.busRoute.routeName;
            return View(routeStop);
        }

        /// <summary>
        /// Allows the user to connect a bus stop with a bus route
        /// </summary>
        /// <returns>The form for creating a new route stop record</returns>
        public ActionResult Create()
        {
            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName");
            ViewBag.busStopNumber = new SelectList(db.busStops, "busStopNumber", "location");
            try
            {

                busRoute busRoute = db.busRoutes.Find(Session["busRouteCode"]);
                ViewBag.BusRoute = busRoute;
                ViewBag.RouteName = busRoute.busRouteCode + " - " + busRoute.routeName;
            }
            catch (Exception)
            {
                TempData["message"] = "Please select a bus route first";
                return RedirectToAction("Index", "DEBusRoute");
            }

            return View();
        }

        /// <summary>
        /// Posts the created routestop record to the database
        /// </summary>
        /// <param name="routeStop">The new routestop object to be saved</param>
        /// <returns>a redirect to the list of bus stops, using the stored route code</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "routeStopId,busRouteCode,busStopNumber,offsetMinutes")] routeStop routeStop)
        {
            if (ModelState.IsValid)
            {
                routeStop.busRouteCode = Session["busRouteCode"].ToString();
                db.routeStops.Add(routeStop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName", routeStop.busRouteCode);
            ViewBag.busStopNumber = new SelectList(db.busStops, "busStopNumber", "location", routeStop.busStopNumber);
            return View(routeStop);
        }

        /// <summary>
        /// Allows the user to edit a selected routestop
        /// </summary>
        /// <param name="id">The id of the selected routestop</param>
        /// <returns>The form for editing the selected route stop</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            routeStop routeStop = db.routeStops.Find(id);
            if (routeStop == null)
            {
                return HttpNotFound();
            }

            ViewBag.RouteName = routeStop.busRoute.busRouteCode + " - " + routeStop.busRoute.routeName;

            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName", routeStop.busRouteCode);
            ViewBag.busStopNumber = new SelectList(db.busStops, "busStopNumber", "location", routeStop.busStopNumber);
            return View(routeStop);
        }

        /// <summary>
        /// Binds and saves the edited route stop to the database
        /// </summary>
        /// <param name="routeStop">The newly edited route stop object</param>
        /// <returns>A redirect to the list of route stops, using the stored route stop code</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "routeStopId,busRouteCode,busStopNumber,offsetMinutes")] routeStop routeStop)
        {
            if (ModelState.IsValid)
            {
                routeStop.busRouteCode = Session["busRouteCode"].ToString();
                db.Entry(routeStop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName", routeStop.busRouteCode);
            ViewBag.busStopNumber = new SelectList(db.busStops, "busStopNumber", "location", routeStop.busStopNumber);
            return View(routeStop);
        }

        /// <summary>
        /// Allows the user to delete a selected route stop
        /// </summary>
        /// <param name="id">The id of the stop to be deleted</param>
        /// <returns>The route stop to be deleted</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            routeStop routeStop = db.routeStops.Find(id);
            if (routeStop == null)
            {
                return HttpNotFound();
            }

            ViewBag.RouteName = routeStop.busRoute.busRouteCode + " - " + routeStop.busRoute.routeName;

            return View(routeStop);
        }

        /// <summary>
        /// Deletes the selected route stop from the database
        /// </summary>
        /// <param name="id">the id of the route stop to be deleted</param>
        /// <returns>a redirect to the list of route stops, using the stored bus route code</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            routeStop routeStop = db.routeStops.Find(id);
            db.routeStops.Remove(routeStop);
            db.SaveChanges();
            return RedirectToAction("Index");
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

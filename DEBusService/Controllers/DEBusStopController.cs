//This is the controller to create, read, update and delete bus stops
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
    /// <summary>
    /// A class which lets users Create, Read, Update and Delete information for bus stops
    /// </summary>
    public class DEBusStopController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        // GET: DEBusStop
        /// <summary>
        /// retrieves all bus stops on file and returns them as a list
        /// </summary>
        /// <returns>a list of all bus stops, ordered by location hash</returns>
        public ActionResult Index(string orderBy)
        {
            List<busStop> busStops = new List<busStop>();
            if (orderBy == "location")
            {
                busStops = db.busStops.OrderBy(s => s.location).ToList();
            }
            else
            {
                busStops = db.busStops.OrderBy(s => s.busStopNumber).ToList();
            }

            return View(busStops);
        }

        // GET: DEBusStop/Details/5
        /// <summary>
        /// Shows the details about a selected bus stop
        /// </summary>
        /// <param name="id">The id of the selected bus stop</param>
        /// <returns>The selected bus stop object</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            busStop busStop = db.busStops.Find(id);
            if (busStop == null)
            {
                return HttpNotFound();
            }
            ViewBag.Hash = GenerateLocationHash(busStop.location);
            return View(busStop);
        }

        // GET: DEBusStop/Create
        /// <summary>
        /// Allows the user to create a new bus stop
        /// </summary>
        /// <returns>a form to create a new bus stop</returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: DEBusStop/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Posts and saves the newly created bus stop
        /// </summary>
        /// <param name="busStop">The new bus stop object to be saved</param>
        /// <returns>if valid, the list of all bus stops including the new one; else, redirect to the create page</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "busStopNumber,location,goingDowntown")] busStop busStop)
        {
            if (ModelState.IsValid)
            {
                busStop.locationHash = GenerateLocationHash(busStop.location);
                db.busStops.Add(busStop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(busStop);
        }

        // GET: DEBusStop/Edit/5
        /// <summary>
        /// Allows the user to edit a selected bus stop 
        /// </summary>
        /// <param name="id">The id of the selected bus stop</param>
        /// <returns>A form to edit the bus stop</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            busStop busStop = db.busStops.Find(id);
            if (busStop == null)
            {
                return HttpNotFound();
            }
            return View(busStop);
        }

        // POST: DEBusStop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Saves the changes made to the selected bus stop, if any
        /// </summary>
        /// <param name="busStop">the bus stop object with new fields</param>
        /// <returns>the list of bus stops, with the newly saved data</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "busStopNumber,location,goingDowntown")] busStop busStop)
        {
            if (ModelState.IsValid)
            {
                busStop.locationHash = GenerateLocationHash(busStop.location);
                db.Entry(busStop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(busStop);
        }

        // GET: DEBusStop/Delete/5
        /// <summary>
        /// Allows the user to delete a selected bus stop
        /// </summary>
        /// <param name="id">The id of the bus stop to be deleted</param>
        /// <returns>The bus stop to be deleted</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            busStop busStop = db.busStops.Find(id);
            if (busStop == null)
            {
                return HttpNotFound();
            }
            return View(busStop);
        }

        // POST: DEBusStop/Delete/5
        /// <summary>
        /// Deletes the selected record from the database
        /// </summary>
        /// <param name="id">The id of the selected bus stop</param>
        /// <returns>the index view, minus the newly deleted record</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            busStop busStop = db.busStops.Find(id);
            db.busStops.Remove(busStop);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RouteSelector(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Please select a bus stop number");
                }
                else
                {
                    Session["busStopId"] = id;
                }

                List<routeStop> routeStops = db.routeStops.Where(r=>r.busStop.busStopNumber == id).ToList();
                if (routeStops.Count == 0)
                {
                    throw new Exception("There are no routes using the selected stop.");
                }
                if (routeStops.Count == 1)
                {
                    routeStop routeStop = db.routeStops.Where(r=>r.busStop.busStopNumber == id && r.busStopNumber == id).FirstOrDefault();
                    return RedirectToAction("RouteStopSchedule", "DERouteSchedule", new { id = routeStop.routeStopId });
                }

                routeStops = routeStops.GroupBy(r=>r.busRoute).SelectMany(r=>r).ToList();
                List<busRoute> busRoutes = new List<busRoute>();

                foreach(routeStop r in routeStops)
                {
                    busRoutes.Add(r.busRoute);
                }


                ViewBag.busRoutes = new SelectList(busRoutes.OrderBy(r=>r.routeName), "busRouteCode", "routeName");

                return View(routeStops);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RouteSelector(string busRoutes)
        {
            int busStopId = int.Parse(Session["busStopId"].ToString());
            routeStop routeStop = db.routeStops.Where(s => s.busRouteCode == busRoutes && s.busStop.busStopNumber == busStopId).FirstOrDefault();
            return RedirectToAction("RouteStopSchedule", "DERouteSchedule", new {id=routeStop.routeStopId} );
        }

        /// <summary>
        /// Returns a numeric value based on the string address of a bus stop
        /// </summary>
        /// <param name="location">The string to be hashed</param>
        /// <returns>Hash value of the address</returns>
        private int GenerateLocationHash(string location)
        {
            int result = 0;

            try
            {
                foreach (char c in location)
                {
                    result += Convert.ToByte(c);
                }
            }
            catch (Exception)
            {
                result = -1;
            }

            return result;
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

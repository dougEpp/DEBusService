//This controller allows users to create, read, update and delete bus routes
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
    public class DEBusRouteController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        // GET: DEBusRoute
        /// <summary>
        /// Retrieves all bus routes on file and returns as a list
        /// </summary>
        /// <returns>The "index" view with a list of all bus routes in table</returns>
        public ActionResult Index()
        {
            return View(db.busRoutes.ToList());
        }

        // GET: DEBusRoute/Details/5
        /// <summary>
        /// Generates the "details" view for selected route
        /// </summary>
        /// <param name="id">The id of the selected bus route</param>
        /// <returns>All details about the selected route</returns>
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            busRoute busRoute = db.busRoutes.Find(id);
            if (busRoute == null)
            {
                return HttpNotFound();
            }
            return View(busRoute);
        }

        // GET: DEBusRoute/Create
        /// <summary>
        /// Generates the "create" view
        /// </summary>
        /// <returns>a view to create a new bus route</returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: DEBusRoute/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Receives and saves the new bus route
        /// </summary>
        /// <param name="busRoute">the user's created bus route</param>
        /// <returns>the newly saved bus route</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "busRouteCode,routeName")] busRoute busRoute)
        {
            if (ModelState.IsValid)
            {
                db.busRoutes.Add(busRoute);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(busRoute);
        }

        // GET: DEBusRoute/Edit/5
        /// <summary>
        /// Allows the user to edit the selected bus route
        /// </summary>
        /// <param name="id">The id of the selected route</param>
        /// <returns>the new information about the route</returns>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            busRoute busRoute = db.busRoutes.Find(id);
            if (busRoute == null)
            {
                return HttpNotFound();
            }
            return View(busRoute);
        }

        // POST: DEBusRoute/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// the httpPost stage of editing a route
        /// </summary>
        /// <param name="busRoute">The new information about the route</param>
        /// <returns>The newly edited bus route</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "busRouteCode,routeName")] busRoute busRoute)
        {
            if (ModelState.IsValid)
            {
                db.Entry(busRoute).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(busRoute);
        }

        // GET: DEBusRoute/Delete/5
        /// <summary>
        /// Allows the user to delete the selected route
        /// </summary>
        /// <param name="id">The id of the selected bus route</param>
        /// <returns>The newly deleted bus route</returns>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            busRoute busRoute = db.busRoutes.Find(id);
            if (busRoute == null)
            {
                return HttpNotFound();
            }
            return View(busRoute);
        }

        // POST: DEBusRoute/Delete/5
        /// <summary>
        /// Deletes the selected bus route
        /// </summary>
        /// <param name="id">The id of the route to be deleted</param>
        /// <returns>deletes the route</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            busRoute busRoute = db.busRoutes.Find(id);
            db.busRoutes.Remove(busRoute);
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

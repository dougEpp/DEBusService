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
    /// A class to create, read, update and delete countries
    /// </summary>
    public class DECountryController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        /// <summary>
        /// Retrieves all countries on file and returns as a list
        /// </summary>
        /// <returns>A list of all countries in table</returns>
        public ActionResult Index()
        {
            return View(db.countries.ToList());
        }

        /// <summary>
        /// Generates the "details" view for selected country
        /// </summary>
        /// <param name="id">The id of the selected country</param>
        /// <returns>All details about the selected country</returns>
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            country country = db.countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        /// <summary>
        /// Generates the "create" view
        /// </summary>
        /// <returns>a view to create a new country</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Receives, validates and saves the new country
        /// </summary>
        /// <param name="country">The newly created country object</param>
        /// <returns>a redirect to the index view, including the newly created country</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "countryCode,name,postalPattern,phonePattern")] country country)
        {
            if (ModelState.IsValid)
            {
                db.countries.Add(country);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(country);
        }

        /// <summary>
        /// Allows the user to edit a specified country record
        /// </summary>
        /// <param name="id">The id of the selected country</param>
        /// <returns>The form to edit the selected country</returns>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            country country = db.countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        /// <summary>
        /// Receives, validates and saves the newly edited country
        /// </summary>
        /// <param name="country">The newly edited country object</param>
        /// <returns>a redirect to the index view, including the newly edited country</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "countryCode,name,postalPattern,phonePattern")] country country)
        {
            if (ModelState.IsValid)
            {
                db.Entry(country).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(country);
        }

        /// <summary>
        /// Allows the user to delete the selected country
        /// </summary>
        /// <param name="id">The id of the selected country</param>
        /// <returns>The country to delete</returns>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            country country = db.countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        /// <summary>
        /// Deletes the selected country
        /// </summary>
        /// <param name="id">The id of the route to be deleted</param>
        /// <returns>deletes the route</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            country country = db.countries.Find(id);
            db.countries.Remove(country);
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

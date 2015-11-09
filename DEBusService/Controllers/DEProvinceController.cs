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
    /// A class to create, read, update and delete provinces
    /// </summary>
    public class DEProvinceController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        /// <summary>
        /// Retrieves all provinces on file and returns as a list
        /// </summary>
        /// <returns>A list of all provinces in table</returns>
        public ActionResult Index()
        {
            var provinces = db.provinces.Include(p => p.country);
            return View(provinces.ToList());
        }

        /// <summary>
        /// Generates the "details" view for selected province
        /// </summary>
        /// <param name="id">The id of the selected province</param>
        /// <returns>All details about the selected province</returns>
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            province province = db.provinces.Find(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            return View(province);
        }

        /// <summary>
        /// Generates the "create" view
        /// </summary>
        /// <returns>a view to create a new province</returns>
        public ActionResult Create()
        {
            ViewBag.countryCode = new SelectList(db.countries, "countryCode", "name");
            return View();
        }

        /// <summary>
        /// Receives, validates and saves the new province
        /// </summary>
        /// <param name="province">The newly created province object</param>
        /// <returns>a redirect to the index view, including the newly created province</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "provinceCode,name,countryCode,taxCode,taxRate,capital")] province province)
        {
            if (ModelState.IsValid)
            {
                db.provinces.Add(province);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.countryCode = new SelectList(db.countries, "countryCode", "name", province.countryCode);
            return View(province);
        }

        /// <summary>
        /// Allows the user to edit a specified province record
        /// </summary>
        /// <param name="id">The id of the selected province</param>
        /// <returns>The form to edit the selected province</returns>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            province province = db.provinces.Find(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            ViewBag.countryCode = new SelectList(db.countries, "countryCode", "name", province.countryCode);
            return View(province);
        }

        /// <summary>
        /// Receives, validates and saves the newly edited province
        /// </summary>
        /// <param name="province">The newly edited province object</param>
        /// <returns>a redirect to the index view, including the newly edited province</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "provinceCode,name,countryCode,taxCode,taxRate,capital")] province province)
        {
            if (ModelState.IsValid)
            {
                db.Entry(province).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.countryCode = new SelectList(db.countries, "countryCode", "name", province.countryCode);
            return View(province);
        }
        /// <summary>
        /// Allows the user to delete the selected province
        /// </summary>
        /// <param name="id">The id of the selected province</param>
        /// <returns>The province to delete</returns>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            province province = db.provinces.Find(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            return View(province);
        }
        /// <summary>
        /// Deletes the selected province
        /// </summary>
        /// <param name="id">The id of the route to be deleted</param>
        /// <returns>deletes the route</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            province province = db.provinces.Find(id);
            db.provinces.Remove(province);
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

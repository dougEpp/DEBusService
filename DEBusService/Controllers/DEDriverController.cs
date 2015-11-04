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
    /// A class to create, read, update and delete a driver record
    /// </summary>
    public class DEDriverController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        /// <summary>
        /// Shows a list of all drivers to the user
        /// </summary>
        /// <returns>A list of driver records</returns>
        public ActionResult Index()
        {
            var drivers = db.drivers.Include(d => d.province)
                .OrderBy(d => d.fullName);
            return View(drivers.ToList());
        }

        /// <summary>
        /// Shows the details of a selected driver record
        /// </summary>
        /// <param name="id">The id of the selected driver</param>
        /// <returns>Details about the selected driver</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driver driver = db.drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        /// <summary>
        /// Allows the user to create a new driver
        /// </summary>
        /// <returns>The form for creating a new driver record</returns>
        public ActionResult Create()
        {
            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(p => p.name), "provinceCode", "name");
            return View();
        }

        /// <summary>
        /// Creates a new driver record based on the submitted information
        /// </summary>
        /// <param name="driver">The newly created driver object</param>
        /// <returns>A redirect to the index action, including the newly created driver</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "driverId,firstName,lastName,homePhone,workPhone,street,city,postalCode,provinceCode,dateHired")] driver driver)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.drivers.Add(driver);
                    driver.fullName = driver.lastName + ", " + driver.firstName;
                    db.SaveChanges();
                    TempData["message"] = "Successfully added new driver: " + driver.fullName;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.GetBaseException().Message);
            }

            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(p => p.name), "provinceCode", "name", driver.provinceCode);
            return View(driver);
        }
        /// <summary>
        /// Allows the user to edit a specified driver record
        /// </summary>
        /// <param name="id">The id of the driver to edit</param>
        /// <returns>Ther form for editing the selected driver record</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driver driver = db.drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(p => p.name), "provinceCode", "name", driver.provinceCode);
            return View(driver);
        }

        /// <summary>
        /// Validates and saves the edited driver record
        /// </summary>
        /// <param name="driver">The newly edited driver record</param>
        /// <returns>A redirect to the index action, including the newly edited driver record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "driverId,firstName,lastName,fullName,homePhone,workPhone,street,city,postalCode,provinceCode,dateHired")] driver driver)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    driver.fullName = driver.lastName + ", " + driver.firstName;
                    db.Entry(driver).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = "Successfully edited driver: " + driver.fullName;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.GetBaseException().Message);
            }
            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(p => p.name), "provinceCode", "name", driver.provinceCode);
            return View(driver);
        }

        /// <summary>
        /// Allows the user to delete a specified driver record
        /// </summary>
        /// <param name="id">The id of the seleced driver</param>
        /// <returns>A confirmation message for deleting the driver</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driver driver = db.drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        /// <summary>
        /// Deletes the selected record
        /// </summary>
        /// <param name="id">The id of the record to delete</param>
        /// <returns>A redirect to the list of drivers, minus the deleted record</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                driver driver = db.drivers.Find(id);
                db.drivers.Remove(driver);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.GetBaseException().Message;
            }
            return Delete(id);
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

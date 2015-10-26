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
    public class DEDriverController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        // GET: DEDriver
        public ActionResult Index()
        {
            var drivers = db.drivers.Include(d => d.province);
            return View(drivers.ToList());
        }

        // GET: DEDriver/Details/5
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

        // GET: DEDriver/Create
        public ActionResult Create()
        {
            ViewBag.provinceCode = new SelectList(db.provinces, "provinceCode", "name");
            return View();
        }

        // POST: DEDriver/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "driverId,firstName,lastName,fullName,homePhone,workPhone,street,city,postalCode,provinceCode,dateHired")] driver driver)
        {
            if (ModelState.IsValid)
            {
                db.drivers.Add(driver);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.provinceCode = new SelectList(db.provinces, "provinceCode", "name", driver.provinceCode);
            return View(driver);
        }

        // GET: DEDriver/Edit/5
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
            ViewBag.provinceCode = new SelectList(db.provinces, "provinceCode", "name", driver.provinceCode);
            return View(driver);
        }

        // POST: DEDriver/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "driverId,firstName,lastName,fullName,homePhone,workPhone,street,city,postalCode,provinceCode,dateHired")] driver driver)
        {
            if (ModelState.IsValid)
            {
                db.Entry(driver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.provinceCode = new SelectList(db.provinces, "provinceCode", "name", driver.provinceCode);
            return View(driver);
        }

        // GET: DEDriver/Delete/5
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

        // POST: DEDriver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            driver driver = db.drivers.Find(id);
            db.drivers.Remove(driver);
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

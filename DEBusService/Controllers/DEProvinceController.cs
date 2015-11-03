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
    public class DEProvinceController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        // GET: DEProvince
        public ActionResult Index()
        {
            var provinces = db.provinces.Include(p => p.country);
            return View(provinces.ToList());
        }

        // GET: DEProvince/Details/5
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

        // GET: DEProvince/Create
        public ActionResult Create()
        {
            ViewBag.countryCode = new SelectList(db.countries, "countryCode", "name");
            return View();
        }

        // POST: DEProvince/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: DEProvince/Edit/5
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

        // POST: DEProvince/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: DEProvince/Delete/5
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

        // POST: DEProvince/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            province province = db.provinces.Find(id);
            db.provinces.Remove(province);
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

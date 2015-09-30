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
    public class DEBusController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        // GET: DEBus
        public ActionResult Index()
        {
            return View(db.buses.ToList());
        }

        // GET: DEBus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bus bus = db.buses.Find(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            return View(bus);
        }

        // GET: DEBus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DEBus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "busId,busNumber,status,comments")] bus bus)
        {
            if (ModelState.IsValid)
            {
                db.buses.Add(bus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bus);
        }

        // GET: DEBus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bus bus = db.buses.Find(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            return View(bus);
        }

        // POST: DEBus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "busId,busNumber,status,comments")] bus bus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bus);
        }

        // GET: DEBus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bus bus = db.buses.Find(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            return View(bus);
        }

        // POST: DEBus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bus bus = db.buses.Find(id);
            db.buses.Remove(bus);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DEBusService.Models;

namespace DEBusService.Controllers
{
    public class DETripController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        public ActionResult Index(string busRouteCode, string routeName)
        {
            try
            {
                if (busRouteCode == null)
                {
                    if (Session["busRouteCode"] == null)
                    {
                        throw new Exception("Please select a bus route");
                    }
                    else
                    {
                        busRouteCode = Session["busRouteCode"].ToString();
                        routeName = Session["routeName"].ToString();
                    }
                }
                else
                {
                    Session["busRouteCode"] = busRouteCode;
                    Session["routeName"] = routeName;
                }
                var trips = from record in db.trips
                            where record.routeSchedule.busRouteCode == busRouteCode
                            orderby record.tripDate descending
                            select record;
                //if (trips.ToList().Count == 0)
                //{
                //    throw new Exception("No trips available");
                //}
                return View(trips);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
            }
            return RedirectToAction("Index", "DEBusRoute");
        }

        public ActionResult Create()
        {
            try
            {
                if (Session["busRouteCode"] == null)
                {
                    throw new Exception("Please select a bus route first.");
                }
                string busRouteCode = Session["busRouteCode"].ToString();

                var routeScheduleId = from record in db.routeSchedules
                                     where record.busRouteCode == busRouteCode
                                     orderby record.isWeekDay descending
                                     select new SelectListItem
                                     {
                                         Text = record.startTime + " - " + ((record.isWeekDay) ? "Weekdays" : "Weekends"),
                                         Value = record.routeScheduleId.ToString()
                                     };
                var driverId = from record in db.drivers
                              orderby record.fullName
                              select new SelectListItem
                              {
                                  Text = record.fullName,
                                  Value = record.driverId.ToString()
                              };
                var buses = from record in db.buses
                            where record.status == "available"
                            select record;
                ViewBag.routeScheduleId = routeScheduleId;
                ViewBag.driverId = driverId;
                ViewBag.Buses = buses;
                return View();
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return RedirectToAction("Index", "DEBusRoute");
            }
        }
        [HttpPost]
        public ActionResult Create(trip trip)
        {
            if (ModelState.IsValid)
            {
                db.trips.Add(trip);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            string busRouteCode = Session["busRouteCode"].ToString();
            var routeScheduleId = from record in db.routeSchedules
                                  where record.busRouteCode == busRouteCode
                                  orderby record.isWeekDay descending
                                  select new SelectListItem
                                  {
                                      Text = record.startTime + " - " + ((record.isWeekDay) ? "Weekdays" : "Weekends"),
                                      Value = record.routeScheduleId.ToString()
                                  };
            var driverId = from record in db.drivers
                           orderby record.fullName
                           select new SelectListItem
                           {
                               Text = record.fullName,
                               Value = record.driverId.ToString()
                           };
            var buses = from record in db.buses
                        where record.status == "available"
                        select record;
            ViewBag.routeScheduleId = routeScheduleId;
            ViewBag.driverId = driverId;
            ViewBag.buses = buses;
            return View(trip);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DEBusService.Models;
using DEBusService.Models.ViewModels;

namespace DEBusService.Controllers
{
    /// <summary>
    /// Controller for displaying and creating monitored trips
    /// </summary>
    public class DETripController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        /// <summary>
        /// Displays a list of trips for a selected bus route
        /// </summary>
        /// <param name="busRouteCode">The selected bus route's code</param>
        /// <param name="routeName">The selected bus route's name</param>
        /// <returns>A list of bus trips for the selected bus route</returns>
        public ActionResult Index(string busRouteCode, string routeName)
        {
            try
            {
                if (busRouteCode == null)//no route has been passed to the action
                {
                    if (Session["busRouteCode"] == null)//a route has not previously been passed
                    {
                        throw new Exception("Please select a bus route");
                    }
                    else //there is a a saved bus route code
                    {
                        busRouteCode = Session["busRouteCode"].ToString();
                        routeName = Session["routeName"].ToString();
                    }
                }
                else //a route is passed to the action
                {
                    Session["busRouteCode"] = busRouteCode;
                    Session["routeName"] = routeName;
                }
                //get the list of trips for the selected bus route
                var trips = from record in db.trips
                            where record.routeSchedule.busRouteCode == busRouteCode
                            orderby record.tripDate descending, record.routeSchedule.startTime
                            select record;

                return View(trips);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
            }
            //If something went wrong, go back to bus routes index
            return RedirectToAction("Index", "DEBusRoute");
        }
        /// <summary>
        /// Allows the user to create a new bus trip
        /// </summary>
        /// <returns>The form for creating a new bus trip</returns>
        public ActionResult Create()
        {
            try
            {
                if (Session["busRouteCode"] == null)//no route has been selected
                {
                    throw new Exception("Please select a bus route first.");
                }

                string busRouteCode = Session["busRouteCode"].ToString();

                //generate a list of custom "StartTime" view models
                List<StartTimeModel> startTimes = new List<StartTimeModel>();

                foreach (var routeSchedule in db.routeSchedules.Where(r => r.busRouteCode == busRouteCode).OrderByDescending(r => r.isWeekDay).ThenBy(r => r.startTime))
                {
                    string startTime = routeSchedule.startTime.ToString("c") + ((routeSchedule.isWeekDay) ? " - Weekdays" : " - Weekends");
                    startTimes.Add(new StartTimeModel(startTime, routeSchedule.routeScheduleId));
                }

                //generate select list of custom startTime objects
                var routeScheduleId = from record in startTimes
                                      select new SelectListItem
                                      {
                                          Text = record.startTime,
                                          Value = record.routeScheduleId.ToString()
                                      };
                //generate select list of drivers
                var driverId = from record in db.drivers
                               orderby record.fullName
                               select new SelectListItem
                               {
                                   Text = record.fullName,
                                   Value = record.driverId.ToString()
                               };
                //get list of available buses
                var buses = from record in db.buses
                            where record.status == "available"
                            orderby record.busNumber
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
        /// <summary>
        /// Adds the newly created bus trip to the database
        /// </summary>
        /// <param name="trip">The newly created trip object</param>
        /// <returns>A redirect to the index of trips for the saved bus route</returns>
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
            //generate a list of custom "StartTime" view models

            List<StartTimeModel> startTimes = new List<StartTimeModel>();

            foreach (var routeSchedule in db.routeSchedules.Where(r => r.busRouteCode == busRouteCode).OrderByDescending(r => r.isWeekDay).ThenBy(r => r.startTime))
            {
                string startTime = routeSchedule.startTime.ToString("c") + ((routeSchedule.isWeekDay) ? " - Weekdays" : " - Weekends");
                startTimes.Add(new StartTimeModel(startTime, routeSchedule.routeScheduleId));
            }

            //generate select list of custom startTime objects
            var routeScheduleId = from record in startTimes
                                  select new SelectListItem
                                  {
                                      Text = record.startTime,
                                      Value = record.routeScheduleId.ToString()
                                  };

            //generate list of drivers
            var driverId = from record in db.drivers
                           orderby record.fullName
                           select new SelectListItem
                           {
                               Text = record.fullName,
                               Value = record.driverId.ToString()
                           };
            //get list of available buses
            var buses = from record in db.buses
                        where record.status == "available"
                        select record;

            ViewBag.routeScheduleId = routeScheduleId;
            ViewBag.driverId = driverId;
            ViewBag.buses = buses;
            return View(trip);
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
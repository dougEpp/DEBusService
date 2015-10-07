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
        // GET: DETrip
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
    }
}
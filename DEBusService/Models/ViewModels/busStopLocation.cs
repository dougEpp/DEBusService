using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEBusService.Models.ViewModels
{
    public class busStopLocation
    {
        private string locationName;

        public string LocationName
        {
            get { return locationName; }
            set { locationName = value; }
        }
        private bool hasRoutes;

        public bool HasRoutes
        {
            get { return hasRoutes; }
            set { hasRoutes = value; }
        }

        public busStopLocation(string locationName, bool hasRoutes)
        {
            this.locationName = locationName;
            this.hasRoutes = hasRoutes;
        }

    }
}
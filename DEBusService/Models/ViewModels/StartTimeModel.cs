using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEBusService.Models.ViewModels
{
    /// <summary>
    /// A class to model bus trip start times and schedules
    /// </summary>
    public class StartTimeModel
    {
        public string startTime;
        public int routeScheduleId;

        /// <summary>
        /// Constructor for the StartTimeModel view model
        /// </summary>
        /// <param name="startTime">The trip's start time</param>
        /// <param name="routeScheduleId">The id of the routeschedule</param>
        public StartTimeModel(string startTime, int routeScheduleId)
        {
            this.startTime = startTime;
            this.routeScheduleId = routeScheduleId;
        }
    }
}
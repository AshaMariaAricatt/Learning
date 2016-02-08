using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigerSpike
{
    #region Object class
    /// <summary>
    /// Meetingroom Allocation Object class
    /// </summary>
    class RoomAllocation
    {
        public DateTime OfficeHourStart { get; set; }
        public DateTime OfficeHoursEnd { get; set; }
        public DateTime SubmissionTime { get; set; }
        public DateTime MeetingTime { get; set; }
        public int MeetingDuration { get; set; }
        public string EmployeeId { get; set; }

        /// <summary>
        /// Meetingroom Allocation Constructor
        /// Accepts office hours and request details as input 
        /// </summary>
        public RoomAllocation(string OfficeHours, string RequestDetails)
        {
            OfficeHourStart = DateTime.Parse(OfficeHours.Substring(0, 2) + ":" + OfficeHours.Substring(2, 2));

            OfficeHoursEnd = DateTime.Parse(OfficeHours.Substring(4, 2) + ":" + OfficeHours.Substring(6, 2));

            SubmissionTime = DateTime.Parse(RequestDetails.Substring(0, 10) + " " + RequestDetails.Substring(10, 8));

            EmployeeId = RequestDetails.Substring(18, 6);

            MeetingTime = DateTime.Parse(RequestDetails.Substring(24, 10) + " " + RequestDetails.Substring(34, 5));

            MeetingDuration = Int32.Parse(RequestDetails.Substring(39, 1));

        }
    }
    #endregion
}

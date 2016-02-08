using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigerSpike
{
    /// <summary>
    /// Performs file read,check conditions,prints output
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            int NumberOfRecords;
            Program obj = new Program();

            #region Readfile
            /// <summary>
            /// Read input file from the location
            /// </summary>
            FileStream fs = new FileStream("Input\\Input.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);

            List<RoomAllocation> RequestList = new List<RoomAllocation>();

            string OfficeHours = sr.ReadLine();
            string RequestDetails = sr.ReadLine();
            RequestDetails = RequestDetails + sr.ReadLine();

            while (RequestDetails != "")
            {
                RequestList.Add(new RoomAllocation(OfficeHours, RequestDetails)); //adds to list of objects
                RequestDetails = sr.ReadLine();
                RequestDetails = RequestDetails + sr.ReadLine();
            }
            NumberOfRecords = RequestList.Count();
            fs.Close();
            #endregion

            //--- sort list in chronological order-----------
            List<RoomAllocation> SortedList = RequestList.OrderBy(o => o.SubmissionTime).ToList();

            #region RemoveOutside OfficeHours
            /// <summary>
            /// remove outside hours
            /// </summary>
            RequestList.Clear();
            foreach (RoomAllocation RAC in SortedList)
            {
                int duration = RAC.MeetingDuration;
                DateTime MeetinEndTime = RAC.MeetingTime.AddHours(duration);
                int t1 = TimeSpan.Compare(RAC.MeetingTime.TimeOfDay, RAC.OfficeHourStart.TimeOfDay);
                int t2 = TimeSpan.Compare(RAC.OfficeHoursEnd.TimeOfDay, MeetinEndTime.TimeOfDay);

                if (t2 == -1 || t1 == -1)
                {
                    //outside office hours
                    // To do :Send an email to employee showing that the booking was unsuccessful;
                }
                else
                {

                    RequestList.Add(RAC); //inside hours added to list
                }

            }
            #endregion

            #region Checking Overlap Requests
            /// <summary>
            /// Check for overlap request by passing four arguments to private method CheckForOverlap
            /// Non overlapping request are added to list
            /// </summary>
            SortedList.Clear();
            SortedList.Add(RequestList[0]);

            bool res = false;
            for (int i = 1; i < RequestList.Count(); i++)
            {
                for (int k = 0; k < SortedList.Count; k++)
                {
                    string result = obj.CheckForOverlap(RequestList[i].MeetingTime, RequestList[i].MeetingDuration, SortedList[k].MeetingTime, SortedList[k].MeetingDuration);
                    if (result.Equals("Success"))
                    {
                        res = true;

                    }
                    else
                    {
                        res = false;
                        break;
                    }
                }

                if (res)
                {
                    SortedList.Add(RequestList[i]);
                }
            }
            #endregion

            #region Output
            /// <summary>
            /// Output calender
            /// </summary>
            //--- sort list -----------
            RequestList = SortedList.OrderBy(o => o.MeetingTime).ToList();


            string meetingdate = RequestList[0].MeetingTime.ToString("yyyy-MM-dd");
            Console.WriteLine(meetingdate);
            for (int i = 0; i < RequestList.Count; i++)
            {
                int dur = RequestList[i].MeetingDuration;
                DateTime endtime = RequestList[i].MeetingTime.AddHours(dur);

                if (RequestList[i].MeetingTime.ToString("yyyy-MM-dd") != meetingdate)
                {
                    meetingdate = RequestList[i].MeetingTime.ToString("yyyy-MM-dd");
                    Console.WriteLine(meetingdate);
                }

                Console.Write(RequestList[i].MeetingTime.ToString("HH:mm"));//start time
                Console.Write(endtime.ToString("HH:mm"));// end time
                Console.WriteLine(RequestList[i].EmployeeId);
            }



            Console.ReadLine();
        }
            #endregion

            #region Checkoverlap private method
        /// <summary>
        /// Compare two timestamps and check overlap
        /// Checks if a given timerange is within the bounds of the instance
        /// </summary>
        /// <param name="T1"></param>
        /// <param name="T1hours"></param>
        /// <param name="T2"></param>
        /// <param name="T2Hours"></param>
        private string CheckForOverlap(DateTime T1, int T1Hours, DateTime T2, int T2Hours)
        {
            DateTime T1End = T1.AddHours(T1Hours);
            DateTime T2End = T2.AddHours(T2Hours);

            int t1 = DateTime.Compare(T1, T2);
            int t2 = DateTime.Compare(T1, T2End);
            if (t1 == -1 && t2 == -1)
            {
                return "Success";
            }
            if (t1 == -1 && t2 != -1)
            {
                return "fail";

            }
            if (t1 == 0)
            {
                return "fail";
            }
            if (t1 == 1 && t2 == -1)
            {
                return "fail";
            }
            if (t1 == 1)
            {
                return "Success";
            }

            return "fail";
        }


        #endregion
       }
}

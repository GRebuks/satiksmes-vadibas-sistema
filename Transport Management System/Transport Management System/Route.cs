using System;
using System.Collections.Generic;
using System.Linq;

namespace Transport_Management_System
{
    /// <summary>
    /// Contains information about available routes.
    /// Contains assigned drivers and transport to the specific route object.
    /// </summary>
    class Route : Information
    {
        // Private variables
        private static string title = "Maršruti";
        private static List<string> columnHeaders = new List<string>() { "Nosaukums", "Transporta tips", "Pieturas", "Laiks starp pieturām", "Maršrutu laiki"};

        private string name;
        private string transportType;
        private List<string> stops;
        private List<DateTime> stopTimeDifference = new List<DateTime>();
        private List<DateTime> routeStartTime = new List<DateTime>();

        // Constructor
        public Route(string name, string transportType, string stopString, string stopTimeDifferenceString, string routeStartTimeString)
        {
            this.name = name;
            this.transportType = transportType;
            stops = stopString.Split(", ").ToList();
            foreach (string timeDifference in stopTimeDifferenceString.Split(", "))
            {
                stopTimeDifference.Add(Convert.ToDateTime(timeDifference));
            }
            foreach (string startTime in routeStartTimeString.Split(","))
            {
                routeStartTime.Add(Convert.ToDateTime(startTime));
            } 
        }

        public Route()
        {
            name = "Sākuma pietura - galamērķis";
            transportType = "Transporta tips";
            stops = new List<string>();
            stopTimeDifference = new List<DateTime>();
            stopTimeDifference.Add(DateTime.MinValue);
            routeStartTime = new List<DateTime>();
            routeStartTime.Add(DateTime.MinValue);
        }
        public override List<dynamic> GetRow()
        {
            List<dynamic> row = new List<dynamic>();
            row.Add(name);
            row.Add(transportType);
            row.Add(stops.Count);
            row.Add(stopTimeDifference.Count);
            row.Add(routeStartTime.Count);
            return row;
        }
        public override List<dynamic> GetValues()
        {
            List<dynamic> row = new List<dynamic>();
            row.Add(name);
            row.Add(transportType);
            row.Add(String.Join(", ", stops));
            List<string> timeDiffString = new List<string>();
            foreach (DateTime timeDiff in stopTimeDifference)
            {
                timeDiffString.Add(timeDiff.ToString("H:m"));
            }
            row.Add(String.Join(", ", timeDiffString));
            timeDiffString = new List<string>();
            foreach (DateTime timeDiff in routeStartTime)
            {
                timeDiffString.Add(timeDiff.ToString("H:m"));
            }
            row.Add(String.Join(", ", timeDiffString));
            return row;
        }
        public override List<List<dynamic>> GetSpecific()
        {
            List<List<dynamic>> table = new List<List<dynamic>>();
            List<dynamic> row = new List<dynamic>();
            row.Add(name);
            row.Add(transportType);
            for (int i = 0; i < Math.Max(stops.Count, Math.Max(stopTimeDifference.Count, routeStartTime.Count)); i++)
            {
                if (i < stops.Count)
                    row.Add(stops[i]);
                else row.Add("");

                if (i < stopTimeDifference.Count)
                    row.Add(stopTimeDifference[i].ToString("H:m"));
                else row.Add("");

                if (i < routeStartTime.Count)
                    row.Add(routeStartTime[i].ToString("H:m"));
                else row.Add("");

                table.Add(row);
                row = new List<dynamic>();
                row.Add("");
                row.Add("");
            }
            foreach (string stop in stops)
            {
                System.Diagnostics.Debug.WriteLine(stop);
            }
            System.Diagnostics.Debug.WriteLine($"Count = {stops.Count}, Length = {stops.ToArray().GetLength(0)}");
            return table;
        }
        public override void SetValues(dynamic[] values)
        {
            this.name = values[0];
            this.transportType = values[1];
            stops = new List<string>(values[2].ToString().Split(", "));

            stopTimeDifference = new List<DateTime>();
            routeStartTime = new List<DateTime>();
            foreach (string timeDifference in values[3].Split(", "))
            {
                stopTimeDifference.Add(Convert.ToDateTime(timeDifference));
            }
            foreach (string startTime in values[4].Split(","))
            {
                routeStartTime.Add(Convert.ToDateTime(startTime));
            }
        }

        // Properties
        public override string Title
        {
            get { return title; }
        }
        public override List<string> ColumnHeaders
        {
            get { return columnHeaders; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private static List<string> columnHeaders = new List<string>() { "Nosaukums", "Pieturas", "Laiks starp pieturām", "Maršrutu laiki"};

        private string name;
        private string transportType;
        private List<string> stops;
        private List<DateTime> stopTimeDifference;
        private List<DateTime> routeStartTime;

        // Constructor
        public Route(string transportTypes)
        {
            this.transportType = transportTypes;
        }
        public override List<dynamic> GetRow()
        {
            List<dynamic> row = new List<dynamic>();
            row.Add(transportType);
            row.Add(String.Join(", ", stops));
            return row;
        }
        public override void SetValues(dynamic[] values)
        {
            throw new NotImplementedException();
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

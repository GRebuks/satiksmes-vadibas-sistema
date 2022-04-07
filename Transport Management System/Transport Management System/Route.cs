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
    class Route
    {
        // Private variables
        private List<string> _transportTypes;
        private List<string> _stops;
        private List<Driver> _drivers;
        private List<Transport> _transport;

        // Constructor
        public Route(List<string> transportTypes)
        {
            _transportTypes = transportTypes;
        }
    }
}

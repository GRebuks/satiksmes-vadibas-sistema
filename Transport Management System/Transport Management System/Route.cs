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
        private List<string> transportTypes;
        private List<string> stops;
        private List<Driver> drivers;
        private List<Transport> transport;

        // Constructor
        public Route(List<string> _transportTypes)
        {
            this.transportTypes = _transportTypes;
        }
    }
}

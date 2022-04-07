namespace Transport_Management_System
{
    /// <summary>
    /// Contains basic information of a transport.
    /// Base class to derived classes: Tram, Trolleybus, Bus, Minibus
    /// </summary>
    class Transport
    {
        // Private variables
        private string _type;
        private string _condition;

        // Constructor
        public Transport (string type, string condition = "N/A")
        {
            _type = type;
            _condition = condition;
        }

        // Properties
        public string Type { get; set; }
        public string Condition { get; set; }
    }
}

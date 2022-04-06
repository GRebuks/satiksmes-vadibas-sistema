namespace Transport_Management_System
{
    /// <summary>
    /// Contains basic information of a transport.
    /// Base class to derived classes: Tram, Trolleybus, Bus, Minibus
    /// </summary>
    class Transport
    {
        // Private variables
        private string type;
        private string condition;

        // Constructor
        public Transport (string _type, string _condition = "N/A")
        {
            this.type = _type;
            this.condition = _condition;
        }

        // Properties
        public string Type { get; set; }
        public string Condition { get; set; }
    }
}

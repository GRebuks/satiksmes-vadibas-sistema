using System.Collections.Generic;

namespace Transport_Management_System
{
    /// <summary>
    /// Contains basic information of a transport.
    /// Base class to derived classes: Tram, Trolleybus, Bus, Minibus
    /// </summary>
    class Transport : Information
    {
        // Private variables
        private static string title = "Transporti";
        public static List<string> columnHeaders = new List<string>() {"ID","Tips", "Stāvoklis" };

        private string type;
        private string condition;
        private int id;

        // Constructor
        public Transport (int id, string type, string condition = "N/A")
        {
            this.id = id;
            this.type = type;
            this.condition = condition;
        }
        public Transport()
        {
            type = "Type";
            condition = "N/A";
        }

        // Methods
        public override List<dynamic> GetRow()
        {
            List<dynamic> row = new List<dynamic>();
            row.Add(id);
            row.Add(type);
            row.Add(condition);
            return row;
        }
        public override void SetValues(dynamic[] values)
        {
            type = values[0];
            condition = values[1];
        }

        // Properties
        public string Type { get; set; }
        public string Condition { get; set; }

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

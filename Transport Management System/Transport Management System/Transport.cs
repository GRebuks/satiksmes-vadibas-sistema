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
        public static List<string> columnHeaders = new List<string>() {"ID", "Tips", "Stāvoklis" };

        private int id;
        private string type;
        private string condition;

        // Constructor
        public Transport (int id, string type, string condition = "N/A")
        {
            this.id = id;
            this.type = type;
            this.condition = condition;
        }
        public Transport()
        {
            id = Program.GetTransport[Program.GetTransport.Count - 1].ID + 1;
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
            id = values[0];
            type = values[1];
            condition = values[2];
        }

        // Properties
        public override string TransportType
        {
            get { return type; }
        }
        public override string TransportCondition
        {
            get { return condition; }
        }
        public override int ID
        {
            get { return id; }
        }
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

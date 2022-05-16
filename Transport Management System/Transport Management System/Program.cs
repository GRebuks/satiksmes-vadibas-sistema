using System;
using System.Collections.Generic;

namespace Transport_Management_System
{
    class Program
    {
        protected static List<Information> drivers = new List<Information>();
        protected static List<Information> routes = new List<Information>();
        protected static List<Information> transport = new List<Information>();
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;
            DBConnection db = new DBConnection();
            GetInformation(db);
            UserInterface.DB = db;
            UserInterface.MainMenu();
        }
        static void GetInformation(DBConnection db)
        {
            List<List<dynamic>> data = new List<List<dynamic>>();

            // Gets information about routes from database
            data = db.Select("Route");
            int id = 1;
            foreach (List<dynamic> row in data)
            {
                string name = row[1];
                string transportType = row[2];
                string stopString = row[3];
                string stopTimeDifferenceString = row[4];
                string routeStartTimeString = row[5];
                routes.Add(new Route(id, name, transportType, stopString, stopTimeDifferenceString, routeStartTimeString));
            }

            // Gets information about transport from database
            data = db.Select("Transport");
            id = 1;
            foreach (List<dynamic> row in data)
            {
                string transportType = row[1];
                string condition = row[2];
                Information new_transport = new Transport(id, transportType, condition);
                transport.Add(new_transport);
                id++;
            }

            // Gets information about drivers from database
            data = db.Select("Driver");
            id = 1;
            foreach (List<dynamic> row in data)
            {
                string name = row[1];
                string surname = row[2];
                string socialNumber = row[3];
                DateTime birthDate = row[4];
                List<string> specialities = new List<string>(row[5].ToString().Split(", "));
                Information driver = new Driver(id, name, surname, socialNumber, birthDate.Date, specialities);
                drivers.Add(driver);
                id++;
            }
        }

        public static List<Information> GetTransport
        {
            get { return transport; }
        }
        public static List<Information> GetDrivers
        {
            get { return drivers; }
        }
        public static List<Information> GetRoutes
        {
            get { return routes; }
        }
    }
}

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
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            DBConnection db = new DBConnection();
            GetInformation(db);
            UserInterface.DB = db;
            UserInterface.MainMenu();
            //TableBuilder tb = new TableBuilder();
            //PrintInformation(drivers);
            //Console.WriteLine(tb.BuildTable("Vadītāji", ObjectToDynamic(drivers)));
        }
        static void GetInformation(DBConnection db)
        {
            List<List<dynamic>> data = new List<List<dynamic>>();
            /*
            data = db.Select("Route");
            foreach (List<dynamic> row in data)
            {
                string speciality = row[5];
                routes.Add(new Route(speciality));
            }

            data = db.Select("Transport");
            */
            data = db.Select("Driver");
            foreach (List<dynamic> row in data)
            {
                string name = row[1];
                string surname = row[2];
                string socialNumber = row[3];
                DateTime birthDate = row[4];
                List<string> specialities = new List<string>(row[5].ToString().Split(","));
                Information driver = new Driver(name, surname, socialNumber, birthDate, specialities);
                drivers.Add(driver);
            }
        }
    }
}

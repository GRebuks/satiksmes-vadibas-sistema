using System;
using System.Collections.Generic;
using System.Linq;

namespace Transport_Management_System
{
    /// <summary>
    /// Contains information about a driver.
    /// </summary>
    class Driver : Information
    {
        // Private variables
        private static string title = "Vadītāji";
        public static List<string> columnHeaders = new List<string>() {"ID", "Vārds", "Uzvārds", "Personas kods", "Dzimšanas diena", "Specialitātes"};

        private int id;
        private string name;
        private string surname;
        private string socialNumber;
        private DateTime birthDate;
        private List<string> specialities;

        // Constructor
        public Driver(int id, string name, string surname, string socialNumber, DateTime birthDate, List<string> specialities)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.socialNumber = socialNumber;
            this.birthDate = birthDate.Date;
            this.specialities = specialities;
        }

        public Driver()
        {
            id = Program.GetDrivers[Program.GetDrivers.Count - 1].ID + 1;
            name = "Name";
            surname = "Surname";
            socialNumber = "Social number";
            birthDate = DateTime.MinValue.Date;
            specialities = new List<string>() {""};
        }

        public override List<dynamic> GetRow()
        {
            List<dynamic> row = new List<dynamic>();
            row.Add(id);
            row.Add(name);
            row.Add(surname);
            row.Add(socialNumber);
            row.Add(birthDate.ToString("yyyy-MM-dd"));
            row.Add(String.Join(", ", specialities));
            return row;
        }
        public override void SetValues(dynamic[] values)
        {
            id = values[0];
            name = values[1];
            surname = values[2];
            socialNumber = values[3];
            birthDate = Convert.ToDateTime(values[4]);
            string[] specs = values[5].Split(", ");
            specialities = specs.ToList();
        }
        // Deconstructor
        ~Driver()
        {
            System.Diagnostics.Debug.WriteLine("A driver has been deconstructed.");
        }
        // Properties
        public override string Title 
        {
            get { return title; }
        }
        public override int ID
        {
            get { return id; }
        }
        public override List<string> ColumnHeaders
        {
            get { return columnHeaders; }
        }
        public override List<string> Specialities
        {
            get { return specialities; }
        }
    }
}

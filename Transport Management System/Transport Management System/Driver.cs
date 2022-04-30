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
        public static List<string> columnHeaders = new List<string>() {"Vārds", "Uzvārds", "Personas kods", "Dzimšanas diena", "Specialitātes"};

        private string name;
        private string surname;
        private string socialNumber;
        private DateTime birthDate;
        private List<string> specialities;

        // Constructor
        public Driver(string name, string surname, string socialNumber, DateTime birthDate, List<string> specialities)
        {
            this.name = name;
            this.surname = surname;
            this.socialNumber = socialNumber;
            this.birthDate = birthDate.Date;
            this.specialities = specialities;
        }

        public Driver()
        {
            name = "Name";
            surname = "Surname";
            socialNumber = "Social number";
            birthDate = DateTime.MinValue.Date;
            specialities = new List<string>() {""};
        }

        public override List<dynamic> GetRow()
        {
            List<dynamic> row = new List<dynamic>();
            row.Add(name);
            row.Add(surname);
            row.Add(socialNumber);
            row.Add(birthDate.ToString("yyyy-MM-dd"));
            row.Add(String.Join(", ", specialities));
            return row;
        }
        public override void SetValues(dynamic[] values)
        {
            name = values[0];
            surname = values[1];
            socialNumber = values[2];
            birthDate = Convert.ToDateTime(values[3]);
            string[] specs = values[4].Split(", ");
            specialities = specs.ToList();
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

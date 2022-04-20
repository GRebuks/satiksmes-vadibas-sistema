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
        private string title = "Vadītāji";
        private List<string> columnHeaders = new List<string>() {"Vārds", "Uzvārds", "Personas kods", "Dzimšanas diena", "Specialitātes"};

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
            this.birthDate = birthDate;
            this.specialities = specialities;
        }

        public override List<dynamic> GetRow()
        {
            List<dynamic> row = new List<dynamic>();
            row.Add(name);
            row.Add(surname);
            row.Add(socialNumber);
            row.Add(birthDate);
            row.Add(String.Join(", ", specialities));
            return row;
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

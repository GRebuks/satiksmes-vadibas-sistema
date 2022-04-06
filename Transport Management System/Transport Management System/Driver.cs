using System;
using System.Collections.Generic;

namespace Transport_Management_System
{
    /// <summary>
    /// Contains information about a driver.
    /// </summary>
    class Driver
    {
        // Private variables
        private string name;
        private string surname;
        private string socialNumber;
        private DateTime birthDate;
        private List<string> specialities;

        // Constructor
        public Driver(string _name, string _surname, string _social_number, DateTime _birth_date, List<string> _specialities)
        {
            this.name = _name;
            this.surname = _surname;
            this.socialNumber = _social_number;
            this.birthDate = _birth_date;
            this.specialities = _specialities;
        }

        // Properties
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SocialNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public List<string> Specialities { get; set; }
    }
}

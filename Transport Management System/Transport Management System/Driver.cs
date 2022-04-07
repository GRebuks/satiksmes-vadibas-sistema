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
        private string _name;
        private string _surname;
        private string _socialNumber;
        private DateTime _birthDate;
        private List<string> _specialities;

        // Constructor
        public Driver(string name, string surname, string social_number, DateTime birth_date, List<string> specialities)
        {
            _name = name;
            _surname = surname;
            _socialNumber = social_number;
            _birthDate = birth_date;
            _specialities = specialities;
        }

        // Properties
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SocialNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public List<string> Specialities { get; set; }
    }
}

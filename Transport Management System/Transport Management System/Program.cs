using System;
using System.Collections.Generic;
using System.Linq;

namespace Transport_Management_System
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            DBConnection db = new DBConnection();
            TableBuilder tb = new TableBuilder();

            List<List<dynamic>> drivers = db.Select("driver");
            Console.WriteLine(tb.BuildTable("Vadītāji", drivers));
        }
    }
}

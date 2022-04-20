using System;
using System.Collections.Generic;
using System.Linq; // .Cast<dynamic>(), .ToList()
using System.Runtime.CompilerServices;

namespace Transport_Management_System
{
    class UserInterface : Program
    {
        private const int DEFAULT_OPTION_COUNT = 2;
        public static void MainMenu()
        {
            TableBuilder tb = new TableBuilder();
            List<string> options = new List<string>() { "Darbības ar informāciju", "Opcija divi", "Opcija trīs" };
            Console.WriteLine(tb.BuildSelector("Sākuma izvēlne", options));

            int input = Input(options.Count);
            
            Console.Clear();
            switch (input)
            {
                case 1:
                    TableSelection();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        static void TableSelection()
        {
            TableBuilder tb = new TableBuilder();
            List<string> options = new List<string>() { "Vadītāji", "Transporti", "Maršruti", "Atpakaļ"};
            Console.WriteLine(tb.BuildSelector("Informācijas izvēlne", options));
            int input = Input(options.Count);
            Console.Clear();
            switch (input)
            {
                case 1:
                    PrintInformation(drivers);
                    break;
                case 4:
                    MainMenu();
                    break;
                default:
                    throw new NotImplementedException();
                /*
                case 2:
                    PrintInformation(routes);
                    break;
                case 3:
                    PrintInformation(transport);
                    break;
                */
            }
        }
        static void PrintInformation(string title, List<List<dynamic>> info)
        {
            TableBuilder tb = new TableBuilder();
            tb.BuildTable("Vadītāji", info);
        }
        static void PrintInformation(List<Information> info, [CallerMemberName] string memberName = "")
        {
            TableBuilder tb = new TableBuilder();
            Console.WriteLine(tb.BuildTable(info[0].Title, ObjectToDynamic(info)));
            int input = Input();
            Console.Clear();
            switch (input)
            {
                case 1:
                    MainMenu();
                    break;
                case 2:
                    Redirect(memberName);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        static List<List<dynamic>> ObjectToDynamic(List<Information> objectTable)
        {
            List<List<dynamic>> dynamicTable = new List<List<dynamic>>();
            dynamicTable.Add(objectTable[0].ColumnHeaders.Cast<dynamic>().ToList());
            foreach (Information obj in objectTable)
            {
                dynamicTable.Add(obj.GetRow());
            }
            return dynamicTable;
        }
        static int Input(int optionCount = DEFAULT_OPTION_COUNT)
        {
            int input = 0;
            do
            {
                try
                {
                    Console.Write(">>");
                    input = Convert.ToInt32(Console.ReadLine());
                    if (input > optionCount)
                    {
                        throw new FormatException();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Kļūda! Nepareiza ievade - mēģiniet vēlreiz");
                }
            }
            while (input == 0);
            return input;
        }
        static void Redirect(string methodName)
        {
            switch (methodName)
            {
                case "MainMenu":
                    Console.Clear();
                    MainMenu();
                    break;
                case "TableSelection":
                    Console.Clear();
                    TableSelection();
                    break;
                default:
                    Console.WriteLine(methodName);
                    break;
            }
        }
    }
}

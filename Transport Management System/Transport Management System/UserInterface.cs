using System;
using System.Collections.Generic;
using System.Linq; // .Cast<dynamic>(), .ToList()
using System.Runtime.CompilerServices;

namespace Transport_Management_System
{
    class UserInterface : Program
    {
        private const int DEFAULT_OPTION_COUNT = 2;
        private static readonly List<string> DEFAULT_OPTIONS = new List<string>() { "Pievienot ierakstu", "Dzēst ierakstu", "Rediģēt ierakstu", "Kārtot pēc...", "Atpakaļ", "Galvenā lapa" };
        private static Type viewingType;
        private static DBConnection db;
        public static void MainMenu()
        {
            TableBuilder tb = new TableBuilder();
            List<string> options = new List<string>() { "Darbības ar informāciju", "Saglabāt informāciju datubāzē", "Opcija trīs" };
            Console.WriteLine(tb.BuildSelector("Sākuma izvēlne", options));

            int input = Input(optionCount: options.Count);
            
            Console.Clear();
            switch (input)
            {
                case 1:
                    TableSelection();
                    break;
                case 2:
                    db.ReplaceAll("driver", ObjectToDynamic(drivers, false));
                    MainMenu();
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
            int input = Input(optionCount: options.Count);
            Console.Clear();
            switch (input)
            {
                case 1:
                    viewingType = typeof(Driver);
                    PrintInformation(drivers, DEFAULT_OPTIONS);
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
        static void PrintInformation(List<Information> info, List<string> options = null, [CallerMemberName] string memberName = "")
        {
            TableBuilder tb = new TableBuilder();
            Console.WriteLine(tb.BuildTable(info[0].Title, ObjectToDynamic(info), options));
            int input = Input(optionCount: DEFAULT_OPTIONS.Count);
            switch (input)
            {
                case 1:
                    Console.Clear();
                    Information newObject = (Information)Activator.CreateInstance(viewingType);
                    info.Add(newObject);
                    EditObject(newObject);
                    PrintInformation(info, options, memberName);
                    break;
                case 2:
                    Console.WriteLine("Ievadiet kārtas numuru rindai, kuru vēlaties dzēst:");
                    input = Input(false);
                    Console.Clear();
                    info.RemoveAt(input - 1);
                    PrintInformation(info, options, memberName);
                    break;
                case 3:
                    Console.WriteLine("Ievadiet kārtas numuru rindai, kuru vēlaties rediģēt:");
                    input = Input(false);
                    Console.Clear();
                    EditObject(info[input - 1]);
                    PrintInformation(info, options, memberName);
                    break;
                case 4:
                    throw new NotImplementedException();
                case 5:
                    MainMenu();
                    break;
                case 6:
                    Redirect(memberName);
                    break;
                default:
                    Console.WriteLine("Nepareiza ievade!");
                    break;
            }
        }
        static void EditObject(Information editObject, [CallerMemberName] string memberName = "")
        {
            TableBuilder tb = new TableBuilder();
            bool editing = true;
            int count = editObject.ColumnHeaders.Count;
            dynamic[] values = editObject.GetRow().ToArray();
            do
            {
                Console.Clear();
                Console.WriteLine(tb.BuildTable(editObject.Title + " - Rediģēšana", ObjectToDynamic(editObject)));
                Console.WriteLine("Ievadiet kolonnas kārtas numuru, lai rediģētu tā vērtību");
                Console.WriteLine($"[ {count + 1} ] Apstiprināt");
                Console.WriteLine($"[ {count + 2} ] Atpakaļ");
                int input = Input(false);
                if (input == count + 1)
                {
                    editing = false;
                }
                else if (input == count + 2)
                {
                    Console.WriteLine("Vai esiet pārliecināts ka vēlaties atgriezties? [Y/N]");
                    string answer = Console.ReadLine();
                    if (answer.ToLower() == "y")
                    {
                        Redirect(memberName);
                        return;
                    }
                    else continue;
                }
                else if (input <= count)
                {
                    Console.WriteLine($"Ievadiet kolonnas '{editObject.ColumnHeaders[input - 1]}' jauno vērtību:");
                    values[input - 1] = Console.ReadLine();
                    editObject.SetValues(values);
                    continue;
                }
            } while (editing);
            Console.Clear();
        }
        static List<List<dynamic>> ObjectToDynamic(List<Information> objectTable, bool includeHeaders = true)
        {
            List<List<dynamic>> dynamicTable = new List<List<dynamic>>();
            dynamicTable.Add(objectTable[0].ColumnHeaders.Cast<dynamic>().ToList());
            foreach (Information obj in objectTable)
            {
                dynamicTable.Add(obj.GetRow());
            }
            return dynamicTable;
        }
        static List<List<dynamic>> ObjectToDynamic(Information obj)
        {
            List<List<dynamic>> dynamicTable = new List<List<dynamic>>();
            dynamicTable.Add(obj.ColumnHeaders.Cast<dynamic>().ToList());
            dynamicTable.Add(obj.GetRow());
            return dynamicTable;
        }
        static List<dynamic> StringToDynamic(List<string> stringList)
        {
            return stringList.Cast<dynamic>().ToList();
        }
        static int Input(bool optionCountDependant = true, int optionCount = DEFAULT_OPTION_COUNT)
        {
            int input = 0;
            do
            {
                try
                {
                    Console.Write(">>");
                    input = Convert.ToInt32(Console.ReadLine());
                    if (input > optionCount && optionCountDependant)
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
                    Console.Clear();
                    MainMenu();
                    break;
            }
        }
        public static DBConnection DB
        {
            set { db = value; }
        }
    }
}

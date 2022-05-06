using System;
using System.Collections.Generic;
using System.Linq; // .Cast<dynamic>(), .ToList()
using System.Runtime.CompilerServices;

namespace Transport_Management_System
{
    class UserInterface : Program
    {
        // Defines the default options and their count
        private const int DEFAULT_OPTION_COUNT = 6;
        private static readonly List<string> DEFAULT_OPTIONS = new List<string>() { "Pievienot ierakstu", "Dzēst ierakstu", "Rediģēt ierakstu", "Kārtot pēc...", "Atpakaļ", "Galvenā lapa" };
        
        // Holds the current viewing type so the program knows which class to create an object for
        private static Type viewingType;

        // Holds the database connection variable
        private static DBConnection db;

        // Prints the main menu of the application
        public static void MainMenu()
        {
            TableBuilder tb = new TableBuilder();
            List<string> options = new List<string>() { "Darbības ar informāciju", "Saglabāt informāciju datubāzē", "Iziet" };
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
                    db.ReplaceAll("route", ObjectToDynamic(routes, false, true));
                    db.ReplaceAll("transport", ObjectToDynamic(transport, false));
                    MainMenu();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        // Selection table so that the user can choose which table to output
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
                case 2:
                    viewingType = typeof(Transport);
                    PrintInformation(transport, DEFAULT_OPTIONS);
                    break;
                case 3:
                    viewingType = typeof(Route);
                    List<string> route_options = new List<string>();
                    route_options.Add("Skatīt sīkāk...");
                    route_options.AddRange(DEFAULT_OPTIONS);
                    PrintInformation(routes, route_options);
                    break;
                case 4:
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Nepareiza ievade!");
                    TableSelection();
                    break;
            }
        }

        // Asks the user input and compares them to the default options
        static void AskInput(List<Information> info, List<string> options = null, [CallerMemberName] string memberName = "", List<List<dynamic>> sortedList = null)
        {
            TableBuilder tb = new TableBuilder();
            int input = Input(optionCount: DEFAULT_OPTIONS.Count);
            int defaultOptionPosition = options.Count - DEFAULT_OPTIONS.Count + 1;
            switch (input)
            {
                // "var value when value" izmantots, lai ievietotu ne-konstantas vērtības iekš "switch"
                case var value when value == defaultOptionPosition:
                    Console.Clear();

                    // Izveido jaunu objektu ar saglabātā tipa "viewingType" tipu
                    // Objekts tiek saglabāts iekš "Information" tipa references
                    Information newObject = (Information)Activator.CreateInstance(viewingType);

                    // Reference tiek saglabāta sarakstā
                    info.Add(newObject);

                    // Izveidotā objekta vērtības tiek rediģētas "EditObject" metodē
                    EditObject(newObject, true, info);

                    // Visas informācijas izvade pēc jauna objekta izveides
                    PrintInformation(info, options, memberName);
                    break;
                case var value when value == defaultOptionPosition + 1:
                    Console.WriteLine("Ievadiet kārtas numuru rindai, kuru vēlaties dzēst:");
                    input = Input(false);
                    Console.Clear();

                    if(sortedList == null)
                        info.RemoveAt(input - 1);
                    else
                    {
                        // PROBĻEMA - Es nezinu kāpēc neiet, bet ja fix tad sorted delete = done
                        info.RemoveAt(ObjectToDynamic(info).IndexOf(sortedList[input - 1]));
                        sortedList.RemoveAt(input - 1);
                    }

                    PrintInformation(info, options, memberName);
                    break;
                case var value when value == defaultOptionPosition + 2:
                    Console.WriteLine("Ievadiet kārtas numuru rindai, kuru vēlaties rediģēt:");
                    input = Input(false);
                    Console.Clear();

                    if (sortedList == null)
                        EditObject(info[input - 1]);
                    else
                    {
                        Console.WriteLine(ObjectToDynamic(info).IndexOf(sortedList[input - 1]));
                        Console.WriteLine(input);
                        EditObject(info[ObjectToDynamic(info).IndexOf(sortedList[input - 1])]);
                    }

                    PrintInformation(info, options, memberName);
                    break;
                case var value when value == defaultOptionPosition + 3:
                    Console.WriteLine("Ievadiet kolonnas kārtas numuru, pēc kuras vēlaties kārtot datus:");
                    input = Input(false);
                    Console.Clear();
                    List<List<dynamic>> sortList = SortListByColumn(ObjectToDynamic(info, false), input - 1);
                    Console.WriteLine(tb.BuildTable(info[0].Title, sortList, options));
                    AskInput(info, options, memberName, sortList);
                    break;
                case var value when value == defaultOptionPosition + 4:
                    Redirect(memberName);
                    break;
                case var value when value == defaultOptionPosition + 5:
                    Console.Clear();
                    MainMenu();
                    break;
                default:
                    // Explicit case operation
                    if (options != DEFAULT_OPTIONS)
                    {
                        if (viewingType == typeof(Route) && input == 1)
                        {
                            Console.WriteLine("Ievadiet kārtas numuru rindai, kuru vēlaties apskatīt:");
                            input = Input(false);
                            Console.Clear();
                            Console.WriteLine(tb.BuildTable(info[0].Title, info[input - 1].GetSpecific(), DEFAULT_OPTIONS));
                            AskInput(info, DEFAULT_OPTIONS, memberName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nepareiza ievade!");
                    }
                    break;
            }
        }
        // Outputs all table information in a table format
        static void PrintInformation(List<Information> info, List<string> options = null, [CallerMemberName] string memberName = "")
        {
            TableBuilder tb = new TableBuilder();
            Console.WriteLine(tb.BuildTable(info[0].Title, ObjectToDynamic(info), options));
            AskInput(info, options, memberName);
        }
        // Edits a single object of class Information, is used in creating new objects as well
        static void EditObject(Information editObject, bool isNewObject = false, List<Information> info = null, [CallerMemberName] string memberName = "")
        {
            TableBuilder tb = new TableBuilder();
            bool editing = true;
            int count = editObject.ColumnHeaders.Count;
            dynamic[] values;
            if (editObject is Route)
            {
                values = editObject.GetValues().ToArray();
            } 
            else
            {
                values = editObject.GetRow().ToArray();
            }
            // Saves information in case the user cancels all changes
            dynamic[] initialValues = new dynamic[values.Length];
            Array.Copy(values, initialValues, values.Length);
            do
            {
                Console.Clear();
                if (editObject is Route)
                {
                    Console.WriteLine(tb.BuildTable(editObject.Title + " - Rediģēšana", ObjectToSpecificDynamic(editObject)));
                }
                else
                {
                    Console.WriteLine(tb.BuildTable(editObject.Title + " - Rediģēšana", ObjectToDynamic(editObject)));
                }
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
                        if (isNewObject)
                        {
                            info.Remove(editObject);
                            Redirect(memberName);
                            return;
                        }
                        editObject.SetValues(initialValues);
                        Redirect(memberName);
                        return;
                    }
                    else continue;
                }
                else if (input <= count)
                {
                    Console.WriteLine($"Ievadiet kolonnas '{editObject.ColumnHeaders[input - 1]}' jauno vērtību:");
                    // Expected a Date entry in DateTime column
                    if (viewingType == typeof(Driver) && input == 4)
                    {
                        try
                        {
                            Console.WriteLine($"Datums tiek ievadīts formātā: yyyy-MM-dd!");
                            values[input - 1] = Convert.ToDateTime(Console.ReadLine());
                            editObject.SetValues(values);
                        }
                        catch (FormatException) { }
                        continue;
                    }
                    values[input - 1] = Console.ReadLine();
                    editObject.SetValues(values);
                    continue;
                }
            } while (editing);
            Console.Clear();
        }

        // Sorts the tables components by ascending or by alphabetical order
        // Must specify the column index to sort by
        static List<List<dynamic>> SortListByColumn(List<List<dynamic>> sortList, int columnIndex)
        {
            int numberOfChanges = 0;
            for(int i = 1; i < sortList.Count() - 1; i++)
            {
                if (sortList[i][columnIndex] is string)
                {
                    if (string.Compare(sortList[i][columnIndex], sortList[i + 1][columnIndex]) < 0
                        || string.Compare(sortList[i][columnIndex], sortList[i + 1][columnIndex]) == 0)
                    {
                        continue;
                    }
                }
                else
                {
                    if (sortList[i][columnIndex] >= sortList[i + 1][columnIndex])
                    {
                        continue;
                    }
                }
                List<dynamic> temp = sortList[i];
                sortList[i] = sortList[i + 1];
                sortList[i + 1] = temp;
                numberOfChanges++;
            }
            if(numberOfChanges != 0)
            {
                SortListByColumn(sortList, columnIndex);
            }
            return sortList;
        }

        // Creates a dynamic two-dimensional list from list of Information class objects to be used elsewhere
        static List<List<dynamic>> ObjectToDynamic(List<Information> objectTable, bool includeHeaders = true, bool isSpecific = false)
        {
            List<List<dynamic>> dynamicTable = new List<List<dynamic>>();
            if(includeHeaders)
            {
                dynamicTable.Add(objectTable[0].ColumnHeaders.Cast<dynamic>().ToList());
            }
            foreach (Information obj in objectTable)
            {
                if (isSpecific)
                {
                    dynamicTable.Add(obj.GetValues());
                }
                else
                {
                    dynamicTable.Add(obj.GetRow());
                }
            }
            return dynamicTable;
        }

        // Creates a dynamic two-dimensional list with one row from an Information class object to be used elsewhere
        static List<List<dynamic>> ObjectToDynamic(Information obj)
        {
            List<List<dynamic>> dynamicTable = new List<List<dynamic>>();
            dynamicTable.Add(obj.ColumnHeaders.Cast<dynamic>().ToList());
            dynamicTable.Add(obj.GetRow());
            return dynamicTable;
        }

        // Creates a dynamic two-dimensional list specifically for Route class, used to output specific information of a single Route object
        static List<List<dynamic>> ObjectToSpecificDynamic(Information obj)
        {
            List<List<dynamic>> dynamicTable = new List<List<dynamic>>();
            dynamicTable.Add(obj.ColumnHeaders.Cast<dynamic>().ToList());
            dynamicTable.AddRange(obj.GetSpecific());
            return dynamicTable;
        }

        // Asks the user for input and checks if input is valid
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

        // Redirects user to the previous window/method
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

        // Properties
        public static DBConnection DB
        {
            set { db = value; }
        }
    }
}

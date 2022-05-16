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
        private static readonly List<string> DEFAULT_OPTIONS = new List<string>() { "Pievienot ierakstu", "Dzēst ierakstu", "Rediģēt ierakstu", "Kārtot datus", "Meklēt ierakstus", "Atpakaļ", "Galvenā lapa" };
        
        // Holds the current viewing type so the program knows which class to create an object for
        private static Type viewingType;

        // Holds the database connection variable
        private static DBConnection db;

        // Prints the main menu of the application
        public static void MainMenu()
        {
            TableBuilder tb = new TableBuilder();
            List<string> options = new List<string>() { "Darbības ar informāciju", "Informācijas apkopojums", "Saglabāt informāciju datubāzē", "Iziet" };
            Console.WriteLine(tb.BuildSelector("Sākuma izvēlne", options));

            int input = Input(optionCount: options.Count);
            
            Console.Clear();
            switch (input)
            {
                case 1:
                    TableSelection();
                    break;
                case 2:
                    Statistics();
                    break;
                case 3:
                    db.ReplaceAll("driver", ObjectToDynamic(drivers, false));
                    db.ReplaceAll("route", ObjectToDynamic(routes, false, true));
                    db.ReplaceAll("transport", ObjectToDynamic(transport, false));
                    MainMenu();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        static void Statistics()
        {
            // Driver statistics
            Console.WriteLine("Vadītāju statistika: ");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"Kopējais vadītāju skaits: {drivers.Count}\n");
            Console.WriteLine($"Vadītāji ar specifikācijām:");
            int tram = 0, bus = 0, trolleybus = 0, minibus = 0;
            foreach(Information driver in drivers)
            {
                if (driver.Specialities.Contains("tramvajs")) tram++;
                if (driver.Specialities.Contains("autobuss")) bus++;
                if (driver.Specialities.Contains("trollejbuss")) trolleybus++;
                if (driver.Specialities.Contains("mikroautobuss")) minibus++;
            }
            Console.WriteLine($"\t- Tramvaju vada: {tram} vadītāji");
            Console.WriteLine($"\t- Autobusu vada: {bus} vadītāji");
            Console.WriteLine($"\t- Trollejbusu vada: {trolleybus} vadītāji");
            Console.WriteLine($"\t- Mikroautobusu vada: {minibus} vadītāji");
            Console.WriteLine("--------------------------------------\n\n");

            // Transport statistics
            Console.WriteLine("Transportu statistika: ");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"Kopējais transportu skaits: {transport.Count}\n");
            Console.WriteLine($"Transportu veidi:");

            tram = 0; bus = 0; trolleybus = 0; minibus = 0;
            foreach (Information single in transport)
            {
                if (single.TransportType.Contains("tramvajs")) tram++;
                if (single.TransportType.Contains("autobuss")) bus++;
                if (single.TransportType.Contains("trollejbuss")) trolleybus++;
                if (single.TransportType.Contains("mikroautobuss")) minibus++;
            }
            Console.WriteLine($"\t- Tramvaji: {tram}");
            Console.WriteLine($"\t- Autobusi: {bus}");
            Console.WriteLine($"\t- Trollejbusi: {trolleybus}");
            Console.WriteLine($"\t- Mikroautobusi: {minibus}\n");

            Console.WriteLine($"Transportu stāvokļi:");

            int ready = 0, broken = 0, inService = 0;
            foreach (Information single in transport)
            {
                if (single.TransportCondition.Contains("gatavs")) ready++;
                if (single.TransportCondition.Contains("salauzts")) broken++;
                if (single.TransportCondition.Contains("apkopē")) inService++;
            }
            Console.WriteLine($"\t- Darbības gatavībā: {ready}");
            Console.WriteLine($"\t- Bojāti: {broken}");
            Console.WriteLine($"\t- Apkopē: {inService}");
            Console.WriteLine("--------------------------------------\n\n");

            // Route statistics
            Console.WriteLine("Maršrutu statistika: ");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"Kopējais Maršrutu skaits: {routes.Count}\n");
            Console.WriteLine($"Maršrutu transportu tipi:");
            tram = 0; bus = 0; trolleybus = 0; minibus = 0;
            foreach (Information route in routes)
            {
                if (route.TransportType.Contains("tramvajs")) tram++;
                if (route.TransportType.Contains("autobuss")) bus++;
                if (route.TransportType.Contains("trollejbuss")) trolleybus++;
                if (route.TransportType.Contains("mikroautobuss")) minibus++;
            }
            Console.WriteLine($"\t- Tramvaju maršrutu līnijas: {tram}");
            Console.WriteLine($"\t- Autobusu maršrutu līnijas: {bus}");
            Console.WriteLine($"\t- Trollejbusu maršrutu līnijas: {trolleybus}");
            Console.WriteLine($"\t- Mikroautobusu maršrutu līnijas: {minibus}");
            Console.WriteLine("--------------------------------------\n\n");

            Console.WriteLine("Spiediet 'Enter' lai atgrieztos sākuma lapā!");
            Console.ReadLine();
            Console.Clear();
            MainMenu();
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
        static void AskInput(List<Information> info, List<string> options = null, [CallerMemberName] string memberName = "")
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
                    Console.WriteLine("Ievadiet identifikatoru rindai, kuru vēlaties dzēst:");
                    input = Input(false);
                    Console.Clear();

                    bool isDeleted = false;
                    foreach (Information obj in info.ToList())
                    {
                        if (obj.ID == input)
                        {
                            info.Remove(obj);
                            isDeleted = true;
                        }
                    }
                    PrintInformation(info, options, memberName);
                    if (!isDeleted)
                    {
                        Console.WriteLine($"Nevarēja atrast ierakstu ar identifikatoru {input}!");
                    }
                    break;
                case var value when value == defaultOptionPosition + 2:
                    Console.WriteLine("Ievadiet identifikatoru rindai, kuru vēlaties rediģēt:");
                    input = Input(false);
                    Console.Clear();

                    foreach (Information obj in info.ToList())
                    {
                        if (obj.ID == input)
                        {
                            EditObject(obj);
                            break;
                        }
                    }

                    PrintInformation(info, options, memberName);
                    break;
                case var value when value == defaultOptionPosition + 3:
                    Sort(info);
                    break;
                case var value when value == defaultOptionPosition + 4:
                    Search(info);
                    break;
                case var value when value == defaultOptionPosition + 5:
                    Redirect(memberName);
                    break;
                case var value when value == defaultOptionPosition + 6:
                    Console.Clear();
                    MainMenu();
                    break;
                default:
                    // Explicit case operation
                    if (options != DEFAULT_OPTIONS)
                    {
                        // Detailed data output of Route object
                        if (viewingType == typeof(Route) && input == 1)
                        {
                            Console.WriteLine("Ievadiet identifikatoru rindai, kuru vēlaties apskatīt:");

                            input = Input(false);
                            Console.Clear();

                            List<List<dynamic>> specificInfo = new List<List<dynamic>>();
                            specificInfo.Add(info[0].ColumnHeaders.Cast<dynamic>().ToList());

                            foreach (Information obj in info.ToList())
                            {
                                if (obj.ID == input)
                                {
                                    specificInfo.AddRange(obj.GetSpecific());
                                    break;
                                }
                            }

                            Console.WriteLine(tb.BuildTable(info[0].Title, specificInfo, DEFAULT_OPTIONS));
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

        // Ascending and descending data sorting by choice
        static void Sort(List<Information> info)
        {
            Console.Clear();

            List<string> options = new List<string>() { "Kārtot pēc kolonnas...", "Atgriezties" };
            TableBuilder tb = new TableBuilder();
            Console.WriteLine(tb.BuildTable($"{info[0].Title} - meklēšana", ObjectToDynamic(info), options));
            int input = Input(true, 2);
            if (input == 2)
            {
                Console.Clear();
                MainMenu();
            }
            else if (input == 1)
            {
                Console.WriteLine("Ievadiet kolonnas kārtas skaitli, pēc kuras vēlaties kārtot informāciju!");
                int columnIndex = Input(false) - 1;

                Console.WriteLine("Kārtot datus: ");
                Console.WriteLine("[ 1 ] Alfabētiskā/augošā secībā");
                Console.WriteLine("[ 2 ] Dilstošā secībā\n");
                input = Input(true, 2);
                bool isAlphabetical;
                if (input == 1) isAlphabetical = true;
                else isAlphabetical = false;

                List<List<dynamic>> sortList = ObjectToDynamic(info, false);
                List<Information> sortInfo = new List<Information>();
                sortInfo.AddRange(info);

                int numberOfChanges = 1;
                while(numberOfChanges != 0)
                {
                    numberOfChanges = 0;
                    for (int i = 0; i < info.Count() - 1; i++)
                    {
                        dynamic thisValue = sortList[i][columnIndex];
                        dynamic nextValue = sortList[i + 1][columnIndex];
                        if (thisValue is string)
                        {
                            if (isAlphabetical)
                            {
                                if (string.Compare(thisValue, nextValue) <= 0)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (string.Compare(thisValue, nextValue) >= 0)
                                {
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            if (isAlphabetical)
                            {
                                if (thisValue >= nextValue)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (thisValue <= nextValue)
                                {
                                    continue;
                                }
                            }
                        }

                        Information tempInfo = sortInfo[i];
                        sortInfo[i] = sortInfo[i + 1];
                        sortInfo[i + 1] = tempInfo;

                        List<dynamic> temp = sortList[i];
                        sortList[i] = sortList[i + 1];
                        sortList[i + 1] = temp;

                        numberOfChanges++;
                    }
                }
                Sort(sortInfo);
            }
        }

        // Lets a user search through information
        static void Search(List<Information> info, List<List<dynamic>> searchedInfo = null)
        {
            Console.Clear();
            List<string> options = new List<string>() {"Meklēt pēc kolonnas...", "Atgriezties"};
            TableBuilder tb = new TableBuilder();
            if (searchedInfo == null)
            {
                Console.WriteLine(tb.BuildTable($"{info[0].Title} - meklēšana", ObjectToDynamic(info), options));
            }
            else
            {
                List<List<dynamic>> searchedColumnInfo = new List<List<dynamic>>();
                searchedColumnInfo.Add(info[0].ColumnHeaders.Cast<dynamic>().ToList());
                searchedColumnInfo.AddRange(searchedInfo);
                Console.WriteLine(tb.BuildTable($"{info[0].Title} - meklēšana", searchedColumnInfo, options));
            }
            int input = Input(true, 2);
            if (input == 2)
            {
                Console.Clear();
                MainMenu();
            }
            else if (input == 1)
            {
                List<string> searchValues = new List<string>();
                int columnNumber;
                while (true)
                {
                    Console.WriteLine("Ievadiet kolonnas kārtas numuru pēc kuras jūs vēlaties meklēt datus!");
                    columnNumber = Input(false);
                    if (columnNumber < 0 && columnNumber > info[0].ColumnHeaders.Count())
                    {
                        Console.WriteLine("Nepareiza ievade!");
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                while (true)
                {
                    Console.WriteLine("Meklējamās vērtības:");
                    foreach (string searchValue in searchValues)
                    {
                        Console.WriteLine($"> {searchValue}");
                    }
                    Console.WriteLine();
                    Console.WriteLine("Ievadiet vērtības, kuras vēlaties atrast kolonnā!");
                    Console.WriteLine("Lai apstiprinātu izvēli, spiediet 'Enter' pogu!");
                    string value = Console.ReadLine();
                    if (value == "")
                    {
                        break;
                    }
                    else
                    {
                        searchValues.Add(value);
                    }
                }
                // Removes column headers
                List<List<dynamic>> allInfo = ObjectToDynamic(info, false); 
                searchedInfo = new List<List<dynamic>>();
                foreach (List<dynamic> row in allInfo)
                {
                    foreach(string searchValue in searchValues)
                    {
                        if (row[columnNumber - 1].ToString().ToLower().Contains(searchValue))
                        {
                            searchedInfo.Add(row);
                        }
                    }
                }
                Search(info, searchedInfo);
            }
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
                    if (viewingType == typeof(Driver) && input == 5)
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

        // Creates a dynamic two-dimensional list from list of Information class objects to be used elsewhere
        static List<List<dynamic>> ObjectToDynamic(List<Information> objectTable, bool includeHeaders = true, bool isSpecific = false)
        {
            List<List<dynamic>> dynamicTable = new List<List<dynamic>>();
            if(includeHeaders)
            {
                if(viewingType == typeof(Route) && !isSpecific)
                {
                    dynamicTable.Add(objectTable[0].IDColumnHeaders.Cast<dynamic>().ToList());
                }
                else
                {
                    dynamicTable.Add(objectTable[0].ColumnHeaders.Cast<dynamic>().ToList());
                }
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
                    input = 0;
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

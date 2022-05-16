using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Transport_Management_System
{
    class DBConnection
    {
        private MySqlConnection conn;
        private string cs = @"server=localhost;port=3306;userid=root;password=;database=satiksmes_vadiba";
        public DBConnection()
        {
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Could not connect to database.");
                Console.WriteLine("Error message: " + e.Message);
            }
        }

        public List<List<dynamic>> Select(string table, List<string> columns = null, List<string> options = null)
        {
            // If no columns specified
            if (columns == null)
            {
                columns = new List<string>() { "*" };
            }

            // If no options specified
            if (options == null)
            {
                options = new List<string>() { "1" };
            }

            MySqlCommand cmd = new MySqlCommand($"SELECT {String.Join(", ", columns)} FROM {table} WHERE {String.Join(", ", options)}", conn);
            MySqlDataReader read = cmd.ExecuteReader();

            List<List<dynamic>> data = new List<List<dynamic>>();
            dynamic[] currentRow = new dynamic[read.FieldCount];

            while (read.Read())
            {
                read.GetValues(currentRow);
                data.Add(new List<dynamic>(currentRow));
            }

            read.Close();
            return data;
        }

        // If there are no options called
        public List<List<dynamic>> Select(string table, List<string> columns)
        {
            List<string> options = new List<string>() {"1"};
            return Select(table, columns, options);
        }

        // Select method if there are no columns or options called called
        public List<List<dynamic>> Select(string table)
        {
            List<string> columns = new List<string>() { "*" };
            List<string> options = new List<string>() { "1" };
            return Select(table, columns, options);
        }

        public MySqlDataReader Insert(string table, List<List<dynamic>> data)
        {
            string query = $"INSERT INTO {table} VALUES ";
            List<string> subquery = new List<string>();
            List<string> subqueryGroup = new List<string>();
            bool hasID = true;
            try
            {
                Convert.ToInt32(data[0][0]);
            }
            catch { hasID = false; }
            if (!hasID) subquery.Add("null");
            foreach (List<dynamic> row in data)
            {
                foreach (dynamic cell in row)
                {
                    subquery.Add($"\"{cell}\"");
                }
                subqueryGroup.Add($"({String.Join(",", subquery)})");
                subquery = new List<string>();
                if (!hasID) subquery.Add("null");
            }
            query += String.Join(",", subqueryGroup);
            System.Diagnostics.Debug.WriteLine(query);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader read = cmd.ExecuteReader();
            read.Close();
            return read;
        }

        public MySqlDataReader ReplaceAll(string table, List<List<dynamic>> data)
        {
            MySqlCommand cmd = new MySqlCommand($"DELETE FROM {table} WHERE 1", conn);
            MySqlDataReader read = cmd.ExecuteReader();
            read.Close();
            return Insert(table, data);
        }

        ~DBConnection() {
            conn.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Transport_Management_System
{
    class DBConnection
    {
        private MySqlConnection conn;
        private string cs = @"server=localhost;userid=root;password=;database=satiksmes_vadiba";
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

        public MySqlDataReader Insert(string table, dynamic[] data)
        {
            MySqlCommand cmd = new MySqlCommand($"INSERT INTO {table} VALUES (\"{String.Join("\", \"", data)}\");", conn);
            MySqlDataReader read = cmd.ExecuteReader();
            read.Close();
            return read;
        }

        public MySqlDataReader Update(string table, dynamic[,] data, dynamic[,] where)
        {
            dynamic updateData = "";
            dynamic whereData = "";

            for (int i = 0; i < data.Length - 1; i++)
            {
                dynamic tempData = String.Join("=", new dynamic[] { data[i, 0], "\"" + data[i, 1] + "\"" });
                updateData += tempData + ",";
            }

            for (int i = 0; i < where.Length - 1; i++)
            {
                dynamic tempData = String.Join("=", new dynamic[] { where[i, 0], where[i, 1] });
                Console.WriteLine(tempData);
                whereData += tempData + ",";
            }
            updateData = updateData.Remove(updateData.Length - 1, 1);
            whereData = whereData.Remove(whereData.Length - 1, 1);
            MySqlCommand cmd = new MySqlCommand($"UPDATE {table} SET {updateData} WHERE {whereData};", conn);
            MySqlDataReader read = cmd.ExecuteReader();
            read.Close();
            return read;
        }

        public MySqlDataReader Delete(string table, dynamic[,] where)
        {
            dynamic whereData = "";

            for (int i = 0; i < where.Length - 1; i++)
            {
                dynamic tempData = String.Join("=", new dynamic[] { where[i, 0], where[i, 1] });
                Console.WriteLine(tempData);
                whereData += tempData + ",";
            }

            whereData = whereData.Remove(whereData.Length - 1, 1);
            MySqlCommand cmd = new MySqlCommand($"DELETE FROM {table} WHERE {whereData};", conn);
            MySqlDataReader read = cmd.ExecuteReader();
            read.Close();

            return read;
        }

        ~DBConnection() {
            conn.Close();
        }
    }
}

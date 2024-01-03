using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
namespace CsvConvertor
{
    internal class FileCreater
    {

        public void ConvertToCsv()
        {
            var obj = new SqlToCsvConvertor();

            using (var connection = new SqlConnection(obj._connectionString))
            {
                string error = "";
                string filename = "xyz.csv";

                try
                {
                    connection.Open();


                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message + " There might be an error in the Connection String.");


                }

                string queryForLog = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tb_log')" +

                    "create table tb_log (Id int primary key identity(1,1),TableName varchar(20),Time DateTime, ErrorMessage nvarchar(max));";


                using (var Logcommand = new SqlCommand(queryForLog, connection))
                {

                    try
                    {
                        Logcommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {

                        Console.WriteLine($"\nAn error has occured: {ex.Message}");
                        error = ex.Message;
                    }


                }

                string query = $"SELECT {string.Join(",", obj._columns)} FROM {obj._tableName} WHERE {obj._dateColumn} BETWEEN '{obj._startDate}' AND '{obj._endDate}'";

                try
                {
                    using (var command = new SqlCommand(query, connection))

                    using (var adapter = new SqlDataAdapter(command))
                    {

                        DataTable dataTable = new DataTable();

                        adapter.Fill(dataTable); bool appendToFile = File.Exists(obj._outputPath + filename);

                        using (var writer = new StreamWriter(obj._outputPath + filename, appendToFile))
                        {
                            if (!appendToFile)
                            {

                                writer.WriteLine(string.Join(",", obj._columns));
                            }
                            foreach (DataRow row in dataTable.Rows)
                            {
                                List<string> rowValues = new List<string>();

                                foreach (string column in obj._columns)
                                {
                                    rowValues.Add(row[column].ToString());
                                }
                                writer.WriteLine(string.Join(",", rowValues));
                            }
                        }

                    }
                }

                catch (SqlException ex)
                {

                    Console.WriteLine($"SQl error: {ex.Message}");

                    error = ex.Message + " You can check if table name and column names are correctly typed.";
                }


                if (error != "")
                {
                    try
                    {

                        string InsertInLog = "INSERT INTO tb_Log(TableName,Time,ErrorMessage) VALUES(@TableName,@Time,@Error) ";


                        using (var InsertCommand = new SqlCommand(InsertInLog, connection))
                        {

                            InsertCommand.Parameters.AddWithValue("@TableName", obj._tableName);

                            InsertCommand.Parameters.AddWithValue("@Time", DateTime.Now);

                            InsertCommand.Parameters.AddWithValue("@Error", error);

                            InsertCommand.ExecuteNonQuery();


                        }

                    }
                    catch (Exception ex)
                    {

                        error = ex.Message;
                        Console.WriteLine("An error occured: " + ex.Message);
                    }

                    try
                    {
                        // Calling Email sender function
                        SendExceptionEmail sendExceptionEmail = new SendExceptionEmail();

                        sendExceptionEmail.SendEmail("There is an Error in your ConsoleApp", error);

                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("An error occured:" + ex.Message);
                    }
                }
                


                Console.WriteLine("\n"+obj._tableName + " converted to CSV successfully at " + DateTime.Now);
            }
        }
    }


}




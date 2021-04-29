using Microsoft.Xna.Framework.Content;
using QuasarConvoy.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace QuasarConvoy
{
    public class DBManager
    {
        private SqlConnection connection;

        public DBManager()
        {

            /*string fileName = "QCdatabase.mdf";
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            path = Path.Combine(path, fileName);*/
            //string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = " + path + "; Integrated Security = True"; 
            string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename =" + Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\QCdatabase.mdf; Integrated Security = True";
            connection = new SqlConnection(connectionString); //daca scriu asa merge 
        }

        private bool OpenConnection()
        {
            StringBuilder errorMessages = new StringBuilder();
            try
            {
                connection.Open();
                return true;
            }catch(SqlException e)
            {
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + e.Errors[i].Message + "\n" +
                        "LineNumber: " + e.Errors[i].LineNumber + "\n" +
                        "Source: " + e.Errors[i].Source + "\n" +
                        "Procedure: " + e.Errors[i].Procedure + "\n");
                }
                //Console.WriteLine(errorMessages.ToString());
            }
            return false;
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (SqlException e){}
            return false;
        }

        public void QueryIUD(string query) //insert, update, delete
        {
            if(OpenConnection() == true)
            {
                SqlCommand _command = new SqlCommand(query, connection);
                _command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public string SelectElement(string query) //select only one element
        {
            if(OpenConnection() == true)
            {
                SqlCommand _command = new SqlCommand(query, connection);
                string element = _command.ExecuteScalar() + "";
                CloseConnection();
                return element;
            }
            return "";
        }

        public List<string> SelectColumnFrom(string table, string column) //select one column in a list of string arrays
        {
            string query = "SELECT * FROM " + table;
            List<string> data = new List<string>();

            if (OpenConnection() == true)
            {
                SqlCommand _command = new SqlCommand(query, connection);
                SqlDataReader dataReader = _command.ExecuteReader();
                while (dataReader.Read())
                    data.Add(dataReader[column] + "");

                dataReader.Close();
                CloseConnection();
                return data;
            }
            else return null;
        }
    }
}

using Microsoft.Xna.Framework.Content;
using QuasarConvoy.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace QuasarConvoy
{
    public class DBManager
    {
        private SqlConnection connection;

        public DBManager()
        {
            string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = \"D:\\cleopatra\\Atestat\\Quasar Convoy\\QuasarConvoy\\QCdatabase.mdf\"; Integrated Security = True";
            connection = new SqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }catch(SqlException e){}
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

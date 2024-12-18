using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficPoliceDomainDLL.model
{
    public class DataBaseConnector
    {

        private static DataBaseConnector _instance;


        private MySqlConnection _connection;


        private readonly string _connectionString = "Server=localhost;Database=traffic_police;User ID=root;Password=password3301;";


        private DataBaseConnector()
        {
            _connection = new MySqlConnection(_connectionString);
        }


        public static DataBaseConnector Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataBaseConnector();
                }
                return _instance;
            }
        }


        public MySqlConnection GetConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    _connection.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при подключении к базе данных: {ex.Message}");
                    throw;
                }
            }
            return _connection;
        }


        public void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}

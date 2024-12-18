using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TrafficPoliceDomainDLL;
using TrafficPoliceDomainDLL.model;
using MySql.Data.MySqlClient;

namespace TrafficPoliceQueryDLL
{
    public class QueryWindowViewModel : ViewModelBase
    {
        private string _query;

        private DataTable _queryResults;
        public ICommand GetData { get; }

        public string Query
        {
            get
            {
                return _query;
            }
            set
            {
                _query = value;
                OnPropertyChanged(nameof(Query));
            }
        }

        public DataTable QueryResults
        {
            get => _queryResults;
            set
            {
                _queryResults = value;
                OnPropertyChanged(nameof(QueryResults));
            }
        }

        public QueryWindowViewModel()
        {
            _queryResults = new DataTable();
            GetData = new RelayCommand(o => GetDataFromQuery(Query));
        }

        private void GetDataFromQuery(string query)
        {
            try
            {

                MySqlConnection connection = DataBaseConnector.Instance.GetConnection();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    if (query.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            QueryResults.Clear();
                            QueryResults = dataTable;
                        }
                    }
                    else
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Затронуто строк: {rowsAffected}");
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Показываем сообщение об ошибке от SQL-сервера
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка от сервера");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}");
            }
            finally
            {
                // Закрытие соединения не требуется, если оно обрабатывается централизованно
                DataBaseConnector.Instance.CloseConnection();
            }
        }
    }
}

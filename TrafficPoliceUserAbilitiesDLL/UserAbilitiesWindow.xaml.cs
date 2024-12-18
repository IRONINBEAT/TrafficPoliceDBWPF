using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrafficPoliceDomainDLL.model;

namespace TrafficPoliceUserAbilitiesDLL
{
    /// <summary>
    /// Логика взаимодействия для UserAbilitiesWindow.xaml
    /// </summary>
    public partial class UserAbilitiesWindow : Window
    {
        private DataTable userAbilitiesTable;

        public UserAbilitiesWindow()
        {
            InitializeComponent();
            LoadUserAbilities();
        }

        /// <summary>
        /// Загружает данные из базы данных и привязывает их к UserAbilitiesDataGrid.
        /// </summary>
        private void LoadUserAbilities()
        {
            var connection = DataBaseConnector.Instance.GetConnection();

            // SQL-запрос для получения всех данных
            string query = @"
                        SELECT 
                            um.id AS UserMenuId,
                            u.username AS `Имя пользователя`,
                            mi.name AS `Элемент меню`,
                            um.R,
                            um.W,
                            um.E,
                            um.D
                        FROM 
                            user_menu um
                        INNER JOIN 
                            user u ON um.user_id = u.id
                        INNER JOIN 
                            menu_item mi ON um.menu_id = mi.id";

            try
            {
                using (var command = new MySqlCommand(query, connection))
                using (var adapter = new MySqlDataAdapter(command))
                {
                    userAbilitiesTable = new DataTable();
                    adapter.Fill(userAbilitiesTable);

                    // Привязываем данные к UserAbilitiesDataGrid
                    UserAbilitiesDataGrid.ItemsSource = userAbilitiesTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Сохраняет изменения, внесенные в DataGrid, в базу данных.
        /// </summary>
        private void SaveUserAbilities()
        {
            var connection = DataBaseConnector.Instance.GetConnection();

            // Формируем команды для обновления таблицы
            string selectQuery = @"
            SELECT 
                um.id AS UserMenuId,
                u.username AS UserName,
                mi.name AS MenuName,
                um.R,
                um.W,
                um.E,
                um.D
            FROM 
                user_menu um
            INNER JOIN 
                user u ON um.user_id = u.id
            INNER JOIN 
                menu_item mi ON um.menu_id = mi.id";

            string updateQuery = @"
            UPDATE user_menu 
            SET 
                R = @R,
                W = @W,
                E = @E,
                D = @D
            WHERE id = @UserMenuId";

            try
            {
                using (var adapter = new MySqlDataAdapter(selectQuery, connection))
                {
                    // Настройка команды обновления
                    adapter.UpdateCommand = new MySqlCommand(updateQuery, connection);
                    adapter.UpdateCommand.Parameters.Add("@R", MySqlDbType.Bit, 1, "R");
                    adapter.UpdateCommand.Parameters.Add("@W", MySqlDbType.Bit, 1, "W");
                    adapter.UpdateCommand.Parameters.Add("@E", MySqlDbType.Bit, 1, "E");
                    adapter.UpdateCommand.Parameters.Add("@D", MySqlDbType.Bit, 1, "D");
                    adapter.UpdateCommand.Parameters.Add("@UserMenuId", MySqlDbType.Int32, 11, "UserMenuId");

                    // Обновляем базу данных
                    adapter.Update(userAbilitiesTable);
                    MessageBox.Show("Изменения успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик кнопки сохранения данных.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveUserAbilities();
            LoadUserAbilities(); // Обновляем данные после сохранения
        }
    }
}

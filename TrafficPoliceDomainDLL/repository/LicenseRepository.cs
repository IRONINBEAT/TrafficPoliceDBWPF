using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficPoliceDomainDLL.model;

namespace TrafficPoliceDomainDLL.repository
{
    public class LicenseRepository
    {
        public List<LicenseCategory> GetAllLicences()
        {
            var licences = new List<LicenseCategory>();

            // Получаем соединение с базой данных
            var connection = DataBaseConnector.Instance.GetConnection();

            // SQL-запрос для получения всех записей из таблицы
            var query = "SELECT id, code, description FROM license_category";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var licence = new LicenseCategory
                        {
                            Id = reader.GetInt32("id"),
                            Code = reader.GetString("code"),
                            Description = reader.GetString("description")
                        };

                        licences.Add(licence);
                    }
                }
            }

            return licences;
        }

        public List<LicenseCategory> GetLicenceByDriverId(int driverId)
        {
            var licences = new List<LicenseCategory>();

            // Получаем соединение с базой данных
            var connection = DataBaseConnector.Instance.GetConnection();

            // SQL-запрос для получения всех записей из таблицы
            var query = $@"SELECT license_category.id AS id, 
                                license_category.code AS code, 
                                license_category.description AS description 
                                FROM license_category 
                                JOIN driver_category ON driver_category.category =  license_category.id
                                WHERE driver_category.driver = {driverId};";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var licence = new LicenseCategory
                        {
                            Id = reader.GetInt32("id"),
                            Code = reader.GetString("code"),
                            Description = reader.GetString("description")
                        };

                        licences.Add(licence);
                    }
                }
            }

            return licences;
        }

        public bool AddLicenceToDriver(int driverId, List<LicenseCategory> categories)
        {
            var isSuccess = false;

            // Получаем соединение с базой данных
            var connection = DataBaseConnector.Instance.GetConnection();

            // SQL-запрос для проверки существования записи
            var queryCheckExisting = @"
                                SELECT COUNT(*) 
                                FROM driver_category 
                                WHERE driver = @DriverId AND category = @CategoryId;
                            ";

            // SQL-запрос для вставки записи
            var queryInsert = @"
                        INSERT INTO driver_category (driver, category) 
                        VALUES (@DriverId, @CategoryId);
                    ";

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var category in categories)
                    {
                        // Проверяем, существует ли уже связь между водителем и категорией
                        using (var commandCheck = new MySqlCommand(queryCheckExisting, connection, transaction))
                        {
                            commandCheck.Parameters.AddWithValue("@DriverId", driverId);
                            commandCheck.Parameters.AddWithValue("@CategoryId", category.Id);

                            var existingCount = Convert.ToInt32(commandCheck.ExecuteScalar());
                            if (existingCount > 0)
                            {
                                // Если связь уже существует, пропускаем
                                continue;
                            }
                        }

                        // Вставляем новую запись в таблицу driver_category
                        using (var commandInsert = new MySqlCommand(queryInsert, connection, transaction))
                        {
                            commandInsert.Parameters.AddWithValue("@DriverId", driverId);
                            commandInsert.Parameters.AddWithValue("@CategoryId", category.Id);

                            commandInsert.ExecuteNonQuery();
                        }
                    }

                    // Подтверждаем транзакцию
                    transaction.Commit();
                    isSuccess = true;
                }
                catch (Exception)
                {
                    // Откатываем транзакцию в случае ошибки
                    transaction.Rollback();
                    throw;
                }
            }

            return isSuccess;
        }

        public bool RemoveLicenceFromDriver(int driverId, int categoryId)
        {
            var isSuccess = false;

            // Получаем соединение с базой данных
            var connection = DataBaseConnector.Instance.GetConnection();

            // SQL-запрос для удаления записи
            var queryDelete = @"
                        DELETE FROM driver_category 
                        WHERE driver = @DriverId AND category = @CategoryId;
                    ";

            using (var command = new MySqlCommand(queryDelete, connection))
            {
                // Устанавливаем параметры
                command.Parameters.AddWithValue("@DriverId", driverId);
                command.Parameters.AddWithValue("@CategoryId", categoryId);

                try
                {
                    // Выполняем команду
                    var rowsAffected = command.ExecuteNonQuery();

                    // Если хотя бы одна строка была удалена, считаем операцию успешной
                    isSuccess = rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    // Логируем или обрабатываем ошибку
                    Console.WriteLine($"Ошибка удаления категории у водителя: {ex.Message}");
                    throw;
                }
            }

            return isSuccess;
        }
    }
}

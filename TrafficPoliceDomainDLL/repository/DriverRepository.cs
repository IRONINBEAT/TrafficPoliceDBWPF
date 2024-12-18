using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficPoliceDomainDLL.model;

namespace TrafficPoliceDomainDLL.repository
{
    public class DriverRepository
    {
        public List<Driver> GetAllDrivers()
        {
            var drivers = new List<Driver>();

            var connection = DataBaseConnector.Instance.GetConnection();

            var query = @"SELECT id, 
                                 name, 
                                 surname,
                                 patronymic, 
                                 phone_number, 
                                 passport_series, 
                                 passport_number, 
                                 passport_date_of_release, 
                                 passport_release_organization,
                                 license_number, 
                                 license_date_of_release,
                                 CONCAT_WS(' ', surname, name, patronymic) AS driver_name
                          FROM driver";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var driver = new Driver
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Surname = reader.GetString("surname"),
                            Patronymic = reader.GetString("patronymic"),
                            PhoneNumber = reader.GetString("phone_number"),
                            PassportSeries = reader.GetString("passport_series"),
                            PassportNumber = reader.GetString("passport_number"),
                            PassportDateOfRelease = reader.GetDateTime("passport_date_of_release"),
                            PassportReleaseOrganization = reader.GetString("passport_release_organization"),
                            LicenseNumber = reader.GetString("license_number"),
                            LicenseDateOfRelease = reader.GetDateTime("license_date_of_release"),
                            DriverFullName = reader.GetString("driver_name")
                        };
                        drivers.Add(driver);
                    }

                }

            }
            return drivers;
        }

        public Driver GetDriverById(int driverId)
        {
            Driver driver = null;

            var connection = DataBaseConnector.Instance.GetConnection();

            var query = @"
                        SELECT id, 
                               name, 
                               surname,
                               patronymic, 
                               phone_number, 
                               passport_series, 
                               passport_number, 
                               passport_date_of_release, 
                               passport_release_organization,
                               license_number, 
                               license_date_of_release, 
                               CONCAT_WS(' ', surname, name, patronymic) AS driver_name
                        FROM driver
                        WHERE id = @DriverId;
                    ";

            using (var command = new MySqlCommand(query, connection))
            {
                // Передаем значение параметра @DriverId
                command.Parameters.AddWithValue("@DriverId", driverId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Заполняем объект Driver данными из результата запроса
                        driver = new Driver
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Surname = reader.GetString("surname"),
                            Patronymic = reader.GetString("patronymic"),
                            PhoneNumber = reader.GetString("phone_number"),
                            PassportSeries = reader.GetString("passport_series"),
                            PassportNumber = reader.GetString("passport_number"),
                            PassportDateOfRelease = reader.GetDateTime("passport_date_of_release"),
                            PassportReleaseOrganization = reader.GetString("passport_release_organization"),
                            LicenseNumber = reader.GetString("license_number"),
                            LicenseDateOfRelease = reader.GetDateTime("license_date_of_release"),
                            DriverFullName = reader.GetString("driver_name")
                        };
                    }
                }
            }

            return driver;
        }

        public List<Driver> GetDriversByVehicleId(int vehicleId)
        {
            var drivers = new List<Driver>();

            var connection = DataBaseConnector.Instance.GetConnection();

            var query = @"
                        SELECT d.id, 
                               d.name, 
                               d.surname,
                               d.patronymic, 
                               d.phone_number, 
                               d.passport_series, 
                               d.passport_number, 
                               d.passport_date_of_release, 
                               d.passport_release_organization,
                               d.license_number, 
                               d.license_date_of_release,
                               CONCAT_WS(' ', d.surname, d.name, d.patronymic) AS driver_name
                        FROM driver d
                        INNER JOIN driver_vehicle dv ON d.id = dv.driver_id
                        WHERE dv.vehicle_id = @VehicleId;
                    ";

            using (var command = new MySqlCommand(query, connection))
            {
                // Передаем значение параметра @VehicleId
                command.Parameters.AddWithValue("@VehicleId", vehicleId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Заполняем объект Driver данными из результата запроса
                        var driver = new Driver
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Surname = reader.GetString("surname"),
                            Patronymic = reader.GetString("patronymic"),
                            PhoneNumber = reader.GetString("phone_number"),
                            PassportSeries = reader.GetString("passport_series"),
                            PassportNumber = reader.GetString("passport_number"),
                            PassportDateOfRelease = reader.GetDateTime("passport_date_of_release"),
                            PassportReleaseOrganization = reader.GetString("passport_release_organization"),
                            LicenseNumber = reader.GetString("license_number"),
                            LicenseDateOfRelease = reader.GetDateTime("license_date_of_release"),
                            DriverFullName = reader.GetString("driver_name")
                        };

                        drivers.Add(driver);
                    }
                }
            }

            return drivers;
        }

        public bool UpdateDriverById(Driver updatedDriver)
        {
            var isUpdated = false;

            var connection = DataBaseConnector.Instance.GetConnection();

            var query = @"
                        UPDATE driver
                        SET 
                            name = @Name,
                            surname = @Surname,
                            patronymic = @Patronymic,
                            phone_number = @PhoneNumber,
                            passport_series = @PassportSeries,
                            passport_number = @PassportNumber,
                            passport_date_of_release = @PassportDateOfRelease,
                            passport_release_organization = @PassportReleaseOrganization,
                            license_number = @LicenseNumber,
                            license_date_of_release = @LicenseDateOfRelease
                        WHERE id = @DriverId;
                    ";

            using (var command = new MySqlCommand(query, connection))
            {
                // Заполняем параметры запроса
                command.Parameters.AddWithValue("@Name", updatedDriver.Name);
                command.Parameters.AddWithValue("@Surname", updatedDriver.Surname);
                command.Parameters.AddWithValue("@Patronymic", updatedDriver.Patronymic);
                command.Parameters.AddWithValue("@PhoneNumber", updatedDriver.PhoneNumber);
                command.Parameters.AddWithValue("@PassportSeries", updatedDriver.PassportSeries);
                command.Parameters.AddWithValue("@PassportNumber", updatedDriver.PassportNumber);
                command.Parameters.AddWithValue("@PassportDateOfRelease", updatedDriver.PassportDateOfRelease);
                command.Parameters.AddWithValue("@PassportReleaseOrganization", updatedDriver.PassportReleaseOrganization);
                command.Parameters.AddWithValue("@LicenseNumber", updatedDriver.LicenseNumber);
                command.Parameters.AddWithValue("@LicenseDateOfRelease", updatedDriver.LicenseDateOfRelease);
                command.Parameters.AddWithValue("@DriverId", updatedDriver.Id);

                // Выполняем команду
                var rowsAffected = command.ExecuteNonQuery();

                // Проверяем, была ли обновлена хотя бы одна запись
                isUpdated = rowsAffected > 0;
            }

            return isUpdated;
        }

        public bool AddDriversToVehicle(int vehicleId, List<Driver> drivers)
        {
            var isSuccess = false;

            var connection = DataBaseConnector.Instance.GetConnection();

            var queryCheckExisting = @"
                                        SELECT COUNT(*) 
                                        FROM driver_vehicle 
                                        WHERE vehicle_id = @VehicleId AND driver_id = @DriverId;
                                    ";

            var queryInsert = @"
                                INSERT INTO driver_vehicle (vehicle_id, driver_id) 
                                VALUES (@VehicleId, @DriverId);
                            ";

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var driver in drivers)
                    {
                        // Проверяем, есть ли уже запись для driver_id и vehicle_id
                        using (var commandCheck = new MySqlCommand(queryCheckExisting, connection, transaction))
                        {
                            commandCheck.Parameters.AddWithValue("@VehicleId", vehicleId);
                            commandCheck.Parameters.AddWithValue("@DriverId", driver.Id);

                            var existingCount = Convert.ToInt32(commandCheck.ExecuteScalar());
                            if (existingCount > 0)
                            {
                                // Если запись уже существует, пропускаем
                                continue;
                            }
                        }

                        // Вставляем новую запись
                        using (var commandInsert = new MySqlCommand(queryInsert, connection, transaction))
                        {
                            commandInsert.Parameters.AddWithValue("@VehicleId", vehicleId);
                            commandInsert.Parameters.AddWithValue("@DriverId", driver.Id);

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

        public bool AddDriver(Driver newDriver)
        {
            var isAdded = false;

            var connection = DataBaseConnector.Instance.GetConnection();

            var query = @"
                            INSERT INTO driver (name, 
                                                surname, 
                                                patronymic, 
                                                phone_number, 
                                                passport_series, 
                                                passport_number, 
                                                passport_date_of_release, 
                                                passport_release_organization, 
                                                license_number, 
                                                license_date_of_release)
                            VALUES (@Name, 
                                    @Surname, 
                                    @Patronymic, 
                                    @PhoneNumber, 
                                    @PassportSeries, 
                                    @PassportNumber, 
                                    @PassportDateOfRelease, 
                                    @PassportReleaseOrganization, 
                                    @LicenseNumber, 
                                    @LicenseDateOfRelease);
                            SELECT LAST_INSERT_ID();
                        ";

            using (var command = new MySqlCommand(query, connection))
            {
                // Заполняем параметры запроса
                command.Parameters.AddWithValue("@Name", newDriver.Name);
                command.Parameters.AddWithValue("@Surname", newDriver.Surname);
                command.Parameters.AddWithValue("@Patronymic", newDriver.Patronymic);
                command.Parameters.AddWithValue("@PhoneNumber", newDriver.PhoneNumber);
                command.Parameters.AddWithValue("@PassportSeries", newDriver.PassportSeries);
                command.Parameters.AddWithValue("@PassportNumber", newDriver.PassportNumber);
                command.Parameters.AddWithValue("@PassportDateOfRelease", newDriver.PassportDateOfRelease);
                command.Parameters.AddWithValue("@PassportReleaseOrganization", newDriver.PassportReleaseOrganization);
                command.Parameters.AddWithValue("@LicenseNumber", newDriver.LicenseNumber);
                command.Parameters.AddWithValue("@LicenseDateOfRelease", newDriver.LicenseDateOfRelease);

                try
                {
                    // Выполняем команду и получаем ID добавленного водителя
                    var insertedId = Convert.ToInt32(command.ExecuteScalar());
                    if (insertedId > 0)
                    {
                        newDriver.Id = insertedId;
                        isAdded = true;
                    }
                }
                catch (Exception ex)
                {
                    // Логируем или обрабатываем ошибку
                    Console.WriteLine($"Ошибка добавления водителя: {ex.Message}");
                    throw;
                }
            }

            return isAdded;
        }

        public bool RemoveDriverById(int driverId)
        {
            var isRemoved = false;

            var connection = DataBaseConnector.Instance.GetConnection();

            // SQL-запросы для удаления записей
            var queryDeleteDriverVehicle = @"
                                    DELETE FROM driver_vehicle 
                                    WHERE driver_id = @DriverId;
                                ";

            var queryDeleteDriver = @"
                            DELETE FROM driver 
                            WHERE id = @DriverId;
                        ";

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Удаляем записи из driver_vehicle
                    using (var commandDeleteDriverVehicle = new MySqlCommand(queryDeleteDriverVehicle, connection, transaction))
                    {
                        commandDeleteDriverVehicle.Parameters.AddWithValue("@DriverId", driverId);
                        commandDeleteDriverVehicle.ExecuteNonQuery();
                    }

                    // Удаляем запись из driver
                    using (var commandDeleteDriver = new MySqlCommand(queryDeleteDriver, connection, transaction))
                    {
                        commandDeleteDriver.Parameters.AddWithValue("@DriverId", driverId);
                        var rowsAffected = commandDeleteDriver.ExecuteNonQuery();

                        // Если из таблицы driver была удалена хотя бы одна запись, считаем операцию успешной
                        isRemoved = rowsAffected > 0;
                    }

                    // Подтверждаем транзакцию
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // В случае ошибки откатываем транзакцию
                    transaction.Rollback();
                    throw;
                }
            }

            return isRemoved;
        }
    }
}

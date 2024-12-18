using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrafficPoliceDomainDLL.model;

namespace TrafficPoliceDomainDLL.repository
{
    public class AccidentRepository
    {
        public Accident GetAccidentInfoById(int id)
        {
            var connection = DataBaseConnector.Instance.GetConnection();

            string query = $@"
                    SELECT 
                        a.id,
                        a.date,
                        a.street_id,
                        s.title AS street_title,
                        a.near_house_number,
                        a.severity,
                        a.inspector_id,
                        i.name AS inspector_name,
                        i.surname AS inspector_surname,
                        i.patronymic AS inspector_patronymic,
                        CONCAT_WS(' ', i.surname, i.name, i.patronymic) AS inspector_fullname,
                        a.description
                    FROM 
                        accident a
                    LEFT JOIN 
                        street s ON a.street_id = s.id
                    LEFT JOIN 
                        inspector i ON a.inspector_id = i.id
                    WHERE 
                        a.id = {id};";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var accident = new Accident
                        {
                            Id = reader.GetInt32("id"),
                            Date = reader.GetDateTime("date"),
                            StreetId = reader.GetInt32("street_id"),
                            StreetTitle = reader.GetString("street_title"),
                            NearHouseNumber = reader.GetString("near_house_number"),
                            Severity = reader.GetString("severity"),
                            InspectorId = reader.GetInt32("inspector_id"),
                            InspectorName = reader.GetString("inspector_name"),
                            InspectorSurname = reader.GetString("inspector_surname"),
                            InspectorPatronymic = reader.GetString("inspector_patronymic"),
                            InspectorFullName = reader.GetString("inspector_fullname"),
                            Description = reader.GetString("description")
                        };

                        return accident;
                    }
                }
            }

            return null;
        }
        public List<Accident_Vehicle> GetAllAccidents()
        {

            var accidents = new List<Accident_Vehicle>();

            var connection = DataBaseConnector.Instance.GetConnection();

            var query = @"SELECT 
                            av.id AS Id,
                            av.accident_id AS AccidentId,
                            a.date AS Date,
                            a.severity AS Severty,
                            av.vehicle_id AS VehicleId,
                            v.state_registration_number AS StateRegistrationNumber
                        FROM 
                            accident_vehicle av
                        INNER JOIN 
                            accident a ON av.accident_id = a.id
                        INNER JOIN 
                            vehicle v ON av.vehicle_id = v.id;";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var accident = new Accident_Vehicle
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            AccidentId = reader.GetInt32(reader.GetOrdinal("AccidentId")),
                            Date = reader.GetDateTime("Date"),
                            Severty = reader.GetString(reader.GetOrdinal("Severty")),
                            VehicleId = reader.GetInt32(reader.GetOrdinal("VehicleId")),
                            StateRegistrationNumber = reader.GetString(reader.GetOrdinal("StateRegistrationNumber"))


                        };
                        accidents.Add(accident);
                    }

                }

            }
            return accidents;
        }

        public List<Accident_Vehicle> GetAccidentsByVehicleId(int vehicleId)
        {
            var accidents = new List<Accident_Vehicle>();

            var connection = DataBaseConnector.Instance.GetConnection();

            var query = $@"
                SELECT 
                    av.id AS Id,
                    av.accident_id AS AccidentId,
                    a.date AS Date,
                    a.severity AS Severty,
                    av.vehicle_id AS VehicleId,
                    v.state_registration_number AS StateRegistrationNumber
                FROM 
                    accident_vehicle av
                INNER JOIN 
                    accident a ON av.accident_id = a.id
                INNER JOIN 
                    vehicle v ON av.vehicle_id = v.id
                WHERE 
                    av.vehicle_id = {vehicleId};";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@VehicleId", vehicleId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var accident = new Accident_Vehicle
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            AccidentId = reader.GetInt32(reader.GetOrdinal("AccidentId")),
                            Date = reader.GetDateTime("Date"),
                            Severty = reader.GetString(reader.GetOrdinal("Severty")),
                            VehicleId = reader.GetInt32(reader.GetOrdinal("VehicleId")),
                            StateRegistrationNumber = reader.GetString(reader.GetOrdinal("StateRegistrationNumber"))
                        };

                        accidents.Add(accident);
                    }
                }
            }

            return accidents;
        }

        public bool UpdateAccident(Accident accident)
        {
            bool isUpdated = false;
            try
            {
                // Получаем соединение с базой данных
                var connection = DataBaseConnector.Instance.GetConnection();

                // SQL-запрос для обновления информации о ДТП
                var query = @"
                            UPDATE accident
                            SET
                                date = @Date,
                                street_id = @StreetId,
                                near_house_number = @NearHouseNumber,
                                severity = @Severity,
                                inspector_id = @InspectorId,
                                description = @Description
                            WHERE id = @Id;
        ";

                using (var command = new MySqlCommand(query, connection))
                {
                    // Заполнение параметров запроса
                    command.Parameters.AddWithValue("@Id", accident.Id);
                    command.Parameters.AddWithValue("@Date", accident.Date);
                    command.Parameters.AddWithValue("@StreetId", accident.StreetId);
                    command.Parameters.AddWithValue("@NearHouseNumber", accident.NearHouseNumber);
                    command.Parameters.AddWithValue("@Severity", accident.Severity);
                    command.Parameters.AddWithValue("@InspectorId", accident.InspectorId);
                    command.Parameters.AddWithValue("@Description", accident.Description);

                    // Выполняем команду
                    int rowsAffected = command.ExecuteNonQuery();

                    // Проверяем, было ли обновление успешным
                    isUpdated = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                MessageBox.Show($"Ошибка при обновлении информации о ДТП: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isUpdated;
        }

        public bool AddAccidentParticipate(Accident accident, List<Vehicle> vehicles)
        {
            bool isSuccess = false;

            try
            {
                // Получаем соединение с базой данных
                var connection = DataBaseConnector.Instance.GetConnection();

                // Начинаем транзакцию для обеспечения целостности данных
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // SQL-запрос для вставки записей в таблицу accident_vehicle, если они ещё не существуют
                        var query = @"
                INSERT INTO accident_vehicle (accident_id, vehicle_id)
                SELECT @AccidentId, @VehicleId
                FROM DUAL
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM accident_vehicle
                    WHERE accident_id = @AccidentId AND vehicle_id = @VehicleId
                );
                ";

                        foreach (var vehicle in vehicles)
                        {
                            using (var command = new MySqlCommand(query, connection, transaction))
                            {
                                // Заполнение параметров запроса
                                command.Parameters.AddWithValue("@AccidentId", accident.Id);
                                command.Parameters.AddWithValue("@VehicleId", vehicle.Id);

                                // Выполняем команду
                                command.ExecuteNonQuery();
                            }
                        }

                        // Подтверждаем транзакцию, если все операции выполнены успешно
                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch (Exception)
                    {
                        // Откатываем транзакцию при ошибке
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                MessageBox.Show($"Ошибка при добавлении участников ДТП: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isSuccess;
        }


        public bool RemoveAccidentParticipate(int vehicleId, int accidentId)
        {
            bool isSuccess = false;

            try
            {
                // Получаем соединение с базой данных
                var connection = DataBaseConnector.Instance.GetConnection();

                // SQL-запрос для удаления записи из таблицы accident_vehicle
                var query = @"
            DELETE FROM accident_vehicle
            WHERE vehicle_id = @VehicleId AND accident_id = @AccidentId;
        ";

                using (var command = new MySqlCommand(query, connection))
                {
                    // Заполнение параметров запроса
                    command.Parameters.AddWithValue("@VehicleId", vehicleId);
                    command.Parameters.AddWithValue("@AccidentId", accidentId);

                    // Выполняем команду
                    int rowsAffected = command.ExecuteNonQuery();

                    // Если строки были затронуты, значит удаление прошло успешно
                    isSuccess = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                MessageBox.Show($"Ошибка при удалении участника ДТП: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isSuccess;
        }

        public int AddAccident(Accident accident)
        {
            try
            {
                var connection = DataBaseConnector.Instance.GetConnection();

                var query = @"
                             INSERT INTO accident (
                                    date, street_id, near_house_number, severity, inspector_id, description)
                             VALUES (
                                    @Date, @StreetId, @NearHouseNumber, @Severity, @InspectorId, @Description);
                             SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(query, connection))
                {
                    // Заполнение параметров запроса
                    command.Parameters.AddWithValue("@Date", accident.Date);
                    command.Parameters.AddWithValue("@StreetId", accident.StreetId);
                    command.Parameters.AddWithValue("@NearHouseNumber", accident.NearHouseNumber);
                    command.Parameters.AddWithValue("@Severity", accident.Severity);
                    command.Parameters.AddWithValue("@InspectorId", accident.InspectorId);
                    command.Parameters.AddWithValue("@Description", accident.Description);

                    var insertedId = Convert.ToInt32(command.ExecuteScalar()); // Получаем ID последней вставленной записи
                    return insertedId;

                }
            }

            catch (Exception ex)
            {
                // Логируем ошибку или обрабатываем её по вашему выбору
                Console.WriteLine($"Ошибка при добавлении транспортного средства: {ex.Message}");
                return 0;
            }
        }



    }
}

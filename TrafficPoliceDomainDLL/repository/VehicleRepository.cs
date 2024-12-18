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
    public class VehicleRepository
    {
        public List<Vehicle> GetAllVehicles()
        {
            var vehicles = new List<Vehicle>();

            var connection = DataBaseConnector.Instance.GetConnection();

            var query = @"
            SELECT v.id, v.color_id, v.car_model_id, v.car_body_model_id, v.car_body_number, v.chassis_number, v.engine_number, 
                   v.engine_volume, v.engine_power, v.annual_tax, v.steering_wheel_orientation, v.awd, v.date_of_release, 
                   v.tecnical_ticket_number, v.technical_ticket_date_of_release, v.owner_id, v.date_of_registration, 
                   v.state_registration_number, v.vin,
                   bm.title AS body_model_title,
                   cm.title AS car_model_title,
                   cb.id AS car_brand_id,
                   cb.title AS car_brand_title,
                   c.title AS color_title,
                   CONCAT_WS(' ', o.surname, o.name, o.patronymic) AS owner_name
            FROM vehicle v
            JOIN body_model bm ON v.car_body_model_id = bm.id
            JOIN car_model cm ON v.car_model_id = cm.id
            JOIN car_brand cb ON cm.brand_id = cb.id
            JOIN colour c ON v.color_id = c.id
            JOIN owner o ON v.owner_id = o.id
            ";
            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var vehicle = new Vehicle
                        {
                            Id = reader.GetInt32("id"),
                            ColorId = reader.GetInt32("color_id"),
                            ColorTitle = reader.GetString("color_title"),
                            CarModelId = reader.GetInt32("car_model_id"),
                            CarModelTitle = reader.GetString("car_model_title"),
                            CarBodyModelId = reader.GetInt32("car_body_model_id"),
                            BodyModelTitle = reader.GetString("body_model_title"),
                            CarBrandId = reader.GetInt32("car_brand_id"),
                            CarBrandTitle = reader.GetString("car_brand_title"),
                            CarBodyNumber = reader.GetString("car_body_number"),
                            ChassisNumber = reader.GetString("chassis_number"),
                            EngineNumber = reader.GetString("engine_number"),
                            EngineVolume = reader.GetDouble("engine_volume"),
                            EnginePower = reader.GetDouble("engine_power"),
                            AnnualTax = reader.GetDecimal("annual_tax"),
                            SteeringWheelOrientation = reader.GetBoolean("steering_wheel_orientation"),
                            AWD = reader.GetBoolean("awd"),
                            DateOfRelease = reader.GetDateTime("date_of_release"),
                            TechnicalTicketNumber = reader.GetString("tecnical_ticket_number"),
                            TechnicalTicketDateOfRelease = reader.GetDateTime("technical_ticket_date_of_release"),
                            OwnerId = reader.GetInt32("owner_id"),
                            OwnerNameTitle = reader.GetString("owner_name"),
                            DateOfRegistration = reader.GetDateTime("date_of_registration"),
                            StateRegistrationNumber = reader.GetString("state_registration_number"),
                            VIN = reader.GetString("vin")
                        };
                        vehicles.Add(vehicle);
                    }
                }
            }

            return vehicles;
        }


        public Vehicle GetVehicleByVehicleId(int vehicleId)
        {
            Vehicle vehicle = null;

            var connection = DataBaseConnector.Instance.GetConnection();

            var query = @"
        SELECT v.id, v.color_id, v.car_model_id, v.car_body_model_id, v.car_body_number, v.chassis_number, v.engine_number, 
               v.engine_volume, v.engine_power, v.annual_tax, v.steering_wheel_orientation, v.awd, v.date_of_release, 
               v.tecnical_ticket_number, v.technical_ticket_date_of_release, v.owner_id, v.date_of_registration, 
               v.state_registration_number, v.vin,
               bm.title AS body_model_title,
               cm.title AS car_model_title,
               cb.id AS car_brand_id,
               cb.title AS car_brand_title,
               c.title AS color_title,
               CONCAT_WS(' ', o.surname, o.name, o.patronymic) AS owner_name
        FROM vehicle v
        JOIN body_model bm ON v.car_body_model_id = bm.id
        JOIN car_model cm ON v.car_model_id = cm.id
        JOIN car_brand cb ON cm.brand_id = cb.id
        JOIN colour c ON v.color_id = c.id
        JOIN owner o ON v.owner_id = o.id
        WHERE v.id = @VehicleId;
    ";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@VehicleId", vehicleId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        vehicle = new Vehicle
                        {
                            Id = reader.GetInt32("id"),
                            ColorId = reader.GetInt32("color_id"),
                            ColorTitle = reader.GetString("color_title"),
                            CarModelId = reader.GetInt32("car_model_id"),
                            CarModelTitle = reader.GetString("car_model_title"),
                            CarBodyModelId = reader.GetInt32("car_body_model_id"),
                            BodyModelTitle = reader.GetString("body_model_title"),
                            CarBrandId = reader.GetInt32("car_brand_id"),
                            CarBrandTitle = reader.GetString("car_brand_title"),
                            CarBodyNumber = reader.GetString("car_body_number"),
                            ChassisNumber = reader.GetString("chassis_number"),
                            EngineNumber = reader.GetString("engine_number"),
                            EngineVolume = reader.GetDouble("engine_volume"),
                            EnginePower = reader.GetDouble("engine_power"),
                            AnnualTax = reader.GetDecimal("annual_tax"),
                            SteeringWheelOrientation = reader.GetBoolean("steering_wheel_orientation"),
                            AWD = reader.GetBoolean("awd"),
                            DateOfRelease = reader.GetDateTime("date_of_release"),
                            TechnicalTicketNumber = reader.GetString("tecnical_ticket_number"),
                            TechnicalTicketDateOfRelease = reader.GetDateTime("technical_ticket_date_of_release"),
                            OwnerId = reader.GetInt32("owner_id"),
                            OwnerNameTitle = reader.GetString("owner_name"),
                            DateOfRegistration = reader.GetDateTime("date_of_registration"),
                            StateRegistrationNumber = reader.GetString("state_registration_number"),
                            VIN = reader.GetString("vin")
                        };
                    }
                }
            }

            return vehicle;
        }

        public List<Vehicle> GetVehiclesByDriverId(int driverId)
        {
            var vehicles = new List<Vehicle>();

            var connection = DataBaseConnector.Instance.GetConnection();

            var query = $@"
            SELECT v.id, v.color_id, v.car_model_id, v.car_body_model_id, v.car_body_number, v.chassis_number, v.engine_number, 
                   v.engine_volume, v.engine_power, v.annual_tax, v.steering_wheel_orientation, v.awd, v.date_of_release, 
                   v.tecnical_ticket_number, v.technical_ticket_date_of_release, v.owner_id, v.date_of_registration, 
                   v.state_registration_number, v.vin,
                   bm.title AS body_model_title,
                   cm.title AS car_model_title,
                   cb.id AS car_brand_id,
                   cb.title AS car_brand_title,
                   c.title AS color_title,
                   CONCAT_WS(' ', o.surname, o.name, o.patronymic) AS owner_name
            FROM vehicle v
            JOIN body_model bm ON v.car_body_model_id = bm.id
            JOIN car_model cm ON v.car_model_id = cm.id
            JOIN car_brand cb ON cm.brand_id = cb.id
            JOIN colour c ON v.color_id = c.id
            JOIN owner o ON v.owner_id = o.id
            JOIN driver_vehicle dv ON v.id = dv.vehicle_id
            WHERE dv.driver_id = {driverId} 
            ";
            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var vehicle = new Vehicle
                        {
                            Id = reader.GetInt32("id"),
                            ColorId = reader.GetInt32("color_id"),
                            ColorTitle = reader.GetString("color_title"),
                            CarModelId = reader.GetInt32("car_model_id"),
                            CarModelTitle = reader.GetString("car_model_title"),
                            CarBodyModelId = reader.GetInt32("car_body_model_id"),
                            BodyModelTitle = reader.GetString("body_model_title"),
                            CarBrandId = reader.GetInt32("car_brand_id"),
                            CarBrandTitle = reader.GetString("car_brand_title"),
                            CarBodyNumber = reader.GetString("car_body_number"),
                            ChassisNumber = reader.GetString("chassis_number"),
                            EngineNumber = reader.GetString("engine_number"),
                            EngineVolume = reader.GetDouble("engine_volume"),
                            EnginePower = reader.GetDouble("engine_power"),
                            AnnualTax = reader.GetDecimal("annual_tax"),
                            SteeringWheelOrientation = reader.GetBoolean("steering_wheel_orientation"),
                            AWD = reader.GetBoolean("awd"),
                            DateOfRelease = reader.GetDateTime("date_of_release"),
                            TechnicalTicketNumber = reader.GetString("tecnical_ticket_number"),
                            TechnicalTicketDateOfRelease = reader.GetDateTime("technical_ticket_date_of_release"),
                            OwnerId = reader.GetInt32("owner_id"),
                            OwnerNameTitle = reader.GetString("owner_name"),
                            DateOfRegistration = reader.GetDateTime("date_of_registration"),
                            StateRegistrationNumber = reader.GetString("state_registration_number"),
                            VIN = reader.GetString("vin")
                        };
                        vehicles.Add(vehicle);
                    }
                }
            }

            return vehicles;
        }

        public bool UpdateVehicle(Vehicle vehicle)
        {
            bool isUpdated = false;
            try
            {


                // Получаем соединение с базой данных
                var connection = DataBaseConnector.Instance.GetConnection();

                // SQL-запрос для обновления информации о ТС
                var query = @"
                            UPDATE vehicle
                            SET
                                color_id = @ColorId,
                                car_model_id = @CarModelId,
                                car_body_model_id = @CarBodyModelId,
                                car_body_number = @CarBodyNumber,
                                chassis_number = @ChassisNumber,
                                engine_number = @EngineNumber,
                                engine_volume = @EngineVolume,
                                engine_power = @EnginePower,
                                annual_tax = @AnnualTax,
                                steering_wheel_orientation = @SteeringWheelOrientation,
                                awd = @AWD,
                                date_of_release = @DateOfRelease,
                                tecnical_ticket_number = @TechnicalTicketNumber,
                                technical_ticket_date_of_release = @TechnicalTicketDateOfRelease,
                                owner_id = @OwnerId,
                                date_of_registration = @DateOfRegistration,
                                state_registration_number = @StateRegistrationNumber,
                                vin = @VIN
                            WHERE id = @Id;
                        ";

                using (var command = new MySqlCommand(query, connection))
                {
                    // Заполнение параметров запроса
                    command.Parameters.AddWithValue("@Id", vehicle.Id);
                    command.Parameters.AddWithValue("@ColorId", vehicle.ColorId);
                    command.Parameters.AddWithValue("@CarModelId", vehicle.CarModelId);
                    command.Parameters.AddWithValue("@CarBodyModelId", vehicle.CarBodyModelId);
                    command.Parameters.AddWithValue("@CarBodyNumber", vehicle.CarBodyNumber);
                    command.Parameters.AddWithValue("@ChassisNumber", vehicle.ChassisNumber);
                    command.Parameters.AddWithValue("@EngineNumber", vehicle.EngineNumber);
                    command.Parameters.AddWithValue("@EngineVolume", vehicle.EngineVolume);
                    command.Parameters.AddWithValue("@EnginePower", vehicle.EnginePower);
                    command.Parameters.AddWithValue("@AnnualTax", vehicle.AnnualTax);
                    command.Parameters.AddWithValue("@SteeringWheelOrientation", vehicle.SteeringWheelOrientation);
                    command.Parameters.AddWithValue("@AWD", vehicle.AWD);
                    command.Parameters.AddWithValue("@DateOfRelease", vehicle.DateOfRelease);
                    command.Parameters.AddWithValue("@TechnicalTicketNumber", vehicle.TechnicalTicketNumber);
                    command.Parameters.AddWithValue("@TechnicalTicketDateOfRelease", vehicle.TechnicalTicketDateOfRelease);
                    command.Parameters.AddWithValue("@OwnerId", vehicle.OwnerId);
                    command.Parameters.AddWithValue("@DateOfRegistration", vehicle.DateOfRegistration);
                    command.Parameters.AddWithValue("@StateRegistrationNumber", vehicle.StateRegistrationNumber);
                    command.Parameters.AddWithValue("@VIN", vehicle.VIN);


                    // Выполняем команду
                    int rowsAffected = command.ExecuteNonQuery();

                    // Проверяем, было ли обновление успешным
                    isUpdated = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении информации о ТС: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            return isUpdated;

        }

        public bool DeleteVehicle(int vehicleId)
        {
            bool isDeleted = false;

            try
            {
                // Получаем соединение с базой данных
                var connection = DataBaseConnector.Instance.GetConnection();

                // SQL-запросы для удаления связанных записей
                var deleteTechnicalInspectionQuery = "DELETE FROM technical_inspection WHERE vehicle_id = @VehicleId;";
                var deleteOwnerQuery = @"
            DELETE FROM owner 
            WHERE id = (SELECT owner_id FROM vehicle WHERE id = @VehicleId);";
                var deleteVehicleQuery = "DELETE FROM vehicle WHERE id = @VehicleId;";

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Удаляем запись о техническом осмотре
                        using (var command = new MySqlCommand(deleteTechnicalInspectionQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@VehicleId", vehicleId);
                            command.ExecuteNonQuery();
                        }

                        // Удаляем запись о владельце
                        using (var command = new MySqlCommand(deleteOwnerQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@VehicleId", vehicleId);
                            command.ExecuteNonQuery();
                        }

                        // Удаляем транспортное средство
                        using (var command = new MySqlCommand(deleteVehicleQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@VehicleId", vehicleId);
                            int rowsAffected = command.ExecuteNonQuery();
                            isDeleted = rowsAffected > 0;
                        }

                        // Подтверждаем транзакцию
                        transaction.Commit();
                    }
                    catch
                    {
                        // Откатываем изменения в случае ошибки
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении транспортного средства: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isDeleted;
        }


        public int AddVehicle(Vehicle vehicle)
        {


            try
            {
                // Получаем соединение с базой данных
                var connection = DataBaseConnector.Instance.GetConnection();

                // SQL-запрос для вставки нового транспортного средства
                var query = @"
                        INSERT INTO vehicle (
                            color_id, car_model_id, car_body_model_id, car_body_number, chassis_number, engine_number, 
                            engine_volume, engine_power, annual_tax, steering_wheel_orientation, awd, date_of_release, 
                            tecnical_ticket_number, technical_ticket_date_of_release, owner_id, date_of_registration, 
                            state_registration_number, vin)
                        VALUES (
                            @ColorId, @CarModelId, @CarBodyModelId, @CarBodyNumber, @ChassisNumber, @EngineNumber, 
                            @EngineVolume, @EnginePower, @AnnualTax, @SteeringWheelOrientation, @AWD, @DateOfRelease, 
                            @TechnicalTicketNumber, @TechnicalTicketDateOfRelease, @OwnerId, @DateOfRegistration, 
                            @StateRegistrationNumber, @VIN);
                        SELECT LAST_INSERT_ID();
                    ";

                using (var command = new MySqlCommand(query, connection))
                {
                    // Заполнение параметров запроса
                    command.Parameters.AddWithValue("@ColorId", vehicle.ColorId);
                    command.Parameters.AddWithValue("@CarModelId", vehicle.CarModelId);
                    command.Parameters.AddWithValue("@CarBodyModelId", vehicle.CarBodyModelId);
                    command.Parameters.AddWithValue("@CarBodyNumber", vehicle.CarBodyNumber);
                    command.Parameters.AddWithValue("@ChassisNumber", vehicle.ChassisNumber);
                    command.Parameters.AddWithValue("@EngineNumber", vehicle.EngineNumber);
                    command.Parameters.AddWithValue("@EngineVolume", vehicle.EngineVolume);
                    command.Parameters.AddWithValue("@EnginePower", vehicle.EnginePower);
                    command.Parameters.AddWithValue("@AnnualTax", vehicle.AnnualTax);
                    command.Parameters.AddWithValue("@SteeringWheelOrientation", vehicle.SteeringWheelOrientation);
                    command.Parameters.AddWithValue("@AWD", vehicle.AWD);
                    command.Parameters.AddWithValue("@DateOfRelease", vehicle.DateOfRelease);
                    command.Parameters.AddWithValue("@TechnicalTicketNumber", vehicle.TechnicalTicketNumber);
                    command.Parameters.AddWithValue("@TechnicalTicketDateOfRelease", vehicle.TechnicalTicketDateOfRelease);
                    command.Parameters.AddWithValue("@OwnerId", vehicle.OwnerId);
                    command.Parameters.AddWithValue("@DateOfRegistration", vehicle.DateOfRegistration);
                    command.Parameters.AddWithValue("@StateRegistrationNumber", vehicle.StateRegistrationNumber);
                    command.Parameters.AddWithValue("@VIN", vehicle.VIN);


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

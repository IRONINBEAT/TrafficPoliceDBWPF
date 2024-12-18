using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficPoliceDomainDLL.model;

namespace TrafficPoliceDomainDLL.repository
{
    public class TechnicalInspectionRepository
    {
        public TechnicalInspection GetTechnicalInspectionByVehicleId(int id)
        {
            var connection = DataBaseConnector.Instance.GetConnection();

            var query = $@"SELECT 
                                ti.id AS Id,
                                ti.vehicle_id AS VehicleId,
                                v.state_registration_number AS StateRegistrationNumber,
                                ti.date_of_inspection AS DateOfInspection,
                                ti.inspector_id AS InspectorId,
                                i.Surname AS InspectorSurname,
                                i.Name AS InspectorName,
                                i.Patronymic AS InspectorPatronymic,
                                CONCAT(i.Surname, ' ', i.Name, ' ', i.Patronymic) AS InspectorFullName,
                                ti.Mileage AS Mileage,
                                ti.inspection_price AS InspectionPrice,
                                ti.sign_price AS SignPrice
                            FROM 
                                technical_inspection ti
                            JOIN 
                                inspector i ON ti.inspector_id = i.id
                            JOIN
                                vehicle v ON ti.vehicle_id = v.id
                            WHERE 
                                ti.vehicle_id = {id};";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var technicalInspection = new TechnicalInspection
                        {
                            Id = reader.GetInt32("Id"),
                            VehicleId = reader.GetInt32("VehicleId"),
                            StateRegistrationNumber = reader.GetString("StateRegistrationNumber"),
                            DateOfInspection = reader.GetDateTime("DateOfInspection"),
                            InspectorId = reader.GetInt32("InspectorId"),
                            InspectorSurname = reader.GetString("InspectorSurname"),
                            InspectorName = reader.GetString("InspectorName"),
                            InspectorPatronymic = reader.GetString("InspectorPatronymic"),
                            InspectorFullName = reader.GetString("InspectorFullName"),
                            Mileage = reader.GetInt32("Mileage"),
                            InspectionPrice = reader.GetDecimal("InspectionPrice"),
                            SignPrice = reader.GetDecimal("SignPrice")
                        };

                        return technicalInspection;
                    }
                }
            }
            return null;
        }

        public bool UpdateTechnicalInspection(TechnicalInspection technicalInspection)
        {
            try
            {
                var connection = DataBaseConnector.Instance.GetConnection();

                var query = @"
                            UPDATE technical_inspection
                            SET 
                                vehicle_id = @VehicleId,
                                inspector_id = @InspectorId,
                                mileage = @Mileage,
                                inspection_price = @InspectionPrice,
                                sign_price = @SignPrice
                            WHERE id = @Id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", technicalInspection.Id);
                    command.Parameters.AddWithValue("@VehicleId", technicalInspection.VehicleId);
                    command.Parameters.AddWithValue("@InspectorId", technicalInspection.InspectorId);
                    command.Parameters.AddWithValue("@Mileage", technicalInspection.Mileage);
                    command.Parameters.AddWithValue("@InspectionPrice", technicalInspection.InspectionPrice);
                    command.Parameters.AddWithValue("@SignPrice", technicalInspection.SignPrice);

                    int affectedRows = command.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку или обрабатываем её по вашему выбору
                Console.WriteLine($"Ошибка обновления техосмотра: {ex.Message}");
                return false;
            }
        }


        public bool AddTechnicalInspection(TechnicalInspection technicalInspection)
        {
            try
            {
                var connection = DataBaseConnector.Instance.GetConnection();
                var query = @"
                            INSERT INTO technical_inspection (vehicle_id, inspector_id, date_of_inspection, mileage, inspection_price, sign_price) 
                            VALUES (@VehicleId, @InspectorId, @DateOfInspection, @Mileage, @InspectionPrice, @SignPrice);
                            SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@VehicleId", technicalInspection.VehicleId);
                    command.Parameters.AddWithValue("@InspectorId", technicalInspection.InspectorId);
                    command.Parameters.AddWithValue("@DateOfInspection", technicalInspection.DateOfInspection);
                    command.Parameters.AddWithValue("@Mileage", technicalInspection.Mileage);
                    command.Parameters.AddWithValue("@InspectionPrice", technicalInspection.InspectionPrice);
                    command.Parameters.AddWithValue("@SignPrice", technicalInspection.SignPrice);

                    int affectedRows = command.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку или обрабатываем её по вашему выбору
                Console.WriteLine($"Ошибка при добавлении техосмотра: {ex.Message}");
                return false;
            }
        }


    }
}

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
    public class OwnerRepository
    {
        public List<Owner> GetAllOwners()
        {
            var owners = new List<Owner>();

            var connection = DataBaseConnector.Instance.GetConnection();

            var query = @"
                SELECT 
                    o.id,
                    o.legal_relation,
                    o.name,
                    o.surname,
                    o.patronymic,
                    o.postal_code,
                    o.phone_number,
                    o.passport_series,
                    o.passport_number,
                    o.passport_date_of_release,
                    o.passport_release_organization,
                    o.organization_name,
                    o.city,
                    o.street_id,
                    s.title AS street_title,
                    o.house_number,
                    o.appartment_number
                FROM owner o
                LEFT JOIN street s ON o.street_id = s.id";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32("legal_relation") == 0)
                        {
                            var owner = new Owner
                            {
                                Id = reader.GetInt32("id"),
                                LegalRelation = reader.GetBoolean("legal_relation"),
                                Name = reader.GetString("name"),
                                Surname = reader.GetString("surname"),
                                Patronymic = reader.GetString("patronymic"),
                                PostalCode = reader.GetString("postal_code"),
                                PhoneNumber = reader.GetString("phone_number"),
                                PassportSeries = reader.GetString("passport_series"),
                                PassportNumber = reader.GetString("passport_number"),
                                PassportDateOfRelease = reader.GetDateTime("passport_date_of_release"),
                                PassportReleaseOrganization = reader.GetString("passport_release_organization"),
                                City = reader.GetString("city"),
                                StreetId = reader.GetInt32("street_id"),
                                StreetTitle = reader.GetString("street_title"),
                                HouseNumber = reader.GetString("house_number"),
                                AppartmentNumber = reader.GetString("appartment_number")
                            };

                            owners.Add(owner);
                        }
                        else
                        {
                            var owner = new Owner
                            {
                                Id = reader.GetInt32("id"),
                                LegalRelation = reader.GetBoolean("legal_relation"),
                                Name = reader.GetString("name"),
                                Surname = reader.GetString("surname"),
                                Patronymic = reader.GetString("patronymic"),
                                PhoneNumber = reader.GetString("phone_number"),
                                OrganizationName = reader.GetString("organization_name"),
                                City = reader.GetString("city"),
                                StreetId = reader.GetInt32("street_id"),
                                StreetTitle = reader.GetString("street_title"),
                                HouseNumber = reader.GetString("house_number"),
                                AppartmentNumber = reader.GetString("appartment_number")
                            };

                            owners.Add(owner);
                        }


                    }

                }
            }
            return owners;
        }


        public Owner GetOwnerById(int id)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = $@"
                SELECT 
                    o.id,
                    o.legal_relation,
                    o.name,
                    o.surname,
                    o.patronymic,
                    o.postal_code,
                    o.phone_number,
                    o.passport_series,
                    o.passport_number,
                    o.passport_date_of_release,
                    o.passport_release_organization,
                    o.organization_name,
                    o.city,
                    o.street_id,
                    s.title AS street_title,
                    o.house_number,
                    o.appartment_number
                FROM owner o
                LEFT JOIN street s ON o.street_id = s.id
                WHERE o.id = {id}";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32("legal_relation") == 0)
                        {
                            var owner = new Owner
                            {
                                Id = reader.GetInt32("id"),
                                LegalRelation = reader.GetBoolean("legal_relation"),
                                Name = reader.GetString("name"),
                                Surname = reader.GetString("surname"),
                                Patronymic = reader.GetString("patronymic"),
                                PostalCode = reader.GetString("postal_code"),
                                PhoneNumber = reader.GetString("phone_number"),
                                PassportSeries = reader.GetString("passport_series"),
                                PassportNumber = reader.GetString("passport_number"),
                                PassportDateOfRelease = reader.GetDateTime("passport_date_of_release"),
                                PassportReleaseOrganization = reader.GetString("passport_release_organization"),
                                City = reader.GetString("city"),
                                StreetId = reader.GetInt32("street_id"),
                                StreetTitle = reader.GetString("street_title"),
                                HouseNumber = reader.GetString("house_number"),
                                AppartmentNumber = reader.GetString("appartment_number")
                            };

                            return owner;
                        }
                        else
                        {
                            var owner = new Owner
                            {
                                Id = reader.GetInt32("id"),
                                LegalRelation = reader.GetBoolean("legal_relation"),
                                Name = reader.GetString("name"),
                                Surname = reader.GetString("surname"),
                                Patronymic = reader.GetString("patronymic"),
                                PhoneNumber = reader.GetString("phone_number"),
                                PostalCode = reader.GetString("postal_code"),
                                OrganizationName = reader.GetString("organization_name"),
                                City = reader.GetString("city"),
                                StreetId = reader.GetInt32("street_id"),
                                StreetTitle = reader.GetString("street_title"),
                                HouseNumber = reader.GetString("house_number"),
                                AppartmentNumber = reader.GetString("appartment_number")
                            };

                            return owner;
                        }


                    }

                }
            }

            return null;
        }

        public bool UpdateOwner(Owner owner)
        {
            try
            {
                var connection = DataBaseConnector.Instance.GetConnection();

                var query = @"
            UPDATE owner
            SET 
                legal_relation = @LegalRelation,
                name = @Name,
                surname = @Surname,
                patronymic = @Patronymic,
                postal_code = @PostalCode,
                phone_number = @PhoneNumber,
                passport_series = @PassportSeries,
                passport_number = @PassportNumber,
                passport_date_of_release = @PassportDateOfRelease,
                passport_release_organization = @PassportReleaseOrganization,
                organization_name = @OrganizationName,
                city = @City,
                street_id = @StreetId,
                house_number = @HouseNumber,
                appartment_number = @AppartmentNumber
            WHERE id = @Id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", owner.Id);
                    command.Parameters.AddWithValue("@LegalRelation", owner.LegalRelation);
                    command.Parameters.AddWithValue("@Name", owner.Name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Surname", owner.Surname ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Patronymic", owner.Patronymic ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PostalCode", owner.PostalCode ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PhoneNumber", owner.PhoneNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PassportSeries", owner.PassportSeries ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PassportNumber", owner.PassportNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PassportDateOfRelease",
                        owner.PassportDateOfRelease == DateTime.MinValue
                        ? (object)DBNull.Value
                        : owner.PassportDateOfRelease);
                    command.Parameters.AddWithValue("@PassportReleaseOrganization", owner.PassportReleaseOrganization ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@OrganizationName", owner.OrganizationName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@City", owner.City ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@StreetId", owner.StreetId);
                    command.Parameters.AddWithValue("@HouseNumber", owner.HouseNumber);
                    command.Parameters.AddWithValue("@AppartmentNumber", owner.AppartmentNumber ?? (object)DBNull.Value);

                    int affectedRows = command.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку или обрабатываем её по вашему выбору
                Console.WriteLine($"Ошибка обновления владельца: {ex.Message}");
                return false;
            }
        }

        public int AddOwner(Owner owner)
        {
            try
            {
                var connection = DataBaseConnector.Instance.GetConnection();

                // SQL-запрос для вставки нового владельца в таблицу
                var query = @"
                            INSERT INTO owner (
                                legal_relation,
                                name,
                                surname,
                                patronymic,
                                postal_code,
                                phone_number,
                                passport_series,
                                passport_number,
                                passport_date_of_release,
                                passport_release_organization,
                                organization_name,
                                city,
                                street_id,
                                house_number,
                                appartment_number
                            )
                            VALUES (
                                @LegalRelation,
                                @Name,
                                @Surname,
                                @Patronymic,
                                @PostalCode,
                                @PhoneNumber,
                                @PassportSeries,
                                @PassportNumber,
                                @PassportDateOfRelease,
                                @PassportReleaseOrganization,
                                @OrganizationName,
                                @City,
                                @StreetId,
                                @HouseNumber,
                                @AppartmentNumber
                            ); SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(query, connection))
                {
                    // Параметры для запроса
                    command.Parameters.AddWithValue("@LegalRelation", owner.LegalRelation);
                    command.Parameters.AddWithValue("@Name", owner.Name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Surname", owner.Surname ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Patronymic", owner.Patronymic ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PostalCode", owner.PostalCode ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PhoneNumber", owner.PhoneNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PassportSeries", owner.PassportSeries ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PassportNumber", owner.PassportNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PassportDateOfRelease",
                        owner.PassportDateOfRelease == DateTime.MinValue
                        ? (object)DBNull.Value
                        : owner.PassportDateOfRelease);
                    command.Parameters.AddWithValue("@PassportReleaseOrganization", owner.PassportReleaseOrganization ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@OrganizationName", owner.OrganizationName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@City", owner.City ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@StreetId", owner.StreetId);
                    command.Parameters.AddWithValue("@HouseNumber", owner.HouseNumber);
                    command.Parameters.AddWithValue("@AppartmentNumber", owner.AppartmentNumber ?? (object)DBNull.Value);

                    var insertedId = Convert.ToInt32(command.ExecuteScalar()); // Получаем ID последней вставленной записи
                    return insertedId;
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку или обрабатываем её по вашему выбору
                Console.WriteLine($"Ошибка при добавлении владельца: {ex.Message}");
                return 0;
            }
        }

        public bool DeleteOwner(int ownerId)
        {
            bool isDeleted = false;
            try
            {
                // Получаем соединение с базой данных
                var connection = DataBaseConnector.Instance.GetConnection();

                var deleteOwnerQuery = @"
            DELETE FROM owner 
            WHERE id = @OwnerId";

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {


                        // Удаляем запись о владельце
                        using (var command = new MySqlCommand(deleteOwnerQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@OwnerId", ownerId);
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
                MessageBox.Show($"Ошибка при удалении владельца: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isDeleted;
        }

    }
}

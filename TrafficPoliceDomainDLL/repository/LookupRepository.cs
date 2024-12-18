using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficPoliceDomainDLL.model;

namespace TrafficPoliceDomainDLL.repository
{
    public class LookupRepository
    {
        public List<LookupItem> GetLookupItems(string tableName, int brandId = 0)
        {
            var items = new List<LookupItem>();

            var connection = DataBaseConnector.Instance.GetConnection();

            if (tableName == "car_model")
            {
                if (brandId != 0)
                {
                    var query = $"SELECT id, title FROM {tableName} WHERE brand_id = {brandId}";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new LookupItem
                                {
                                    Id = reader.GetInt32(0),
                                    Title = reader.GetString(1)
                                });
                            }
                        }
                    }
                }
                else
                {
                    var query = $"SELECT id, title FROM {tableName}";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new LookupItem
                                {
                                    Id = reader.GetInt32(0),
                                    Title = reader.GetString(1)
                                });
                            }
                        }
                    }
                }
            }
            else if (tableName == "license_category")
            {
                var query = $@"
                                SELECT 
                                    id, 
                                    code, 
                                    description
                                FROM 
                                    {tableName}";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var code = reader.GetString("code");
                            var description = reader.GetString("description");

                            var resultString = code + ", " + description;

                            items.Add(new LookupItem
                            {
                                Id = reader.GetInt32(0),
                                Title = resultString
                            });
                        }
                    }
                }
            }
            else if (tableName == "inspector")
            {
                var query = @"
                                SELECT 
                                    i.id, 
                                    i.name, 
                                    i.surname, 
                                    i.patronymic, 
                                    p.title AS PostTitle
                                FROM 
                                    inspector i
                                LEFT JOIN 
                                    post p ON i.post_id = p.id";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var name = reader.GetString("name");
                            var surname = reader.GetString("surname");
                            var patronymic = reader.GetString("patronymic");
                            var postTitle = reader.GetString("PostTitle");

                            var resultString = surname + " " + name + " " + patronymic + " звание: " + postTitle;

                            items.Add(new LookupItem
                            {
                                Id = reader.GetInt32(0),
                                Title = resultString
                            });
                        }
                    }
                }

            }
            else if (tableName == "vehicle")
            {
                var query = @"SELECT v.id, v.state_registration_number, o.name, o.surname, o.patronymic
                              FROM vehicle v
                              JOIN owner o ON v.owner_id = o.id";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var name = reader.GetString("name");
                            var surname = reader.GetString("surname");
                            var patronymic = reader.GetString("patronymic");
                            var StateRegistrationNumber = reader.GetString("state_registration_number");

                            var resultString = StateRegistrationNumber + " владелец: " + surname + " " + name + " " + patronymic;

                            items.Add(new LookupItem
                            {
                                Id = reader.GetInt32(0),
                                Title = resultString
                            });
                        }
                    }
                }
            }
            else if (tableName == "owner")
            {
                var query = $"SELECT id, name, surname, patronymic, passport_series," +
                            $"passport_number, passport_date_of_release, passport_release_organization" +
                            $" FROM {tableName}";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var surname = reader.IsDBNull(reader.GetOrdinal("surname")) ? string.Empty : reader.GetString("surname");
                            var name = reader.IsDBNull(reader.GetOrdinal("name")) ? string.Empty : reader.GetString("name");
                            var patronymic = reader.IsDBNull(reader.GetOrdinal("patronymic")) ? string.Empty : reader.GetString("patronymic");
                            var passportSeries = reader.IsDBNull(reader.GetOrdinal("passport_series")) ? string.Empty : reader.GetString("passport_series");
                            var passportNumber = reader.IsDBNull(reader.GetOrdinal("passport_number")) ? string.Empty : reader.GetString("passport_number");
                            var passportDateOfRelease = reader.IsDBNull(reader.GetOrdinal("passport_date_of_release"))
                                ? string.Empty
                                : reader.GetDateTime("passport_date_of_release").ToString("d");
                            var passportReleaseOrganization = reader.IsDBNull(reader.GetOrdinal("passport_release_organization"))
                                ? string.Empty
                                : reader.GetString("passport_release_organization");

                            var resultTitle = (surname + " " + name + " " + patronymic + " серия: " +
                                               passportSeries + " номер: " + passportNumber + " выдан: " +
                                               passportReleaseOrganization + " " + passportDateOfRelease);

                            items.Add(new LookupItem
                            {
                                Id = reader.GetInt32(0),
                                Title = resultTitle
                            });
                        }

                    }
                }
            }
            else
            {
                var query = $"SELECT id, title FROM {tableName}";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new LookupItem
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return items;
        }
    }
}

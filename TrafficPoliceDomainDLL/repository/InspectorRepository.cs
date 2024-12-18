using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficPoliceDomainDLL.model;

namespace TrafficPoliceDomainDLL.repository
{
    public class InspectorRepository
    {
        public List<Inspector> GetAllInspectors()
        {
            var inspectors = new List<Inspector>();

            var connection = DataBaseConnector.Instance.GetConnection();

            var query = @"
                    SELECT 
                        i.id, 
                        i.name, 
                        i.surname, 
                        i.patronymic, 
                        i.post_id, 
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
                        var inspector = new Inspector
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Surname = reader.GetString("surname"),
                            Patronymic = reader.GetString("patronymic"),
                            PostId = reader.GetInt32("post_id"),
                            PostTitle = reader.GetString("PostTitle")
                        };
                        inspectors.Add(inspector);
                    }

                }

            }
            return inspectors;
        }
    }
}

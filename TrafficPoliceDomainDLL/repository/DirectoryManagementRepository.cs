using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficPoliceDomainDLL.model;

namespace TrafficPoliceDomainDLL.repository
{
    public class DirectoryManagementRepository
    {
        public List<LookupItem> GetAllStreets()
        {
            var streets = new List<LookupItem>();
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "SELECT id, title FROM street";

            using (var command = new MySqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    streets.Add(new LookupItem
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("title")
                    });
                }
            }

            return streets;
        }

        public void AddStreet(string title)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "INSERT INTO street (title) VALUES (@Title)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateStreet(int id, string title)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "UPDATE street SET title = @Title WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteStreet(int id)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "DELETE FROM street WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public List<LookupItem> GetAllColors()
        {
            var colors = new List<LookupItem>();
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "SELECT id, title FROM colour";

            using (var command = new MySqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    colors.Add(new LookupItem
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("title")
                    });
                }
            }

            return colors;
        }

        public void AddColor(string title)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "INSERT INTO colour (title) VALUES (@Title)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateColor(int id, string title)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "UPDATE colour SET title = @Title WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteColor(int id)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "DELETE FROM colour WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public List<LookupItem> GetAllPosts()
        {
            var ranks = new List<LookupItem>();
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "SELECT id, title FROM post";

            using (var command = new MySqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ranks.Add(new LookupItem
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("title")
                    });
                }
            }

            return ranks;
        }

        public LookupItem GetPostById(int postId)
        {
            var rank = new LookupItem();
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = @"SELECT id, title FROM post WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", postId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return rank;
                    }
                }
            }

            return null;

        }

        public void AddPost(string title)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "INSERT INTO post (title) VALUES (@Title)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.ExecuteNonQuery();
            }
        }

        public void UpdatePost(int id, string title)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "UPDATE post SET title = @Title WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public void DeletePost(int id)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "DELETE FROM post WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }



        public List<LookupItem> GetAllLicenseCategories()
        {
            var categories = new List<LookupItem>();
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "SELECT id, code, description FROM license_category";

            using (var command = new MySqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var code = reader.GetString("code");
                    var description = reader.GetString("description");

                    var resultString = code + ", " + description;

                    categories.Add(new LookupItem
                    {
                        Id = reader.GetInt32("id"),
                        Title = resultString
                    });
                }
            }

            return categories;
        }

        public void AddLicenseCategory(string code, string description)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "INSERT INTO license_category (code, description) VALUES (@Code, @Description)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Code", code);
                command.Parameters.AddWithValue("@Description", description);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateLicenseCategory(int id, string code, string description)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "UPDATE license_category SET code = @Code, description = @Description WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Code", code);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteLicenseCategory(int id)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "DELETE FROM license_category WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public List<LookupItem> GetAllInspectors()
        {
            var inspectors = new List<LookupItem>();
            var connection = DataBaseConnector.Instance.GetConnection();
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
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var name = reader.GetString("name");
                    var surname = reader.GetString("surname");
                    var patronymic = reader.GetString("patronymic");
                    var postTitle = reader.GetString("PostTitle");

                    var resultString = surname + " " + name + " " + patronymic;

                    inspectors.Add(new LookupItem
                    {
                        Id = reader.GetInt32("id"),
                        Title = resultString
                    });
                }
            }

            return inspectors;
        }

        public Inspector GetInspectorById(int inspectorId)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = @"
                                SELECT 
                                    i.id, 
                                    i.name, 
                                    i.surname, 
                                    i.patronymic, 
                                    p.title AS PostTitle,
                                    p.id AS PostId
                                FROM 
                                    inspector i
                                
                                LEFT JOIN 
                                    post p ON i.post_id = p.id
                                WHERE i.id = @id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", inspectorId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        var inspector = new Inspector()
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Surname = reader.GetString("surname"),
                            Patronymic = reader.GetString("patronymic"),
                            PostId = reader.GetInt32("PostId"),
                            PostTitle = reader.GetString("PostTitle")
                        };

                        return inspector;
                    }
                }
            }

            return null;
        }

        public void AddInspector(string name, string surname, string patronymic, int postId)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "INSERT INTO inspector (name, surname, patronymic, post_id) VALUES (@Name, @Surname, @Patronymic, @PostId)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Surname", surname);
                command.Parameters.AddWithValue("@Patronymic", patronymic);
                command.Parameters.AddWithValue("@PostId", postId);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateInspector(int id, string name, string surname, string patronymic, int postId)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "UPDATE inspector SET name = @Name, surname = @Surname, patronymic = @Patronymic, post_id = @PostId WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Surname", surname);
                command.Parameters.AddWithValue("@Patronymic", patronymic);
                command.Parameters.AddWithValue("@PostId", postId);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteInspector(int id)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "DELETE FROM inspector WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public List<LookupItem> GetAllBrands()
        {
            var brands = new List<LookupItem>();
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "SELECT id, title FROM car_brand";

            using (var command = new MySqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    brands.Add(new LookupItem
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("title")
                    });
                }
            }

            return brands;
        }

        public void AddBrand(string title)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "INSERT INTO car_brand (title) VALUES (@Title)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateBrand(int id, string title)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "UPDATE car_brand SET title = @Title WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteBrand(int id)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "DELETE FROM car_brand WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public List<LookupItem> GetAllModels()
        {
            var models = new List<LookupItem>();
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "SELECT id, title, brand_id FROM car_model";

            using (var command = new MySqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    models.Add(new LookupItem
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("title")
                    });
                }
            }

            return models;
        }

        public void AddModel(string title, int brandId)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "INSERT INTO car_model (title, brand_id) VALUES (@Title, @BrandId)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@BrandId", brandId);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateModel(int id, string title, int brandId)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "UPDATE car_model SET title = @Title, brand_id = @BrandId WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@BrandId", brandId);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteModel(int id)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "DELETE FROM car_model WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public List<LookupItem> GetModelsByBrandId(int brandId)
        {
            var models = new List<LookupItem>();
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "SELECT id, title, brand_id FROM car_model WHERE brand_id = @BrandId";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BrandId", brandId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        models.Add(new LookupItem
                        {
                            Id = reader.GetInt32("id"),
                            Title = reader.GetString("title"),
                        });
                    }
                }
            }

            return models;
        }

        // Методы для работы с body_model
        public List<LookupItem> GetAllBodyModels()
        {
            var bodyModels = new List<LookupItem>();
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "SELECT id, title FROM body_model";

            using (var command = new MySqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    bodyModels.Add(new LookupItem
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("title")
                    });
                }
            }

            return bodyModels;
        }

        public void AddBodyModel(string title)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "INSERT INTO body_model (title) VALUES (@Title)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateBodyModel(int id, string title)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "UPDATE body_model SET title = @Title WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteBodyModel(int id)
        {
            var connection = DataBaseConnector.Instance.GetConnection();
            var query = "DELETE FROM body_model WHERE id = @Id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

    }
}

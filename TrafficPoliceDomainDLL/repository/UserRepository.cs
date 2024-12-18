using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrafficPoliceDomainDLL.model;

namespace TrafficPoliceDomainDLL.repository
{
    public class UserRepository
    {
        public bool AddUser(string username, string password)
        {
            try
            {
                // Устанавливаем соединение
                var connection = DataBaseConnector.Instance.GetConnection();

                // Проверяем, существует ли пользователь с таким же именем
                var checkQuery = "SELECT COUNT(*) FROM user WHERE username = @username;";
                using (var checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@username", username);
                    var existingUserCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (existingUserCount > 0)
                    {
                        MessageBox.Show("Пользователь с таким именем уже существует.");
                        return false;
                    }
                }

                // Генерируем хэш пароля
                string passwordHash = GeneratePasswordHash(password);

                // SQL-запрос для добавления пользователя
                var insertQuery = @"
                    INSERT INTO user (username, password_hash) 
                    VALUES (@username, @password_hash);";

                using (var insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    // Параметры запроса
                    insertCommand.Parameters.AddWithValue("@username", username);
                    insertCommand.Parameters.AddWithValue("@password_hash", passwordHash);

                    // Выполняем запрос
                    var rowsAffected = insertCommand.ExecuteNonQuery();
                    return rowsAffected > 0; // Успешно добавлено, если затронуто хотя бы 1 поле
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении пользователя: {ex.Message}");
                return false;
            }
        }

        // Метод для авторизации пользователя
        public bool AuthenticateUser(string username, string password)
        {
            try
            {
                // Устанавливаем соединение
                var connection = DataBaseConnector.Instance.GetConnection();

                // SQL-запрос для получения хэша пароля
                var query = "SELECT password_hash FROM user WHERE username = @username;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    // Выполняем запрос и получаем хэш пароля
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        string storedPasswordHash = result.ToString();

                        // Генерируем хэш введенного пароля
                        string enteredPasswordHash = GeneratePasswordHash(password);

                        // Сравниваем хэши
                        if (storedPasswordHash == enteredPasswordHash)
                        {
                            Console.WriteLine("Авторизация успешна!");
                            return true;
                        }
                    }

                    Console.WriteLine("Неверное имя пользователя или пароль.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при авторизации: {ex.Message}");
                return false;
            }
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                // Устанавливаем соединение
                var connection = DataBaseConnector.Instance.GetConnection();

                // SQL-запрос для получения текущего хэша пароля
                var query = "SELECT password_hash FROM user WHERE username = @username;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    // Выполняем запрос и получаем хэш пароля
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        string storedPasswordHash = result.ToString();

                        // Генерируем хэш старого введенного пароля
                        string enteredOldPasswordHash = GeneratePasswordHash(oldPassword);

                        // Сравниваем хэши старого пароля
                        if (storedPasswordHash == enteredOldPasswordHash)
                        {
                            // Если старый пароль верный, генерируем хэш для нового пароля
                            string newPasswordHash = GeneratePasswordHash(newPassword);

                            // SQL-запрос для обновления пароля
                            var updateQuery = "UPDATE user SET password_hash = @newPasswordHash WHERE username = @username;";
                            using (var updateCommand = new MySqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@newPasswordHash", newPasswordHash);
                                updateCommand.Parameters.AddWithValue("@username", username);

                                // Выполняем запрос на обновление
                                var rowsAffected = updateCommand.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    Console.WriteLine("Пароль успешно изменен.");
                                    return true; // Успешно изменен пароль
                                }
                                else
                                {
                                    Console.WriteLine("Не удалось изменить пароль.");
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неверный старый пароль.");
                            return false; // Старый пароль неверный
                        }
                    }
                    else
                    {
                        Console.WriteLine("Пользователь с таким именем не найден.");
                        return false; // Пользователь не найден
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при смене пароля: {ex.Message}");
                return false;
            }
        }



        // Метод для генерации хэша пароля
        private string GeneratePasswordHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Конвертируем хэш в строку в формате Hex
                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }

                return hashString.ToString();
            }
        }
    }
}

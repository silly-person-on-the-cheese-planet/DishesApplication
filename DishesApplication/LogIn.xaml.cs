using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;

namespace DishesApplication
{
    public partial class LogIn : Window
    {
        private string connectionString = "Server=desktop-uijbk3u;Database=My;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

        public LogIn()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            string login = UserLoginTextBox.Text;
            string password = UserPasswordBox.Password;

            if (AuthenticateUser(login, password))
            {
                int roleId = GetUserRoleId(login);
                string role = GetRoleName(roleId);
                string userSurname = GetUserSurname(login);
                string userName = GetUserName(login);
                string userPatronymic = GetUserPatronymic(login);
                OpenCatalogWindow(role, userSurname, userName, userPatronymic);
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool AuthenticateUser(string login, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM [dbo].[User] WHERE CAST([UserLogin] AS NVARCHAR(MAX)) = @UserLogin AND CAST([UserPassword] AS NVARCHAR(MAX)) = @UserPassword";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserLogin", login);
                        command.Parameters.AddWithValue("@UserPassword", password);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при авторизации: " + ex.Message);
                return false;
            }
        }

        private int GetUserRoleId(string login)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT [UserRole] FROM [dbo].[User] WHERE CAST([UserLogin] AS NVARCHAR(MAX)) = @UserLogin";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserLogin", login);
                        var result = command.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении роли пользователя: " + ex.Message);
                return 0;
            }
        }

        private string GetRoleName(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    return "Администратор";
                case 2:
                    return "Менеджер";
                case 3:
                    return "Клиент";
                default:
                    return string.Empty;
            }
        }

        private string GetUserSurname(string login)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT [UserSurname] FROM [dbo].[User] WHERE CAST([UserLogin] AS NVARCHAR(MAX)) = @UserLogin";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserLogin", login);
                        var result = command.ExecuteScalar();
                        return result?.ToString() ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении фамилии пользователя: " + ex.Message);
                return string.Empty;
            }
        }

        private string GetUserName(string login)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT [UserName] FROM [dbo].[User] WHERE CAST([UserLogin] AS NVARCHAR(MAX)) = @UserLogin";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserLogin", login);
                        var result = command.ExecuteScalar();
                        return result?.ToString() ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении имени пользователя: " + ex.Message);
                return string.Empty;
            }
        }

        private string GetUserPatronymic(string login)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT [UserPatronymic] FROM [dbo].[User] WHERE CAST([UserLogin] AS NVARCHAR(MAX)) = @UserLogin";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserLogin", login);
                        var result = command.ExecuteScalar();
                        return result?.ToString() ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении отчества пользователя: " + ex.Message);
                return string.Empty;
            }
        }

        private void OpenCatalogWindow(string role, string userSurname, string userName, string userPatronymic)
        {
            switch (role)
            {
                case "Менеджер":
                case "Клиент":
                    CatalogAuthorized catalogAuthorized = new CatalogAuthorized(userSurname, userName, userPatronymic);
                    catalogAuthorized.Show();
                    Close();
                    break;
                case "Администратор":
                    CatalogAdministrator catalogAdministrator = new CatalogAdministrator(userSurname, userName, userPatronymic);
                    catalogAdministrator.Show();
                    Close();
                    break;
                default:
                    MessageBox.Show("Неизвестная роль пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            CatalogNonAuthorized catalogNonAuthorized = new CatalogNonAuthorized();
            catalogNonAuthorized.Show();
            Close();
        }
    }
}

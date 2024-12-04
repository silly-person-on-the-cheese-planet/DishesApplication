using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DishesApplication
{
    public partial class LogIn : Window
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
        private bool captchaRequired = false;
        private string captchaText;

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

        private async void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            string login = UserLoginTextBox.Text;
            string password = UserPasswordBox.Password;

            if (captchaRequired)
            {
                if (CaptchaTextBox.Text != captchaText)
                {
                    MessageBox.Show("Неверная CAPTCHA. Ожидалось: " + captchaText, "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    await LockInputFields(10000); // Блокировка на 10 секунд
                    return;
                }
            }

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
                if (!captchaRequired)
                {
                    captchaRequired = true;
                    GenerateCaptcha();
                    CaptchaLabel.Visibility = Visibility.Visible;
                    CaptchaTextBox.Visibility = Visibility.Visible;
                    CaptchaBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    await LockInputFields(10000); // Блокировка на 10 секунд
                }
            }
        }

        private async Task LockInputFields(int delayMillis)
        {
            UserLoginTextBox.Background = Brushes.LightGray;
            UserPasswordBox.Background = Brushes.LightGray;
            CaptchaTextBox.Background = Brushes.LightGray;
            UserLoginTextBox.Text = "";
            UserPasswordBox.Password = "";
            CaptchaTextBox.Text = "";
            UserLoginTextBox.IsReadOnly = true;
            UserPasswordBox.Visibility = Visibility.Collapsed;
            UserPasswordTextBoxReadOnly.Visibility = Visibility.Visible;
            UserPasswordTextBoxReadOnly.Text = UserPasswordBox.Password;
            CaptchaTextBox.IsReadOnly = true;
            await Task.Delay(delayMillis);
            UserLoginTextBox.Background = Brushes.White;
            UserPasswordBox.Background = Brushes.White;
            CaptchaTextBox.Background = Brushes.White;
            UserLoginTextBox.IsReadOnly = false;
            UserPasswordBox.Visibility = Visibility.Visible;
            UserPasswordTextBoxReadOnly.Visibility = Visibility.Collapsed;
            CaptchaTextBox.IsReadOnly = false;
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

        private void GenerateCaptcha()
        {
            Random random = new Random();
            captchaText = new string(Enumerable.Range(0, 4).Select(i => random.Next(0, 2) == 0 ? (char)random.Next('0', '9' + 1) : (char)random.Next('A', 'Z' + 1)).ToArray());

            CaptchaCanvas.Children.Clear();

            double xOffset = 0;
            double yOffset = 0;

            for (int i = 0; i < captchaText.Length; i++)
            {
                TextBlock textBlock = new TextBlock
                {
                    Text = captchaText[i].ToString(),
                    FontSize = 24,
                    Foreground = new SolidColorBrush(Colors.Black),
                    Margin = new Thickness(0, 0, 0, 80) // Добавление отступов, чтобы символы не выходили за границы
                };

                Canvas.SetLeft(textBlock, xOffset);
                Canvas.SetTop(textBlock, yOffset + random.Next(-5, 0)); // Смещение по вертикали для создания эффекта волны
                CaptchaCanvas.Children.Add(textBlock);

                xOffset += 25; // Смещение по горизонтали для следующего символа
            }

            // Добавление графического шума
            for (int j = 0; j < 30; j++)
            {
                Line line = new Line
                {
                    Stroke = new SolidColorBrush(Colors.Gray),
                    X1 = random.Next(0, (int)CaptchaCanvas.Width),
                    Y1 = random.Next(0, (int)CaptchaCanvas.Height),
                    X2 = random.Next(0, (int)CaptchaCanvas.Width),
                    Y2 = random.Next(0, (int)CaptchaCanvas.Height),
                    StrokeThickness = 1
                };
                CaptchaCanvas.Children.Add(line);
            }
        }
    }
}

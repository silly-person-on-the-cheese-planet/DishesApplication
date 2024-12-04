using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;

namespace DishesApplication
{
    public partial class ProductAddForm : Window
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
        public string productPhotoPath;

        public ProductAddForm()
        {
            InitializeComponent();
            productPhotoPath = "/picture.png";
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
            DialogResult = false;
        }

        private async void RegButton_Click(object sender, RoutedEventArgs e)
        {
            if (await AddProductToDatabaseAsync())
            {
                MessageBox.Show("Товар успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении товара.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddPhotoToProductButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Image Files (*.png;*.jpg)|*.png;*.jpg",
                Title = "Выберите изображение профиля"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                productPhotoPath = openFileDialog.FileName;
                var bitmap = new BitmapImage(new Uri(productPhotoPath));
                ImageProduct.Source = bitmap;
            }
        }

        public async Task<bool> AddProductToDatabaseAsync()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = "INSERT INTO [dbo].[Product] ([ProductArticleNumber], [ProductName], [ProductDescription], [ProductCategory], [ProductPhoto], [ProductManufacturer], [ProductCost], [ProductDiscountAmount], [ProductQuantityInStock], [ProductStatus]) " +
                                   "VALUES (@ProductArticleNumber, @ProductName, @ProductDescription, @ProductCategory, @ProductPhoto, @ProductManufacturer, @ProductCost, @ProductDiscountAmount, @ProductQuantityInStock, @ProductStatus)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductArticleNumber", ProductArticleNumberTextBox.Text);
                        command.Parameters.AddWithValue("@ProductName", ProductNameTextBox.Text);
                        command.Parameters.AddWithValue("@ProductDescription", ProductDescriptionTextBox.Text);
                        command.Parameters.AddWithValue("@ProductCategory", ProductCategoryTextBox.Text);
                        command.Parameters.AddWithValue("@ProductPhoto", productPhotoPath);
                        command.Parameters.AddWithValue("@ProductManufacturer", ProductManufacturerTextBox.Text);
                        command.Parameters.AddWithValue("@ProductCost", decimal.Parse(ProductCostTextBox.Text));
                        command.Parameters.AddWithValue("@ProductDiscountAmount", 0); // Set discount amount to 0
                        command.Parameters.AddWithValue("@ProductQuantityInStock", int.Parse(ProductQuantityInStockTextBox.Text));
                        command.Parameters.AddWithValue("@ProductStatus", "Да"); // Assuming the product is available

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении товара: " + ex.Message);
                return false;
            }
        }
    }
}

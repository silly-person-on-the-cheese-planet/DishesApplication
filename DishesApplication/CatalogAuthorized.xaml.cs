using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Data.SqlClient;

namespace DishesApplication
{
    public partial class CatalogAuthorized : Window
    {
        private string connectionString = "Server=desktop-uijbk3u;Database=My;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
        private List<Product> products;

        public CatalogAuthorized()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT [ProductArticleNumber], [ProductName], [ProductDescription], [ProductCategory], [ProductPhoto], [ProductManufacturer], [ProductCost], [ProductDiscountAmount], [ProductQuantityInStock], [ProductStatus] FROM [dbo].[Product]";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    products = new List<Product>();

                    while (reader.Read())
                    {
                        string productName = reader.IsDBNull(reader.GetOrdinal("ProductName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProductName"));
                        string productDescription = reader.IsDBNull(reader.GetOrdinal("ProductDescription")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProductDescription"));
                        string productCategory = reader.IsDBNull(reader.GetOrdinal("ProductCategory")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProductCategory"));
                        string productPhoto = reader.IsDBNull(reader.GetOrdinal("ProductPhoto")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProductPhoto"));
                        string productManufacturer = reader.IsDBNull(reader.GetOrdinal("ProductManufacturer")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProductManufacturer"));
                        decimal productCost = reader.IsDBNull(reader.GetOrdinal("ProductCost")) ? 0 : reader.GetDecimal(reader.GetOrdinal("ProductCost"));
                        int productQuantityInStock = reader.IsDBNull(reader.GetOrdinal("ProductQuantityInStock")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProductQuantityInStock"));
                        string productStatus = reader.IsDBNull(reader.GetOrdinal("ProductStatus")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProductStatus"));

                        products.Add(new Product
                        {
                            ProductName = productName,
                            ProductDescription = productDescription,
                            ProductCategory = productCategory,
                            ProductPhoto = productPhoto,
                            ProductManufacturer = productManufacturer,
                            ProductCost = productCost,
                            ProductQuantityInStock = productQuantityInStock,
                            ProductStatus = productStatus
                        });
                    }

                    DisplayProducts(products);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void DisplayProducts(List<Product> products)
        {
            ProductStackPanel.Children.Clear();

            foreach (var product in products)
            {
                // Создание элементов управления для отображения данных
                Border productBorder = new Border
                {
                    Height = 250,
                    BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF4B8C4F"),
                    BorderThickness = new Thickness(2),
                    Margin = new Thickness(0, 0, 0, 50)
                };

                Grid productGrid = new Grid();
                productGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(251, GridUnitType.Star) });
                productGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(444, GridUnitType.Star) });
                productGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70, GridUnitType.Star) });

                Border imageBorder = new Border
                {
                    Margin = new Thickness(10),
                    BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF4B8C4F"),
                    BorderThickness = new Thickness(2)
                };

                // Установка изображения или картинки-заполнителя
                Image productImage = new Image();
                if (string.IsNullOrEmpty(product.ProductPhoto))
                {
                    productImage.Source = new BitmapImage(new Uri("picture.png", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    productImage.Source = new BitmapImage(new Uri(product.ProductPhoto, UriKind.RelativeOrAbsolute));
                }

                imageBorder.Child = productImage;
                Grid.SetColumn(imageBorder, 0);
                productGrid.Children.Add(imageBorder);

                StackPanel textStackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(10)
                };
                TextBlock productNameTextBlock = new TextBlock
                {
                    Text = product.ProductName,
                    FontSize = 20,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                TextBlock productDescriptionTextBlock = new TextBlock
                {
                    Text = product.ProductDescription,
                    FontSize = 16,
                    Margin = new Thickness(0, 0, 0, 15),
                    Foreground = (Brush)new BrushConverter().ConvertFrom("#FF4A4A4A")
                };
                StackPanel manufacturerStackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                TextBlock manufacturerLabel = new TextBlock
                {
                    Text = "Производитель:",
                    FontSize = 16,
                    Margin = new Thickness(0, 0, 10, 0)
                };
                TextBlock productManufacturerTextBlock = new TextBlock
                {
                    Text = product.ProductManufacturer,
                    FontSize = 16
                };
                manufacturerStackPanel.Children.Add(manufacturerLabel);
                manufacturerStackPanel.Children.Add(productManufacturerTextBlock);

                StackPanel costStackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                TextBlock costLabel = new TextBlock
                {
                    Text = "Цена:",
                    FontSize = 20,
                    Margin = new Thickness(0, 0, 10, 0)
                };
                TextBlock productCostTextBlock = new TextBlock
                {
                    Text = product.ProductCost.ToString("C"),
                    FontSize = 20
                };
                costStackPanel.Children.Add(costLabel);
                costStackPanel.Children.Add(productCostTextBlock);

                textStackPanel.Children.Add(productNameTextBlock);
                textStackPanel.Children.Add(productDescriptionTextBlock);
                textStackPanel.Children.Add(manufacturerStackPanel);
                textStackPanel.Children.Add(costStackPanel);
                Grid.SetColumn(textStackPanel, 1);
                productGrid.Children.Add(textStackPanel);

                // Изменение надписи "Наличие на складе" в зависимости от значения колонки ProductStatus
                TextBlock availabilityTextBlock = new TextBlock
                {
                    Text = product.ProductStatus.ToLower() == "да" ? "Есть в наличии" : "Нет в наличии",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 14,
                    TextWrapping = TextWrapping.WrapWithOverflow
                };
                Grid.SetColumn(availabilityTextBlock, 2);
                productGrid.Children.Add(availabilityTextBlock);

                Border basketBorder = new Border
                {
                    Margin = new Thickness(5),
                    VerticalAlignment = VerticalAlignment.Bottom
                };
                Image basketImage = new Image
                {
                    Source = new BitmapImage(new Uri("/icon-basket.png", UriKind.RelativeOrAbsolute))
                };
                basketImage.MouseDown += new MouseButtonEventHandler(basketImageButton_MouseDown);
                basketBorder.Child = basketImage;
                Grid.SetColumn(basketBorder, 2);
                productGrid.Children.Add(basketBorder);

                productBorder.Child = productGrid;
                ProductStackPanel.Children.Add(productBorder);
            }
        }

        private void SearchProductsByDescription(string searchText)
        {
            var filteredProducts = products.Where(p => p.ProductDescription.ToLower().Contains(searchText.ToLower())).ToList();
            DisplayProducts(filteredProducts);
        }

        private void SearchProductsByCategory(string searchText)
        {
            var filteredProducts = products.Where(p => p.ProductCategory.ToLower().Contains(searchText.ToLower())).ToList();
            DisplayProducts(filteredProducts);
        }

        private void SortProductsByPriceAscending()
        {
            var sortedProducts = products.OrderBy(p => p.ProductCost).ToList();
            DisplayProducts(sortedProducts);
        }

        private void SortProductsByPriceDescending()
        {
            var sortedProducts = products.OrderByDescending(p => p.ProductCost).ToList();
            DisplayProducts(sortedProducts);
        }

        private void ProductFindImageButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchProductsByDescription(ProductFindTextBox.Text);
        }

        private void CategoryFindImageButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchProductsByCategory(CategoryFindTextBox.Text);
        }

        private void ProductFindTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchProductsByDescription(ProductFindTextBox.Text);
            }
        }

        private void CategoryFindTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchProductsByCategory(CategoryFindTextBox.Text);
            }
        }

        private void SortyrovkaUpButton_Click(object sender, RoutedEventArgs e)
        {
            SortyrovkaUpBorder.Visibility = Visibility.Visible;
            SortyrovkaDownBorder.Visibility = Visibility.Collapsed;
            SortProductsByPriceAscending();
            
        }

        private void SortyrovkaDownButton_Click(object sender, RoutedEventArgs e)
        {
            SortyrovkaUpBorder.Visibility = Visibility.Collapsed;
            SortyrovkaDownBorder.Visibility = Visibility.Visible;
            SortProductsByPriceDescending();
            
        }

        private void basketImageButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Логика добавления товара в корзину
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LogIn logIn = new(); logIn.Show(); Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BasketForm basketForm = new(); basketForm.ShowDialog();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CategoryFindTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CategoryFindTextBox.Text == "Категория" && CategoryFindTextBox.Foreground == Brushes.Gray)
            {
                CategoryFindTextBox.Text = "";
                CategoryFindTextBox.Foreground = Brushes.Black;
            }
        }

        private void CategoryFindTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CategoryFindTextBox.Text == "")
            {
                CategoryFindTextBox.Foreground = Brushes.Gray;
                CategoryFindTextBox.Text = "Категория";
            }
        }

        private void ProductFindTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ProductFindTextBox.Text == "Поиск нужного товара" && ProductFindTextBox.Foreground == Brushes.Gray)
            {
                ProductFindTextBox.Text = "";
                ProductFindTextBox.Foreground = Brushes.Black;
            }
        }

        private void ProductFindTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ProductFindTextBox.Text == "")
            {
                ProductFindTextBox.Foreground = Brushes.Gray;
                ProductFindTextBox.Text = "Поиск нужного товара";
            }
        }
    }

    public class Product
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }
        public string ProductPhoto { get; set; }
        public string ProductManufacturer { get; set; }
        public decimal ProductCost { get; set; }
        public int ProductQuantityInStock { get; set; }
        public string ProductStatus { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DishesApplication
{
    public partial class CatalogAuthorized : Window
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
        private List<Product> products;
        private List<BasketItem> basketItems = new List<BasketItem>();
        private List<string> manufacturers;
        private string selectedManufacturer = "Все производители";
        private string searchText = string.Empty;

        public CatalogAuthorized(string userSurname, string userName, string userPatronymic)
        {
            InitializeComponent();
            UserSurnameTextBlock.Text = userSurname;
            UserNameTextBlock.Text = userName;
            UserPatronymicTextBlock.Text = userPatronymic;
            LoadProducts();
            LoadManufacturers();
            LoadCategories();
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
                        string productArticleNumber = reader.IsDBNull(reader.GetOrdinal("ProductArticleNumber")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProductArticleNumber"));
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
                            ProductArticleNumber = productArticleNumber,
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
                    UpdateDisplayedItemsCount(products.Count, products.Count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void LoadManufacturers()
        {
            manufacturers = products.Select(p => p.ProductManufacturer).Distinct().ToList();
            manufacturers.Insert(0, "Все производители");
            ManufacturerFilterComboBox.ItemsSource = manufacturers;
            ManufacturerFilterComboBox.SelectedIndex = 0;
        }

        private void LoadCategories()
        {
            var categories = products.Select(p => p.ProductCategory).Distinct().ToList();
            categories.Insert(0, "Все категории");
            CategoryFindComboBox.ItemsSource = categories;
            CategoryFindComboBox.SelectedIndex = 0;
        }

        private void DisplayProducts(List<Product> products)
        {
            ProductStackPanel.Children.Clear();

            foreach (var product in products)
            {
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

                TextBlock availabilityTextBlock = new TextBlock
                {
                    Text = product.ProductStatus.ToLower() == "да" ? "Есть в наличии" : "Нет в наличии",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 14,
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    Margin = new Thickness(0, 0, 0, 70)
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
                basketImage.MouseDown += (sender, e) => AddToBasket(product);
                basketBorder.Child = basketImage;
                Grid.SetColumn(basketBorder, 2);
                productGrid.Children.Add(basketBorder);

                productBorder.Child = productGrid;
                ProductStackPanel.Children.Add(productBorder);
            }
        }

        private void AddToBasket(Product product)
        {
            basketItems.Add(new BasketItem
            {
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPhoto = product.ProductPhoto,
                ProductManufacturer = product.ProductManufacturer,
                ProductCost = product.ProductCost
            });
            MessageBox.Show("Товар добавлен в корзину", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SearchProductsByDescription(string searchText)
        {
            string selectedCategory = CategoryFindComboBox.SelectedItem?.ToString() ?? "Все категории";
            var filteredProducts = products.Where(p => p.ProductDescription.ToLower().Contains(searchText.ToLower()) &&
                                                      (selectedManufacturer == "Все производители" || p.ProductManufacturer == selectedManufacturer) &&
                                                      (selectedCategory == "Все категории" || p.ProductCategory == selectedCategory)).ToList();
            DisplayProducts(filteredProducts);
            UpdateDisplayedItemsCount(filteredProducts.Count, products.Count);
        }

        private void SearchProductsByCategory(string searchText)
        {
            var categories = products.Select(p => p.ProductCategory).Distinct().ToList();
            if (categories.Contains(searchText, StringComparer.OrdinalIgnoreCase))
            {
                var filteredProducts = products.Where(p => p.ProductCategory.ToLower().Contains(searchText.ToLower()) && (selectedManufacturer == "Все производители" || p.ProductManufacturer == selectedManufacturer)).ToList();
                DisplayProducts(filteredProducts);
                UpdateDisplayedItemsCount(filteredProducts.Count, products.Count);
            }
            else
            {
                MessageBox.Show("Неправильная категория. Возможные категории:\n" + string.Join("\n", categories), "Неправильная категория", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SortProductsByPriceAscending()
        {
            string selectedCategory = CategoryFindComboBox.SelectedItem?.ToString() ?? "Все категории";
            var sortedProducts = products.Where(p => (selectedManufacturer == "Все производители" || p.ProductManufacturer == selectedManufacturer) &&
                                                     p.ProductDescription.ToLower().Contains(searchText.ToLower()) &&
                                                     (selectedCategory == "Все категории" || p.ProductCategory == selectedCategory)).OrderBy(p => p.ProductCost).ToList();
            DisplayProducts(sortedProducts);
        }

        private void SortProductsByPriceDescending()
        {
            string selectedCategory = CategoryFindComboBox.SelectedItem?.ToString() ?? "Все категории";
            var sortedProducts = products.Where(p => (selectedManufacturer == "Все производители" || p.ProductManufacturer == selectedManufacturer) &&
                                                     p.ProductDescription.ToLower().Contains(searchText.ToLower()) &&
                                                     (selectedCategory == "Все категории" || p.ProductCategory == selectedCategory)).OrderByDescending(p => p.ProductCost).ToList();
            DisplayProducts(sortedProducts);
        }

        private void ProductFindImageButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchProductsByDescription(ProductFindTextBox.Text);
        }

        private void ProductFindTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchProductsByDescription(ProductFindTextBox.Text);
            }
        }

        private void SortyrovkaUpButton_Click(object sender, RoutedEventArgs e)
        {
            SortProductsByPriceAscending();
            SortyrovkaDownBorder.Visibility = Visibility.Visible;
            SortyrovkaUpBorder.Visibility = Visibility.Collapsed;
        }

        private void SortyrovkaDownButton_Click(object sender, RoutedEventArgs e)
        {
            SortProductsByPriceDescending();
            SortyrovkaDownBorder.Visibility = Visibility.Collapsed;
            SortyrovkaUpBorder.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LogIn logIn = new LogIn();
            logIn.Show();
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
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

        private void ManufacturerFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedManufacturer = ManufacturerFilterComboBox.SelectedItem as string;
            SearchProductsByDescription(searchText);
        }

        private void UpdateDisplayedItemsCount(int displayedCount, int totalCount)
        {
            DisplayedItemsTextBlock.Text = $"{displayedCount} из {totalCount}";
        }

        private void basketImageButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BasketForm basketForm = new BasketForm(basketItems);
            basketForm.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BasketForm basketForm = new BasketForm(basketItems);
            basketForm.ShowDialog();
        }

        private void AddProductImageButton_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void CategoryFindComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCategory = CategoryFindComboBox.SelectedItem?.ToString() ?? "Все категории";
            var filteredProducts = products.Where(p => (selectedCategory == "Все категории" || p.ProductCategory == selectedCategory) &&
                                                      (selectedManufacturer == "Все производители" || p.ProductManufacturer == selectedManufacturer) &&
                                                      p.ProductDescription.ToLower().Contains(searchText.ToLower())).ToList();
            DisplayProducts(filteredProducts);
            UpdateDisplayedItemsCount(filteredProducts.Count, products.Count);
        }
    }
}

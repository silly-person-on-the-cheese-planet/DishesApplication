using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DishesApplication
{
    public partial class BasketForm : Window
    {
        public List<BasketItem> basketItems;

        public BasketForm(List<BasketItem> basketItems)
        {
            InitializeComponent();
            this.basketItems = basketItems;
            DisplayBasketItems();
        }

        public void AddToBasket(Product product)
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

        private void DisplayBasketItems()
        {
            StackPanel productStackPanel = (StackPanel)FindName("ProductStackPanel");
            productStackPanel.Children.Clear();

            foreach (var item in basketItems)
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
                if (string.IsNullOrEmpty(item.ProductPhoto))
                {
                    productImage.Source = new BitmapImage(new Uri("picture.png", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    productImage.Source = new BitmapImage(new Uri(item.ProductPhoto, UriKind.RelativeOrAbsolute));
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
                    Text = item.ProductName,
                    FontSize = 20,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                TextBlock productDescriptionTextBlock = new TextBlock
                {
                    Text = item.ProductDescription,
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
                    Text = item.ProductManufacturer,
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
                    Text = item.ProductCost.ToString("C"),
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

                Border cancelBorder = new Border
                {
                    Margin = new Thickness(5),
                    VerticalAlignment = VerticalAlignment.Bottom
                };
                Image cancelImage = new Image
                {
                    Source = new BitmapImage(new Uri("/icon-minus-sign-of-a-line-in-horizontal-position.png", UriKind.RelativeOrAbsolute))
                };
                cancelImage.MouseDown += (sender, e) => RemoveFromBasket(item);
                cancelBorder.Child = cancelImage;
                Grid.SetColumn(cancelBorder, 2);
                productGrid.Children.Add(cancelBorder);

                productBorder.Child = productGrid;
                productStackPanel.Children.Add(productBorder);
            }

            UpdateTotalCost();
        }

        public void RemoveFromBasket(BasketItem item)
        {
            basketItems.Remove(item);
            DisplayBasketItems();
        }

        public void UpdateTotalCost()
        {
            decimal totalCost = basketItems.Sum(item => item.ProductCost);
            EndCostOrderTextBlock.Text = totalCost.ToString("C");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (basketItems.Count == 0)
            {
                MessageBox.Show("В корзине нет товаров.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Успешно!", "Заказ оформлен", MessageBoxButton.OK, MessageBoxImage.Information);
                basketItems.Clear();
                DisplayBasketItems();
                DialogResult = true;
            }
        }

        private void CancelProductItemButton_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}

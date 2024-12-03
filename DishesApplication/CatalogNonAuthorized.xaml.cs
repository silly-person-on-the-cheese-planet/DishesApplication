using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DishesApplication
{
    /// <summary>
    /// Логика взаимодействия для CatalogNonAuthorized.xaml
    /// </summary>
    public partial class CatalogNonAuthorized : Window
    {
        public CatalogNonAuthorized()
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LogIn logIn = new(); logIn.Show(); Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Войдите в аккаунт!", "Режим гостя", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void basketImageButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Войдите в аккаунт!", "Режим гостя", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SortyrovkaUpButton_Click(object sender, RoutedEventArgs e)
        {
            SortyrovkaDownBorder.Visibility = Visibility.Visible;
            SortyrovkaUpBorder.Visibility = Visibility.Collapsed;
        }

        private void SortyrovkaDownButton_Click(object sender, RoutedEventArgs e)
        {
            SortyrovkaUpBorder.Visibility = Visibility.Visible;
            SortyrovkaDownBorder.Visibility = Visibility.Collapsed;
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
}

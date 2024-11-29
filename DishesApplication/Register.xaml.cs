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
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
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

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            CatalogAuthorized catalogAuthorized = new(); catalogAuthorized.Show(); Close();
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            CatalogNonAuthorized catalogNonAuthorized = new(); catalogNonAuthorized.Show(); Close();
        }
    }
}

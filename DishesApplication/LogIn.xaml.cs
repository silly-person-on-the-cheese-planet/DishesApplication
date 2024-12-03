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
    /// Логика взаимодействия для LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
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
            
            CatalogAuthorized catalogAuthorized = new(); catalogAuthorized.Show();
            CatalogAdministrator catalogAdministrator = new(); catalogAdministrator.Show(); Close();
            
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            CatalogNonAuthorized catalogNonAuthorized = new(); catalogNonAuthorized.Show(); Close();
        }
    }
}

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
            Register register = new(); register.Show();
            CatalogAuthorized catalogAuthorized = new(); catalogAuthorized.Show();
            CatalogNonAuthorized catalogNonAuthorized = new(); catalogNonAuthorized.Show();
            CatalogAdministrator catalogAdministrator = new(); catalogAdministrator.Show();
            BasketForm basketForm = new(); basketForm.Show();
            ProductAddForm productAddForm = new(); productAddForm.Show();
            ProductRedactForm productRedactForm = new(); productRedactForm.Show();
            ProductCardForm productCardForm = new(); productCardForm.Show();
        }
    }
}

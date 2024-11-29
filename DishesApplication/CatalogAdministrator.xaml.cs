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
    /// Логика взаимодействия для CatalogAdministrator.xaml
    /// </summary>
    public partial class CatalogAdministrator : Window
    {
        public CatalogAdministrator()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ProductAddForm productAddForm = new(); productAddForm.ShowDialog();
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            ProductRedactForm productRedactForm = new(); productRedactForm.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LogIn logIn = new(); logIn.Show(); Close();
        }
    }
}

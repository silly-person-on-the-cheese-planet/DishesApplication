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
    /// Логика взаимодействия для BasketForm.xaml
    /// </summary>
    public partial class BasketForm : Window
    {
        public BasketForm()
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

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult= true;
        }
    }
}

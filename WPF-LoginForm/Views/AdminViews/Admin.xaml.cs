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
using WPF_LoginForm.Views.DoctorViews;

namespace WPF_LoginForm.Views.AdminViews
{
    /// <summary>
    /// Logika interakcji dla klasy Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void deleteUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editUser_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Get ID
            //string idText = appoinmentIdTextBox.Text;
            int id;

            if (true)
            {
                //TODO: ID VERYFICATION
                //TODO: GET TEST DATA AND SEND IT TO THE NEXT SCREEN
                //TODO: Make Singleton out of next screen
                AdminEditUser nextScreen = new AdminEditUser();
                nextScreen.Show();
            }
            else
            {
                //TODO SHOW SOME ERROR OR CREATE PROPER VALIDTION ON THIS FIELD
            }
        }

        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            

            if (true)
            {
                //TODO: Make Singleton out of next screen
                AdminEditUser nextScreen = new AdminEditUser();
                nextScreen.Show();
            }
            else
            {
                //TODO SHOW SOME ERROR OR CREATE PROPER VALIDTION ON THIS FIELD
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}

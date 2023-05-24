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
using WPF_LoginForm.Views.LabManagerViews;

namespace WPF_LoginForm.Views.ReceptionistViews
{
    /// <summary>
    /// Logika interakcji dla klasy Receptionist.xaml
    /// </summary>
    public partial class Receptionist : Window
    {
        public Receptionist()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cancelAppoinmentBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void registerVisitBtn_Click(object sender, RoutedEventArgs e)
        {
            //Get ID
            
            int id;


            if (true)
            {   //TODO: ID VERYFICATION
                //TODO: GET TEST DATA AND SEND IT TO THE NEXT SCREEN
                //TODO: Make Singleton out of next screen
                ReceptionistRegister nextScreen = new ReceptionistRegister();
                nextScreen.Show();
            }
            else
            {
                //TODO: SHOW SOME ERROR OR CREATE PROPER VALIDTION ON THIS FIELD
            }
        }
    }
}

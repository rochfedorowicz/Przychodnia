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
using WPF_LoginForm.Views;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// Logika interakcji dla klasy Lab_Assistant.xaml
    /// </summary>
    public partial class Lab_Assistant : Window
    {
        public Lab_Assistant()
        {
            InitializeComponent();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void doTest_Click(object sender, RoutedEventArgs e)
        {
            //Get ID
            string idText = testIdBox.Text;
            int id;

            if (int.TryParse(idText, out id))
            {
                //TODO GET TEST DATA AND SEND IT TO THE NEXT SCREEN
                MakeTest nextScreen = new MakeTest();
                nextScreen.Show();
            }
            else
            {
                //TODO SHOW SOME ERROR OR CREATE PROPER VALIDTION ON THIS FIELD
            }
        }
    }
}

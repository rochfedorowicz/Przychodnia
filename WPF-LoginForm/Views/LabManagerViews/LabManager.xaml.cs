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
using WPF_LoginForm.Views.LabAssistantViews;

namespace WPF_LoginForm.Views.LabManagerViews
{
    /// <summary>
    /// Logika interakcji dla klasy Lab_Manager.xaml
    /// </summary>
    public partial class LabManager : Window
    {
        public LabManager()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void checkLabTestBtn_Click(object sender, RoutedEventArgs e)
        {
            //Get ID
            string idText = testIdTextBox.Text;
            int id;

            
            if (int.TryParse(idText, out id))
            {   //TODO: ID VERYFICATION
                //TODO: GET TEST DATA AND SEND IT TO THE NEXT SCREEN
                //TODO: Make Singleton out of next screen
                LabManagerApprove nextScreen = new LabManagerApprove();
                nextScreen.Show();
            }
            else
            {
                //TODO: SHOW SOME ERROR OR CREATE PROPER VALIDTION ON THIS FIELD
            }
        }
    }
}

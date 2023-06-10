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

namespace WPF_LoginForm.Views.DoctorViews
{
    /// <summary>
    /// Logika interakcji dla klasy LabTest.xaml
    /// </summary>
    public partial class LabTest : Window
    {
        public LabTest(string nameOfLabTest, string result, string remarks)
        {
            InitializeComponent();
            this.nameOfLabTest.Content = nameOfLabTest;
            this.result.Text = result;
            this.remarks.Text = remarks;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void orderTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

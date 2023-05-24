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
    /// Logika interakcji dla klasy InterviewWithThePatient.xaml
    /// </summary>
    public partial class InterviewWithThePatient : Window
    {
        public InterviewWithThePatient()
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
            this.Close();
        }

        private void cancelAppointmentBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void orderLabTestBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void doPhysicalExaminationBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

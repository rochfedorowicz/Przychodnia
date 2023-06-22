using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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
using WPF_LoginForm.Model;
using WPF_LoginForm.Views.LabManagerViews;

namespace WPF_LoginForm.Views.ReceptionistViews
{
    /// <summary>
    /// Logika interakcji dla klasy Receptionist.xaml
    /// </summary>
    public partial class Receptionist : Window
    {
        private ClinicEntities contextDB;

        public ClinicEntities getContextDB()
        {
            return contextDB;
        }
        public Receptionist()
        {
            InitializeComponent();
            contextDB = new ClinicEntities();
            contextDB.Appointments.Load();
            receptionTable.ItemsSource = contextDB.Appointments.Local;

            checkBoxRegistered.IsChecked = true;
            checkBoxToday.IsChecked = true;
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

        private void cancelAppoinmentBtn_Click(object sender, RoutedEventArgs e)
        {
            Appointment selectedAppointment = (Appointment)receptionTable.SelectedItem;

            selectedAppointment.Status = 3;
            contextDB.SaveChanges();
            refreshView();
            filterAppoitments(null,null);
        }

        private void addPatientBtn_Click(object sender, RoutedEventArgs e)
        {
            NewPatient nextScreen = new NewPatient();
            nextScreen.Show();
        }

        private void registerVisitBtn_Click(object sender, RoutedEventArgs e)
        {
            ReceptionistRegister nextScreen = new ReceptionistRegister(this);
            nextScreen.Show();
        }

        public void refreshView()
        {
            receptionTable.UnselectAll();
            ICollectionView view = CollectionViewSource.GetDefaultView(contextDB.Appointments.Local);
            view.Refresh();
            filterAppoitments(null, null);
        }
        private void receptionTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (receptionTable.SelectedItem != null && ((Appointment)receptionTable.SelectedItem).Status == 1)
            {
                enableButtons();
            }
            else
            {
                disableButtons();
            }
        }
        private void disableButtons()
        {
            cancelAppoinmentBtn.IsEnabled = false;
        }
        private void enableButtons()
        {
            cancelAppoinmentBtn.IsEnabled = true;
        }
        private void filterAppoitments(object sender, RoutedEventArgs e)
        {
            bool appoitmentRegistered = checkBoxRegistered.IsChecked == true;
            bool appoitmentFinalized = checkBoxDone.IsChecked == true;
            bool appoitmentCanceled = checkBoxCanceled.IsChecked == true;
            bool onlyTodayAppoinments = checkBoxToday.IsChecked == true;

            DateTime today = DateTime.Today;
            DateTime startDate = today.Date;
            DateTime endDate = today.Date.AddDays(1).AddTicks(-1);

            if (appoitmentRegistered || appoitmentFinalized || appoitmentCanceled)
            {
                var appoitments = contextDB.Appointments.Where(el =>
                    el.Patient.Name.StartsWith(pNameTextBox.Text) &&
                    el.Patient.Surname.StartsWith(pSurameTextBox.Text) &&
                    el.Patient.Pesel.StartsWith(pPeselTextBox.Text) &&
                    el.Doctor.Name.StartsWith(dNameTextBox.Text) &&
                    el.Doctor.Surname.StartsWith(dSurameTextBox.Text) &&
                    (onlyTodayAppoinments ? el.Reg_date >= startDate && el.Reg_date <= endDate : true) &&
                    (
                        (el.AppointmentState.Id_state == 1 && appoitmentRegistered) ||
                        (el.AppointmentState.Id_state == 2 && appoitmentFinalized) ||
                        (el.AppointmentState.Id_state == 3 && appoitmentCanceled)
                    )).ToList();

                receptionTable.ItemsSource = appoitments;
            }
            else
            {
                receptionTable.ItemsSource = null;
            }
        }
    }
}

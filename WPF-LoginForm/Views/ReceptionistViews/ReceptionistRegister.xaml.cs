using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_LoginForm.Model;

namespace WPF_LoginForm.Views.ReceptionistViews
{
    /// <summary>
    /// Logika interakcji dla klasy Receptionist.xaml
    /// </summary>

    public partial class ReceptionistRegister : Window
    {
        private ClinicEntities contextDB;
        private Receptionist parent;

        public ClinicEntities getContextDB()
        {
            return contextDB;
        }

        public ReceptionistRegister(Receptionist parentScreen)
        {
            InitializeComponent();
            parent = parentScreen;
            contextDB = parent.getContextDB();
            contextDB.Patients.Load();
            PatientTable.ItemsSource = contextDB.Patients.Local;
            dataPicker.DisplayDateStart = DateTime.Today;

            doctorComboBox.ItemsSource = contextDB.Doctors.Where(el => el.Logging.Active == true).ToList() ;
        }

        private void filterPatients()
        {
            var users = contextDB.Patients.Where(el => el.Name.StartsWith(NameTextBox.Text) && el.Surname.StartsWith(surnameTextBox.Text) && el.Pesel.StartsWith(peselTextBox.Text)).ToList();
            PatientTable.ItemsSource = users;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void registerVisitBtn_Click(object sender, RoutedEventArgs e)
        {
            Appointment newAppoinment = new Appointment();
            newAppoinment.Id_reg = App.userId;
            newAppoinment.Id_doc = ((Doctor)doctorComboBox.SelectedItem).Id_doc;
            newAppoinment.Id_pat = ((Patient)PatientTable.SelectedItem).Id_pat;
            newAppoinment.Description = "";
            //TODO:
            //enum with appoinment state
            newAppoinment.Status = 1;
            newAppoinment.Diagnosis = null;
            newAppoinment.Reg_date = DateTime.Now;
            newAppoinment.End_date = null;

            newAppoinment.Doctor = (Doctor)doctorComboBox.SelectedItem;
            newAppoinment.Registration = contextDB.Registrations.Where(el => el.Id_reg == App.userId).SingleOrDefault();

            contextDB.Appointments.Add(newAppoinment);
            contextDB.SaveChanges();
            parent.refreshView();
            Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addPatientBtn_Click(object sender, RoutedEventArgs e)
        {
            NewPatient nextScreen = new NewPatient(this);
            nextScreen.Show();
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterPatients();
        }

        private void surnameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterPatients();
        }

        private void peselTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterPatients();
        }

        private void PatientTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PatientTable.SelectedItem != null)
            {
                fillTextBoxes((Patient)PatientTable.SelectedItem);
            }
            registerVisitBtn.IsEnabled = canRegister();
        }

        public void fillTextBoxes(Patient selectedPatient)
        {
            NameTextBox.Text = selectedPatient.Name;
            surnameTextBox.Text = selectedPatient.Surname;
            peselTextBox.Text = selectedPatient.Pesel;
        }

        public void selectPatient(Patient selectedPatient)
        {
            PatientTable.SelectedItem = selectedPatient;
        }

        private void clearTextBoxes()
        {
            NameTextBox.Text = "";
            surnameTextBox.Text = "";
            peselTextBox.Text = "";
        }

        private void btnBack_Copy_Click(object sender, RoutedEventArgs e)
        {
            PatientTable.UnselectAll();
            clearTextBoxes();
        }

        private void doctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            registerVisitBtn.IsEnabled = canRegister();
        }

        private bool canRegister()
        {
            //bool description = !string.IsNullOrWhiteSpace(appointmentDescTextBox.Text);

            return /*description &&*/ doctorComboBox != null && PatientTable.SelectedItem!=null;

        }

        private void appointmentDescTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            registerVisitBtn.IsEnabled = canRegister();
        }
    }
}

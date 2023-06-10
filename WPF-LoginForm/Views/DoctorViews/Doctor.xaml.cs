using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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
using WPF_LoginForm.Views.LabAssistantViews;

namespace WPF_LoginForm.Views.DoctorViews
{
    /// <summary>
    /// Logika interakcji dla klasy Doctor.xaml
    /// </summary>
    public partial class Doctor : Window
    {
        private ClinicEntities contextDB;
        private int doctorId;
        private List<int> childrenIds;
        private bool readOnly = false;

        public ClinicEntities getContextDB()
        {
            return contextDB;
        }
        public Doctor(int userId)
        {
            doctorId = userId;
            childrenIds = new List<int>();
            InitializeComponent();
            contextDB = new ClinicEntities();
            contextDB.Appointments.Load();
            contextDB.MedicalGlossaries.Load();
            contextDB.PhysicalExaminations.Load();
            contextDB.LaboratoryTests.Load();
            appoinmentTable.ItemsSource = contextDB.Appointments.Local;
            filterDoctorsNameCombobox.ItemsSource = contextDB.Doctors.Where(el => el.Logging.Active == true).ToList();
            filterDoctorsNameCombobox.SelectedItem = contextDB.Doctors.Where(el => el.Id_doc == doctorId).Single();
            filterDataPicker.SelectedDate = DateTime.Now;
            var appointmentStatesList = contextDB.AppointmentStates.ToList();
            filterStatusCombobox.ItemsSource = appointmentStatesList;
            filterStatusCombobox.SelectedItem = appointmentStatesList[0];
            shouldFilterByDoctorsNameCheckbox.IsChecked = true;
            shouldFilterByStatusCheckbox.IsChecked = true;
            shouldFilterDayCheckbox.IsChecked = true;
            filterAppointments();
        }

        public Doctor(int patientId, ClinicEntities contextDB)
        {
            InitializeComponent();
            Title.Text = "History";
            readOnly = true;
            this.contextDB = contextDB;
            doctorId = patientId;
            shouldFilterByDoctorsNameCheckbox.IsChecked = false;
            shouldFilterDayCheckbox.IsChecked = false;
            shouldFilterByStatusCheckbox.IsChecked = true;
            filterDataPicker.IsEnabled = false;
            filterDoctorsNameCombobox.IsEnabled = false;
            filterStatusCombobox.IsEnabled = false;
            shouldFilterByDoctorsNameCheckbox.IsEnabled = false;
            shouldFilterDayCheckbox.IsEnabled = false;
            shouldFilterByStatusCheckbox.IsEnabled = false;
            filterDataPicker.Visibility = Visibility.Collapsed;
            filterDoctorsNameCombobox.Visibility = Visibility.Collapsed;
            filterStatusCombobox.Visibility = Visibility.Collapsed;
            shouldFilterByDoctorsNameCheckbox.Visibility = Visibility.Collapsed;
            shouldFilterDayCheckbox.Visibility = Visibility.Collapsed;
            shouldFilterByStatusCheckbox.Visibility = Visibility.Collapsed;
            var patient = contextDB.Patients.Local
                .Where(el =>
                    el.Id_pat == doctorId
                ).Single();
            mainLabel.Content = patient.Name + " " + patient.Surname + " - historia zrealizowanych wizyt";
            mainLabel.Width = 665;
            mainLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            mainLabel.FontSize = 14;
            appoinmentTable.ItemsSource = contextDB.Appointments.Local
                .Where(el =>
                    el.Id_pat == doctorId &&
                    el.Status == 2
                );
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
            if (readOnly)
            {
                Close();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        public void filterAppointments()
        {
            if (filterDataPicker.SelectedDate != null &&
                filterStatusCombobox.SelectedItem != null &&
                filterDoctorsNameCombobox.SelectedItem != null)
            {
                var filteredAppointments = contextDB.Appointments.
                Where(
                    el =>
                        (!shouldFilterByDoctorsNameCheckbox.IsChecked.Value ||
                            el.Id_doc == ((Model.Doctor)filterDoctorsNameCombobox.SelectedItem).Id_doc) &&
                        (!shouldFilterByStatusCheckbox.IsChecked.Value ||
                            el.Status == ((AppointmentState)filterStatusCombobox.SelectedItem).Id_state) &&
                        (!shouldFilterDayCheckbox.IsChecked.Value ||
                            DbFunctions.TruncateTime(el.Reg_date) == filterDataPicker.SelectedDate.Value)
                ).ToList();
                appoinmentTable.ItemsSource = filteredAppointments;
            }
        }

        private void acknowledgeAppoinmentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (appoinmentTable.SelectedValue != null)
            {
                Appointment selectedAppointment = (Appointment)appoinmentTable.SelectedValue;
                if (selectedAppointment.Id_doc == doctorId &&
                    !childrenIds.Contains(selectedAppointment.Id_app) &&
                    selectedAppointment.Status == 1)
                {
                    errorMessageTextBlock.Visibility = Visibility.Hidden;
                    DoctorVisit nextScreen = new DoctorVisit(this, selectedAppointment.Id_app);
                    childrenIds.Add(selectedAppointment.Id_app);
                    nextScreen.Show();
                }
                else
                {
                    errorMessageTextBlock.Text = childrenIds.Contains(selectedAppointment.Id_app)
                        ? "Istnieje już otwarte okno dla tej wizyty!"
                        : selectedAppointment.Status != 1 
                            ? "Nie da się zrealizować ponownie zatwierdzonych lub anulowanych wizyt" 
                            : "Nie jesteś uprawniony do realizacji tej wizyty!";
                    errorMessageTextBlock.Visibility = Visibility.Visible;
                }
            }
        }

        private void selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filterAppointments();
        }

        
        private void propagateCheckboxToCombobox(ComboBox comboBox, CheckBox checkBox)
        {
            comboBox.IsEnabled = checkBox.IsChecked.Value;
            comboBox.Foreground = checkBox.IsChecked.Value ?
                Brushes.Black : Brushes.Gray;
        }
        private void propagateCheckboxToDatePicker(DatePicker datePicker, CheckBox checkBox)
        {
            datePicker.IsEnabled = checkBox.IsChecked.Value;
            datePicker.Foreground = checkBox.IsChecked.Value ?
                Brushes.Black : Brushes.Gray;
        }
        private void shouldFilterByDoctorsNameCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            propagateCheckboxToCombobox(filterDoctorsNameCombobox, shouldFilterByDoctorsNameCheckbox);
            filterAppointments();
        }

        private void shouldFilterDayCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            propagateCheckboxToDatePicker(filterDataPicker, shouldFilterDayCheckbox);
            filterAppointments();
        }

        private void shouldFilterByStatusCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            propagateCheckboxToCombobox(filterStatusCombobox, shouldFilterByStatusCheckbox);
            filterAppointments();
        }

        public void attemptToDeleteChildId(int visitId)
        {
            childrenIds.Remove(visitId);
        }

        private void appoinmentTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (appoinmentTable.SelectedValue != null)
            {
                Appointment selectedAppointment = (Appointment)appoinmentTable.SelectedValue;
                if (selectedAppointment.Status != 1)
                {
                    DoctorVisit nextScreen = new DoctorVisit(this, selectedAppointment.Id_app, true);
                    nextScreen.Show();
                }
            }
        }
    }
}

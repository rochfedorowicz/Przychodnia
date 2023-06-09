using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using WPF_LoginForm.Views.ReceptionistViews;

namespace WPF_LoginForm.Views.LabManagerViews
{
    /// <summary>
    /// Logika interakcji dla klasy Lab_Manager.xaml
    /// </summary>
    public partial class LabManager : Window
    {
        private ClinicEntities contextDB;
        private String doctorsNote;
        private String result;
        private String statusFilter;
        private String dataFilter;
        private int chosenTestId;
        IEnumerable<dynamic> joinedData;
        IEnumerable<dynamic> filteredData;

        public ClinicEntities getContextDB()
        {
            return contextDB;
        }
        public LabManager()
        {
            InitializeComponent();
            contextDB = new ClinicEntities();
            contextDB.LaboratoryTests.Load();
            contextDB.Patients.Load();
            contextDB.Appointments.Load();
            joinedData = from lt in contextDB.LaboratoryTests
                             join a in contextDB.Appointments
                             on lt.Id_app equals a.Id_app
                             join p in contextDB.Patients
                             on a.Id_pat equals p.Id_pat
                             select new { lt.Id_labTest, lt.Code, lt.RealizationDate, p.Name, p.Surname, lt.DoctorsNote, lt.ManagersNote, lt.Result, lt.Status };

            joinedData = joinedData.OrderByDescending(data => data.RealizationDate);
            filteredData = joinedData.Where(data => data.Status == "End");
            laboratoryTestsTable.ItemsSource = filteredData.ToList();
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
            LabManagerApprove nextScreen = new LabManagerApprove(this);
            nextScreen.doctorsNote.Text = doctorsNote;
            nextScreen.result.Text = result;
            nextScreen.idLabTest = chosenTestId;
            nextScreen.Show();
        }

        private void filterBtn_Click(object sender, RoutedEventArgs e)
        {
            filteredData = joinedData;
            statusFilter = statusFilterCombo.Text;
            dataFilter = dataFilterPicker.Text;
            if (statusFilter != "Wszystkie" && statusFilter != "")
            {
                filteredData = filteredData.Where(data => data.Status == statusFilter);
            }
            if(dataFilter != "")
            {
                DateTime date = DateTime.ParseExact(dataFilter, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                filteredData = filteredData.Where(data => data.RealizationDate == date);
            }
            laboratoryTestsTable.ItemsSource = filteredData.ToList();
            
        }

        private void receptionTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (laboratoryTestsTable.SelectedItem != null && ((dynamic)laboratoryTestsTable.SelectedItem).Status == "End")
            {
                enableButtons();
                doctorsNote = ((dynamic)laboratoryTestsTable.SelectedItem).DoctorsNote;
                result = ((dynamic)laboratoryTestsTable.SelectedItem).Result;
                chosenTestId = ((dynamic)laboratoryTestsTable.SelectedItem).Id_labTest;

            }
            else
            {
                disableButtons();
            }
        }
        private void enableButtons()
        {
            checkLabTestBtn.IsEnabled = true;
        }
        private void disableButtons()
        {
            checkLabTestBtn.IsEnabled = false;
        }

        public void refreshView()
        {
            laboratoryTestsTable.ItemsSource = filteredData.ToList();
        }
    }
}

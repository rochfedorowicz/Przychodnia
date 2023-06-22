using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
using WPF_LoginForm.Views;
using WPF_LoginForm.Views.LabManagerViews;

namespace WPF_LoginForm.Views.LabAssistantViews
{
    /// <summary>
    /// Logika interakcji dla klasy Lab_Assistant.xaml
    /// </summary>
    public partial class LabAssistant : Window
    {
        int user;
        private ClinicEntities contextDB;
        private String doctorsNote;
        private String techNote;
        private String result;
        private String statusFilter;
        private String surnameFilter;
        private String dataFilter;
        private int chosenTestId;
        IEnumerable<dynamic> joinedData;
        IEnumerable<dynamic> filteredData;


        public ClinicEntities getContextDB()
        {
            return contextDB;
        }
        public LabAssistant(int userId)
        {
            user = userId;
            InitializeComponent();
            doTestBtn.IsEnabled = false;
            contextDB = new ClinicEntities();
            contextDB.LaboratoryTests.Load();
            contextDB.Patients.Load();
            contextDB.Appointments.Load();
            joinedData = from lt in contextDB.LaboratoryTests
                         join a in contextDB.Appointments
                         on lt.Id_app equals a.Id_app
                         join p in contextDB.Patients
                         on a.Id_pat equals p.Id_pat
                         select new { lt.Id_labTest, lt.Code, lt.CreateDate, p.Name, p.Surname, lt.DoctorsNote, lt.ManagersNote, lt.Result, lt.Status };

            joinedData = joinedData.OrderByDescending(data => data.CreateDate);
            filteredData = joinedData.Where(data => data.Status == "In progress");
            testTable.ItemsSource = filteredData.ToList();

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
        private void doTest_Click(object sender, RoutedEventArgs e)
        {
            //Get ID
            //string idText = testIdTextBox.Text;
            //int id;

            //if (int.TryParse(idText, out id))
            //{
            //    //TODO: ID VERYFICATION
            //    //TODO: GET TEST DATA AND SEND IT TO THE NEXT SCREEN
            //    //TODO: Make Singleton out of next screen
            //    MakeTest nextScreen = new MakeTest();
            //    nextScreen.Show();
            //}
            //else
            //{
            //    //TODO SHOW SOME ERROR OR CREATE PROPER VALIDTION ON THIS FIELD
            //}
        }

        private void receptionTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (testTable.SelectedItem != null && ((dynamic)testTable.SelectedItem).Status == "In progress")
            {
                enableButtons();
                doctorsNote = ((dynamic)testTable.SelectedItem).DoctorsNote;
                techNote = ((dynamic)testTable.SelectedItem).ManagersNote;
                chosenTestId = ((dynamic)testTable.SelectedItem).Id_labTest;

            }
            else
            {
                disableButtons();
            }
        }
        private void enableButtons()
        {
            doTestBtn.IsEnabled = true;
        }
        private void disableButtons()
        {
            doTestBtn.IsEnabled = false;
        }

        public void refreshView()
        {
            testTable.ItemsSource = filteredData.ToList();
        }
        private void filterBtn_Click(object sender, RoutedEventArgs e)
        {
            filteredData = joinedData;
            statusFilter = statusFilterCombo.Text;
            surnameFilter = surnameTextBox.Text;
            dataFilter = dataFilterPicker.Text;
            if (statusFilter != "Wszystkie" && statusFilter != "")
            {
                filteredData = filteredData.Where(data => data.Status == statusFilter);
            }
            if (dataFilter != "")
            {
                DateTime date = DateTime.ParseExact(dataFilter, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                filteredData = filteredData.Where(data => data.CreateDate == date);
            }
            if (surnameFilter != "")
            {
                filteredData = filteredData.Where(data => data.Surname.StartsWith(surnameFilter));
            }
            testTable.ItemsSource = filteredData.ToList();

        }

        private void doTestBtn_Click(object sender, RoutedEventArgs e)
        {
            MakeTest nextScreen = new MakeTest(this);
            nextScreen.TechNote.Text = techNote;
            nextScreen.DoctorsNote.Text = doctorsNote;
            nextScreen.idLabTest = chosenTestId;
            nextScreen.user = user;
            nextScreen.Show();
        }

        private void surnameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

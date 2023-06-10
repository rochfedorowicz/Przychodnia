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
using WPF_LoginForm.Model;
using WPF_LoginForm.Views.LabManagerViews;

namespace WPF_LoginForm.Views.LabAssistantViews
{
    /// <summary>
    /// Logika interakcji dla klasy MakeTest.xaml
    /// </summary>
    public partial class MakeTest : Window
    {
        private ClinicEntities contextDB;
        private LabAssistant parent;
        public int idLabTest;

        public ClinicEntities getContextDB()
        {
            return contextDB;
        }

        public MakeTest(LabAssistant parentScreen)
        {
            InitializeComponent();
            parent = parentScreen;
            contextDB = parent.getContextDB();
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


        
        private void updateData(String gStatus)
        {
            LaboratoryTest labTest = new LaboratoryTest();
            labTest = contextDB.LaboratoryTests.Find(idLabTest);
            if (labTest != null)
            {
                if (resultTextBox.Text != null && resultTextBox.Text != "" && resultTextBox.Text != "Miejsce na wynik")
                {
                    labTest.Result = resultTextBox.Text;
                }
                labTest.Status = gStatus;
                contextDB.SaveChanges();
                parent.refreshView();
            }
            else
            {
                MessageBox.Show("Coś poszło bardzo nie tak", "Patrz uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void resultTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (resultTextBox.Text != null)
            {
                enableButton();
            }
            if (resultTextBox.Text == null || resultTextBox.Text == "" || resultTextBox.Text == "Wynik: ")
            {
                disbleButton();
            }
        }
        private void noteInputGotFocus(object sender, RoutedEventArgs e)
        {
            if (resultTextBox.Text == "Wpisz uwagi dotyczące badania.")
            {
                resultTextBox.Text = string.Empty;
            }
        }

        private void noteInputLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(resultTextBox.Text))
            {
                resultTextBox.Text = "Wpisz uwagi dotyczące badania.";
            }
        }
        private void fininshTestBtn_Click(object sender, RoutedEventArgs e)
        {
            updateData("End");
            this.Close();
        }
        private void cancelTestBtn_Click(object sender, RoutedEventArgs e)
        {
            updateData("In progress");
            this.Close();
        }
        private void enableButton()
        {
            cancelTestBtn.IsEnabled = true;
            fininshTestBtn.IsEnabled = true;
        }
        private void disbleButton()
        {
            cancelTestBtn.IsEnabled = false;
            fininshTestBtn.IsEnabled = false;
        }
    }
}

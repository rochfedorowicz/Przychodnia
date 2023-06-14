using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
using WPF_LoginForm.Views.ReceptionistViews;

namespace WPF_LoginForm.Views.LabManagerViews
{
    /// <summary>
    /// Logika interakcji dla klasy MakeTest.xaml
    /// </summary>
    
    public partial class LabManagerApprove : Window
    {
        private ClinicEntities contextDB;
        private LabManager parent;
        public int idLabTest;

        public ClinicEntities getContextDB()
        {
            return contextDB;
        }

        public LabManagerApprove(LabManager parentScreen)
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
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (noteInput.Text != null)
            {
                enableCancelButton();
            }
            if (noteInput.Text == null || noteInput.Text == "" || noteInput.Text == "Wpisz uwagi dotyczące badania.")
            {
                disbleCancelButton();
            }

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
                if (noteInput.Text != null && noteInput.Text != "" && noteInput.Text !=  "Wpisz uwagi dotyczące badania.")
                {
                    labTest.ManagersNote = noteInput.Text;
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

        private void confirmTestBtn_Click(object sender, RoutedEventArgs e)
        {
            updateData("Approved");
            this.Close();
        }

        private void cancelTestBtn_Click(object sender, RoutedEventArgs e)
        {
            updateData("Rejected");
            this.Close();
        }

        private void noteInputGotFocus(object sender, RoutedEventArgs e)
        {
            if (noteInput.Text == "Wpisz uwagi dotyczące badania.")
            {
                noteInput.Text = string.Empty;
            }
        }

        private void noteInputLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(noteInput.Text))
            {
                noteInput.Text = "Wpisz uwagi dotyczące badania.";
            }
        }

        private void enableCancelButton()
        {
            cancelTestBtn.IsEnabled = true;
        }
        private void disbleCancelButton()
        {
            cancelTestBtn.IsEnabled = false;
        }
    }
}

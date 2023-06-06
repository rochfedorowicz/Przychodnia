using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WPF_LoginForm.Views.ReceptionistViews
{
    /// <summary>
    /// Logika interakcji dla klasy NewPatient.xaml
    /// </summary>
    public partial class NewPatient : Window
    {
        private ReceptionistRegister parent;
        public NewPatient(ReceptionistRegister parentScreen=null)
        {
            InitializeComponent();
            parent = parentScreen;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AreAllFieldsFilled())
            {
                if (!checkPesel(peselTextBox.Text))
                {
                    MessageBox.Show("Podany numer PESEL jest niepoprawny", "Patrz uwaga", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (!ValidatePostalCode(postalCodeTextBox.Text))
                {
                    MessageBox.Show("Podany kod pocztowy jest niepoprawny", "Patrz uwaga", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Patient newPatient = insertPatient();
                    insertAddress(newPatient.Id_pat);
                    if (parent != null)
                    {
                        parent.fillTextBoxes(newPatient);
                        parent.selectPatient(newPatient);
                    }
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Nie wszystkie pola zostawły wypełnione", "Patrz uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void insertAddress(int newPatientId)
        {
            Address newAdress = new Address();
            newAdress.Id_Pat = newPatientId;
            newAdress.City = cityTextBox.Text;
            newAdress.Street = streetTextBox.Text;
            //TODO:
            //check streen number validity
            newAdress.HouseNr = streetNumberTextBox.Text;
            if (flatNumberTextBox.Text == "")
                newAdress.FlatNr = null;
            else
                newAdress.FlatNr = Int32.Parse(flatNumberTextBox.Text);

            newAdress.Postcode = postalCodeTextBox.Text;

            if (parent != null)
            {
                parent.getContextDB().Addresses.Add(newAdress);
                parent.getContextDB().SaveChanges();
            }
            else
            {
                using (var context = new ClinicEntities())
                {
                    context.Addresses.Add(newAdress);
                    context.SaveChanges();
                }
            }
        }

        private Patient insertPatient()
        {
            Patient newPatient = new Patient();
            newPatient.Surname = surnameTextBox.Text;
            newPatient.Name = nameTextBox.Text;
            newPatient.Pesel = peselTextBox.Text;

            if (parent != null)
            {
                parent.getContextDB().Patients.Add(newPatient);
                parent.getContextDB().SaveChanges();
            }
            else
            {
                using (var context = new ClinicEntities())
                {
                    context.Patients.Add(newPatient);
                    context.SaveChanges();
                }
                
            }
            return newPatient;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private bool AreAllFieldsFilled()
        {

            bool name = !string.IsNullOrWhiteSpace(nameTextBox.Text);
            bool surname = !string.IsNullOrWhiteSpace(surnameTextBox.Text);
            bool pesel = !string.IsNullOrWhiteSpace(peselTextBox.Text);
            bool city = !string.IsNullOrWhiteSpace(cityTextBox.Text);
            bool postalCode = !string.IsNullOrWhiteSpace(postalCodeTextBox.Text);
            bool street = !string.IsNullOrWhiteSpace(streetTextBox.Text);
            bool streetNumber = !string.IsNullOrWhiteSpace(streetNumberTextBox.Text);

            bool mandatoryFields = name && surname && pesel && city && postalCode && street && streetNumber;

            return mandatoryFields;
        }
        private bool checkPesel(String pesel)
        {
            if (pesel.Length != 11)
            {
                return false;
            }

            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3, 1 };
            int sum = 0;

            for (int i = 0; i < 11; i++)
            {
                if (!char.IsDigit(pesel[i]))
                {
                    return false;
                }

                sum += int.Parse(pesel[i].ToString()) * weights[i];
            }

            int checksum = sum % 10;
            return checksum == 0;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public bool ValidatePostalCode(string postalCode)
        {
            Regex regex = new Regex(@"^\d{2}-\d{3}$");
            return regex.IsMatch(postalCode);
        }
    }
}

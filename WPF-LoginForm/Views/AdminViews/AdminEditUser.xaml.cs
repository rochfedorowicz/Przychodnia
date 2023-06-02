using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_LoginForm.Model;
using System.Text.RegularExpressions;

namespace WPF_LoginForm.Views.AdminViews
{
    /// <summary>
    /// Logika interakcji dla klasy MakeTest.xaml
    /// </summary>
    public partial class AdminEditUser : Window
    {
        ClinicEntities contextDB;
        List<string> comboBoxOptions = new List<string> { "Administrator", "Doktor", "Laborant","Kierownik laboratorium","Rejestratorka" };
        Admin parentScreen;

        public Logging EditUser { get; set; }

        public AdminEditUser(Admin parent, ClinicEntities context, Logging editUser = null)
        {
            InitializeComponent();
            contextDB = context;
            EditUser = editUser;
            parentScreen = parent;
            if (editUser != null)
            {
                var userFullName = getUserFullName();
                nameTextBox.Text = userFullName.Item1;
                surnameTextBox.Text = userFullName.Item2;
                loginTextBox.Text = EditUser.Login;
                passwordTextBox.Text = EditUser.Password;
                functionText.Content = roleCodeToRole();
                if (editUser.Doctor != null)
                    showNPWZ();
            }
            else
            {
                WindowNameTextBox.Text = "Dodaj użytkownika";
                confirmUserChangesBtn.Content = "Dodaj użytkownika";
                functionText.Visibility = Visibility.Hidden;
                userRoleComboBox.Visibility = Visibility.Visible;
            }

            userRoleComboBox.ItemsSource = comboBoxOptions;
            EditUser = editUser;
        }

        private void showNPWZ()
        {
            NPWZTextBox.IsEnabled = true;
            NPWZTextBox.Text = EditUser.Doctor.NPWZ.ToString();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void confirmUserChangesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (EditUser == null)
            {
                if (AreAllFieldsFilled())
                {
                    EditUser = new Logging();
                    EditUser.Role = getRoleCode();
                    EditUser.Login = loginTextBox.Text;
                    EditUser.Password = passwordTextBox.Text;
                    EditUser.Active = true;

                    contextDB.Loggings.Add(EditUser);

                    setUserReference();
                    setUserFullName();
                    if (EditUser.Doctor != null)
                        setNPWZ();
                }
                else
                {
                    MessageBox.Show("Nie wszystkie pola zostawły wypełnione", "Patrz uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
        {
                setUserFullName();
                EditUser.Login = loginTextBox.Text;
                EditUser.Password = passwordTextBox.Text;
                if (EditUser.Doctor != null)
                    setNPWZ();
            }

            contextDB.SaveChanges();
            parentScreen.refreshView();
            Close();
        }

        private void setNPWZ()
        {
            EditUser.Doctor.NPWZ = Int32.Parse(NPWZTextBox.Text);
        }

        private void setUserReference()
        {
            switch (EditUser.Role)
            {
                case "REG":
                    Registration registration = new Registration();
                    contextDB.Registrations.Add(registration);
                    EditUser.Registration = registration;
                    break;
                case "DOC":
                    Doctor doctor = new Doctor();
                    contextDB.Doctors.Add(doctor);
                    EditUser.Doctor = doctor;
                    break;
                case "LABM":
                    LaboratoryManager laboratoryManager = new LaboratoryManager();
                    contextDB.LaboratoryManagers.Add(laboratoryManager);
                    EditUser.LaboratoryManager = laboratoryManager;
                    break;
                case "LAB":
                    LabTech labTech = new LabTech();
                    contextDB.LabTeches.Add(labTech);
                    EditUser.LabTech = labTech;
                    break;
                case "ADM":
                    WPF_LoginForm.Model.Admin admin = new WPF_LoginForm.Model.Admin();
                    contextDB.Admins.Add(admin);
                    EditUser.Admin = admin;
                    break;
                default:
                    throw new DllNotFoundException("No such role");
            }
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        Tuple<string, string> getUserFullName()
        {
            string name, surname;
            switch (EditUser.Role)
            {
                case "REG":
                    name = EditUser.Registration.Name;
                    surname = EditUser.Registration.Surname;
                    break;
                case "DOC":
                    name = EditUser.Doctor.Name;
                    surname = EditUser.Doctor.Surname;
                    break;
                case "LABM":
                    name = EditUser.LaboratoryManager.Name;
                    surname = EditUser.LaboratoryManager.Surname;
                    break;
                case "LAB":
                    name = EditUser.LabTech.Name;
                    surname = EditUser.LabTech.Surname;
                    break;
                case "ADM":
                    name = EditUser.Admin.Name;
                    surname = EditUser.Admin.Surname;
                    break;
                default:
                    throw new DllNotFoundException("No such role");
            }
            return Tuple.Create(name, surname);
        }

        void setUserFullName()
        {
            switch (EditUser.Role)
            {
                case "REG":
                    EditUser.Registration.Name = nameTextBox.Text;
                    EditUser.Registration.Surname = surnameTextBox.Text;
                    break;
                case "DOC":
                    EditUser.Doctor.Name = nameTextBox.Text;
                    EditUser.Doctor.Surname = surnameTextBox.Text;
                    break;
                case "LABM":
                    EditUser.LaboratoryManager.Name = nameTextBox.Text;
                    EditUser.LaboratoryManager.Surname = surnameTextBox.Text;
                    break;
                case "LAB":
                    EditUser.LabTech.Name = nameTextBox.Text;
                    EditUser.LabTech.Surname = surnameTextBox.Text;
                    break;
                case "ADM":
                    EditUser.Admin.Name = nameTextBox.Text;
                    EditUser.Admin.Surname = surnameTextBox.Text;
                    break;
                default:
                    throw new DllNotFoundException("No such role");
            }
        }

        private void userRoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NPWZTextBox.IsEnabled = userRoleComboBox.SelectedItem == "Doktor";
        }

        string roleCodeToRole()
        {
            switch (EditUser.Role)
            {
                case "REG":
                    return "Rejestratorka";
                case "DOC":
                    return "Doktor";
                case "LABM":
                    return "Kierownik laboratorium";
                case "LAB":
                    return "Laborant";
                case "ADM":
                    return "Administrator";
                default:
                    throw new DllNotFoundException("No such role");
            }
        }
        string getRoleCode()
        {
            switch (userRoleComboBox.SelectedItem)
            {
                case "Rejestratorka":
                    return "REG";
                case "Doktor":
                    return "DOC";
                case "Kierownik laboratorium":
                    return "LABM";
                case "Laborant":
                    return "LAB";
                case "Administrator":
                    return "ADM";
                default:
                    throw new DllNotFoundException("No such role");
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private bool AreAllFieldsFilled()
        {
            
           bool name =  !string.IsNullOrWhiteSpace(nameTextBox.Text);
           bool surname =  !string.IsNullOrWhiteSpace(surnameTextBox.Text);
           bool login =  !string.IsNullOrWhiteSpace(loginTextBox.Text);
           bool password =  !string.IsNullOrWhiteSpace(passwordTextBox.Text);
           bool NPWZ =  !string.IsNullOrWhiteSpace(NPWZTextBox.Text);
           bool role =  userRoleComboBox.SelectedItem!=null;

           bool mandatoryFields = name && surname && login && password && role;

            return userRoleComboBox.SelectedItem == "Doktor" ? mandatoryFields && NPWZ : mandatoryFields;
        }

    }
}

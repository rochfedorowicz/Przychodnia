using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPF_LoginForm.Views.DoctorViews;


namespace WPF_LoginForm.Views.AdminViews
{
    /// <summary>
    /// Logika interakcji dla klasy Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        private ClinicEntities contextDB;
        private string password;
        private bool isPasswordVisible;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public bool IsPasswordVisible
        {
            get { return isPasswordVisible; }
            set
            {
                isPasswordVisible = value;
                OnPropertyChanged(nameof(IsPasswordVisible));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Admin()
        {
            InitializeComponent();
            contextDB = new ClinicEntities();
            contextDB.Loggings.Load();
            users.ItemsSource = contextDB.Loggings.Local;
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

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            disableButtons();
            AdminEditUser nextScreen = new AdminEditUser(this,contextDB);
            nextScreen.Show();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            disableButtons();
            AdminEditUser nextScreen = new AdminEditUser(this,contextDB,(Logging)users.SelectedItem);
            nextScreen.Show();
        }

        private void deactiveButton_Click(object sender, RoutedEventArgs e)
        {
            ((Logging)users.SelectedItem).Active = !((Logging)users.SelectedItem).Active;
            refreshView();
            deactiveButton.Background = new SolidColorBrush(Colors.Gray);
            contextDB.SaveChanges();
        }

        public void refreshView()
        {
            users.UnselectAll();
            ICollectionView view = CollectionViewSource.GetDefaultView(contextDB.Loggings.Local);
            view.Refresh();
        }
        private void users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (users.SelectedItem != null)
            {
                enableButtons();
                refreshDeactivateButton();
            }
            else
            {
                disableButtons();
            }
        }
        private void enableButtons()
        {
            deactiveButton.IsEnabled = true;
            editButton.IsEnabled = true;
        }
        private void disableButtons()
        {
            deactiveButton.IsEnabled = false;
            editButton.IsEnabled = false;
            deactiveButton.Background = new SolidColorBrush(Colors.Gray);
        }
        private void refreshDeactivateButton()
        {
            Logging currentUser = (Logging)users.SelectedItem;
            deactiveButton.Content = currentUser.Active ? "Deaktywuj użytkownika" : "Aktywuj użytkownika";
            deactiveButton.Background = currentUser.Active ? new SolidColorBrush(Colors.OrangeRed) : new SolidColorBrush(Colors.DeepSkyBlue);
        }
    }
}

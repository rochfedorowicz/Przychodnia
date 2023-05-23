using System;
using System.Collections.Generic;
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

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// Logika interakcji dla klasy Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        PrzychodniaEntities contextDB;
        public Admin()
        {
            InitializeComponent();
            contextDB = new PrzychodniaEntities();
            contextDB.Logowanie.Load();
            users.ItemsSource = contextDB.Logowanie.Local;
        }

        private void reception_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (users.SelectedItem != null)
            {
                deactiveButton.IsEnabled = true;
                editButton.IsEnabled = true;
            }
            else
            {
                deactiveButton.IsEnabled = false;
                editButton.IsEnabled = false;
            }
            
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void deactiveButton_Click(object sender, RoutedEventArgs e)
        {
            if (users.SelectedItem != null)
            {
                Logowanie log = (Logowanie)users.SelectedItem;
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz dezaktywować konto użytkownika "+ log.Imie + " "+ log.Nazwisko + "?", "Potwierdzenie", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // ...
                }
            }  
        }
    }
}

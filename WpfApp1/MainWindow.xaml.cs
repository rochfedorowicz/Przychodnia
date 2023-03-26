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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.app_windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int role = 4;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Window myWindow;
            
            switch (role)
            {
                case 1:
                    {
                        
                        myWindow = new AdminWindow();
                        break;
                    }
                case 2:
                    {
                        myWindow = new DoctorWindow();
                        break;
                    }
                case 3:
                    {
                        myWindow = new LabTechnicianWindow();
                        break;
                    }
                case 4:
                    {
                        myWindow = new ReceptionistWindow();
                        break;
                    }
                default:
                    {
                        myWindow = new ReceptionistWindow();
                        //Commit suacide
                        break;
                    }
            
            }
            this.Close();
            myWindow.ShowDialog();
        }
    }
}

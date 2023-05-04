using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPF_LoginForm.Views;

namespace WPF_LoginForm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static public string Funkcja;
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var loginView = new LoginView();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
              {
                  if (loginView.IsVisible == false && loginView.IsLoaded)
                  {
                     // var mainView = new MainView();

                      //mainView.Show();

                      Window nextScreen;

                      switch (Funkcja)
                      {
                          case "REJ":
                              nextScreen = new Receptionist();
                              break;
                          case "LEK":
                              nextScreen = new Doctor();
                              break;
                          case "KLAB":
                              nextScreen = new Lab_Manager();
                              break;
                          case "LAB":
                              nextScreen = new Lab_Assistant();
                              break;
                          case "ADM":
                              nextScreen = new Admin();
                              break;
                          default:
                              nextScreen = null;
                              break;
                      }
                      nextScreen.Show();

                      loginView.Close();
                  }
              };
        }
    }
}

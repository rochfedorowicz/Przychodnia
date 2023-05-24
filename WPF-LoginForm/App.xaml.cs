using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPF_LoginForm.Views;
using WPF_LoginForm.Views.AdminViews;
using WPF_LoginForm.Views.DoctorViews;
using WPF_LoginForm.Views.LabAssistantViews;
using WPF_LoginForm.Views.LabManagerViews;
using WPF_LoginForm.Views.ReceptionistViews;

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

            //Static Entrence
            Funkcja = "ADM";

            var loginView = new LoginView();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
              {
                  if (loginView.IsVisible == false && loginView.IsLoaded)
                  {
                      Window nextScreen;

                      switch (Funkcja)
                      {
                          case "REG":
                              nextScreen = new Receptionist();
                              break;
                          case "DOC":
                              nextScreen = new Doctor();
                              break;
                          case "LABM":
                              nextScreen = new LabManager();
                              break;
                          case "LAB":
                              nextScreen = new LabAssistant();
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

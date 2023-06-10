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

namespace WPF_LoginForm.Views.DoctorViews
{
    /// <summary>
    /// Logika interakcji dla klasy Doctor.xaml
    /// </summary>
    public partial class DoctorVisit : Window
    {
        private ClinicEntities contextDB;
        private Doctor parent;
        private int visitId;
        private int patientId;
        private Doctor historyWindow;
        bool readOnly = false;

        public ClinicEntities getContextDB()
        {
            return contextDB;
        }
        public DoctorVisit(Doctor parent, int visitId, bool readOnly = false)
        {
            this.parent = parent;
            this.visitId = visitId;
            InitializeComponent();
            contextDB = parent.getContextDB();
            var patient = contextDB.Appointments.Where(el => el.Id_app == visitId).Join(contextDB.Patients,
                a => a.Id_pat,
                p => p.Id_pat,
                (a, p) => p).Single();
            patientId = patient.Id_pat;
            patientsInfo.Content = "" + patient.Name + " " + patient.Surname + " " + patient.Pesel;
            physicalExaminationTypeComboBox.ItemsSource = contextDB.MedicalGlossaries.Local.Where(el => el.Type.Equals("P"));
            labTestTypeComboBox.ItemsSource = contextDB.MedicalGlossaries.Local.Where(el => el.Type.Equals("L"));
            var alreadyConductedPhysicalExaminations = contextDB.PhysicalExaminations.Local.Where(el => el.Id_app == visitId).ToList();
            alreadyConductedPhysicalExaminations.ForEach(x => { physicalExaminationsListBox.Items.Add(x); });
            var alreadyCommissionedLabTests= contextDB.LaboratoryTests.Local.Where(el => el.Id_app == visitId).ToList();
            alreadyCommissionedLabTests.ForEach(x => { laboratoryTestsListBox.Items.Add(x); });
            
            if (readOnly)
            {
                this.readOnly = true;

                var visit = contextDB.Appointments
                    .Where(el => el.Id_app == visitId).Single();

                DiagnosisTextBox.Text = visit.Diagnosis;
                visitDescTextBox.Text = visit.Description;

                visitDescTextBox.IsReadOnly = true;
                DiagnosisTextBox.IsReadOnly = true;
                cancelVisitBtn.IsEnabled = false;
                confirmVisitBtn_Copy.IsEnabled = false;
                del________Btn.IsEnabled = false;
                del____2_Btn.IsEnabled = false;
                doPhysicalExamination.IsEnabled = false;
                doLabTest.IsEnabled = false;
                checkHistoryBtn.IsEnabled = false;
                physicalExaminationResultTextBox.IsReadOnly = true;
                physicalExaminationResultTextBox.IsEnabled = false;
                labTestRemarksTextBox.IsReadOnly = true;
                labTestRemarksTextBox.IsEnabled = false;
                physicalExaminationTypeComboBox.IsEnabled = false;
                labTestTypeComboBox.IsEnabled = false;

                cancelVisitBtn.Visibility = Visibility.Collapsed;
                confirmVisitBtn_Copy.Visibility = Visibility.Collapsed;
                del________Btn.Visibility = Visibility.Collapsed;
                del____2_Btn.Visibility = Visibility.Collapsed;
                doPhysicalExamination.Visibility = Visibility.Collapsed;
                doLabTest.Visibility = Visibility.Collapsed;
                checkHistoryBtn.Visibility = Visibility.Collapsed;
                labTestTypeLabel.Visibility = Visibility.Collapsed;
                physicalExaminationTypeLabel.Visibility = Visibility.Collapsed;
                labTestRemarksLabel.Visibility = Visibility.Collapsed;
                physicalExaminationResultLabel.Visibility = Visibility.Collapsed;
                orderPhysicalExaminationLabel.Visibility = Visibility.Collapsed;
                conductPhysicalExaminationLabel.Visibility = Visibility.Collapsed;
                physicalExaminationResultTextBox.Visibility = Visibility.Collapsed;
                physicalExaminationTypeComboBox.Visibility = Visibility.Collapsed;
                labTestTypeComboBox.Visibility = Visibility.Collapsed;
                labTestRemarksTextBox.Visibility = Visibility.Collapsed;

                DiagnosisTextBox.Height = 146;
                laboratoryTestsLabel.Margin = new Thickness(240, 55, 0, 0);
                physicalExaminationsLabel.Margin = new Thickness(14, 55, 0, 0);
                laboratoryTestsListBox.Margin = new Thickness(240, 80, 356, 39);
                physicalExaminationsListBox.Margin = new Thickness(14, 80, 586, 39);
            }
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
            if (!readOnly)
                parent.attemptToDeleteChildId(visitId);
            Close();
        }

        private void confirmVisitBtn_Click(object sender, RoutedEventArgs e)
        {
            var visit = contextDB.Appointments
                .Where(el => el.Id_app == visitId).Single();
            visit.Status = 2;
            visit.Diagnosis = DiagnosisTextBox.Text;
            visit.Description = visitDescTextBox.Text;
            contextDB.SaveChanges();
            parent.filterAppointments();
            parent.attemptToDeleteChildId(visitId);
            Close();
        }

        private void doLabTest_Click(object sender, RoutedEventArgs e)
        {
            if (labTestTypeComboBox.SelectedItem != null)
            {
                var laboratoryTestToBeAdded = new LaboratoryTest();
                laboratoryTestToBeAdded.Code = ((MedicalGlossary)labTestTypeComboBox.SelectedItem).Code;
                laboratoryTestToBeAdded.Id_app = visitId;
                laboratoryTestToBeAdded.DoctorsNote = labTestRemarksTextBox.Text;
                laboratoryTestToBeAdded.CreateDate = DateTime.Now;
                laboratoryTestToBeAdded.Status = "In progress";
                contextDB.LaboratoryTests.Add(laboratoryTestToBeAdded);
                contextDB.SaveChanges();
                laboratoryTestsListBox.Items.Add(laboratoryTestToBeAdded);
                labTestRemarksTextBox.Text = "Uwagi do badania";
            } 
            else
            {
                MessageBox.Show("Wybierz najpierw typ badania", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void doPhysicalExamination_Click(object sender, RoutedEventArgs e)
        {
            if (physicalExaminationTypeComboBox.SelectedItem != null)
            {
                var physicalExaminationToBeAdded = new PhysicalExamination();
                physicalExaminationToBeAdded.Code = ((MedicalGlossary)physicalExaminationTypeComboBox.SelectedItem).Code;
                physicalExaminationToBeAdded.Id_app = visitId;
                physicalExaminationToBeAdded.Result = physicalExaminationResultTextBox.Text;
                contextDB.PhysicalExaminations.Add(physicalExaminationToBeAdded);
                contextDB.SaveChanges();
                physicalExaminationsListBox.Items.Add(physicalExaminationToBeAdded);
                physicalExaminationResultTextBox.Text = "Wynik badania";
            }
            else
            {
                MessageBox.Show("Wybierz najpierw typ badania", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void del____2_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (physicalExaminationsListBox.SelectedItem != null)
            {
                PhysicalExamination physicalExaminationToBeDeleted = (PhysicalExamination)physicalExaminationsListBox.SelectedItem;
                contextDB.PhysicalExaminations.Remove(physicalExaminationToBeDeleted);
                physicalExaminationsListBox.Items.Remove(physicalExaminationToBeDeleted);
                contextDB.SaveChanges();
            }
        }

        private void physicalExaminationsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (physicalExaminationsListBox.SelectedItem != null)
            {
                PhysicalExamination physicalExamination = (PhysicalExamination)physicalExaminationsListBox.SelectedItem;
                PhysicalTest physicalExaminationView = new PhysicalTest(physicalExamination.MedicalGlossary.Name,physicalExamination.Result);
                physicalExaminationView.Show();
            }
        }

        private void del________Btn_Click(object sender, RoutedEventArgs e)
        {
            if (laboratoryTestsListBox.SelectedItem != null)
            {
                LaboratoryTest labTest = (LaboratoryTest)laboratoryTestsListBox.SelectedItem;
                contextDB.LaboratoryTests.Remove(labTest);
                laboratoryTestsListBox.Items.Remove(labTest);
                contextDB.SaveChanges();
            }
        }

        private void laboratoryTestsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (laboratoryTestsListBox.SelectedItem != null)
            {
                LaboratoryTest labTest = (LaboratoryTest)laboratoryTestsListBox.SelectedItem;
                LabTest labTestView = new LabTest(labTest.MedicalGlossary.Name, labTest.Result, labTest.DoctorsNote);
                labTestView.Show();
            }
        }

        private void checkHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (historyWindow == null || historyWindow.IsLoaded == false)
            {
                historyWindow = new Doctor(patientId, contextDB);
                historyWindow.Show();
            }
        }

        private void cancelVisitBtn_Click(object sender, RoutedEventArgs e)
        {
            var visit = contextDB.Appointments
                .Where(el => el.Id_app == visitId).Single();
            visit.Status = 3;
            visit.Diagnosis = DiagnosisTextBox.Text;
            visit.Description = visitDescTextBox.Text;
            var alreadyConductedPhysicalExaminations = contextDB.PhysicalExaminations.Local
                .Where(el => el.Id_app == visitId).ToList();
            alreadyConductedPhysicalExaminations
                .ForEach(x => {
                    physicalExaminationsListBox.Items.Remove(x);
                    contextDB.PhysicalExaminations.Remove(x);
                });
            var alreadyCommissionedLabTests = contextDB.LaboratoryTests.Local
                .Where(el => el.Id_app == visitId).ToList();
            alreadyCommissionedLabTests
                .ForEach(x => {
                    laboratoryTestsListBox.Items.Remove(x);
                    contextDB.LaboratoryTests.Remove(x);
                });
            contextDB.SaveChanges();
            parent.filterAppointments();
            parent.attemptToDeleteChildId(visitId);
            Close();
        }
    }
}

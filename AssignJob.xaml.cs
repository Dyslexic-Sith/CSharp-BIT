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

namespace BITClientServer
{
    /// <summary>
    /// Interaction logic for AssignJob.xaml
    /// </summary>
    public partial class AssignJob : Window
    {
        Login cred = new Login();
        public AssignJob(string empType)
        {
            cred.EmpType = empType;
            InitializeComponent();
            ShowData();
        }

        private void ShowData()
        {
            JobCollection myjobs = new JobCollection("Submitted");
            lvAssignJob.ItemsSource = myjobs;
        }

        private void btnAssignJobGoBack_Click(object sender, RoutedEventArgs e)
        {
            MainPage mainp = new MainPage(cred.EmpType);
            mainp.Show();
            Close();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainw = new MainWindow();
            mainw.Show();
            Close();
        }

        private void btnFindContractor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JobRequest selectJob = (JobRequest)lvAssignJob.SelectedItem;
                ContractorCollection skillCons = new ContractorCollection(selectJob.SkillID);
                lvAssignContractor.ItemsSource = skillCons;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnAssignContractor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JobRequest selectJob = (JobRequest)lvAssignJob.SelectedItem;
                Contractor selectCon = (Contractor)lvAssignContractor.SelectedItem;
                selectJob.AssignContractor(selectCon.ContractorID);
                ShowData();
                lvAssignContractor.ItemsSource = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
    }
}

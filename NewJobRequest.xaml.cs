using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for NewJobRequest.xaml
    /// </summary>
    public partial class NewJobRequest : Window
    {
        Login cred = new Login();
        public NewJobRequest(string empType)
        {
            cred.EmpType = empType;
            InitializeComponent();
            LoadSkillList();
            LoadStatusList();
            LoadClientsList();
            LoadCoordinatorList();
        }

        private void LoadCoordinatorList()
        {
            DataTable dtCoordinators = Coordinator.ReadCoordinator();
            cmbNewJobCoordinator.SelectedValuePath = "employeeID";
            cmbNewJobCoordinator.DisplayMemberPath = "employeeFirstName";
            cmbNewJobCoordinator.ItemsSource = dtCoordinators.DefaultView;
        }

        private void LoadClientsList()
        {
            DataTable dtClients = BITClient.ReadClientCombo();            
            cmbNewJobClient.SelectedValuePath = "clientID";
            cmbNewJobClient.DisplayMemberPath = "clientFirstName";
            cmbNewJobClient.ItemsSource = dtClients.DefaultView;
        }

        private void LoadStatusList()
        {
            cmbNewJobStatus.Items.Add("Submitted");
            cmbNewJobStatus.Items.Add("Assigned");
            cmbNewJobStatus.Items.Add("Completed");
        }

        private void LoadSkillList()
        {
            DataTable dtSkills = Skill.ReadSkill();
            cmbNewJobSkill.SelectedValuePath = "skillsID";
            cmbNewJobSkill.DisplayMemberPath = "skillsTitle";
            cmbNewJobSkill.ItemsSource = dtSkills.DefaultView;
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainw = new MainWindow();
            mainw.Show();
            Close();
        }

        private void btnNewJobGoBack_Click(object sender, RoutedEventArgs e)
        {
            MainPage mainp = new MainPage(cred.EmpType);
            mainp.Show();
            Close();
        }

        private void cmbNewJobClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selected = Convert.ToInt32(cmbNewJobClient.SelectedValue);
            DataTable dtLocations = BITClient.ReadClientLocation(selected);
                cmbNewJobLocation.SelectedValuePath = "clientLocationID";
                cmbNewJobLocation.DisplayMemberPath = "clientLocationSuburb";
                cmbNewJobLocation.ItemsSource = dtLocations.DefaultView;
        }

        private void btnNewJobSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbNewJobClient.SelectedValue == null || cmbNewJobLocation.SelectedValue == null || cmbNewJobSkill.SelectedValue == null || cmbNewJobStatus.SelectedValue == null || cmbNewJobCoordinator == null || txtBxNewJobDescription.Text == "")
                {
                    MessageBox.Show("Please make sure all fields have been chosen before submitting the job request.");
                }
                else {
                    JobRequest newJob = new JobRequest();
                    DateTime dueDateBeforeConvert = DateTime.Parse(calDueDate.SelectedDate.ToString());
                    newJob.ClientID = Convert.ToInt32(cmbNewJobClient.SelectedValue);
                    newJob.ClientLocationID = Convert.ToInt32(cmbNewJobLocation.SelectedValue);
                    newJob.EntryDate = DateTime.Today.Date;
                    newJob.DueDate = dueDateBeforeConvert.Date;
                    newJob.SkillID = Convert.ToInt32(cmbNewJobSkill.SelectedValue);
                    newJob.Status = cmbNewJobStatus.SelectedValue.ToString();
                    newJob.CoordinatorID = Convert.ToInt32(cmbNewJobCoordinator.SelectedValue);
                    newJob.JobRequestDetails = txtBxNewJobDescription.Text.Replace("'", @"\'");
                    newJob.AddJobRequest();
                    ClearFields();
                }
            }
            catch(MySqlException sqlEx)
            {
                MessageBox.Show("There was an error, please check the details and try again.", sqlEx.Message);
            }
            catch (Exception ex)
            {

                MessageBox.Show("There was an error, please check the details and try again.", ex.Message);
            }

        }

        private void ClearFields()
        {
            cmbNewJobClient.SelectedIndex = -1;
            cmbNewJobCoordinator.SelectedIndex = -1;
            cmbNewJobLocation.SelectedIndex = -1;
            cmbNewJobSkill.SelectedIndex = -1;
            cmbNewJobStatus.SelectedIndex = -1;
            txtBxNewJobDescription.Clear();
        }
    }
}

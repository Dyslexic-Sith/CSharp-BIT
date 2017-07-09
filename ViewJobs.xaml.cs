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
    /// Interaction logic for ViewJobs.xaml
    /// </summary>
    public partial class ViewJobs : Window
    {
        Login cred = new Login();
        public ViewJobs(string empType)
        {
            cred.EmpType = empType;
            InitializeComponent();
            ShowData();
        }

        private void ShowData()
        {
            try
            {
                JobCollection myjobs = new JobCollection();
                lvJobs.ItemsSource = myjobs;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Something went wrong.", ex.Message);
            }

        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            ShowData();
        }

        private void btnViewSubmittedJobs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JobCollection myjobs = new JobCollection("Submitted");
                lvJobs.ItemsSource = myjobs;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Something went wrong.", ex.Message);
            }

        }

        private void btnViewAssignedJobs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JobCollection myjobs = new JobCollection("Assigned");
                lvJobs.ItemsSource = myjobs;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Something went wrong.", ex.Message);
            }

        }

        private void btnViewCompletedJobs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JobCollection myjobs = new JobCollection("Completed");
                lvJobs.ItemsSource = myjobs;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Something went wrong.", ex.Message);
            }

        }

        private void btnMarkJobCompleted_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JobRequest completedJob = (JobRequest)lvJobs.SelectedItem;
                completedJob.MarkJobAsCompleted();
                ShowData();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Something went wrong.", ex.Message);
            }
            
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
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
    }
}

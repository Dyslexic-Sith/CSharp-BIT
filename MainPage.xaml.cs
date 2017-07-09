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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        Login cred = new Login();
        public MainPage(string empType)
        {
            InitializeComponent();
            cred.EmpType = empType;
            if (empType == "Coordinator")
            {
                btnCoOrdinators.Visibility = Visibility.Hidden;
            }
            else if (empType == "Administrator")
            {
                btnCoOrdinators.Visibility = Visibility.Visible;
            }
        }

        private void btnContractors_Click(object sender, RoutedEventArgs e)
        {
            ContractorsScreen con = new ContractorsScreen(cred.EmpType);
            con.Show();
            this.Close();
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            Client cl = new Client(cred.EmpType);
            cl.Show();
            this.Close();
        }

        private void btnCoOrdinators_Click(object sender, RoutedEventArgs e)
        {
            CoOrdinators co = new CoOrdinators(cred.EmpType);
            co.Show();
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainw = new MainWindow();
            mainw.Show();
            Close();
        }

        private void btnNewJobRequest_Click(object sender, RoutedEventArgs e)
        {
            NewJobRequest njr = new NewJobRequest(cred.EmpType);
            njr.Show();
            Close();
        }

        private void btnAddNewSkills_Click(object sender, RoutedEventArgs e)
        {
            AddNewSkill newskill = new AddNewSkill(cred.EmpType);
            newskill.Show();
            Close();
        }

        private void btnAssignJob_Click(object sender, RoutedEventArgs e)
        {
            AssignJob assign = new AssignJob(cred.EmpType);
            assign.Show();
            Close();
        }

        private void btnViewJobs_Click(object sender, RoutedEventArgs e)
        {
            ViewJobs viewJobs = new ViewJobs(cred.EmpType);
            viewJobs.Show();
            Close();
        }
    }
}

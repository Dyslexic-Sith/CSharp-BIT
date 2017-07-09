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
    /// Interaction logic for Contractors.xaml
    /// </summary>
    public partial class ContractorsScreen : Window
    {
        Login cred = new Login();
        DataTable dtSkills = Skill.ReadSkillCombo();
        public ContractorsScreen(string empType)
        {
            cred.EmpType = empType;
            InitializeComponent();
            LoadSkillCombo();
            ShowData();
        }

        private void LoadSkillCombo()
        {
            SkillCollection skills = new SkillCollection();
            foreach (Skill s in skills)
            {
                CheckBox txt = new CheckBox();
                lbSkills.SelectedValuePath = "skillsID";
                lbSkills.Items.Add(txt);
                txt.Content = s.SkillTitle;
                txt.Tag = s.SkillID.ToString();

            }

        }


        private void ShowData()
        {
            try
            {
                ContractorCollection myContractors = new ContractorCollection();
                lvContractors.DataContext = myContractors;
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        private void btnContractorGoBack_Click(object sender, RoutedEventArgs e)
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

        private void btnContractorClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtContractorFName.Clear();
            txtContractorLName.Clear();
            txtContractorPhone.Clear();
            txtContractorEmail.Clear();
            txtContractorUnit.Clear();
            txtContractorStreet.Clear();
            txtContractorSuburb.Clear();
            txtContractorPostcode.Clear();
            txtContractorState.Clear();
            txtBxPassword.Clear();
            txtBxUsername.Clear();
            lbSkills.SelectedIndex = -1;
            lvContractors.SelectedIndex = -1;
            
        }

        private void lvContractors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (CheckBox c in lbSkills.Items)
            {
                c.IsChecked = false;
            }
        

        Contractor con = (Contractor)lvContractors.SelectedItem;

            if (lvContractors.SelectedItem == null)
            {
                foreach (CheckBox c in lbSkills.Items)
                {
                    c.IsChecked = false;
                }
            }
            else if (lvContractors.SelectedItem != null)
            {
                SkillCollection skills = new SkillCollection(con);
                //int i = 0;
                foreach (Skill s in skills)
                {
                    foreach (CheckBox c in lbSkills.Items)
                    {

                        if (c.Content.ToString() == s.SkillTitle.ToString())
                        { c.IsChecked = true; }
                       // if (c.Content.ToString() != s.SkillTitle.ToString())
                        //{ c.IsChecked = false; }

                    }

                }
            }
            else
                foreach (CheckBox c in lbSkills.Items)
                {
                    c.IsChecked = false;
                };
        }

        private void btnContractorAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtContractorFName.Text == "" || txtContractorLName.Text == "" || txtContractorPhone.Text == "" || txtContractorEmail.Text == "" || txtContractorUnit.Text == "" || txtContractorStreet.Text == "" || txtContractorSuburb.Text == "" || txtContractorPostcode.Text == "" || txtContractorState.Text == "" || txtBxUsername.Text == "" || txtBxPassword.Text == "")
            {
                MessageBox.Show("Please fill in all fields before adding a contractor.");
                return;
            }
            if (!IsValidEmail(txtContractorEmail.Text)){
                MessageBox.Show("Please enter a valid email address.");
                return;
            }
            else {
                Contractor newCon = new Contractor();
                newCon.EmployeeFName = txtContractorFName.Text;
                newCon.EmployeeLName = txtContractorLName.Text;
                newCon.EmployeePhone = txtContractorPhone.Text;
                newCon.EmployeeEmail = txtContractorEmail.Text;
                newCon.EmployeeLocationUnit = txtContractorUnit.Text;
                newCon.EmployeeLocationStreet = txtContractorStreet.Text;
                newCon.EmployeeLocationSuburb = txtContractorSuburb.Text;
                newCon.EmployeeLocationPostcode = txtContractorPostcode.Text;
                newCon.EmployeeLocationState = txtContractorState.Text;
                newCon.EmployeeUsername = txtBxUsername.Text;
                newCon.EmployeePassword = txtBxPassword.Text;
                newCon.Status = 1;
                foreach (CheckBox c in lbSkills.Items)
                {
                    if (c.IsChecked == true)
                    {
                        newCon.SkillID.Add(Convert.ToInt32(c.Tag));
                    }
                }
                newCon.AddContractor();
                ShowData();
            }
        }

        private void btnContractorDelete_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Contractor selectedCon = (Contractor)lvContractors.SelectedItem;
                selectedCon.DeleteContractor();
                ShowData();
            }
            catch (Exception ex)
            {

                throw new Exception("We could not delete this client", ex);
            }
        }

        private void btnContractorUpdate_Click(object sender, RoutedEventArgs e)
        {
            Contractor newCon = (Contractor)lvContractors.SelectedItem;

            if (!IsValidEmail(txtContractorEmail.Text)){
                MessageBox.Show("Please enter a valid email address.");
                return;
            }
            if (txtContractorFName.Text == "" || txtContractorLName.Text == "" || txtContractorPhone.Text == "" || txtContractorEmail.Text == "" || txtContractorUnit.Text == "" || txtContractorStreet.Text == "" || txtContractorSuburb.Text == "" || txtContractorPostcode.Text == "" || txtContractorState.Text == "" || txtBxUsername.Text == "" || txtBxPassword.Text == "")
            {
                var result = MessageBox.Show("Is this correct? Do you want to proceed with the update?", "You have some empty fields.", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    newCon.EmployeeFName = txtContractorFName.Text;
                    newCon.EmployeeLName = txtContractorLName.Text;
                    newCon.EmployeePhone = txtContractorPhone.Text;
                    newCon.EmployeeEmail = txtContractorEmail.Text;
                    newCon.EmployeeLocationUnit = txtContractorUnit.Text;
                    newCon.EmployeeLocationStreet = txtContractorStreet.Text;
                    newCon.EmployeeLocationSuburb = txtContractorSuburb.Text;
                    newCon.EmployeeLocationPostcode = txtContractorPostcode.Text;
                    newCon.EmployeeLocationState = txtContractorState.Text;
                    newCon.EmployeeUsername = txtBxUsername.Text;
                    newCon.EmployeePassword = txtBxPassword.Text;
                    newCon.Status = 1;
                    foreach (CheckBox c in lbSkills.Items)
                    {
                        if (c.IsChecked == true)
                        {
                            newCon.SkillID.Add(Convert.ToInt32(c.Tag));
                        }
                    }
                    newCon.UpdateContractor();
                    ShowData();
                }
            }
        }

        private void btnCoOrdinatorPayJob_Click(object sender, RoutedEventArgs e)
        {
            ViewJobs viewjobs = new ViewJobs(cred.EmpType);
            viewjobs.Show();
            Close();
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

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
    /// Interaction logic for CoOrdinators.xaml
    /// </summary>
    public partial class CoOrdinators : Window
    {
        Login cred = new Login();
        public CoOrdinators(string empType)
        {
            cred.EmpType = empType;
            InitializeComponent();
            ShowData();
        }

        private void ShowData()
        {
            try
            {
                //lvClients.Items.Clear();
                CoordinatorCollection myCoords = new CoordinatorCollection();
                lvCoOrdinators.ItemsSource = myCoords;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnCoOrdinatorGoBack_Click(object sender, RoutedEventArgs e)
        {
            MainPage mainp = new MainPage(cred.EmpType);
            mainp.Show();
            Close();
        }

        private void btnCoOrdinatorAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(txtCoOrdinatorFName.Text == "" || txtCoOrdinatorLName.Text == "" || txtCoOrdinatorPhone.Text == "" || txtCoOrdinatorEmail.Text == "" || txtCoOrdinatorUnit.Text == "" || txtCoOrdinatorStreet.Text == "" || txtCoOrdinatorSuburb.Text == "" || txtCoOrdinatorPostcode.Text == "" || txtCoOrdinatorState.Text == "")
                {
                    MessageBox.Show("Please make sure all fields are filled in before adding a coordinator.");
                    return;
                }
                else if (!IsValidEmail(txtCoOrdinatorEmail.Text)){
                    MessageBox.Show("Please enter a valid email address.");
                    return;
                }
                else
                {
                    Coordinator newCoord = new Coordinator();
                    newCoord.EmployeeFName = txtCoOrdinatorFName.Text;
                    newCoord.EmployeeLName = txtCoOrdinatorLName.Text;
                    newCoord.EmployeePhone = txtCoOrdinatorPhone.Text;
                    newCoord.EmployeeEmail = txtCoOrdinatorEmail.Text;
                    newCoord.EmployeeLocationUnit = txtCoOrdinatorUnit.Text;
                    newCoord.EmployeeLocationStreet = txtCoOrdinatorStreet.Text;
                    newCoord.EmployeeLocationSuburb = txtCoOrdinatorSuburb.Text;
                    newCoord.EmployeeLocationPostcode = txtCoOrdinatorPostcode.Text;
                    newCoord.EmployeeLocationState = txtCoOrdinatorState.Text;
                    newCoord.EmployeeUsername = txtBxUsername.Text;
                    newCoord.EmployeePassword = txtBxPassword.Text;
                    newCoord.AddCoordinator();
                    ClearFields();
                    ShowData();
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ClearFields()
        {
            txtCoOrdinatorFName.Clear();
            txtCoOrdinatorLName.Clear();
            txtCoOrdinatorPhone.Clear();
            txtCoOrdinatorEmail.Clear();
            txtCoOrdinatorUnit.Clear();
            txtCoOrdinatorStreet.Clear();
            txtCoOrdinatorSuburb.Clear();
            txtCoOrdinatorPostcode.Clear();
            txtCoOrdinatorState.Clear();
            txtBxUsername.Clear();
            txtBxPassword.Clear();
            lvCoOrdinators.SelectedIndex = -1;
        }

        private void btnCoOrdinatorClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainw = new MainWindow();
            mainw.Show();
            Close();
        }

        private void btnCoOrdinatorDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Coordinator selectedCoord = (Coordinator)lvCoOrdinators.SelectedItem;
                selectedCoord.DeleteCoordinator();
                ShowData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong.");
            }
            
        }

        private void btnCoOrdinatorUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsValidEmail(txtCoOrdinatorEmail.Text)){
                    MessageBox.Show("Please enter a valid email address.");
                    return;
                }
                if (txtCoOrdinatorFName.Text == "" || txtCoOrdinatorLName.Text == "" || txtCoOrdinatorPhone.Text == "" || txtCoOrdinatorEmail.Text == "" || txtCoOrdinatorUnit.Text == "" || txtCoOrdinatorStreet.Text == "" || txtCoOrdinatorSuburb.Text == "" || txtCoOrdinatorPostcode.Text == "" || txtCoOrdinatorState.Text == "" || txtBxPassword.Text == "" || txtBxUsername.Text == "")
                {
                    var result = MessageBox.Show("Is this correct? Do you want to proceed with the update?", "You have some empty fields.", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        Coordinator selectedCoord = (Coordinator)lvCoOrdinators.SelectedItem;
                        selectedCoord.EmployeeFName = txtCoOrdinatorFName.Text;
                        selectedCoord.EmployeeLName = txtCoOrdinatorLName.Text;
                        selectedCoord.EmployeePhone = txtCoOrdinatorPhone.Text;
                        selectedCoord.EmployeeEmail = txtCoOrdinatorEmail.Text;
                        selectedCoord.EmployeeLocationUnit = txtCoOrdinatorUnit.Text;
                        selectedCoord.EmployeeLocationStreet = txtCoOrdinatorStreet.Text;
                        selectedCoord.EmployeeLocationSuburb = txtCoOrdinatorSuburb.Text;
                        selectedCoord.EmployeeLocationPostcode = txtCoOrdinatorPostcode.Text;
                        selectedCoord.EmployeeLocationState = txtCoOrdinatorState.Text;
                        selectedCoord.EmployeeUsername = txtBxUsername.Text;
                        selectedCoord.EmployeePassword = txtBxPassword.Text;
                        selectedCoord.UpdateCoordinator();
                        ClearFields();
                        ShowData();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
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

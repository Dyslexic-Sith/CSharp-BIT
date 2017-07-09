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
    /// Interaction logic for Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        Login cred = new Login();
        public Client(string empType)
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
                ClientCollection myClients = new ClientCollection();
                lvClients.ItemsSource = myClients;
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Something went wrong.");
            }
        }

        private void btnClientGoBack_Click(object sender, RoutedEventArgs e)
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

        private void btnClientPageAddNewJob_Click(object sender, RoutedEventArgs e)
        {
            NewJobRequest njr = new NewJobRequest(cred.EmpType);
            njr.Show();
            Close();
        }

        private void btnClientAdd_Click(object sender, RoutedEventArgs e)
        {
                add_client();
        }

        private void add_client()
        {
            try
            {
                if (!IsValidEmail(txtClientEmail.Text))
                {
                    MessageBox.Show("Please enter a valid email address.");
                    return;
                }
                
                    //Put the text box values into the client object
                    BITClient newClient = new BITClient();

                    if (string.IsNullOrEmpty(txtClientState.Text) || string.IsNullOrEmpty(txtClientStreet.Text))
                    {
                        newClient.ClientBusinessName = txtClientBusinessName.Text.Replace("'", @"\'");
                        newClient.ClientFName = txtClientFName.Text.Replace("'", @"\'");
                        newClient.ClientLName = txtClientLName.Text.Replace("'", @"\'");
                        newClient.ClientPhone = txtClientPhone.Text;
                        newClient.ClientEmail = txtClientEmail.Text;
                        newClient.ClientFax = txtClientFax.Text;
                        newClient.ClientUsername = txtBxUsername.Text;
                        newClient.ClientPassword = txtBxPassword.Text;
                        newClient.IsPrimary = null;
                        newClient.ClientLocationID = null;
                        newClient.ClientLocationUnit = null;
                        newClient.ClientLocationStreet = null;
                        newClient.ClientLocationSuburb = null;
                        newClient.ClientLocationPostcode = null;
                        newClient.ClientLocationState = null;
                        newClient.ClientLocationDetails = null;
                        newClient.AddClientNoAddress();
                        ShowData();
                        clearFields();
                    }
                    else {
                        newClient.ClientBusinessName = txtClientBusinessName.Text.Replace("'", @"\'");
                        newClient.ClientFName = txtClientFName.Text.Replace("'", @"\'");
                        newClient.ClientLName = txtClientLName.Text.Replace("'", @"\'");
                        newClient.ClientPhone = txtClientPhone.Text;
                        newClient.ClientEmail = txtClientEmail.Text;
                        newClient.ClientFax = txtClientFax.Text;
                        newClient.ClientUsername = txtBxUsername.Text;
                        newClient.ClientPassword = txtBxPassword.Text;
                        if (chkBxMarkAsPrimary.IsChecked == true)
                        {
                            newClient.IsPrimary = 1;
                        }
                        else newClient.IsPrimary = 0;
                        newClient.ClientLocationUnit = txtClientUnit.Text;
                        newClient.ClientLocationStreet = txtClientStreet.Text.Replace("'", @"\'");
                        newClient.ClientLocationSuburb = txtClientSuburb.Text.Replace("'", @"\'");
                        newClient.ClientLocationPostcode = txtClientPostcode.Text;
                        newClient.ClientLocationState = txtClientState.Text;
                        newClient.ClientLocationDetails = txtClientDetails.Text;
                        //Add the patient to the database

                        newClient.AddClientWithAddress();
                    }
                
                //Refresh the view and clear the textboxes
                ShowData();
            clearFields();
        }
            catch (Exception ex)
            {

                MessageBox.Show("You probably need to do something with the parameters and the inputs for the stored procedure. " + ex.Message);
            }
}

        private void clearFields()
        {
            txtClientBusinessName.Clear();
            txtClientFName.Clear();
            txtClientLName.Clear();
            txtClientPhone.Clear();
            txtClientEmail.Clear();
            txtClientFax.Clear();
            txtClientUnit.Clear();
            txtClientStreet.Clear();
            txtClientSuburb.Clear();
            txtClientPostcode.Clear();
            txtClientState.Clear();
            txtClientDetails.Clear();
            txtBxPassword.Clear();
            txtBxUsername.Clear();
            chkBxMarkAsPrimary.IsChecked = false;
            lvClients.SelectedIndex = -1;
        }


        private void btnClientClear_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void btnClientDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BITClient selectedClient = (BITClient)lvClients.SelectedItem;
                selectedClient.DeleteClient();
                ShowData();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Something went wrong.");
            }

        }

        private void btnClientUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsValidEmail(txtClientEmail.Text))
                {
                    MessageBox.Show("Please enter a valid email address.");
                    return;
                }

                if (txtClientBusinessName.Text == "" || txtClientFName.Text == "" || txtClientLName.Text == "" || txtClientPhone.Text == "" || txtClientEmail.Text == "" || txtClientFax.Text == "" || txtClientUnit.Text == "" || txtClientStreet.Text == "" || txtClientSuburb.Text == "" || txtClientState.Text == "" || txtClientPostcode.Text == "" || txtClientDetails.Text == "")
                    {
                        var result = MessageBox.Show("Is this correct? Do you want to proceed with the update?", "You have some empty fields.", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            BITClient selectedClient = (BITClient)lvClients.SelectedItem;
                            selectedClient.ClientBusinessName = txtClientBusinessName.Text.Replace("'", @"\'");
                            selectedClient.ClientFName = txtClientFName.Text.Replace("'", @"\'");
                            selectedClient.ClientLName = txtClientLName.Text.Replace("'", @"\'");
                            selectedClient.ClientPhone = txtClientPhone.Text;
                            selectedClient.ClientEmail = txtClientEmail.Text;
                            selectedClient.ClientFax = txtClientFax.Text;
                            selectedClient.ClientLocationUnit = txtClientUnit.Text;
                            selectedClient.ClientLocationStreet = txtClientStreet.Text.Replace("'", @"\'");
                            selectedClient.ClientLocationSuburb = txtClientSuburb.Text.Replace("'", @"\'");
                            selectedClient.ClientLocationState = txtClientState.Text;
                            selectedClient.ClientLocationPostcode = txtClientPostcode.Text;
                            selectedClient.ClientUsername = txtBxUsername.Text;
                            selectedClient.ClientPassword = txtBxPassword.Text;
                            if (chkBxMarkAsPrimary.IsChecked == true)
                                selectedClient.IsPrimary = 1;
                            else if (chkBxMarkAsPrimary.IsChecked == false)
                                selectedClient.IsPrimary = 0;
                            selectedClient.ClientLocationDetails = txtClientDetails.Text;

                            selectedClient.UpdateClient();
                        }
                    }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Something went wrong.");
            }
            ShowData();
        }

        private void btnAddLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BITClient selectedClient = (BITClient)lvClients.SelectedItem;
                selectedClient.ClientLocationUnit = txtClientUnit.Text;
                selectedClient.ClientLocationStreet = txtClientStreet.Text.Replace("'", @"\'");
                selectedClient.ClientLocationSuburb = txtClientSuburb.Text.Replace("'", @"\'");
                selectedClient.ClientLocationState = txtClientState.Text;
                selectedClient.ClientLocationPostcode = txtClientPostcode.Text;
                if (chkBxMarkAsPrimary.IsChecked == true)
                    selectedClient.IsPrimary = 1;
                else if (chkBxMarkAsPrimary.IsChecked == false)
                    selectedClient.IsPrimary = 0;
                selectedClient.ClientLocationDetails = txtClientDetails.Text;

                selectedClient.AddNewLocation();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "We could not add this client.");
            }
            ShowData();
        }

        private void btnDeleteLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BITClient selectedClient = (BITClient)lvClients.SelectedItem;
                selectedClient.DeleteLocation();
                ShowData();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Something went wrong.");
            }
        }



        private void chkBxMarkAsPrimary_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                BITClient client = (BITClient)lvClients.SelectedItem;
                if (client == null)
                {
                    return;
                }
                else client.IsPrimary = 1;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Something went wrong.");
            }

        }

        private void chkBxMarkAsPrimary_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                BITClient client = (BITClient)lvClients.SelectedItem;
                if (client == null)
                {
                    return;
                }
                else client.IsPrimary = 0;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Something went wrong.");
            }
            
        }

        private void btnViewJobs_Click(object sender, RoutedEventArgs e)
        {
            ViewJobs view = new ViewJobs(cred.EmpType);
            Close();
            view.Show();
        }

        private void lvClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BITClient client = (BITClient)lvClients.SelectedItem;
            if (lvClients.SelectedItem == null)
            {
                chkBxMarkAsPrimary.IsChecked = false;
            }
            else if (client.IsPrimary == 1)
            { chkBxMarkAsPrimary.IsChecked = true; }
            else chkBxMarkAsPrimary.IsChecked = false;
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

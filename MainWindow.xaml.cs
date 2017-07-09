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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace BITClientServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(txtBxUsername.Text == "")
            {
                MessageBox.Show("Please enter a username.");
            }
            else if (passwordBox.Password == "")
            {
                MessageBox.Show("Please enter a password.");
            }
            else
            {
                Login login = new Login();
                login.Username = txtBxUsername.Text;
                login.Password = passwordBox.Password;
                login.getLogin();
                if (login.EmpType == "none")
                {
                    return;
                }
                else
                {
                    MainPage w = new MainPage(login.EmpType);
                    w.Show();
                    this.Close();
                }
            }
            
        }
    }
}

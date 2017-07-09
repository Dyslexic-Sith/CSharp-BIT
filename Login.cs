using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BITClientServer
{
    public class Login
    {
        #region Private Fields
        private int empID;
        private string username;
        private string password;
        private string empType;
        private string _connectionString;
        #endregion

        #region Getters and Setters
        public int EmpID
        {
            get
            {
                return empID;
            }

            set
            {
                empID = value;
            }
        }

        public string Username
        {
            get
            {
                return username;
            }

            set
            {
                username = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string EmpType
        {
            get
            {
                return empType;
            }

            set
            {
                empType = value;
            }
        }
        #endregion

        #region Public Constructors
        public Login()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public Login(int id, string user, string pass)
        {
            empID = id;
            username = user;
            password = pass;
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public Login(string user, string pass)
        {
            username = user;
            password = pass;
        }
        #endregion

        #region Public Methods
        public void getLogin()
        {
            DataAccessLayer dal = new DataAccessLayer(_connectionString);
            DataTable rowsAffected = dal.ReadRecords("SELECT * FROM Login WHERE username = '" + username + "' AND loginPassword = '" + password + "'");
            bool containsUser = rowsAffected.AsEnumerable().Any(row => username == row.Field<string>("username"));
            bool containsPass = rowsAffected.AsEnumerable().Any(row => password == row.Field<string>("loginPassword"));
            if (!containsUser || !containsPass)
            {
                MessageBox.Show("The username and password do not match our records.");
                empType = "none";
                return;
            }

            else
            {
                DataTable dt = dal.RunMySQLDataTable("SELECT employeeID FROM Login WHERE username = '" + username + "' AND loginPassword = '" + password + "'");
                empID = Convert.ToInt32(dt.Rows[0][0]);

                DataTable isCoord = dal.ReadRecords("SELECT * FROM Coordinator WHERE coordinatorID = '" + empID + "'");
                bool contains = isCoord.AsEnumerable().Any(row => empID == row.Field<int>("coordinatorID"));
                if (!contains)
                {
                    DataTable isCont = dal.ReadRecords("SELECT * FROM Administrator WHERE administratorID = '" + empID + "'");
                    bool contains1 = isCont.AsEnumerable().Any(row => empID == row.Field<int>("administratorID"));
                    if (!contains1)
                    {
                        MessageBox.Show("Login credentials are neither Administrator nor Coordinator. You do not have access to this software.");
                        empType = "none";
                        return;
                    }
                    else empType = "Administrator";
                }
                else empType = "Coordinator";
            }
        } 
        #endregion
    }
}

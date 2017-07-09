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
    public class Coordinator : Employee
    {
        #region Private Fields
        private int coordinatorID;
        private string _connectionString;

        public int CoordinatorID
        {
            get
            {
                return coordinatorID;
            }

            set
            {
                coordinatorID = value;
            }
        }
        #endregion

        #region Public Constructors
        public Coordinator() : base()
        {
            coordinatorID = this.EmployeeID;
            this._connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public Coordinator(string fname, string lname) : base()
        {
            EmployeeLName = lname;
            EmployeeFName = fname;
        }

        public Coordinator(DataRow drCoordinator) : base()
        {
            coordinatorID = int.Parse(drCoordinator["employeeID"].ToString());
            EmployeeFName = drCoordinator["employeeFirstName"].ToString();
            EmployeeLName = drCoordinator["employeeLastName"].ToString();
            EmployeePhone = drCoordinator["employeePhone"].ToString();
            EmployeeEmail = drCoordinator["employeeEmail"].ToString();
            EmployeeLocationUnit = drCoordinator["employeeAddressUnit"].ToString();
            EmployeeLocationStreet = drCoordinator["employeeAddressStreet"].ToString();
            EmployeeLocationSuburb = drCoordinator["employeeAddressSuburb"].ToString();
            EmployeeLocationState = drCoordinator["employeeAddressState"].ToString();
            EmployeeLocationPostcode = drCoordinator["employeeAddressPostcode"].ToString();
            EmployeeUsername = drCoordinator["username"].ToString();
            EmployeePassword = drCoordinator["loginPassword"].ToString();
            Status = int.Parse(drCoordinator["status"].ToString());
            this._connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public Coordinator(int empID, string empFName, string empLName, string empPhone, string empEmnail, string empLocationUnit, string empLocationStreet, string empLocationSuburb, string empLocationPostcode, string empLocationState, int status) : base()
        {
            coordinatorID = empID;
            this._connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public Coordinator(string empFName, string empLName, string empPhone, string empEmnail, string empLocationUnit, string empLocationStreet, string empLocationSuburb, string empLocationPostcode, string empLocationState, int status) : base()
        {
            coordinatorID = EmployeeID;
            this._connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }
        #endregion

        #region Public Methods
        public static DataTable ReadCoordinator()
        {
            string readQuery = "select * from Employee, Coordinator WHERE Employee.status = '1' AND coordinatorID = employeeID";
            DataAccessLayer dal = new DataAccessLayer();
            return (dal.ReadRecords(readQuery));
        }

        public void AddCoordinator()
        {
            try
            {
                DataAccessLayer DAL = new DataAccessLayer(this._connectionString);
                int rowsAffected = DAL.RunQuery("INSERT INTO Employee(employeeFirstName, employeeLastName, employeePhone, employeeEmail, employeeAddressUnit, employeeAddressStreet, employeeAddressSuburb, employeeAddressPostcode, employeeAddressState, status) VALUES ('" + EmployeeFName + "', '" + EmployeeLName + "', '" + EmployeePhone + "', '" + EmployeeEmail + "', '" + EmployeeLocationUnit + "', '" + EmployeeLocationStreet + "', '" + EmployeeLocationSuburb + "', '" + EmployeeLocationPostcode + "', '" + EmployeeLocationState + "', 1 )");
                if (rowsAffected == 0)
                {
                    throw new Exception("Coordinator was not added to the database. Error message 1");
                }
                else
                {
                    DataTable dt = DAL.RunMySQLDataTable("SELECT MAX(employeeID) FROM Employee");
                    int  coordID = Convert.ToInt32(dt.Rows[0][0]);
                    int rowsAffected2 = DAL.RunQuery("INSERT INTO Coordinator (coordinatorID) VALUES ('" + coordID + "')");
                    if (rowsAffected2 == 0)
                    {
                        throw new Exception("Coordinator was not added to the database. Error message 2");
                    }
                    else
                    {
                        int rowsAffected3 = DAL.RunQuery("INSERT INTO Login (employeeID, username, loginPassword) VALUES ('" + coordID + "', '" + EmployeeUsername + "', '" + EmployeePassword + "')");
                        if (rowsAffected3 == 0)
                        {
                            MessageBox.Show("We could not add the coordinator login.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DeleteCoordinator()
        {
            try
            {
                DataAccessLayer dal = new DataAccessLayer(_connectionString);

                int rowsAffected = dal.RunQuery("UPDATE Employee SET status = '0' WHERE employeeID ='" + this.coordinatorID + "'");
                if (rowsAffected == 0)
                {
                    throw new Exception("Coordinator was not deleted.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void UpdateCoordinator()
        {
            try
            {
                DataAccessLayer dal = new DataAccessLayer(_connectionString);
                int rowsAffected = dal.RunQuery("UPDATE Employee SET employeeFirstName = '" + EmployeeFName + "', employeeLastName = '" + EmployeeLName + "', employeePhone = '" + EmployeePhone + "', employeeEmail = '" + EmployeeEmail + "', employeeAddressUnit = '" + EmployeeLocationUnit + "', employeeAddressStreet = '" + EmployeeLocationStreet + "', employeeAddressSuburb = '" + EmployeeLocationSuburb + "', employeeAddressPostcode = '" + EmployeeLocationPostcode + "', employeeAddressState = '" + EmployeeLocationState + "' WHERE employeeID = '" + coordinatorID + "';");

                if (rowsAffected == 0)
                {
                    throw new Exception("Coordinator was not updated.");
                }
                else
                {
                    int rowsAffected2 = dal.RunQuery("UPDATE Login SET username = '" + EmployeeUsername + "', loginPassword = '" + EmployeePassword + "' WHERE employeeID = '" + coordinatorID + "'");
                    if (rowsAffected2 == 0)
                    {
                        MessageBox.Show("We could not update the coordinator login details");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        } 
        #endregion
    }
}

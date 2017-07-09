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
    public class Contractor : Employee
    {
        #region Private Field Members
        private int contractorID;
        private string _connectionString;
        private List<int> skillID;
        #endregion

        #region Public Getters and Setters
        public int ContractorID
        {
            get
            {
                return contractorID;
            }

            set
            {
                contractorID = value;
            }
        }

        public List<int> SkillID
        {
            get
            {
                return skillID;
            }

            set
            {
                skillID = value;
            }
        }

        #endregion

        #region Public Constructors
        public Contractor():base()
        {
            contractorID = this.EmployeeID;
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            skillID = new List<int>();
            
        }

        public Contractor(string fname, string lname): base()
        {
            EmployeeFName = fname;
            EmployeeLName = lname;
        }

        public Contractor(DataRow drContractor):base()
        {
            contractorID = int.Parse(drContractor["employeeID"].ToString());

            this.EmployeeFName = drContractor["employeeFirstName"].ToString();
            this.EmployeeLName = drContractor["employeeLastName"].ToString();
            this.EmployeePhone = drContractor["employeePhone"].ToString();
            this.EmployeeEmail = drContractor["employeeEmail"].ToString();
            this.EmployeeLocationUnit = drContractor["employeeAddressUnit"].ToString();
            this.EmployeeLocationStreet = drContractor["employeeAddressStreet"].ToString();
            this.EmployeeLocationSuburb = drContractor["employeeAddressSuburb"].ToString();
            this.EmployeeLocationState = drContractor["employeeAddressState"].ToString();
            this.EmployeeLocationPostcode = drContractor["employeeAddressPostcode"].ToString();
            EmployeeUsername = drContractor["username"].ToString();
            EmployeePassword = drContractor["loginPassword"].ToString();
            this.Status = int.Parse(drContractor["status"].ToString());
            skillID = new List<int>();
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }
      
        public Contractor(int employeeID, string employeeFName, string employeeLName, string employeePhone, string employeeEmail, string employeeLocationUnit, string employeeLocationStreet, string employeeLocationSuburb, string employeeLocationPostcode, string employeeLocationState, int status) : base()
        {
            contractorID = employeeID;
            skillID = new List<int>();
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public Contractor(string employeeFName, string employeeLName, string employeePhone, string employeeEmail, string employeeLocationUnit, string employeeLocationStreet, string employeeLocationSuburb, string employeeLocationPostcode, string employeeLocationState, int status) : base()
        {
            contractorID = this.EmployeeID;
            skillID = new List<int>();
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }
        #endregion

        public static DataTable ReadContractor()
        {
            string readQuery = "select * from Employee INNER JOIN Contractor ON employeeID = contractorID WHERE status = '1'";
            DataAccessLayer dal = new DataAccessLayer();
            return (dal.ReadRecords(readQuery));
        }



        public void AddContractor()
        {
            try
            {
                DataAccessLayer dal = new DataAccessLayer(_connectionString);
                int rowsAffected = dal.RunQuery("INSERT INTO Employee(employeeFirstName, employeeLastName, employeePhone, employeeEmail,  employeeAddressUnit, employeeAddressStreet, employeeAddressSuburb, employeeAddressPostcode, employeeAddressState, status) VALUES('" + EmployeeFName + "', '" + EmployeeLName + "', '" + EmployeePhone + "', '" + EmployeeEmail + "', '" + EmployeeLocationUnit + "', '" + EmployeeLocationStreet + "', '" + EmployeeLocationSuburb + "', '" + EmployeeLocationPostcode + "', '" + EmployeeLocationState + "', '" + Status + "')");

                if (rowsAffected == 0)
                {
                    throw new Exception("Contractor was not inserted into the database. Error Message 1");
                }
                else {
                    DataTable dt = dal.RunMySQLDataTable("SELECT MAX(employeeID) FROM Employee");
                    int employID = Convert.ToInt32(dt.Rows[0][0]);
                    int rowsAffected2 = dal.RunQuery("INSERT INTO Contractor (contractorID) VALUES('" + employID + "')");
                    if (rowsAffected2 == 0)
                    {
                        throw new Exception("Contractor was not inserted into the database. Error Message 2");
                    }
                    else
                    {
                        foreach(int i in SkillID)
                        {
                            int rowsAffected3 = dal.RunQuery("INSERT INTO ContractorSkills(skillsID, contractorID, status) VALUES (" + i + ", " + employID + ", 1)");
                            if (rowsAffected3 == 0)
                            {
                                throw new Exception("Contractor skills were not inserted into the database. Error Message 3");
                            }
                            else
                            {
                                int rowsAffected4 = dal.RunQuery("INSERT INTO Login(employeeID, username, loginPassword) VALUES ('" + employID + "', '" + EmployeeUsername + "', '" + EmployeePassword + "' WHERE employeeID = '" + contractorID + "')");
                                if (rowsAffected4 == 0)
                                {
                                    MessageBox.Show("We could not insert the contractor login details.");
                                }
                            }
                        }
                        
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteContractor()
        {
            try
            {
                //Create DAL
                DataAccessLayer objDAL = new DataAccessLayer(this._connectionString);
                //Execute query

                int rowsAffected2 = objDAL.RunQuery("UPDATE Employee SET status = '0' WHERE employeeID = '" + this.ContractorID + "'");
                if (rowsAffected2 == 0)
                {
                    throw new Exception("Contractor was not deleted");
                }

            }
            catch (Exception ex)
            {


                throw ex;
            }
        }

        public void UpdateContractor()
        {
            try
            {
                DataAccessLayer objDAL = new DataAccessLayer(this._connectionString);
                int rowsAffected = objDAL.RunQuery("UPDATE Employee SET employeeFirstName = '" + EmployeeFName + "', employeeLastName = '" + EmployeeLName + "', employeePhone = '" + EmployeePhone + "', employeeEmail = '" + EmployeeEmail + "', employeeAddressUnit = '" + EmployeeLocationUnit + "', employeeAddressStreet = '" + EmployeeLocationStreet + "', employeeAddressSuburb = '" + EmployeeLocationSuburb + "', employeeAddressPostcode = '" + EmployeeLocationPostcode + "', employeeAddressState = '" + EmployeeLocationState + "' WHERE employeeID = '" + ContractorID + "';");

                //Check if the client was successfully added. 1 row should be affected
                if (rowsAffected == 0)
                {
                    throw new Exception("Contractor was not updated");
                }
                else
                {
                    int rowsAffected2 = objDAL.RunQuery("DELETE FROM ContractorSkills WHERE contractorID = '" + ContractorID + "'");
                    if (rowsAffected2 != 0)
                    {
                        foreach (int i in SkillID)
                        {
                            int rowsAffected3 = objDAL.RunQuery("INSERT INTO ContractorSkills(skillsID, contractorID, status) VALUES (" + i + ", " + ContractorID + ", 1)");
                            if (rowsAffected3 == 0)
                            {
                                throw new Exception("Contractor skills were not updated");
                            }
                            else
                            {
                                int rowsAffected4 = objDAL.RunQuery("UPDATE Login SET username = '" + EmployeeUsername + "', loginPassword = '" + EmployeePassword + "' WHERE employeeID = '" + contractorID + "'");
                                if (rowsAffected4 == 0)
                                {
                                    MessageBox.Show("We could not update the contractor login details.");
                                }
                            }
                        }
                    }
                    else throw new Exception("Something went wrong.");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

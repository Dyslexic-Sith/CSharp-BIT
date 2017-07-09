using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITClientServer
{
    public class Employee
    {
        #region Private Fields
        private int employeeID;
        private string employeeFName;
        private string employeeLName;
        private string employeePhone;
        private string employeeEmail;
        private string employeeLocationUnit;
        private string employeeLocationStreet;
        private string employeeLocationSuburb;
        private string employeeLocationPostcode;
        private string employeeLocationState;
        private string employeeUsername;
        private string employeePassword;
        private int status;
        #endregion

        #region Public Getters and Setters
        public int EmployeeID
        {
            get
            {
                return employeeID;
            }

            set
            {
                employeeID = value;
            }
        }

        public string EmployeeFName
        {
            get
            {
                return employeeFName;
            }

            set
            {
                employeeFName = value;
            }
        }

        public string EmployeeLName
        {
            get
            {
                return employeeLName;
            }

            set
            {
                employeeLName = value;
            }
        }

        public string EmployeePhone
        {
            get
            {
                return employeePhone;
            }

            set
            {
                employeePhone = value;
            }
        }

        public string EmployeeEmail
        {
            get
            {
                return employeeEmail;
            }

            set
            {
                employeeEmail = value;
            }
        }

        public string EmployeeLocationUnit
        {
            get
            {
                return employeeLocationUnit;
            }

            set
            {
                employeeLocationUnit = value;
            }
        }

        public string EmployeeLocationStreet
        {
            get
            {
                return employeeLocationStreet;
            }

            set
            {
                employeeLocationStreet = value;
            }
        }

        public string EmployeeLocationSuburb
        {
            get
            {
                return employeeLocationSuburb;
            }

            set
            {
                employeeLocationSuburb = value;
            }
        }

        public string EmployeeLocationPostcode
        {
            get
            {
                return employeeLocationPostcode;
            }

            set
            {
                employeeLocationPostcode = value;
            }
        }

        public string EmployeeLocationState
        {
            get
            {
                return employeeLocationState;
            }

            set
            {
                employeeLocationState = value;
            }
        }

        public int Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        public string EmployeeUsername
        {
            get
            {
                return employeeUsername;
            }

            set
            {
                employeeUsername = value;
            }
        }

        public string EmployeePassword
        {
            get
            {
                return employeePassword;
            }

            set
            {
                employeePassword = value;
            }
        }
        #endregion

        #region Public Constructors
        public Employee(int employeeID, string employeeFName, string employeeLName, string employeePhone, string employeeEmail, string employeeLocationUnit, string employeeLocationStreet, string employeeLocationSuburb, string employeeLocationPostcode, string employeeLocationState, string username, string password, int status)
        {
            this.employeeID = employeeID;
            this.employeeFName = employeeFName;
            this.employeeLName = employeeLName;
            this.employeePhone = employeePhone;
            this.employeeEmail = employeeEmail;
            this.employeeLocationUnit = employeeLocationUnit;
            this.employeeLocationStreet = employeeLocationStreet;
            this.employeeLocationSuburb = employeeLocationSuburb;
            this.employeeLocationPostcode = employeeLocationPostcode;
            this.employeeLocationState = employeeLocationState;
            employeeUsername = username;
            employeePassword = password;
            this.status = status;

        }

        public Employee(string employeeFName, string employeeLName, string employeePhone, string employeeEmail, string employeeLocationUnit, string employeeLocationStreet, string employeeLocationSuburb, string employeeLocationPostcode, string employeeLocationState, string username, string password, int status)
        {
            this.employeeFName = employeeFName;
            this.employeeLName = employeeLName;
            this.employeePhone = employeePhone;
            this.employeeEmail = employeeEmail;
            this.employeeLocationUnit = employeeLocationUnit;
            this.employeeLocationStreet = employeeLocationStreet;
            this.employeeLocationSuburb = employeeLocationSuburb;
            this.employeeLocationPostcode = employeeLocationPostcode;
            this.employeeLocationState = employeeLocationState;
            employeeUsername = username;
            employeePassword = password;
            this.status = status;
        }

        public Employee(DataRow drEmployee)
        {
            this.employeeID = int.Parse(drEmployee["employeeID"].ToString());
            this.employeeFName = drEmployee["employeeFirstName"].ToString();
            this.employeeLName = drEmployee["employeeLastName"].ToString();
            this.employeePhone = drEmployee["employeePhone"].ToString();
            this.employeeEmail = drEmployee["employeeEmail"].ToString();
            this.employeeLocationUnit = drEmployee["employeeAddressUnit"].ToString();
            this.employeeLocationStreet = drEmployee["employeeAddressStreet"].ToString();
            this.employeeLocationSuburb = drEmployee["employeeAddressSuburb"].ToString();
            this.employeeLocationState = drEmployee["employeeAddressState"].ToString();
            this.employeeLocationPostcode = drEmployee["employeeAddressPostcode"].ToString();
            employeeUsername = drEmployee["username"].ToString();
            employeePassword = drEmployee["loginPassword"].ToString();
            this.status = int.Parse(drEmployee["status"].ToString());
        }

        public Employee()
        {

        }

        public Employee(string fname, string lname)
        {
            employeeFName = fname;
            employeeLName = lname;
        }
        #endregion

    }
}

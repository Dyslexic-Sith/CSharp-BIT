using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITClientServer
{
    [DataObject(true)]
    public class ContractorCollection : List<Contractor> 
    {
        public ContractorCollection contractorCollection { get { return contractorCollection; } set { contractorCollection = value; } }
        public ContractorCollection()
        {
            DataAccessLayer DAL = new DataAccessLayer(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

            DataTable dtContractors = DAL.RunMySQLDataTable("select * from employee, login inner join contractor where employee.employeeID = contractor.contractorID AND Employee.status = 1 AND login.employeeID = contractorID ");

            foreach (DataRow drContractor in dtContractors.Rows)
            {
                Contractor newContractor = new Contractor(drContractor);
                this.Add(newContractor);
            }
        }

        public ContractorCollection(int id)
        {
            DataAccessLayer DAL = new DataAccessLayer(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

            DataTable dtContractors = DAL.RunMySQLDataTable("select Employee.*, login.*, skillsID from employee, login inner join ContractorSkills where employee.employeeID = contractorID AND login.employeeID = employee.employeeID AND skillsID = " + id + " AND Employee.status = 1");

            foreach (DataRow drContractor in dtContractors.Rows)
            {
                Contractor newContractor = new Contractor(drContractor);
                this.Add(newContractor);
            }
        }

    }
}

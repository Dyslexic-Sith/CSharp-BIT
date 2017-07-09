using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BITClientServer
{
    [DataObject(true)]
    public class JobCollection : List<JobRequest>
    {
        public JobCollection jobCollection { get { return jobCollection; } set { jobCollection = value; } }

        //public JobCollection(int id)
        //{
        //    DataTable dtJobs;
        //    DataAccessLayer DAL = new DataAccessLayer(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

        //    DataTable rowsAffected = DAL.ReadRecords("SELECT employeeID FROM Employee WHERE employeeID = '" + id + "'");
        //    bool containsUser = rowsAffected.AsEnumerable().Any(row => id == row.Field<int>("employeeID"));
        //    DataTable rowsAffected1 = DAL.ReadRecords("SELECT clientID FROM Client WHERE clientID = '" + id + "'");
        //    bool containsUser1 = rowsAffected1.AsEnumerable().Any(row => id == row.Field<int>("clientID"));
        //    if (!containsUser && containsUser1)
        //    {
        //        dtJobs = DAL.RunMySQLDataTable("select * from JobRequest where clientID =" + id);
        //        foreach (DataRow drJob in dtJobs.Rows)
        //        {
        //            JobRequest newJob = new JobRequest(drJob);
        //            this.Add(newJob);
        //        }
        //    }
        //    else if (containsUser && !containsUser1)
        //    { dtJobs = DAL.RunMySQLDataTable("select * from JobRequest where contractorID =" + id);
        //        foreach (DataRow drJob in dtJobs.Rows)
        //        {
        //            JobRequest newJob = new JobRequest(drJob);
        //            this.Add(newJob);
        //        }
        //    }
        //    else if (!containsUser && !containsUser1)
        //    {
        //        MessageBox.Show("You done goofed, there isn't a job for this contractor or client.");

        //    }
            
        //}

        public JobCollection()
        {
            DataAccessLayer DAL = new DataAccessLayer(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

            DataTable dtJobs = DAL.RunMySQLDataTable("select JobRequest.*, ClientLocation.clientLocationSuburb, skillsTitle from JobRequest, ClientLocation, Skills WHERE JobRequest.clientLocationID = ClientLocation.clientLocationID AND JobRequest.skillsID = Skills.skillsID");

            foreach (DataRow drJob in dtJobs.Rows)
            {
                JobRequest newJob = new JobRequest(drJob);
                this.Add(newJob);
            }
        }

        public JobCollection(string status)
        {
            DataAccessLayer DAL = new DataAccessLayer(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

            DataTable dtJobs = DAL.RunMySQLDataTable("select JobRequest.*, ClientLocation.clientLocationSuburb, skillsTitle from JobRequest, ClientLocation, Skills WHERE JobRequest.clientLocationID = ClientLocation.clientLocationID AND JobRequest.skillsID = Skills.skillsID AND JobRequest.status = '" + status + "'");

            foreach (DataRow drJob in dtJobs.Rows)
            {
                JobRequest newJob = new JobRequest(drJob);
                this.Add(newJob);
            }
        }
    }
}

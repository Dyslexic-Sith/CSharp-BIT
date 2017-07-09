using MySql.Data.MySqlClient;
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
    public class JobRequest
    {
        
        #region Private Fields
        private int jobRequestID;
        private int clientID;
        private int? contractorID;
        private int? coordinatorID;
        private string jobRequestDetails;
        private int clientLocationID;
        private string clientSuburb;
        private DateTime entryDate;
        private DateTime dueDate;
        private DateTime? completionDate;
        private string status;
        private int skillID;
        private string skillTitle;
        private string _connectionString;
        #endregion

        #region Getters and Setters
        public int JobRequestID
        {
            get
            {
                return jobRequestID;
            }

            set
            {
                jobRequestID = value;
            }
        }

        public int ClientID
        {
            get
            {
                return clientID;
            }

            set
            {
                clientID = value;
            }
        }

        public int? ContractorID
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

        public int? CoordinatorID
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

        public string JobRequestDetails
        {
            get
            {
                return jobRequestDetails;
            }

            set
            {
                jobRequestDetails = value;
            }
        }

        public int ClientLocationID
        {
            get
            {
                return clientLocationID;
            }

            set
            {
                clientLocationID = value;
            }
        }

        public DateTime EntryDate
        {
            get
            {
                return entryDate;
            }

            set
            {
                entryDate = value;
            }
        }

        public DateTime DueDate
        {
            get
            {
                return dueDate;
            }

            set
            {
                dueDate = value;
            }
        }

        public DateTime? CompletionDate
        {
            get
            {
                return completionDate;
            }

            set
            {
                completionDate = value;
            }
        }

        public string Status
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

        public int SkillID
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

        public string ClientSuburb
        {
            get
            {
                return clientSuburb;
            }

            set
            {
                clientSuburb = value;
            }
        }

        public string SkillTitle
        {
            get
            {
                return skillTitle;
            }

            set
            {
                skillTitle = value;
            }
        }
        #endregion

        #region Public Methods
        public JobRequest()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public JobRequest(int clientid, int coordinatorid, string jobdetails, int locationid, DateTime createdate, DateTime datedue)
        {
            clientID = clientid;
            coordinatorID = coordinatorid;
            jobRequestDetails = jobdetails;
            clientLocationID = locationid;
            entryDate = createdate;
            dueDate = datedue;
        }
        public JobRequest(int clientiD)
        {
            clientID = clientiD;
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public JobRequest(int jobRequestID, int clientID, int? contractorID, int? coordinatorID, string jobRequestDetails, int clientLocationID, DateTime entryDate, DateTime dueDate, DateTime? completionDate, string status, int skillID, string _connectionString)
        {
            this.jobRequestID = jobRequestID;
            this.clientID = clientID;
            this.contractorID = contractorID;
            this.coordinatorID = coordinatorID;
            this.jobRequestDetails = jobRequestDetails;
            this.clientLocationID = clientLocationID;
            this.entryDate = entryDate;
            this.dueDate = dueDate;
            this.completionDate = completionDate;
            this.status = status;
            this.skillID = skillID;
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public JobRequest(int clientID, int? contractorID, int? coordinatorID, string jobRequestDetails, int clientLocationID, DateTime entryDate, DateTime dueDate, DateTime? completionDate, string status, int skillID, string _connectionString)
        {
            this.clientID = clientID;
            this.contractorID = contractorID;
            this.coordinatorID = coordinatorID;
            this.jobRequestDetails = jobRequestDetails;
            this.clientLocationID = clientLocationID;
            this.entryDate = entryDate.Date;
            this.dueDate = dueDate.Date;
            this.completionDate = completionDate;
            this.status = status;
            this.skillID = skillID;
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public JobRequest(DataRow drRequest)
        {
            DateTime createDate = Convert.ToDateTime(drRequest["jobEntryDate"].ToString());
            DateTime requiredDate = Convert.ToDateTime(drRequest["jobRequiredDate"].ToString());
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            jobRequestID = int.Parse(drRequest["jobRequestID"].ToString());
            clientID = int.Parse(drRequest["clientID"].ToString());
            contractorID = ToNullableInt(drRequest["contractorID"].ToString());
            coordinatorID = ToNullableInt(drRequest["coOrdinatorID"].ToString());
            jobRequestDetails = drRequest["jobDescription"].ToString();
            clientLocationID = int.Parse(drRequest["clientLocationID"].ToString());
            entryDate = createDate.Date;
            dueDate = requiredDate.Date;
            clientSuburb = drRequest["clientLocationSuburb"].ToString();
            skillTitle = drRequest["skillsTitle"].ToString();
            if (drRequest["jobCompletedDate"].ToString() != string.Empty)
            {
                completionDate = Convert.ToDateTime(drRequest["jobCompletedDate"].ToString());
                // DateTime.TryParse(drRequest["jobCompletedDate"].ToString(), out completionDate);

            }
            else completionDate = null;
            skillID = int.Parse(drRequest["skillsID"].ToString());
            status = drRequest["status"].ToString();
        }

        public static DataTable ReadSubmittedJobs()
        {
            string readQuery = "SELECT * FROM JobRequest WHERE status = 'Submitted'";
            DataAccessLayer dal = new DataAccessLayer();
            return dal.ReadRecords(readQuery);
        }

        public static DataTable ReadAssignedJobs()
        {
            string readQuery = "SELECT * FROM JobRequest WHERE status = 'Assigned'";
            DataAccessLayer dal = new DataAccessLayer();
            return dal.ReadRecords(readQuery);
        }

        public static DataTable ReadCompletedJobs()
        {
            string readQuery = "SELECT * FROM JobRequest WHERE status = 'Completed'";
            DataAccessLayer dal = new DataAccessLayer();
            return dal.ReadRecords(readQuery);
        }

        public static DataTable ReadAllJobs()
        {
            string readQuery = "SELECT * FROM JobRequest";
            DataAccessLayer dal = new DataAccessLayer();
            return dal.ReadRecords(readQuery);
        }

        public static DataTable ReadJobsForClient(int clientID)
        {
            string readQuery = "SELECT * FROM JobRequest WHERE clientID = '" + clientID + "'";
            DataAccessLayer dal = new DataAccessLayer();
            return dal.ReadRecords(readQuery);
        }

        public void AddJobRequest()
        {
            try
            {
                DataAccessLayer dal = new DataAccessLayer(_connectionString);
                int rowsAffected = dal.RunQuery("INSERT INTO JobRequest(clientID, clientLocationID, coOrdinatorID, skillsID, jobDescription, jobEntryDate, jobRequiredDate, status) VALUES('" + clientID + "', '" + clientLocationID + "', '" + coordinatorID + "', '" + skillID + "', '" + jobRequestDetails + "', '" + entryDate + "', '" + dueDate.Date.ToString("yyyy-MM-dd") + "', '" + status + "')");

                if (rowsAffected == 0)
                {
                    throw new Exception("Job was not inserted into the database. Error Message 1");
                }
                else { MessageBox.Show("Job request was successfully added."); }
            }
            catch(MySqlException sqlEx)
            {
                MessageBox.Show(sqlEx.Message + sqlEx.Number, "Nope.");
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.InnerException.Message, "Nope.");
                
            }
        }

        public void AssignContractor(int id)
        {
            try
            {
                DataAccessLayer dal = new DataAccessLayer(_connectionString);
                int rowsAffected = dal.RunQuery("UPDATE JobRequest SET contractorID = " + id + ", status = 'Assigned' WHERE jobRequestID = " + this.jobRequestID);
                if (rowsAffected == 0)
                {
                    throw new Exception("Contractor was not inserted into the job request record. Error Message 1");
                }
                else { MessageBox.Show("Contractor was successfully assigned to the job."); }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void MarkJobAsCompleted()
        {
            try
            {
                DataAccessLayer dal = new DataAccessLayer(_connectionString);
                int rowsAffected = dal.RunQuery("UPDATE JobRequest SET status = 'Completed' WHERE jobRequestID = " + this.jobRequestID);
                if (rowsAffected == 0)
                {
                    MessageBox.Show("Job was not marked as completed. Please try again later.");
                }
                else { MessageBox.Show("Job has been completed."); }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public int? ToNullableInt(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        } 
        #endregion

    }
}

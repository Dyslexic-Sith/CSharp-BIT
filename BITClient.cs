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
    public class BITClient
    {
        #region Private Fields
        private int clientID;
        private string clientBusinessName;
        private string clientFName;
        private string clientLName;
        private string clientPhone;
        private string clientEmail;
        private string clientFax;
        private string clientUsername;
        private string clientPassword;

        private int? clientLocationID;
        private string clientLocationUnit;
        private string clientLocationStreet;
        private string clientLocationSuburb;
        private string clientLocationState;
        private string clientLocationPostcode;
        private int? isPrimary;
        private string clientLocationDetails;

        private DataTable _dtClient;
        private string _connectionString; 
        private DataRow drClient; 
        #endregion

        #region Getters and Setters
        public string ClientBusinessName
        {
            get
            {
                return clientBusinessName;
            }

            set
            {
                clientBusinessName = value;
            }
        }

        public string ClientFName
        {
            get
            {
                return clientFName;
            }

            set
            {
                clientFName = value;
            }
        }

        public string ClientLName
        {
            get
            {
                return clientLName;
            }

            set
            {
                clientLName = value;
            }
        }

        public string ClientPhone
        {
            get
            {
                return clientPhone;
            }

            set
            {
                clientPhone = value;
            }
        }

        public string ClientEmail
        {
            get
            {
                return clientEmail;
            }

            set
            {
                clientEmail = value;
            }
        }

        public string ClientFax
        {
            get
            {
                return clientFax;
            }

            set
            {
                clientFax = value;
            }
        }

        public string ClientLocationUnit
        {
            get
            {
                return clientLocationUnit;
            }

            set
            {
                clientLocationUnit = value;
            }
        }

        public string ClientLocationStreet
        {
            get
            {
                return clientLocationStreet;
            }

            set
            {
                clientLocationStreet = value;
            }
        }

        public string ClientLocationSuburb
        {
            get
            {
                return clientLocationSuburb;
            }

            set
            {
                clientLocationSuburb = value;
            }
        }

        public string ClientLocationState
        {
            get
            {
                return clientLocationState;
            }

            set
            {
                clientLocationState = value;
            }
        }

        public string ClientLocationPostcode
        {
            get
            {
                return clientLocationPostcode;
            }

            set
            {
                clientLocationPostcode = value;
            }
        }

        public int? IsPrimary
        {
            get
            {
                return isPrimary;
            }

            set
            {
                isPrimary = value;
            }
        }

        public string ClientLocationDetails
        {
            get
            {
                return clientLocationDetails;
            }

            set
            {
                clientLocationDetails = value;
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

        public int? ClientLocationID
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

        public string ClientUsername
        {
            get
            {
                return clientUsername;
            }

            set
            {
                clientUsername = value;
            }
        }

        public string ClientPassword
        {
            get
            {
                return clientPassword;
            }

            set
            {
                clientPassword = value;
            }
        }
        #endregion

        #region Public Constructors

        public BITClient(string clientBusinessName, string clientFName, string clientLName, string clientPhone, string clientEmail, string clientFax)
        {

            this.clientBusinessName = clientBusinessName;
            this.clientFName = clientFName;
            this.clientLName = clientLName;
            this.clientPhone = clientPhone;
            this.clientEmail = clientEmail;
            this.clientFax = clientFax;
            this._connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public BITClient()
        {
        }

        public BITClient(string fname, string lname)
        {
            clientFName = fname;
            clientLName = lname;
        }

        public BITClient(DataRow drClient)
        {
            this.clientID = int.Parse(drClient["ClientID"].ToString());
            this.clientBusinessName = drClient["clientBusinessName"].ToString();
            this.ClientFName = drClient["ClientFirstName"].ToString();
            this.clientLName = drClient["ClientLastName"].ToString();
            this.clientPhone = drClient["ClientPhone"].ToString();
            this.clientEmail = drClient["ClientEmail"].ToString();
            this.clientFax = drClient["ClientFax"].ToString();
            this.clientLocationID = ToNullableInt((drClient["ClientLocationID"].ToString()));
            this.clientLocationUnit = drClient["ClientLocationUnit"].ToString();
            this.clientLocationStreet = drClient["ClientLocationStreet"].ToString();
            this.clientLocationSuburb = drClient["ClientLocationSuburb"].ToString();
            this.clientLocationState = drClient["ClientLocationState"].ToString();
            this.clientLocationPostcode = drClient["ClientLocationPostcode"].ToString();
            this.clientLocationDetails = drClient["ClientLocationDetails"].ToString();
            clientUsername = drClient["clientUsername"].ToString();
            clientPassword = drClient["clientPassword"].ToString();
            this.isPrimary = ToNullableInt((drClient["IsPrimary"].ToString()));
            this._connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        
        public BITClient(string clientBusinessName, string clientFName, string clientLName, string clientPhone, string clientEmail, string clientFax, int? clientLocationID, string clientLocationUnit, string clientLocationStreet, string clientLocationSuburb, string clientLocationPostcode, string clientLocationState, string clientLocationDetails, int? isPrimary)
        {
            this.clientBusinessName = clientBusinessName;
            this.clientFName = clientFName;
            this.clientLName = clientLName;
            this.clientPhone = clientPhone;
            this.clientEmail = clientEmail;
            this.clientFax = clientFax;
            this.clientLocationID = clientLocationID;
            this.clientLocationUnit = clientLocationUnit;
            this.clientLocationStreet = clientLocationStreet;
            this.clientLocationSuburb = clientLocationSuburb;
            this.clientLocationState = clientLocationState;
            this.clientLocationPostcode = clientLocationPostcode;
            this.isPrimary = isPrimary;
            this.clientLocationDetails = clientLocationDetails;
            this._connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This is to be used when getting the information from both the Client table and the ClientLocation table
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable ReadClient()
        {
            string readQuery = "select * from Client INNER JOIN ClientLocation ON Client.clientID = ClientLocation.clientID WHERE Client.status = '1' ORDER BY Client.clientID";
            DataAccessLayer dal = new DataAccessLayer();
            return (dal.ReadRecords(readQuery));
        }
        /// <summary>
        /// This is to be used when you need just the Client information (Name, Phone, etc) and not the Location information
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable ReadClientCombo()
        {
            string readQuery = "SELECT * from Client WHERE status = '1'";
            DataAccessLayer dal = new DataAccessLayer();
            return (dal.ReadRecords(readQuery));
        }

        public static DataTable ReadClientWithID(int id)
        {
            string readQuery = "select * from Client, ClientLogin INNER JOIN ClientLocation ON Client.clientID = ClientLocation.clientID WHERE Client.status = '1' AND Client.clientID = '" + id + "' AND ClientLogin.clientID = '" + id + "' ORDER BY Client.clientID";
            DataAccessLayer dal = new DataAccessLayer();
            return (dal.ReadRecords(readQuery));
        }

        public static DataTable ReadClientLocation(int clientid)
        {
            string readQuery = "SELECT * FROM ClientLocation WHERE clientID = '" + clientid + "';";
            DataAccessLayer dal = new DataAccessLayer();
            return (dal.ReadRecords(readQuery));
        }

        /// <summary>
        /// This is used when the Client does not have a Location associated with them.
        /// </summary>
        public void AddClientNoAddress()
        {
            try
            {


                DataAccessLayer objDAL = new DataAccessLayer(this._connectionString);

                int rowsAffected = objDAL.RunQuery("INSERT INTO Client( clientFirstName, clientLastName, clientPhone, clientEmail, clientFax, clientBusinessName, status) VALUES ('" + ClientFName + "', '" + clientLName + "', '" + clientPhone + "', '" + clientEmail + "', '" + clientFax + "', '" + clientBusinessName + "', 1 )");

                //Check if the client was successfully added. 1 row should be affected
                if (rowsAffected == 0)
                {
                    throw new Exception("Client was not added to the database");
                }
                else
                {
                    DataTable dt = objDAL.RunMySQLDataTable("SELECT MAX(clientID) FROM Client");
                    int nclientID = Convert.ToInt32(dt.Rows[0][0]);
                    int rowsAffected2 = objDAL.RunQuery("INSERT INTO ClientLogin (clientID, clientUsername, clientPassword) VALUES ('" + nclientID + "', '" + clientUsername + "', '" + clientPassword + "')");
                    if (rowsAffected2 == 0)
                    {
                        MessageBox.Show("We could not insert the client login details.");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// This is used when there is a Location associated with the Client
        /// </summary>
        public void AddClientWithAddress()
        {
            try
            {


                DataAccessLayer objDAL = new DataAccessLayer(this._connectionString);

                int rowsAffected = objDAL.RunQuery("INSERT INTO Client( clientFirstName, clientLastName, clientPhone, clientEmail, clientFax, clientBusinessName, status) VALUES ('" + ClientFName + "', '" + clientLName + "', '" + clientPhone + "', '" + clientEmail + "', '" + clientFax + "', '" + clientBusinessName + "', 1 )");

                //Check if the client was successfully added. 1 row should be affected
                if (rowsAffected == 0)
                {
                    throw new Exception("Client was not added to the database. Error Message 1");
                }
                else
                {
                    DataTable dt = objDAL.RunMySQLDataTable("SELECT MAX(clientID) FROM Client");
                    int clientID = Convert.ToInt32(dt.Rows[0][0]);
                    int rowsAffected2 = objDAL.RunQuery("INSERT INTO ClientLocation( clientID, clientLocationUnit, clientLocationStreet, clientLocationSuburb, clientLocationPostcode, clientLocationState, clientLocationDetails, isPrimary ) VALUES ( '" + clientID + "', '" + clientLocationUnit + "', '" + clientLocationStreet + "', '" + clientLocationSuburb + "', '" + clientLocationPostcode + "', '" + clientLocationState + "', '" + clientLocationDetails + "', '" + isPrimary + "' )");

                    if (rowsAffected2 == 0)
                    {
                        throw new Exception("Client Location was not added to the database. Error Message 2");
                    }
                    else
                    {
                        DataTable dt1 = objDAL.RunMySQLDataTable("SELECT MAX(clientID) FROM Client");
                        int nclientID = Convert.ToInt32(dt1.Rows[0][0]);
                        int rowsAffected3 = objDAL.RunQuery("INSERT INTO ClientLogin (clientID, clientUsername, clientPassword) VALUES ('" + nclientID + "', '" + clientUsername + "', '" + clientPassword + "')");
                        if (rowsAffected3 == 0)
                        {
                            MessageBox.Show("We could not insert the client login details.");
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// This is used to removed the Client from the database, in this instance it changes the client status from Active (1), to Inactive (0)
        /// </summary>
        public void DeleteClient()
        {
            try
            {
                //Create DAL
                DataAccessLayer objDAL = new DataAccessLayer(this._connectionString);
                //Execute query

                int rowsAffected2 = objDAL.RunQuery("UPDATE Client SET status = '0' WHERE clientID = '" + this.clientID + "'");
                if (rowsAffected2 == 0)
                {
                    throw new Exception("Client was not deleted");
                }

            }
            catch (Exception ex)
            {


                throw ex;
            }
        }

        public void UpdateClient()
        {
            try
            {
                DataAccessLayer objDAL = new DataAccessLayer(this._connectionString);
                int rowsAffected = objDAL.RunQuery("UPDATE Client SET clientFirstName = '" + ClientFName + "', clientLastName = '" + clientLName + "', clientPhone = '" + clientPhone + "', clientEmail = '" + clientEmail + "', clientFax = '" + clientFax + "', clientBusinessName = '" + clientBusinessName + "' WHERE clientID = '" + clientID + "';");

                //Check if the client was successfully added. 1 row should be affected
                if (rowsAffected == 0)
                {
                    throw new Exception("Client was not updated");
                }
                else
                {
                    int rowsAffected2 = objDAL.RunQuery("UPDATE ClientLocation SET clientLocationUnit = '" + clientLocationUnit + "', clientLocationStreet = '" + clientLocationStreet + "', clientLocationSuburb = '" + clientLocationSuburb + "', clientLocationPostcode = '" + clientLocationPostcode + "', clientLocationState = '" + clientLocationState + "', clientLocationDetails = '" + clientLocationDetails + "', isPrimary = '" + isPrimary + "' WHERE clientID = '" + clientID + "' AND clientLocationID = '" + clientLocationID + "';");

                    if (rowsAffected2 == 0)
                    {
                        throw new Exception("Client Location was not updated");
                    }
                    else
                    {
                        int rowsAffected3 = objDAL.RunQuery("UPDATE ClientLogin SET clientUsername = '" + clientUsername + "', clientPassword = '" + clientPassword + "' WHERE clientID = '" + clientID + "'");
                        if (rowsAffected3 == 0)
                        {
                            MessageBox.Show("We could not update the client login details.");
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddNewLocation()
        {
            try
            {
                DataAccessLayer objDAL = new DataAccessLayer(this._connectionString);
                int rowsAffected = objDAL.RunQuery("INSERT INTO ClientLocation( clientID, clientLocationUnit, clientLocationStreet, clientLocationSuburb, clientLocationPostcode, clientLocationState, clientLocationDetails, isPrimary ) VALUES ( '" + clientID + "', '" + clientLocationUnit + "', '" + clientLocationStreet + "', '" + clientLocationSuburb + "', '" + clientLocationPostcode + "', '" + clientLocationState + "', '" + clientLocationDetails + "', '" + isPrimary + "' )");
                if (rowsAffected == 0)
                {
                    throw new Exception("Client Location was not added. Message 1");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("We could not insert a new entry into the Client Location table. Message 2", ex);
            }
        }

        public void DeleteLocation()
        {
            try
            {
                DataAccessLayer objDAL = new DataAccessLayer(this._connectionString);
                int rowsAffected = objDAL.RunQuery("DELETE FROM ClientLocation WHERE clientLocationID ='" + this.clientLocationID + "';");
                if (rowsAffected == 0)
                {
                    throw new Exception("Client Location could not be deleted. Message 1");
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Client Location could not be deleted. Message 2", ex);

            }
        }
        public int? ToNullableInt(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }

        public override string ToString()
        {
            return ClientFName + " " + ClientLName;
        }
        #endregion

    }
}

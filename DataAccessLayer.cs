using System;
using System.Configuration;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Collections;
using System.Diagnostics;

namespace BITClientServer
{

    public sealed class DataAccessLayer : System.ComponentModel.Component
    {
        #region Private Member Variables
        // ADO.NET objects are defined
        private MySqlConnection _sqlConnection = null;
        private MySqlCommand _sqlCommand;
        private MySqlDataAdapter _sqlDataAdapter;
        private MySqlDataReader _sqlDataReader;
        private DataSet _dataSet;
        private DataTable _dataTable;
        private XmlReader _xmlReader;

        // This array will hold the parameters for the sql commands
        private ArrayList _parameterList;

        // Connection string variables are defined
        private string _connectionString;
        private string _server;
        private string _database;
        private string _username;
        private string _password;

        // Naming, cleanup and exception variables are defined
        private string _moduleName;
        private const string _exceptionMessage = "Data application error. Detailed error information can be found in the application log.";
        private bool _disposedBoolean;

        #endregion Private Member Variables

        #region Public Properties
        // This enumeration will be used to convert the non-MySQL server datatypes passed from the UI tier or the BO tier to this DAL
        public enum GenericDataType
        {
            genString,
            genChar,
            genInteger,
            genBit,
            genDateTime,
            genDate,
            genTime,
            genDecimal,
            genMoney,
            genImage
        }

        public string ConnectionString
        {
            get
            {
                //If the ConnectionString doesn't exist, catch the exception and return an empty string
                try
                {
                    return _connectionString;
                }
                catch
                {
                    return "";
                }
            }
            set { _connectionString = value; }
        }

        public string Server
        {
            get
            {
                try
                {
                    return _server;
                }
                catch
                {
                    return "";
                }
            }

            set { _server = value; }
        }

        public string Database
        {
            get
            {
                try
                {
                    return _database;
                }
                catch
                {
                    return "";
                }
            }

            set { _database = value; }
        }

        public string Username
        {
            get
            {
                try
                {
                    return _username;
                }
                catch
                {
                    return "";
                }
            }

            set { _username = value; }
        }

        public string Password
        {
            get
            {
                try
                {
                    return _password;
                }
                catch
                {
                    return "";
                }
            }

            set { _password = value; }
        }
        #endregion Public Properties

        #region Public Constructors
        // 3 constructors
        // 1 has no parameters, it just stores the module name
        // 2 is used if we know the connection string and we can pass it to the constructor
        // 3 accepts all the parts of the connection string and then builds it internally

        /// <summary>
        /// Create a data access layer object
        /// </summary>
        public DataAccessLayer() : base()
        {
            _moduleName = this.GetType().ToString();
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        /// <summary>
        /// Create a data access layer object giving a full connection string.
        /// </summary>
        /// <param name="connectionString">A connection string for the database.</param>
        public DataAccessLayer(string connectionString) : base()
        {
            _connectionString = connectionString;
            _moduleName = this.GetType().ToString();
        }

        /// <summary>
        /// Create a data access layer object giving all database connection details.
        /// </summary>
        /// <param name="server">The database server name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="username">The database username.</param>
        /// <param name="password">The database password</param>
        public DataAccessLayer(string server, string database, string username, string password) : base()
        {
            _connectionString = "Server=" + server + ";Database=" + database + ";User ID=" + username + ";Password=" + password + ";";
            _moduleName = this.GetType().ToString();
        }

        #endregion Public Constructors

        #region Data Access Methods
        //Run MySQL data table
        /// <summary>
        /// Executes a MySQL string and returns a DataTable object.
        /// Use for non-updatable, forward only, read only data access.
        /// </summary>
        /// <param name="strMySql">string: The MySQL Statement to execute</param>
        /// <returns>System.Data.DataTable</returns>
        public DataTable RunMySQLDataTable(string strMySql)
        {
            // Validate the SQL string
            ValidateMySqlStatement(strMySql);
            try
            {
                // Check to see if this object has already been disposed
                if (_disposedBoolean)
                {
                    throw new ObjectDisposedException(_moduleName, "this object has already been disposed. You cannot reuse it.");
                }

                // Create connection
                _sqlConnection = new MySqlConnection(_connectionString);

                // Create command using the MySQL statement and the connection
                // The commandtype doesn't have to be set because the default is "Text"
                _sqlCommand = new MySqlCommand(strMySql, _sqlConnection);

                // Open the connection - need to explicitly open the connection for the DataReader
                _sqlCommand.Connection.Open();

                // Execute the command
                _sqlDataReader = _sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

                // Translate reader into a data table
                _dataTable = new DataTable();
                _dataTable.Load(_sqlDataReader, LoadOption.OverwriteChanges);

                // Return the datatable
                return _dataTable;
            }
            catch (Exception ex)
            {
                // Log the exception through our private LogException Method
               // LogException(ex);
                throw new Exception(_exceptionMessage, ex);
            }
            finally
            {
                // Check if the connection is still open, and close it if it is
                if (_sqlCommand.Connection.State == ConnectionState.Open)
                {
                    _sqlCommand.Connection.Close();
                }
            }
        }

        // Run SP DataTable
        public DataTable RunSPDataTable(string SPName)
        {
            // Validate the MySQL statement
            ValidateSPStatement(SPName);

            try
            {
                // Setting objects to handle parameters
                Parameter myGenericParameter;
                MySqlParameter myMySqlParameter;

                // the usedEnumerator makes it easy to step through the ArrayList of parameters
                IEnumerator usedEnumerator = _parameterList.GetEnumerator();

                // Check to see if this object has already been disposed
                if (_disposedBoolean)
                {
                    throw new ObjectDisposedException(_moduleName, "This object has already been disposed. You cannot reuse it.");
                }

                // Create connection
                _sqlConnection = new MySqlConnection(_connectionString);

                // Define the command object and set the commandType to process a stored procedure
                _sqlCommand = new MySqlCommand(SPName, _sqlConnection);
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                while (usedEnumerator.MoveNext())
                {
                    myGenericParameter = null;
                    myGenericParameter = (Parameter)usedEnumerator.Current;
                    //Convert the parameter to a MySqlParameter
                    myMySqlParameter = convertParameters(myGenericParameter);
                    _sqlCommand.Parameters.Add(myMySqlParameter);
                }

                // Open the connection
                _sqlCommand.Connection.Open();

                // Execute the command
                _sqlDataReader = _sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

                // Translate reader into a data table
                _dataTable = new DataTable();
                _dataTable.Load(_sqlDataReader, LoadOption.OverwriteChanges);

                // Return the data table
                return _dataTable;
            }
            catch (Exception ex)
            {
                //LogException(ex);
                throw new Exception(_exceptionMessage, ex);
            }
            finally
            {
                if (_sqlCommand.Connection.State == ConnectionState.Open)
                {
                    _sqlCommand.Connection.Close();
                }
            }


        }

        /// <summary>
        /// Executes a Stored Procedure (SPName) and returns a scalar
        /// </summary>
        /// <param name="SPName">string: The name of the Stored Procedure to execute</param>
        /// <returns>object</returns>
        public object RunSPScalar(string SPName)
        {
            ValidateSPStatement(SPName);

            try
            {
                Parameter myGenericParameter;
                MySqlParameter myMySqlParameter;

                IEnumerator usedEnumerator = _parameterList.GetEnumerator();

                if (_disposedBoolean)
                {
                    throw new ObjectDisposedException(_moduleName, "This object has already been disposed. You cannot reuse it.");
                }

                // Create connection
                _sqlConnection = new MySqlConnection(_connectionString);

                // Define the command object and set the commandType to process a stored procedure
                _sqlCommand = new MySqlCommand(SPName, _sqlConnection);
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                while (usedEnumerator.MoveNext())
                {
                    myGenericParameter = null;
                    myGenericParameter = (Parameter)usedEnumerator.Current;
                    //Convert the parameter to a MySqlParameter
                    myMySqlParameter = convertParameters(myGenericParameter);
                    _sqlCommand.Parameters.Add(myMySqlParameter);
                }

                // Open the connection
                _sqlCommand.Connection.Open();

                // Execute Scalar returns the first column of the first row in the result set. Additional columns or rows are ignored.
                object objResult = _sqlCommand.ExecuteScalar();
                return objResult;

            }
            catch (Exception ex)
            {
                //LogException(ex);
                throw new Exception(_exceptionMessage, ex);
            }
            finally
            {
                if (_sqlCommand.Connection.State == ConnectionState.Open)
                {
                    _sqlCommand.Connection.Close();
                }
            }
        }
            // Run SP Data Set
            /// <summary>
            /// Executes a stored procedure and returns a DataSet object.
            /// </summary>
            /// <param name="SPName">string: The name of the stored procedure to execute.</param>
            /// <returns></returns>
            public DataSet RunSPDataSet(string SPName)
        {
            ValidateSPStatement(SPName);

            try
            {
                Parameter myGenericParameter;
                MySqlParameter myMySqlParameter;

                IEnumerator usedEnumerator = _parameterList.GetEnumerator();

                if (_disposedBoolean)
                {
                    throw new ObjectDisposedException(_moduleName, "This object has already been disposed. You cannot reuse it.");
                }

                // Create connection
                _sqlConnection = new MySqlConnection(_connectionString);

                // Define the command object and set the commandType to process a stored procedure
                _sqlCommand = new MySqlCommand(SPName, _sqlConnection);
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                while (usedEnumerator.MoveNext())
                {
                    myGenericParameter = null;
                    myGenericParameter = (Parameter)usedEnumerator.Current;
                    //Convert the parameter to a MySqlParameter
                    myMySqlParameter = convertParameters(myGenericParameter);
                    _sqlCommand.Parameters.Add(myMySqlParameter);
                }

                // Use DataAdapter to fill DataSet object
                _sqlDataAdapter = new MySqlDataAdapter(_sqlCommand);
                _dataSet = new DataSet();
                // The DataAdapter will manage the opening and closing of the connection
                _sqlDataAdapter.Fill(_dataSet);
                return _dataSet;

            }
            catch (Exception ex)
            {
                //LogException(ex);
                throw new Exception(_exceptionMessage, ex);
            }
            finally
            {
                if (_sqlCommand.Connection.State == ConnectionState.Open)
                {
                    _sqlCommand.Connection.Close();
                }
            }
        }
        
        // Run SP non-query
        /// <summary>
        /// Executes a stored procedure and returns the number of rows affected. Use for INSERTs, UPDATEs and DELETEs.
        /// If the stored procedure fails, an Exception will be thrown.
        /// </summary>
        /// <param name="SPName">string: The name of the stored procedure to execute.</param>
        /// <returns>The number of rows affected (int).</returns>
        public int RunSPExecNonQuery(string SPName)
        {
            ValidateSPStatement(SPName);

            try
            {
                Parameter myGenericParameter;
                MySqlParameter myMySqlParameter;

                IEnumerator usedEnumerator = _parameterList.GetEnumerator();

                if (_disposedBoolean)
                {
                    throw new ObjectDisposedException(_moduleName, "This object has already been disposed. You cannot reuse it.");
                }

                // Create connection
                _sqlConnection = new MySqlConnection(_connectionString);

                // Define the command object and set the commandType to process a stored procedure
                _sqlCommand = new MySqlCommand(SPName, _sqlConnection);
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                while (usedEnumerator.MoveNext())
                {
                    myGenericParameter = null;
                    myGenericParameter = (Parameter)usedEnumerator.Current;
                    //Convert the parameter to a MySqlParameter
                    myMySqlParameter = convertParameters(myGenericParameter);
                    _sqlCommand.Parameters.Add(myMySqlParameter);
                }

                // Open the connection and execute the command.
                // If the sp fails, an exception will be thrown, otherwise the number of rows affected will be returned
                _sqlCommand.Connection.Open();
                return _sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //LogException(ex);
                throw new Exception(_exceptionMessage, ex);
            }
            finally
            {
                if (_sqlCommand.Connection.State == ConnectionState.Open)
                {
                    _sqlCommand.Connection.Close();
                }
            }
        }

        public int RunQuery(string Query)
        {
            try
            {
                _sqlConnection = new MySqlConnection(_connectionString);

                // Define the command object and set the commandType to process a stored procedure
                _sqlCommand = new MySqlCommand(Query, _sqlConnection);
                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.Connection.Open();
                return _sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               // LogException(ex);
                throw new Exception(_exceptionMessage, ex);

            }
            finally
            {
                if (_sqlCommand.Connection.State == ConnectionState.Open)
                {
                    _sqlCommand.Connection.Close();
                }
            }
            
        }

        #endregion Data Access Methods

        private void ConnectDB()
        {
            string connString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            _sqlConnection = new MySqlConnection(connString);
        }

        public DataTable ReadRecords(string readQuery)
        {
            try
            {
                ConnectDB();
                MySqlCommand readcmd = new MySqlCommand(readQuery, _sqlConnection);
                MySqlDataAdapter readAdap = new MySqlDataAdapter(readcmd);
                DataTable dt = new DataTable();
                readAdap.Fill(dt);
                return dt;
            }
            catch (MySqlException sqle)
            {
                DataTable emptydt = new DataTable();
                MessageBox.Show(sqle.Message);
                return emptydt;
            }
        }

        private class Parameter
        {
            #region Private and Public Fields and Properties
            private string _parameterName;
            private object _parameterValue;
            private MySqlDbType _parameterDataType;
            private int _parameterSize;
            private ParameterDirection _parameterDirection;

            public string ParameterName
            {
                get
                {
                    return _parameterName;
                }

                set
                {
                    _parameterName = value;
                }
            }

            public object ParameterValue
            {
                get
                {
                    return _parameterValue;
                }

                set
                {
                    _parameterValue = value;
                }
            }

            public MySqlDbType ParameterDataType
            {
                get
                {
                    return _parameterDataType;
                }

                set
                {
                    _parameterDataType = value;
                }
            }

            public int ParameterSize
            {
                get
                {
                    return _parameterSize;
                }

                set
                {
                    _parameterSize = value;
                }
            }

            public ParameterDirection ParameterDirection
            {
                get
                {
                    return _parameterDirection;
                }

                set
                {
                    _parameterDirection = value;
                }
            }
            #endregion Private and Public Fields and Properties

            #region Constructors
            public Parameter(string passedName)
            {
                _parameterName = passedName;
            }

            public Parameter(string passedName, object passedValue)
            {
                _parameterName = passedName;
                _parameterValue = passedValue;
            }

            public Parameter(string passedName, object passedValue, MySqlDbType passedSqlType)
            {
                _parameterName = passedName;
                _parameterValue = passedValue;
                _parameterDataType = passedSqlType;
            }

            public Parameter(string passedName, object passedValue, MySqlDbType passedSqlType, int passedSize)
            {
                _parameterName = passedName;
                _parameterValue = passedValue;
                _parameterDataType = passedSqlType;
                _parameterSize = passedSize;
            }

            public Parameter(string passedName, object passedValue, MySqlDbType passedSqlType, int passedSize, ParameterDirection passedDirection)
            {
                _parameterName = passedName;
                _parameterValue = passedValue;
                _parameterDataType = passedSqlType;
                _parameterSize = passedSize;
                _parameterDirection = passedDirection;
            }
            #endregion Constructors
        }

        #region addParameter Public Methods
        /*The minimum requirement for MySQL Server
        * Stored Procedures is the Parameter Name, value and data type
        */
        /// <summary>
        /// Allows non MySqlDbType parameters to be passed to the DataAccessLayer which
        /// will convert the parameters internally to MySQL Server specific data types
        /// </summary>
        /// <param name="parameterName">System.String The name of the parameter</param>
        /// <param name="value">System.object The value of the parameter</param>
        /// <param name="genType">DataAccessLayer.GenericDataType The generic data type of the parameter</param>
        public void addParameter(string parameterName, object value, GenericDataType genType)
        {
            MySqlDbType targetMySqlDataType = default(MySqlDbType);
            Parameter buildParameter = null;
            _parameterList = new ArrayList();
            // Call to convertToMySqlDbType that does the conversion
            targetMySqlDataType = convertToMySqlDbType(genType);
            buildParameter = new Parameter(parameterName, value, targetMySqlDataType);
            _parameterList.Add(buildParameter);
        }

        /// <summary>
        /// Allows non MySqlDbType parameters to be passed to the DataAccessLayer which
        /// will convert the parameters internally to MySQL Server specific data types
        /// </summary>
        /// <param name="parameterName">System.String The name of the parameter</param>
        /// <param name="value">System.object The value of the parameter</param>
        /// <param name="genType">DataAccessLayer.GenericDataType The generic data type of the parameter</param>
        /// <param name="size">System.Int32 The size of the parameter</param>
        public void addParameter(string parameterName, object value, GenericDataType genType, int size)
        {
            MySqlDbType targetMySqlDataType = default(MySqlDbType);
            Parameter buildParameter = null;
            _parameterList = new ArrayList();
            targetMySqlDataType = convertToMySqlDbType(genType);
            buildParameter = new Parameter(parameterName, value, targetMySqlDataType, size);
            _parameterList.Add(buildParameter);
        }

        /// <summary>
        /// Allows non MySqlDbType parameters to be passed to the DataAccessLayer which
        /// will convert the parameters internally to MySQL Server specific data types
        /// </summary>
        /// <param name="parameterName">System.String The name of the parameter</param>
        /// <param name="value">System.object The value of the parameter</param>
        /// <param name="genType">DataAccessLayer.GenericDataType The generic data type of the parameter</param>
        /// <param name="size">System.Int32 The size of the parameter</param>
        /// <param name="direction">System.Data.ParameterDirection The direction of the parameter (Input/Output)</param>
        public void addParameter(string parameterName, object value, GenericDataType genType, int size, ParameterDirection direction)
        {
            MySqlDbType targetMySqlDataType = default(MySqlDbType);
            Parameter buildParameter = null;
            _parameterList = new ArrayList();
            targetMySqlDataType = convertToMySqlDbType(genType);
            buildParameter = new Parameter(parameterName, value, targetMySqlDataType, size, direction);
            _parameterList.Add(buildParameter);
        }
        #endregion addParameter Public Methods

        #region Parameter Conversion
        private MySqlParameter convertParameters(Parameter passedParameter)
        {
            MySqlParameter convertedSqlParam = new MySqlParameter();
            convertedSqlParam.ParameterName = passedParameter.ParameterName;
            convertedSqlParam.Value = passedParameter.ParameterValue;
            convertedSqlParam.MySqlDbType = passedParameter.ParameterDataType;
            convertedSqlParam.Size = passedParameter.ParameterSize;
            convertedSqlParam.Direction = passedParameter.ParameterDirection;

            return convertedSqlParam;
        }

        private MySqlDbType convertToMySqlDbType(GenericDataType typeToConvert)
        {
            MySqlDbType convertedType;
            switch (typeToConvert)
            {
                case GenericDataType.genString:
                    convertedType = MySqlDbType.VarString;
                    break;
                case GenericDataType.genChar:
                    convertedType = MySqlDbType.VarChar;
                    break;
                case GenericDataType.genInteger:
                    convertedType = MySqlDbType.Int32;
                    break;
                case GenericDataType.genBit:
                    convertedType = MySqlDbType.Bit;
                    break;
                case GenericDataType.genDateTime:
                    convertedType = MySqlDbType.DateTime;
                    break;
                case GenericDataType.genDate:
                    convertedType = MySqlDbType.Date;
                    break;
                case GenericDataType.genTime:
                    convertedType = MySqlDbType.Time;
                    break;
                case GenericDataType.genDecimal:
                    convertedType = MySqlDbType.Decimal;
                    break;
                case GenericDataType.genMoney:
                    convertedType = MySqlDbType.Double;
                    break;
                case GenericDataType.genImage:
                    convertedType = MySqlDbType.LongBlob;
                    break;
                default:
                    convertedType = MySqlDbType.VarString;
                    break;
            }
            return convertedType;
        }
        #endregion Parameter Conversion

        #region Validation
        private void ValidateMySqlStatement(string strMySql)
        {
            if (strMySql.Length < 10)
            {
                throw new Exception(_exceptionMessage + " The MySQL statement must be provided and must be at least 10 characters long.");
            }
        }

        private void ValidateSPStatement(string SPName)
        {
            //Stored Proc name should be greater than 1 character
            if (SPName.Length < 1)
            {
                throw new Exception(_exceptionMessage + " The Name of the Stored Procedure should be at least 1 character long");
            }
        }
        #endregion Validation

        //#region Exception Logging
        //private void LogException(Exception ex)
        //{
        //    string eventLogMessage;

        //    try
        //    {
        //        /*
        //         * Create the message to be logged:
        //         *  .Source - gets the name of the application or object that causes the error
        //         *  .Message - gets a message that describes the current exception
        //         *  .StackTrace - gets a string representation of the frames on the call stack at the time the exception was thrown
        //         *  .TargetSite - gets the method that throws the current exception
        //         */
        //        eventLogMessage = "An error occurred in the following module: " + _moduleName + Environment.NewLine +
        //                         "The source (application/object) was: " + ex.Source + Environment.NewLine +
        //                         "The message was: " + ex.Message + Environment.NewLine +
        //                         "Stack trace: " + ex.StackTrace + Environment.NewLine +
        //                         "The target site (method) was: " + ex.TargetSite.ToString();

        //        //Define EventLog as an Application log entry (as opposed to Security log, System log, etc)
        //        EventLog myEventLog = new EventLog("Application");

        //        //Set the source of the EventLog
        //        myEventLog.Source = _moduleName;

        //        //Write the entry to Application log
        //        myEventLog.WriteEntry(eventLogMessage, EventLogEntryType.Error);
        //    }
        //    catch (Exception eventLogException)
        //    {

        //        throw new Exception("EventLog Error: " + eventLogException.Message, eventLogException);
        //    }
        //}
        //#endregion Exception Logging

        #region Overloaded dispose method

        protected override void Dispose(bool disposing)
        {

            //Check if Dispose has already been called, if not we can proceed
            //if (!_disposedBoolean)
            if (_disposedBoolean == false)
            {
                try
                {
                    //Dispose of and free up any resources used by the connection (if not already done)
                    _sqlConnection.Dispose();
                }
                catch (Exception ex)
                {
                    //Log the exception
                   // LogException(ex);
                }
                finally
                {
                    //Call the base class's (Component) Dispose method
                    base.Dispose(disposing);

                    //Suppress the Garbage Collector (GC) having to take 2 passes
                    GC.SuppressFinalize(this);

                    //Set the disposed boolean flag
                    //This flag lets us check between disposal and garbage collection so we don't resurrect the object falesly
                    _disposedBoolean = true;
                }
            }


        }

        #endregion
    }


}

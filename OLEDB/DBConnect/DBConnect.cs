using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace JunX.NETStandard.OLEDB
{
    /// <summary>
    /// Provides functionalities for an intuitive OLEDB Database manipulation and querying.
    /// </summary>
    /// <remarks>
    /// This class requires <c>System.Data.OleDb</c> NuGet Package.
    /// </remarks>
    public partial class DBConnect
    {
        private string _connSTR;
        private OleDbConnection _conn;
        private OleDbDataReader _reader;
        private OleDbDataAdapter _adapter;
        private OleDbCommand _cmd;
        private DataSet _ds;
        private string _cmdSTR;
        private List<string> _values;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBConnect"/> class using the specified connection string.
        /// </summary>
        /// <param name="ConnectionString">The database connection string used to establish connectivity with the target data source.</param>
        public DBConnect(string ConnectionString)
        {
            InitializeAll();
            _connSTR = ConnectionString;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DBConnect"/> class using the specified <see cref="OleDbConnection"/>.
        /// </summary>
        /// <param name="Connection">An existing <see cref="OleDbConnection"/> to be used for database operations.</param>
        public DBConnect(OleDbConnection Connection)
        {
            InitializeAll();
            _conn = Connection;
            _cmd.Connection = _conn;
            _cmd.CommandType = CommandType.Text;
        }

        private void InitializeAll()
        {
            _connSTR = string.Empty;
            _conn = new OleDbConnection();
            _adapter = new OleDbDataAdapter();
            _cmd = new OleDbCommand();
            _ds = new DataSet();
            _cmdSTR = string.Empty;
            _values = new List<string>();
        }
    }
}

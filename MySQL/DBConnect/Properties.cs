using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JunX.NETStandard.MySQL
{
    public partial class DBConnect
    {
        /// <summary>
        /// Gets the internal <see cref="MySqlConnection"/> instance used for database operations.
        /// </summary>
        /// <value>
        /// The current <see cref="MySqlConnection"/> object managed by the <c>InternalVariables</c> container.
        /// </value>
        /// <remarks>
        /// This property exposes the underlying connection object for advanced scenarios such as transaction management or manual command execution.
        /// </remarks>
        public MySqlConnection Connection { get { return InternalVariables.Connection; } }
        /// <summary>
        /// Gets the current state of the internal <see cref="MySqlConnection"/> object.
        /// </summary>
        /// <value>
        /// A <see cref="ConnectionState"/> value indicating whether the connection is open, closed, connecting, executing, or broken.
        /// </value>
        /// <remarks>
        /// This property reflects the runtime status of the internal connection and is useful for validating connection readiness before executing commands.
        /// </remarks>
        public ConnectionState State { get { return InternalVariables.Connection.State; } }
        /// <summary>
        /// Gets the internal <see cref="MySqlDataReader"/> containing the result set from the most recent <c>SELECT</c> execution.
        /// </summary>
        /// <value>
        /// A <see cref="MySqlDataReader"/> instance used to read rows returned by the executed SQL query.
        /// </value>
        /// <remarks>
        /// This property provides direct access to the raw data reader stored in <c>InternalVariables.Reader</c>.
        /// It is typically used for manual row and field traversal after invoking <c>ExecuteReader()</c>.
        /// </remarks>
        public MySqlDataReader Reader { get { return InternalVariables.Reader; } }
        /// <summary>
        /// Gets the internal <see cref="MySqlDataAdapter"/> used for data operations such as filling datasets or updating tables.
        /// </summary>
        /// <value>
        /// A <see cref="MySqlDataAdapter"/> instance configured for use with the current database connection and command context.
        /// </value>
        /// <remarks>
        /// This property provides access to the adapter stored in <c>InternalVariables.Adapter</c>.
        /// It is typically used for disconnected data access scenarios, including populating <see cref="DataSet"/> or synchronizing changes with the database.
        /// </remarks>
        public MySqlDataAdapter Adapter { get { return InternalVariables.Adapter; } }
        /// <summary>
        /// Gets the internal <see cref="DataSet"/> used for storing tabular data retrieved from the database.
        /// </summary>
        /// <value>
        /// A <see cref="DataSet"/> instance that holds one or more <see cref="DataTable"/> objects populated via data adapters or manual assignment.
        /// </value>
        /// <remarks>
        /// This property provides access to <c>InternalVariables.Dataset</c>, typically used for disconnected data operations,
        /// such as binding to UI controls, performing in-memory queries, or exporting structured results.
        /// </remarks>
        public DataSet DataSet { get { return InternalVariables.Dataset; } }
        /// <summary>
        /// Gets a value indicating whether the current <see cref="MySqlDataReader"/> contains one or more rows.
        /// </summary>
        /// <value>
        /// <c>true</c> if the reader has at least one row; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property reflects the <c>HasRows</c> state of <c>InternalVariables.Reader</c>.
        /// It is typically checked after executing a <c>SELECT</c> command to determine if any results were returned.
        /// </remarks>
        public bool HasRows { get { return InternalVariables.Reader.HasRows; } }
        /// <summary>
        /// Gets the internal list of stringified values retrieved from the most recent SQL <c>SELECT</c> execution.
        /// </summary>
        /// <value>
        /// A <see cref="List{String}"/> containing the field values from all rows returned by the executed query.
        /// </value>
        /// <remarks>
        /// This property reflects the contents of <c>InternalVariables.Values</c>, which is populated during <c>ExecuteReader()</c>.
        /// Each entry corresponds to a field value, converted to string, from the result set.
        /// Useful for lightweight result inspection or quick value extraction without schema mapping.
        /// </remarks>
        public List<string> Values 
        { 
            get 
            {
                CloseReader();
                return InternalVariables.Values; 
            } 
        }

        /// <summary>
        /// Gets or sets the internal MySQL connection string used for database operations.
        /// </summary>
        /// <value>
        /// A string representing the connection details required to establish a MySQL database connection.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying connection string stored in <see cref="InternalVariables"/>.
        /// Updating this value affects how the internal connection is configured during initialization or opening.
        /// </remarks>
        public string ConnectionString { get { return InternalVariables.ConnectionString; } set { InternalVariables.ConnectionString = value; } }
        /// <summary>
        /// Gets or sets the SQL command text used for database operations.
        /// </summary>
        /// <value>
        /// A string containing the SQL query or command to be executed by the internal <see cref="MySqlCommand"/> object.
        /// </value>
        /// <remarks>
        /// This property provides access to the raw SQL statement stored in <c>InternalVariables.CommandText</c>.
        /// It is typically assigned before executing queries or commands against the database.
        /// </remarks>
        public string CommandText { get { return InternalVariables.CommandText; } set { InternalVariables.CommandText = value; } }
        /// <summary>
        /// Gets or sets the current connection string metadata used for MySQL access.
        /// </summary>
        public ConnectionStringMetadata ConnectionStringInformation { get { return InternalVariables.ConnectionStringInformation; } set { InternalVariables.ConnectionStringInformation = value; } }
    }
}

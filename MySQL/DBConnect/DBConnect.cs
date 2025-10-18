using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace JunX.NETStandard.MySQL
{
    /// <summary>
    /// Contains methods and properties that provides additional functionalities for an easier and more intuitive MySQL Database manipulation and querying.
    /// </summary>
    public partial class DBConnect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DBConnect"/> class and sets the connection string for internal database operations.
        /// </summary>
        /// <param name="ConnString">
        /// The connection string used to configure the internal MySQL connection.
        /// </param>
        /// <remarks>
        /// This constructor resets all internal database-related variables and assigns the provided connection string for subsequent use.
        /// </remarks>
        public DBConnect(string ConnString)
        {
            InternalVariables.InitializeAll();
            InternalVariables.ConnectionString = ConnString;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DBConnect"/> class using the specified <see cref="MySqlConnection"/>.
        /// </summary>
        /// <param name="Connection">
        /// An existing <see cref="MySqlConnection"/> instance to be used for command execution and data operations.
        /// </param>
        /// <remarks>
        /// This constructor initializes all internal variables, assigns the provided connection,
        /// and configures the internal <see cref="MySqlCommand"/> to use text-based SQL commands.
        /// </remarks>
        public DBConnect(MySqlConnection Connection)
        {
            InternalVariables.InitializeAll();
            InternalVariables.Connection = Connection;
            InternalVariables.Command.Connection = InternalVariables.Connection;
            InternalVariables.Command.CommandType = CommandType.Text;
        }
    }
}
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
        /// Validates the internal connection and command text before executing a SQL <c>SELECT</c> operation.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown when the connection is not open, the command text is missing, or the command is not a <c>SELECT</c> statement.
        /// </exception>
        /// <remarks>
        /// This method performs three validation checks:
        /// <list type="bullet">
        /// <item><description>Ensures the internal <see cref="MySqlConnection"/> is open.</description></item>
        /// <item><description>Verifies that <see cref="CommandText"/> is not null, empty, or whitespace.</description></item>
        /// <item><description>Confirms that the command text contains a valid SQL <c>SELECT</c> keyword.</description></item>
        /// </list>
        /// Intended to safeguard query execution logic by enforcing preconditions.
        /// </remarks>
        internal void ValidateForExecution()
        {
            if (State != ConnectionState.Open)
                throw new Exception("Current MySQLConnection is not open.");

            if (string.IsNullOrWhiteSpace(CommandText))
                throw new Exception("No command text to execute.");

            if (!IsSQLSelect())
                throw new Exception("Current command text is not an SQL SELECT command.");
        }
        /// <summary>
        /// Determines whether the current SQL command text represents a <c>SELECT</c> statement.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the command text contains the keyword <c>SELECT</c>; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method performs a case-insensitive search for the keyword <c>SELECT</c> within the <see cref="CommandText"/> property.
        /// Useful for distinguishing query operations from non-query commands such as <c>INSERT</c>, <c>UPDATE</c>, or <c>DELETE</c>.
        /// </remarks>
        internal bool IsSQLSelect()
        {
            if (CommandText.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase) == -1)
                return false;
            return true;
        }
    }
}

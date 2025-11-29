using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NETStandard.MySQL
{
    public partial class DBConnect
    {
        /// <summary>
        /// Opens the internal MySQL connection and configures the command object for text-based execution.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown when the connection attempt fails due to an invalid connection string or database access error.
        /// </exception>
        /// <remarks>
        /// This method checks whether the internal <see cref="MySqlConnection"/> is already open.
        /// If not, it assigns the connection string, opens the connection, and sets up the <see cref="MySqlCommand"/> with the active connection and <c>CommandType.Text</c>.
        /// This ensures that the command object is ready for executing SQL statements immediately after the connection is established.
        /// </remarks>
        public void Open(out bool IsOpened)
        {
            if (InternalVariables.Connection.State != ConnectionState.Open)
            {
                InternalVariables.Connection.ConnectionString = InternalVariables.ConnectionString;
                try
                {
                    InternalVariables.Connection.Open();
                    InternalVariables.Command.Connection = InternalVariables.Connection;
                    InternalVariables.Command.CommandType = CommandType.Text;
                    IsOpened = true;
                    ConnectionOpened?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    IsOpened = false;
                    throw new Exception("An error occured while trying to open the connection.\n\n" + ex.Message.ToString());
                }
            }
            else
                IsOpened = true;
        }
        /// <summary>
        /// Closes the internal MySQL connection if it is currently open.
        /// </summary>
        /// <remarks>
        /// This method invokes <see cref="MySqlConnection.Close"/> on the internally managed connection object.
        /// It is recommended to call this after completing database operations to release resources and avoid connection leaks.
        /// </remarks>
        public void CloseConnection()
        {
            InternalVariables.Connection.Close();
            ConnectionClosed?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Closes the internal <see cref="MySqlDataReader"/> if it is currently open.
        /// </summary>
        /// <remarks>
        /// This method checks the <see cref="MySqlDataReader.IsClosed"/> property and closes the reader only if it is still active.
        /// Useful for releasing data stream resources after query execution to prevent locking or memory leaks.
        /// </remarks>
        public void CloseReader()
        {
            if (!InternalVariables.Reader.IsClosed)
            {
                InternalVariables.Reader.Close();
                ReaderClosed?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Asynchronously disposes internal database-related resources and resets associated metadata.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous disposal operation.
        /// </returns>
        /// <remarks>
        /// This method performs a thorough cleanup of internal objects used during database operations:
        /// <list type="bullet">
        /// <item><description>Disposes <see cref="MySqlCommand"/>, <see cref="MySqlDataAdapter"/>, and <see cref="DataSet"/> instances.</description></item>
        /// <item><description>Closes and disposes the <see cref="MySqlConnection"/> if it is still open.</description></item>
        /// <item><description>Asynchronously disposes the <see cref="MySqlDataReader"/> if it is active.</description></item>
        /// <item><description>Resets internal metadata fields such as connection string, command text, and value list to <c>null</c>.</description></item>
        /// </list>
        /// Recommended for use in asynchronous workflows to ensure proper resource release and prevent memory or connection leaks.
        /// </remarks>
        public async Task DisposeAsync()
        {
            InternalVariables.Command.Dispose();
            InternalVariables.Adapter.Dispose();
            InternalVariables.Dataset.Dispose();

            if (InternalVariables.Connection.State != ConnectionState.Closed && InternalVariables.Connection != null)
            {
                InternalVariables.Connection.Close();
                InternalVariables.Connection.Dispose();
            }

            if (InternalVariables.Reader != null & !InternalVariables.Reader.IsClosed)
                await InternalVariables.Reader.DisposeAsync();

            InternalVariables.ConnectionString = null;
            InternalVariables.CommandText = null;
            InternalVariables.Values = null;

            Disposed?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Resets the internal MySQL connection by closing it if open and reopening it.
        /// </summary>
        /// <remarks>
        /// This method ensures the connection is in a clean state by explicitly closing it if necessary,
        /// then invoking <see cref="Open"/> to reinitialize the connection using the current connection string.
        /// Useful for recovering from transient connection issues or refreshing stale connections.
        /// </remarks>
        public void ResetConnection()
        {
            if (InternalVariables.Connection.State != ConnectionState.Closed)
                InternalVariables.Connection.Close();

            Open(out _);
        }
    }
}

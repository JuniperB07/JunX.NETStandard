using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace JunX.NETStandard.OLEDB
{
    partial class DBConnect
    {
        /// <summary>
        /// Attempts to open the database connection and indicates whether the operation was successful.
        /// </summary>
        /// <param name="IsOpened">
        /// When this method returns, contains <c>true</c> if the connection was successfully opened; otherwise, <c>false</c>.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown when an error occurs while attempting to open the database connection.
        /// </exception>
        public void Open(out bool IsOpened)
        {
            IsOpened = false;

            if(_conn.State == ConnectionState.Open)
            {
                IsOpened = true;
                return;
            }

            _conn.ConnectionString = _connSTR;
            try
            {
                _conn.Open();
                _cmd.Connection = _conn;
                _cmd.CommandType = CommandType.Text;
                IsOpened = true;
            }
            catch(Exception e)
            {
                throw new Exception("An error occurred while trying to connect to database.\n\n" + e.Message.ToString());
            }
        }
        /// <summary>
        /// Closes the active database connection.
        /// </summary>
        public void CloseConnection() => _conn.Close();
        /// <summary>
        /// Closes the active data reader if it is not already closed.
        /// </summary>
        public void CloseReader()
        {
            if(_reader != null)
            {
                if (!_reader.IsClosed)
                    _reader.Close();
            }
        }
        /// <summary>
        /// Asynchronously disposes all database-related resources and resets internal state.
        /// </summary>
        /// <returns>A task that represents the asynchronous dispose operation.</returns>
        /// <exception cref="Exception">
        /// May propagate exceptions thrown during connection closure or resource disposal.
        /// </exception>
        public async Task DisposeAllAsync()
        {
            _cmd.Dispose();
            _adapter.Dispose();
            _ds.Dispose();

            if(_conn != null)
            {
                if( _conn.State != ConnectionState.Closed)
                {
                    _conn.Close();
                    _conn.Dispose();
                }
            }

            if(_reader != null && !_reader.IsClosed)
            {
                _reader.Close();
                _reader = null;
            }

            _connSTR = null;
            _cmdSTR = null;
            _values = null;
        }
        /// <summary>
        /// Closes the current database connection if open, then reopens it using the stored connection string.
        /// </summary>
        public void Reopen()
        {
            if (_conn.State != ConnectionState.Closed)
                _conn.Close();

            Open(out _);
        }
    }
}

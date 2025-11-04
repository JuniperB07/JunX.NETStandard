using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.MySQL
{
    partial class DBConnect
    {
        /// <summary>
        /// Occurs when the connection string value has been changed.
        /// </summary>
        /// <remarks>
        /// This event provides both the old and new connection string values via the <see cref="ConnectionStringChangedEventArgs{T}"/> payload.
        /// It is typically used to trigger configuration updates, logging, or validation logic in response to connection changes.
        /// </remarks>
        public event EventHandler<ConnectionStringChangedEventArgs<string>> ConnectionStringChanged;
        /// <summary>
        /// Occurs when the SQL command text has been modified.
        /// </summary>
        /// <remarks>
        /// This event provides both the previous and updated command text values via the <see cref="CommandTextChangedEventArgs{T}"/> payload.
        /// It is typically used to trigger logging, validation, or dynamic query adjustments in response to command text changes.
        /// </remarks>
        public event EventHandler<CommandTextChangedEventArgs<string>> CommandTextChanged;

        /// <summary>
        /// Occurs when a connection to the data source has been successfully opened.
        /// </summary>
        /// <remarks>
        /// This event is typically used to trigger initialization routines, logging, or diagnostics after a connection becomes active.
        /// It does not carry additional event data and serves as a simple notification hook.
        /// </remarks>
        public event EventHandler ConnectionOpened;
        /// <summary>
        /// Occurs when the connection to the data source has been closed.
        /// </summary>
        /// <remarks>
        /// This event is typically used to trigger cleanup routines, logging, or diagnostics after a connection is terminated.
        /// It does not carry additional event data and serves as a simple notification hook.
        /// </remarks>
        public event EventHandler ConnectionClosed;
        /// <summary>
        /// Occurs when a data reader has been closed after completing its operation.
        /// </summary>
        /// <remarks>
        /// This event is typically used to trigger cleanup routines, release resources, or log the end of a data reading session.
        /// It does not carry additional event data and serves as a simple notification hook.
        /// </remarks>
        public event EventHandler ReaderClosed;
    }
}

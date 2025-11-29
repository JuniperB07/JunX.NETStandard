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
        /// <summary>
        /// Occurs after values have been read from the data source.
        /// </summary>
        /// <remarks>
        /// This event provides subscribers with a notification that data values 
        /// have been successfully retrieved. It can be used to trigger custom 
        /// logic such as logging, validation, or post‑processing once the read 
        /// operation completes.
        /// </remarks>
        public event EventHandler ValuesRead;
        /// <summary>
        /// Occurs when the object has been disposed.
        /// </summary>
        /// <remarks>
        /// This event notifies subscribers that the object’s resources have been released 
        /// and it is no longer usable. It can be used to trigger cleanup logic, 
        /// unsubscribe handlers, or perform other finalization tasks when disposal occurs.
        /// </remarks>
        public event EventHandler Disposed;
        /// <summary>
        /// Occurs when the object has been initialized.
        /// </summary>
        /// <remarks>
        /// This event notifies subscribers that the object has completed its initialization 
        /// process and is ready for use. It can be used to trigger setup logic, 
        /// resource allocation, or other operations that depend on the object being fully initialized.
        /// </remarks>
        public event EventHandler Initialized;

        /// <summary>
        /// Occurs after a database reader command has been executed.
        /// </summary>
        /// <remarks>
        /// This event uses <see cref="ReaderExecutedEventArgs"/> to provide subscribers 
        /// with details about the executed SQL command and whether the reader returned rows. 
        /// It is typically raised by the query execution logic to allow logging, monitoring, 
        /// or custom handling of results.
        /// </remarks>
        public event EventHandler<ReaderExecutedEventArgs> ReaderExecuted;
        /// <summary>
        /// Occurs after a database adapter command has been executed.
        /// </summary>
        /// <remarks>
        /// This event uses <see cref="AdapterExecutedEventArgs"/> to provide subscribers 
        /// with details about the executed SQL command. It is typically raised by the 
        /// adapter execution logic to allow logging, monitoring, or custom handling of 
        /// executed commands.
        /// </remarks>
        public event EventHandler<AdapterExecutedEventArgs> AdapterExecuted;
        /// <summary>
        /// Occurs after a database command has been executed that returns a <see cref="DataSet"/>.
        /// </summary>
        /// <remarks>
        /// This event uses <see cref="DataSetExecutedEventArgs"/> to provide subscribers 
        /// with both the executed SQL command text and the resulting <see cref="DataSet"/>. 
        /// It is typically raised by the query execution logic to enable logging, monitoring, 
        /// or custom handling of the returned dataset.
        /// </remarks>
        public event EventHandler<DataSetExecutedEventArgs> DataSetExecuted;
        /// <summary>
        /// Occurs after a non‑query database command has been executed.
        /// </summary>
        /// <remarks>
        /// This event uses <see cref="NonQueryExecutedEventArgs"/> to provide subscribers 
        /// with details about the executed SQL command. It is typically raised by the 
        /// execution logic to allow logging, monitoring, or custom handling of non‑query 
        /// operations such as INSERT, UPDATE, or DELETE.
        /// </remarks>
        public event EventHandler<NonQueryExecutedEventArgs> NonQueryExecuted;
        /// <summary>
        /// Occurs when the database connection has been reset.
        /// </summary>
        /// <remarks>
        /// This event uses <see cref="ConnectionResetEventArgs"/> to provide subscribers 
        /// with information about the connection status following a reset operation. 
        /// It can be used to trigger reconnection logic, logging, or other custom handling 
        /// whenever the connection state changes due to a reset.
        /// </remarks>
        public event EventHandler<ConnectionResetEventArgs> ConnectionReset;
    }
}

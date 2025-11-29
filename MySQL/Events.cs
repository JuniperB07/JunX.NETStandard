using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.MySQL
{
    /// <summary>
    /// Represents event data for a connection string change, capturing both the old and new values.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the connection string value, typically <see cref="string"/> or a structured connection descriptor.
    /// </typeparam>
    /// <remarks>
    /// This class is used in event-driven systems to notify subscribers when a connection string has been updated.
    /// It provides access to both the previous and updated values, enabling logging, validation, or rollback logic.
    /// </remarks>
    public class ConnectionStringChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the previous value before the connection string was changed.
        /// </summary>
        /// <value>
        /// A value of type <typeparamref name="T"/> representing the original connection string prior to the update.
        /// </value>
        /// <remarks>
        /// This property is useful for auditing, rollback logic, or notifying subscribers of what the connection string used to be.
        /// </remarks>
        public T OldValue { get; }
        /// <summary>
        /// Gets the updated value after the connection string has changed.
        /// </summary>
        /// <value>
        /// A value of type <typeparamref name="T"/> representing the new connection string following the update.
        /// </value>
        /// <remarks>
        /// This property is useful for tracking configuration changes, validating new connection targets, or notifying subscribers of the updated value.
        /// </remarks>
        public T NewValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringChangedEventArgs{T}"/> class with the specified old and new connection string values.
        /// </summary>
        /// <param name="SetOldValue">
        /// The previous connection string value before the change occurred.
        /// </param>
        /// <param name="SetNewValue">
        /// The updated connection string value after the change.
        /// </param>
        /// <remarks>
        /// This constructor captures both the original and updated values, enabling event subscribers to respond to configuration changes with full context.
        /// </remarks>
        public ConnectionStringChangedEventArgs(T SetOldValue, T SetNewValue)
        {
            OldValue = SetOldValue;
            NewValue = SetNewValue;
        }
    }

    /// <summary>
    /// Represents event data for a change in SQL command text, capturing both the previous and updated values.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the command text value, typically <see cref="string"/> or a structured command descriptor.
    /// </typeparam>
    /// <remarks>
    /// This class is used in event-driven systems to notify subscribers when the SQL command text has been modified.
    /// It provides access to both the original and new values, enabling logging, auditing, or dynamic query adjustments.
    /// </remarks>
    public class CommandTextChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the previous value of the SQL command text before the change occurred.
        /// </summary>
        /// <value>
        /// A value of type <typeparamref name="T"/> representing the original command text.
        /// </value>
        /// <remarks>
        /// This property is useful for auditing, debugging, or tracking changes in dynamically generated SQL statements.
        /// </remarks>
        public T OldValue { get; }
        /// <summary>
        /// Gets the updated value of the SQL command text after the change occurred.
        /// </summary>
        /// <value>
        /// A value of type <typeparamref name="T"/> representing the new command text.
        /// </value>
        /// <remarks>
        /// This property is useful for tracking modifications, validating updated queries, or responding to dynamic SQL changes in event-driven systems.
        /// </remarks>
        public T NewValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandTextChangedEventArgs{T}"/> class with the specified old and new command text values.
        /// </summary>
        /// <param name="SetOldValue">
        /// The previous command text value before the change occurred.
        /// </param>
        /// <param name="SetNewValue">
        /// The updated command text value after the change.
        /// </param>
        /// <remarks>
        /// This constructor captures both the original and updated SQL command text, enabling event subscribers to respond to query modifications with full context.
        /// </remarks>
        public CommandTextChangedEventArgs(T SetOldValue, T SetNewValue)
        {
            OldValue = SetOldValue;
            NewValue = SetNewValue;
        }
    }

    /// <summary>
    /// Provides data for events raised after a database reader command has been executed.
    /// </summary>
    /// <remarks>
    /// This event argument class encapsulates both the SQL command text that was executed 
    /// and a flag indicating whether the resulting reader contained any rows. 
    /// It is typically used in event handlers to inspect or log the outcome of query execution.
    /// </remarks>
    public class ReaderExecutedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the SQL command text that was executed.
        /// </summary>
        /// <remarks>
        /// This read‑only property is initialized through the constructor of 
        /// <see cref="ReaderExecutedEventArgs"/> and provides event subscribers 
        /// with the exact SQL command string for inspection or logging.
        /// </remarks>
        public string Command { get; }
        /// <summary>
        /// Gets a value indicating whether the executed reader returned any rows.
        /// </summary>
        /// <value>
        /// <c>true</c> if the query produced one or more rows; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This read‑only property is initialized through the constructor of 
        /// <see cref="ReaderExecutedEventArgs"/> and allows event subscribers 
        /// to determine whether the executed SQL command yielded results.
        /// </remarks>
        public bool HadRows { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderExecutedEventArgs"/> class 
        /// with the specified SQL command text and a flag indicating whether the reader 
        /// returned any rows.
        /// </summary>
        /// <param name="SetCommand">
        /// The SQL command text that was executed.
        /// </param>
        /// <param name="SetHadRows">
        /// A boolean value indicating whether the executed reader contained one or more rows.
        /// </param>
        public ReaderExecutedEventArgs(string SetCommand, bool SetHadRows)
        {
            Command = SetCommand;
            HadRows = SetHadRows;
        }
    }

    /// <summary>
    /// Provides data for events raised after a database adapter command has been executed.
    /// </summary>
    /// <remarks>
    /// This event argument class encapsulates the SQL command text that was executed, 
    /// allowing event subscribers to inspect or log the command associated with the adapter.
    /// </remarks>
    public class AdapterExecutedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the SQL command text that was executed.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the SQL command associated with the event.
        /// </value>
        /// <remarks>
        /// This property is read‑only and is initialized through the constructor of 
        /// <see cref="AdapterExecutedEventArgs"/>. It allows event subscribers to 
        /// inspect or log the executed command.
        /// </remarks>
        public string Command { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdapterExecutedEventArgs"/> class 
        /// with the specified SQL command text.
        /// </summary>
        /// <param name="SetCommand">
        /// The SQL command text that was executed.
        /// </param>
        public AdapterExecutedEventArgs(string SetCommand)
        {
            Command = SetCommand;
        }
    }

    /// <summary>
    /// Provides data for events raised after a database command has been executed 
    /// that returns a <see cref="DataSet"/>.
    /// </summary>
    /// <remarks>
    /// This event argument class encapsulates both the SQL command text that was executed 
    /// and the resulting <see cref="DataSet"/>. It is typically used in event handlers 
    /// to inspect, log, or further process the query results.
    /// </remarks>
    public class DataSetExecutedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="DataSet"/> returned by the executed database command.
        /// </summary>
        /// <value>
        /// A <see cref="DataSet"/> instance containing the results of the executed query.
        /// </value>
        /// <remarks>
        /// This read‑only property is initialized through the constructor of 
        /// <see cref="DataSetExecutedEventArgs"/> and allows event subscribers 
        /// to access the full set of query results for inspection, logging, or further processing.
        /// </remarks>
        public DataSet Dataset { get; }
        /// <summary>
        /// Gets the SQL command text that was executed.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the SQL command associated with the event.
        /// </value>
        /// <remarks>
        /// This read‑only property is initialized through the constructor of 
        /// <see cref="DataSetExecutedEventArgs"/>. It allows event subscribers 
        /// to inspect or log the executed command alongside the returned dataset.
        /// </remarks>
        public string Command { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetExecutedEventArgs"/> class 
        /// with the specified SQL command text and the resulting <see cref="DataSet"/>.
        /// </summary>
        /// <param name="SetCommand">
        /// The SQL command text that was executed.
        /// </param>
        /// <param name="SetDataset">
        /// The <see cref="DataSet"/> returned by the executed command.
        /// </param>
        public DataSetExecutedEventArgs(string SetCommand, DataSet SetDataset)
        {
            Command = SetCommand;
            Dataset = SetDataset;
        }
    }

    /// <summary>
    /// Provides data for events raised after a non‑query database command has been executed.
    /// </summary>
    /// <remarks>
    /// This event argument class encapsulates the SQL command text that was executed. 
    /// It is typically used in event handlers to inspect or log commands such as 
    /// INSERT, UPDATE, or DELETE operations that do not return result sets.
    /// </remarks>
    public class NonQueryExecutedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the SQL command text that was executed.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the SQL command associated with the event.
        /// </value>
        /// <remarks>
        /// This read‑only property is initialized through the constructor of 
        /// <see cref="NonQueryExecutedEventArgs"/>. It allows event subscribers 
        /// to inspect or log the executed non‑query command, such as INSERT, UPDATE, or DELETE.
        /// </remarks>
        public string Command { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonQueryExecutedEventArgs"/> class 
        /// with the specified SQL command text.
        /// </summary>
        /// <param name="SetCommand">
        /// The SQL command text that was executed, typically representing a non‑query 
        /// operation such as INSERT, UPDATE, or DELETE.
        /// </param>
        public NonQueryExecutedEventArgs(string SetCommand)
        {
            Command = SetCommand;
        }
    }

    /// <summary>
    /// Provides data for events raised when a database connection has been reset.
    /// </summary>
    /// <remarks>
    /// This event argument class encapsulates the connection status after a reset operation. 
    /// It allows event subscribers to determine whether the connection is currently active 
    /// or has been closed following the reset.
    /// </remarks>
    public class ConnectionResetEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a value indicating the status of the database connection after a reset.
        /// </summary>
        /// <value>
        /// <c>true</c> if the connection is active following the reset; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This read‑only property is initialized through the constructor of 
        /// <see cref="ConnectionResetEventArgs"/>. It allows event subscribers 
        /// to determine whether the connection remains open or has been closed 
        /// after the reset operation.
        /// </remarks>
        public bool ConnectionStatus { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionResetEventArgs"/> class 
        /// with the specified connection status.
        /// </summary>
        /// <param name="SetConnectionStatus">
        /// A Boolean value indicating the status of the database connection after the reset. 
        /// Pass <c>true</c> if the connection is active, or <c>false</c> if it is closed.
        /// </param>
        public ConnectionResetEventArgs(bool SetConnectionStatus)
        {
            ConnectionStatus = SetConnectionStatus;
        }
    }
}

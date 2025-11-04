using System;
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
}

using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Provides a fluent, strongly typed builder for composing SQL <c>UPDATE</c> statements using enum-defined column metadata.
    /// </summary>
    /// <typeparam name="T">
    /// An enum type representing the target table's column schema. Each enum member corresponds to a column name.
    /// </typeparam>
    /// <remarks>
    /// This class supports dynamic SQL generation for update operations, including column-value assignments and conditional filtering.
    /// It tracks internal state to manage clause sequencing, grouping, and connector placement.
    /// Designed for scenarios where schema is represented via enums, enabling type-safe and metadata-driven query composition.
    /// </remarks>
    public class UpdateCommand<T> :
        IUpdateable<UpdateCommand<T>, UpdateMetadata<T>>,
        IConditionable<WhereClause<UpdateCommand<T>, T>>
        where T: Enum
    {
        StringBuilder cmd;
        bool _hasSets;
        bool _hasWhere;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommand{T}"/> class for building a SQL <c>UPDATE</c> statement.
        /// </summary>
        /// <remarks>
        /// This constructor sets up the internal command buffer and begins the statement with <c>UPDATE</c> followed by the name of the enum type <typeparamref name="T"/>.
        /// It also resets the internal flag used to track whether any <c>SET</c> clauses have been appended.
        /// </remarks>
        public UpdateCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("UPDATE " + typeof(T).Name);
            _hasSets = false;
            _hasWhere = false;
        }
        /// <summary>
        /// Returns the composed SQL <c>UPDATE</c> statement as a string, terminated with a semicolon.
        /// </summary>
        /// <returns>
        /// A complete SQL query string representing the current state of the update builder.
        /// </returns>
        /// <remarks>
        /// This override finalizes the command buffer for execution or inspection. It assumes that the <c>SET</c> clauses have been properly appended.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ";";
        }

        #region Properties
        /// <summary>
        /// Begins composition of a SQL <c>WHERE</c> clause for an <c>UPDATE</c> command involving a single primary table.
        /// </summary>
        /// <returns>A new <see cref="WhereClause&lt;UpdateCommand&lt;T&gt;, T&gt;"/> instance bound to this command.</returns>
        public WhereClause<UpdateCommand<T>, T> StartWhere => new WhereClause<UpdateCommand<T>, T>(this, cmd);
        /*
        /// <summary>
        /// Appends a <c>WHERE</c> clause to the SQL <c>UPDATE</c> statement and returns the current builder for condition chaining.
        /// </summary>
        /// <value>
        /// The current <see cref="UpdateCommand{T}"/> instance, allowing fluent chaining of conditional expressions.
        /// </value>
        /// <remarks>
        /// This property inserts the <c>WHERE</c> keyword into the query. It assumes that subsequent calls will append valid conditions.
        /// Use in combination with condition methods to filter which rows are affected by the update.
        /// </remarks>
        public UpdateCommand<T> Where
        {
            get
            {
                cmd.Append(" WHERE ");
                return this;
            }
        }
        /// <summary>
        /// Begins a grouped <c>WHERE</c> clause by appending an opening parenthesis to the SQL <c>UPDATE</c> statement.
        /// </summary>
        /// <value>
        /// The current <see cref="UpdateCommand{T}"/> instance, allowing fluent chaining of grouped conditional expressions.
        /// </value>
        /// <remarks>
        /// This property inserts an opening parenthesis for logical grouping. If no <c>WHERE</c> clause has been started, it assumes one is already in progress.
        /// Use in combination with <see cref="EndGroupedWhere"/> to wrap complex conditions within parentheses.
        /// </remarks>
        public UpdateCommand<T> StartGroupedWhere
        {
            get
            {
                cmd.Append(" (");
                return this;
            }
        }
        /// <summary>
        /// Ends a grouped <c>WHERE</c> clause by appending a closing parenthesis to the SQL <c>UPDATE</c> statement.
        /// </summary>
        /// <value>
        /// The current <see cref="UpdateCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </value>
        /// <remarks>
        /// This property completes a logical grouping started with <see cref="StartGroupedWhere"/>, enabling nested or compound conditions in the <c>WHERE</c> clause.
        /// </remarks>
        public UpdateCommand<T> EndGroupedWhere
        {
            get
            {
                cmd.Append(")");
                return this;
            }
        }
        */
        #endregion

        #region Set
        /// <summary>
        /// Appends a column-value assignment to the SQL <c>SET</c> clause of the <c>UPDATE</c> statement using typed metadata.
        /// </summary>
        /// <param name="UpdateData">
        /// An instance of <see cref="UpdateMetadata{T}"/> containing the target column, raw value, and associated data type for SQL-safe formatting.
        /// </param>
        /// <returns>
        /// The current <see cref="UpdateCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the <c>SET</c> clause if it hasn't already started, and appends the column assignment in the form <c>Column = Value</c>.
        /// Each value is formatted safely using the metadata's <c>Value</c> property.
        /// </remarks>
        public UpdateCommand<T> Set(UpdateMetadata<T> UpdateData)
        {
            if (_hasSets)
                cmd.Append(", ");
            else
            {
                cmd.Append(" SET ");
                _hasSets = true;
            }
            cmd.Append(UpdateData.Column.ToString() + "=" + UpdateData.Value);
            return this;
        }
        /// <summary>
        /// Appends multiple column-value assignments to the SQL <c>SET</c> clause of the <c>UPDATE</c> statement using typed metadata.
        /// </summary>
        /// <param name="UpdateData">
        /// A sequence of <see cref="UpdateMetadata{T}"/> instances, each containing a column, raw value, and associated data type for SQL-safe formatting.
        /// </param>
        /// <returns>
        /// The current <see cref="UpdateCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the <c>SET</c> clause if it hasn't already started, and appends each assignment in the form <c>Column = Value</c>, separated by commas.
        /// Each value is formatted safely using the metadata's <c>Value</c> property.
        /// </remarks>
        public UpdateCommand<T> Set(IEnumerable<UpdateMetadata<T>> UpdateData)
        {
            foreach (UpdateMetadata<T> UD in UpdateData)
            {
                if (_hasSets)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(" SET ");
                    _hasSets = true;
                }
                cmd.Append(UD.Column.ToString() + "=" + UD.Value);
            }
            return this;
        }
        /// <summary>
        /// Appends one or more column-value pairs to the SET clause of the SQL UPDATE command.
        /// </summary>
        /// <param name="UpdateData">
        /// A variable-length array of <see cref="UpdateMetadata{T}"/> objects, each containing a column and its corresponding value.
        /// </param>
        /// <returns>
        /// The current <see cref="UpdateCommand{T}"/> instance for fluent chaining.
        /// </returns>
        /// <remarks>
        /// Builds a comma-separated SET clause using the format: <c>Column=Value</c>.
        /// </remarks>
        public UpdateCommand<T> Set(params UpdateMetadata<T>[] UpdateData)
        {
            if (UpdateData.Length < 1)
                throw new ArgumentException("Invalid parameter length.");

            foreach (UpdateMetadata<T> UD in UpdateData)
            {
                if (_hasSets)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(" SET ");
                    _hasSets = true;
                }
                cmd.Append(UD.Column.ToString() + "=" + UD.Value);
            }
            return this;
        }
        #endregion

        #region Where
        /*
        /// <summary>
        /// Appends a conditional expression to the SQL <c>WHERE</c> clause of the <c>UPDATE</c> statement, optionally prefixed by a logical connector.
        /// </summary>
        /// <param name="Condition">
        /// The raw SQL condition to append (e.g., <c>Amount &gt; 100</c>, <c>Status = 'Active'</c>).
        /// </param>
        /// <param name="Connector">
        /// The logical connector to prepend before the condition (e.g., <c>AND</c>, <c>OR</c>). Defaults to <see cref="WhereConnectors.NONE"/>, which omits the connector.
        /// </param>
        /// <returns>
        /// The current <see cref="UpdateCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method tracks whether the <c>SET</c> clause has been started and inserts the appropriate connector if needed.
        /// Use in combination with <see cref="StartGroupedWhere"/> and <see cref="EndGroupedWhere"/> for complex logical expressions.
        /// </remarks>
        public UpdateCommand<T> Condition(string Condition, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasSets)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a structured conditional expression to the SQL <c>WHERE</c> clause of the <c>UPDATE</c> statement.
        /// </summary>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="T"/> representing the column to filter on.
        /// </param>
        /// <param name="Operator">
        /// The SQL comparison operator to apply (e.g., <c>=</c>, <c>&gt;</c>, <c>LIKE</c>), represented by the <see cref="SQLOperator"/> enum.
        /// </param>
        /// <param name="Value">
        /// The raw string value to compare against, assumed to be SQL-safe or pre-formatted.
        /// </param>
        /// <param name="Connector">
        /// The logical connector to prepend before the condition (e.g., <c>AND</c>, <c>OR</c>). Defaults to <see cref="WhereConnectors.NONE"/>, which omits the connector.
        /// </param>
        /// <returns>
        /// The current <see cref="UpdateCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method constructs a condition like <c>Status = 'Active'</c> or <c>Amount &gt; 100</c>, and inserts the appropriate connector if needed.
        /// Use in combination with <see cref="StartGroupedWhere"/> and <see cref="EndGroupedWhere"/> for complex logical expressions.
        /// </remarks>
        public UpdateCommand<T> Condition(T Column, SQLOperator Operator, string Value, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(Column.ToString() + Operator.ToSymbol() + Value);
            return this;
        }
        */
        #endregion
    }
}

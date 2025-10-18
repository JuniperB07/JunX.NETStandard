using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Represents a dynamic SQL <c>UPDATE</c> statement builder without compile-time column typing.
    /// </summary>
    /// <remarks>
    /// This class provides a fluent interface for constructing SQL <c>UPDATE</c> commands using string-based column names and values.
    /// It tracks internal state for <c>SET</c> and <c>WHERE</c> clause composition, enabling safe and modular update logic.
    /// Intended for scenarios where column metadata is resolved at runtime rather than via enums.
    /// </remarks>
    public class UpdateCommand
    {
        StringBuilder cmd;
        bool _hasSets;
        bool _hasWhere;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommand"/> class for building a SQL <c>UPDATE</c> statement.
        /// </summary>
        /// <remarks>
        /// This constructor sets up the internal command buffer and begins the statement with the <c>UPDATE</c> keyword.
        /// It also resets internal flags used to track whether any <c>SET</c> or <c>WHERE</c> clauses have been appended.
        /// Intended for dynamic SQL composition without compile-time column typing.
        /// </remarks>
        public UpdateCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("UPDATE ");
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
        /// This override finalizes the command buffer for execution or inspection. It assumes that any necessary <c>SET</c> and <c>WHERE</c> clauses have been appended.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ";";
        }

        #region PROPERTIES
        /*
        /// <summary>
        /// Begins the <c>WHERE</c> clause of the SQL <c>UPDATE</c> statement.
        /// </summary>
        /// <value>
        /// The current <see cref="UpdateCommand"/> instance, allowing fluent chaining of conditional expressions.
        /// </value>
        /// <remarks>
        /// This property appends the <c>WHERE</c> keyword to the command buffer, enabling the addition of conditional logic for targeted updates.
        /// It should be followed by column comparisons or logical expressions.
        /// </remarks>
        public UpdateCommand Where
        {
            get
            {
                cmd.Append(" WHERE ");
                return this;
            }
        }
        /// <summary>
        /// Begins a grouped <c>WHERE</c> clause in the SQL <c>UPDATE</c> statement, allowing compound conditions to be enclosed in parentheses.
        /// </summary>
        /// <value>
        /// The current <see cref="UpdateCommand"/> instance, allowing fluent chaining of conditional expressions.
        /// </value>
        /// <remarks>
        /// If a <c>WHERE</c> clause has already been started, this appends an opening parenthesis for grouping.
        /// Otherwise, it begins the clause with <c>WHERE (</c>, enabling nested or compound logic.
        /// If this method is used for the first time, there is no need to call the <c>Where</c> property separately.
        /// Use in combination with <c>EndGroupedWhere</c> to close the group.
        /// </remarks>
        public UpdateCommand StartGroupedWhere
        {
            get
            {
                if (_hasWhere)
                    cmd.Append(" (");
                else
                    cmd.Append(" WHERE (");
                return this;
            }
        }
        /// <summary>
        /// Closes a grouped <c>WHERE</c> clause in the SQL <c>UPDATE</c> statement.
        /// </summary>
        /// <value>
        /// The current <see cref="UpdateCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </value>
        /// <remarks>
        /// This property appends a closing parenthesis to the command buffer, completing a grouped conditional expression started by <c>StartGroupedWhere</c>.
        /// It should only be used after a corresponding <c>StartGroupedWhere</c> call to ensure proper SQL syntax.
        /// </remarks>
        public UpdateCommand EndGroupedWhere
        {
            get
            {
                cmd.Append(")");
                return this;
            }
        }
        */
        /// <summary>
        /// Begins composition of a SQL <c>WHERE</c> clause for an <c>UPDATE</c> command using fluent syntax.
        /// </summary>
        /// <returns>A new <see cref="WhereClause&lt;UpdateCommand&gt;"/> instance bound to this command.</returns>
        public WhereClause<UpdateCommand> StartWhere => new WhereClause<UpdateCommand>(this, cmd);
        #endregion

        #region UPDATE REGION
        /// <summary>
        /// Appends the target table name to the SQL <c>UPDATE</c> statement.
        /// </summary>
        /// <param name="Table">
        /// The name of the table to be updated.
        /// </param>
        /// <returns>
        /// The current <see cref="UpdateCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method completes the initial <c>UPDATE</c> clause by specifying the table name.
        /// It should be called before appending <c>SET</c> or <c>WHERE</c> clauses.
        /// </remarks>
        public UpdateCommand Update(string Table)
        {
            cmd.Append(Table);
            return this;
        }
        #endregion

        #region SET REGION
        /// <summary>
        /// Appends a single column-value assignment to the <c>SET</c> clause of the SQL <c>UPDATE</c> statement.
        /// </summary>
        /// <param name="UpdateData">
        /// An <see cref="UpdateMetadata"/> instance containing the column name, raw value, and its associated SQL data type.
        /// </param>
        /// <returns>
        /// The current <see cref="UpdateCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the <c>SET</c> clause if it hasn't been started, and appends the assignment in the form <c>Column=Value</c>.
        /// The value is formatted safely using <c>Construct.SQLSafeValue</c> via the metadata's <c>Value</c> property.
        /// </remarks>
        public UpdateCommand Set(UpdateMetadata UpdateData)
        {
            if (_hasSets)
                cmd.Append(", ");
            else
            {
                cmd.Append(" SET ");
                _hasSets = true;
            }
            cmd.Append(UpdateData.Column + "=" + UpdateData.Value);
            return this;
        }
        /// <summary>
        /// Appends multiple column-value assignments to the <c>SET</c> clause of the SQL <c>UPDATE</c> statement using metadata.
        /// </summary>
        /// <param name="UpdateData">
        /// A sequence of <see cref="UpdateMetadata"/> instances, each containing a column name, raw value, and its associated SQL data type.
        /// </param>
        /// <returns>
        /// The current <see cref="UpdateCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the <c>SET</c> clause if it hasn't been started, and appends each assignment in the form <c>Column=Value</c>.
        /// Values are formatted safely using the metadata's <c>Value</c> property, ensuring proper escaping and type-aware SQL hygiene.
        /// </remarks>
        public UpdateCommand Set(IEnumerable<UpdateMetadata> UpdateData)
        {
            foreach (UpdateMetadata UD in UpdateData)
            {
                if (_hasSets)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(" SET ");
                    _hasSets = true;
                }

                cmd.Append(UD.Column + "=" + UD.Value);
            }
            return this;
        }
        #endregion

        #region WHERE REGION
        /*
        /// <summary>
        /// Appends a raw conditional expression to the <c>WHERE</c> clause of the SQL <c>UPDATE</c> statement.
        /// </summary>
        /// <param name="Condition">
        /// A string representing the SQL condition (e.g., <c>"Amount &gt; 100"</c>) to be added to the <c>WHERE</c> clause.
        /// </param>
        /// <param name="Connector">
        /// A logical connector from <see cref="WhereConnectors"/> (e.g., <c>AND</c>, <c>OR</c>) used to join this condition with previous ones. Defaults to <c>NONE</c>.
        /// </param>
        /// <returns>
        /// The current <see cref="UpdateCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// If a <c>WHERE</c> clause has already been started, the specified connector is inserted before the condition.
        /// Otherwise, this marks the beginning of the <c>WHERE</c> clause and sets the internal flag accordingly.
        /// Use this method for dynamic or loosely typed condition injection. For grouped logic, pair with <c>StartGroupedWhere</c> and <c>EndGroupedWhere</c>.
        /// </remarks>
        public UpdateCommand Condition(string Condition, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a typed conditional expression to the <c>WHERE</c> clause of the SQL <c>UPDATE</c> statement.
        /// </summary>
        /// <param name="Left">
        /// The left-hand operand, typically a column name or expression.
        /// </param>
        /// <param name="Operator">
        /// The <see cref="SQLOperator"/> representing the comparison operator (e.g., <c>=</c>, <c>&gt;</c>, <c>LIKE</c>).
        /// </param>
        /// <param name="Right">
        /// The right-hand operand, typically a literal value or expression. It should be SQL-safe or preformatted.
        /// </param>
        /// <param name="Connector">
        /// A logical connector from <see cref="WhereConnectors"/> (e.g., <c>AND</c>, <c>OR</c>) used to join this condition with previous ones. Defaults to <c>NONE</c>.
        /// </param>
        /// <returns>
        /// The current <see cref="UpdateCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// If a <c>WHERE</c> clause has already been started, the specified connector is inserted before the condition.
        /// Otherwise, this marks the beginning of the <c>WHERE</c> clause and sets the internal flag accordingly.
        /// Use this method for structured condition composition. For grouped logic, pair with <c>StartGroupedWhere</c> and <c>EndGroupedWhere</c>.
        /// </remarks>
        public UpdateCommand Condition(string Left, SQLOperator Operator, string Right, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(Left + Operator.ToSymbol() + Right);
            return this;
        }
        */
        #endregion
    }
}

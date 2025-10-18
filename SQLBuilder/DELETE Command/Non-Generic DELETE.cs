using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Provides a fluent builder for composing SQL <c>DELETE</c> statements with optional conditional logic.
    /// </summary>
    /// <remarks>
    /// This class enables dynamic construction of SQL <c>DELETE</c> queries, including support for grouped and connected <c>WHERE</c> clauses.
    /// It tracks internal state to ensure proper clause sequencing and syntax.
    /// Designed for scenarios requiring flexible, string-based query generation without relying on strongly typed schema.
    /// </remarks>
    public class DeleteCommand
    {
        StringBuilder cmd;
        bool _hasWhere;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommand"/> class for building a SQL <c>DELETE</c> statement.
        /// </summary>
        /// <remarks>
        /// This constructor sets up the internal command buffer and begins the statement with <c>DELETE FROM</c>.
        /// It also resets the internal flag used to track whether any <c>WHERE</c> clauses have been appended.
        /// </remarks>
        public DeleteCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("DELETE FROM ");
            _hasWhere = false;
        }
        /// <summary>
        /// Returns the composed SQL <c>DELETE</c> statement as a string, terminated with a semicolon.
        /// </summary>
        /// <returns>
        /// A complete SQL query string representing the current state of the delete builder.
        /// </returns>
        /// <remarks>
        /// This override finalizes the command buffer for execution or inspection. It assumes that any necessary <c>WHERE</c> clauses have been appended.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ";";
        }

        #region Properties
        /*
        /// <summary>
        /// Begins the <c>WHERE</c> clause of the SQL <c>DELETE</c> statement.
        /// </summary>
        /// <value>
        /// The current <see cref="DeleteCommand{T}"/> instance, allowing fluent chaining of conditional expressions.
        /// </value>
        /// <remarks>
        /// This property appends the <c>WHERE</c> keyword to the command buffer, enabling the addition of conditional logic for targeted deletions.
        /// It should be followed by column comparisons or logical expressions.
        /// If <c>StartGroupedWhere</c> is used first, there is no need to call this property separately.
        /// </remarks>
        public DeleteCommand Where
        {
            get
            {
                cmd.Append(" WHERE ");
                return this;
            }
        }
        /// <summary>
        /// Begins a grouped <c>WHERE</c> clause in the SQL <c>DELETE</c> statement, allowing compound conditions to be enclosed in parentheses.
        /// </summary>
        /// <value>
        /// The current <see cref="DeleteCommand{T}"/> instance, allowing fluent chaining of conditional expressions.
        /// </value>
        /// <remarks>
        /// If a <c>WHERE</c> clause has already been started, this appends an opening parenthesis for grouping.
        /// Otherwise, it begins the clause with <c>WHERE (</c>, enabling nested or compound logic.
        /// If this method is used first, there is no need to call the <c>Where</c> property separately.
        /// Use in combination with <c>EndGroupedWhere</c> to close the group.
        /// </remarks>
        public DeleteCommand StartGroupedWhere
        {
            get
            {
                cmd.Append(" (");
                return this;
            }
        }
        /// <summary>
        /// Closes a grouped <c>WHERE</c> clause in the SQL <c>DELETE</c> statement.
        /// </summary>
        /// <value>
        /// The current <see cref="DeleteCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </value>
        /// <remarks>
        /// This property appends a closing parenthesis to the command buffer, completing a grouped conditional expression started by <c>StartGroupedWhere</c>.
        /// It should only be used after a corresponding <c>StartGroupedWhere</c> call to ensure proper SQL syntax.
        /// </remarks>
        public DeleteCommand EndGroupedWhere
        {
            get
            {
                cmd.Append(")");
                return this;
            }
        }
        */
        /// <summary>
        /// Begins composition of a SQL <c>WHERE</c> clause for a <c>DELETE</c> command without generic table bindings.
        /// </summary>
        /// <returns>A new <see cref="WhereClause&lt;DeleteCommand&gt;"/> instance bound to this command.</returns>
        public WhereClause<DeleteCommand> StartWhere => new WhereClause<DeleteCommand>(this, cmd);
        #endregion

        #region Where
        /*
        /// <summary>
        /// Appends a raw conditional expression to the SQL <c>WHERE</c> clause of the <c>DELETE</c> statement, optionally prefixed by a logical connector.
        /// </summary>
        /// <param name="Condition">
        /// The raw SQL condition to append (e.g., <c>Amount &gt; 100</c>, <c>Status = 'Inactive'</c>).
        /// </param>
        /// <param name="Connector">
        /// The logical connector to prepend before the condition (e.g., <c>AND</c>, <c>OR</c>). Defaults to <see cref="WhereConnectors.NONE"/>, which omits the connector.
        /// </param>
        /// <returns>
        /// The current <see cref="DeleteCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method tracks whether the <c>WHERE</c> clause has already been started and inserts the appropriate connector if needed.
        /// Use in combination with <see cref="StartGroupedWhere"/> and <see cref="EndGroupedWhere"/> to build compound conditional logic.
        /// </remarks>
        public DeleteCommand Condition(string Condition, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a structured conditional expression to the SQL <c>WHERE</c> clause of the <c>DELETE</c> statement.
        /// </summary>
        /// <param name="Left">
        /// The left-hand operand of the condition, typically a column name.
        /// </param>
        /// <param name="Operator">
        /// The SQL comparison operator to apply (e.g., <c>=</c>, <c>&gt;</c>, <c>LIKE</c>), represented by the <see cref="SQLOperator"/> enum.
        /// </param>
        /// <param name="Right">
        /// The right-hand operand of the condition, typically a raw or SQL-safe value.
        /// </param>
        /// <param name="Connector">
        /// The logical connector to prepend before the condition (e.g., <c>AND</c>, <c>OR</c>). Defaults to <see cref="WhereConnectors.NONE"/>, which omits the connector.
        /// </param>
        /// <returns>
        /// The current <see cref="DeleteCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method constructs a condition like <c>Status = 'Inactive'</c> or <c>Amount &gt; 100</c>, and inserts the appropriate connector if needed.
        /// Use in combination with <see cref="StartGroupedWhere"/> and <see cref="EndGroupedWhere"/> for compound logical expressions.
        /// </remarks>
        public DeleteCommand Condition(string Left, SQLOperator Operator, string Right, WhereConnectors Connector = WhereConnectors.NONE)
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

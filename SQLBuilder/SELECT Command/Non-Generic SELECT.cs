using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Provides a fluent, loosely typed builder for composing SQL <c>SELECT</c> statements using raw string identifiers.
    /// </summary>
    /// <remarks>
    /// This class supports dynamic SQL generation with fluent chaining of clauses such as <c>SELECT</c>, <c>FROM</c>, <c>JOIN</c>, <c>WHERE</c>, <c>ORDER BY</c>, and aliasing.
    /// It tracks internal state to manage comma placement, condition grouping, and clause sequencing.
    /// Intended for scenarios where table and column names are provided as strings, enabling flexible query composition without enum constraints.
    /// </remarks>
    public class SelectCommand
    {
        private StringBuilder cmd = new StringBuilder();
        private bool _hasColumns = false;
        private bool _hasWhere = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommand"/> class and begins a SQL <c>SELECT</c> statement.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the internal command buffer with <c>SELECT</c> and resets tracking flags for column and <c>WHERE</c> clause composition.
        /// Use this as the starting point for building dynamic SQL queries.
        /// </remarks>
        public SelectCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("SELECT ");
            _hasColumns = false;
            _hasWhere = false;
        }
        /// <summary>
        /// Returns the composed SQL <c>SELECT</c> statement as a string, terminated with a semicolon.
        /// </summary>
        /// <returns>
        /// A complete SQL query string representing the current state of the builder.
        /// </returns>
        /// <remarks>
        /// This override finalizes the query for execution or inspection by appending a terminating semicolon.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ";";
        }

        #region Properties
        /// <summary>
        /// Appends a DISTINCT clause to the SQL command to eliminate duplicate rows from the result set.
        /// </summary>
        public SelectCommand Distinct
        {
            get
            {
                cmd.Append(" DISTINCT ");
                return this;
            }
        }
        /// <summary>
        /// Begins composition of a SQL <c>WHERE</c> clause for a <c>SELECT</c> command without generic table bindings.
        /// </summary>
        /// <returns>A new <see cref="WhereClause&lt;SelectCommand&gt;"/> instance bound to this command.</returns>
        public WhereClause<SelectCommand> StartWhere => new WhereClause<SelectCommand>(this, cmd);
        /*
        /// <summary>
        /// Appends a <c>WHERE</c> clause to the SQL <c>SELECT</c> statement and returns the current builder for condition chaining.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of conditional expressions.
        /// </returns>
        /// <remarks>
        /// This property inserts the <c>WHERE</c> keyword into the query. It assumes that subsequent calls will append valid conditions.
        /// </remarks>
        public SelectCommand Where
        {
            get
            {
                cmd.Append(" WHERE ");
                return this;
            }
        }
        /// <summary>
        /// Begins a grouped <c>WHERE</c> clause by appending <c>WHERE (</c> or <c>(</c> depending on clause state.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of grouped conditional expressions.
        /// </returns>
        /// <remarks>
        /// This property inserts an opening parenthesis for grouped conditions. If no <c>WHERE</c> clause has been started, it prepends <c>WHERE</c>.
        /// Use in combination with <c>EndGroupedWhere</c> to wrap complex logical expressions.
        /// </remarks>
        public SelectCommand StartGroupedWhere
        {
            get
            {
                cmd.Append(" (");
                return this;
            }
        }
        /// <summary>
        /// Ends a grouped <c>WHERE</c> clause by appending a closing parenthesis to the SQL statement.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This property completes a logical grouping started with <see cref="StartGroupedWhere"/>, enabling nested or compound conditions.
        /// </remarks>
        public SelectCommand EndGroupedWhere
        {
            get
            {
                cmd.Append(")");
                return this;
            }
        }
        */
        #endregion

        #region Select
        /// <summary>
        /// Appends a column to the SQL <c>SELECT</c> clause using a raw string identifier.
        /// </summary>
        /// <param name="Column">
        /// The name of the column to include in the <c>SELECT</c> clause.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method supports dynamic or loosely typed column selection. If one or more columns have already been added, a comma is prepended.
        /// </remarks>
        public SelectCommand Select(string Column)
        {
            if (_hasColumns)
                cmd.Append(", ");
            else
                _hasColumns = true;
            cmd.Append(Column);
            return this;
        }
        /// <summary>
        /// Appends one or more raw column names to the SELECT clause of the SQL command.
        /// </summary>
        /// <param name="Columns">
        /// A variable-length array of raw column name strings to include in the SELECT clause.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance for fluent chaining.
        /// </returns>
        /// <remarks>
        /// Builds a comma-separated SELECT clause from plain string column names.
        /// </remarks>
        public SelectCommand Select(params string[] Columns)
        {
            if (Columns.Length < 1)
                throw new ArgumentException("Invalid parameter length.");

            foreach(string C in Columns)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                    _hasColumns = true;
                cmd.Append(C);
            }
            return this;
        }
        /// <summary>
        /// Appends multiple columns to the SQL <c>SELECT</c> clause using raw string identifiers.
        /// </summary>
        /// <param name="Columns">
        /// A sequence of column names to include in the <c>SELECT</c> clause.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method supports dynamic column selection. Commas are automatically inserted between columns as needed.
        /// </remarks>
        public SelectCommand Select(IEnumerable<string> Columns)
        {
            foreach (string C in Columns)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                    _hasColumns = true;
                cmd.Append(C);
            }
            return this;
        }
        #endregion

        #region Alias
        /// <summary>
        /// Appends an <c>AS</c> alias clause to the SQL statement, renaming the most recently added column or expression.
        /// </summary>
        /// <param name="Alias">
        /// The alias to assign, enclosed in single quotes (e.g., <c>'TotalAmount'</c>).
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method is typically used after a column or expression to assign a readable or contextual alias.
        /// It assumes that a valid column or expression was previously appended to the command buffer.
        /// </remarks>
        public SelectCommand As(string Alias)
        {
            cmd.Append(" AS '" + Alias + "'");
            return this;
        }
        #endregion

        #region From
        /// <summary>
        /// Appends a <c>FROM</c> clause to the SQL <c>SELECT</c> statement using a raw table name.
        /// </summary>
        /// <param name="Table">
        /// The name of the table to query from.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method sets the source table for the query. It assumes that the <c>SELECT</c> clause has already been initialized.
        /// </remarks>
        public SelectCommand From(string Table)
        {
            cmd.Append(" FROM " + Table);
            return this;
        }
        #endregion

        #region Where
        /*
        /// <summary>
        /// Appends a conditional expression to the SQL <c>WHERE</c> clause, optionally prefixed by a logical connector.
        /// </summary>
        /// <param name="Condition">
        /// The raw SQL condition to append (e.g., <c>Amount &gt; 100</c>).
        /// </param>
        /// <param name="Connector">
        /// The logical connector to prepend before the condition (e.g., <c>AND</c>, <c>OR</c>). Defaults to <c>NONE</c>, which omits the connector.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method tracks whether a <c>WHERE</c> clause has been started and inserts the appropriate connector if needed.
        /// Use in combination with <see cref="StartGroupedWhere"/> and <see cref="EndGroupedWhere"/> for complex logic.
        /// </remarks>
        public SelectCommand Condition(string Condition, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a conditional expression to the SQL <c>WHERE</c> clause using structured operands and a logical connector.
        /// </summary>
        /// <param name="Left">
        /// The left-hand side of the condition, typically a column name or expression.
        /// </param>
        /// <param name="Operator">
        /// The SQL comparison operator to apply (e.g., <c>=</c>, <c>&gt;</c>, <c>LIKE</c>), represented by the <see cref="SQLOperator"/> enum.
        /// </param>
        /// <param name="Right">
        /// The right-hand side of the condition, typically a literal value or parameter.
        /// </param>
        /// <param name="Connector">
        /// The logical connector to prepend before the condition (e.g., <c>AND</c>, <c>OR</c>). Defaults to <c>NONE</c>, which omits the connector.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method constructs a condition like <c>Amount &gt; 100</c> or <c>Name LIKE 'J%'</c>, and inserts the appropriate connector if needed.
        /// Use in combination with <see cref="StartGroupedWhere"/> and <see cref="EndGroupedWhere"/> for complex logical expressions.
        /// </remarks>
        public SelectCommand Condition(string Left, SQLOperator Operator, string Right, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(Left + Operator.ToString() + Right);
            return this;
        }
        */
        #endregion

        #region Join
        /// <summary>
        /// Appends a SQL <c>JOIN</c> clause to the <c>SELECT</c> statement using the specified join mode and condition.
        /// </summary>
        /// <param name="JoinMode">
        /// The type of join to apply (e.g., <c>INNER_JOIN</c>, <c>LEFT_JOIN</c>), represented by the <see cref="JoinModes"/> enum.
        /// Underscores in enum names are automatically replaced with spaces (e.g., <c>LEFT_JOIN</c> → <c>LEFT JOIN</c>).
        /// </param>
        /// <param name="JoinTable">
        /// The name of the table to join.
        /// </param>
        /// <param name="OnLeft">
        /// The left-hand side of the join condition, typically a column from the base table.
        /// </param>
        /// <param name="OnRight">
        /// The right-hand side of the join condition, typically a column from the joined table.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method constructs a join clause like <c>LEFT JOIN Orders ON Customers.Id = Orders.CustomerId</c>.
        /// It assumes that the base table has already been specified via <see cref="From(string)"/>.
        /// </remarks>
        public SelectCommand Join(JoinModes JoinMode, string JoinTable, string OnLeft, string OnRight)
        {
            cmd.Append(" " + JoinMode.ToString().Replace("_", " ") + " ");
            cmd.Append(JoinTable);
            cmd.Append(" ON " + OnLeft + "=" + OnRight);
            return this;
        }
        #endregion

        #region Order By
        /// <summary>
        /// Appends an <c>ORDER BY</c> clause to the SQL <c>SELECT</c> statement using the specified column and sort direction.
        /// </summary>
        /// <param name="OrderBy">
        /// The name of the column to sort by.
        /// </param>
        /// <param name="OrderMode">
        /// The sort direction, represented by the <see cref="MySQLOrderBy"/> enum (e.g., <c>ASC</c>, <c>DESC</c>).
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method constructs a clause like <c>ORDER BY CreatedDate DESC</c>. It assumes that the <c>SELECT</c> and <c>FROM</c> clauses have already been composed.
        /// </remarks>
        public SelectCommand OrderBy(string OrderBy, OrderByModes OrderMode)
        {
            cmd.Append(" ORDER BY " + OrderBy + " " + OrderMode.ToString());
            return this;
        }
        #endregion
    }

}

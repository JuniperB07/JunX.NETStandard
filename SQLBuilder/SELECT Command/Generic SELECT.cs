using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Provides a fluent, strongly typed builder for composing SQL <c>SELECT</c> statements using enum-based column definitions.
    /// </summary>
    /// <typeparam name="T">
    /// The enum type representing the table's column names. Must be a valid <see cref="Enum"/>.
    /// </typeparam>
    /// <remarks>
    /// This class supports dynamic SQL generation with fluent chaining of clauses such as <c>SELECT</c>, <c>FROM</c>, <c>WHERE</c>, <c>ORDER BY</c>, and aliasing.
    /// It tracks internal state to manage comma placement, condition grouping, and clause sequencing.
    /// Intended for scenarios where enum-based metadata drives query composition, such as code-generated schemas or SDK-level abstractions.
    /// </remarks>
    public class SelectCommand<T> where T: Enum
    {
        internal StringBuilder cmd = new StringBuilder();
        private bool _hasColumns = false;
        private bool _hasWhere = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommand{T}"/> class and begins composing an SQL <c>SELECT</c> statement.
        /// </summary>
        /// <remarks>
        /// Resets internal flags to track column and condition state during query construction.
        /// </remarks>
        public SelectCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("SELECT ");
            _hasColumns = false;
            _hasWhere = false;
        }
        /// <summary>
        /// Returns the composed SQL <c>SELECT</c> statement as a complete string.
        /// </summary>
        /// <returns>
        /// A string representing the finalized SQL query, terminated with a semicolon.
        /// </returns>
        /// <remarks>
        /// This method completes the builder output for execution or inspection.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ";";
        }

        #region Properties
        /// <summary>
        /// Appends the <c>FROM</c> clause to the SQL <c>SELECT</c> statement using the enum type name as the table name.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This property infers the table name from the enum type <typeparamref name="T"/> and appends it to the query.
        /// </remarks>
        public SelectCommand<T> From
        {
            get
            {
                cmd.Append(" FROM " + typeof(T).Name);
                return this;
            }
        }
        /// <summary>
        /// Appends a wildcard <c>*</c> to the SQL <c>SELECT</c> clause, selecting all columns from the primary table.
        /// </summary>
        /// <returns>The current <see cref="SelectCommand&lt;T&gt;"/> instance for fluent chaining.</returns>
        public SelectCommand<T> SelectAll
        {
            get
            {
                _hasColumns = true;
                cmd.Append("*");
                return this;
            }
        }
        /// <summary>
        /// Begins composition of a SQL <c>WHERE</c> clause for a <c>SELECT</c> command involving a single primary table.
        /// </summary>
        /// <returns>A new <see cref="WhereClause&lt;SelectCommand&lt;T&gt;, T&gt;"/> instance bound to this command.</returns>
        public WhereClause<SelectCommand<T>, T> StartWhere => new WhereClause<SelectCommand<T>, T>(this, cmd);
        /// <summary>
        /// Appends a DISTINCT clause to the SQL command to ensure unique rows in the result set.
        /// </summary>
        public SelectCommand<T> Distinct
        {
            get
            {
                cmd.Append(" DISTINCT ");
                return this;
            }
        }
        /*
        /// <summary>
        /// Appends a <c>WHERE</c> clause to the SQL <c>SELECT</c> statement, initiating conditional filtering.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This property begins the <c>WHERE</c> clause, enabling subsequent condition composition.
        /// </remarks>
        public SelectCommand<T> Where
        {
            get
            {
                cmd.Append(" WHERE ");
                return this;
            }
        }
        /// <summary>
        /// Begins a grouped <c>WHERE</c> clause in the SQL <c>SELECT</c> statement, opening a parenthesis for nested conditions.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// If a <c>WHERE</c> clause has already started, this appends an opening parenthesis for grouping.
        /// Otherwise, it starts the <c>WHERE</c> clause and opens the group.
        /// </remarks>
        public SelectCommand<T> StartGroupedWhere
        {
            get
            {
                cmd.Append(" (");
                return this;
            }
        }
        /// <summary>
        /// Ends a grouped <c>WHERE</c> clause in the SQL <c>SELECT</c> statement by appending a closing parenthesis.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This property closes a logical grouping started by <see cref="StartGroupedWhere"/>, enabling nested condition structures.
        /// </remarks>
        public SelectCommand<T> EndGroupedWhere
        {
            get
            {
                cmd.Append(")");
                return this;
            }
        }
        */
        #endregion

        #region Columns
        /// <summary>
        /// Appends a column to the SQL <c>SELECT</c> clause using an enum-defined column, with optional full qualification.
        /// </summary>
        /// <param name="Column">
        /// The enum member representing the column to select.
        /// </param>
        /// <param name="IsFullyQualified">
        /// If <c>true</c>, prefixes the column with the enum type name to fully qualify it (e.g., <c>Table.Column</c>); otherwise, uses the column name alone.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method builds the column list for the SQL select statement, inserting commas as needed.
        /// </remarks>
        public SelectCommand<T> Select(T Column, bool IsFullyQualified = false)
        {
            if (_hasColumns)
                cmd.Append(", ");
            else
                _hasColumns = true;

            if (IsFullyQualified)
                cmd.Append(typeof(T).Name + "." + Column.ToString());
            else
                cmd.Append(Column.ToString());

            return this;
        }
        /// <summary>
        /// Appends multiple columns to the SQL <c>SELECT</c> clause using an enumerable of enum-defined columns, with optional full qualification.
        /// </summary>
        /// <param name="Columns">
        /// A sequence of enum members representing the columns to select.
        /// </param>
        /// <param name="IsFullyQualified">
        /// If <c>true</c>, prefixes each column with the enum type name to fully qualify it (e.g., <c>Table.Column</c>); otherwise, uses the column name alone.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method builds the column list for the SQL select statement, inserting commas as needed.
        /// </remarks>
        public SelectCommand<T> Select(IEnumerable<T> Columns)
        {
            foreach (T C in Columns)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                    _hasColumns = true;

                cmd.Append(C.ToString());
            }
            return this;
        }
        #endregion

        #region Alias
        /// <summary>
        /// Appends an <c>AS</c> alias clause to the SQL statement, renaming the current expression or column.
        /// </summary>
        /// <param name="Alias">
        /// The alias to assign, enclosed in single quotes (e.g., <c>'TotalAmount'</c>).
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method is typically used after a column or expression to assign a readable or contextual alias.
        /// </remarks>
        public SelectCommand<T> As(string Alias)
        {
            cmd.Append(" AS '" + Alias + "'");
            return this;
        }
        /// <summary>
        /// Appends aliased column selections to the SQL <c>SELECT</c> clause using the provided metadata.
        /// </summary>
        /// <param name="SelectAs">
        /// A collection of <see cref="AliasMetadata{T}"/> instances, each specifying a column and its corresponding alias.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance for fluent chaining.
        /// </returns>
        /// <remarks>
        /// This method appends each column-alias pair to the internal SQL command builder.
        /// If columns have already been added, a comma separator is inserted before appending the next.
        /// Aliases are wrapped in single quotes to preserve casing and spacing.
        /// Intended for use in dynamic query composition where column renaming is required.
        /// </remarks>
        public SelectCommand<T> SelectAs(IEnumerable<AliasMetadata<T>> SelectAs)
        {
            foreach (AliasMetadata<T> SA in SelectAs)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                    _hasColumns = true;

                cmd.Append(SA.Column.ToString() + " AS '" + SA.Alias + "'");
            }
            return this;
        }
        #endregion

        #region Where Conditions
        /*
        /// <summary>
        /// Appends a conditional expression to the SQL <c>WHERE</c> clause using the specified connector.
        /// </summary>
        /// <param name="Where">
        /// The conditional expression to append, such as <c>"Age &gt; 30"</c> or <c>"Status = 'Active'"</c>.
        /// </param>
        /// <param name="Connector">
        /// The logical connector to use if a previous condition already exists, such as <c>AND</c> or <c>OR</c>. Defaults to <c>NONE</c>, which omits the connector for the first condition.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance for fluent chaining.
        /// </returns>
        /// <remarks>
        /// This method appends the provided condition to the internal SQL command builder.
        /// If a prior condition exists, the specified connector is inserted before the new condition.
        /// Useful for dynamically composing multi-part <c>WHERE</c> clauses in fluent query construction.
        /// </remarks>
        public SelectCommand<T> Condition(string Where, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(Where);
            return this;
        }
        /// <summary>
        /// Appends a typed conditional expression to the SQL <c>WHERE</c> clause using the specified column, operator, and value.
        /// </summary>
        /// <param name="Column">
        /// The column to filter, represented by the generic type <typeparamref name="T"/>.
        /// </param>
        /// <param name="Operator">
        /// The SQL comparison operator to apply, such as <c>=</c>, <c>&gt;</c>, or <c>LIKE</c>, provided via <see cref="SQLOperator"/>.
        /// </param>
        /// <param name="Value">
        /// The value to compare against, passed as a string literal.
        /// </param>
        /// <param name="Connector">
        /// The logical connector to use if a previous condition already exists, such as <c>AND</c> or <c>OR</c>. Defaults to <c>NONE</c>, which omits the connector for the first condition.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance for fluent chaining.
        /// </returns>
        /// <remarks>
        /// This method appends a typed condition to the internal SQL command builder.
        /// If a prior condition exists, the specified connector is inserted before the new condition.
        /// The operator symbol is resolved via <see cref="SQLOperator.ToSymbol"/>.
        /// Useful for building strongly typed, composable <c>WHERE</c> clauses in dynamic query generation.
        /// </remarks>
        public SelectCommand<T> Condition(T Column, SQLOperator Operator, string Value, WhereConnectors Connector = WhereConnectors.NONE)
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

        #region Order By
        /// <summary>
        /// Appends an <c>ORDER BY</c> clause to the SQL query using the specified column and sort direction.
        /// </summary>
        /// <param name="OrderBy">
        /// The column to sort by, represented by the generic type <typeparamref name="T"/>.
        /// </param>
        /// <param name="OrderMode">
        /// The sort direction, either <c>ASC</c> or <c>DESC</c>, specified via <see cref="OrderByModes"/>.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance for fluent chaining.
        /// </returns>
        /// <remarks>
        /// This method appends an <c>ORDER BY</c> clause to the internal SQL command builder,
        /// using the provided column and sort mode. Intended for use in dynamic query composition
        /// where result ordering is required.
        /// </remarks>
        public SelectCommand<T> OrderBy(T OrderBy, OrderByModes OrderMode)
        {
            cmd.Append(" ORDER BY " + OrderBy.ToString() + " " + OrderMode.ToString());
            return this;
        }
        #endregion
    }
}

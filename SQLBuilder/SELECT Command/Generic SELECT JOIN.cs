using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Provides a fluent, strongly typed builder for composing SQL <c>SELECT</c> statements involving joined tables represented by enum types.
    /// </summary>
    /// <typeparam name="T">
    /// The enum type representing the primary table's column names.
    /// </typeparam>
    /// <typeparam name="J">
    /// The enum type representing the joined table's column names.
    /// </typeparam>
    /// <remarks>
    /// This class supports dynamic SQL generation with fluent chaining of clauses such as <c>SELECT</c>, <c>FROM</c>, <c>JOIN</c>, <c>WHERE</c>, <c>ORDER BY</c>, and aliasing.
    /// It tracks internal state to manage comma placement, condition grouping, and clause sequencing across multiple enum-based sources.
    /// Intended for scenarios where joined table metadata is modeled via enums, enabling type-safe query composition and SDK-level abstraction.
    /// </remarks>
    public class SelectCommand<T, J> 
        where T : Enum 
        where J : Enum
    {
        private StringBuilder cmd = new StringBuilder();
        private bool _hasColumns = false;
        private bool _hasWhere = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommand{T, J}"/> class and begins composing an SQL <c>SELECT</c> statement.
        /// </summary>
        /// <remarks>
        /// This constructor sets up the internal SQL command builder with a starting <c>SELECT</c> clause,
        /// and resets internal flags used to track column and condition state during fluent query composition.
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
        /// This method completes the internal SQL builder output for inspection, logging, or execution.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ";";
        }
        internal StringBuilder SelectCMD { get { return cmd; } }

        #region Properties
        /// <summary>
        /// Appends the <c>FROM</c> clause to the SQL <c>SELECT</c> statement using the enum type <typeparamref name="T"/> as the table name.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This property infers the table name from the type <typeparamref name="T"/> and appends it to the query.
        /// Intended for use in queries where <typeparamref name="T"/> represents the primary table in a join or standalone selection.
        /// </remarks>
        public SelectCommand<T, J> From
        {
            get
            {
                cmd.Append(" FROM " + typeof(T).Name);
                return this;
            }
        }
        /// <summary>
        /// Appends a wildcard column selector for the primary table to the SQL <c>SELECT</c> clause.
        /// </summary>
        /// <returns>The current <see cref="SelectCommand&lt;T, J&gt;"/> instance for fluent chaining.</returns>
        public SelectCommand<T, J> SelectAllPrimary
        {
            get
            {
                _hasColumns = true;
                cmd.Append(typeof(T).Name + ".*");
                return this;
            }
        }
        /// <summary>
        /// Appends a wildcard column selector for the joined table to the SQL <c>SELECT</c> clause.
        /// </summary>
        /// <returns>The current <see cref="SelectCommand&lt;T, J&gt;"/> instance for fluent chaining.</returns>
        public SelectCommand<T, J> SelectAllJoined
        {
            get
            {
                _hasColumns = true;
                cmd.Append(typeof(J).Name + ".*");
                return this;
            }
        }
        /// <summary>
        /// Appends a DISTINCT clause to the SQL command to ensure unique rows in the result set.
        /// </summary>
        public SelectCommand<T, J> Distinct
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
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This property begins the <c>WHERE</c> clause, enabling subsequent condition composition using <c>Condition</c> methods.
        /// It does not include any conditions itself—only the clause keyword.
        /// </remarks>
        public SelectCommand<T, J> Where
        {
            get
            {
                cmd.Append(" WHERE ");
                return this;
            }
        }
        /// <summary>
        /// Begins a grouped <c>WHERE</c> clause in the SQL <c>SELECT</c> statement by appending an opening parenthesis for nested conditions.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This property appends an opening parenthesis to the SQL command builder, enabling logical grouping of conditions.
        /// It is typically used after initiating a <c>WHERE</c> clause to structure nested or compound filters.
        /// </remarks>
        public SelectCommand<T, J> StartGroupedWhere
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
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This property appends a closing parenthesis to the SQL command builder, completing a logical grouping started by <see cref="StartGroupedWhere"/>.
        /// Useful for structuring nested or compound conditions in dynamic query composition.
        /// </remarks>
        public SelectCommand<T, J> EndGroupedWhere
        {
            get
            {
                cmd.Append(")");
                return this;
            }
        }
        */
        /// <summary>
        /// Begins composition of a SQL <c>WHERE</c> clause for a <c>SELECT</c> command involving a primary and joined table.
        /// </summary>
        /// <returns>A new <see cref="WhereClause&lt;SelectCommand&lt;T, J&gt;, T, J&gt;"/> instance bound to this command.</returns>
        public WhereClause<SelectCommand<T, J>, T, J> StartWhere => new WhereClause<SelectCommand<T, J>, T, J>(this, cmd);
        #endregion

        #region Columns
        /// <summary>
        /// Appends a column to the SQL <c>SELECT</c> clause using an enum-defined column from the primary table <typeparamref name="T"/>.
        /// </summary>
        /// <param name="Column">
        /// The enum member representing the column to select from the primary table.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method builds the column list for the SQL <c>SELECT</c> statement, inserting commas as needed.
        /// The column is fully qualified using the enum type name (e.g., <c>Table.Column</c>) to support multi-table joins.
        /// </remarks>
        public SelectCommand<T, J> Select(T Column)
        {
            if (_hasColumns)
                cmd.Append(", ");
            else
                _hasColumns = true;
            cmd.Append(typeof(T).Name + "." + Column.ToString());
            return this;
        }
        /// <summary>
        /// Appends a column to the SQL <c>SELECT</c> clause using an enum-defined column from the joined table <typeparamref name="J"/>.
        /// </summary>
        /// <param name="Column">
        /// The enum member representing the column to select from the joined table.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method builds the column list for the SQL <c>SELECT</c> statement, inserting commas as needed.
        /// The column is fully qualified using the enum type name (e.g., <c>JoinedTable.Column</c>) to support multi-table joins.
        /// </remarks>
        public SelectCommand<T, J> Select(J Column)
        {
            if (_hasColumns)
                cmd.Append(", ");
            else
                _hasColumns = true;
            cmd.Append(typeof(J).Name + "." + Column.ToString());
            return this;
        }
        /// <summary>
        /// Appends multiple columns to the SQL <c>SELECT</c> clause using an enumerable of enum-defined columns from the primary table <typeparamref name="T"/>.
        /// </summary>
        /// <param name="Columns">
        /// A sequence of enum members representing the columns to select from the primary table.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method builds the column list for the SQL <c>SELECT</c> statement, inserting commas as needed.
        /// Each column is fully qualified using the enum type name (e.g., <c>PrimaryTable.Column</c>) to support multi-table joins.
        /// </remarks>
        public SelectCommand<T, J> Select(IEnumerable<T> Columns)
        {
            foreach (T C in Columns)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                    _hasColumns = true;

                cmd.Append(typeof(T).Name + "." + C.ToString());
            }
            return this;
        }
        /// <summary>
        /// Appends multiple columns to the SQL <c>SELECT</c> clause using an enumerable of enum-defined columns from the joined table <typeparamref name="J"/>.
        /// </summary>
        /// <param name="Columns">
        /// A sequence of enum members representing the columns to select from the joined table.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method builds the column list for the SQL <c>SELECT</c> statement, inserting commas as needed.
        /// Each column is fully qualified using the enum type name of <typeparamref name="J"/> (e.g., <c>JoinedTable.Column</c>) to support multi-table joins.
        /// </remarks>
        public SelectCommand<T, J> Select(IEnumerable<J> Columns)
        {
            foreach (J C in Columns)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                    _hasColumns = true;

                cmd.Append(typeof(J).Name + "." + C.ToString());
            }
            return this;
        }
        #endregion

        #region Alias
        /// <summary>
        /// Appends an <c>AS</c> alias clause to the SQL statement, renaming the most recently appended column or expression.
        /// </summary>
        /// <param name="Alias">
        /// The alias to assign, enclosed in single quotes (e.g., <c>'TotalAmount'</c>).
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method is typically used immediately after a column or expression to assign a readable or contextual alias.
        /// Aliases are wrapped in single quotes to preserve casing, spacing, or reserved keywords.
        /// </remarks>
        public SelectCommand<T, J> As(string Alias)
        {
            cmd.Append(" AS '" + Alias + "'");
            return this;
        }
        /// <summary>
        /// Appends aliased column selections to the SQL <c>SELECT</c> clause using metadata from the primary table <typeparamref name="T"/>.
        /// </summary>
        /// <param name="SelectAs">
        /// A collection of <see cref="AliasMetadata{T}"/> instances, each specifying a fully qualified column and its corresponding alias.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends each column-alias pair to the internal SQL command builder.
        /// If columns have already been added, a comma separator is inserted before appending the next.
        /// Aliases are wrapped in single quotes to preserve casing, spacing, or reserved keywords.
        /// Intended for use in dynamic query composition where column renaming is required for the primary table.
        /// </remarks>
        public SelectCommand<T, J> SelectAs(IEnumerable<AliasMetadata<T>> SelectAs)
        {
            foreach (AliasMetadata<T> SA in SelectAs)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                    _hasColumns = true;

                cmd.Append(SA.FullyQualified + " AS '" + SA.Alias + "'");
            }
            return this;
        }
        /// <summary>
        /// Appends aliased column selections to the SQL <c>SELECT</c> clause using metadata from the joined table <typeparamref name="J"/>.
        /// </summary>
        /// <param name="SelectAs">
        /// A collection of <see cref="AliasMetadata{J}"/> instances, each specifying a fully qualified column and its corresponding alias.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends each column-alias pair to the internal SQL command builder.
        /// If columns have already been added, a comma separator is inserted before appending the next.
        /// Aliases are wrapped in single quotes to preserve casing, spacing, or reserved keywords.
        /// Intended for use in dynamic query composition where column renaming is required for the joined table.
        /// </remarks>
        public SelectCommand<T, J> SelectAs(IEnumerable<AliasMetadata<J>> SelectAs)
        {
            foreach (AliasMetadata<J> SA in SelectAs)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                    _hasColumns = true;

                cmd.Append(SA.FullyQualified + " AS '" + SA.Alias + "'");
            }
            return this;
        }
        #endregion

        #region Join
        /// <summary>
        /// Appends a SQL <c>JOIN</c> clause to the <c>SELECT</c> statement using the specified join mode and column pair.
        /// </summary>
        /// <param name="JoinMode">
        /// The type of join to apply (e.g., <c>INNER_JOIN</c>, <c>LEFT_JOIN</c>), formatted with spaces between words.
        /// </param>
        /// <param name="Left">
        /// The enum member from the primary table <typeparamref name="T"/> representing the left-side join column.
        /// </param>
        /// <param name="Right">
        /// The enum member from the joined table <typeparamref name="J"/> representing the right-side join column.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a join clause to the SQL command builder, using fully qualified column names from both enum types.
        /// The join mode is formatted by replacing underscores with spaces to match SQL syntax.
        /// </remarks>
        public SelectCommand<T, J> Join(JoinModes JoinMode, T Left, J Right)
        {
            cmd.Append(" " + JoinMode.ToString().Replace("_", " ") + " " + typeof(J).Name);
            cmd.Append(" ON " + typeof(T).Name + "." + Left.ToString());
            cmd.Append("=" + typeof(J).Name + "." + Right.ToString());
            return this;
        }
        #endregion

        #region Condition
        /*
        /// <summary>
        /// Appends a raw SQL condition to the <c>WHERE</c> clause, optionally prefixed by a logical connector.
        /// </summary>
        /// <param name="Condition">
        /// The raw SQL condition string to append (e.g., <c>"Amount &gt; 100"</c>).
        /// </param>
        /// <param name="Connector">
        /// The logical connector to prepend before the condition (e.g., <c>AND</c>, <c>OR</c>). Defaults to <c>NONE</c>.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a condition to the <c>WHERE</c> clause, inserting the specified connector if prior conditions exist.
        /// Use this overload when composing custom or complex conditions not tied to enum-defined columns.
        /// </remarks>
        public SelectCommand<T, J> Condition(string Condition, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a typed condition to the SQL <c>WHERE</c> clause using a column from the primary table <typeparamref name="T"/>.
        /// </summary>
        /// <param name="Column">
        /// The enum member representing the column from the primary table to filter on.
        /// </param>
        /// <param name="Operator">
        /// The SQL comparison operator to apply (e.g., <c>=</c>, <c>&gt;</c>, <c>&lt;</c>), resolved via <see cref="SQLOperator.ToSymbol"/>.
        /// </param>
        /// <param name="Value">
        /// The value to compare against, inserted directly into the SQL condition.
        /// </param>
        /// <param name="Connector">
        /// The logical connector to prepend before the condition (e.g., <c>AND</c>, <c>OR</c>). Defaults to <c>NONE</c>.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a fully qualified condition to the <c>WHERE</c> clause, using the enum type name and column name.
        /// It inserts the connector only if prior conditions exist, enabling compound filtering logic.
        /// </remarks>
        public SelectCommand<T, J> Condition(T Column, SQLOperator Operator, string Value, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(typeof(T).Name + "." + Column.ToString() + Operator.ToSymbol() + Value);
            return this;
        }
        /// <summary>
        /// Appends a typed condition to the SQL <c>WHERE</c> clause using a column from the joined table <typeparamref name="J"/>.
        /// </summary>
        /// <param name="Column">
        /// The enum member representing the column from the joined table to filter on.
        /// </param>
        /// <param name="Operator">
        /// The SQL comparison operator to apply (e.g., <c>=</c>, <c>&gt;</c>, <c>&lt;</c>), resolved via <see cref="SQLOperator.ToSymbol"/>.
        /// </param>
        /// <param name="Value">
        /// The value to compare against, inserted directly into the SQL condition.
        /// </param>
        /// <param name="Connector">
        /// The logical connector to prepend before the condition (e.g., <c>AND</c>, <c>OR</c>). Defaults to <c>NONE</c>.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a fully qualified condition to the <c>WHERE</c> clause, using the enum type name and column name from <typeparamref name="J"/>.
        /// It inserts the connector only if prior conditions exist, enabling compound filtering logic across joined tables.
        /// </remarks>
        public SelectCommand<T, J> Condition(J Column, SQLOperator Operator, string Value, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(typeof(J).Name + "." + Column.ToString() + Operator.ToSymbol() + Value);
            return this;
        }
        */
        #endregion

        #region Order By
        /// <summary>
        /// Appends an <c>ORDER BY</c> clause to the SQL <c>SELECT</c> statement using a column from the primary table <typeparamref name="T"/>.
        /// </summary>
        /// <param name="OrderBy">
        /// The enum member representing the column to sort by from the primary table.
        /// </param>
        /// <param name="OrderMode">
        /// The sort direction to apply (e.g., <c>ASC</c>, <c>DESC</c>), specified via <see cref="OrderByModes"/>.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a fully qualified column and sort direction to the SQL <c>ORDER BY</c> clause.
        /// Intended for use when sorting results based on columns from the primary table.
        /// </remarks>
        public SelectCommand<T, J> OrderBy(T OrderBy, OrderByModes OrderMode)
        {
            cmd.Append(" ORDER BY " + typeof(T).Name + "." + OrderBy.ToString() + " " + OrderMode.ToString());
            return this;
        }
        /// <summary>
        /// Appends an <c>ORDER BY</c> clause to the SQL <c>SELECT</c> statement using a column from the joined table <typeparamref name="J"/>.
        /// </summary>
        /// <param name="OrderBy">
        /// The enum member representing the column to sort by from the joined table.
        /// </param>
        /// <param name="OrderMode">
        /// The sort direction to apply (e.g., <c>ASC</c>, <c>DESC</c>), specified via <see cref="OrderByModes"/>.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T, J}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a fully qualified column and sort direction to the SQL <c>ORDER BY</c> clause.
        /// Intended for use when sorting results based on columns from the joined table.
        /// </remarks>
        public SelectCommand<T, J> OrderBy(J OrderBy, OrderByModes OrderMode)
        {
            cmd.Append(" ORDER BY " + typeof(J).Name + "." + OrderBy.ToString() + " " + OrderMode.ToString());
            return this;
        }
        #endregion
    }
}

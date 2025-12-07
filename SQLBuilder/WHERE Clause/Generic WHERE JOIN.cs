using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Provides a fluent interface for composing SQL <c>WHERE</c> clauses with support for logical operators and joined table references.
    /// </summary>
    /// <typeparam name="TCommand">The parent SQL builder type that this clause is attached to, enabling fluent chaining.</typeparam>
    /// <typeparam name="T">An enum type representing the primary table's column identifiers.</typeparam>
    /// <typeparam name="J">An enum type representing the joined table's column identifiers.</typeparam>
    public class WhereClause<TCommand, T, J>
        where TCommand : class
        where T : Enum
        where J : Enum
    {
        private readonly TCommand _parent;
        private readonly StringBuilder _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhereClause&lt;TCommand, T, J&gt;"/> class, binding it to the parent SQL builder and the shared command buffer.
        /// </summary>
        /// <param name="parent">The parent SQL builder instance that this clause is attached to, enabling fluent chaining.</param>
        /// <param name="cmd">The <see cref="StringBuilder"/> used to compose and accumulate the SQL command text.</param>
        public WhereClause(TCommand parent, StringBuilder cmd)
        {
            _parent = parent;
            _cmd = cmd;
        }
        /// <summary>
        /// Ends the <c>WHERE</c> clause composition and returns control to the parent SQL builder for continued fluent chaining.
        /// </summary>
        /// <returns>The parent <typeparamref name="TCommand"/> instance.</returns>
        public TCommand EndWhere => _parent;


        #region Where
        /// <summary>
        /// Appends a SQL <c>WHERE</c> clause with the specified raw condition string to the command buffer.
        /// </summary>
        /// <param name="Condition">The raw SQL condition to include in the <c>WHERE</c> clause.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> Where(string Condition)
        {
            _cmd.Append(" WHERE ").Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>WHERE</c> clause using a column from the primary table, the specified operator, and comparison value.
        /// </summary>
        /// <param name="Left">The enum value representing the column name from the primary table.</param>
        /// <param name="Operator">The SQL operator used in the comparison (e.g., equals, less than).</param>
        /// <param name="Right">The value to compare against, represented as a string.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> Where(T Left, SQLOperator Operator, object Right)
        {
            _cmd.Append(" WHERE ");
            _cmd.Append(typeof(T).Name + "." + Left.ToString() + Operator.ToSymbol() + Right.ToString());
            return this;
        }
        /// <summary>
        /// Appends a WHERE clause condition to the SQL command being built.
        /// </summary>
        /// <param name="Left">
        /// The column or field on the left-hand side of the condition, represented by the generic type <typeparamref name="T"/>.
        /// </param>
        /// <param name="Operator">
        /// The SQL operator (e.g., Equal, NotEqual, GreaterThan) used to compare the values.
        /// </param>
        /// <param name="Right">
        /// A <see cref="ConditionMetadata"/> instance representing the right-hand side value,
        /// including its raw data and type-safe SQL representation.
        /// </param>
        /// <returns>
        /// Returns the current <see cref="WhereClause{TCommand, T, J}"/> instance so that
        /// additional conditions can be chained fluently.
        /// </returns>
        /// <remarks>
        /// This method generates a WHERE clause in the form:
        /// <c>WHERE Table.Column Operator Value</c>
        /// and appends it to the SQL command builder.
        /// </remarks>

        /// <summary>
        /// Appends a SQL <c>WHERE</c> clause using a column from the joined table, the specified operator, and comparison value.
        /// </summary>
        /// <param name="Left">The enum value representing the column name from the joined table.</param>
        /// <param name="Operator">The SQL operator used in the comparison.</param>
        /// <param name="Right">The value to compare against, represented as a string.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> Where(J Left, SQLOperator Operator, object Right)
        {
            _cmd.Append(" WHERE ");
            _cmd.Append(typeof(J).Name + "." + Left.ToString() + Operator.ToSymbol() + Right.ToString());
            return this;
        }
        /// <summary>
        /// Appends a WHERE clause condition to the SQL command being built,
        /// with the column or field represented by an enumerated type.
        /// </summary>
        /// <typeparam name="J">
        /// The enumeration type representing the columns of the joined table.
        /// Must be an <see cref="Enum"/>.
        /// </typeparam>
        /// <param name="Left">
        /// The enum value indicating the left-hand side column or field of the condition.
        /// </param>
        /// <param name="Operator">
        /// The SQL operator (e.g., Equal, NotEqual, GreaterThan) used to compare the values.
        /// </param>
        /// <param name="Right">
        /// A <see cref="ConditionMetadata"/> instance representing the right-hand side value,
        /// including its raw data and type-safe SQL representation.
        /// </param>
        /// <returns>
        /// Returns the current <see cref="WhereClause{TCommand, T, J}"/> instance so that
        /// additional conditions can be chained fluently.
        /// </returns>
        /// <remarks>
        /// This method generates a WHERE clause in the form:
        /// <c>WHERE JoinedTable.Column Operator Value</c>
        /// and appends it to the SQL command builder.
        /// </remarks>
        #endregion

        #region Group
        /// <summary>
        /// Begins a grouped SQL condition by appending an opening parenthesis followed by the specified raw condition string.
        /// </summary>
        /// <param name="Condition">The raw SQL condition to start within the group.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> StartGroup(string Condition)
        {
            _cmd.Append(" (").Append(Condition);
            return this;
        }
        /// <summary>
        /// Begins a grouped SQL condition by appending an opening parenthesis followed by a comparison using a column from the primary table.
        /// </summary>
        /// <param name="Left">The enum value representing the column name from the primary table.</param>
        /// <param name="Operator">The SQL operator used in the comparison.</param>
        /// <param name="Right">The value to compare against, represented as a string.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> StartGroup(T Left, SQLOperator Operator, object Right)
        {
            _cmd.Append(" (");
            _cmd.Append(typeof(T).Name + "." + Left.ToString() + Operator.ToSymbol() + Right.ToString());
            return this;
        }
        /// <summary>
        /// Begins a grouped SQL condition by appending an opening parenthesis followed by a comparison using a column from the joined table.
        /// </summary>
        /// <param name="Left">The enum value representing the column name from the joined table.</param>
        /// <param name="Operator">The SQL operator used in the comparison.</param>
        /// <param name="Right">The value to compare against, represented as a string.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> StartGroup(J Left, SQLOperator Operator, string Right)
        {
            _cmd.Append(" (");
            _cmd.Append(typeof(J).Name + "." + Left.ToString() + Operator.ToSymbol() + Right);
            return this;
        }
        /// <summary>
        /// Ends a grouped SQL condition by appending a closing parenthesis to the command buffer.
        /// </summary>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> EndGroup
        {
            get
            {
                _cmd.Append(")");
                return this;
            }
        }
        #endregion

        #region And
        /// <summary>
        /// Appends a SQL <c>AND</c> connector to the current <c>WHERE</c> clause, enabling composition of additional conditions across joined tables.
        /// </summary>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> And()
        {
            _cmd.Append(" AND ");
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>AND</c> condition to the current <c>WHERE</c> clause using the specified raw condition string.
        /// </summary>
        /// <param name="Condition">The raw SQL condition to append after the <c>AND</c> keyword.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> And(string Condition)
        {
            _cmd.Append(" AND ").Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>AND</c> condition to the current <c>WHERE</c> clause using a column from the primary table, the specified operator, and comparison value.
        /// </summary>
        /// <param name="Left">The enum value representing the column name from the primary table.</param>
        /// <param name="Operator">The SQL operator used in the comparison.</param>
        /// <param name="Right">The value to compare against, represented as a string.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> And(T Left, SQLOperator Operator, string Right)
        {
            _cmd.Append(" AND ");
            _cmd.Append(typeof(T).Name + "." + Left.ToString() + Operator.ToSymbol() + Right);
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>AND</c> condition to the current <c>WHERE</c> clause using a column from the joined table, the specified operator, and comparison value.
        /// </summary>
        /// <param name="Left">The enum value representing the column name from the joined table.</param>
        /// <param name="Operator">The SQL operator used in the comparison.</param>
        /// <param name="Right">The value to compare against, represented as a string.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> And(J Left, SQLOperator Operator, string Right)
        {
            _cmd.Append(" AND ");
            _cmd.Append(typeof(J).Name + "." + Left.ToString() + Operator.ToSymbol() + Right);
            return this;
        }
        #endregion

        #region Or
        /// <summary>
        /// Appends a SQL <c>OR</c> connector to the current <c>WHERE</c> clause, enabling composition of alternative conditions across joined tables.
        /// </summary>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> Or()
        {
            _cmd.Append(" OR ");
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>OR</c> condition to the current <c>WHERE</c> clause using the specified raw condition string.
        /// </summary>
        /// <param name="Condition">The raw SQL condition to append after the <c>OR</c> keyword.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> Or(string Condition)
        {
            _cmd.Append(" OR ").Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>OR</c> condition to the current <c>WHERE</c> clause using a column from the primary table, the specified operator, and comparison value.
        /// </summary>
        /// <param name="Left">The enum value representing the column name from the primary table.</param>
        /// <param name="Operator">The SQL operator used in the comparison.</param>
        /// <param name="Right">The value to compare against, represented as a string.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> Or(T Left, SQLOperator Operator, string Right)
        {
            _cmd.Append(" OR ");
            _cmd.Append(typeof(T).Name + "." + Left.ToString() + Operator.ToSymbol() + Right);
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>OR</c> condition to the current <c>WHERE</c> clause using a column from the joined table, the specified operator, and comparison value.
        /// </summary>
        /// <param name="Left">The enum value representing the column name from the joined table.</param>
        /// <param name="Operator">The SQL operator used in the comparison.</param>
        /// <param name="Right">The value to compare against, represented as a string.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T, J&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T, J> Or(J Left, SQLOperator Operator, string Right)
        {
            _cmd.Append(" OR ");
            _cmd.Append(typeof(J).Name + "." + Left.ToString() + Operator.ToSymbol() + Right);
            return this;
        }
        #endregion

        #region BETWEEN
        /// <summary>
        /// Appends a SQL <c>WHERE</c> clause with a <c>BETWEEN</c> condition for the specified column and range.
        /// </summary>
        /// <param name="Column">The column to apply the <c>BETWEEN</c> filter to.</param>
        /// <param name="Left">The lower bound of the range, inserted as-is into the SQL statement.</param>
        /// <param name="Right">The upper bound of the range, inserted as-is into the SQL statement.</param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T, J}"/> instance with the appended <c>WHERE ... BETWEEN</c> clause.
        /// </returns>
        /// <remarks>
        /// This method constructs a SQL fragment in the form:
        /// <code>
        /// WHERE Column BETWEEN Left AND Right
        /// </code>
        /// It does not escape or quote values, and assumes the caller provides valid SQL-compatible strings.
        /// If a <c>WHERE</c> clause already exists, this method may produce invalid SQL unless manually adjusted.
        /// </remarks>
        public WhereClause<TCommand, T, J> WhereBetween(T Column, string Left, string Right)
        {
            _cmd.Append(" WHERE " + Column.ToString());
            _cmd.Append(" BETWEEN " + Left + " AND " + Right);
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>WHERE</c> clause with a <c>BETWEEN</c> condition for the specified join column and range.
        /// </summary>
        /// <param name="Column">The join column to apply the <c>BETWEEN</c> filter to.</param>
        /// <param name="Left">The lower bound of the range, inserted directly into the SQL statement.</param>
        /// <param name="Right">The upper bound of the range, inserted directly into the SQL statement.</param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T, J}"/> instance with the appended <c>WHERE ... BETWEEN</c> clause.
        /// </returns>
        /// <remarks>
        /// This method constructs a SQL fragment in the form:
        /// <code>
        /// WHERE Column BETWEEN Left AND Right
        /// </code>
        /// It assumes no prior <c>WHERE</c> clause exists and does not escape or quote values.
        /// Use with caution when chaining multiple conditions or working with string/date values.
        /// </remarks>
        public WhereClause<TCommand, T, J> WhereBetween(J Column, string Left, string Right)
        {
            _cmd.Append(" WHERE " + Column.ToString());
            _cmd.Append(" BETWEEN " + Left + " AND " + Right);
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>BETWEEN</c> clause for the specified column and range values.
        /// </summary>
        /// <param name="Column">The column to apply the <c>BETWEEN</c> condition to.</param>
        /// <param name="Left">The lower bound of the range, inserted directly into the SQL statement.</param>
        /// <param name="Right">The upper bound of the range, inserted directly into the SQL statement.</param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T, J}"/> instance with the appended <c>BETWEEN</c> clause.
        /// </returns>
        /// <remarks>
        /// This method constructs a SQL fragment in the form:
        /// <code>
        /// Column BETWEEN Left AND Right
        /// </code>
        /// It assumes that a <c>WHERE</c> or <c>AND</c> clause has already been appended.
        /// Values are inserted as-is, without quoting or escaping. Use with caution when working with strings or dates.
        /// </remarks>
        public WhereClause<TCommand, T, J> Between(T Column, string Left, string Right)
        {
            _cmd.Append(" " + Column.ToString());
            _cmd.Append(" BETWEEN " + Left + " AND " + Right);
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>BETWEEN</c> clause for the specified join column and range values.
        /// </summary>
        /// <param name="Column">The join column to apply the <c>BETWEEN</c> condition to.</param>
        /// <param name="Left">The lower bound of the range, inserted directly into the SQL statement.</param>
        /// <param name="Right">The upper bound of the range, inserted directly into the SQL statement.</param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T, J}"/> instance with the appended <c>BETWEEN</c> clause.
        /// </returns>
        /// <remarks>
        /// This method constructs a SQL fragment in the form:
        /// <code>
        /// Column BETWEEN Left AND Right
        /// </code>
        /// It assumes that a <c>WHERE</c> or <c>AND</c> clause has already been appended.
        /// Values are inserted as-is, without quoting or escaping. Use with caution when working with strings or dates.
        /// </remarks>
        public WhereClause<TCommand, T, J> Between(J Column, string Left, string Right)
        {
            _cmd.Append(" " + Column.ToString());
            _cmd.Append(" BETWEEN " + Left + " AND " + Right);
            return this;
        }
        #endregion
    }
}

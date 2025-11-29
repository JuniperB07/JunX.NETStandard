using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Provides a fluent interface for composing SQL <c>WHERE</c> clauses, including logical operators and grouping, for use within generic SQL builder commands.
    /// </summary>
    /// <typeparam name="TCommand">The parent SQL builder type that this clause is attached to, enabling fluent chaining back to the command.</typeparam>
    /// <typeparam name="T">An enum type representing the schema or column identifiers used in the conditional expressions.</typeparam>
    public class WhereClause<TCommand, T>
        where TCommand: class
        where T: Enum
    {
        private readonly TCommand _parent;
        private readonly StringBuilder _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhereClause{TCommand, T}"/> class, binding it to the parent SQL builder and the shared command buffer.
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
        /// Appends a SQL <c>WHERE</c> clause with the specified condition to the command buffer.
        /// </summary>
        /// <param name="Condition">The raw SQL condition to include in the <c>WHERE</c> clause.</param>
        /// <returns>The current <see cref="WhereClause{TCommand, T}"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T> Where (string Condition)
        {
            _cmd.Append(" WHERE ").Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>WHERE</c> clause using the specified column, operator, and value.
        /// </summary>
        /// <param name="Left">The enum value representing the column name to compare.</param>
        /// <param name="Operator">The SQL operator used in the comparison (e.g., equals, greater than).</param>
        /// <param name="Right">The value to compare against, represented as a string.</param>
        /// <returns>The current <see cref="WhereClause{TCommand, T}"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T> Where(T Left, SQLOperator Operator, string Right)
        {
            _cmd.Append(" WHERE ");
            _cmd.Append($"{typeof(T).Name}.{Left.ToString()}" + Operator.ToSymbol() + Right);
            return this;
        }
        /// <summary>
        /// Appends a <c>WHERE</c> clause to the SQL statement, 
        /// comparing a column from the specified table <typeparamref name="J"/> 
        /// against a provided value.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the table whose column is being used in the condition.
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="Left">
        /// The enum member representing the column from <typeparamref name="J"/> 
        /// to be used on the left side of the condition.
        /// </param>
        /// <param name="Operator">
        /// The comparison operator to apply, specified via <see cref="SQLOperator"/>.
        /// </param>
        /// <param name="Right">
        /// The value to compare against the specified column.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a fully qualified <c>WHERE</c> clause in the form 
        /// <c>[TableName].[Column] [Operator] [Value]</c>. 
        /// The operator is resolved using <see cref="SQLOperator.ToSymbol"/>.
        /// </remarks>
        public WhereClause<TCommand, T> Where<J>(J Left, SQLOperator Operator, object Right) where J: Enum
        {
            _cmd.Append($" WHERE {typeof(J).Name}.{Left.ToString()} {Operator.ToSymbol()} {Right.ToString()}");
            return this;
        }
        #endregion


        #region Group
        /// <summary>
        /// Begins a grouped SQL condition by appending an opening parenthesis followed by the specified condition.
        /// </summary>
        /// <param name="Condition">The condition to include within the opening of the group.</param>
        /// <returns>The current <see cref="WhereClause{TCommand, T}"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T> StartGroup(string Condition)
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
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T> StartGroup(T Left, SQLOperator Operator, string Right)
        {
            _cmd.Append(" (");
            _cmd.Append(Left.ToString() + Operator.ToSymbol() + Right);
            return this;
        }
        /// <summary>
        /// Appends a closing parenthesis to terminate a grouped SQL condition.
        /// </summary>
        /// <returns>The current <see cref="WhereClause{TCommand, T}"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T> EndGroup
        {
            get
            {
                _cmd.Append(")");
                return this;
            }
        }
        #endregion

        #region AND
        /// <summary>
        /// Appends a SQL <c>AND</c> connector to the current <c>WHERE</c> clause, enabling composition of additional conditions.
        /// </summary>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T> And()
        {
            _cmd.Append(" AND ");
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>AND</c> condition to the current <c>WHERE</c> clause using the specified raw condition string.
        /// </summary>
        /// <param name="Condition">The raw SQL condition to append after the <c>AND</c> keyword.</param>
        /// <returns>The current <see cref="WhereClause{TCommand, T}"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T> And(string Condition)
        {
            _cmd.Append(" AND ").Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>AND</c> condition to the current <c>WHERE</c> clause using the specified column, operator, and value.
        /// </summary>
        /// <param name="Left">The enum value representing the column name to compare.</param>
        /// <param name="Operator">The SQL operator used in the comparison.</param>
        /// <param name="Right">The value to compare against, represented as a string.</param>
        /// <returns>The current <see cref="WhereClause{TCommand, T}"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T> And(T Left, SQLOperator Operator, string Right)
        {
            _cmd.Append(" AND ");
            _cmd.Append($"{typeof(T).Name}.{Left.ToString()}" + Operator.ToSymbol() + Right);
            return this;
        }
        /// <summary>
        /// Appends an additional <c>AND</c> condition to the SQL <c>WHERE</c> clause, 
        /// comparing a column from the specified table <typeparamref name="J"/> against a provided value.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the table whose column is being used in the condition.
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="Left">
        /// The enum member representing the column from <typeparamref name="J"/> 
        /// to be used on the left side of the condition.
        /// </param>
        /// <param name="Operator">
        /// The comparison operator to apply, specified via <see cref="SQLOperator"/>.
        /// </param>
        /// <param name="Right">
        /// The value to compare against the specified column.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a fully qualified <c>AND</c> condition in the form 
        /// <c>[TableName].[Column] [Operator] [Value]</c>. 
        /// The operator is resolved using <see cref="SQLOperator.ToSymbol"/>. 
        /// It is intended for use after an initial <c>WHERE</c> clause to build compound conditions.
        /// </remarks>
        public WhereClause<TCommand, T> And<J>(J Left, SQLOperator Operator, object Right) where J: Enum
        {
            _cmd.Append($" AND {typeof(J).Name}.{Left.ToString()} {Operator.ToSymbol()} {Right.ToString()}");
            return this;
        }
        #endregion

        #region OR
        /// <summary>
        /// Appends a SQL <c>OR</c> connector to the current <c>WHERE</c> clause, enabling composition of alternative conditions.
        /// </summary>
        /// <returns>The current <see cref="WhereClause&lt;TCommand, T&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T> Or()
        {
            _cmd.Append(" OR ");
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>OR</c> condition to the current <c>WHERE</c> clause using the specified raw condition string.
        /// </summary>
        /// <param name="Condition">The raw SQL condition to append after the <c>OR</c> keyword.</param>
        /// <returns>The current <see cref="WhereClause{TCommand, T}"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T> Or(string Condition)
        {
            _cmd.Append(" OR ").Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>OR</c> condition to the current <c>WHERE</c> clause using the specified column, operator, and value.
        /// </summary>
        /// <param name="Left">The enum value representing the column name to compare.</param>
        /// <param name="Operator">The SQL operator used in the comparison.</param>
        /// <param name="Right">The value to compare against, represented as a string.</param>
        /// <returns>The current <see cref="WhereClause{TCommand, T}"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand, T> Or(T Left, SQLOperator Operator, string Right)
        {
            _cmd.Append(" OR ");
            _cmd.Append($"{typeof(T).Name}.{Left.ToString()}" + Operator.ToSymbol() + Right);
            return this;
        }
        /// <summary>
        /// Appends an additional <c>OR</c> condition to the SQL <c>WHERE</c> clause, 
        /// comparing a column from the specified table <typeparamref name="J"/> against a provided value.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the table whose column is being used in the condition.
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="Left">
        /// The enum member representing the column from <typeparamref name="J"/> 
        /// to be used on the left side of the condition.
        /// </param>
        /// <param name="Operator">
        /// The comparison operator to apply, specified via <see cref="SQLOperator"/>.
        /// </param>
        /// <param name="Right">
        /// The value to compare against the specified column.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a fully qualified <c>OR</c> condition in the form 
        /// <c>[TableName].[Column] [Operator] [Value]</c>. 
        /// The operator is resolved using <see cref="SQLOperator.ToSymbol"/>. 
        /// It is intended for use after an initial <c>WHERE</c> clause or <c>AND</c> condition 
        /// to build compound logical expressions.
        /// </remarks>
        public WhereClause<TCommand, T> Or<J>(J Left, SQLOperator Operator, object Right) where J: Enum
        {
            _cmd.Append($" OR {typeof(J).Name}.{Left.ToString()} {Operator.ToSymbol()} {Right.ToString()}");
            return this;
        }
        #endregion

        #region BETWEEN
        /// <summary>
        /// Appends a SQL <c>WHERE</c> clause that filters the specified column between two values.
        /// </summary>
        /// <param name="Column">The column to apply the <c>BETWEEN</c> condition to.</param>
        /// <param name="Left">The lower bound of the range.</param>
        /// <param name="Right">The upper bound of the range.</param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance with the appended <c>WHERE ... BETWEEN</c> clause.
        /// </returns>
        /// <remarks>
        /// This method constructs a SQL fragment in the form:
        /// <code>
        /// WHERE Column BETWEEN Left AND Right
        /// </code>
        /// It does not escape or quote values, and assumes the caller provides valid SQL-compatible strings.
        /// If a <c>WHERE</c> clause already exists, this method may produce invalid SQL unless manually adjusted.
        /// </remarks>
        public WhereClause<TCommand, T> WhereBetween(T Column, string Left, string Right)
        {
            _cmd.Append(" WHERE " + $"{typeof(T).Name}.{Column.ToString()}");
            _cmd.Append(" BETWEEN " + Left + " AND " + Right);
            return this;
        }
        /// <summary>
        /// Appends a <c>WHERE</c> clause with a <c>BETWEEN</c> condition to the SQL statement, 
        /// restricting results to values within the specified range.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the table whose column is being used in the condition.
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="Column">
        /// The enum member representing the column from <typeparamref name="J"/> 
        /// to be evaluated in the <c>BETWEEN</c> condition.
        /// </param>
        /// <param name="Left">
        /// The lower bound value of the range.
        /// </param>
        /// <param name="Right">
        /// The upper bound value of the range.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a fully qualified <c>WHERE</c> clause in the form 
        /// <c>[TableName].[Column] BETWEEN [LeftValue] AND [RightValue]</c>. 
        /// It is intended for filtering results based on a range of values.
        /// </remarks>
        public WhereClause<TCommand, T> WhereBetween<J>(J Column, object Left, object Right) where J: Enum
        {
            _cmd.Append($" WHERE {typeof(J).Name}.{Column.ToString()} BETWEEN {Left.ToString()} AND {Right.ToString()}");
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>BETWEEN</c> clause for the specified column and range values.
        /// </summary>
        /// <param name="Column">The column to apply the <c>BETWEEN</c> condition to.</param>
        /// <param name="Left">The lower bound of the range.</param>
        /// <param name="Right">The upper bound of the range.</param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance with the appended <c>BETWEEN</c> clause.
        /// </returns>
        /// <remarks>
        /// This method constructs a SQL fragment in the form:
        /// <code>
        /// Column BETWEEN Left AND Right
        /// </code>
        /// It assumes the caller has already appended a <c>WHERE</c> or <c>AND</c> clause if needed.
        /// Values are inserted as-is, without quoting or escaping. Use with caution for strings or dates.
        /// </remarks>
        public WhereClause<TCommand, T> Between(T Column, string Left, string Right)
        {
            _cmd.Append(" " + $"{typeof(T).Name}.{Column.ToString()}");
            _cmd.Append(" BETWEEN " + Left + " AND " + Right);
            return this;
        }
        /// <summary>
        /// Appends a <c>BETWEEN</c> condition to the SQL statement, 
        /// restricting results to values within the specified range.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the table whose column is being used in the condition.
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="Column">
        /// The enum member representing the column from <typeparamref name="J"/> 
        /// to be evaluated in the <c>BETWEEN</c> condition.
        /// </param>
        /// <param name="Left">
        /// The lower bound value of the range.
        /// </param>
        /// <param name="Right">
        /// The upper bound value of the range.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a fully qualified <c>BETWEEN</c> condition in the form 
        /// <c>[TableName].[Column] BETWEEN [LeftValue] AND [RightValue]</c>. 
        /// It is intended for use after an initial <c>WHERE</c> clause or logical operator 
        /// to build compound range conditions.
        /// </remarks>
        public WhereClause<TCommand, T> Between<J>(J Column, object Left, object Right) where J: Enum
        {
            _cmd.Append($" {typeof(J).Name}.{Column.ToString()} BETWEEN {Left.ToString()} AND {Right.ToString()}");
            return this;
        }
        #endregion

        #region EXISTS
        /// <summary>
        /// Appends a <c>WHERE EXISTS</c> clause to the SQL command, using the provided 
        /// subquery to test for the existence of rows.
        /// </summary>
        /// <param name="TestQuery">
        /// A <see cref="SelectCommand{T}"/> representing the subquery whose result set 
        /// will be evaluated for existence.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>WHERE EXISTS (subquery)</c> expression to the SQL command. 
        /// The <c>EXISTS</c> operator returns <c>TRUE</c> if the subquery produces one or more rows, 
        /// and <c>FALSE</c> otherwise. It is commonly used to test for related data in 
        /// correlated subqueries.
        /// <para>
        /// The implementation trims the trailing character from the subquery’s string 
        /// representation before embedding it, ensuring syntactically valid SQL.
        /// </para>
        /// </remarks>
        public WhereClause<TCommand, T> WhereExists(SelectCommand<T> TestQuery)
        {
            string tQry = TestQuery.ToString();
            tQry = tQry.Substring(0, tQry.Length - 1);

            _cmd.Append($" WHERE EXISTS ({tQry})");
            return this;
        }
        /// <summary>
        /// Appends a <c>WHERE EXISTS</c> clause to the SQL command, using the provided 
        /// subquery to test for the existence of rows in a joined table.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the joined table used in the subquery. 
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="TestQuery">
        /// A <see cref="SelectCommand{J}"/> representing the subquery whose result set 
        /// will be evaluated for existence.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>WHERE EXISTS (subquery)</c> expression to the SQL command. 
        /// The <c>EXISTS</c> operator returns <c>TRUE</c> if the subquery produces one or more rows, 
        /// and <c>FALSE</c> otherwise. It is commonly used to test for related data in 
        /// correlated subqueries.
        /// <para>
        /// The implementation trims the trailing character from the subquery’s string 
        /// representation before embedding it, ensuring syntactically valid SQL. 
        /// To avoid accidental truncation, consider explicitly checking for a trailing 
        /// semicolon before removing it.
        /// </para>
        /// </remarks>
        public WhereClause<TCommand, T> WhereExists<J>(SelectCommand<J> TestQuery) where J: Enum
        {
            string tQry = TestQuery.ToString();
            tQry = tQry.Substring(0, tQry.Length - 1);

            _cmd.Append($" WHERE EXISTS ({tQry})");
            return this;
        }

        /// <summary>
        /// Appends an <c>EXISTS</c> clause to the SQL command, using the provided 
        /// subquery to test for the existence of rows.
        /// </summary>
        /// <param name="TestQuery">
        /// A <see cref="SelectCommand{T}"/> representing the subquery whose result set 
        /// will be evaluated for existence.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends an <c>EXISTS (subquery)</c> expression to the SQL command. 
        /// The <c>EXISTS</c> operator returns <c>TRUE</c> if the subquery produces one or more rows, 
        /// and <c>FALSE</c> otherwise. It is commonly used to test for related data in 
        /// correlated subqueries.
        /// <para>
        /// The implementation trims the trailing character from the subquery’s string 
        /// representation before embedding it, ensuring syntactically valid SQL. 
        /// To avoid accidental truncation, consider explicitly checking for a trailing 
        /// semicolon before removing it.
        /// </para>
        /// </remarks>
        public WhereClause<TCommand, T> Exists(SelectCommand<T> TestQuery)
        {
            string tQry = TestQuery.ToString();
            tQry = tQry.Substring(0, tQry.Length - 1);

            _cmd.Append($" EXISTS ({tQry})");
            return this;
        }
        /// <summary>
        /// Appends an <c>EXISTS</c> clause to the SQL command, using the provided 
        /// subquery to test for the existence of rows in a joined table.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the joined table used in the subquery. 
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="TestQuery">
        /// A <see cref="SelectCommand{J}"/> representing the subquery whose result set 
        /// will be evaluated for existence.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends an <c>EXISTS (subquery)</c> expression to the SQL command. 
        /// The <c>EXISTS</c> operator returns <c>TRUE</c> if the subquery produces one or more rows, 
        /// and <c>FALSE</c> otherwise. It is commonly used to test for related data in 
        /// correlated subqueries.
        /// <para>
        /// The implementation trims the trailing character from the subquery’s string 
        /// representation before embedding it, ensuring syntactically valid SQL. 
        /// To avoid accidental truncation, consider explicitly checking for a trailing 
        /// semicolon before removing it.
        /// </para>
        /// </remarks>
        public WhereClause<TCommand, T> Exists<J>(SelectCommand<J> TestQuery) where J: Enum
        {
            string tQry = TestQuery.ToString();
            tQry = tQry.Substring(0, tQry.Length - 1);

            _cmd.Append($" EXISTS ({tQry})");
            return this;
        }
        #endregion

        #region ANY
        /// <summary>
        /// Appends a <c>WHERE ... ANY</c> clause to the SQL command, comparing the specified 
        /// column against the results of a subquery using the given operator.
        /// </summary>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="T"/> representing the column 
        /// to be compared against the subquery results.
        /// </param>
        /// <param name="Operator">
        /// The <see cref="SQLOperator"/> that defines the comparison operation 
        /// (e.g., <c>=</c>, <c>&gt;</c>, <c>&lt;</c>, <c>&gt;=</c>, <c>&lt;=</c>).
        /// </param>
        /// <param name="Subquery">
        /// A <see cref="SelectCommand{T}"/> representing the subquery whose result set 
        /// will be evaluated with the <c>ANY</c> operator.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>WHERE [TableName].[Column] Operator ANY (subquery)</c> 
        /// expression to the SQL command. The <c>ANY</c> operator compares the column value 
        /// against each value returned by the subquery and evaluates to <c>TRUE</c> if the 
        /// condition holds for at least one of them.
        /// </remarks>
        public WhereClause<TCommand, T> WhereAny(T Column, SQLOperator Operator, SelectCommand<T> Subquery)
        {
            string sQry = Subquery.ToString();
            sQry = sQry.Substring(0, sQry.Length - 1);

            _cmd.Append($" WHERE {typeof(T).Name}.{Column.ToString()} {Operator.ToSymbol()} ANY ({sQry})");
            return this;
        }
        /// <summary>
        /// Appends a <c>WHERE ... ANY</c> clause to the SQL command, comparing the specified 
        /// column from a joined table against the results of a subquery using the given operator.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the joined table whose column is being compared. 
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="J"/> representing the column 
        /// to be compared against the subquery results.
        /// </param>
        /// <param name="Operator">
        /// The <see cref="SQLOperator"/> that defines the comparison operation 
        /// (e.g., <c>=</c>, <c>&gt;</c>, <c>&lt;</c>, <c>&gt;=</c>, <c>&lt;=</c>).
        /// </param>
        /// <param name="Subquery">
        /// A <see cref="SelectCommand{J}"/> representing the subquery whose result set 
        /// will be evaluated with the <c>ANY</c> operator.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>WHERE [JoinedTable].[Column] Operator ANY (subquery)</c> 
        /// expression to the SQL command. The <c>ANY</c> operator compares the column value 
        /// against each value returned by the subquery and evaluates to <c>TRUE</c> if the 
        /// condition holds for at least one of them.
        /// </remarks>
        public WhereClause<TCommand, T> WhereAny<J>(J Column, SQLOperator Operator, SelectCommand<J> Subquery) where J: Enum
        {
            string sQry = Subquery.ToString();
            sQry = sQry.Substring(0, sQry.Length - 1);

            _cmd.Append($" WHERE {typeof(J).Name}.{Column.ToString()} {Operator.ToSymbol()} ANY ({sQry})");
            return this;
        }

        /// <summary>
        /// Appends an <c>ANY</c> comparison expression to the SQL command, 
        /// comparing the specified column against the results of a subquery.
        /// </summary>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="T"/> representing the column 
        /// to be compared against the subquery results.
        /// </param>
        /// <param name="Operator">
        /// The <see cref="SQLOperator"/> that defines the comparison operation 
        /// (e.g., <c>=</c>, <c>&gt;</c>, <c>&lt;</c>, <c>&gt;=</c>, <c>&lt;=</c>).
        /// </param>
        /// <param name="Subquery">
        /// A <see cref="SelectCommand{T}"/> representing the subquery whose result set 
        /// will be evaluated with the <c>ANY</c> operator.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>[TableName].[Column] Operator ANY (subquery)</c> 
        /// expression to the SQL command. The <c>ANY</c> operator compares the column value 
        /// against each value returned by the subquery and evaluates to <c>TRUE</c> if the 
        /// condition holds for at least one of them.
        /// </remarks>
        public WhereClause<TCommand, T> Any(T Column, SQLOperator Operator, SelectCommand<T> Subquery)
        {
            string sQry = Subquery.ToString();
            sQry = sQry.Substring(0, sQry.Length - 1);

            _cmd.Append($" {typeof(T).Name}.{Column.ToString()} {Operator.ToSymbol()} ANY ({sQry})");
            return this;
        }
        /// <summary>
        /// Appends an <c>ANY</c> comparison expression to the SQL command, 
        /// comparing the specified column from a joined table against the results of a subquery.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the joined table whose column is being compared. 
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="J"/> representing the column 
        /// to be compared against the subquery results.
        /// </param>
        /// <param name="Operator">
        /// The <see cref="SQLOperator"/> that defines the comparison operation 
        /// (e.g., <c>=</c>, <c>&gt;</c>, <c>&lt;</c>, <c>&gt;=</c>, <c>&lt;=</c>).
        /// </param>
        /// <param name="Subquery">
        /// A <see cref="SelectCommand{J}"/> representing the subquery whose result set 
        /// will be evaluated with the <c>ANY</c> operator.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>[JoinedTable].[Column] Operator ANY (subquery)</c> 
        /// expression to the SQL command. The <c>ANY</c> operator compares the column value 
        /// against each value returned by the subquery and evaluates to <c>TRUE</c> if the 
        /// condition holds for at least one of them.
        /// </remarks>
        public WhereClause<TCommand, T> Any<J>(J Column, SQLOperator Operator, SelectCommand<J> Subquery) where J : Enum
        {
            string sQry = Subquery.ToString();
            sQry = sQry.Substring(0, sQry.Length - 1);

            _cmd.Append($" {typeof(J).Name}.{Column.ToString()} {Operator.ToSymbol()} ANY ({sQry})");
            return this;
        }
        #endregion

        #region ALL
        /// <summary>
        /// Appends a <c>WHERE ... ALL</c> clause to the SQL command, comparing the specified 
        /// column against the results of a subquery using the given operator.
        /// </summary>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="T"/> representing the column 
        /// to be compared against the subquery results.
        /// </param>
        /// <param name="Operator">
        /// The <see cref="SQLOperator"/> that defines the comparison operation 
        /// (e.g., <c>=</c>, <c>&gt;</c>, <c>&lt;</c>, <c>&gt;=</c>, <c>&lt;=</c>).
        /// </param>
        /// <param name="Subquery">
        /// A <see cref="SelectCommand{T}"/> representing the subquery whose result set 
        /// will be evaluated with the <c>ALL</c> operator.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>WHERE [TableName].[Column] Operator ALL (subquery)</c> 
        /// expression to the SQL command. The <c>ALL</c> operator compares the column value 
        /// against every value returned by the subquery and evaluates to <c>TRUE</c> only if 
        /// the condition holds for all of them.
        /// </remarks>
        public WhereClause<TCommand, T> WhereAll(T Column, SQLOperator Operator, SelectCommand<T> Subquery)
        {
            string sQry = Subquery.ToString();
            sQry = sQry.Substring(0, sQry.Length - 1);

            _cmd.Append($" WHERE {typeof(T).Name}.{Column.ToString()} {Operator.ToSymbol()} ALL ({sQry})");
            return this;
        }
        /// <summary>
        /// Appends a <c>WHERE ... ALL</c> clause to the SQL command, comparing the specified 
        /// column from a joined table against the results of a subquery using the given operator.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the joined table whose column is being compared. 
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="J"/> representing the column 
        /// to be compared against the subquery results.
        /// </param>
        /// <param name="Operator">
        /// The <see cref="SQLOperator"/> that defines the comparison operation 
        /// (e.g., <c>=</c>, <c>&gt;</c>, <c>&lt;</c>, <c>&gt;=</c>, <c>&lt;=</c>).
        /// </param>
        /// <param name="Subquery">
        /// A <see cref="SelectCommand{J}"/> representing the subquery whose result set 
        /// will be evaluated with the <c>ALL</c> operator.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>WHERE [JoinedTable].[Column] Operator ALL (subquery)</c> 
        /// expression to the SQL command. The <c>ALL</c> operator compares the column value 
        /// against every value returned by the subquery and evaluates to <c>TRUE</c> only if 
        /// the condition holds for all of them.
        /// </remarks>
        public WhereClause<TCommand, T> WhereAll<J>(J Column, SQLOperator Operator, SelectCommand<J> Subquery) where J : Enum
        {
            string sQry = Subquery.ToString();
            sQry = sQry.Substring(0, sQry.Length - 1);

            _cmd.Append($" WHERE {typeof(J).Name}.{Column.ToString()} {Operator.ToSymbol()} ALL ({sQry})");
            return this;
        }

        /// <summary>
        /// Appends an <c>ALL</c> comparison expression to the SQL command, 
        /// comparing the specified column against the results of a subquery.
        /// </summary>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="T"/> representing the column 
        /// to be compared against the subquery results.
        /// </param>
        /// <param name="Operator">
        /// The <see cref="SQLOperator"/> that defines the comparison operation 
        /// (e.g., <c>=</c>, <c>&gt;</c>, <c>&lt;</c>, <c>&gt;=</c>, <c>&lt;=</c>).
        /// </param>
        /// <param name="Subquery">
        /// A <see cref="SelectCommand{T}"/> representing the subquery whose result set 
        /// will be evaluated with the <c>ALL</c> operator.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>[TableName].[Column] Operator ALL (subquery)</c> 
        /// expression to the SQL command. The <c>ALL</c> operator compares the column value 
        /// against every value returned by the subquery and evaluates to <c>TRUE</c> only if 
        /// the condition holds for all of them.
        /// </remarks>
        public WhereClause<TCommand, T> All(T Column, SQLOperator Operator, SelectCommand<T> Subquery)
        {
            string sQry = Subquery.ToString();
            sQry = sQry.Substring(0, sQry.Length - 1);

            _cmd.Append($" {typeof(T).Name}.{Column.ToString()} {Operator.ToSymbol()} ALL ({sQry})");
            return this;
        }
        /// <summary>
        /// Appends an <c>ALL</c> comparison expression to the SQL command, 
        /// comparing the specified column from a joined table against the results of a subquery.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the joined table whose column is being compared. 
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="J"/> representing the column 
        /// to be compared against the subquery results.
        /// </param>
        /// <param name="Operator">
        /// The <see cref="SQLOperator"/> that defines the comparison operation 
        /// (e.g., <c>=</c>, <c>&gt;</c>, <c>&lt;</c>, <c>&gt;=</c>, <c>&lt;=</c>).
        /// </param>
        /// <param name="Subquery">
        /// A <see cref="SelectCommand{J}"/> representing the subquery whose result set 
        /// will be evaluated with the <c>ALL</c> operator.
        /// </param>
        /// <returns>
        /// The current <see cref="WhereClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>[JoinedTable].[Column] Operator ALL (subquery)</c> 
        /// expression to the SQL command. The <c>ALL</c> operator compares the column value 
        /// against every value returned by the subquery and evaluates to <c>TRUE</c> only if 
        /// the condition holds for all of them.
        /// </remarks>
        public WhereClause<TCommand, T> All<J>(J Column, SQLOperator Operator, SelectCommand<J> Subquery) where J : Enum
        {
            string sQry = Subquery.ToString();
            sQry = sQry.Substring(0, sQry.Length - 1);

            _cmd.Append($" {typeof(J).Name}.{Column.ToString()} {Operator.ToSymbol()} ALL ({sQry})");
            return this;
        }
        #endregion
    }
}

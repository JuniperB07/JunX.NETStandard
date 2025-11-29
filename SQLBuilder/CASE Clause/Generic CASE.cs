using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Provides a fluent interface for composing SQL <c>CASE</c> clauses, including logical operators and grouping, for use within generic SQL builder commands.
    /// </summary>
    /// <typeparam name="TCommand">The parent SQL builder type that this clause is attached to, enabling fluent chaining back to the command.</typeparam>
    /// <typeparam name="T">An enum type representing the schema or column identifiers used in the conditional expressions.</typeparam>
    public class CaseClause<TCommand, T>
        where TCommand: class
        where T: Enum
    {
        private readonly TCommand _parent;
        private readonly StringBuilder _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaseClause{TCommand, T}"/> class, 
        /// binding it to the parent command and the shared SQL string builder, and 
        /// appending the <c>CASE</c> keyword to begin a conditional expression.
        /// </summary>
        /// <param name="Parent">
        /// The parent command object of type <typeparamref name="TCommand"/> that owns 
        /// this <c>CASE</c> clause. This allows fluent chaining back to the parent query builder.
        /// </param>
        /// <param name="CMD">
        /// The <see cref="StringBuilder"/> instance used to construct the SQL command text. 
        /// The <c>CASE</c> clause will append its expressions to this builder.
        /// </param>
        /// <remarks>
        /// This constructor wires the <c>CASE</c> clause into the parent query builder, 
        /// enabling fluent SQL construction. By appending the <c>CASE</c> keyword immediately, 
        /// the clause is ready to accept <c>WHEN</c>, <c>THEN</c>, <c>ELSE</c>, and 
        /// <c>END</c> segments.
        /// <para>
        /// Example usage:
        /// <code>
        /// SELECT 
        ///   CASE 
        ///     WHEN Status = 'Active' THEN 1 
        ///     ELSE 0 
        ///   END AS IsActive
        /// FROM Users;
        /// </code>
        /// </para>
        /// </remarks>
        public CaseClause(TCommand Parent, StringBuilder CMD)
        {
            _parent = Parent;
            _cmd = CMD;

            _cmd.Append(" CASE");
        }
        /// <summary>
        /// Appends the <c>END</c> keyword to the SQL command, marking the termination 
        /// of a <c>CASE</c> expression.
        /// </summary>
        /// <returns>
        /// The parent command of type <typeparamref name="TCommand"/>, enabling fluent 
        /// chaining back to the main query builder after completing the <c>CASE</c> clause.
        /// </returns>
        /// <remarks>
        /// This property finalizes a <c>CASE</c> expression by appending <c>END</c> to the 
        /// SQL command. It returns the parent command so that query construction can continue 
        /// seamlessly.
        /// <para>
        /// Example usage:
        /// <code>
        /// SELECT 
        ///   CASE 
        ///     WHEN Status = 'Active' THEN 1 
        ///     ELSE 0 
        ///   END AS IsActive
        /// FROM Users;
        /// </code>
        /// </para>
        /// </remarks>
        public TCommand EndCase
        {
            get
            {
                _cmd.Append(" END");
                return _parent;
            }
        }

        #region WHEN-THEN
        /// <summary>
        /// Appends a <c>WHEN ... THEN ...</c> segment to the current <c>CASE</c> expression, 
        /// defining a conditional branch based on the specified column comparison.
        /// </summary>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="T"/> representing the column 
        /// to be evaluated in the condition.
        /// </param>
        /// <param name="Operator">
        /// The <see cref="SQLOperator"/> that defines the comparison operation 
        /// (e.g., <c>=</c>, <c>&gt;</c>, <c>&lt;</c>, <c>&gt;=</c>, <c>&lt;=</c>).
        /// </param>
        /// <param name="Right">
        /// The right‑hand side value or expression to compare against the column.
        /// </param>
        /// <param name="Result">
        /// The value or expression returned when the condition evaluates to <c>TRUE</c>. 
        /// This may be numeric or non‑numeric depending on <paramref name="ResultDataType"/>.
        /// </param>
        /// <param name="ResultDataType">
        /// A <see cref="DataTypes"/> enum indicating whether the <paramref name="Result"/> 
        /// should be treated as numeric or non‑numeric. Non‑numeric results are wrapped in quotes.
        /// </param>
        /// <returns>
        /// The current <see cref="CaseClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional <c>WHEN</c>, <c>ELSE</c>, or <c>END</c> segments.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>WHEN [TableName].[Column] Operator Right THEN Result</c> 
        /// expression to the SQL command. If <paramref name="ResultDataType"/> is 
        /// <see cref="DataTypes.NonNumeric"/>, the result is wrapped in single quotes to 
        /// ensure valid SQL syntax.
        /// <para>
        /// Example usage:
        /// <code>
        /// CASE 
        ///   WHEN Users.Status = 'Active' THEN '1' 
        ///   WHEN Users.Age &gt; 18 THEN 1 
        ///   ELSE 0 
        /// END
        /// </code>
        /// </para>
        /// </remarks>
        public CaseClause<TCommand, T> When(T Column, SQLOperator Operator, object Right, object Result, DataTypes ResultDataType)
        {
            if (ResultDataType == DataTypes.NonNumeric)
                _cmd.Append($" WHEN {typeof(T).Name}.{Column.ToString()} " +
                    $"{Operator.ToSymbol()} " +
                    $"{Right.ToString()} " +
                    $"THEN '{Result.ToString()}'");
            else
                _cmd.Append($" WHEN {typeof(T).Name}.{Column.ToString()} " +
                    $"{Operator.ToSymbol()} " +
                    $"{Right.ToString()} " +
                    $"THEN {Result.ToString()}");
            return this;
        }
        /// <summary>
        /// Appends a <c>WHEN ... THEN ...</c> segment to the current <c>CASE</c> expression, 
        /// defining a conditional branch based on the specified column comparison from a joined table.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the joined table whose column is being compared. 
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="J"/> representing the column 
        /// to be evaluated in the condition.
        /// </param>
        /// <param name="Operator">
        /// The <see cref="SQLOperator"/> that defines the comparison operation 
        /// (e.g., <c>=</c>, <c>&gt;</c>, <c>&lt;</c>, <c>&gt;=</c>, <c>&lt;=</c>).
        /// </param>
        /// <param name="Right">
        /// The right‑hand side value or expression to compare against the column.
        /// </param>
        /// <param name="Result">
        /// The value or expression returned when the condition evaluates to <c>TRUE</c>. 
        /// This may be numeric or non‑numeric depending on <paramref name="ResultDataType"/>.
        /// </param>
        /// <param name="ResultDataType">
        /// A <see cref="DataTypes"/> enum indicating whether the <paramref name="Result"/> 
        /// should be treated as numeric or non‑numeric. Non‑numeric results are wrapped in quotes.
        /// </param>
        /// <returns>
        /// The current <see cref="CaseClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional <c>WHEN</c>, <c>ELSE</c>, or <c>END</c> segments.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>WHEN [JoinedTable].[Column] Operator Right THEN Result</c> 
        /// expression to the SQL command. If <paramref name="ResultDataType"/> is 
        /// <see cref="DataTypes.NonNumeric"/>, the result is wrapped in single quotes to 
        /// ensure valid SQL syntax.
        /// <para>
        /// Example usage:
        /// <code>
        /// CASE 
        ///   WHEN Orders.Status = 'Pending' THEN 'P' 
        ///   WHEN Orders.Quantity &gt; 10 THEN 1 
        ///   ELSE 0 
        /// END
        /// </code>
        /// </para>
        /// </remarks>
        public CaseClause<TCommand, T> When<J>(J Column, SQLOperator Operator, object Right, object Result, DataTypes ResultDataType)
            where J: Enum
        {
            if (ResultDataType == DataTypes.NonNumeric)
                _cmd.Append($" WHEN {typeof(J).Name}.{Column.ToString()} " +
                    $"{Operator.ToSymbol()} " +
                    $"{Right.ToString()} " +
                    $"THEN '{Result.ToString()}'");
            else
                _cmd.Append($" WHEN {typeof(J).Name}.{Column.ToString()} " +
                    $"{Operator.ToSymbol()} " +
                    $"{Right.ToString()} " +
                    $"THEN {Result.ToString()}");
            return this;
        }

        /// <summary>
        /// Appends a <c>WHEN ... IS NOT NULL THEN ...</c> segment to the current <c>CASE</c> 
        /// expression, defining a conditional branch that evaluates whether the specified 
        /// column contains a non‑null value.
        /// </summary>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="T"/> representing the column 
        /// to be checked for non‑null values.
        /// </param>
        /// <param name="Result">
        /// The value or expression returned when the column is not null. 
        /// This may be numeric or non‑numeric depending on <paramref name="ResultDataType"/>.
        /// </param>
        /// <param name="ResultDataType">
        /// A <see cref="DataTypes"/> enum indicating whether the <paramref name="Result"/> 
        /// should be treated as numeric or non‑numeric. Non‑numeric results are wrapped in quotes.
        /// </param>
        /// <returns>
        /// The current <see cref="CaseClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional <c>WHEN</c>, <c>ELSE</c>, or <c>END</c> segments.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>WHEN [TableName].[Column] IS NOT NULL THEN Result</c> 
        /// expression to the SQL command. If <paramref name="ResultDataType"/> is 
        /// <see cref="DataTypes.NonNumeric"/>, the result is wrapped in single quotes to 
        /// ensure valid SQL syntax.
        /// <para>
        /// Example usage:
        /// <code>
        /// CASE 
        ///   WHEN Users.Email IS NOT NULL THEN 'HasEmail' 
        ///   ELSE 'NoEmail' 
        /// END
        /// </code>
        /// </para>
        /// </remarks>
        public CaseClause<TCommand, T> WhenNotNull(T Column, object Result, DataTypes ResultDataType)
        {
            if (ResultDataType == DataTypes.NonNumeric)
                _cmd.Append($" WHEN {typeof(T).Name}.{Column.ToString()} IS NOT NULL THEN '{Result.ToString()}'");
            else
                _cmd.Append($" WHEN {typeof(T).Name}.{Column.ToString()} IS NOT NULL THEN {Result.ToString()}");
            return this;
        }
        /// <summary>
        /// Appends a <c>WHEN ... IS NOT NULL THEN ...</c> segment to the current <c>CASE</c> 
        /// expression, defining a conditional branch that evaluates whether the specified 
        /// column from a joined table contains a non‑null value.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the joined table whose column is being checked. 
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="J"/> representing the column 
        /// to be checked for non‑null values.
        /// </param>
        /// <param name="Result">
        /// The value or expression returned when the column is not null. 
        /// This may be numeric or non‑numeric depending on <paramref name="ResultDataType"/>.
        /// </param>
        /// <param name="ResultDataType">
        /// A <see cref="DataTypes"/> enum indicating whether the <paramref name="Result"/> 
        /// should be treated as numeric or non‑numeric. Non‑numeric results are wrapped in quotes.
        /// </param>
        /// <returns>
        /// The current <see cref="CaseClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional <c>WHEN</c>, <c>ELSE</c>, or <c>END</c> segments.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>WHEN [JoinedTable].[Column] IS NOT NULL THEN Result</c> 
        /// expression to the SQL command. If <paramref name="ResultDataType"/> is 
        /// <see cref="DataTypes.NonNumeric"/>, the result is wrapped in single quotes to 
        /// ensure valid SQL syntax.
        /// <para>
        /// Example usage:
        /// <code>
        /// CASE 
        ///   WHEN Orders.Comments IS NOT NULL THEN 'HasNotes' 
        ///   ELSE 'NoNotes' 
        /// END
        /// </code>
        /// </para>
        /// </remarks>
        public CaseClause<TCommand, T> WhenNotNull<J>(J Column, object Result, DataTypes ResultDataType)
            where J: Enum
        {
            if (ResultDataType == DataTypes.NonNumeric)
                _cmd.Append($" WHEN {typeof(J).Name}.{Column.ToString()} IS NOT NULL THEN '{Result.ToString()}'");
            else
                _cmd.Append($" WHEN {typeof(J).Name}.{Column.ToString()} IS NOT NULL THEN {Result.ToString()}");
            return this;
        }

        /// <summary>
        /// Appends a <c>WHEN ... IS NULL THEN ...</c> segment to the current <c>CASE</c> 
        /// expression, defining a conditional branch that evaluates whether the specified 
        /// column contains a null value.
        /// </summary>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="T"/> representing the column 
        /// to be checked for null values.
        /// </param>
        /// <param name="Result">
        /// The value or expression returned when the column is null. 
        /// This may be numeric or non‑numeric depending on <paramref name="ResultDataType"/>.
        /// </param>
        /// <param name="ResultDataType">
        /// A <see cref="DataTypes"/> enum indicating whether the <paramref name="Result"/> 
        /// should be treated as numeric or non‑numeric. Non‑numeric results are wrapped in quotes.
        /// </param>
        /// <returns>
        /// The current <see cref="CaseClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional <c>WHEN</c>, <c>ELSE</c>, or <c>END</c> segments.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>WHEN [TableName].[Column] IS NULL THEN Result</c> 
        /// expression to the SQL command. If <paramref name="ResultDataType"/> is 
        /// <see cref="DataTypes.NonNumeric"/>, the result is wrapped in single quotes to 
        /// ensure valid SQL syntax.
        /// <para>
        /// Example usage:
        /// <code>
        /// CASE 
        ///   WHEN Users.Email IS NULL THEN 'NoEmail' 
        ///   ELSE 'HasEmail' 
        /// END
        /// </code>
        /// </para>
        /// </remarks>
        public CaseClause<TCommand, T> WhenNull(T Column, object Result, DataTypes ResultDataType)
        {
            if (ResultDataType == DataTypes.NonNumeric)
                _cmd.Append($" WHEN {typeof(T).Name}.{Column.ToString()} IS NULL THEN '{Result.ToString()}'");
            else
                _cmd.Append($" WHEN {typeof(T).Name}.{Column.ToString()} IS NULL THEN {Result.ToString()}");
            return this;
        }
        /// <summary>
        /// Appends a <c>WHEN ... IS NULL THEN ...</c> segment to the current <c>CASE</c> 
        /// expression, defining a conditional branch that evaluates whether the specified 
        /// column from a joined table contains a null value.
        /// </summary>
        /// <typeparam name="J">
        /// The enum type representing the joined table whose column is being checked. 
        /// Each enum member corresponds to a column in that table.
        /// </typeparam>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="J"/> representing the column 
        /// to be checked for null values.
        /// </param>
        /// <param name="Result">
        /// The value or expression returned when the column is null. 
        /// This may be numeric or non‑numeric depending on <paramref name="ResultDataType"/>.
        /// </param>
        /// <param name="ResultDataType">
        /// A <see cref="DataTypes"/> enum indicating whether the <paramref name="Result"/> 
        /// should be treated as numeric or non‑numeric. Non‑numeric results are wrapped in quotes.
        /// </param>
        /// <returns>
        /// The current <see cref="CaseClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional <c>WHEN</c>, <c>ELSE</c>, or <c>END</c> segments.
        /// </returns>
        /// <remarks>
        /// This method appends a <c>WHEN [JoinedTable].[Column] IS NULL THEN Result</c> 
        /// expression to the SQL command. If <paramref name="ResultDataType"/> is 
        /// <see cref="DataTypes.NonNumeric"/>, the result is wrapped in single quotes to 
        /// ensure valid SQL syntax.
        /// <para>
        /// Example usage:
        /// <code>
        /// CASE 
        ///   WHEN Orders.Comments IS NULL THEN 'NoNotes' 
        ///   ELSE 'HasNotes' 
        /// END
        /// </code>
        /// </para>
        /// </remarks>
        public CaseClause<TCommand, T> WhenNull<J>(J Column, object Result, DataTypes ResultDataType)
            where J : Enum
        {
            if (ResultDataType == DataTypes.NonNumeric)
                _cmd.Append($" WHEN {typeof(J).Name}.{Column.ToString()} IS NULL THEN '{Result.ToString()}'");
            else
                _cmd.Append($" WHEN {typeof(J).Name}.{Column.ToString()} IS NULL THEN {Result.ToString()}");
            return this;
        }
        #endregion

        #region ELSE
        /// <summary>
        /// Appends an <c>ELSE</c> segment to the current <c>CASE</c> expression, 
        /// defining the default branch when none of the preceding <c>WHEN</c> conditions are met.
        /// </summary>
        /// <param name="Result">
        /// The value or expression returned when no <c>WHEN</c> condition evaluates to <c>TRUE</c>. 
        /// This may be numeric or non‑numeric depending on <paramref name="ResultDataType"/>.
        /// </param>
        /// <param name="ResultDataType">
        /// A <see cref="DataTypes"/> enum indicating whether the <paramref name="Result"/> 
        /// should be treated as numeric or non‑numeric. Non‑numeric results should be wrapped in quotes.
        /// </param>
        /// <returns>
        /// The current <see cref="CaseClause{TCommand, T}"/> instance, enabling fluent 
        /// chaining of additional <c>WHEN</c> segments or finalizing with <c>END</c>.
        /// </returns>
        /// <remarks>
        /// This method appends an <c>ELSE Result</c> expression to the SQL command. 
        /// If <paramref name="ResultDataType"/> is <see cref="DataTypes.NonNumeric"/>, 
        /// the result should be wrapped in single quotes to ensure valid SQL syntax.
        /// <para>
        /// Example usage:
        /// <code>
        /// CASE 
        ///   WHEN Users.Status = 'Active' THEN 1 
        ///   WHEN Users.Status = 'Inactive' THEN 0 
        ///   ELSE -1 
        /// END
        /// </code>
        /// </para>
        /// </remarks>
        public CaseClause<TCommand, T> Else(object Result, DataTypes ResultDataType)
        {
            if (ResultDataType == DataTypes.NonNumeric)
                _cmd.Append($" ELSE '{Result.ToString()}'");
            else
                _cmd.Append($" ELSE {Result.ToString()}");
            return this;
        }
        #endregion
    }
}

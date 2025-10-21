using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Specifies logical connectors used to combine conditions in SQL <c>WHERE</c> clauses.
    /// </summary>
    public enum WhereConnectors
    {
        NONE,
        AND,
        OR
    }
    /// <summary>
    /// Specifies SQL join strategies for combining rows from multiple tables based on related columns.
    /// </summary>
    /// <remarks>
    /// These modes define how records from the primary and secondary tables are matched and included in the result set.
    /// Commonly used in query builders to control join semantics.
    /// </remarks>
    public enum JoinModes
    {
        INNER_JOIN,
        RIGHT_JOIN,
        LEFT_JOIN,
        FULL_OUTER_JOIN
    }
    /// <summary>
    /// Specifies the ordering mode to be used in generating MySQL <c>SELECT</c> commands.
    /// </summary>
    public enum OrderByModes
    {
        /// <summary>
        /// Sets the ordering mode to Ascending.
        /// </summary>
        ASC,
        /// <summary>
        /// Sets the ordering mode to Descending
        /// </summary>
        DESC
    }
    /// <summary>
    /// Specifies the classification of a data value as either numeric or non-numeric.
    /// </summary>
    /// <remarks>
    /// This enum is typically used to distinguish between values that support arithmetic operations (e.g., integers, decimals)
    /// and those that represent textual or categorical data (e.g., strings, dates).
    /// Useful for schema validation, dynamic query generation, and type-aware formatting.
    /// </remarks>
    public enum DataTypes
    {
        Numeric,
        NonNumeric
    }



    /// <summary>
    /// Defines symbolic SQL comparison operators for use in <c>WHERE</c> clauses and conditional expressions.
    /// </summary>
    /// <remarks>
    /// These operators represent common relational comparisons such as equality, inequality, and pattern matching.
    /// </remarks>
    public enum SQLOperator
    {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanEqualTo,
        LessThan,
        LessThanEqualTo,
        Like
    }
    /// <summary>
    /// Provides extension methods for the <see cref="SqlOperator"/> enum, enabling symbolic SQL rendering.
    /// </summary>
    /// <remarks>
    /// These methods convert enum members into their corresponding SQL operator symbols for use in query generation.
    /// </remarks>
    public static class SqlOperatorExtensions
    {
        /// <summary>
        /// Converts a <see cref="SqlOperator"/> enum value into its corresponding SQL symbol.
        /// </summary>
        /// <param name="op">
        /// The <see cref="SqlOperator"/> value to convert.
        /// </param>
        /// <returns>
        /// A string representing the SQL symbol, such as <c>=</c>, <c>!=</c>, <c>&gt;</c>, <c>&lt;</c>, or <c>LIKE</c>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the provided <paramref name="op"/> value is not defined in <see cref="SQLOperator"/>.
        /// </exception>
        /// <remarks>
        /// This method enables symbolic rendering of SQL operators for use in query builders and expression generators.
        /// </remarks>
        public static string ToSymbol(this SQLOperator op)
        {
            switch (op)
            {
                case SQLOperator.Equal:
                    return "=";
                case SQLOperator.NotEqual:
                    return "!=";
                case SQLOperator.GreaterThan:
                    return ">";
                case SQLOperator.GreaterThanEqualTo:
                    return ">=";
                case SQLOperator.LessThan:
                    return "<";
                case SQLOperator.LessThanEqualTo:
                    return "<=";
                case SQLOperator.Like:
                    return " LIKE ";
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, "Unsupported SQL operator.");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Provides a fluent, strongly typed builder for composing SQL <c>INSERT INTO</c> statements using enum-defined column metadata.
    /// </summary>
    /// <typeparam name="T">
    /// An enum type representing the target table's column schema. Each enum member corresponds to a column name.
    /// </typeparam>
    /// <remarks>
    /// This class supports dynamic SQL generation for insert operations, including column selection and value formatting.
    /// It tracks internal state to manage parentheses, comma placement, and clause sequencing.
    /// Designed for scenarios where schema is represented via enums, enabling type-safe and metadata-driven query composition.
    /// </remarks>
    public class InsertIntoCommand<T> :
        IInsertable<InsertIntoCommand<T>, T>
        where T : Enum
    {
        StringBuilder cmd;
        bool _hasColumns;
        bool _hasValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertIntoCommand{T}"/> class and begins an SQL <c>INSERT INTO</c> statement.
        /// </summary>
        /// <remarks>
        /// This constructor sets up the internal command buffer with the target table name inferred from the enum type <typeparamref name="T"/>.
        /// It also resets internal flags for column and value tracking, preparing the builder for structured insertion logic.
        /// </remarks>
        public InsertIntoCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("INSERT INTO " + typeof(T).Name);
            _hasColumns = false;
            _hasValues = false;
        }
        /// <summary>
        /// Returns the composed SQL <c>INSERT INTO</c> statement as a string, terminated with a closing parenthesis and semicolon.
        /// </summary>
        /// <returns>
        /// A complete SQL query string representing the current state of the insertion builder.
        /// </returns>
        /// <remarks>
        /// This override finalizes the query for execution or inspection. It assumes that column and value sections have been properly appended.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ");";
        }

        #region Columns
        /// <summary>
        /// Appends a column to the SQL <c>INSERT INTO</c> clause using an enum member representing the column name.
        /// </summary>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="T"/> that identifies the column to insert into.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the column list with an opening parenthesis if it's the first column, and appends commas between subsequent columns.
        /// Use in combination with <c>Values(...)</c> to complete the insertion statement.
        /// </remarks>
        public InsertIntoCommand<T> Column(T Column)
        {
            if (_hasColumns)
                cmd.Append(", ");
            else
            {
                cmd.Append(" (");
                _hasColumns = true;
            }
            cmd.Append(Column.ToString());
            return this;
        }
        /// <summary>
        /// Appends multiple columns to the SQL <c>INSERT INTO</c> clause using enum members representing column names.
        /// </summary>
        /// <param name="Columns">
        /// A sequence of enum members of type <typeparamref name="T"/> that identify the columns to insert into.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the column list with an opening parenthesis if it's the first column, and appends commas between subsequent columns.
        /// Use in combination with <c>Values(...)</c> to complete the insertion statement.
        /// </remarks>
        public InsertIntoCommand<T> Column(IEnumerable<T> Columns)
        {
            foreach (T C in Columns)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(" (");
                    _hasColumns = true;
                }
                cmd.Append(C.ToString());
            }
            return this;
        }
        /// <summary>
        /// Appends one or more column descriptors to the INSERT INTO clause of the SQL command.
        /// </summary>
        /// <param name="Columns">
        /// A variable-length array of column descriptors of type <typeparamref name="T"/> to include in the INSERT INTO clause.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand{T}"/> instance for fluent chaining.
        /// </returns>
        /// <remarks>
        /// Builds a comma-separated column list enclosed in parentheses for the INSERT INTO clause.
        /// </remarks>
        public InsertIntoCommand<T> Column(params T[] Columns)
        {
            if (Columns.Length < 1)
                throw new ArgumentException("Invalid parameter length.");

            foreach (T C in Columns)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(" (");
                    _hasColumns = true;
                }
                cmd.Append(C.ToString());
            }
            return this;
        }
        #endregion

        #region Values
        /// <summary>
        /// Appends a single value to the SQL <c>VALUES</c> clause, formatted according to its declared <see cref="DataType"/>.
        /// </summary>
        /// <param name="Value">
        /// The raw string value to insert into the target column.
        /// </param>
        /// <param name="DataType">
        /// The classification of the value as either <see cref="DataTypes.Numeric"/> or <see cref="DataTypes.NonNumeric"/>, used for safe SQL formatting.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the <c>VALUES</c> clause if it hasn't already started, and appends the SQL-safe representation of the value.
        /// Use in combination with <c>Column(...)</c> to ensure column-value alignment.
        /// </remarks>
        public InsertIntoCommand<T> Values(object Value, DataTypes DataType)
        {
            if (_hasValues)
                cmd.Append(", ");
            else
            {
                cmd.Append(") VALUES (");
                _hasValues = true;
            }
            cmd.Append(Methods.SQLSafeValue(Value.ToString(), DataType));
            return this;
        }
        /// <summary>
        /// Appends multiple values to the SQL <c>VALUES</c> clause, each formatted according to its declared <see cref="DataTypes"/>.
        /// </summary>
        /// <param name="Values">
        /// A sequence of <see cref="ValuesMetadata"/> instances, each containing a raw value and its associated data type for SQL-safe formatting.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the <c>VALUES</c> clause if it hasn't already started, and appends each SQL-safe value with comma separation.
        /// Use in combination with <c>Column(...)</c> to ensure column-value alignment.
        /// </remarks>
        public InsertIntoCommand<T> Values(IEnumerable<ValuesMetadata> Values)
        {
            foreach(ValuesMetadata V in Values)
            {
                if (_hasValues)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(") VALUES (");
                    _hasValues = true;
                }

                cmd.Append(V.Value);
            }
            return this;
        }
        /// <summary>
        /// Appends one or more value descriptors to the VALUES clause of the INSERT INTO command.
        /// </summary>
        /// <param name="Values">
        /// A variable-length array of <see cref="ValuesMetadata"/> objects representing the values to insert.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand{T}"/> instance for fluent chaining.
        /// </returns>
        /// <remarks>
        /// Builds a comma-separated VALUES clause, appending each value in order after the column list.
        /// </remarks>
        public InsertIntoCommand<T> Values(params ValuesMetadata[] Values)
        {
            if (Values.Length < 1)
                throw new ArgumentException("Invalid parameter length.");

            foreach (ValuesMetadata V in Values)
            {
                if (_hasValues)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(") VALUES (");
                    _hasValues = true;
                }

                cmd.Append(V.Value);
            }
            return this;
        }
        #endregion
    }
}

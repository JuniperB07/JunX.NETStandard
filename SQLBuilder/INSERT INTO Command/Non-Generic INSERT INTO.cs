using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Provides a fluent, loosely typed builder for composing SQL <c>INSERT INTO</c> statements using raw string identifiers.
    /// </summary>
    /// <remarks>
    /// This class supports dynamic SQL generation for insert operations, including table targeting, column selection, and value formatting.
    /// It tracks internal state to manage clause sequencing, parentheses, and comma placement.
    /// Designed for scenarios where schema is not enum-bound, enabling flexible and runtime-driven query composition.
    /// </remarks>
    public class InsertIntoCommand
    {
        StringBuilder cmd;
        bool _hasColumns;
        bool _hasValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertIntoCommand"/> class for building an SQL <c>INSERT INTO</c> statement.
        /// </summary>
        /// <remarks>
        /// This constructor sets up the internal command buffer and resets column/value tracking flags.
        /// The target table name must be specified separately before appending columns and values.
        /// </remarks>
        public InsertIntoCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("INSERT INTO ");
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

        #region INSERT INTO REGION
        /// <summary>
        /// Specifies the target table for the SQL <c>INSERT INTO</c> statement.
        /// </summary>
        /// <param name="Table">
        /// The name of the table into which data will be inserted.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends the table name directly to the command buffer. It should be called before defining columns and values.
        /// </remarks>
        public InsertIntoCommand InsertInto(string Table)
        {
            cmd.Append(Table);
            return this;
        }
        #endregion

        #region COLUMN REGION
        /// <summary>
        /// Appends a column name to the SQL <c>INSERT INTO</c> clause.
        /// </summary>
        /// <param name="Column">
        /// The name of the column to insert into.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the column list with an opening parenthesis if it's the first column, and appends commas between subsequent columns.
        /// Use in combination with <c>Values(...)</c> to complete the insertion statement.
        /// </remarks>
        public InsertIntoCommand Column(string Column)
        {
            if (_hasColumns)
                cmd.Append(", ");
            else
            {
                cmd.Append(" (");
                _hasColumns = true;
            }
            cmd.Append(Column);
            return this;
        }
        /// <summary>
        /// Appends multiple column names to the SQL <c>INSERT INTO</c> clause.
        /// </summary>
        /// <param name="Columns">
        /// A sequence of column names to insert into.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the column list with an opening parenthesis if it's the first column, and appends commas between subsequent columns.
        /// Use in combination with <c>Values(...)</c> to complete the insertion statement.
        /// </remarks>
        public InsertIntoCommand Column(IEnumerable<string> Columns)
        {
            foreach (string C in Columns)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(" (");
                    _hasColumns = true;
                }
                cmd.Append(C);
            }
            return this;
        }
        #endregion

        #region VALUES REGION
        /// <summary>
        /// Appends a single SQL-safe value to the <c>VALUES</c> clause of the <c>INSERT INTO</c> statement.
        /// </summary>
        /// <param name="Value">
        /// The raw string value to insert.
        /// </param>
        /// <param name="DataType">
        /// The <see cref="MySQLDataType"/> representing the SQL type of the value (e.g., <c>VARCHAR</c>, <c>INT</c>, <c>DATE</c>).
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the <c>VALUES</c> clause with a closing parenthesis from the column list, followed by <c>VALUES (</c>.
        /// Subsequent calls append comma-separated values. Each value is formatted safely using <c>Construct.SQLSafeValue</c>.
        /// </remarks>
        public InsertIntoCommand Values(string Value, DataTypes DataType)
        {
            if (_hasValues)
                cmd.Append(", ");
            else
            {
                cmd.Append(") VALUES (");
                _hasValues = true;
            }
            cmd.Append(Methods.SQLSafeValue(Value, DataType));
            return this;
        }
        /// <summary>
        /// Appends multiple SQL-safe values to the <c>VALUES</c> clause of the <c>INSERT INTO</c> statement using typed metadata.
        /// </summary>
        /// <param name="Values">
        /// A sequence of <see cref="ValuesMetadata"/> instances, each containing a raw value and its associated SQL data type.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the <c>VALUES</c> clause with a closing parenthesis from the column list, followed by <c>VALUES (</c>.
        /// Each value is appended with comma separation and formatted safely using the metadata's <c>Value</c> property.
        /// </remarks>
        public InsertIntoCommand Values(IEnumerable<ValuesMetadata> Values)
        {
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

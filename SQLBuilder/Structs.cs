using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Represents metadata for aliasing a column in SQL, pairing an enum-defined column with its alias and fully qualified name.
    /// </summary>
    /// <typeparam name="T">
    /// An <see cref="Enum"/> type representing the table whose member defines the column being aliased.
    /// </typeparam>
    /// <remarks>
    /// This struct is used to associate a column with its SQL alias and generate fully qualified references for query composition.
    /// </remarks>
    public struct AliasMetadata<T> where T : System.Enum
    {
        public T Column { get; set; }
        public string Alias { get; set; }
        public string FullyQualified { get { return typeof(T).Name + "." + Column.ToString(); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasMetadata{T}"/> struct with the specified column and alias.
        /// </summary>
        /// <param name="Select">
        /// The enum member representing the column to be aliased.
        /// </param>
        /// <param name="As">
        /// The alias to assign to the column in SQL output.
        /// </param>
        /// <remarks>
        /// This constructor pairs a metadata-defined column with its alias, enabling qualified selection and readable output in SQL queries.
        /// </remarks>
        public AliasMetadata(T Select, string As)
        {
            Column = Select;
            Alias = As;
        }
    }

    /// <summary>
    /// Encapsulates a raw value and its associated data type, providing a SQL-safe representation for query composition.
    /// </summary>
    /// <remarks>
    /// This struct is typically used to wrap literal values with type metadata, enabling safe formatting for SQL statements.
    /// The <see cref="Value"/> property returns a sanitized version of the raw input using <c>Methods.SQLSafeValue</c>, based on its <see cref="DataType"/>.
    /// </remarks>
    public struct ValuesMetadata
    {
        private string RawValue { get; set; }
        private DataTypes DataType { get; set; }

        /// <summary>
        /// Gets the SQL-safe representation of the raw value, formatted according to its declared <see cref="DataType"/>.
        /// </summary>
        /// <value>
        /// A string that is safe for direct inclusion in SQL statements, escaped or quoted as needed.
        /// </value>
        /// <remarks>
        /// This property delegates to <c>Methods.SQLSafeValue</c> to sanitize the raw input based on its type.
        /// Useful for dynamic query generation where values must be safely embedded without risking injection or formatting errors.
        /// </remarks>
        public string Value { get { return Methods.SQLSafeValue(RawValue, DataType); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuesMetadata"/> struct with a raw value and its associated data type.
        /// </summary>
        /// <param name="SetValue">
        /// The raw string value to be wrapped and later formatted for SQL usage.
        /// </param>
        /// <param name="SetDataType">
        /// The classification of the value as either <see cref="DataTypes.Numeric"/> or <see cref="DataTypes.NonNumeric"/>, used for safe SQL formatting.
        /// </param>
        /// <remarks>
        /// This constructor sets the internal fields used by the <see cref="Value"/> property to produce a SQL-safe output.
        /// </remarks>
        public ValuesMetadata(string SetValue, DataTypes SetDataType)
        {
            RawValue = SetValue;
            DataType = SetDataType;
        }
    }

    /// <summary>
    /// Encapsulates metadata for a column-value pair used in SQL <c>UPDATE</c> statements, including type-safe column identification and SQL-safe value formatting.
    /// </summary>
    /// <typeparam name="T">
    /// An enum type representing the column schema of the target table. Each enum member corresponds to a column name.
    /// </typeparam>
    /// <remarks>
    /// This struct wraps a raw value with its associated data type and column reference, enabling safe and structured update logic.
    /// The <see cref="Value"/> property returns a sanitized version of the raw input using <c>Methods.SQLSafeValue</c>, based on its <see cref="DataType"/>.
    /// </remarks>
    public struct UpdateMetadata<T> where T: Enum
    {
        private string RawValue { get; set; }
        private DataTypes DataType { get; set; }

        /// <summary>
        /// Gets or sets the enum member representing the column to be updated in the SQL <c>UPDATE</c> statement.
        /// </summary>
        /// <value>
        /// An enum value of type <typeparamref name="T"/> that identifies the target column for the update operation.
        /// </value>
        /// <remarks>
        /// This property provides type-safe access to the column being updated, ensuring alignment with the schema defined by <typeparamref name="T"/>.
        /// </remarks>
        public T Column { get; set; }
        /// <summary>
        /// Gets the SQL-safe representation of the raw value, formatted according to its declared <see cref="DataType"/>.
        /// </summary>
        /// <value>
        /// A string that is safe for direct inclusion in SQL <c>UPDATE</c> statements, escaped or quoted as needed.
        /// </value>
        /// <remarks>
        /// This property delegates to <c>Methods.SQLSafeValue</c> to sanitize the raw input based on its type,
        /// ensuring protection against injection and preserving correct formatting for numeric and non-numeric values.
        /// </remarks>
        public string Value { get { return Methods.SQLSafeValue(RawValue, DataType); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMetadata{T}"/> struct with a column, raw value, and associated data type.
        /// </summary>
        /// <param name="SetColumn">
        /// The enum member representing the column to be updated.
        /// </param>
        /// <param name="SetValue">
        /// The raw string value to assign to the column.
        /// </param>
        /// <param name="SetDataType">
        /// The classification of the value as either <see cref="DataTypes.Numeric"/> or <see cref="DataTypes.NonNumeric"/>, used for SQL-safe formatting.
        /// </param>
        /// <remarks>
        /// This constructor sets the internal metadata required to produce a SQL-safe update expression via the <see cref="Value"/> property.
        /// </remarks>
        public UpdateMetadata(T SetColumn, string SetValue, DataTypes SetDataType)
        {
            Column = SetColumn;
            DataType = SetDataType;
            RawValue = SetValue;
        }
    }

    /// <summary>
    /// Encapsulates metadata for a column-value pair used in SQL <c>UPDATE</c> statements, including the column name, raw value, and its associated data type.
    /// </summary>
    /// <remarks>
    /// This struct provides a type-aware wrapper for update logic, ensuring that values are formatted safely for SQL execution via the <see cref="Value"/> property.
    /// It is designed for dynamic update scenarios where column names are represented as strings rather than enums.
    /// </remarks>
    public struct UpdateMetadata
    {
        private string RawValue { get; set; }
        private DataTypes DataType { get; set; }

        /// <summary>
        /// Gets or sets the name of the column to be updated in the SQL <c>UPDATE</c> statement.
        /// </summary>
        /// <value>
        /// A string representing the target column name in the database.
        /// </value>
        /// <remarks>
        /// This property provides a flexible, string-based reference to the column, suitable for dynamic or loosely typed update scenarios.
        /// </remarks>
        public string Column { get; set; }
        /// <summary>
        /// Gets the SQL-safe representation of the raw value, formatted according to its declared <see cref="DataType"/>.
        /// </summary>
        /// <value>
        /// A string that is safe for direct inclusion in SQL <c>UPDATE</c> statements, escaped or quoted as needed.
        /// </value>
        /// <remarks>
        /// This property delegates to <c>Methods.SQLSafeValue</c> to sanitize the raw input based on its type,
        /// ensuring protection against injection and preserving correct formatting for numeric and non-numeric values.
        /// </remarks>
        public string Value { get { return Methods.SQLSafeValue(RawValue, DataType); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMetadata"/> struct with a column name, raw value, and associated data type.
        /// </summary>
        /// <param name="SetColumn">
        /// The name of the column to be updated.
        /// </param>
        /// <param name="SetValue">
        /// The raw string value to assign to the column.
        /// </param>
        /// <param name="SetDataType">
        /// The classification of the value as either <see cref="DataTypes.Numeric"/> or <see cref="DataTypes.NonNumeric"/>, used for SQL-safe formatting.
        /// </param>
        /// <remarks>
        /// This constructor sets the internal metadata required to produce a SQL-safe update expression via the <see cref="Value"/> property.
        /// </remarks>
        public UpdateMetadata(string SetColumn, string SetValue, DataTypes SetDataType)
        {
            Column = SetColumn;
            DataType = SetDataType;
            RawValue = SetValue;
        }
    }

    /// <summary>
    /// Represents metadata for a SQL <c>JOIN</c> clause between the primary table <typeparamref name="T"/> 
    /// and a joined table <typeparamref name="J"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The enum type representing the primary table. Each enum member corresponds to a column in that table.
    /// </typeparam>
    /// <typeparam name="J">
    /// The enum type representing the joined table. Each enum member corresponds to a column in that table.
    /// </typeparam>
    /// <remarks>
    /// This struct encapsulates the join mode, the column from the primary table, and the column from the joined table. 
    /// It is intended for use in fluent SQL builder operations to define join conditions.
    /// </remarks>
    public struct JoinMetadata<T, J>
        where T: Enum
        where J: Enum
    {
        /// <summary>
        /// Gets or sets the type of SQL <c>JOIN</c> to apply, specified via <see cref="JoinModes"/>.
        /// </summary>
        /// <value>
        /// A <see cref="JoinModes"/> value that determines how the primary table <typeparamref name="T"/> 
        /// is joined with the secondary table <typeparamref name="J"/> (e.g., <c>INNER JOIN</c>, <c>LEFT JOIN</c>).
        /// </value>
        /// <remarks>
        /// This property defines the join behavior used when constructing the SQL statement.
        /// </remarks>
        public JoinModes JoinMode { get; set; }
        /// <summary>
        /// Gets or sets the column from the primary table <typeparamref name="T"/> 
        /// that participates in the SQL <c>JOIN</c> condition.
        /// </summary>
        /// <value>
        /// An enum member of type <typeparamref name="T"/> representing the column 
        /// on the left side of the join expression.
        /// </value>
        /// <remarks>
        /// This property defines which column from the primary table is used when constructing 
        /// the <c>ON</c> clause of the SQL <c>JOIN</c>.
        /// </remarks>
        public T Left { get; set; }
        /// <summary>
        /// Gets or sets the column from the joined table <typeparamref name="J"/> 
        /// that participates in the SQL <c>JOIN</c> condition.
        /// </summary>
        /// <value>
        /// An enum member of type <typeparamref name="J"/> representing the column 
        /// on the right side of the join expression.
        /// </value>
        /// <remarks>
        /// This property defines which column from the joined table is used when constructing 
        /// the <c>ON</c> clause of the SQL <c>JOIN</c>.
        /// </remarks>
        public J Right { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinMetadata{T, J}"/> struct 
        /// with the specified join mode and column references.
        /// </summary>
        /// <param name="SetJoinMode">
        /// A <see cref="JoinModes"/> value that defines the type of SQL <c>JOIN</c> to apply 
        /// (e.g., <c>INNER JOIN</c>, <c>LEFT JOIN</c>).
        /// </param>
        /// <param name="SetLeft">
        /// An enum member of type <typeparamref name="T"/> representing the column 
        /// from the primary table used in the join condition.
        /// </param>
        /// <param name="SetRight">
        /// An enum member of type <typeparamref name="J"/> representing the column 
        /// from the joined table used in the join condition.
        /// </param>
        /// <remarks>
        /// This constructor sets the <see cref="JoinMode"/>, <see cref="Left"/>, and <see cref="Right"/> 
        /// properties to define a fully qualified SQL <c>JOIN</c> clause between the primary 
        /// and joined tables.
        /// </remarks>
        public JoinMetadata(JoinModes SetJoinMode, T SetLeft, J SetRight)
        {
            JoinMode = SetJoinMode;
            Left = SetLeft;
            Right = SetRight;
        }
    }
}

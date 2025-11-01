using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.OLEDB
{
    /// <summary>
    /// Represents a parameter-value pair used in OLEDB command construction.
    /// </summary>
    /// <remarks>
    /// The <c>OLEDB_Parameter</c> struct encapsulates a named SQL parameter and its corresponding string value.
    /// It is typically used to bind parameters in OLEDB queries or stored procedure calls, enabling safe and dynamic command execution.
    /// </remarks>
    public struct OLEDB_Parameter
    {
        /// <summary>
        /// Gets or sets the name of the OLEDB parameter to be used in a command or query.
        /// </summary>
        /// <remarks>
        /// This property typically represents a named placeholder in a SQL statement, such as <c>@TenantID</c> or <c>@StartDate</c>.
        /// It should match the parameter name expected by the OLEDB command or stored procedure.
        /// </remarks>
        public string Parameter { get; set; }
        /// <summary>
        /// Gets or sets the string representation of the value to be bound to the OLEDB parameter.
        /// </summary>
        /// <remarks>
        /// This value is typically passed to an OLEDB command or query as the input for the corresponding named parameter.
        /// It should be formatted appropriately for the target database field, such as numeric, date, or text.
        /// </remarks>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OLEDB_Parameter"/> struct with the specified parameter name and value.
        /// </summary>
        /// <param name="SetParameter">The name of the OLEDB parameter, typically prefixed with <c>@</c>.</param>
        /// <param name="SetValue">The string value to bind to the specified parameter.</param>
        /// <remarks>
        /// This constructor is used to define a parameter-value pair for OLEDB command execution, enabling dynamic and safe query construction.
        /// </remarks>
        public OLEDB_Parameter(string SetParameter, string SetValue)
        {


            Parameter = SetParameter;
            Value = SetValue; 
        }
    }
}

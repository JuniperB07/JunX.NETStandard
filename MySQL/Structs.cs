using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.MySQL
{
    /// <summary>
    /// Represents metadata for a SQL parameter, including its name and associated value.
    /// </summary>
    /// <remarks>
    /// This structure is typically used to encapsulate parameter information for dynamic command construction or query execution.
    /// It supports both primitive and complex types via the <see cref="object"/> value container.
    /// </remarks>
    public struct ParametersMetadata
    {
        public string ParameterName { get; set; }
        public object Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParametersMetadata"/> struct with the specified parameter name and value.
        /// </summary>
        /// <param name="SetParameterName">
        /// The name of the SQL parameter to be used in command execution.
        /// </param>
        /// <param name="SetValue">
        /// The value associated with the parameter, which can be of any object type.
        /// </param>
        /// <remarks>
        /// This constructor is typically used to encapsulate parameter metadata for dynamic SQL command construction or parameterized queries.
        /// </remarks>
        public ParametersMetadata(string SetParameterName, object SetValue)
        {
            ParameterName = SetParameterName;
            Value = SetValue;
        }
    }
}

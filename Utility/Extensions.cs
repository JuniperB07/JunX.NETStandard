using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.Utility
{
    /// <summary>
    /// Provides extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Attempts to parse the current string into the specified enumeration type.
        /// </summary>
        /// <typeparam name="T">
        /// The target enumeration type to parse into. Must be a non-nullable <see cref="Enum"/>.
        /// </typeparam>
        /// <param name="value">
        /// The string value to parse into the enumeration.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the parsed enumeration value if successful;
        /// otherwise the default value of <typeparamref name="T"/>.
        /// </param>
        /// <returns>
        /// True if the string was successfully parsed into the specified enumeration type;
        /// otherwise false.
        /// </returns>
        /// <remarks>
        /// This extension method provides a convenient way to call <see cref="Enum.TryParse{TEnum}(string, out TEnum)"/>
        /// directly on a string instance, with case-insensitive parsing enabled.
        /// </remarks>
        public static bool TryParse<T>(this string value, out T result)
            where T: struct, Enum
        {
            return Enum.TryParse(value, true, out result);
        }
    }
}

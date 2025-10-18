using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JunX.NETStandard.Utility
{
    /// <summary>
    /// Provides static utility methods for working with enum types, including name extraction and metadata access.
    /// </summary>
    /// <typeparam name="T">
    /// The enum type to operate on. Must be a valid <see cref="System.Enum"/>.
    /// </typeparam>
    /// <remarks>
    /// This generic helper class enables type-safe reflection over enum members, allowing operations like listing names or values.
    /// It is designed for scenarios where enum metadata needs to be accessed without relying on instance context.
    /// </remarks>
    public static class EnumHelper<T> where T: Enum
    {
        /// <summary>
        /// Returns a list of all member names defined in the enum type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="List{String}"/> containing the names of each enum member in declaration order.
        /// </returns>
        /// <remarks>
        /// This method uses <see cref="Enum.GetNames(Type)"/> to extract the member names of the specified enum type <typeparamref name="T"/>.
        /// It is useful for generating dropdowns, validation lists, or metadata-driven UI elements based on enum definitions.
        /// </remarks>
        public static List<string> ToList()
        {
            return Enum.GetNames(typeof(T)).ToList();
        }

        /// <summary>
        /// Returns the simple name of the enum type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing the type name of the enum, without namespace or qualifiers.
        /// </returns>
        /// <remarks>
        /// This method uses <c>typeof(T).Name</c> to extract the unqualified name of the enum type.
        /// Useful for generating SQL table names, logging, or metadata tagging based on enum identity.
        /// </remarks>
        public static string Name()
        {
            return typeof(T).Name; 
        }

        /// <summary>
        /// Converts a human-readable string into its corresponding enum value of type <typeparamref name="T"/>.
        /// Replaces the specified <paramref name="ReadableValueDelimiter"/> with <paramref name="EnumDelimiter"/> to match the enum's internal format,
        /// then parses the result against the available enum values returned by <c>ToList()</c>.
        /// Throws an exception if no matching enum value is found.
        /// </summary>
        /// <param name="ReadableValue">The input string representing the enum value in a readable format.</param>
        /// <param name="ReadableValueDelimiter">The delimiter used in the readable string format.</param>
        /// <param name="EnumDelimiter">The delimiter used in the actual enum value format.</param>
        /// <returns>The parsed enum value of type <typeparamref name="T"/>.</returns>
        /// <exception cref="Exception">Thrown when the formatted string does not match any known enum value.</exception>
        public static T GetEnumValue(string ReadableValue, char ReadableValueDelimiter, char EnumDelimiter)
        {
            string enumFormat = ReadableValue.Replace(ReadableValueDelimiter, EnumDelimiter);

            foreach(string value in ToList())
                if(value == enumFormat)
                    return (T)Enum.Parse(typeof(T), value);

            throw new Exception("Invalid StringValue parameter.");
        }

        /// <summary>
        /// Converts an enum value of type <typeparamref name="T"/> into a human-readable string by replacing the specified delimiter with spaces.
        /// Useful for displaying enum values in UI or reports with improved clarity.
        /// </summary>
        /// <param name="EnumValue">The enum value to convert.</param>
        /// <param name="EnumValueDelimiter">The delimiter used in the enum's internal format (e.g., underscore or hyphen).</param>
        /// <returns>A human-readable string representation of the enum value.</returns>
        public static string GetReadableValue(T EnumValue, char EnumValueDelimiter)
        {
            return EnumValue.ToString().Replace(EnumValueDelimiter, ' ');
        }
    }
}

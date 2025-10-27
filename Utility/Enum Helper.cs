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

        /// <summary>
        /// Converts a sequence of enum values into a list of human-readable strings by replacing delimiter characters.
        /// Useful for displaying enum values in UI or reports with improved readability.
        /// </summary>
        /// <typeparam name="T">The enum type to process.</typeparam>
        /// <param name="EnumValues">The collection of enum values to convert.</param>
        /// <param name="EnumValueDelimiter">The character used in enum names that should be replaced with a space.</param>
        /// <returns>
        /// A list of strings where each enum value has its delimiters replaced with spaces for readability.
        /// </returns>
        public static List<string> GetReadableValues(char EnumValueDelimiter)
        {
            List<string> rValues = new List<string>();
            List<string> rawVal = ToList();

            foreach (string EV in rawVal)
                rValues.Add(EV.ToString().Replace(EnumValueDelimiter, ' '));

            return rValues;
        }

        /// <summary>
        /// Converts a string value to its corresponding enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="Value">The string representation of the enum value.</param>
        /// <returns>The enum value of type <typeparamref name="T"/> that matches the input string.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input string does not match any defined enum value in <typeparamref name="T"/>.
        /// </exception>
        /// <remarks>
        /// This method iterates through the list of valid enum names returned by <c>ToList()</c> and performs a strict equality check.
        /// If a match is found, it parses and returns the corresponding enum value.
        /// Otherwise, it throws an <see cref="ArgumentException"/> indicating the input is invalid.
        /// Ensure that <c>ToList()</c> returns all valid enum names for type <typeparamref name="T"/>.
        /// </remarks>
        public static T ToEnum(string Value)
        {
            foreach (string enums in ToList())
                if (Value == enums)
                    return (T)Enum.Parse(typeof(T), enums);

            throw new ArgumentException("Invalid parameter.");
        }

        /// <summary>
        /// Attempts to parse a string into an enumeration value of type <typeparamref name="T"/>, with optional support for readable formats and delimiters.
        /// </summary>
        /// <typeparam name="T">The target enumeration type to parse into.</typeparam>
        /// <param name="Value">The input string representing the enumeration value or readable format.</param>
        /// <param name="EnumValue">
        /// When this method returns, contains the parsed enumeration value if successful; otherwise, the default value of <typeparamref name="T"/>.
        /// </param>
        /// <param name="IsReadableValue">
        /// Indicates whether the input string uses a human-readable format (e.g., "Read, Write, Execute") that requires delimiter-based parsing.
        /// </param>
        /// <param name="ReadableValueDelimiter">
        /// The character used to separate readable values (e.g., comma, pipe). Ignored if <paramref name="IsReadableValue"/> is <c>false</c>.
        /// </param>
        /// <param name="EnumDelimiter">
        /// The character used to separate enum values internally. Ignored if <paramref name="IsReadableValue"/> is <c>false</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the input string was successfully parsed into a valid enumeration value; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method supports both raw enum string parsing and human-readable formats with custom delimiters.
        /// Parsing is wrapped in a try-catch block to ensure safe failure handling without throwing exceptions.
        /// If parsing fails, <paramref name="EnumValue"/> is assigned the default value of <typeparamref name="T"/>.
        /// </remarks>
        public static bool TryParse(string Value, out T EnumValue, bool IsReadableValue = false, char ReadableValueDelimiter = '\0', char EnumDelimiter = '\0')
        {
            if (IsReadableValue)
            {
                try
                {
                    EnumValue = GetEnumValue(Value, ReadableValueDelimiter, EnumDelimiter);
                    return true;
                }
                catch
                {
                    EnumValue = default(T);
                    return false;
                }
            }
            else
            {
                try
                {
                    EnumValue = ToEnum(Value);
                    return true;
                }
                catch
                {
                    EnumValue = default(T);
                    return false;
                }
            }
        }
    }
}

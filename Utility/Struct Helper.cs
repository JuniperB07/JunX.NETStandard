using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.Utility
{
    /// <summary>
    /// Provides a centralized set of utility methods for working with value-type instances of <typeparamref name="T"/>.
    /// Designed to support structural comparison, inspection, and future extensibility for common operations on structs.
    /// </summary>
    /// <typeparam name="T">
    /// The value type (struct) to operate on. Must be a non-nullable struct.
    /// </typeparam>
    public static class StructHelper<T> where T: struct
    {
        /// <summary>
        /// Determines whether two instances of <typeparamref name="T"/> are equal by comparing the values of all public properties.
        /// Uses reflection to perform a shallow, property-level comparison.
        /// </summary>
        /// <param name="Left">The first struct instance to compare.</param>
        /// <param name="Right">The second struct instance to compare.</param>
        /// <returns>
        /// <c>true</c> if all public property values are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool AreEqual(T Left, T Right)
        {
            foreach(var prop in typeof(T).GetProperties())
            {
                var leftVal = prop.GetValue(Left);
                var rightVal = prop.GetValue(Right);
                if (leftVal != rightVal)
                    return false;
            }
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQL.DataTypes
{
    /// <summary>
    /// Represents a custom SQL-like VARCHAR type in C#, encapsulating a string value
    /// with an associated maximum length constraint and an optional default fallback string.
    /// </summary>
    /// <remarks>
    /// The <see cref="VarChar"/> struct enforces length restrictions similar to SQL
    /// VARCHAR(n) columns. It stores the underlying string in a helper type and
    /// provides validation, equality comparison, and operator overloads.
    /// </remarks>
    public struct VarChar :
        IValidateable,
        IValueAccessible<string>
    {
        private VarCharBase _base;
        private readonly int _maxLen;
        private readonly string _default;

        /// <summary>
        /// Gets or sets the underlying string value of the <see cref="VarChar"/> instance.
        /// </summary>
        /// <value>
        /// The string content stored within the internal <see cref="VarCharBase"/> wrapper.
        /// </value>
        /// <remarks>
        /// When setting this property, if the provided value is <c>null</c>, empty, or consists
        /// only of whitespace, the internal value is replaced with a default string using the
        /// current <see cref="MaxLength"/> constraint. Otherwise, the provided value is wrapped
        /// in a new <see cref="VarCharBase"/> that enforces the same maximum length.
        /// </remarks>
        public string Value
        {
            get => _base;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _base = new VarCharBase(_default, _maxLen);
                else
                    _base = new VarCharBase(value, _maxLen);
            }
        }
        /// <summary>
        /// Gets the maximum allowed length for the current <see cref="VarChar"/> instance.
        /// </summary>
        /// <value>
        /// An integer representing the length constraint applied to the stored string value.
        /// </value>
        /// <remarks>
        /// This property defines the maximum number of characters permitted in the 
        /// <see cref="Value"/>. It is set during construction and remains constant 
        /// throughout the lifetime of the instance.
        /// </remarks>
        public int MaxLength => _maxLen;
        /// <summary>
        /// Gets the default string value used when the <see cref="Value"/> property
        /// is assigned a null, empty, or whitespace string.
        /// </summary>
        /// <value>
        /// A string representing the fallback value applied internally when invalid
        /// input is provided to <see cref="Value"/>.
        /// </value>
        /// <remarks>
        /// This property ensures that the <see cref="VarChar"/> instance always contains
        /// a valid string, even if the assigned value is null or whitespace.
        /// </remarks>
        public string Default => _default;

        /// <summary>
        /// Initializes a new instance of the <see cref="VarChar"/> struct with the specified
        /// string value, maximum length constraint, and an optional default value.
        /// </summary>
        /// <param name="value">
        /// The string value to be stored within the <see cref="VarChar"/> instance.
        /// </param>
        /// <param name="maxLength">
        /// The maximum number of characters permitted for the stored string.
        /// </param>
        /// <param name="defaultValue">
        /// An optional fallback string used when <paramref name="value"/> is null, empty,
        /// or consists only of whitespace. Defaults to an empty string if not provided.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="maxLength"/> is less than or equal to zero, or when
        /// <paramref name="defaultValue"/> exceeds the specified maximum length.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when both <paramref name="value"/> and <paramref name="defaultValue"/> are
        /// null, empty, or whitespace.
        /// </exception>
        /// <remarks>
        /// This constructor enforces SQL-like semantics for VARCHAR(n) by associating the
        /// provided string with a fixed length constraint. If the initial <paramref name="value"/>
        /// is invalid, the <paramref name="defaultValue"/> is used instead, ensuring that the
        /// instance always contains a valid string within the defined length.
        /// </remarks>
        public VarChar(string value, int maxLength, string defaultValue = "")
        {
            if (maxLength <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            if (defaultValue.Length > maxLength)
                throw new ArgumentOutOfRangeException(nameof(defaultValue));

            _default = defaultValue;
            _maxLen = maxLength;

            if (string.IsNullOrWhiteSpace(value))
            {
                if (string.IsNullOrWhiteSpace(defaultValue))
                    throw new ArgumentNullException(nameof(value));
                else
                    _base = new VarCharBase(defaultValue, maxLength);
            }
            else
                _base = new VarCharBase(value, maxLength);
        }

        /// <summary>
        /// Determines whether the current <see cref="VarChar"/> instance contains a valid value
        /// that respects the defined length constraint.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the underlying string is non-empty and its length is less than or equal
        /// to <see cref="MaxLength"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method verifies both that the stored string is not empty and that it does not
        /// exceed the maximum length specified when the instance was constructed.
        /// </remarks>
        public bool IsValid()
        {
            return _base.Value.Length > 0 && Value.Length <= _maxLen;
        }
        /// <summary>
        /// Determines whether the current <see cref="VarChar"/> instance is equal to another
        /// <see cref="VarChar"/> instance.
        /// </summary>
        /// <param name="other">
        /// The other <see cref="VarChar"/> instance to compare against.
        /// </param>
        /// <returns>
        /// <c>true</c> if both instances contain the same string value and have the same
        /// <see cref="MaxLength"/> constraint; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method performs a value-based comparison that considers both the stored
        /// string and the maximum length constraint. It does not rely on reference equality.
        /// </remarks>
        public bool IsEqual(VarChar other)
        {
            return
                Value == other.Value &&
                Default == other.Default &&
                MaxLength == other.MaxLength;
        }

        /// <summary>
        /// Determines whether two <see cref="VarChar"/> instances are equal.
        /// </summary>
        /// <param name="lhs">
        /// The left-hand side <see cref="VarChar"/> operand.
        /// </param>
        /// <param name="rhs">
        /// The right-hand side <see cref="VarChar"/> operand.
        /// </param>
        /// <returns>
        /// <c>true</c> if both operands are valid and contain the same string value
        /// with the same <see cref="MaxLength"/> constraint; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This operator first verifies that both operands are valid using <see cref="IsValid()"/>.
        /// If valid, it compares them using <see cref="IsEqual(VarChar)"/> to ensure both
        /// the string content and length constraint match.
        /// </remarks>
        public static bool operator ==(VarChar lhs, VarChar rhs)
        {
            if (!lhs.IsValid() || !rhs.IsValid())
                return false;

            return lhs.IsEqual(rhs);
        }
        /// <summary>
        /// Determines whether two <see cref="VarChar"/> instances are not equal.
        /// </summary>
        /// <param name="lhs">
        /// The left-hand side <see cref="VarChar"/> operand.
        /// </param>
        /// <param name="rhs">
        /// The right-hand side <see cref="VarChar"/> operand.
        /// </param>
        /// <returns>
        /// <c>true</c> if both operands are valid and differ either in their string value
        /// or in their <see cref="MaxLength"/> constraint; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This operator first verifies that both operands are valid using <see cref="IsValid()"/>.
        /// If valid, it evaluates equality with the <see cref="=="/> operator and returns the negated result.
        /// </remarks>
        public static bool operator !=(VarChar lhs, VarChar rhs)
        {
            if (!lhs.IsValid() || !rhs.IsValid())
                return false;

            return !(lhs == rhs); ;
        }

        /// <summary>
        /// Returns the string representation of the current <see cref="VarChar"/> instance.
        /// </summary>
        /// <returns>
        /// The underlying string value stored in the internal <see cref="VarCharBase"/>.
        /// </returns>
        /// <remarks>
        /// This override provides a convenient way to obtain the raw string content of the
        /// <see cref="VarChar"/> instance, making it suitable for display, logging, or debugging.
        /// </remarks>
        public override string ToString() => _base;
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    internal struct VarCharBase
    {
        private string _value;

        public string Value => _value;
        public int MaxLength { get; }

        public VarCharBase(string value, int maxLength)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _value = value;
            MaxLength = maxLength;
        }

        public static implicit operator VarCharBase(string value) => new VarCharBase(value, int.MaxValue);
        public static implicit operator string(VarCharBase varchar) => varchar._value;

        public override string ToString() => _value;
    }
}

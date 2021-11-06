using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LccApiNet.Enums
{
    /// <summary>
    /// Enum with type safety
    /// </summary>
    /// <typeparam name="TEntity">Type of the class that represents enum</typeparam>
    public abstract class SafetyEnum<TEntity> : IEqualityComparer<SafetyEnum<TEntity>>, IEquatable<SafetyEnum<TEntity>>
        where TEntity : SafetyEnum<TEntity>, new()
    {
        private string _value = null!;
        private static List<string> _values = new List<string>();

        /// <summary>
        /// Access value of the enum element
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string GetValue(TEntity entity) =>
            entity._value;

        /// <summary>
        /// Tries to assign string value to the enum value
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Enum entity based on the <paramref name="value"/></returns>
        public static TEntity Assign(string value)
        {
            if (_values.Contains(value)) {
                return new TEntity() { _value = value };
            } else {
                throw new ArgumentException("Attempted to assign value that was not registered");
            }
        }

        /// <summary>
        /// Registers value
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Enum type value</returns>
        protected static TEntity RegisterValue(string value)
        {
            if (_values.Contains(value)) {
                throw new ArgumentException("Value is already registered");
            }

            _values.Add(value);
            return new TEntity() { _value = value };
        }

        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="left">Left value</param>
        /// <param name="right">Right value</param>
        /// <returns><see cref="true"/> if the specified objects are equal; otherwise, <see cref="false"/>.</returns>
        public static bool operator ==(SafetyEnum<TEntity>? left, SafetyEnum<TEntity>? right)
        {
            if (ReferenceEquals(objA: right, objB: left)) {
                return true;
            }

            if (left is null || right is null)
                return false;

            return left._value == right._value;
        }

        /// <summary>
        /// Inequality operator
        /// </summary>
        /// <param name="left">Left value</param>
        /// <param name="right">Right value</param>
        /// <returns><see cref="false"/> if the specified objects are equal; otherwise, <see cref="true"/></returns>
        public static bool operator !=(SafetyEnum<TEntity>? left, SafetyEnum<TEntity>? right)
        {
            return !(left == right);
        }
        
        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return this == obj as SafetyEnum<TEntity>;
        }

        /// <inheritdoc />
        public bool Equals(SafetyEnum<TEntity>?  other) {
            return this == other;
        }

        /// <inheritdoc />
        public bool Equals(SafetyEnum<TEntity>? x, SafetyEnum<TEntity>? y) {
            return x == y;
        }

        /// <inheritdoc />
        public int GetHashCode(SafetyEnum<TEntity>? obj)
        {
            return GetHashCode(this);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        /// <inheritdoc />
		public override string ToString()
        {
            return _value;
        }
    }
}

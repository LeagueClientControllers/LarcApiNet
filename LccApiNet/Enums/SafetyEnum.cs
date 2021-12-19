using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LccApiNet.Enums
{
    /// <summary>
    /// Enum with type safety.
    /// </summary>
    /// <typeparam name="TEntity">Type of the class that represents enum.</typeparam>
    public abstract class SafetyEnum<TEntity> : IEqualityComparer<SafetyEnum<TEntity>>, IEquatable<SafetyEnum<TEntity>>
        where TEntity : SafetyEnum<TEntity>, new()
    {
        protected string _value = null!;
        private static bool _wasInitialized = false;
        private static List<string> _values = new List<string>();

        static SafetyEnum()
        {
            Init();
        }

        /// <summary>
        /// Initializes enum
        /// </summary>
        protected static void Init()
        {
            if (_wasInitialized)
                return;

            _wasInitialized = true;

            typeof(TEntity).GetProperties(BindingFlags.Static | BindingFlags.Public).ToList().ForEach(a => Debug.WriteLine(a.Name));
            foreach (PropertyInfo property in typeof(TEntity).GetProperties(BindingFlags.Static | BindingFlags.Public)) {
                SafetyEnumValueAttribute? attribute = property.GetCustomAttribute<SafetyEnumValueAttribute>();
                if (attribute == null)
                    continue;

                if (_values.Contains(attribute.Value)) {
                    throw new ArgumentException("Value is already registered");
                }

                _values.Add(attribute.Value);
                property.SetValue(null,
                    new TEntity() { _value = attribute.Value });
            }
        }

        /// <summary>
        /// Access value of the enum element.
        /// </summary>
        /// <param name="entity">Enum entity.</param>
        /// <returns>Value of the enum entity.</returns>
        public static string GetValue(TEntity entity) =>
            entity._value;

        /// <summary>
        /// Tries to assign string value to the enum value.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <returns>Enum entity based on the <paramref name="value"/>.</returns>
        public static TEntity Assign(string value)
        {
            if (_values.Contains(value)) {
                return new TEntity() { _value = value };
            } else {
                throw new ArgumentException("Attempted to assign value that was not registered");
            }
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
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
        /// Inequality operator.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>false if the specified objects are equal; otherwise, true.</returns>
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

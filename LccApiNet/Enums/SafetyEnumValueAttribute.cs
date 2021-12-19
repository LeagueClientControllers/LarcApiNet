using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Enums
{
    /// <summary>
    /// Attribute of the possible enum entities
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SafetyEnumValueAttribute : Attribute
    {
        /// <summary>
        /// String value of the entity
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Creates instance of the attribute
        /// </summary>
        /// <param name="value">String value that represents enum entity</param>
        public SafetyEnumValueAttribute(string value) {
            Value = value;
        }
    }
}

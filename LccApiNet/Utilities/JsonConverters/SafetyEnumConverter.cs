using LccApiNet.Enums;

using Newtonsoft.Json;

using System;

namespace LccApiNet.Utilities.JsonConverters
{
    /// <summary>
    /// Converter from safety enum to json
    /// </summary>
    public class SafetyEnumConverter<TEntity> : JsonConverter
         where TEntity : SafetyEnum<TEntity>, new()
    {
        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SafetyEnum<TEntity>);
        }
        
        /// <inheritdoc />
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return SafetyEnum<TEntity>.Assign((string)reader.Value!);
        }
        
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteValue(SafetyEnum<TEntity>.GetValue((TEntity)value!));
        }
    }
}

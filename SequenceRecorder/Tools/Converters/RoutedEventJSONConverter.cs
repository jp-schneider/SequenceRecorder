using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sequence.Recorder.Tools.Converters
{
    /// <summary>
    /// Converter for <see cref="RoutedEvent"/>.
    /// </summary>
    public class RoutedEventJSONConverter : JsonConverter<RoutedEvent>
    {
        /// <summary>
        /// Gets the default(<see cref="RoutedEvent"/>).
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="hasExistingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override RoutedEvent ReadJson(JsonReader reader, Type objectType, RoutedEvent existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return default(RoutedEvent);
        }
        /// <summary>
        /// Writes the RoutedEvent.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, RoutedEvent value, JsonSerializer serializer)
        {
            writer.WriteRawValue(JsonConvert.SerializeObject(value));
        }
    }
}

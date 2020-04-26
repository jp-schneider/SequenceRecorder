using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sequence.Recorder.Tools.Converters
{
    /// <summary>
    /// Converter for <see cref="KeyboardDevice"/>.
    /// </summary>
    public class KeyboardDeviceJSONConverter : JsonConverter<KeyboardDevice>
    {
        /// <summary>
        /// Gets the default(<see cref="KeyboardDevice"/>).
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="hasExistingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override KeyboardDevice ReadJson(JsonReader reader, Type objectType, KeyboardDevice existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return default(KeyboardDevice);
        }
        /// <summary>
        /// Does not write anything, because type is not serializable.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, KeyboardDevice value, JsonSerializer serializer)
        {
            writer.WriteNull();
        }
    }
}

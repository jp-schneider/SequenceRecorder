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
    /// Converter for <see cref="PresentationSource"/>.
    /// </summary>
    public class PresentationSourceJSONConverter : JsonConverter<PresentationSource>
    {
        /// <summary>
        /// Gets the default(<see cref="PresentationSource"/>).
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="hasExistingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override PresentationSource ReadJson(JsonReader reader, Type objectType, PresentationSource existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return default(PresentationSource);
        }
        /// <summary>
        /// Does not write anything, because type is not serializable.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, PresentationSource value, JsonSerializer serializer)
        {
            writer.WriteNull();
        }
    }
}

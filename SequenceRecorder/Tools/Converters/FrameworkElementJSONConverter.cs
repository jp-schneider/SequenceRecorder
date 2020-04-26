using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sequence.Recorder.GUI;
using Sequence.Recorder.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sequence.Recorder.Tools.Converters
{
    /// <summary>
    /// Converter for <see cref="FrameworkElement"/> to <see cref="FrameworkElementSmall"/> conversion.
    /// </summary>
    public class FrameworkElementJSONConverter : JsonConverter<FrameworkElement>
    {
        /// <summary>
        /// Returning default(<see cref="FrameworkElement"/>)
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="hasExistingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override FrameworkElement ReadJson(JsonReader reader, Type objectType, FrameworkElement existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return default(FrameworkElement);
        }
        /// <summary>
        /// Writes a <see cref="FrameworkElementSmall"/> instead of <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, FrameworkElement value, JsonSerializer serializer)
        {
            
            writer.WriteRawValue(JsonConvert.SerializeObject((FrameworkElementSmall)value));
        }
    }


    
}

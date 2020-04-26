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

namespace Sequence.Test.UI
{
    /// <summary>
    /// Converter for <see cref="FrameworkElement"/>to null conversion.
    /// </summary>
    public class FrameworkElementNullConverter : JsonConverter<FrameworkElement>
    {
        public override FrameworkElement ReadJson(JsonReader reader, Type objectType, FrameworkElement existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return default(FrameworkElement);
        }

        public override void WriteJson(JsonWriter writer, FrameworkElement value, JsonSerializer serializer)
        {

            writer.WriteNull();
        }
    }


    
}

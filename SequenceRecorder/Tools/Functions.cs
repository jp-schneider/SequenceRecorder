using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Sequence.Recorder.Configuration;
using Sequence.Recorder.GUI;
using Sequence.Recorder.Processing;
using Sequence.Recorder.Tools.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sequence.Recorder.Tools
{
    /// <summary>
    /// Class presenting Functions.
    /// </summary>
    public static class Functions
    {
        internal static int Val(this int? @int)
        {
            return (@int == null) ? 0 : @int.Value;
        }

        internal static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<T>().SingleOrDefault();
        }

        /// <summary>
        /// Gets the Declaring Instance of an UI Element, like the Page or the Window.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        internal static FrameworkElement DeclaringInstance(this FrameworkElement element)
        {
            var parent = VisualTreeHelper.GetParent(element) as FrameworkElement;
            if (parent == null || element.GetType().Equals(typeof(Page)) || element.GetType().Equals(typeof(Window)))
            {
                return element;
            }
            else
            {
                return DeclaringInstance(parent);
            }
        }
        /// <summary>
        /// Gets the children of a Dependency Object as List.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        internal static List<DependencyObject> GetChildren(this DependencyObject element)
        {
            List<DependencyObject> result = new List<DependencyObject>();
            int count = VisualTreeHelper.GetChildrenCount(element);
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    result.Add(VisualTreeHelper.GetChild(element, i));
                }
            }
            return result;
        }

        /// <summary>
        /// To List function for a Enumerator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        internal static List<T> ToList<T>(this IEnumerator<T> enumerator)
        {
            List<T> list = new List<T>();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }
            return list;
        }

        /// <summary>
        /// Formatting a string in JSON format in a more readably JSON Representation.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string JsonPrettify(this string json)
        {
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Attaching Compiler Information: CallerLineNumber, CallerMember and FileName to a string.Format: {message} at line {lineNumber} ({file}:{caller}).
        /// Only set message! Other Parameters are set by Compiler.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lineNumber"></param>
        /// <param name="caller"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static string AttachCallerInformation(this string message, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = "", [CallerFilePath] string source = "")
        {
            string file = "";
            if (!String.IsNullOrWhiteSpace(source) && source.Contains(@"\"))
            {
                var array = source.Split('\\');
                if (array != null)
                {
                    file = array[array.Count() - 1];
                }
            }
            return $"{message.Trim()} at line {lineNumber} ({file}:{caller}).";
        }

        /// <summary>
        /// Serializing Eventargs into a JsonString. When no settings are passed, it will use them from <see cref="Config.JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string SerializeEventArgs(EventArgs args, JsonSerializerSettings settings = null)
        {
            if (settings == null)
            {
                settings = Config.Instance.JsonSerializerSettings;
            }
            var serialized = JsonConvert.SerializeObject(args, settings);

            return serialized;
        }

        /// <summary>
        /// Deserializing a JsonString into Eventargs. FrameworkElementSmall will be replaced with null.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T DeserializeEventArgs<T>(string args, JsonSerializerSettings settings = null) where T : EventArgs
        {
            if (settings == null)
            {
                settings = Config.Instance.JsonSerializerSettings;
            }
            var deserialized = JsonConvert.DeserializeObject<T>(args, settings);
            return deserialized;
        }

        /// <summary>
        /// Concats a IEnumarable of strings with a Seperator. Basicly its the reverse Split operation.
        /// </summary>
        /// <param name="enum"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        internal static string Concat(this IEnumerable<string> @enum, string seperator = "")
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < @enum.Count(); i++)
            {
                sb.Append(@enum.ElementAt(i));
                if (i < @enum.Count() - 1)
                {
                    sb.Append(seperator);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Concats a IEnumarable of strings with a Seperator. Basicly its the reverse Split operation.
        /// </summary>
        /// <param name="enum"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        internal static string Concat(this IEnumerable<string> @enum, char seperator)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < @enum.Count(); i++)
            {
                sb.Append(@enum.ElementAt(i));
                if (i < @enum.Count() - 1)
                {
                    sb.Append(seperator);
                }
            }
            return sb.ToString();
        }
    }
}

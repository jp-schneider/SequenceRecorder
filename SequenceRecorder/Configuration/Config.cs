using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sequence.Recorder.Enums;
using Sequence.Recorder.GUI;
using Sequence.Recorder.Store;
using Sequence.Recorder.Tools;
using Sequence.Recorder.Tools.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sequence.Recorder.Configuration
{
    /// <summary>
    /// Class containing configurations for the libary.
    /// </summary>
    public class Config
    {
        private static Config _instance;
        /// <summary>
        /// Returning the Singleton Instance.
        /// </summary>
        public static Config Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Config();
                }
                return _instance;
            }
        }
        private Dictionary<Type, Settings> _settings;
        /// <summary>
        /// Containing <see cref="Settings"/> for a UI - Element type.
        /// </summary>
        public ReadOnlyDictionary<Type, Settings> Settings
        {
            get
            {
                return new ReadOnlyDictionary<Type, Settings>(_settings);
            }
        }
        /// <summary>
        /// The serializer used to convert event arguments. Configure if necessary.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; set; }
        /// <summary>
        /// The Store for the events.
        /// </summary>
        public IEventStorage Store { get; set; }
        /// <summary>
        /// Event handler for log messages.
        /// </summary>
        public event EventHandler<LogEventArgs> Log;
        /// <summary>
        /// Defining Log level of the libary.
        /// </summary>
        public LogType LogLevel { get; set; } = LogType.Error;

        private Config()
        {
            SetUpSettings();
        }
        internal void WriteLog(object sender, string message, LogType type)
        {
            if (type > LogLevel) return;
            var time = DateTime.Now;
            Action action = () => Log.Invoke(sender, new LogEventArgs(message, type, time));
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,action);
        }

        private void SetUpSettings()
        {
            _settings = new Dictionary<Type, Settings>();
            JsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.None,
                MaxDepth = 5,
                Error = (sender, e) =>
                {
                    Config.Instance.WriteLog(this, $"Exception on (De)Serializing {e.ToString()}".AttachCallerInformation(), LogType.Error);
                    e.ErrorContext.Handled = true;
                },
                Converters = { new IsoDateTimeConverter(), new FrameworkElementJSONConverter(), new PresentationSourceJSONConverter(), new StringEnumConverter(), new KeyboardDeviceJSONConverter(), new RoutedEventJSONConverter() }
            };
        }
        /// <summary>
        /// Adds a setting.
        /// </summary>
        /// <param name="settings"></param>
        public void AddSettings(Settings settings)
        {
            if (settings.Type == null) throw new ArgumentException("The Settings Type was null!");
            if (!typeof(FrameworkElement).IsAssignableFrom(settings.Type)) throw new ArgumentException(settings.Type.Name + " is an Unsupported Type. Type has to be assignable to FrameworkElement.");
            if(_settings.TryGetValue(settings.Type,  out var val))
            {
                _settings.Remove(settings.Type);
               
            }
            _settings.Add(settings.Type,settings);
        }

        /// <summary>
        /// Removes the given setting.
        /// </summary>
        /// <param name="settings"></param>
        public void RemoveSettings(Settings settings)
        {
            if (settings.Type == null) throw new ArgumentException("The Settings Type was null!");
            if (!typeof(FrameworkElement).IsAssignableFrom(settings.Type)) throw new ArgumentException(settings.Type.Name + " is an Unsupported Type. Type has to be assignable to FrameworkElement.");
            _settings.Remove(settings.Type);
        }
    }
}

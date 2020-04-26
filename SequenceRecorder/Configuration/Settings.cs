using Sequence.Recorder.Enums;
using Sequence.Recorder.Events;
using Sequence.Recorder.GUI;
using Sequence.Recorder.Store;
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
    /// Class defining settings for a Type inheritating from <see cref="FrameworkElement"/>.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// The <see cref="FrameworkElement"/> Type where these settings are valid.
        /// </summary>
        public Type Type { get; set; }
        internal List<EventDefinition> _defaultEvents { get; private set; }
        /// <summary>
        /// Containing the default events for the <see cref="Type"/>.
        /// </summary>
        public List<EventDefinition> DefaultEvents
        {
            get
            {
                return _defaultEvents;
            } 
        }

        /// <summary>
        /// Constructor for the Settings. The settingsType have to inheritate from FrameworkElement.
        /// </summary>
        /// <param name="settingsType"></param>
        public Settings(Type settingsType)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(settingsType)) throw new ArgumentException("This Class only Supports Types which are assignable to FrameworkElement.");
            Type = settingsType;
            _defaultEvents = new List<EventDefinition>();
        }
    }
}

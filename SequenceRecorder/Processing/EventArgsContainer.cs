using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sequence.Recorder.Configuration;
using Sequence.Recorder.Events;
using Sequence.Recorder.GUI;
using Sequence.Recorder.Store;
using Sequence.Recorder.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Sequence.Recorder.Processing
{
    /// <summary>
    /// The Container for Libary internal usage.
    /// </summary>
    public sealed class EventArgsContainer : IEventStore
    {
        /// <summary>
        /// The original Event Args. Use at own risk, because of potential threading errors.
        /// </summary>
        public EventArgs EventArgs { get; private set; }
        /// <summary>
        /// Time when the event occured.
        /// </summary>
        public DateTime EventTime { get;  set; }
        /// <summary>
        /// The sender of the original event.
        /// </summary>
        public FrameworkElementSmall Sender { get; private set; }
        /// <summary>
        /// The element where the <see cref="Tracker"/> was registerd on.
        /// </summary>
        public FrameworkElementSmall TrackingElement { get; private set; }
        /// <summary>
        /// The EventContext of the event.
        /// </summary>
        public Dictionary<string, object> EventContext { get;private set; } = new Dictionary<string, object>();

        private string _eventData;
        /// <summary>
        /// The serialized and Threadsave Event Arguments.
        /// </summary>
        public string EventData
        {
            get
            {
                return _eventData;
            }
            private set
            {
                if(value == null || !value.Equals(_eventData))
                {
                    _eventData = value;
                }
            }
        }
        /// <summary>
        /// The Event Definition of the event.
        /// </summary>
        public EventDefinition EventDefinition { get ; set; }
        /// <summary>
        /// Creating a new Instance for wrapping EventArgs. Sets the Eventtime Property to Now.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <param name="trackingElement"></param>
        internal EventArgsContainer(object sender,EventArgs args, FrameworkElementSmall trackingElement)
        {
            TrackingElement = trackingElement;
            Sender = (FrameworkElement)sender;
            EventArgs = args;
            EventTime = DateTime.Now;
            EventData = Functions.SerializeEventArgs(args,Config.Instance.JsonSerializerSettings);
        }
        /// <summary>
        /// 
        /// </summary>
        public object SequenceIdentifier { get; set; }
        IEventDefinition IEventStore.EventDefinition { get => EventDefinition; }
       
    }
}

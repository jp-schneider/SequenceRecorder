using Newtonsoft.Json;
using Sequence.Recorder.Enums;
using Sequence.Recorder.Events;
using Sequence.Recorder.GUI;
using Sequence.Recorder.Processing;
using Sequence.Recorder.Store;
using Sequence.Recorder.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Test.Store
{
    public class OccuredEvent : IEventStore
    {
        public DateTime EventTime { get ; set ; }
        [JsonConverter(typeof(RawJsonConverter))]
        public string EventData { get; set ;}
        public object DataContext { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public EventDefinition Event { get; set; }
        public object SequenceIdentifier { get; set ; }
        IEventDefinition IEventStore.EventDefinition { get => Event; }
        public FrameworkElementSmall Sender { get; set; }
        public Dictionary<string, object> EventContext { get; set; }

        public FrameworkElementSmall TrackingElement { get; set; }
        [JsonIgnore]
        public string FormattedEventData
        {
            get
            {
                return EventData.JsonPrettify();
            }
        }

        
    }

}

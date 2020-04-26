using PropertyChanged;
using Sequence.Recorder.Enums;
using Sequence.Recorder.Events;
using Sequence.Recorder.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Test.UI
{

    public class EventDefinitionChild : EventDefinition, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public EventTypeWrapper EventTypeWrapper { 
            get
            {
                return Event;
            }
            set
            {
                Event = value;
            }
        }
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            if (property.Equals(nameof(EventType)))
            {
                OnPropertyChanged(nameof(EventTypeWrapper));
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        [AlsoNotifyFor(nameof(EventTypeWrapper))]
        public EventType Event
        {
            get
            {
                return EventType;
            }
            set
            {
                EventType = value;
            }
        }

        public bool NotAdded { get; set; } = false;

  
        public EventDefinitionChild():base(EventType.UNKNOWN,false,null)
        {

        }
        public EventDefinitionChild(EventType type, bool recursive = false, EventHandler<EventOccuredEventArgs> eventOccuredHandler = null): base(type,recursive,eventOccuredHandler)
        {

        }

        private EventDefinitionChild(IEventDefinition @event):base(@event.EventType,@event.AssignRecursively,@event.EventOccurredHandler)
        {
        }
        
        public static new EventDefinitionChild CastOrCreate(IEventDefinition @event)
        {
            if (@event is EventDefinitionChild)
            {
                return (EventDefinitionChild)@event;
            }
            else
            {
                return new EventDefinitionChild(@event);
            }
        }
    }
}

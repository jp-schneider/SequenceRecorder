using Sequence.Recorder.Enums;
using Sequence.Recorder.Processing;
using Sequence.Recorder.Store;
using Sequence.Recorder.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sequence.Recorder.Events
{
    /// <summary>
    /// Implementation for wrapping a <see cref="EventType"/> and additional arguments.
    /// </summary>
    public class EventDefinition : IEventDefinition
    {
        /// <summary>
        /// Value for the <see cref="EventType"/>.
        /// </summary>
        public EventType EventType { get; set; }
        /// <summary>
        /// Indicating whether the event handler for this <see cref="EventDefinition"/> should be assigned recursively to child elements, if they are support the given event. 
        /// </summary>
        public bool AssignRecursively { get; set; } = false;

        /// <summary>
        /// Representing the behavior of Routed Events and the ability to also listen for already handled events.
        /// </summary>
        public bool HandledEventsToo { get; set; } = true;
        /// <summary>
        /// Additional name for the <see cref="EventDefinition"/>. Default value is the EventType value.
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// Holding the event handler code, which should be run additionally on the STA thread which invokes the UI-event.
        /// </summary>
        public EventHandler<EventOccuredEventArgs> EventOccurredHandler { get; set; }

        /// <summary>
        /// Returning boolean whether the <see cref="EventDefinition"/> is assignable on the given <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="element"><see cref="FrameworkElement"/> to check the assign ability for.</param>
        /// <returns></returns>
        internal bool IsAssignable(FrameworkElement element)
        {
            var t = element.GetType();
            var desc = EventType.GetAttribute<EventDescription>();
            return desc.DeclaringTypes.Any(type => type.IsAssignableFrom(t) || type.Equals(t));
        }
        /// <summary>
        /// An EventDefinition fullfills the Equals criteria, if <see cref="EventType"/> and <see cref="AssignRecursively"/> is Equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is EventDefinition)) return false;
            var val = obj as EventDefinition;
            if (EventType == val.EventType)
            {
                if (AssignRecursively == val.AssignRecursively)
                {
                    return true;
                }

            }
            return false;

        }
        /// <summary>
        /// Basic Constructor for the Event.
        /// </summary>
        /// <param name="type"><see cref="EventType"/> of the Event.</param>
        /// <param name="recursive">Indicating whether it has to be assigned recursively.</param>
        /// <param name="eventOccuredHandler">Event handler code.</param>
        public EventDefinition(EventType type, bool recursive = false, EventHandler<EventOccuredEventArgs> eventOccuredHandler = null)
        {
            EventType = type;
            AssignRecursively = recursive;
            EventOccurredHandler = eventOccuredHandler;
            EventName = Enum.GetName(typeof(EventType), type);
        }
        /// <summary>
        /// Constructor to create the event from the IEventDefinition interface.
        /// </summary>
        /// <param name="event"></param>
        private EventDefinition(IEventDefinition @event)
        {
            EventType = @event.EventType;
            AssignRecursively = @event.AssignRecursively;
            EventName = @event.EventName;
            EventOccurredHandler = @event.EventOccurredHandler;
        }
        /// <summary>
        /// Calculates the hashcode for the event.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashFunction.GetHashCode(EventType,AssignRecursively);
        }
        /// <summary>
        /// The <see cref="EventDescription"/> of the <see cref="EventType"/> value.
        /// </summary>
        public EventDescription Description
        {
            get
            {
                return EventType.GetAttribute<EventDescription>();
            }
        }
        
        /// <summary>
        /// Gets the <see cref="System.Reflection.EventInfo"/> of one of the declaring types, by checking which is a supertype for applyToType.
        /// </summary>
        /// <param name="applyToType"></param>
        /// <returns></returns>
        public EventInfo GetEventInfo(Type applyToType)
        {
            if (applyToType == null) throw new ArgumentNullException("applyToType was null. It has to be set to determine where to get the event info from.");
            var type = Description.DeclaringTypes.Where(i => i.IsAssignableFrom(applyToType)).FirstOrDefault();
            if (type == null) throw new ArgumentException("It seems that the given type is not one / a subtype of one Type in DeclaringTypes!");
            return type.GetEvent(Description.EventHandlerPropertyName);
        }

        public FieldInfo GetFieldInfo(Type applyToType)
        {
            if (applyToType == null) throw new ArgumentNullException("applyToType was null. It has to be set to determine where to get the field info from.");
            var type = Description.DeclaringTypes.Where(i => i.IsAssignableFrom(applyToType)).FirstOrDefault();
            if (type == null) throw new ArgumentException("It seems that the given type is not one / a subtype of one Type in DeclaringTypes!");
            return type.GetField(Description.EventHandlerPropertyName + "Event", BindingFlags.Public | BindingFlags.Static);
        }

        /// <summary>
        /// Implicit operator creating an <see cref="EventDefinition"/> with the given <see cref="Sequence.Recorder.Enums.EventType"/> value.
        /// </summary>
        /// <param name="type"><see cref="Sequence.Recorder.Enums.EventType"/> where the <see cref="EventDefinition"/> should be created from.</param>
        public static implicit operator EventDefinition(EventType type)
        {
            return new EventDefinition(type, false);
        }
        /// <summary>
        /// Implicit operator returning the <see cref="Sequence.Recorder.Enums.EventType"/> of an <see cref="EventDefinition"/>.
        /// </summary>
        /// <param name="event"></param>
        public static implicit operator EventType(EventDefinition @event)
        {
            return @event.EventType;
        }
        /// <summary>
        /// If possible, casts a <see cref="IEventDefinition"/> into an <see cref="EventDefinition"/> or creates one.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static EventDefinition CastOrCreate(IEventDefinition @event)
        {
            if(@event is EventDefinition)
            {
                return (EventDefinition)@event;
            }
            else
            {
                return new EventDefinition(@event);
            }
        }
    }
}

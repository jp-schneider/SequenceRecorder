using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Recorder.Events
{
    /// <summary>
    /// Class for annotating the fields of the EventType Enum.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EventDescription : Attribute
    {
        /// <summary>
        /// Containing all types, which implementing the annotated specific event.
        /// </summary>
        public List<Type> DeclaringTypes { get; set; } = new List<Type>();
        /// <summary>
        /// Gets or sets the first of the DeclaringTypes.
        /// </summary>
        public Type DeclaringType {
            get
            {
                if(DeclaringTypes?.Count > 0)
                {
                    return DeclaringTypes[0];
                }
                return null;
            }
            set
            {
                if(DeclaringType != null)
                {
                    DeclaringTypes.RemoveAt(0);
                    
                }
                DeclaringTypes.Insert(0,value);
            }
        }
        /// <summary>
        /// The property name of the event handler.
        /// </summary>
        public string EventHandlerPropertyName { get; set; }
        /// <summary>
        /// Type of the event handler.
        /// </summary>
        public Type EventHandler { get; set; }
        /// <summary>
        /// The type of the event handler arguments.
        /// </summary>
        public Type EventArgsType { get; set; }
        /// <summary>
        /// Description text for this Attribute.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Constructor for creating the EventDescription object.
        /// </summary>
        /// <param name="declaringTypes">Types which are declaring the event.</param>
        /// <param name="eventHandlerPropertyName">Property name of the event handler.</param>
        /// <param name="eventHandler">Type of the event handler.</param>
        /// <param name="eventArgsType">Type of the event arguments.</param>
        /// <param name="description">Description text for the event.</param>
        public EventDescription(Type[] declaringTypes, string eventHandlerPropertyName, Type eventHandler, Type eventArgsType, string description)
        {
            DeclaringTypes = declaringTypes.ToList();
            EventHandlerPropertyName = eventHandlerPropertyName;
            EventHandler = eventHandler;
            EventArgsType = eventArgsType;
            Description = description;
        }
        public override int GetHashCode()
        {
            int hash = 0;
            hash += DeclaringTypes.GetHashCode();
            hash += EventHandlerPropertyName.GetHashCode();
            hash += EventHandler.GetHashCode();
            hash += EventArgsType.GetHashCode();
            hash += Description.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EventDescription)) return false;
            EventDescription val = obj as EventDescription;
            return (
                this.DeclaringTypes.Equals(val.DeclaringTypes) &&
                this.EventHandlerPropertyName.Equals(val.EventHandlerPropertyName) &&
                this.EventHandler.Equals(val.EventHandler) &&
                this.EventArgsType.Equals(val.EventArgsType) &&
                this.Description.Equals(val.Description)
                );
        }

    }
    /// <summary>
    /// Class comparing EnumDescriptions without the Declaring Type and Description. 
    /// </summary>
    public class EnumDescriptionComparer : EqualityComparer<EventDescription>
    {
        private static EnumDescriptionComparer _instance;
        public static EnumDescriptionComparer Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new EnumDescriptionComparer();
                }
                return _instance;
            }
        }

        public override bool Equals(EventDescription x, EventDescription y)
        {
          
            return (
                x.EventHandlerPropertyName.Equals(y.EventHandlerPropertyName) &&
                x.EventHandler.Equals(y.EventHandler) &&
                x.EventArgsType.Equals(y.EventArgsType)
                );
        }

        public override int GetHashCode(EventDescription obj)
        {
            int hash = 0;
 
            hash += obj.EventHandlerPropertyName.GetHashCode();
            hash += obj.EventHandler.GetHashCode();
            hash += obj.EventArgsType.GetHashCode();
            return hash;
        }
    }
}

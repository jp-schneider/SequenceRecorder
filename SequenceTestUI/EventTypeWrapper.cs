using PropertyChanged;
using Sequence.Recorder.Enums;
using Sequence.Recorder.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Test.UI
{

    public class EventTypeWrapper : INotifyPropertyChanged
    {
        public static List<EventTypeWrapper> GETALL
        {
            get
            {
                var list = new List<EventTypeWrapper>();
                var values = Enum.GetValues(typeof(EventType));
                foreach(var val in values)
                {
                    list.Add(new EventTypeWrapper((EventType)val));
                }
                return list;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public EventTypeWrapper(EventType eventType)
        {
            EventType = eventType;
        }
        public int Value {
            get 
            {
                return (int)EventType;    
            }
        }

        public string Name { 
            get {
                return Enum.GetName(typeof(EventType), EventType);   
            }
        }

        public string Text {
            get {
                return Description.Description;
            }
        }
        [AlsoNotifyFor(nameof(Value), nameof(Name), nameof(Text), nameof(Description), nameof(Display))]
        public EventType EventType { get; set; }
        public EventDescription Description { 
            get {
                var enumType = EventType.GetType();
                var name = Enum.GetName(enumType, EventType);
                return enumType.GetField(name).GetCustomAttributes(false).OfType<EventDescription>().SingleOrDefault();
            } 
        }
        public string Display
        {
            get
            {
                return Name + " - " + Value;
            }
        }
        public override string ToString()
        {
            return Name + " - " + Value;
        }

        public static implicit operator EventTypeWrapper(EventType t)
        {
            return new EventTypeWrapper(t);
        }
        public static implicit operator EventType(EventTypeWrapper t)
        {
            return t.EventType;
        }
    }
}

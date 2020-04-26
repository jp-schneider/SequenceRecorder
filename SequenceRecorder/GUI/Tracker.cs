using Sequence.Recorder.Configuration;
using Sequence.Recorder.Events;
using Sequence.Recorder.Store;
using Sequence.Recorder.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Sequence.Recorder.GUI
{
    /// <summary>
    /// This Class contains the logic for administrating the tracking Elements and Events.
    /// </summary>
    public class Tracker
    {
        /// <summary>
        /// Collection representing the Events which will be tracked. Collection can be modified to add or remove events. It will be initialized with the events defined in the settings.
        /// </summary>
        public ObservableCollection<IEventDefinition> TrackingEvents { get; private set; } = new ObservableCollection<IEventDefinition>();
        private Dictionary<FrameworkElementEvent, Delegate> _eventDelegates;
        /// <summary>
        /// Dictionary of FrameworkElementEvents with the registered Delegates.
        /// </summary>
        internal Dictionary<FrameworkElementEvent, Delegate> EventDelegates
        {
            get
            {
                return _eventDelegates;
            }
            private set
            {
                if (value == null || !value.Equals(_eventDelegates))
                {
                    _eventDelegates = value;
                }
            }
        }
        /// <summary>
        /// Gets a ReadOnlyCollection of every tracked item-Event pair of the current Tracker.
        /// </summary>
        public ReadOnlyCollection<FrameworkElementEvent> TrackingElements
        {
            get
            {
                return new ReadOnlyCollection<FrameworkElementEvent>(EventDelegates.Keys.ToList());
            }
        }

        private FrameworkElement _element;
        /// <summary>
        /// Reference on the item where the Tracker is registered on.
        /// </summary>
        public FrameworkElement Element
        {
            get
            {
                return _element;
            }
            internal set
            {
                if(value == null || !value.Equals(_element))
                {
                    _element = value;
                }
            }
        }

        /// <summary>
        /// Initialzes a Tracker with the referenced <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="element"></param>
        public Tracker(FrameworkElement element)
        {
            Element = element ?? throw new NullReferenceException("The depending FrameworkElement of the Tracker is null. Maybe it was set as null or the FrameworkElement is not yet initialized.");
            EventDelegates = new Dictionary<FrameworkElementEvent, Delegate>();    
            ApplyDefaultEvents();
            TrackingEvents.CollectionChanged += OnTrackingEventsChanged;
        }

        private void OnTrackingEventsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (((IEventDefinition)item).EventType == Enums.EventType.UNKNOWN) throw new ArgumentException("The UNKNOWN value is not allowed to set!");
                    AddEventHandler(Element, EventDefinition.CastOrCreate((IEventDefinition)item), Element);
                }
            }
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    RemoveEventHandler(Element, EventDefinition.CastOrCreate((IEventDefinition)item));
                }
            }
        }

        /// <summary>
        /// Adds the EventDelegate to the event handler for the given Event. If the EventType is not Assignable to the given FrameworkElement,
        /// it will Query through the Visual Tree and register this Event to the first FrameworkElement in each path where it fits.
        /// Method is not running Synchronous, it will add the event handlers in the Loaded Event, when the given Framework Element is not actually part of the Visual tree.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="event"></param>
        /// <param name="trackingElement"></param>
        private void AddEventHandler(FrameworkElement element, EventDefinition @event, FrameworkElement trackingElement)
        {
            var key = new FrameworkElementEvent(element, @event, trackingElement);
            //Check if Event is already Added
            if (!EventDelegates.ContainsKey(key))
            {
                Action<object, EventArgs> loadedAction = (sender, e) =>
                {
                    var children = element.GetChildren().Where(i => i is FrameworkElement).Cast<FrameworkElement>().ToList();
                    foreach (var child in children)
                    {
                        AddEventHandler(child, @event, trackingElement);
                    }
                };
                //Test if Event is assignable
                if (@event.IsAssignable(element))
                {
                    //Register Delegate
                    var @delegate = Delegate.CreateDelegate(
                        @event.Description.EventHandler, 
                        key, 
                        key.GetType()
                            .GetMethod("EventDelegate", 
                            BindingFlags.NonPublic | BindingFlags.Instance ));
                    RegisterDelegate(element,@delegate,@event);
                    EventDelegates.Add(key, @delegate);
                    Config.Instance.WriteLog(this,$"Adding {key?.Element?.ToString()} with EventType {key?.Event?.EventType.ToString()} to Tracked Events of {Element?.ToString()}.",Enums.LogType.Info);
                    if (@event.AssignRecursively)
                    {
                        var parent = VisualTreeHelper.GetParent(element);
                        var cCount = VisualTreeHelper.GetChildrenCount(element);
                        if (parent == null && cCount == default(int))
                        {
                            //Creating a delegate with Executes this after Loaded Event, because then is the Visual tree generated.
                            element.Loaded += (sender, e) => loadedAction(sender, e);
                        }
                        else
                        {
                            //Element is attached to Visual Tree so get the Children directly
                            loadedAction.Invoke(element, new RoutedEventArgs());
                        }
                    }
                }
                else
                {
                    //If not assignable: Query the visual tree for Elements where the Event can be Assigned to
                    var parent = VisualTreeHelper.GetParent(element);
                    var cCount = VisualTreeHelper.GetChildrenCount(element);
                    if (parent == null && cCount == default(int))
                    {
                        //Creating a delegate with Executes this after Loaded Event, because then is the Visual tree generated.
                        element.Loaded += (sender, e) => loadedAction(sender, e);
                    }
                    else
                    {
                        //Element is attached to Visual Tree so get the Children directly
                        loadedAction.Invoke(element, new RoutedEventArgs());
                    }
                }
            }
        }
        /// <summary>
        /// Method for registering the delegate. Based on the field type of the events, 
        /// its switches the event registration between standard CLR and WPF registration for RoutedEvents to also keep track on Handled events.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="delegate"></param>
        /// <param name="definition"></param>
        private void RegisterDelegate(FrameworkElement target, Delegate @delegate, EventDefinition definition)
        {
            //Testing if its an routed Event
            var field = definition.GetFieldInfo(target.GetType());
            if(field == null || !field.FieldType.Equals(typeof(RoutedEvent)) || !typeof(RoutedEvent).IsAssignableFrom(field.FieldType))
            {
                definition.GetEventInfo(target.GetType()).AddEventHandler(target,@delegate);
            }
            else
            {
                target.AddHandler((RoutedEvent)field.GetValue(target), @delegate, definition.HandledEventsToo);
            }
        }

        /// <summary>
        /// Removes the <see cref="FrameworkElement"/> with the given <see cref="EventDefinition"/> from Tracking.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="event"></param>
        private void RemoveEventHandler(FrameworkElement element, EventDefinition @event)
        {
            IEnumerable<KeyValuePair<FrameworkElementEvent, Delegate>> registerd = null;
            if (@event.AssignRecursively || !@event.IsAssignable(element))
            {
                registerd = EventDelegates.Where(i => i.Key.Event.Equals(@event)).ToList();
            }
            else
            {
                var key = new FrameworkElementEvent(element, @event, Element);
                registerd = EventDelegates.Where(i => i.Key.Equals(key)).ToList();
            }

            if (registerd.Count() > 0)
            {
                foreach (var item in registerd)
                {
                    var info = @event.GetEventInfo(item.Key.Element.GetType());
                    info.RemoveEventHandler(item.Key.Element, item.Value);
                    EventDelegates.Remove(item.Key);
                    Config.Instance.WriteLog(this, $"Removed {item.Key?.Element?.ToString()} with EventType {item.Key?.Event?.EventType.ToString()} to Tracked Events of {Element?.ToString()}.", Enums.LogType.Info);
                }

            }

        }

        /// <summary>
        /// Applying the default events which are set in the settings for the specific type.
        /// </summary>
        private void ApplyDefaultEvents()
        {
            var settings = GetBestFittingSettings();
            if (settings == null) return;
            settings.DefaultEvents.ForEach(eventDefinition =>
            {
                if (eventDefinition.EventType == Enums.EventType.UNKNOWN) 
                    throw new ArgumentException("The UNKNOWN value is not allowed to set!");
                TrackingEvents.Add(eventDefinition);
                AddEventHandler(Element, eventDefinition, Element);
            });
        }
        /// <summary>
        /// Picks the Settings where the Type is in the sense of the inheritance hierarchy the closest to the <see cref="Element"/> type.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        private Settings GetBestFittingSettings()
        {
            List<Type> possibleTypes = Config.Instance.Settings.Keys.Where(i => i.IsAssignableFrom(Element.GetType())).ToList();
            if (possibleTypes.Count == 0) return null; //No matching settings available 
            if (possibleTypes.Contains(Element.GetType())) return Config.Instance.Settings[Element.GetType()];
            Type current = Element.GetType();
            while(!(possibleTypes.Contains(current)) && current.BaseType != null) //Getting Hierarchie upwards
            {
                current = current.BaseType;
            }
            if (possibleTypes.Contains(current)) return Config.Instance.Settings[current];
            return null;
        }
    }
}

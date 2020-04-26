using Sequence.Recorder.Configuration;
using Sequence.Recorder.Enums;
using Sequence.Recorder.Events;
using Sequence.Recorder.Processing;
using Sequence.Recorder.Store;
using Sequence.Recorder.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sequence.Recorder.GUI
{
    /// <summary>
    /// This class is combining the <see cref="TrackingElement"/> and the registered event <see cref="EventDefinition"/>.
    /// </summary>
    public class FrameworkElementEvent
    {
        public FrameworkElementEvent()
        {
        }
        /// <summary>
        /// Constructor of the FrameworkElementEvent class.
        /// </summary>
        /// <param name="element">The FrameworkElement where the specified event will be registered.</param>
        /// <param name="event">The IEventDefinition of the event which has to be registered.</param>
        /// <param name="trackingElement">The FrameworkElement where the Tracker is associated to.</param>
        public FrameworkElementEvent(FrameworkElement element, EventDefinition @event, FrameworkElement trackingElement)
        {
            Element = element;
            Event = @event;
            TrackingElement = trackingElement;
            if(@event.EventOccurredHandler != null)
            {
                EventOccurred += @event.EventOccurredHandler;
            }
        }
        /// <summary>
        /// The FrameworkElement where the Tracker is associated to.
        /// </summary>
        public FrameworkElement TrackingElement { get;private set; }
        /// <summary>
        /// The FrameworkElement where the specified event is registered.
        /// </summary>
        public FrameworkElement Element { get;private set; }
        /// <summary>
        /// The IEventDefinition of the event which has been registered.
        /// </summary>
        public EventDefinition Event { get; private set; }
        /// <summary>
        /// Event which is invoked by the Dispatcher. Creates ability for making changes on EventArgsContainer (e.g. add Data).
        /// </summary>
        public event EventHandler<EventOccuredEventArgs> EventOccurred;

        /// <summary>
        /// Returning a hash code based on <see cref="Element"/>,<see cref="Event"/> and <see cref="TrackingElement"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashFunction.GetHashCode(Element,Event,TrackingElement);
        }
        /// <summary>
        /// Equals method for comparing instances. <see cref="Element"/>,<see cref="Event"/> and <see cref="TrackingElement"/> are taken into account.
        /// </summary>
        /// <param name="obj">The object where to current instance should be compared to.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is FrameworkElementEvent)) return false;
            var fe = (FrameworkElementEvent)obj;
            if (this.Element == null && fe.Element == null || (this.Element != null && this.Element.Equals(fe.Element)))
            {
                if (this.TrackingElement == null && fe.TrackingElement == null || (this.TrackingElement != null && this.TrackingElement.Equals(fe.TrackingElement)))
                {
                    if (this.Event.Equals(fe.Event))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Delegate Method which will be used as event listener.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void EventDelegate(object sender, EventArgs e)
        {
            try
            {
                var container = new EventArgsContainer(sender, e, TrackingElement)
                {
                    EventDefinition = Sequence.Recorder.Events.EventDefinition.CastOrCreate(Event)
                };
                try
                {
                    Config.Instance.WriteLog(this, $"Invoking EventOccured Event for {container?.Sender?.StringRepresentation} and Event {container?.EventDefinition?.EventType}.", LogType.Info);
                    EventOccurred?.Invoke(this, new EventOccuredEventArgs(sender, e, container));
                }
                catch (Exception ex)
                {
                    Config.Instance.WriteLog(this, $"Exception on user defined EventOccuredAction: \r\n {ex.ToString()}".AttachCallerInformation(), LogType.Error);
                }
                EventProcessing.Instance.ProcessEvent(container);
            }catch(Exception ex)
            {
                Config.Instance.WriteLog(this,$"Exception on Processing event {e.ToString()}: \r\n {ex.ToString()}".AttachCallerInformation(),LogType.Error);
            }
        }
    }
}

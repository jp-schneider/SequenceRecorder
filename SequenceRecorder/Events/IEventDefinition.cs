using Sequence.Recorder.Enums;
using Sequence.Recorder.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Recorder.Events
{
    /// <summary>
    /// Interface describing properties of the EventDefinition.
    /// </summary>
    public interface IEventDefinition
    {
        /// <summary>
        /// Type of the event.
        /// </summary>
        EventType EventType { get; set; }
        /// <summary>
        /// Name of the event.
        /// </summary>
        string EventName { get; set; }
        /// <summary>
        /// Indication whether the event handler should be assigned recursively to child elements in the visual tree, if they support the given event.
        /// </summary>
        bool AssignRecursively { get; set; }
        /// <summary>
        /// Representing the behavior of Routed Events and the ability to also listen for already handled events.
        /// </summary>
        bool HandledEventsToo { get; set; }
        /// <summary>
        /// Event handler which gets invoked by the STA-Thread, to do STA related calls.
        /// </summary>
        EventHandler<EventOccuredEventArgs> EventOccurredHandler { get; set; }
    }
}

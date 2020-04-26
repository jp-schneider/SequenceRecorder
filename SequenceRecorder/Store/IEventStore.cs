using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Sequence.Recorder.Events;
using Sequence.Recorder.Processing;
using Sequence.Recorder.GUI;

namespace Sequence.Recorder.Store
{ 
    /// <summary>
    /// This interface defines the needed properties for storing a processed event. 
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Sender of the event.
        /// </summary>
        FrameworkElementSmall Sender { get;}
        /// <summary>
        /// Element on which the tracker was registered.
        /// </summary>
        FrameworkElementSmall TrackingElement { get; }
        /// <summary>
        /// Event definition for the occured event.
        /// </summary>
        IEventDefinition EventDefinition { get; }
        /// <summary>
        /// Time when event occurs.
        /// </summary>
        DateTime EventTime { get; }
        /// <summary>
        /// Representing the event arguments in a JSON format.
        /// </summary>
        string EventData { get; } 
        /// <summary>
        /// Carring data which was stored in the processing part.
        /// </summary>
        Dictionary<string,object> EventContext { get;}
        /// <summary>
        /// Object to identify the current sequence.
        /// </summary>
        object SequenceIdentifier { get; set; }
    }
}

using Sequence.Recorder.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Recorder.Processing
{
    /// <summary>
    /// EventArgs for the Event which is fired after an Event occured.
    /// </summary>
    public class EventOccuredEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor for the <see cref="EventOccuredEventArgs"/>.
        /// </summary>
        /// <param name="originalSender">Sender of the original event.</param>
        /// <param name="originalEventArgs">Event arguments of the original event.</param>
        /// <param name="createdContainer">Container for the original event.</param>
        public EventOccuredEventArgs(object originalSender, EventArgs originalEventArgs, IEventStore createdContainer)
        {
            OriginalSender = originalSender;
            OriginalEventArgs = originalEventArgs;
            CreatedContainer = createdContainer;
            CreatedTime = DateTime.Now;
        }
        /// <summary>
        /// Sender of the original event.
        /// </summary>
        public object OriginalSender { get; private set; }
        /// <summary>
        /// Event arguments of the original event.
        /// </summary>
        public EventArgs OriginalEventArgs { get; private set; }
        /// <summary>
        /// Container for the original event.
        /// </summary>
        public IEventStore CreatedContainer { get; private set; }
        /// <summary>
        /// Time when this Arguments were created.
        /// </summary>
        public DateTime CreatedTime { get; private set; }
    }
}

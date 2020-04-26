using Sequence.Recorder.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Recorder.Processing
{
    /// <summary>
    /// Event arguments for the Processing Finished event.
    /// </summary>
    public class EventProcessingFinishedEventArgs : CancelEventArgs
    {
        internal EventProcessingFinishedEventArgs(IEventStore eventArgsContainer, EventProcessingStartedEventArgs eventProcessingStartedEventArgs)
        {
            EventArgsContainer = eventArgsContainer;
            ProcessingStarted = eventProcessingStartedEventArgs.ProcessingStarted;
            ProcessingFinished = DateTime.Now;
        }
        /// <summary>
        /// TimeStamp after processing was finished.
        /// </summary>
        public DateTime ProcessingFinished { get; private set; }
        /// <summary>
        /// TimeStamp before processing was started.
        /// </summary>
        public DateTime ProcessingStarted { get; private set; }
        /// <summary>
        /// Container for the original event.
        /// </summary>
        public IEventStore EventArgsContainer { get; private set; }

        /// <summary>
        /// <para>Cancellation flag.</para>
        /// <para>If it's set to true, no more processing takes place and the Data will not be stored. The evaluation takes place after every delegate invocation.</para>
        /// </summary>
        public new bool Cancel { get; set; } = false;
    }
}

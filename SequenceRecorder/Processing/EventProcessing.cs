using Newtonsoft.Json;
using Sequence.Recorder.Configuration;
using Sequence.Recorder.Enums;
using Sequence.Recorder.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sequence.Recorder.Processing
{
    /// <summary>
    /// Class containing event processing logic.
    /// </summary>
    public class EventProcessing
    {
        private static EventProcessing _instance;
        public static EventProcessing Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new EventProcessing();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Event handler raised on ProcessingStart of the given Event, before any processing was made and before the Event was stored.
        /// </summary>
        public event EventHandler<EventProcessingStartedEventArgs> ProcessingStarted;
        /// <summary>
        /// Event handler raised on ProcessingFinished of the given Event, after any processing, but before storing, takes place.
        /// </summary>
        public event EventHandler<EventProcessingFinishedEventArgs> ProcessingFinished;

        /// <summary>
        /// Gets the Count of available threads in the thread pool.
        /// </summary>
        public int AvailableThreads {
            get
            {
                ThreadPool.GetAvailableThreads(out int workers,out int ea);
                return workers;
            }
        }
           

        /// <summary>
        /// Creating Processing Class. Initialize ThreadPool
        /// </summary>
        private EventProcessing()
        {
            int cores = Environment.ProcessorCount;
            ThreadPool.SetMaxThreads(cores - 1, cores - 1);

        }

        /// <summary>
        /// Adding Event to the Threadpool and Starts its Processing.
        /// </summary>
        /// <param name="args"></param>
        internal void ProcessEvent(EventArgsContainer args)
        {
            ThreadPool.QueueUserWorkItem(ProcessingWrapper,args);
        }

        /// <summary>
        /// Wrapper for Processing Code. Adding Events.
        /// </summary>
        /// <param name="args"></param>
        private void ProcessingWrapper(object state)
        {
            var args = (EventArgsContainer)state;
            var started = new EventProcessingStartedEventArgs(args);
            Config.Instance.WriteLog(this, $"Invoking ProcessingStarted Event for {args?.Sender?.StringRepresentation} and Event {args?.EventDefinition?.EventType}.", LogType.Info);
            try
            {
                InvokeCancelEvent(ProcessingStarted, started);
            }
            catch(Exception e)
            {
                Config.Instance.WriteLog(this, $"Exception on user defined ProcessingStarted EventHandler: \r\n {e.ToString()}".AttachCallerInformation(), LogType.Error);
            }
            if (started.Cancel)
            {
                Config.Instance.WriteLog(this, $"ProcessingStarted canceled further processing for {args?.Sender?.StringRepresentation} and Event {args?.EventDefinition?.EventType}.", LogType.Info);
            }
            if (started.Cancel) return;
            var finished = new EventProcessingFinishedEventArgs(args, started);
            Config.Instance.WriteLog(this, $"Invoking ProcessingFinished Event for {args?.Sender?.StringRepresentation} and Event {args?.EventDefinition?.EventType}.", LogType.Info);
            try
            {
                InvokeCancelEvent(ProcessingFinished, finished);
            }catch(Exception e)
            {
                Config.Instance.WriteLog(this, $"Exception on user defined ProcessingFinished EventHandler: \r\n {e.ToString()}".AttachCallerInformation(), LogType.Error);
            }
            if (finished.Cancel)
            {
                Config.Instance.WriteLog(this, $"ProcessingFinished canceled further processing for {args?.Sender?.StringRepresentation} and Event {args?.EventDefinition?.EventType}.", LogType.Info);
            }
            if (finished.Cancel) return;
            Storing(finished);
        }
        private void InvokeCancelEvent<T>(EventHandler<T> eventHandler, T args) where T : CancelEventArgs
        {
            if (eventHandler != null)
            {
                try
                {
                    foreach (EventHandler<T> handler in eventHandler.GetInvocationList())
                    {
                        handler(this, args);
                        if (args.Cancel)
                        {
                            break;
                        }
                    }
                }catch(Exception e)
                {
                    Config.Instance.WriteLog(this, $"Invocation of the {eventHandler?.ToString()} Eventhandler failed. Exception: \r\n {e.ToString()}", LogType.Error);
                }
            }
        }
        private void Storing(EventProcessingFinishedEventArgs args)
        {
            Config.Instance.WriteLog(this, $"Calling Store for {args?.EventArgsContainer?.Sender?.StringRepresentation} and Event {args?.EventArgsContainer?.EventDefinition?.EventType}.", LogType.Info);
            Configuration.Config.Instance.Store?.Store(args.EventArgsContainer);
        }

    }
}

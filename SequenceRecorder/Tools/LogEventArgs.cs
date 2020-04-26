using Sequence.Recorder.Configuration;
using Sequence.Recorder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Recorder.Tools
{
    /// <summary>
    /// Event arguments for the <see cref="Config.Log"/> event.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        internal LogEventArgs(string message, LogType type = 0, DateTime time = default(DateTime))
        {
            Message = message;
            Type = type;
            Time = time == default(DateTime) ? DateTime.Now : time;
        }
        /// <summary>
        /// The Logging Message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// The Logging type.
        /// </summary>
        public LogType Type { get; set; }

        /// <summary>
        /// Time of the log.
        /// </summary>
        public DateTime Time { get; set; }
    }
}

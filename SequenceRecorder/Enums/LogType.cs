using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Recorder.Enums
{
    /// <summary>
    /// Type for specifying Log output.
    /// </summary>
    public enum LogType : byte
    {
        /// <summary>
        /// <see cref="LogType"/> for error messages.
        /// </summary>
        Error = 0,
        /// <summary>
        /// <see cref="LogType"/> for warning messages.
        /// </summary>
        Warning = 1,
        /// <summary>
        /// <see cref="LogType"/> for info messages.
        /// </summary>
        Info = 2
    }
}

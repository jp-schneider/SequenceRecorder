using Sequence.Recorder.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Recorder.Store
{
    /// <summary>
    /// This interface needs to be implemented in a class, where the storing takes place.
    /// </summary>
    public interface IEventStorage
    {
        /// <summary>
        /// Method for storing the event.
        /// </summary>
        /// <param name="container">Container which wraps the <see cref="EventDefinition"/> with additional data.</param>
        void Store(IEventStore container);
    }
}

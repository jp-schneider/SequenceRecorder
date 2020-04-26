using Newtonsoft.Json;
using Sequence.Recorder.Events;
using Sequence.Recorder.Store;
using Sequence.Recorder.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Test.Store
{
    public class Storage : IEventStorage
    {
        public ConcurrentBag<OccuredEvent> StoreMock { get; set; }
        public Storage()
        {
            StoreMock = new ConcurrentBag<OccuredEvent>();
        }

        /// <summary>
        /// Gets the Eventdata an insert it into a Mock.
        /// </summary>
        /// <param name="container"></param>
        public void Store(IEventStore container)
        {
            var ev = new OccuredEvent()
            {
                EventData = container.EventData,
                EventTime = container.EventTime,
                DataContext = container.EventContext,
                Event = container.EventDefinition as EventDefinition,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Sender = container.Sender,
                SequenceIdentifier = container.SequenceIdentifier,
                TrackingElement = container.TrackingElement
            };
            StoreMock.Add(ev);
        }
        /// <summary>
        /// Saves the non persistent mock into a Json file.
        /// </summary>
        /// <returns></returns>
        public bool SaveMock(string path, JsonSerializerSettings settings = null)
        {
            bool success = false;
            var orderdList = StoreMock.OrderByDescending(i => i.EventTime).ToList();
            try
            {
                using (var stream = new FileStream(path, System.IO.FileMode.Create))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine("[");
                        for (int i = 0; i < orderdList.Count; i++)
                        {
                            var item = orderdList[i];
                            writer.Write(JsonConvert.SerializeObject(item, settings).JsonPrettify());
                            if (i + 1 < orderdList.Count)
                            {
                                writer.Write($",{Environment.NewLine}");
                            }
                        }
                        writer.WriteLine("]");
                    }
                }
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in Saving Mock: {Environment.NewLine} {e.ToString()}");
            }
            return success;
        }

    }
}

using Sequence.Recorder.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Tools
{
    public class EnumDefinition
    {
        public string Name { get; set; }
        public int? Value { get; set; }
        public EventDescription Description { get; set; }

        public EnumDefinition(string name, int? value, EventDescription description)
        {
            Name = name;
            Value = value;
            Description = description;
        }
    }
}

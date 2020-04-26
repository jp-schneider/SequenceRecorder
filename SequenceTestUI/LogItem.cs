using Newtonsoft.Json.Linq;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Test.UI
{
    public class LogItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public string TypeName { get; set; }
        public DateTime OccuredTime { get; set; }

        public string Value { get; set; }

        public string EventName { get; set; }
        public string Sender { get; set; }
        public List<JToken> Tokens{get; set;}

        public void CalculateTokens()
        {
            Value = Value.Replace("\\\"","\"");
            Value = Value.Replace("\"{", "{");
            Value = Value.Replace("}\"", "}");
            var token = JToken.Parse(Value);
            var tokens = new List<JToken>();
            if (token != null)
            {
                tokens.Add(token);
            }
            Tokens = tokens;
        }
    }
}

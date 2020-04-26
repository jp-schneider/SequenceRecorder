using Sequence.Recorder.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Tools
{
    public class EnumDefinitionComparer : EqualityComparer<EnumDefinition>
    {
        private static EnumDefinitionComparer _instance;
        public static EnumDefinitionComparer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EnumDefinitionComparer();
                }
                return _instance;
            }
        }
        public override bool Equals(EnumDefinition x, EnumDefinition y)
        {

            return (
                x.Name.Equals(y.Name) &&
                EnumDescriptionComparer.Instance.Equals(x.Description, y.Description)
                );
        }

        public override int GetHashCode(EnumDefinition obj)
        {
            int hash = 0;
            hash += obj.Name.GetHashCode();
            hash += EnumDescriptionComparer.Instance.GetHashCode(obj.Description);
            return hash;
        }
    }
}

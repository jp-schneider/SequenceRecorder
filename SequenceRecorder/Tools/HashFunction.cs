using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Recorder.Tools
{
    internal static class HashFunction
    {
        private const int Initial = 13;
        private const int Multiplier = 19;

        /// <summary>
        /// Hashcode function with multiple Parameters.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int GetHashCode(params object[] values)
        {
            unchecked
            {
                int hash = Initial;
                if (values != null)
                    for (int i = 0; i < values.Length; i++)
                    {
                        object cur = values[i];
                        hash = hash * Multiplier + ((cur != null) ? cur.GetHashCode() : 0);
                    }

                return hash;
            }
        }
    }
}

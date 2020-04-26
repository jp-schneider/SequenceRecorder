using Sequence.Recorder.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sequence.Recorder.GUI
{
    /// <summary>
    /// Class representing a few identifying properties for a <see cref="FrameworkElement"/> to avoid STA failures.
    /// </summary>
    public class FrameworkElementSmall
    {
        /// <summary>
        /// Constructor for a <see cref="FrameworkElementSmall"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="namespace"></param>
        /// <param name="type"></param>
        /// <param name="stringRep"></param>
        /// <param name="hashcode"></param>
        public FrameworkElementSmall(string name, string @namespace, Type type, string stringRep, int hashcode)
        {
            Name = name;
            Namespace = @namespace;
            Type = type;
            StringRepresentation = stringRep;
            HashCode = hashcode;
        }
        /// <summary>
        /// Name of the original <see cref="FrameworkElement"/>.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Namespace of the original <see cref="FrameworkElement"/>.
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// Type of the original <see cref="FrameworkElement"/>.
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// String Representation of the original <see cref="FrameworkElement"/>.
        /// </summary>
        public string StringRepresentation { get; set; }
        /// <summary>
        /// Hashcode of the original <see cref="FrameworkElement"/>.
        /// </summary>
        public int HashCode { get; private set; }

        /// <summary>
        /// Equals method comparing <see cref="Name"/>, <see cref="Namespace"/>, <see cref="Type"/> and <see cref="HashCode"/>.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is FrameworkElementSmall)) return false;
            var comp = (FrameworkElementSmall)obj;
            if (!String.IsNullOrEmpty(Name))
            {
                if (!Name.Equals(comp.Name)) return false;
            }
            else if (!String.IsNullOrEmpty(comp.Name))
            {
                return false;
            }
            if (!String.IsNullOrEmpty(Namespace))
            {
                if (!Namespace.Equals(comp.Namespace)) return false;
            }
            if (Type != null)
            {
                if (!Type.Equals(comp.Type)) return false;
            }
            else if (comp.Type != null)
            {
                return false;
            }
            if (HashCode != comp.HashCode) return false;

            return true;
        }
        /// <summary>
        /// Calculates the hashcode. If <see cref="HashCode"/> is not 0 it will return the <see cref="HashCode"/> value, otherwise it calculates the hashcode out of <see cref="Name"/>, <see cref="Namespace"/> and <see cref="Type"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (HashCode != 0) ? HashCode : HashFunction.GetHashCode(Name, Namespace, Type);
        }
        /// <summary>
        /// Operator for implicit conversion of a <see cref="FrameworkElement"/> to a <see cref="FrameworkElementSmall"/>.
        /// </summary>
        /// <param name="elem"></param>
        public static implicit operator FrameworkElementSmall(FrameworkElement elem)
        {
            return new FrameworkElementSmall(
                elem.Name,
                elem.DeclaringInstance().GetType().Namespace + "." + elem.DeclaringInstance().GetType().Name,
                elem.GetType(),
                elem.ToString(),
                elem.GetHashCode());
        }
    }
}

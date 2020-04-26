using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sequence.Recorder.GUI
{
    /// <summary>
    /// An implementation for the Tracker Attached Property.
    /// </summary>
    public class Recorder : DependencyObject
    {
        /// <summary>
        /// Representing the attached Tracker property.
        /// </summary>
        public static readonly DependencyProperty TrackerProperty = DependencyProperty.RegisterAttached(
          "Tracker",
          typeof(Tracker),
          typeof(FrameworkElement),
          new FrameworkPropertyMetadata(null)
        );
        /// <summary>
        /// Setter for the <see cref="Tracker"/> object on the given <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="element"><see cref="FrameworkElement"/> on which the <see cref="Tracker"/> has to be registered.</param>
        /// <param name="value">Instance of the <see cref="Tracker"/>.</param>
        public static void SetTracker(FrameworkElement element, Tracker value)
        {
            element.SetValue(TrackerProperty, value);
        }
        /// <summary>
        /// Returning the <see cref="Tracker"/> for the given <see cref="FrameworkElement"/> if specified.
        /// </summary>
        /// <param name="element">Element where the <see cref="Tracker"/> should be returned from.</param>
        /// <returns></returns>
        public static Tracker GetTracker(FrameworkElement element)
        {
            return (Tracker)element.GetValue(TrackerProperty);
        }
    }
}

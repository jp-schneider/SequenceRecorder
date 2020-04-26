using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sequence.Recorder.Configuration;
using Sequence.Recorder.Enums;
using Sequence.Recorder.Events;
using Sequence.Recorder.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sequence.Test.UnitTests
{
    [TestClass]
    public class TrackerTests
    {
        FrameworkElement Element { get; set; }
        List<Settings> Settings = new List<Settings>();

        public void SetUpSomeVisualStuff()
        {
            Element = new Page();
            var grid = new Grid();
            ((Page)Element).Content = grid;
            var textbox = new TextBox();
            grid.Children.Add(textbox);
        }

        public void RemoveAllSettings()
        {
            var settings = Config.Instance.Settings.ToList();
            foreach(var s in settings)
            {
                Config.Instance.RemoveSettings(s.Value);
            }
        }

        public void AddSettings()
        {
            RemoveAllSettings();
            foreach (var s in Settings)
            {
                Config.Instance.AddSettings(s);
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            SetUpSomeVisualStuff();
            RemoveAllSettings();
            Settings = new List<Settings>();
            Settings settings = new Settings(typeof(Page));
            settings.DefaultEvents.AddRange(new List<EventDefinition>() {
                new EventDefinition(Sequence.Recorder.Enums.EventType.Loaded),
                new EventDefinition(Sequence.Recorder.Enums.EventType.KeyDown),
                new EventDefinition(Sequence.Recorder.Enums.EventType.KeyUp)
                }
                );
            Settings.Add(settings);
            AddSettings();
        }

        [TestMethod]
        public void InitialEventRegistrationTest()
        {
            var eventDefinition = new EventDefinition(EventType.GotFocus);
            var setting = Settings.Where(i => i.Type.Equals(typeof(Page))).FirstOrDefault();
            setting.DefaultEvents.Add(eventDefinition);
            var Tracker = new Tracker(Element);
            Assert.IsTrue(Tracker.TrackingEvents.All(i => setting.DefaultEvents.Contains(i)));
        }

        [TestMethod]
        public void RuntimeEventRegistrationTest()
        { 
            var Tracker = new Tracker(Element);
            var eventDefinition = new EventDefinition(EventType.GotFocus);
            Tracker.TrackingEvents.Add(eventDefinition);
            Assert.IsTrue(Tracker.TrackingElements.Any(i => i.Element.Equals(Element) && i.Event.Equals(eventDefinition)));
        }

        
        [TestMethod]
        public void RegistrationNonRecursiveTest()
        {
            var Tracker = new Tracker(Element);
            var eventDefinition = new EventDefinition(EventType.GotFocus, false);
            Tracker.TrackingEvents.Add(eventDefinition);
            Assert.IsTrue(Tracker.TrackingElements.Where(i => i.Event.Equals(eventDefinition)).ToList().Count == 1);
        }

        [TestMethod]
        public void RuntimeEventRemovalTest()
        {
            var Tracker = new Tracker(Element);
            var eventDefinition = new EventDefinition(EventType.GotFocus);
            Tracker.TrackingEvents.Add(eventDefinition);
            Assert.IsTrue(Tracker.TrackingElements.Any(i => i.Element.Equals(Element) && i.Event.Equals(eventDefinition)));

            var removed = Tracker.TrackingEvents.Remove(new EventDefinition(EventType.GotFocus));
            Assert.IsTrue(removed);
            Assert.IsFalse(Tracker.TrackingElements.Any(i => i.Element.Equals(Element) && i.Event.Equals(eventDefinition)));
        }

        [TestMethod]
        public void RuntimeEventRemovalTest2()
        {
            var Tracker = new Tracker(Element);
            var eventDefinition = new EventDefinition(EventType.GotFocus);
            Tracker.TrackingEvents.Add(eventDefinition);
            Assert.IsTrue(Tracker.TrackingElements.Any(i => i.Element.Equals(Element) && i.Event.Equals(eventDefinition)));

            var removed = Tracker.TrackingEvents.Remove(eventDefinition);
            Assert.IsTrue(removed);
            Assert.IsFalse(Tracker.TrackingElements.Any(i => i.Element.Equals(Element) && i.Event.Equals(eventDefinition)));
        }

        [TestMethod]
        public void RuntimeEventRemovalTest3()
        {
            var Tracker = new Tracker(Element);
            var eventDefinition = new EventDefinition(EventType.GotFocus);
            Tracker.TrackingEvents.Add(eventDefinition);
            Assert.IsTrue(Tracker.TrackingElements.Any(i => i.Element.Equals(Element) && i.Event.Equals(eventDefinition)));

            var removed = Tracker.TrackingEvents.Remove(new EventDefinition(EventType.GotFocus, recursive: true));
            Assert.IsFalse(removed);
            Assert.IsTrue(Tracker.TrackingElements.Any(i => i.Element.Equals(Element) && i.Event.Equals(eventDefinition)));
        }

        [TestMethod]
        public void RuntimeEventRemovalTest4()
        {
            var Tracker = new Tracker(Element);
            var eventDefinition = new EventDefinition(EventType.GotFocus);
            Tracker.TrackingEvents.Add(eventDefinition);
            Assert.IsTrue(Tracker.TrackingElements.Any(i => i.Element.Equals(Element) && i.Event.Equals(eventDefinition)));

            var removed = Tracker.TrackingEvents.Remove(new EventDefinition(EventType.GotFocus,false,eventOccuredHandler: (s,e) => { }));
            Assert.IsTrue(removed);
            Assert.IsFalse(Tracker.TrackingElements.Any(i => i.Element.Equals(Element) && i.Event.Equals(eventDefinition)));
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            RemoveAllSettings();
        }

        
    }
}


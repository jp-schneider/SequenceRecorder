using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sequence.Recorder.Configuration;
using Sequence.Recorder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Test.UnitTests
{
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void SettingsConstructorExceptionTest()
        {
            Action a = () => {
                Settings s = new Settings(this.GetType());

            };

            Assert.ThrowsException<ArgumentException>(a);
        }

        [TestMethod]
        public void SettingsConstructorTest()
        {
            
            Settings s = new Settings(typeof(System.Windows.Controls.Page));
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void SettingsAddTest()
        {
            Settings s = new Settings(typeof(System.Windows.Controls.Page));
            s.DefaultEvents.Add(EventType.KeyDown);
            Config.Instance.AddSettings(s);
            Assert.IsTrue(Config.Instance.Settings.Any(i => i.Value.Equals(s)));
        }

        [TestMethod]
        public void SettingsDuplicatedAddTest()
        {
            Settings s = new Settings(typeof(System.Windows.Controls.Page));
            s.DefaultEvents.Add(EventType.KeyDown);
            Config.Instance.AddSettings(s);
            Config.Instance.AddSettings(s);
            Assert.IsTrue(Config.Instance.Settings.Where(i => i.Value.Equals(s)).Count() == 1);
        }

        [TestMethod]
        public void SettingsRemoveTest()
        {
            Settings s = new Settings(typeof(System.Windows.Controls.Page));
            s.DefaultEvents.Add(EventType.KeyDown);
            Config.Instance.AddSettings(s);
            Assert.IsTrue(Config.Instance.Settings.Where(i => i.Value.Equals(s)).Count() == 1);
            Config.Instance.RemoveSettings(s);
            Assert.IsTrue(Config.Instance.Settings.Where(i => i.Value.Equals(s)).Count() == 0);
        }
    }
}

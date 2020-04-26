using PropertyChanged;
using Sequence.Recorder.Configuration;
using Sequence.Recorder.Enums;
using Sequence.Recorder.Events;
using Sequence.Recorder.GUI;
using Sequence.Recorder.Processing;
using Sequence.Recorder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sequence.Test.Store;

namespace Sequence.Test.UI
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window, INotifyPropertyChanged
    {
        public Storage Storage { get; set; }
        public Tracker Tracker { get; set; }
        public InspectWindow Inspect { get; set; }
        public TestPage TestPage
        {
            get
            {
                return pgTestPage;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public MainWindow()
        {
            Storage = new Storage();
            Config.Instance.Store = Storage;


            InitializeComponent();
            CalculateStartupLocation();
            pgTestPage.MainWindow = this;

            Tracker = new Tracker(this);
            Sequence.Recorder.GUI.Recorder.SetTracker(this,Tracker);

            Inspect = new InspectWindow(this);
            Inspect.Closed += Inspect_Closed;

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Inspect.Show();
            Inspect.Owner = this;
        }

        private void Inspect_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Log(object sender, EventArgs e)
        {
            Inspect?.LogEvents(sender, e);
        }
        public void CreateTestSettings(Type type)
        {
            List<EventDefinition> eventDefinitions = new List<EventDefinition>() { new EventDefinition(EventType.KeyDown, false, Log) };
            bool skip = false;
            do
            {
                if (!skip)
                {
                    var set = new Settings(type);
                    set.DefaultEvents.AddRange(eventDefinitions);
                    Config.Instance.AddSettings(set);
                }
                skip = !skip;
                if (typeof(FrameworkElement).IsAssignableFrom(type.BaseType))
                {
                    type = type.BaseType;
                }
                else
                {
                    type = null;
                }
            } while (type != null);
        }

        private void CalculateStartupLocation()
        {
            double cwidth = System.Windows.SystemParameters.PrimaryScreenWidth / 2; 
            double cheight = System.Windows.SystemParameters.PrimaryScreenHeight / 2;
            double left = (cwidth -10)- this.Width;
            double height = cheight - this.Height / 2;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = left;
            this.Top = height;
        }
    }
}

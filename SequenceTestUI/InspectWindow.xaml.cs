using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using PropertyChanged;
using Sequence.Recorder.Configuration;
using Sequence.Recorder.Enums;
using Sequence.Recorder.Events;
using Sequence.Recorder.Processing;
using Sequence.Recorder.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Sequence.Test.UI
{
    /// <summary>
    /// Interaktionslogik für InspectWindow.xaml
    /// </summary>
    public partial class InspectWindow : Window, INotifyPropertyChanged
    {
        public MainWindow MainWindow { get; set; }
        public EventType AddEventType { get; set; }
        public bool AddIsRecursive { get; set; }
        public bool ShowEventOccured { get; set; } = true;
        public bool ShowProcessingStarted { get; set; } = true;
        public bool ShowProcessingFinished { get; set; } = true;
        public bool ShowLog { get; set; } = true;

        public JsonSerializerSettings Settings { get; set; }

        public SortedList<LogItemsKey, LogItem> LogItems { get; set; } = new SortedList<LogItemsKey, LogItem>(new LogItemsKeyComparer());
        public List<LogItem> Target { get 
            {
                return LogItems.Values.ToList();
            }
        } 
        public List<LogType> Types
        {
            get
            {
                return new List<LogType>() { LogType.Error, LogType.Warning, LogType.Info };
            }
        }
        public LogType Type
        {
            get
            {
                return Config.Instance.LogLevel;
            }
            set
            {
                Config.Instance.LogLevel = value;
            }
        }

        private void GetSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Error += (sender, e) => {
                Console.WriteLine(e.ToString());
            };
            settings.Converters.Add(new FrameworkElementNullConverter());
            settings.ContractResolver = ShouldSerializeContractResolver.Instance;
            Settings = settings;
        }
        public ObservableCollection<EventDefinitionChild> EventDefinitions { get; set; } = new ObservableCollection<EventDefinitionChild>();
        public InspectWindow(MainWindow window)
        {
            InitializeComponent();
            CalculateStartupLocation();
            GetSettings();
            this.DataContext = this;
            MainWindow = window;
            MainWindow.Tracker.TrackingEvents.ToList().ForEach(i => EventDefinitions.Add(EventDefinitionChild.CastOrCreate(i)));
            EventDefinitions.CollectionChanged += EventDefinitions_CollectionChanged;
            Config.Instance.Log += LogEvents;
            EventProcessing.Instance.ProcessingStarted += LogEvents;
            EventProcessing.Instance.ProcessingFinished += LogEvents;
      
        }
        private void EventDefinitions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e != null)
            {
                if(e.NewItems != null)
                {
                    foreach(var item in e.NewItems)
                    {
                        var def = ((EventDefinitionChild)item);
                        MainWindow.Tracker.TrackingEvents.Add(def);
                        
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        MainWindow.Tracker.TrackingEvents.Remove((EventDefinition)item);
                    }
                }
            }
        }

       
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        private void CalculateStartupLocation()
        {
            double cwidth = System.Windows.SystemParameters.PrimaryScreenWidth / 2;
            double cheight = System.Windows.SystemParameters.PrimaryScreenHeight / 2;
            double left = cwidth + 10;
            double height = cheight - this.Height / 2;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = left;
            this.Top = height;
        }
        public List<EventTypeWrapper> EventTypes { get; } = EventTypeWrapper.GETALL;

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName){
                case "EventTypeWrapper":
                    {
                        var text = ((DataGridTextColumn)e.Column);
                        var combo = new DataGridComboBoxColumn();
                        var b1 = new Binding("Event");
                        b1.Mode = BindingMode.TwoWay;
                        combo.SelectedValueBinding = b1;
                        combo.ItemsSource = EventTypes;
                        combo.DisplayMemberPath = "Display";
                        combo.SelectedValuePath = "EventType";
                        combo.SortMemberPath = "Name";
                        combo.Header = "EventType";
                        e.Column = combo;
                    break;
                }
            case "AssignRecursively":
                {
                        e.Column.Header = "Recursive";
                    break;
                }
            default:
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        private void FontColumnEditTemplateComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void ComboBoxColumn_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(sender is ComboBox)
            {
                var combo = (ComboBox)sender;
                combo.IsDropDownOpen = true;
            //    combo.ItemsSource = EventTypes.Where(p => p.Display.Contains(e.Text)).ToList();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (AddEventType == EventType.UNKNOWN) return;
            EventDefinitionChild eventDef = new EventDefinitionChild(AddEventType,AddIsRecursive,LogEvents);
            EventDefinitions.Add(eventDef);
            AddEventType = EventType.UNKNOWN;
            AddIsRecursive = false;
        }

        public void LogEvents(object sender, EventArgs e)
        {
            var logitem = new LogItem();
            switch (e.GetType().Name)
            {
                case "EventOccuredEventArgs":
                    {
                        logitem.TypeName = "EventOccured";
                        logitem.OccuredTime = ((EventOccuredEventArgs)e).CreatedTime;
                        logitem.Sender = ((EventOccuredEventArgs)e).CreatedContainer.Sender.StringRepresentation;
                        logitem.EventName = ((EventOccuredEventArgs)e).CreatedContainer.EventDefinition.EventName;
                        break;
                    }
                case "EventProcessingStartedEventArgs":
                    {
                        logitem.TypeName = "ProcessingStarted";
                        logitem.OccuredTime = ((EventProcessingStartedEventArgs)e).ProcessingStarted;
                        logitem.Sender = ((EventProcessingStartedEventArgs)e).EventArgsContainer.Sender.StringRepresentation;
                        logitem.EventName = ((EventProcessingStartedEventArgs)e).EventArgsContainer.EventDefinition.EventName;
                        break;
                    }
                case "EventProcessingFinishedEventArgs":
                    {
                        logitem.TypeName = "ProcessingFinished";
                        logitem.OccuredTime = ((EventProcessingFinishedEventArgs)e).ProcessingFinished;
                        logitem.Sender = ((EventProcessingFinishedEventArgs)e).EventArgsContainer.Sender.StringRepresentation;
                        logitem.EventName = ((EventProcessingFinishedEventArgs)e).EventArgsContainer.EventDefinition.EventName;
                        break;
                    }
                case "LogEventArgs":
                    {
                        logitem.TypeName = "Log";
                        logitem.OccuredTime = ((LogEventArgs)e).Time;
                        logitem.Sender = " - ";
                        logitem.EventName = " - ";

                        break;
                    }
                case "ErrorEventArgs":
                    {
                        logitem.TypeName = "SerialzeError";
                        logitem.OccuredTime = DateTime.Now;
                        logitem.Sender = " - ";
                        logitem.EventName = " - ";
                        break;
                    }
            }
            try
            {
                logitem.Value =  Sequence.Recorder.Tools.Functions.SerializeEventArgs(e,Settings);
            }catch(Exception ex)
            {
                logitem.Value = JsonConvert.SerializeObject(ex);
            }
            Dispatcher.InvokeAsync(() => {
                if (!LogItems.ContainsKey(logitem))
                {
                    LogItems.Add(logitem, logitem);
                    UpdateLogBinding();
                }
            });
        }
        private void UpdateLogBinding()
        {
            var be = BindingOperations.GetMultiBindingExpression(lvLogItems, ListView.ItemsSourceProperty);
            be.UpdateTarget();
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button)) return;
            var btn = sender as Button;
            if(btn.DataContext != null && (btn.DataContext is LogItem))
            {
                (btn.DataContext as LogItem).CalculateTokens();
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dia = new System.Windows.Forms.FolderBrowserDialog();
            dia.ShowNewFolderButton = true;
            dia.ShowDialog();
            if (!String.IsNullOrWhiteSpace(dia.SelectedPath))
            {
                Task.Run(() =>
                {
                    string path = System.IO.Path.Combine(dia.SelectedPath,$"EventData_{DateTime.Now.ToString("dd_MM_yyyy-hh_mm_ss_fff")}.json");
                    var suc = MainWindow.Storage.SaveMock(path,Settings);
                    if (suc)
                    {
                        Dispatcher.Invoke(() => MessageBox.Show($"Event Data from store mock was saved to: \r\n {path}","Saved!",MessageBoxButton.OK, MessageBoxImage.Information,MessageBoxResult.OK));
                    }
                    else
                    {
                        Dispatcher.Invoke(() => MessageBox.Show($"Event Data could not be stored to: \r\n {path} \r\n See Console output for details.", "Error while Saving!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK));
                    }
                });
            }
        }

        private void BtnDeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if(sender != null && (sender is Button))
            {
                var btn = sender as Button;
                if(btn.DataContext != null && btn.DataContext is EventDefinitionChild)
                {
                    EventDefinitions.Remove((EventDefinitionChild)btn.DataContext);
                }
            }
        }
    }


    public class LogItemsKey 
    {
        public DateTime Time { get; set; }

        public event EventHandler<EventArgs> TestEvent;

        private string _name;
        public string Name {
            get
            {
                return _name;

            }
            set {
                _name = value;
                if (_name.Equals("EventOccured"))
                {
                    Step = 0;
                }
                if (_name.Equals("ProcessingStarted"))
                {
                    Step = 1;
                }
                if (_name.Equals("ProcessingFinished"))
                {
                    Step = 2;
                }
            } }

        public LogItemsKey(DateTime time, string name)
        {
            Time = time;
            Name = name;
        }
        public LogItemsKey(LogItem item)
        {
            Time = item.OccuredTime;
            Name = item.TypeName;
        }
        public int Step { get; set; } = 3;
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is LogItemsKey)) return false;
            var k = obj as LogItemsKey;
            if (Time.Equals(k.Time) && Name.Equals(k.Name)) return true;
            return false;
        }
        public override int GetHashCode()
        {
            
            unchecked
            {
                int i = 0;
                i += Time.GetHashCode();
                i += Name?.GetHashCode() == null ? 0 : Name.GetHashCode();
                return i;
            }
            return 0;
        }

        public static implicit operator LogItemsKey(LogItem item)
        {
            return new LogItemsKey(item);
        }
    }

    public class LogItemsKeyComparer : Comparer<LogItemsKey>
    {
        public override int Compare(LogItemsKey x, LogItemsKey y)
        {
            if (x == null) return -1;
            if (y == null) return 1;
            if (x.Time.CompareTo(y.Time) != 0) return x.Time.CompareTo(y.Time);
            if (x.Step.CompareTo(y.Step) != 0) return x.Step.CompareTo(y.Step);
            return (x.Name.CompareTo(y.Name));
        }

    }
    
}

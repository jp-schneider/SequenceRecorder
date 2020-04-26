using Sequence.Recorder.Configuration;
using Sequence.Recorder.Enums;
using Sequence.Recorder.GUI;
using Sequence.Recorder.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Page = System.Windows.Controls.Page;

namespace Sequence.Test.UI
{
    /// <summary>
    /// Interaktionslogik für TestPage.xaml
    /// </summary>
    public partial class TestPage : Page
    {
        public int SelectedIndexValue { get; set; } = 0;
        public MainWindow MainWindow { get; set; }
        public TestPage() : base()
        {
            this.DataContext = this;
            InitializeComponent();
        }
        private void TbInsertText_KeyDown(object sender, KeyEventArgs e)
        {     
           // e.Handled = true;
        }

        public void Oc(object sender, EventOccuredEventArgs e)
        {

        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {

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
            get; set;
        } = LogType.Info;

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

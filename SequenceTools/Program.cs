using CsvHelper;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sequence.Recorder.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace Sequence.Tools
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            
            ConsoleKeyInfo key = default(ConsoleKeyInfo);
            do
            {
                Console.WriteLine("Press: \r\n" +
                "E for Enum generation,\r\n" +
                "and Esc for exit.");
                key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key == ConsoleKey.E)
                {
                    EnumGenerator.GenerateEventEnum();
                }

            } while (key.Key != ConsoleKey.Escape);

        }
    }
}

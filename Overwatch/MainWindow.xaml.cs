using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BackgroundApplication
{

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // UI
        private double Interval = 1000;
        private Timer Timer = new Timer();

        // Performance Counters
        private PerformanceCounter CPU = null;
        private PerformanceCounter RAM = null;


        #region Constructor

        public MainWindow()
        {
            Timer.Interval = Interval;
            Timer.Elapsed += OnTimerTick;
            Timer.Start();

            try
            {
                CPU = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                RAM = new PerformanceCounter("Memory", "Available MBytes");
            }
            catch (Exception e)
            {
                // TODO: Handle exception
                Debug.Print(e.Message);
            }

            Loaded += (o, e) => RefreshStaticValues();
        }
        
        #endregion


        /// <summary>
        /// Global refresh method for dynamic fields called on Timer tick
        /// </summary>
        private void RefreshDynamicValues()
        {
            Dispatcher.BeginInvoke((Action)(() => {
                if (CPU != null)
                    CPUFieldValue.Text = CPU.NextValue().ToString("00.00") + " %";
                else
                    CPUFieldValue.Text = "??";
            }));

            Dispatcher.BeginInvoke((Action)(() => {
                if (RAM != null)
                    RAMFieldValue.Text = RAM.NextValue().ToString() + " Mbytes";
                else
                    RAMFieldValue.Text = "??";
            }));
        }
        
        /// <summary>
        /// Global refresh method for static fields called on demand
        /// </summary>
        private void RefreshStaticValues()
        {
            MachineNameFieldValue.Text = Environment.MachineName;
        }


        #region Callbacks

        /// <summary>
        /// Called every time Timer has ticked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            // Stop Timer so it doesn't run while computing
            Timer.Stop();

            RefreshDynamicValues();

            // Start it again
            Timer.Start();
        }

        /// <summary>
        /// Handle drag action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        /// <summary>
        /// Handle close action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Close(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        #endregion

    }

}

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
        private PerformanceCounter CPU = new PerformanceCounter("Processor", "% Processor Time", "_Total");


        #region Constructor

        public MainWindow()
        {
            Timer.Interval = Interval;
            Timer.Elapsed += OnTimerTick;
            Timer.Start();
        }

        #endregion



        private void RefreshValues()
        {
            Dispatcher.BeginInvoke((Action)(() => { CPUFieldValue.Text = CPU.NextValue().ToString("00.00") + " %"; }));
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

            RefreshValues();

            // Start it again
            Timer.Start();
        }

        /// <summary>
        /// Handle drag action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        #endregion

    }

}

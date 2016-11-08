using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SevenAstro2.Views
{
    /// <summary>
    /// Interaction logic for TransitPartial.xaml
    /// </summary>
    internal partial class TransitPartial : UserControl
    {
        DispatcherTimer _transientTimer;

        public TransitPartial()
        {
            InitializeComponent();

            _transientTimer = new DispatcherTimer();
            _transientTimer.Tick += TransientTimerTickAct;
        }

        SevenAstro2.Models.CircumstanceViewModel Data { get { return this.DataContext as SevenAstro2.Models.CircumstanceViewModel; } }

        private void StopTransient()
        {
            imgBtnStartStop.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("../Assets/Images/start.png", UriKind.Relative));

            _transientTimer.Stop();

            if (Data != null) Data.TransientStarted = false;
        }

        private void StartTransient()
        {
            imgBtnStartStop.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("../Assets/Images/stop.png", UriKind.Relative));

            _transientTimer.Interval = TimeSpan.FromSeconds(1);
            _transientTimer.Start();

            if (Data != null) Data.TransientStarted = true;
        }

        void TransientTimerTickAct(object sender, EventArgs e)
        {
            if (Data != null)
            {
                Data.UpdateTransient();
            }
        }

        private void btnSetTransientTime_Click(object sender, RoutedEventArgs e)
        {
            if (Data != null) Data.TransientDateTime = Data.TransientDateTime;

            StopTransient();

            if (Data != null)
            {
                Data.TransientStep = 0;
                Data.UpdateTransient();
            }
        }

        private void btnRealTime_Click(object sender, RoutedEventArgs e)
        {
            if (Data != null) Data.TransientStep = 1;
        }

        private void btnMinutely_Click(object sender, RoutedEventArgs e)
        {
            if (Data != null) Data.TransientStep = 60;
        }

        private void btnHourly_Click(object sender, RoutedEventArgs e)
        {
            if (Data != null) Data.TransientStep = 3600;
        }

        private void btnDaily_Click(object sender, RoutedEventArgs e)
        {
            if (Data != null) Data.TransientStep = 3600 * 24;
        }

        private void btnWeekly_Click(object sender, RoutedEventArgs e)
        {
            if (Data != null) Data.TransientStep = 3600 * 24 * 7;
        }

        private void btnBackward1_Click(object sender, RoutedEventArgs e)
        {
            if (Data != null)
            {
                Data.Direction = -1;
                Data.UpdateTransient();
            }
        }

        private void btnBackward_Click(object sender, RoutedEventArgs e)
        {
            if (Data != null) Data.Direction = -1;
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (Data != null) Data.Direction = 1;
        }

        private void btnForward1_Click(object sender, RoutedEventArgs e)
        {
            if (Data != null)
            {
                Data.Direction = 1;
                Data.UpdateTransient();
            }
        }

        private void btnStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (Data != null)
                if (!Data.TransientStarted)
                {
                    StartTransient();
                }
                else
                {
                    StopTransient();
                }
        }

        private void btnMatchPoint_Click(object sender, RoutedEventArgs e)
        {
            StopTransient();

            if (Data != null)
            {
                Data.TransientStep = 0;

                double same = 0;
                same = GetTheSame(same);
                Data.MatchDegree = same;

                var time = Calculations.Supplementary.Kernel.Converge1(
                    Data.MatchPlanet,
                    Data.MatchLongitude,
                    new Calculations.Supplementary.Event(
                        Data.TransientDateTime,//Fex.ToUT(_birthData.VDateTime, _birthData.VTimezone, _birthData.VDST),
                        new Calculations.Position(Data.BirthData.VLongitude, Data.BirthData.VLatitude, 0),
                        Global.Conf));

                Data.TransientDateTime = Global.FromUT(time, Data.BirthData.VTimezone, Data.BirthData.VDST);

                Data.UpdateTransient();

                var sameDT = DateTime.Now;
                sameDT = GetTheSame(sameDT);
                Data.TransientDateTime = sameDT;
            }
        }

        private DateTime GetTheSame(DateTime sameDT)
        {
            sameDT = Data.TransientDateTime;
            return sameDT;
        }

        private double GetTheSame(double same)
        {
            same = Data.MatchDegree;
            return same;
        }
    }
}

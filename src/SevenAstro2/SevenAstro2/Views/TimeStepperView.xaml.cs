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
using System.Windows.Shapes;

namespace SevenAstro2.Views
{
    /// <summary>
    /// Interaction logic for TimeStepperView.xaml
    /// </summary>
    internal partial class TimeStepperView : Window
    {
        public TimeStepperView()
        {
            InitializeComponent();
        }

        Models.CircumstanceViewModel Data { get { return this.DataContext as Models.CircumstanceViewModel; } }

        private void btnDecreaseTime_Click(object sender, RoutedEventArgs e)
        {
            if (Data == null) return;

            Data.DasaDeep = 1;

            var btn = sender as Button;
            if (btn != null)
            {
                //_dasaDeep = 1;
                try
                {
                    btn.IsEnabled = false;

                    int i = 0;
                    int.TryParse((btn.CommandParameter ?? string.Empty).ToString(), out i);
                    var ndat = Data.BirthData.VDateTime.AddSeconds(i * -1);
                    Data.BirthData.Date = ndat.Date;
                    Data.BirthData.Time = string.Format("{0:HH:mm:ss}", ndat);

                    Data.Update();
                }
                finally { btn.IsEnabled = true; }
            }
        }

        private void btnIncreaseTime_Click(object sender, RoutedEventArgs e)
        {
            if (Data == null) return;

            Data.DasaDeep = 1;

            var btn = sender as Button;
            if (btn != null)
            {
                //_dasaDeep = 1;

                try
                {
                    btn.IsEnabled = false;

                    int i = 0;
                    int.TryParse((btn.CommandParameter ?? string.Empty).ToString(), out i);
                    var ndat = Data.BirthData.VDateTime.AddSeconds(i);
                    Data.BirthData.Date = ndat.Date;
                    Data.BirthData.Time = string.Format("{0:HH:mm:ss}", ndat);

                    Data.Update();
                }
                finally { btn.IsEnabled = true; }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Global.TimeStepperView = null;
        }
    }
}

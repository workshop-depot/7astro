using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SevenAstro2
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    internal partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();

            this.Topmost = false;
            var t = new Task(() =>
            {
                var startedAt = DateTime.Now;
                
                var passed = DateTime.Now - startedAt;

                if (passed.TotalSeconds < 1.5) System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1.5 - passed.TotalSeconds));
            });
            t.ContinueWith(tc =>
            {
                this.Topmost = true;

                var main = new MainWindow();
                main.Show();

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.5));

                this.Close();
            }, TaskScheduler.FromCurrentSynchronizationContext());

            t.Start();
        }
    }
}

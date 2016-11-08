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

namespace SevenAstro2.Views
{
    /// <summary>
    /// Interaction logic for DasaPartial.xaml
    /// </summary>
    internal partial class DasaPartial : UserControl
    {
        public DasaPartial()
        {
            InitializeComponent();
        }

        void Scroll()
        {
            try
            {
                var dasas = dgDasaa.ItemsSource as System.Collections.ObjectModel.ObservableCollection<Models.Dasa>;

                if (dasas != null)
                {
                    dgDasaa.SelectedIndex = -1;
                    int index = 0;
                    foreach (var dasa in dasas)
                    {
                        var parts = dasa.Date.Split('/');
                        if (parts.Length == 3)
                        {
                            int day = 0;
                            int month = 0;
                            int year = 0;

                            int.TryParse(parts[0], out day);
                            int.TryParse(parts[1], out month);
                            int.TryParse(parts[2], out year);

                            var date = new DateTime(year, month, day);

                            if (date > DateTime.Now)
                            {
                                dgDasaa.SelectedIndex = index - 1;
                                dgDasaa.ScrollIntoView(dgDasaa.SelectedItems[0]);
                                break;
                            }
                        }

                        index++;
                    }
                }

                //RefreshUI(dasaDeep);
            }
            catch { }
            finally { }
        }

        int _dasaDeep = 1;
        //public int DasaDeep { get { return _dasaDeep; } }

        private void btnCalculateDasa_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            if (btn != null)
            {
                int.TryParse(btn.CommandParameter.ToString(), out _dasaDeep);

                //RaiseDasaDeepChangedEvent();

                DasaDeep = _dasaDeep;

                //ServiceBus.CurrentCircumstance.DasaDeep = _dasaDeep;
                //ServiceBus.CurrentCircumstance.Update();

                Dispatcher.Invoke(new Action(() =>
                {
                    //System.Threading.Thread.Sleep(1000);

                    Scroll();
                }), System.Windows.Threading.DispatcherPriority.Input);
            }
        }

        //void RaiseDasaDeepChangedEvent()
        //{
        //    RoutedEventArgs newEventArgs = new RoutedEventArgs(DasaPartial.DasaDeepChangedEvent, this);
        //    RaiseEvent(newEventArgs);
        //}

        public System.Collections.ObjectModel.ObservableCollection<Models.Dasa> Dasa
        {
            get { return (System.Collections.ObjectModel.ObservableCollection<Models.Dasa>)GetValue(DasaProperty); }
            set { SetValue(DasaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Dasa.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DasaProperty =
            DependencyProperty.Register("Dasa", typeof(System.Collections.ObjectModel.ObservableCollection<Models.Dasa>), typeof(DasaPartial));//, new FrameworkPropertyMetadata(Changed));

        public int DasaDeep
        {
            get { return (int)GetValue(DasaDeepProperty); }
            set { SetValue(DasaDeepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DasaDeep.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DasaDeepProperty =
            DependencyProperty.Register("DasaDeep", typeof(int), typeof(DasaPartial));

        //static void Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        //{
        //    var ctrl = dependencyObject as DasaPartial;
        //    //ctrl.Scroll();

        //    ctrl.Dispatcher.Invoke(new Action(() =>
        //    {
        //        ctrl.Scroll();
        //    }), System.Windows.Threading.DispatcherPriority.Input);
        //}

        //public event RoutedEventHandler DasaDeepChanged
        //{
        //    add { AddHandler(DasaDeepChangedEvent, value); }
        //    remove { RemoveHandler(DasaDeepChangedEvent, value); }
        //}

        //public static readonly RoutedEvent DasaDeepChangedEvent = EventManager.RegisterRoutedEvent(
        //    "DasaDeepChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DasaPartial));
    }
}

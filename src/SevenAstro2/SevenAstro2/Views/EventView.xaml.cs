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
    /// Interaction logic for EventView.xaml
    /// </summary>
    internal partial class EventView : Page
    {
        public Models.CircumstanceViewModel Event;
        public bool Changed = false;
        public bool Saved = true;

        public EventView()
        {
            InitializeComponent();

            Event = new Models.CircumstanceViewModel();
            Event.Updated += Event_Updated;

            this.DataContext = Event;

            Event.Update();
        }

        void Event_Updated(object sender, EventArgs e)
        {
            Changed = true;
            Saved = false;
        }

        public void SetAsJustOpened()
        {
            Changed = false;
            Saved = true;
        }

        //private void PartialDasa_DasaDeepChanged(object sender, RoutedEventArgs e)
        //{
        //    var dasaPartial = sender as DasaPartial;

        //    if (dasaPartial != null)
        //    {
        //        //this.Event.ApplyUpdate = false;
        //        this.Event.DasaDeep = dasaPartial.DasaDeep;
        //        //this.Event.Update();
        //        //this.Event.ApplyUpdate = true;
        //    }
        //}
    }
}

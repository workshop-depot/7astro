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
    /// Interaction logic for ConvertDateView.xaml
    /// </summary>
    internal partial class ConvertDateView : Window
    {
        public ConvertDateView()
        {
            InitializeComponent();

            this.DataContext = new SevenAstro2.Models.ConvertDateViewModel();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Global.ConvertDateView = null;
        }
    }
}

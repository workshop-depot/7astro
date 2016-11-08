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
    /// Interaction logic for JDPartial.xaml
    /// </summary>
    internal partial class JDPartial : UserControl
    {
        public JDPartial()
        {
            InitializeComponent();
        }

        private void btnCalculateJD_Click(object sender, RoutedEventArgs e)
        {
            var data = this.DataContext as Models.CircumstanceViewModel;

            if (data != null)
            {
                data.UpdateJD();
            }
        }
    }
}

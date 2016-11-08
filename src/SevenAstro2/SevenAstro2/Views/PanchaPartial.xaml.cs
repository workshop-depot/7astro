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
    /// Interaction logic for PanchaPartial.xaml
    /// </summary>
    internal partial class PanchaPartial : UserControl
    {
        public PanchaPartial()
        {
            InitializeComponent();

            this.dgPlanetData.DataContextChanged += dgPlanetData_DataContextChanged;
        }

        void dgPlanetData_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        void PanchaPartial_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}

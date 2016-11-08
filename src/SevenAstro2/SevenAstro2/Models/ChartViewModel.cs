using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenAstro2.Models
{
    class ChartViewModel : ModelBase
    {
        SevenAstro2.Calculations.Supplementary.VdChart _chart;
        bool _showDegree;

        public SevenAstro2.Calculations.Supplementary.VdChart Chart
        {
            get { return _chart; }
            set { Set(ref _chart, value, "Chart"); }
        }

        public bool ShowDegree
        {
            get { return _showDegree; }
            set { Set(ref _showDegree, value, "ShowDegree"); }
        }
    }
}

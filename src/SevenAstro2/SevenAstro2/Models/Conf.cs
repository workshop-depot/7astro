using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SevenAstro2.Models
{
    class Conf : ModelBase
    {
        Calculations.Supplementary.NodeType _nodeType = Calculations.Supplementary.NodeType.TrueNode;

        public Calculations.Supplementary.NodeType NodeType
        {
            get { return _nodeType; }
            set { Set(ref _nodeType, value,"NodeType"); }
        }

        public string[] NodeTypes
        {
            get
            {
                return new string[] 
                {  
                    Calculations.Supplementary.NodeType.TrueNode.ToString(), 
                    Calculations.Supplementary.NodeType.MeanNode.ToString() 
                };
            }
        }
    }
}

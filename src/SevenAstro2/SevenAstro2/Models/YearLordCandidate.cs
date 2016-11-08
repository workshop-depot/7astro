using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenAstro2.Models
{
    class YearLordCandidate : ModelBase
    {
        string _name;
        string _memo;

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value, "Name"); }
        }

        public string Memo
        {
            get { return _memo; }
            set { Set(ref _memo, value, "Memo"); }
        }
    }
}

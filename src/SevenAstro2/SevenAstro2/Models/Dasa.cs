using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenAstro2.Models
{
    class Dasa : ModelBase
    {
        string _dasa;
        string _date;
        string _time;
        string _persianDate;

        public string DasaMark
        {
            get { return _dasa; }
            set { Set(ref _dasa, value, "DasaMark"); }
        }

        public string Date
        {
            get { return _date; }
            set { Set(ref _date, value, "Date"); }
        }

        public string Time
        {
            get { return _time; }
            set { Set(ref _time, value, "Time"); }
        }

        public string PersianDate
        {
            get { return _persianDate; }
            set { Set(ref _persianDate, value, "PersianDate"); }
        }
    }
}

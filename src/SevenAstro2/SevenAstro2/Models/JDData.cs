using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SevenAstro2.Models
{
    class JDData : ModelBase
    {
        DateTime _date;
        string _time;

        public DateTime Date
        {
            get { return _date; }
            set
            {
                Set(ref _date, value, "Date");
            }
        }

        public string Time
        {
            get { return _time; }
            set
            {
                var m = Regex.Match(value, "(?<hour>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*");

                if (!m.Success) throw new ApplicationException();

                Set(ref _time, value, "Time");
            }
        }

        public DateTime VDateTime
        {
            get
            {
                var m = Regex.Match(_time, "(?<hour>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*");

                if (!m.Success) throw new ApplicationException();

                int hour = int.Parse(m.Groups["hour"].Value);
                int minute = int.Parse(m.Groups["minute"].Value);
                int second = 0;
                if (m.Groups["second"].Success) int.TryParse(m.Groups["second"].Value, out second);

                var ts = new TimeSpan(hour, minute, second);

                return this._date.Date.Add(ts);
            }
        }
    }
}

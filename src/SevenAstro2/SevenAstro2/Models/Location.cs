using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SevenAstro2.Models
{
    class Location : ModelBase
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value, "Name"); }
        }

        string _longitude;
        public string Longitude
        {
            get { return _longitude; }
            set
            {
                var m = Regex.Match(value, "^(?<sign>[+-]*)(?<hour>\\d{1,3}):(?<minute>\\d{1,2})(:(?<second>\\d{1,2}))*$");

                if (!m.Success) throw new ApplicationException();

                int hour = 0;
                int minute = 0;
                int second = 0;

                try
                {
                    hour = int.Parse(m.Groups["hour"].Value);
                    minute = int.Parse(m.Groups["minute"].Value);
                    second = 0;
                    if (m.Groups["second"].Success)
                        second = int.Parse(m.Groups["second"].Value);
                    var sign = m.Groups["sign"].Value;
                }
                catch { throw new ApplicationException(); }

                if (hour < 0 || 180 < hour) throw new ApplicationException();
                if (minute < 0 || 59 < minute) throw new ApplicationException();
                if (second < 0 || 59 < second) throw new ApplicationException();

                Set(ref _longitude, value, "Longitude");
            }
        }

        string _latitude;
        public string Latitude
        {
            get { return _latitude; }
            set
            {
                var m = Regex.Match(value, "^(?<sign>[+-]*)(?<hour>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*$");

                if (!m.Success) throw new ApplicationException();

                int hour = 0;
                int minute = 0;
                int second = 0;

                try
                {
                    hour = int.Parse(m.Groups["hour"].Value);
                    minute = int.Parse(m.Groups["minute"].Value);
                    second = 0;
                    if (m.Groups["second"].Success)
                        second = int.Parse(m.Groups["second"].Value);
                    var sign = m.Groups["sign"].Value;
                }
                catch { throw new ApplicationException(); }

                if (hour < 0 || 180 < hour) throw new ApplicationException();
                if (minute < 0 || 59 < minute) throw new ApplicationException();
                if (second < 0 || 59 < second) throw new ApplicationException();

                Set(ref _latitude, value, "Latitude");
            }
        }

        string _timezone;
        public string Timezone
        {
            get { return _timezone; }
            set
            {
                var m = Regex.Match(value, "^(?<sign>[+-]*)(?<hour>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*$");

                if (!m.Success) throw new ApplicationException();

                int hour = 0;
                int minute = 0;
                int second = 0;

                try
                {
                    hour = int.Parse(m.Groups["hour"].Value);
                    minute = int.Parse(m.Groups["minute"].Value);
                    second = 0;
                    if (m.Groups["second"].Success)
                        int.TryParse(m.Groups["second"].Value, out second);
                }
                catch { throw new ApplicationException(); }

                if (hour < 0 || 23 < hour) throw new ApplicationException();
                if (minute < 0 || 59 < minute) throw new ApplicationException();
                if (second < 0 || 59 < second) throw new ApplicationException();

                Set(ref _timezone, value, "Timezone");
            }
        }

        string _dst;
        public string DST
        {
            get { return _dst; }
            set
            {
                var m = Regex.Match(value, "^(?<sign>[+-]*)(?<hour>\\d+)(:(?<minute>\\d+):(?<second>\\d+))*$");

                if (!m.Success) throw new ApplicationException();

                Set(ref _dst, value, "DST");
            }
        }

        public TimeSpan VTimezone
        {
            get
            {
                var m = Regex.Match(_timezone, "^(?<sign>[+-]*)(?<hour>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*$");

                if (!m.Success) throw new ApplicationException();

                int hour = int.Parse(m.Groups["hour"].Value);
                int minute = int.Parse(m.Groups["minute"].Value);
                int second = 0;
                if (m.Groups["second"].Success)
                    second = int.Parse(m.Groups["second"].Value);
                var sign = m.Groups["sign"].Value;

                var ts = new TimeSpan(hour, minute, second);

                if (sign == "-") return TimeSpan.FromHours(ts.TotalHours * -1);
                return ts;
            }
        }

        public TimeSpan VDST
        {
            get
            {
                var m = Regex.Match(_dst, "^(?<sign>[+-]*)(?<hour>\\d+)(:(?<minute>\\d+):(?<second>\\d+))*$");

                if (!m.Success) throw new ApplicationException();

                int hour = int.Parse(m.Groups["hour"].Value);
                int minute = 0;
                if (m.Groups["minute"].Success)
                    minute = int.Parse(m.Groups["minute"].Value);
                int second = 0;
                if (m.Groups["second"].Success)
                    second = int.Parse(m.Groups["second"].Value);
                var sign = m.Groups["sign"].Value;

                var ts = new TimeSpan(hour, minute, second);

                if (sign == "-") return TimeSpan.FromHours(ts.TotalHours * -1);
                return ts;
            }
        }

        public double VLongitude
        {
            get
            {
                var m = Regex.Match(_longitude, "^(?<sign>[+-]*)(?<hour>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*$");

                if (!m.Success) throw new ApplicationException();

                int hour = int.Parse(m.Groups["hour"].Value);
                int minute = int.Parse(m.Groups["minute"].Value);
                int second = 0;
                if (m.Groups["second"].Success)
                    second = int.Parse(m.Groups["second"].Value);
                var sign = m.Groups["sign"].Value;

                var ts = TimeSpan.FromHours((new TimeSpan(hour, minute, second).TotalHours).Range(-180, 180));

                if (sign == "-") return ts.TotalHours * -1d;
                return ts.TotalHours;
            }
        }

        public double VLatitude
        {
            get
            {
                var m = Regex.Match(_latitude, "^(?<sign>[+-]*)(?<hour>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*$");

                if (!m.Success) throw new ApplicationException();

                int hour = int.Parse(m.Groups["hour"].Value);
                int minute = int.Parse(m.Groups["minute"].Value);
                int second = 0;
                if (m.Groups["second"].Success)
                    second = int.Parse(m.Groups["second"].Value);
                var sign = m.Groups["sign"].Value;

                var ts = TimeSpan.FromHours((new TimeSpan(hour, minute, second).TotalHours).Range(-180, 180));

                if (sign == "-") return ts.TotalHours * -1d;
                return ts.TotalHours;
            }
        }

        public override string ToString()
        {
            return string.Join(",", Name, Longitude, Latitude, Timezone, DST);
        }

        public object[] Fields { get { return new object[] { Name, Longitude, Latitude, Timezone, DST }; } }

        public static Location From(string[] vals)
        {
            try
            {
                var loc = new Location();

                loc.Name = vals[0];
                loc.Longitude = vals[1];
                loc.Latitude = vals[2];
                loc.Timezone = vals[3];
                loc.DST = vals[4];

                return loc;
            }
            catch { return null; }
        }
    }
}

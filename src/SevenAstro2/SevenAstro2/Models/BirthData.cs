using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SevenAstro2.Models
{
    class BirthData : ModelBase
    {
        string _name;
        DateTime _date;
        string _time;
        string _timezone;
        string _dst;
        string _longitude;
        string _latitude;
        string _birthPlace;
        Gender _gender = Gender.Female;
        string _notes;

        public string Name
        {
            get { return _name; }
            set
            {
                var v = value.Text();

                var m = Regex.Match(v, "^[\\w ]+$");

                if (!m.Success)
                {
                    //throw new ApplicationException();
                    v = "New";
                }

                Set(ref _name, v, "Name");
            }
        }

        public Gender Gender
        {
            get { return _gender; }
            set { Set(ref _gender, value, "Gender"); }
        }

        [JsonIgnore]
        public string[] Genders
        {
            get { return new string[] { Gender.Female.ToString(), Gender.Male.ToString() }; }
        }

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
                var m = Regex.Match(value, "^(?<hour>\\d{1,2}):(?<minute>\\d{1,2})(:(?<second>\\d{1,2}))*$");

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

                var ts = new TimeSpan(hour, minute, second);

                Set(ref _time, value, "Time");
            }
        }

        public string BirthPlace
        {
            get { return _birthPlace; }
            set { Set(ref _birthPlace, value, "BirthPlace"); }
        }

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

                Set(ref _longitude, value, "Longitude", "Lon");
            }
        }

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

                Set(ref _latitude, value, "Latitude", "Lat");
            }
        }

        public string Notes
        {
            get { return _notes; }
            set { Set(ref _notes, value, "Notes"); }
        }

        [JsonIgnore]
        public DateTime VDateTime
        {
            get
            {
                var m = Regex.Match(_time, "^(?<hour>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*$");

                if (!m.Success) throw new ApplicationException();

                int hour = int.Parse(m.Groups["hour"].Value);
                int minute = int.Parse(m.Groups["minute"].Value);
                int second = 0;
                if (m.Groups["second"].Success)
                    int.TryParse(m.Groups["second"].Value, out second);

                var ts = new TimeSpan(hour, minute, second);

                return this._date.Date.Add(ts);
            }
        }

        [JsonIgnore]
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

        [JsonIgnore]
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

        [JsonIgnore]
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

        [JsonIgnore]
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

        //

        [JsonIgnore]
        public string Lon
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

                return string.Format("{0:000}{1}{2:00}:{3:00}", hour, sign == "-" ? "W" : "E", minute, second);
            }
            set
            {
                var m = Regex.Match(value, "^(?<hour>\\d{1,3})(?<dir>[E|e|W|w])(?<minute>\\d{1,2})(:(?<second>\\d{1,2}))*$");

                if (!m.Success) throw new ApplicationException();

                int hour = 0;
                int minute = 0;
                int second = 0;
                var sign = string.Empty;

                try
                {
                    hour = int.Parse(m.Groups["hour"].Value);
                    minute = int.Parse(m.Groups["minute"].Value);
                    second = 0;
                    if (m.Groups["second"].Success)
                        second = int.Parse(m.Groups["second"].Value);
                    sign = m.Groups["dir"].Value.Text().ToUpper() == "E" ? "+" : "-";
                }
                catch { throw new ApplicationException(); }

                if (hour < 0 || 180 < hour) throw new ApplicationException();
                if (minute < 0 || 59 < minute) throw new ApplicationException();
                if (second < 0 || 59 < second) throw new ApplicationException();

                var val = string.Format("{0}{1:000}:{2:00}:{3:00}", sign, hour, minute, second);

                Set(ref _longitude, val, "Lon", "Longitude");
            }
        }

        [JsonIgnore]
        public string Lat
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

                return string.Format("{0:00}{1}{2:00}:{3:00}", hour, sign == "-" ? "S" : "N", minute, second);
            }
            set
            {
                var m = Regex.Match(value, "^(?<hour>\\d{1,3})(?<dir>[S|s|N|n])(?<minute>\\d{1,2})(:(?<second>\\d{1,2}))*$");

                if (!m.Success) throw new ApplicationException();

                int hour = 0;
                int minute = 0;
                int second = 0;
                var sign = string.Empty;

                try
                {
                    hour = int.Parse(m.Groups["hour"].Value);
                    minute = int.Parse(m.Groups["minute"].Value);
                    second = 0;
                    if (m.Groups["second"].Success)
                        second = int.Parse(m.Groups["second"].Value);
                    sign = m.Groups["dir"].Value.Text().ToUpper() == "N" ? "+" : "-";
                }
                catch { throw new ApplicationException(); }

                if (hour < 0 || 180 < hour) throw new ApplicationException();
                if (minute < 0 || 59 < minute) throw new ApplicationException();
                if (second < 0 || 59 < second) throw new ApplicationException();

                var val = string.Format("{0}{1:00}:{2:00}:{3:00}", sign, hour, minute, second);

                Set(ref _latitude, val, "Lat", "Latitude");
            }
        }
    }

    enum Gender
    {
        Female,
        Male
    }
}

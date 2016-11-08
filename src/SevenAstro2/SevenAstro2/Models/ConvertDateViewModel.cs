using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace SevenAstro2.Models
{
    class ConvertDateViewModel : ModelBase
    {
        private string _persianDate;
        public string PersianDate
        {
            get { return _persianDate; }
            set
            {
                var m = Regex.Match(value, "^(?<day>\\d{1,2})/(?<month>\\d{1,2})/(?<year>\\d\\d\\d\\d)$");

                if (!m.Success) throw new ApplicationException();

                int day = 0;
                int month = 0;
                int year = 0;

                try
                {
                    day = int.Parse(m.Groups["day"].Value);
                    month = int.Parse(m.Groups["month"].Value);
                    year = int.Parse(m.Groups["year"].Value);
                }
                catch { throw new ApplicationException(); }

                if (day < 1 || 31 < day) throw new ApplicationException();
                if (month < 1 || 12 < month) throw new ApplicationException();

                var pc = new System.Globalization.PersianCalendar();

                try { pc.ToDateTime(year, month, day, 0, 0, 0, 0); }
                catch { throw new ApplicationException(); }

                Set(ref _persianDate, value, "PersianDate");
            }
        }

        private string _gregDate;
        public string GregDate
        {
            get { return _gregDate; }
            set
            {
                var m = Regex.Match(value, "^(?<day>\\d{1,2})/(?<month>\\d{1,2})/(?<year>\\d\\d\\d\\d)$");

                if (!m.Success) throw new ApplicationException();

                int day = 0;
                int month = 0;
                int year = 0;

                try
                {
                    day = int.Parse(m.Groups["day"].Value);
                    month = int.Parse(m.Groups["month"].Value);
                    year = int.Parse(m.Groups["year"].Value);
                }
                catch { throw new ApplicationException(); }

                if (day < 1 || 31 < day) throw new ApplicationException();
                if (month < 1 || 12 < month) throw new ApplicationException();

                try { var _ = new DateTime(year, month, day); }
                catch { throw new ApplicationException(); }

                Set(ref _gregDate, value, "GregDate");
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { Set(ref _message, value, "Message"); }
        }

        void PersToGreg(object o)
        {
            Message = string.Empty;
            var m = Regex.Match(_persianDate, "^(?<day>\\d{1,2})/(?<month>\\d{1,2})/(?<year>\\d\\d\\d\\d)$");

            if (!m.Success)
            {
                Message = "Persian date has an invalid format.";
                return;
            }

            int day = 0;
            int month = 0;
            int year = 0;

            try
            {
                day = int.Parse(m.Groups["day"].Value);
                month = int.Parse(m.Groups["month"].Value);
                year = int.Parse(m.Groups["year"].Value);
            }
            catch
            {
                Message = "Persian date has an invalid format.";
                return;
            }

            if (day < 1 || 31 < day)
            {
                Message = "Persian date day must be between 1 and 31.";
                return;
            }
            if (month < 1 || 12 < month)
            {
                Message = "Persian date month must be between 1 and 12.";
                return;
            }

            var pc = new System.Globalization.PersianCalendar();

            var date = DateTime.Now;
            try { date = pc.ToDateTime(year, month, day, 0, 0, 0, 0); }
            catch
            {
                Message = "Persian date is not valid.";
                return;
            }

            GregDate = string.Format("{0:dd/MM/yyyy}", date);
        }

        void GregToPers(object o)
        {
            Message = string.Empty;
            var m = Regex.Match(_gregDate, "^(?<day>\\d{1,2})/(?<month>\\d{1,2})/(?<year>\\d\\d\\d\\d)$");

            if (!m.Success)
            {
                Message = "Gregorian date has an invalid format.";
                return;
            }

            int day = 0;
            int month = 0;
            int year = 0;

            try
            {
                day = int.Parse(m.Groups["day"].Value);
                month = int.Parse(m.Groups["month"].Value);
                year = int.Parse(m.Groups["year"].Value);
            }
            catch
            {
                Message = "Gregorian date has an invalid format.";
                return;
            }

            if (day < 1 || 31 < day)
            {
                Message = "Gregorian date day must be between 1 and 31.";
                return;
            }
            if (month < 1 || 12 < month)
            {
                Message = "Gregorian date month must be between 1 and 12.";
                return;
            }

            var date = DateTime.Now;
            try { date = new DateTime(year, month, day); }
            catch
            {
                Message = "Gregorian date is not valid.";
                return;
            }

            var pc = new System.Globalization.PersianCalendar();
            PersianDate = string.Format("{0}/{1}/{2}", pc.GetDayOfMonth(date), pc.GetMonth(date), pc.GetYear(date));
        }

        ICommand _PersToGregCommand;
        public ICommand PersToGregCommand
        {
            get
            {
                if (_PersToGregCommand == null)
                {
                    _PersToGregCommand = new RelayCommand(o => { PersToGreg(o); });
                }

                return _PersToGregCommand;
            }
        }

        ICommand _GregToPersCommand;
        public ICommand GregToPersCommand
        {
            get
            {
                if (_GregToPersCommand == null)
                {
                    _GregToPersCommand = new RelayCommand(o => { GregToPers(o); });
                }

                return _GregToPersCommand;
            }
        }
    }
}

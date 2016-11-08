using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenAstro2.Calculations
{
    struct Time
    {
        public Time(
            int year,
            int month,
            int day,
            double hour)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = Math.Floor(hour * 1000000d) / 1000000d;
        }

        public readonly int Year;
        public readonly int Month;
        public readonly int Day;
        public readonly double Hour;
    }
}

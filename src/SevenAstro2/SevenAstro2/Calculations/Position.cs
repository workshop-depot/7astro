using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenAstro2.Calculations
{
    struct Position
    {
        public Position(
            double longitude,
            double latitude,
            double altitude)
        {
            Longitude = longitude;
            Latitude = latitude;
            Altitude = altitude;
        }

        public readonly double Longitude;
        public readonly double Latitude;
        public readonly double Altitude;

        public override string ToString()
        {
            return new { Longitude, Latitude, Altitude }.ToString();
        }
    }
}

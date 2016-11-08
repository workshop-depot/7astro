using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenAstro2.Calculations.Primary
{
    /// <summary>
    /// in normal condition: 
    /// we use either IPL or Name to identify the Point;
    /// use IPL for bodies and Name for stars
    /// </summary>
    class Point
    {
        private Point() { }
        /// <summary>
        /// in normal condition: 
        /// we use either IPL or Name to identify the Point;
        /// use IPL for bodies and Name for stars
        /// </summary>
        /// <param name="ipl"></param>
        /// <param name="name"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="distance"></param>
        /// <param name="speedInLongitude"></param>
        /// <param name="speedInLatitude"></param>
        /// <param name="speedInDistance"></param>
        public Point(
            int? ipl,
            StringBuilder name,
            double longitude,
            double latitude,
            double distance,
            double speedInLongitude,
            double speedInLatitude,
            double speedInDistance)
        {
            IPL = ipl;
            Name = name;
            Longitude = longitude;
            Latitude = latitude;
            Distance = distance;
            SpeedInLongitude = speedInLongitude;
            SpeedInLatitude = speedInLatitude;
            SpeedInDistance = speedInDistance;
        }

        public int? IPL { get; private set; }
        public StringBuilder Name { get; private set; }

        public double Longitude { get; private set; }
        public double Latitude { get; private set; }
        /// <summary>
        /// Distance in AU
        /// </summary>
        public double Distance { get; private set; }
        /// <summary>
        /// Speed in longitude (deg/day)
        /// </summary>
        public double SpeedInLongitude { get; private set; }
        /// <summary>
        /// Speed in latitude (deg/day)
        /// </summary>
        public double SpeedInLatitude { get; private set; }
        /// <summary>
        /// Speed in distance (AU/day)
        /// </summary>
        public double SpeedInDistance { get; private set; }
    }
}

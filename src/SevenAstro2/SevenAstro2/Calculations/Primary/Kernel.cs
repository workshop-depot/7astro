using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenAstro2.Calculations.Primary
{
    partial class Kernel : IDisposable
    {
        Time _time;
        Position _position;

        int _iflag;
        int _houseSystem;

        double _julday_ut;

        double[] _cusps;
        double[] _ascmc;

        Kernel() { }
        public Kernel(string ephemerisPath, int siderealMode, int positionType, int houseSystem, Time time, Position position)
        {
            //1
            if (!string.IsNullOrWhiteSpace(ephemerisPath) && Directory.Exists(ephemerisPath))
            {
                __sweph.swe_set_ephe_path(ephemerisPath);
                _iflag = (int)__sweph.SEFLG_SWIEPH;
            }
            else
                _iflag = (int)__sweph.SEFLG_MOSEPH;

            //2
            __sweph.swe_set_sid_mode(siderealMode, 0d, 0d);

            //3
            _iflag = positionType | (int)__sweph.SEFLG_SPEED | _iflag;
            _houseSystem = houseSystem;
            _time = time;
            _position = position;

            //4
            _julday_ut = JD(time);

            //5
            _cusps = new double[13];
            _ascmc = new double[10];
            var res = __sweph.swe_houses_ex(_julday_ut, _iflag, _position.Latitude, _position.Longitude, houseSystem, _cusps, _ascmc);
        }

        public double Ascendant { get { return _ascmc[__sweph.SE_ASC]; } }
        public double MC { get { return _ascmc[__sweph.SE_MC]; } }
        public double ARMC { get { return _ascmc[__sweph.SE_ARMC]; } }
        public double Vertex { get { return _ascmc[__sweph.SE_VERTEX]; } }
        public double EquatorialAscendant { get { return _ascmc[__sweph.SE_EQUASC]; } }
        public double CoAscendantKoch { get { return _ascmc[__sweph.SE_COASC1]; } }
        public double CoAscendantMunkasey { get { return _ascmc[__sweph.SE_COASC2]; } }
        public double PolarAscendantMunkasey { get { return _ascmc[__sweph.SE_POLASC]; } }
        public double NASCMC { get { return _ascmc[__sweph.SE_NASCMC]; } }

        public double[] Cusps { get { return _cusps; } }

        public Time Time { get { return _time; } }
        public Position Position { get { return _position; } }

        /// <summary>
        /// finding a body
        /// </summary>
        /// <param name="ipl">body id</param>
        /// <returns></returns>
        public Point Point(int ipl)
        {
            double[] xx = new double[6] { 0, 0, 0, 0, 0, 0 };
            StringBuilder serr = new StringBuilder(256);
            int res = 0;
            Point point = null;

            xx = new double[6] { 0, 0, 0, 0, 0, 0 };
            serr = new StringBuilder(256);
            res = __sweph.swe_calc_ut(_julday_ut, ipl, _iflag, xx, serr);

            point = new Point(
                ipl,
                null,
                xx[0],
                xx[1],
                xx[2],
                xx[3],
                xx[4],
                xx[5]);

            return point;
        }

        /// <summary>
        /// finding a star
        /// </summary>
        /// <param name="name">star name</param>
        /// <returns></returns>
        public Point Point(StringBuilder name)
        {
            double[] xx = new double[6] { 0, 0, 0, 0, 0, 0 };
            StringBuilder serr = new StringBuilder(256);
            long res = 0;
            Point point = null;

            xx = new double[6] { 0, 0, 0, 0, 0, 0 };
            serr = new StringBuilder(256);
            res = __sweph.swe_fixstar_ut(name, _julday_ut, _iflag, xx, serr);

            point = new Point(
                null,
                name,
                xx[0],
                xx[1],
                xx[2],
                xx[3],
                xx[4],
                xx[5]);

            return point;
        }

        public void Dispose()
        {
            __sweph.swe_close();
        }

        public static double JD(Time time)
        {
            var gregflag = __sweph.SE_GREG_CAL;

            if ((long)time.Year * 10000L + (long)time.Month * 100L + (long)time.Day < 15821015L)
                gregflag = __sweph.SE_JUL_CAL;
            else
                gregflag = __sweph.SE_GREG_CAL;

            var julday_ut = __sweph.swe_julday(
                time.Year,
                time.Month,
                time.Day,
                time.Hour,
                gregflag);

            return julday_ut;
        }
    }
}

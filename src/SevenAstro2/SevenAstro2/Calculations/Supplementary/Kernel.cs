using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenAstro2.Calculations.Supplementary
{
    static partial class Kernel
    {
        static Kernel()
        {
            VdStaticConstructor();

            IsStaticConstructor();
        }

        static Primary.Kernel CreatePrimaryKernel(
            Event ev)
        {
            var time = ev.Time;
            var time2 = new Time(time.Year, time.Month, time.Day, time.TimeOfDay.TotalHours);
            var kernel = new Primary.Kernel(ev.Conf.EphemerisPath, (int)ev.Conf.SiderealMode, (int)ev.Conf.PositionType, (int)ev.Conf.HouseSystem, time2, ev.Position);
            return kernel;
        }

        public static Point[] Points(
            PointId[] ids,
            Event ev)
        {
            using (var kernel = CreatePrimaryKernel(ev))
            {
                var result = new List<Point>();

                var ascSign = SignNo(kernel.Ascendant);
                var asc = new Point(PointId.Asc, kernel.Ascendant, 0, 0, 0, 0, 0, (Sign)ascSign, ClassicHouse.H1, SignDegree(kernel.Ascendant), true);

                foreach (var id in ids)
                {
                    if (PointId.__BodiesStart <= id && id <= PointId.__BodiesEnd)
                    {
                        var p = kernel.Point((int)id);
                        var pSign = SignNo(p.Longitude);
                        result.Add(
                            new Point(
                                id,
                                p.Longitude,
                                p.Latitude,
                                p.Distance,
                                p.SpeedInLongitude,
                                p.SpeedInLatitude,
                                p.SpeedInDistance,
                                (Sign)pSign,
                                (ClassicHouse)HouseNo(ascSign, pSign),
                                SignDegree(p.Longitude),
                                IsDirect(p.Longitude))); // <- should be p.SpeedInLongitude
                    }
                    else if (PointId.__ConstructedBodiesStart <= id && id <= PointId.__ConstructedBodiesEnd)
                    {
                        switch (id)
                        {
                            case PointId.Ra:
                                switch (ev.Conf.NodeType)
                                {
                                    case NodeType.TrueNode:
                                        {
                                            var p = kernel.Point((int)PointId.TrueNode);
                                            var pSign = SignNo(p.Longitude);
                                            result.Add(
                                                new Point(
                                                    id,
                                                    p.Longitude,
                                                    p.Latitude,
                                                    p.Distance,
                                                    p.SpeedInLongitude,
                                                    p.SpeedInLatitude,
                                                    p.SpeedInDistance,
                                                    (Sign)pSign,
                                                    (ClassicHouse)HouseNo(ascSign, pSign),
                                                    SignDegree(p.Longitude),
                                                    IsDirect(p.Longitude)));
                                        }
                                        break;
                                    case NodeType.MeanNode:
                                        {
                                            var p = kernel.Point((int)PointId.MeanNode);
                                            var pSign = SignNo(p.Longitude);
                                            result.Add(
                                                new Point(
                                                    id,
                                                    p.Longitude,
                                                    p.Latitude,
                                                    p.Distance,
                                                    p.SpeedInLongitude,
                                                    p.SpeedInLatitude,
                                                    p.SpeedInDistance,
                                                    (Sign)pSign,
                                                    (ClassicHouse)HouseNo(ascSign, pSign),
                                                    SignDegree(p.Longitude),
                                                    IsDirect(p.Longitude)));
                                        }
                                        break;
                                }
                                break;
                            case PointId.Ke:
                                switch (ev.Conf.NodeType)
                                {
                                    case NodeType.TrueNode:
                                        {
                                            var p = kernel.Point((int)PointId.TrueNode);
                                            var lon = (p.Longitude + 180).Range(0, 360);
                                            var lat = p.Latitude * -1d;
                                            var pSign = SignNo(lon);
                                            result.Add(
                                                new Point(
                                                    id,
                                                    lon,
                                                    lat,
                                                    p.Distance,
                                                    p.SpeedInLongitude,
                                                    p.SpeedInLatitude,
                                                    p.SpeedInDistance,
                                                    (Sign)pSign,
                                                    (ClassicHouse)HouseNo(ascSign, pSign),
                                                    SignDegree(p.Longitude), // <- should be lon
                                                    IsDirect(p.Longitude)));
                                        }
                                        break;
                                    case NodeType.MeanNode:
                                        {
                                            var p = kernel.Point((int)PointId.MeanNode);
                                            var lon = (p.Longitude + 180).Range(0, 360);
                                            var lat = p.Latitude * -1d;
                                            var pSign = SignNo(lon);
                                            result.Add(
                                                new Point(
                                                    id,
                                                    lon,
                                                    lat,
                                                    p.Distance,
                                                    p.SpeedInLongitude,
                                                    p.SpeedInLatitude,
                                                    p.SpeedInDistance,
                                                    (Sign)pSign,
                                                    (ClassicHouse)HouseNo(ascSign, pSign),
                                                    SignDegree(p.Longitude),
                                                    IsDirect(p.Longitude)));
                                        }
                                        break;
                                }
                                break;
                            case PointId.Lilith:
                                switch (ev.Conf.ApogType)
                                {
                                    case ApogType.IntpApog:
                                        {
                                            var p = kernel.Point((int)PointId.IntpApog);
                                            var pSign = SignNo(p.Longitude);
                                            result.Add(
                                                new Point(
                                                    id,
                                                    p.Longitude,
                                                    p.Latitude,
                                                    p.Distance,
                                                    p.SpeedInLongitude,
                                                    p.SpeedInLatitude,
                                                    p.SpeedInDistance,
                                                    (Sign)pSign,
                                                    (ClassicHouse)HouseNo(ascSign, pSign),
                                                    SignDegree(p.Longitude),
                                                    IsDirect(p.Longitude)));
                                        }
                                        break;
                                    case ApogType.OscuApog:
                                        {
                                            var p = kernel.Point((int)PointId.OscuApog);
                                            var pSign = SignNo(p.Longitude);
                                            result.Add(
                                                new Point(
                                                    id,
                                                    p.Longitude,
                                                    p.Latitude,
                                                    p.Distance,
                                                    p.SpeedInLongitude,
                                                    p.SpeedInLatitude,
                                                    p.SpeedInDistance,
                                                    (Sign)pSign,
                                                    (ClassicHouse)HouseNo(ascSign, pSign),
                                                    SignDegree(p.Longitude),
                                                    IsDirect(p.Longitude)));
                                        }
                                        break;
                                    case ApogType.MeanApog:
                                        {
                                            var p = kernel.Point((int)PointId.MeanApog);
                                            var pSign = SignNo(p.Longitude);
                                            result.Add(
                                                new Point(
                                                    id,
                                                    p.Longitude,
                                                    p.Latitude,
                                                    p.Distance,
                                                    p.SpeedInLongitude,
                                                    p.SpeedInLatitude,
                                                    p.SpeedInDistance,
                                                    (Sign)pSign,
                                                    (ClassicHouse)HouseNo(ascSign, pSign),
                                                    SignDegree(p.Longitude),
                                                    IsDirect(p.Longitude)));
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                    else if (PointId.__ASCMCStart <= id && id <= PointId.__ASCMCEnd)
                    {
                        switch (id)
                        {
                            case PointId.Asc:
                                {
                                    var sg = SignNo(kernel.Ascendant);
                                    result.Add(new Point(id, kernel.Ascendant, 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Ascendant), true));
                                } break;
                            case PointId.MC:
                                {
                                    var sg = SignNo(kernel.MC);
                                    result.Add(new Point(id, kernel.MC, 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.MC), true));
                                } break;
                            case PointId.ARMC:
                                {
                                    var sg = SignNo(kernel.ARMC);
                                    result.Add(new Point(id, kernel.ARMC, 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.ARMC), true));
                                } break;
                            case PointId.Vertex:
                                {
                                    var sg = SignNo(kernel.Vertex);
                                    result.Add(new Point(id, kernel.Vertex, 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Vertex), true));
                                } break;
                            case PointId.EquatorialAscendant:
                                {
                                    var sg = SignNo(kernel.EquatorialAscendant);
                                    result.Add(new Point(id, kernel.EquatorialAscendant, 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.EquatorialAscendant), true));
                                } break;
                            case PointId.CoAscendantKoch:
                                {
                                    var sg = SignNo(kernel.CoAscendantKoch);
                                    result.Add(new Point(id, kernel.CoAscendantKoch, 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.CoAscendantKoch), true));
                                } break;
                            case PointId.CoAscendantMunkasey:
                                {
                                    var sg = SignNo(kernel.CoAscendantMunkasey);
                                    result.Add(new Point(id, kernel.CoAscendantMunkasey, 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.CoAscendantMunkasey), true));
                                } break;
                            case PointId.PolarAscendantMunkasey:
                                {
                                    var sg = SignNo(kernel.PolarAscendantMunkasey);
                                    result.Add(new Point(id, kernel.PolarAscendantMunkasey, 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.PolarAscendantMunkasey), true));
                                } break;
                            case PointId.NASCMC:
                                {
                                    var sg = SignNo(kernel.NASCMC);
                                    result.Add(new Point(id, kernel.NASCMC, 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.NASCMC), true));
                                } break;
                        }
                    }
                    else if (PointId.__HousesStart <= id && id <= PointId.__HousesEnd)
                    {
                        switch (id)
                        {
                            case PointId.H1:
                                {
                                    var sg = SignNo(kernel.Cusps[1]);
                                    result.Add(new Point(id, kernel.Cusps[1], 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Cusps[1]), true));
                                } break;
                            case PointId.H2:
                                {
                                    var sg = SignNo(kernel.Cusps[2]);
                                    result.Add(new Point(id, kernel.Cusps[2], 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Cusps[2]), true));
                                } break;
                            case PointId.H3:
                                {
                                    var sg = SignNo(kernel.Cusps[3]);
                                    result.Add(new Point(id, kernel.Cusps[3], 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Cusps[3]), true));
                                } break;
                            case PointId.H4:
                                {
                                    var sg = SignNo(kernel.Cusps[4]);
                                    result.Add(new Point(id, kernel.Cusps[4], 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Cusps[4]), true));
                                } break;
                            case PointId.H5:
                                {
                                    var sg = SignNo(kernel.Cusps[5]);
                                    result.Add(new Point(id, kernel.Cusps[5], 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Cusps[5]), true));
                                } break;
                            case PointId.H6:
                                {
                                    var sg = SignNo(kernel.Cusps[6]);
                                    result.Add(new Point(id, kernel.Cusps[6], 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Cusps[6]), true));
                                } break;
                            case PointId.H7:
                                {
                                    var sg = SignNo(kernel.Cusps[7]);
                                    result.Add(new Point(id, kernel.Cusps[7], 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Cusps[7]), true));
                                } break;
                            case PointId.H8:
                                {
                                    var sg = SignNo(kernel.Cusps[8]);
                                    result.Add(new Point(id, kernel.Cusps[8], 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Cusps[8]), true));
                                } break;
                            case PointId.H9:
                                {
                                    var sg = SignNo(kernel.Cusps[9]);
                                    result.Add(new Point(id, kernel.Cusps[9], 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Cusps[9]), true));
                                } break;
                            case PointId.H10:
                                {
                                    var sg = SignNo(kernel.Cusps[10]);
                                    result.Add(new Point(id, kernel.Cusps[10], 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Cusps[10]), true));
                                } break;
                            case PointId.H11:
                                {
                                    var sg = SignNo(kernel.Cusps[11]);
                                    result.Add(new Point(id, kernel.Cusps[11], 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Cusps[11]), true));
                                } break;
                            case PointId.H12:
                                {
                                    var sg = SignNo(kernel.Cusps[12]);
                                    result.Add(new Point(id, kernel.Cusps[12], 0, 0, 0, 0, 0, (Sign)sg, (ClassicHouse)HouseNo(ascSign, sg), SignDegree(kernel.Cusps[12]), true));
                                } break;
                        }
                    }
                }

                return result.ToArray();
            }
        }

        public static double JD(DateTime time)
        {
            return Primary.Kernel.JD(new Time(time.Year, time.Month, time.Day, Math.Round(time.TimeOfDay.TotalHours, 7)));
        }

        public static double DeltaJD(DateTime pcTime, DateTime eventTime)
        {
            if (eventTime < pcTime) return -1;

            var jd1 = JD(pcTime);
            var jd2 = JD(eventTime);

            var deltaJD = jd2 - jd1;

            return deltaJD / 1.014583;
        }

        public const double YearLen = 365.2421897d;

        public static DateTime Converge1(
            PointId id,
            double targetLongitude,
            Event ev)
        {
            var result = ConvergeHelper1(id, targetLongitude, ev, 500, false);

            var dt = result;
            result = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, ev.Time.Second);

            return result;
        }

        static DateTime ConvergeHelper1(
            PointId id,
            double targetLongitude,
            Event ev,
            double ms,
            bool reverse)
        {
            DateTime currentGuess = ev.Time;
            var currentBody = Points(new PointId[] { id }, ev)[0];

            if (EqDeg(currentBody.Longitude, targetLongitude, ms)) return currentGuess;

            DateTime newGuess = default(DateTime);

            double yearPercent = default(double);
            yearPercent = -1d * (currentBody.Longitude - targetLongitude) / 360d;
            if (id == PointId.Ra || id == PointId.Ke || id == PointId.MeanNode || id == PointId.TrueNode) yearPercent *= -1d;

            if (reverse) yearPercent *= -1d;

            if (id == PointId.Mo) newGuess = currentGuess.AddDays(yearPercent * YearLen / 13d);
            else newGuess = currentGuess.AddDays(yearPercent * YearLen);

            return ConvergeHelper1(
                id,
                targetLongitude,
                new Event(
                    newGuess,
                    ev.Position,
                    ev.Conf),
                ms,
                reverse);
        }

        static bool EqDeg(double d1, double d2, double precision)
        {
            var ts1 = TimeSpan.FromHours(d1);
            var ts2 = TimeSpan.FromHours(d2);

            return
                Math.Floor(ts1.TotalHours) == Math.Floor(ts2.TotalHours) &&
                ts1.Minutes == ts2.Minutes &&
                ts1.Seconds == ts2.Seconds &&
                (precision > 0 ? Math.Abs(ts1.TotalMilliseconds - ts2.TotalMilliseconds) < precision : true);
        }

        public static double SignDegree(double longitude) { return longitude % 30d; }
        static bool IsDirect(double speedInLongitude) { return speedInLongitude >= 0; }
        static int SignNo(double longitude)
        {
            return (int)(Math.Floor((longitude - (longitude % 30d)) / 30d) + 1d);
        }
        static int HouseNo(int ascSign, int pointSign)
        {
            var n = pointSign - ascSign + 1;
            if (n <= 0) n += 12;
            return n;
        }
    }

    class Point
    {
        protected Point() { }
        public Point(PointId planet, Sign sign, ClassicHouse house) : this(planet, 0, 0, 0, 0, 0, 0, sign, house, 0, true) { }
        public Point(
            PointId id,
            double longitude,
            double latitude,
            double distance,
            double speedInLongitude,
            double speedInLatitude,
            double speedInDistance,
            Sign sign,
            ClassicHouse classicHouse,
            double degree,
            bool isDirect)
        {
            Id = id;
            Longitude = longitude;
            Latitude = latitude;
            Distance = distance;
            SpeedInLongitude = speedInLongitude;
            SpeedInLatitude = speedInLatitude;
            SpeedInDistance = speedInDistance;

            Sign = sign;
            ClassicHouse = classicHouse;
            Degree = degree;
            IsDirect = isDirect;
        }

        public PointId Id { get; protected set; }

        public double Longitude { get; protected set; }
        public double Latitude { get; protected set; }
        /// <summary>
        /// Distance in AU
        /// </summary>
        public double Distance { get; protected set; }
        /// <summary>
        /// Speed in longitude (deg/day)
        /// </summary>
        public double SpeedInLongitude { get; protected set; }
        /// <summary>
        /// Speed in latitude (deg/day)
        /// </summary>
        public double SpeedInLatitude { get; protected set; }
        /// <summary>
        /// Speed in distance (AU/day)
        /// </summary>
        public double SpeedInDistance { get; protected set; }

        public Sign Sign { get; protected set; }
        public ClassicHouse ClassicHouse { get; protected set; }
        public double Degree { get; protected set; }
        public bool IsDirect { get; protected set; }
    }

    enum ClassicHouse
    {
        None = int.MinValue,
        H1 = 1,
        H2 = 2,
        H3 = 3,
        H4 = 4,
        H5 = 5,
        H6 = 6,
        H7 = 7,
        H8 = 8,
        H9 = 9,
        H10 = 10,
        H11 = 11,
        H12 = 12
    }

    enum Sign : int
    {
        None = int.MinValue,
        Aries = 1,
        Taurus = 2,
        Gemini = 3,
        Cancer = 4,
        Leo = 5,
        Virgo = 6,
        Libra = 7,
        Scorpio = 8,
        Sagittarius = 9,
        Capricorn = 10,
        Aquarius = 11,
        Pisces = 12
    }

    enum PointId : int
    {
        None = int.MinValue,
        __BodiesStart = -1,
        //Sun = 0,
        Su = 0,
        //Moon = 1,
        Mo = 1,
        //Mercury = 2,
        Me = 2,
        //Venus = 3,
        Ve = 3,
        //Mars = 4,
        Ma = 4,
        //Jupiter = 5,
        Ju = 5,
        //Saturn = 6,
        Sa = 6,
        //Uranus = 7,
        Ur = 7,
        //Neptune = 8,
        Ne = 8,
        //Pluto = 9,
        Pl = 9,
        MeanNode = 10,
        TrueNode = 11,
        MeanApog = 12,
        OscuApog = 13,
        IntpApog = 21,
        __BodiesEnd = 1000, // may change
        __ConstructedBodiesStart = 1000000001,
        //Rahu = 1000000002,
        Ra = 1000000002,
        //Ketu = 1000000003,
        Ke = 1000000003,
        Lilith = 1000000004,
        Li = 1000000004,
        __ConstructedBodiesEnd = 1000000999,
        __ASCMCStart = 1000001000,
        //Ascendant = 1000001001,
        Asc = 1000001001,
        MC = 1000001002,
        ARMC = 1000001003,
        Vertex = 1000001004,
        EquatorialAscendant = 1000001005,
        CoAscendantKoch = 1000001006,
        CoAscendantMunkasey = 1000001007,
        PolarAscendantMunkasey = 1000001008,
        NASCMC = 1000001009,
        __ASCMCEnd = 1000001900,
        __HousesStart = 1000002000,
        H1 = 1000002001,
        H2 = 1000002002,
        H3 = 1000002003,
        H4 = 1000002004,
        H5 = 1000002005,
        H6 = 1000002006,
        H7 = 1000002007,
        H8 = 1000002008,
        H9 = 1000002009,
        H10 = 1000002010,
        H11 = 1000002011,
        H12 = 1000002012,
        __HousesEnd = 1000002999,
    }

    class Event
    {
        private Event() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">universal time</param>
        /// <param name="position"></param>
        /// <param name="conf"></param>
        public Event(
            DateTime time,
            Position position,
            Conf conf)
        {
            Time = time;
            Position = position;
            Conf = conf;
        }

        public DateTime Time { get; private set; }
        public Position Position { get; private set; }
        public Conf Conf { get; private set; }
    }

    class Conf
    {
        private Conf() { }
        public Conf(
            string ephemerisPath,
            SiderealMode siderealMode,
            PositionType positionType,
            HouseSystem houseSystem,
            NodeType nodeType,
            ApogType apogType)
        {
            EphemerisPath = ephemerisPath;
            SiderealMode = siderealMode;
            PositionType = positionType;
            HouseSystem = houseSystem;
            NodeType = nodeType;
            ApogType = apogType;
        }

        public string EphemerisPath { get; private set; }
        public SiderealMode SiderealMode { get; private set; }
        public PositionType PositionType { get; private set; }
        public HouseSystem HouseSystem { get; private set; }
        public NodeType NodeType { get; private set; }
        public ApogType ApogType { get; private set; }
    }

    enum NodeType : int
    {
        MeanNode = 10,
        TrueNode = 11
    }

    enum ApogType : int
    {
        MeanApog = 12,
        OscuApog = 13,
        IntpApog = 21
    }

    enum HouseSystem : int
    {
        None = int.MinValue,
        Placidus = 'P',
        Koch = 'K',
        Campanus = 'C',
        Equal = 'A',
        Alcabitus = 'B'
    }

    enum PositionType : long
    {
        None = int.MinValue,
        Equatorial = 2048L,
        Barycentric = 16384L,
        Topocentric = (32 * 1024L),
        Sidereal = (64 * 1024L)
    }

    enum SiderealMode : int
    {
        None = int.MinValue,
        FaganBradley = 0,
        Lahiri = 1,
        Krishnamurti = 5,
        Yukteshwar = 7
    }
}

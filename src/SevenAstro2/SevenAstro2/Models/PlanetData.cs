using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenAstro2.Models
{
    class PlanetData : ModelBase
    {
        Point _planet;
        Calculations.Supplementary.PointId _pointId;
        Calculations.Supplementary.Sign _sign;
        double _degree;
        string _nakshatra;
        string _nakshatraAndSubRuler;
        double _longitude;
        //double _pancha;
        string _mansion;

        public Point Planet
        {
            get { return _planet; }
            set { Set(ref _planet, value, "Planet"); }
        }

        public Calculations.Supplementary.PointId PointId
        {
            get { return _pointId; }
            set
            {
                Set(ref _pointId, value, "PointId");

                switch (_pointId)
                {
                    case Calculations.Supplementary.PointId.Asc:
                        Set(ref _planet, Point.Asc, "Planet");
                        break;
                    case Calculations.Supplementary.PointId.Su:
                        Set(ref _planet, Point.Su, "Planet");
                        break;
                    case Calculations.Supplementary.PointId.Mo:
                        Set(ref _planet, Point.Mo, "Planet");
                        break;
                    case Calculations.Supplementary.PointId.Me:
                        Set(ref _planet, Point.Me, "Planet");
                        break;
                    case Calculations.Supplementary.PointId.Ve:
                        Set(ref _planet, Point.Ve, "Planet");
                        break;
                    case Calculations.Supplementary.PointId.Ma:
                        Set(ref _planet, Point.Ma, "Planet");
                        break;
                    case Calculations.Supplementary.PointId.Ju:
                        Set(ref _planet, Point.Ju, "Planet");
                        break;
                    case Calculations.Supplementary.PointId.Sa:
                        Set(ref _planet, Point.Sa, "Planet");
                        break;
                    case Calculations.Supplementary.PointId.Ra:
                        Set(ref _planet, Point.Ra, "Planet");
                        break;
                    case Calculations.Supplementary.PointId.Ke:
                        Set(ref _planet, Point.Ke, "Planet");
                        break;
                }
            }
        }

        public Calculations.Supplementary.Sign Sign
        {
            get { return _sign; }
            set { Set(ref _sign, value, "Sign"); }
        }

        public double Degree
        {
            get { return _degree; }
            set { Set(ref _degree, value, "Degree"); }
        }

        public string Nakshatra
        {
            get { return _nakshatra; }
            set { Set(ref _nakshatra, value, "Nakshatra"); }
        }

        public string NakshatraAndSubRuler
        {
            get { return _nakshatraAndSubRuler; }
            set { Set(ref _nakshatraAndSubRuler, value, "NakshatraAndSubRuler"); }
        }

        public double Longitude
        {
            get { return _longitude; }
            set { Set(ref _longitude, value, "Longitude"); }
        }

        //public double Pancha
        //{
        //    get { return _pancha; }
        //    set { Set(ref _pancha, value, "Pancha"); }
        //}

        public string Mansion { get { return _mansion; } set { Set(ref _mansion, value, "Mansion"); } }
    }

    enum Point
    {
        None = 0,
        Asc = 1,
        Su = 2,
        Mo = 3,
        Ma = 4,
        Me = 5,
        Ju = 6,
        Ve = 7,
        Sa = 8,
        Ra = 9,
        Ke = 10
    }
}

/*
    enum Point
    {
        None = 0,
        Asc = 1,
        Su = 2,
        Mo = 3,
        Ma = 4,
        Me = 5,
        Ju = 6,
        Ve = 7,
        Sa = 8,
        Ra = 9,
        Ke = 10
    }
    enum Point
    {
        None = 0,
        Asc = 1,
        Su = 2,
        Mo = 3,
        Me = 4,
        Ve = 5,
        Ma = 6,
        Ju = 7,
        Sa = 8,
        Ra = 9,
        Ke = 10
    }
*/

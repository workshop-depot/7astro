using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenAstro2.Models
{
    class PanchaDwaadasaViewModel : ModelBase
    {
        Calculations.Supplementary.PointId _pointId;
        Point _planet;

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

        public Point Planet
        {
            get { return _planet; }
            set { Set(ref _planet, value, "Planet"); }
        }

        double? _pancha;
        public double? Pancha
        {
            get { return _pancha; }
            set { Set(ref _pancha, value, "Pancha"); }
        }

        double? _dwaadasa;
        public double? Dwaadasa
        {
            get { return _dwaadasa; }
            set { Set(ref _dwaadasa, value, "Dwaadasa"); }
        }
    }
}

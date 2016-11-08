using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SevenAstro2.Models
{
    class MatchPointData : ModelBase
    {
        string _degree;
        Calculations.Supplementary.Sign _sign;
        Calculations.Supplementary.PointId _planet;

        public Calculations.Supplementary.PointId Planet
        {
            get { return _planet; }
            set { Set(ref _planet, value, "Planet"); }
        }

        public Calculations.Supplementary.Sign Sign
        {
            get { return _sign; }
            set { Set(ref _sign, value, "Sign"); }
        }

        public string Degree
        {
            get { return _degree; }
            set
            {
                var m = Regex.Match(value, "(?<degree>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*");

                if (!m.Success) throw new ApplicationException();

                Set(ref _degree, value, "Degree");
            }
        }

        public double VDegree
        {
            get
            {
                var m = Regex.Match(_degree, "(?<degree>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*");

                if (!m.Success) throw new ApplicationException();

                int degree = int.Parse(m.Groups["degree"].Value);
                int minute = int.Parse(m.Groups["minute"].Value);
                int second = 0;
                if (m.Groups["second"].Success)
                    second = int.Parse(m.Groups["second"].Value);

                var ts = TimeSpan.FromHours((new TimeSpan(degree, minute, second).TotalHours).Range(0, 30));

                return ts.TotalHours;
            }
            set
            {
                var ts = TimeSpan.FromHours(value.Range(0, 30));
                Degree = ts.ShowAsDegree();
            }
        }

        public double VLongitude
        {
            get
            {
                return ((int)Sign - 1) * 30d + VDegree;
            }
        }

        public string[] Planets
        {
            get
            {
                return new string[]
                {
                    Calculations.Supplementary.PointId.Su.ToString(),
                    Calculations.Supplementary.PointId.Mo.ToString(),
                    Calculations.Supplementary.PointId.Ma.ToString(),
                    Calculations.Supplementary.PointId.Me.ToString(),
                    Calculations.Supplementary.PointId.Ju.ToString(),
                    Calculations.Supplementary.PointId.Ve.ToString(),
                    Calculations.Supplementary.PointId.Sa.ToString(),
                    Calculations.Supplementary.PointId.Ra.ToString(),
                    Calculations.Supplementary.PointId.Ke.ToString()
                };
            }
        }

        public string[] Signs
        {
            get
            {
                return (from sn in Enum.GetNames(typeof(Calculations.Supplementary.Sign))
                        where sn != Calculations.Supplementary.Sign.None.ToString()
                        select sn).ToArray();
            }
        }
    }
}


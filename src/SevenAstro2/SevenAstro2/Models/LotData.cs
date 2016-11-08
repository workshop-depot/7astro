using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenAstro2.Models
{
    class LotData : ModelBase
    {
        string _sanskritName;
        //string _sign;
        Calculations.Supplementary.Sign _sign;
        string _house;
        double _degree;
        string _persianName;
        string _englishName;

        public string SanskritName
        {
            get { return _sanskritName; }
            set { Set(ref _sanskritName, value, "SanskritName"); }
        }
        public string PersianName
        {
            get { return _persianName; }
            set { Set(ref _persianName, value, "PersianName"); }
        }
        public string EnglishName
        {
            get { return _englishName; }
            set { Set(ref _englishName, value, "EnglishName"); }
        }
        //public string Sign
        //{
        //    get { return _sign; }
        //    set { Set(ref _sign, value); }
        //}
        public Calculations.Supplementary.Sign Sign
        {
            get { return _sign; }
            set { Set(ref _sign, value, "Sign"); }
        }
        public string House
        {
            get { return _house; }
            set { Set(ref _house, value, "House"); }
        }
        public double Degree
        {
            get { return _degree; }
            set { Set(ref _degree, value, "Degree"); }
        }

        double _longitude;
        public double Longitude
        {
            get { return _longitude; }
            set { Set(ref _longitude, value, "Longitude"); }
        }

        string _lotIndicator;
        public string LotIndicator
        {
            get { return _lotIndicator; }
            set { Set(ref _lotIndicator, value, "LotIndicator"); }
        }

        string _description;
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value, "Description"); }
        }
    }
}

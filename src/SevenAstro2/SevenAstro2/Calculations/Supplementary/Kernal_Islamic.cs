using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenAstro2.Calculations.Supplementary
{
    static partial class Kernel
    {
        static void IsStaticConstructor()
        {
            IsInitMansions();
        }

        static List<IsMansion> IsMansions { get; set; }

        static void IsInitMansions()
        {
            const double mansionLen = 360.0d / 28.0d;
            var currentMansion = 1;

            IsMansions = new List<IsMansion>
            {
                new IsMansion ("شَرَطَین - نَطح", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("بُطَین", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("ثُرَیّا", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("دَبَران", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("هَقعَه", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("هَنعَه", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("ذِراع", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("نَثرَه", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("طَرف", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("جَبهَه", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("زُبرَه", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("صَرفَه", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("عَوّاء", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("سِمّاک الاعزَل", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("غَفر", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("زُبانا", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("اِکلیل", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("قَلب", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("شولَه", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("نَعائم", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("بَلدَه", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("سَعدُ الذابِح", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("سَعدُ بُلَع", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("سَعدُ سُعود", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("سَعدُالاَخبیَّه", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("فَرغُ الاَوَّل", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("فَرغُ الثانی", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen)),
                new IsMansion ("رِشاء", new Location((currentMansion - 1) * mansionLen, currentMansion++ * mansionLen))
            };
        }

        static IsMansion IsFindMansion(double longitude)
        {
            var mansion = (from n in IsMansions
                           where n.Location.Start <= longitude && longitude < n.Location.End
                           select n).First();

            return mansion;
        }
    }

    struct IsMansion
    {
        public IsMansion(string name, Location location)
        {
            Name = name;
            Location = location;
        }

        public readonly string Name;
        public readonly Location Location;
    }
}

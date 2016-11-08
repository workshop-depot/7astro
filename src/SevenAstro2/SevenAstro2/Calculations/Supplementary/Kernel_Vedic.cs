using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenAstro2.Calculations.Supplementary
{
    static partial class Kernel
    {
        static void VdStaticConstructor()
        {
            VdInitDiptamsa();

            VdInitTrirashi();

            VdInitTrinityNature();

            VdInitElementals();

            VdInitRulingYears();

            VdInitLordship();

            VdInitNakshatras();

            VdInitLotDefinitions();

            VdInitNaturalRelationship();

            VdInitDignitiesOfPlanets();

            VdInitHaddaLords();

            VdInitPartyRelationships();
        }

        const double VdNakshatraLen = 360d / 27d;
        const double VdSubLen = VdNakshatraLen / 120d;
        const double VdDegPerSecond = 360d / (YearLen * 24d * 60d * 60d);

        static List<VdNakshatra> VdNakshatras { get; set; }
        static Dictionary<PointId, int> VdRulingYears { get; set; }
        static Dictionary<Sign, PointId> VdLordship { get; set; }
        static List<VdLotDefinition> VdLotDefinitions { get; set; }
        static Dictionary<Sign, VdElement> VdElementals { get; set; }
        static Dictionary<Sign, VdTrinity> VdTrinityNature { get; set; }
        static Dictionary<Sign, PointId> VdTrirashiDay { get; set; }
        static Dictionary<Sign, PointId> VdTrirashiNight { get; set; }
        static Dictionary<PointId, int> VdDiptamsa { get; set; }
        static Dictionary<PointId, VdNaturalRelationship> VdNaturalRelationships { get; set; }
        static Dictionary<PointId, PointId[]> VdPartyRelationships { get; set; }
        static Dictionary<PointId, VdDignityOfPlanet> VdDignitiesOfPlanets { get; set; }
        static Dictionary<Sign, VdHaddaLordRange[]> VdHaddaLords { get; set; }

        #region init i
        static void VdInitLotDefinitions()
        {
            VdLotDefinitions = new List<VdLotDefinition>();

            var punya = new VdLotDefinition(
                "Punya", "سعادت", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Su), new VdLotParticipant(PointId.Asc), true);
                },
                "Mo - Su + Asc (day)");
            VdLotDefinitions.Add(punya);
            var vidya = new VdLotDefinition(
                "Vidya", "غیب", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Su), new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Asc), true);
                },
                "Su - Mo + Asc (day)");
            VdLotDefinitions.Add(vidya);
            var yasha = new VdLotDefinition(
                "Yasha", "شهرت", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ju), new VdLotParticipant(punya.LotMark), new VdLotParticipant(PointId.Asc), true);
                },
                "Ju - Punya + Asc (day)");
            VdLotDefinitions.Add(yasha);
            var artha = new VdLotDefinition(
                "Artha", "دارایی", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(ClassicHouse.H2), new VdLotParticipant(ClassicHouse.H2, true), new VdLotParticipant(PointId.Asc), false);
                },
                "B2 - Lord(B2) + Asc (same)");
            VdLotDefinitions.Add(artha);
            var bandhana = new VdLotDefinition(
                "Bandhana", "زندان", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(punya.LotMark), new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Asc), true);
                },
                "Punya - Sa + Asc (day)");
            VdLotDefinitions.Add(bandhana);
            var bandhu = new VdLotDefinition(
                "Bandhu", "بستگان", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Me), new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Asc), false);
                },
                "Me - Mo + Asc (same)");
            VdLotDefinitions.Add(bandhu);
            var gaurava = new VdLotDefinition(
                "Gaurava", "احترام", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ju), new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Su), true);
                },
                "Ju - Mo + Su (day)");
            VdLotDefinitions.Add(gaurava);
            var karyasidhi = new VdLotDefinition(
                "Karyasidhi", "موفقیت", string.Empty,
                c =>
                {
                    var isNightly = false;

                    var sun = c.Su;

                    var asc = c.Asc;

                    var sunLen = sun.Longitude;
                    var ascLen = asc.Longitude;

                    if (sunLen < ascLen) sunLen += 360;

                    if (ascLen < sunLen && sunLen < ascLen + 180) isNightly = true;
                    else isNightly = false;

                    if (!isNightly)
                    {
                        return new VdLotDescription(new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Su), new VdLotParticipant(PointId.Su, true), false);
                    }
                    else
                    {
                        return new VdLotDescription(new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Mo, true), false);
                    }
                },
                "Sa - Su/Mo + Lord(Su/Mo) (day/night)");
            VdLotDefinitions.Add(karyasidhi);
            var mitra = new VdLotDefinition(
                "Mitra", "دوستی", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ju), new VdLotParticipant(punya.LotMark), new VdLotParticipant(PointId.Ve), true);
                },
                "Ju - Punya + Ve (day)");
            VdLotDefinitions.Add(mitra);
            var asha = new VdLotDefinition(
                "Asha", "خواسته", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Asc), true);
                },
                "Sa - Ma + Asc (day)");
            VdLotDefinitions.Add(asha);
            var mahatmya = new VdLotDefinition(
                "Mahatmya", "بزرگی", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(punya.LotMark), new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Asc), true);
                },
                "Punya - Ma + Asc (day)");
            VdLotDefinitions.Add(mahatmya);
            var samartha = new VdLotDefinition(
                "Samarthya", "توانایی", string.Empty,
                c =>
                {
                    var ascSign = c.Asc.Sign;

                    if (ascSign == Sign.Aries || ascSign == Sign.Scorpio)
                    {
                        return new VdLotDescription(new VdLotParticipant(PointId.Ju), new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Asc), true);
                    }
                    else
                    {
                        return new VdLotDescription(new VdLotParticipant(PointId.Ma), new VdLotParticipant(ClassicHouse.H1, true), new VdLotParticipant(PointId.Asc), true);
                    }

                    //var maHouse = c.Ma.ClassicHouse;

                    //if (maHouse == ClassicHouse.H1)
                    //{
                    //    return new VdLotDescription(new VdLotParticipant(PointId.Ju), new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Asc), true);
                    //}
                    //else
                    //{
                    //    return new VdLotDescription(new VdLotParticipant(PointId.Ma), new VdLotParticipant(ClassicHouse.H1, true), new VdLotParticipant(PointId.Asc), true);
                    //}
                },
                "if Lord is Ma: Ju - Ma + Asc (day) \r\nelse Ma - Lord(B1) + Asc (day)");
            VdLotDefinitions.Add(samartha);
            var pitri = new VdLotDefinition(
                "Pitri", "پدر", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Su), new VdLotParticipant(PointId.Asc), true);
                },
                "Sa - Su + Asc (day)");
            VdLotDefinitions.Add(pitri);
            var bhratri = new VdLotDefinition(
                "Bhratri", "برادران", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ju), new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Asc), false);
                },
                "Ju - Sa + Asc (same)");
            VdLotDefinitions.Add(bhratri);
            var matri = new VdLotDefinition(
                "Matri", "مادر", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Ve), new VdLotParticipant(PointId.Asc), true);
                },
                "Mo - Ve + Asc (day)");
            VdLotDefinitions.Add(matri);
            var putra = new VdLotDefinition(
                "Putra", "فرزند", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ju), new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Asc), true);
                },
                "Ju - Mo + Asc (day)");
            VdLotDefinitions.Add(putra);
            var jeeva = new VdLotDefinition(
                "Jeeva", "زندگی", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Ju), new VdLotParticipant(PointId.Asc), true);
                },
                "Sa - Ju + Asc (day)");
            VdLotDefinitions.Add(jeeva);
            var karma = new VdLotDefinition(
                "Karma", "کارما", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Me), new VdLotParticipant(PointId.Asc), true);
                },
                "Ma - Me + Asc (day)");
            VdLotDefinitions.Add(karma);
            var roja = new VdLotDefinition(
                "Roja", "بیماری", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Asc), true);
                },
                "Sa - Mo + Asc (day)");
            VdLotDefinitions.Add(roja);
            var roja2 = new VdLotDefinition(
                "Roja2", "بیماری", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Asc), new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Asc), false, false);
                },
                "Asc - Mo + Asc (same) (no 30°)");
            VdLotDefinitions.Add(roja2);
            var kali = new VdLotDefinition(
                "Kali", "نزاع", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ju), new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Asc), true);
                },
                "Ju - Ma + Asc (day)");
            VdLotDefinitions.Add(kali);
            var sastra = new VdLotDefinition(
                "Sastra", "دانش", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ju), new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Me), false);
                },
                "Ju - Sa + Me (same)");
            VdLotDefinitions.Add(sastra);
            var mrityu = new VdLotDefinition(
                "Mrityu", "مرگ", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(ClassicHouse.H8), new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Asc), false);
                },
                "B8 - Mo + Asc (same)");
            VdLotDefinitions.Add(mrityu);
            var mrityu2 = new VdLotDefinition(
                "Mrityu2", "مرگ", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(ClassicHouse.H8), new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Sa), false);
                },
                "B8 - Mo + Sa (same)");
            VdLotDefinitions.Add(mrityu2);
            var apamrityu = new VdLotDefinition(
                "Apamrityu", "مرگ تصادفی", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(ClassicHouse.H8), new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Asc), false);
                },
                "B8 - Ma + Asc (same)");
            VdLotDefinitions.Add(apamrityu);
            var vivaha = new VdLotDefinition(
                "Vivaha", "ازدواج - روز و شب یکسان", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ve), new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Asc), false);
                },
                "Ve - Sa + Asc (same)");
            VdLotDefinitions.Add(vivaha);
            //var vivaha2 = new VdLotDefinition(
            //    "Vivaha2", "ازدواج", string.Empty,
            //    c =>
            //    {
            //        return new VdLotDescription(new VdLotParticipant(PointId.Ve), new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Asc), true);
            //    });
            //VdLotDefinitions.Add(vivaha2);
            var vyapara = new VdLotDefinition(
                "Vyapara", "شغل", "",
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Asc), false);
                },
                "Ma - Sa + Asc (same)");
            VdLotDefinitions.Add(vyapara);
            var preeti = new VdLotDefinition(
                "Preeti", "عشق", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(sastra.LotMark), new VdLotParticipant(punya.LotMark), new VdLotParticipant(PointId.Asc), true);
                },
                "Sastra - Punya + Asc (day)");
            VdLotDefinitions.Add(preeti);
            var shraddha = new VdLotDefinition(
                "Shraddha", "اطاعت", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ve), new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Asc), false);
                },
                "Ve - Ma + Asc (same)");
            VdLotDefinitions.Add(shraddha);
            var paradara = new VdLotDefinition(
                "Paradara", "بی عفتی", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ve), new VdLotParticipant(PointId.Su), new VdLotParticipant(PointId.Asc), false);
                },
                "Ve - Su + Asc (same)");
            VdLotDefinitions.Add(paradara);
            var paradara2 = new VdLotDefinition(
                "Paradara2", "بی عفتی", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Asc), false);
                },
                "Mo - Ma + Asc (same)");
            VdLotDefinitions.Add(paradara2);
            var paradesa = new VdLotDefinition(
                "Paradesa", "کشورهای بیگانه", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(ClassicHouse.H9), new VdLotParticipant(ClassicHouse.H9, true), new VdLotParticipant(PointId.Asc), false);
                },
                "B9 - Lord(B9) + Asc (same)");
            VdLotDefinitions.Add(paradesa);
            var manmatha = new VdLotDefinition(
                "Manmatha", "شیدایی", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Mo), new VdLotParticipant(ClassicHouse.H1, true), new VdLotParticipant(PointId.Asc), true);
                },
                "Mo - Lord(B1) + Asc (day)");
            VdLotDefinitions.Add(manmatha);
            var jadya = new VdLotDefinition(
                "Jadya", "بیماریهای مزمن", "",
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Me), true);
                },
                "Ma - Sa + Me (day)");
            VdLotDefinitions.Add(jadya);
            var santapa = new VdLotDefinition(
                "Santapa", "اندوه", "",
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Mo), new VdLotParticipant(ClassicHouse.H6), false);
                },
                "Sa - Mo + B6 (same)");
            VdLotDefinitions.Add(santapa);
            var satru = new VdLotDefinition(
                "Satru", "دشمن", "",
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Asc), true);
                },
                "Ma - Sa + Asc (day)");
            VdLotDefinitions.Add(satru);
            var satru2 = new VdLotDefinition(
                "Satru2", "دشمن", "",
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ma), new VdLotParticipant(ClassicHouse.H6), new VdLotParticipant(PointId.Asc), false);
                },
                "Ma - B6 + Asc (same)");
            VdLotDefinitions.Add(satru2);
            var vanik = new VdLotDefinition(
                "Vanik", "تجارت", "",
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Me), new VdLotParticipant(PointId.Asc), false);
                },
                "Mo - Me + Asc (same)");
            VdLotDefinitions.Add(vanik);
            var پارسایی = new VdLotDefinition(
                "", "پارسایی", "",
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Me), new VdLotParticipant(PointId.Asc), true);
                },
                "Mo - Me + Asc (day)");
            VdLotDefinitions.Add(پارسایی);
            var مکافات = new VdLotDefinition(
                "", "مکافات", "",
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Su), new VdLotParticipant(PointId.Asc), true);
                },
                "Ma - Su + Asc (day)");
            VdLotDefinitions.Add(مکافات);
            var زیارت = new VdLotDefinition(
                "", "زیارت", "",
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(ClassicHouse.H10), new VdLotParticipant(ClassicHouse.H10, true), new VdLotParticipant(PointId.Asc), false);
                },
                "B10 - Lord(B10) + Asc (same)");
            VdLotDefinitions.Add(زیارت);
            var prasava = new VdLotDefinition(
                "Prasava", "بچه دار شدن", "",
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Ju), new VdLotParticipant(PointId.Me), new VdLotParticipant(PointId.Asc), true);
                },
                "Ju - Me + Asc (day)");
            VdLotDefinitions.Add(prasava);
            //var paata = new VdLotDefinition(
            //    "Paata", "افسردگی", "",
            //    c =>
            //    {
            //        return new VdLotDescription(new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Mo), new VdLotParticipant(PointId.Asc), true);
            //    });
            //VdLotDefinitions.Add(paata);
            var jalapatha = new VdLotDefinition(
                "Jalapatha", "سفر دریایی", "",
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(105), new VdLotParticipant(PointId.Sa), new VdLotParticipant(PointId.Asc), true);
                },
                "15° of Cancer - Sa + Asc (day)");
            VdLotDefinitions.Add(jalapatha);
            var labha = new VdLotDefinition(
                "Labha", "دستاورد مالی", "",
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(ClassicHouse.H11), new VdLotParticipant(ClassicHouse.H11, true), new VdLotParticipant(PointId.Asc), false);
                },
                "B11 - Lord(B11) + Asc (same)");
            VdLotDefinitions.Add(labha);
            var مکر = new VdLotDefinition(
                "", "مکر و حیله", string.Empty,
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Me), new VdLotParticipant(vidya.LotMark), new VdLotParticipant(PointId.Asc), true);
                },
                "Me - Vidya + Asc (day)");
            VdLotDefinitions.Add(مکر);
            var کار = new VdLotDefinition(
                "", "کار حق", "",
                c =>
                {
                    return new VdLotDescription(new VdLotParticipant(PointId.Me), new VdLotParticipant(PointId.Ma), new VdLotParticipant(PointId.Asc), true);
                },
                "Me - Ma + Asc (day)");
            VdLotDefinitions.Add(کار);
        }

        static void VdInitLordship()
        {
            VdLordship = new Dictionary<Sign, PointId>();

            VdLordship[Sign.Aries] = PointId.Ma;
            VdLordship[Sign.Taurus] = PointId.Ve;
            VdLordship[Sign.Gemini] = PointId.Me;
            VdLordship[Sign.Cancer] = PointId.Mo;
            VdLordship[Sign.Leo] = PointId.Su;
            VdLordship[Sign.Virgo] = PointId.Me;
            VdLordship[Sign.Libra] = PointId.Ve;
            VdLordship[Sign.Scorpio] = PointId.Ma;
            VdLordship[Sign.Sagittarius] = PointId.Ju;
            VdLordship[Sign.Capricorn] = PointId.Sa;
            VdLordship[Sign.Aquarius] = PointId.Sa;
            VdLordship[Sign.Pisces] = PointId.Ju;
        }

        static void VdInitElementals()
        {
            VdElementals = new Dictionary<Sign, VdElement>();

            VdElementals[Sign.Aries] = VdElement.Fire;
            VdElementals[Sign.Leo] = VdElement.Fire;
            VdElementals[Sign.Sagittarius] = VdElement.Fire;

            VdElementals[Sign.Taurus] = VdElement.Earth;
            VdElementals[Sign.Virgo] = VdElement.Earth;
            VdElementals[Sign.Capricorn] = VdElement.Earth;

            VdElementals[Sign.Gemini] = VdElement.Air;
            VdElementals[Sign.Libra] = VdElement.Air;
            VdElementals[Sign.Aquarius] = VdElement.Air;

            VdElementals[Sign.Cancer] = VdElement.Water;
            VdElementals[Sign.Scorpio] = VdElement.Water;
            VdElementals[Sign.Pisces] = VdElement.Water;
        }

        static void VdInitTrinityNature()
        {
            VdTrinityNature = new Dictionary<Sign, VdTrinity>();

            VdTrinityNature[Sign.Aries] = VdTrinity.Movable;
            VdTrinityNature[Sign.Cancer] = VdTrinity.Movable;
            VdTrinityNature[Sign.Libra] = VdTrinity.Movable;
            VdTrinityNature[Sign.Capricorn] = VdTrinity.Movable;

            VdTrinityNature[Sign.Taurus] = VdTrinity.Fixed;
            VdTrinityNature[Sign.Leo] = VdTrinity.Fixed;
            VdTrinityNature[Sign.Scorpio] = VdTrinity.Fixed;
            VdTrinityNature[Sign.Aquarius] = VdTrinity.Fixed;

            VdTrinityNature[Sign.Gemini] = VdTrinity.Dual;
            VdTrinityNature[Sign.Virgo] = VdTrinity.Dual;
            VdTrinityNature[Sign.Sagittarius] = VdTrinity.Dual;
            VdTrinityNature[Sign.Pisces] = VdTrinity.Dual;
        }

        static void VdInitTrirashi()
        {
            VdTrirashiDay = new Dictionary<Sign, PointId>();
            VdTrirashiNight = new Dictionary<Sign, PointId>();

            VdTrirashiDay[Sign.Aries] = PointId.Su;
            VdTrirashiDay[Sign.Taurus] = PointId.Ve;
            VdTrirashiDay[Sign.Gemini] = PointId.Sa;
            VdTrirashiDay[Sign.Cancer] = PointId.Ve;
            VdTrirashiDay[Sign.Leo] = PointId.Ju;
            VdTrirashiDay[Sign.Virgo] = PointId.Mo;
            VdTrirashiDay[Sign.Libra] = PointId.Me;
            VdTrirashiDay[Sign.Scorpio] = PointId.Ma;
            VdTrirashiDay[Sign.Sagittarius] = PointId.Sa;
            VdTrirashiDay[Sign.Capricorn] = PointId.Ma;
            VdTrirashiDay[Sign.Aquarius] = PointId.Ju;
            VdTrirashiDay[Sign.Pisces] = PointId.Mo;

            VdTrirashiNight[Sign.Aries] = PointId.Ju;
            VdTrirashiNight[Sign.Taurus] = PointId.Mo;
            VdTrirashiNight[Sign.Gemini] = PointId.Me;
            VdTrirashiNight[Sign.Cancer] = PointId.Ma;
            VdTrirashiNight[Sign.Leo] = PointId.Su;
            VdTrirashiNight[Sign.Virgo] = PointId.Ve;
            VdTrirashiNight[Sign.Libra] = PointId.Sa;
            VdTrirashiNight[Sign.Scorpio] = PointId.Ve;
            VdTrirashiNight[Sign.Sagittarius] = PointId.Sa;
            VdTrirashiNight[Sign.Capricorn] = PointId.Ma;
            VdTrirashiNight[Sign.Aquarius] = PointId.Ju;
            VdTrirashiNight[Sign.Pisces] = PointId.Mo;
        }

        static void VdInitDiptamsa()
        {
            VdDiptamsa = new Dictionary<PointId, int>();
            VdDiptamsa[PointId.Su] = 15;
            VdDiptamsa[PointId.Mo] = 12;
            VdDiptamsa[PointId.Ma] = 8;
            VdDiptamsa[PointId.Me] = 7;
            VdDiptamsa[PointId.Ju] = 9;
            VdDiptamsa[PointId.Ve] = 7;
            VdDiptamsa[PointId.Sa] = 9;
            VdDiptamsa[PointId.Ra] = 8;
            VdDiptamsa[PointId.Ke] = 8;
        }

        static void VdInitRulingYears()
        {
            VdRulingYears = new Dictionary<PointId, int>();
            VdRulingYears[PointId.Ke] = 7;
            VdRulingYears[PointId.Ve] = 20;
            VdRulingYears[PointId.Su] = 6;
            VdRulingYears[PointId.Mo] = 10;
            VdRulingYears[PointId.Ma] = 7;
            VdRulingYears[PointId.Ra] = 18;
            VdRulingYears[PointId.Ju] = 16;
            VdRulingYears[PointId.Sa] = 19;
            VdRulingYears[PointId.Me] = 17;
        }

        static void VdInitNakshatras()
        {
            VdNakshatras = new List<VdNakshatra>();

            var nameAndRuler = new Tuple<string, PointId>[]
            {
                new Tuple<string, PointId>("Ashvinī", PointId.Ke),
                new Tuple<string, PointId>("Bharanī", PointId.Ve),
                new Tuple<string, PointId>("Krittikā", PointId.Su),
                new Tuple<string, PointId>("Rohini", PointId.Mo),
                new Tuple<string, PointId>("Mrigashīrsha", PointId.Ma),
                new Tuple<string, PointId>("Ārdrā", PointId.Ra),
                new Tuple<string, PointId>("Punarvasu", PointId.Ju),
                new Tuple<string, PointId>("Pushya", PointId.Sa),
                new Tuple<string, PointId>("Āshleshā", PointId.Me),
                new Tuple<string, PointId>("Maghā", PointId.Ke),
                new Tuple<string, PointId>("Pūrva Phalgunī", PointId.Ve),
                new Tuple<string, PointId>("Uttara Phalgunī", PointId.Su),
                new Tuple<string, PointId>("Hasta", PointId.Mo),
                new Tuple<string, PointId>("Chitrā", PointId.Ma),
                new Tuple<string, PointId>("Svātī", PointId.Ra),
                new Tuple<string, PointId>("Vishākhā", PointId.Ju),
                new Tuple<string, PointId>("Anurādhā", PointId.Sa),
                new Tuple<string, PointId>("Jyeshtha", PointId.Me),
                new Tuple<string, PointId>("Mūla", PointId.Ke),
                new Tuple<string, PointId>("Pūrva Ashādhā", PointId.Ve),
                new Tuple<string, PointId>("Uttara Ashadha", PointId.Su),
                new Tuple<string, PointId>("Shravana", PointId.Mo),
                new Tuple<string, PointId>("Shravishthā", PointId.Ma),
                new Tuple<string, PointId>("Shatabhishā", PointId.Ra),
                new Tuple<string, PointId>("Pūrva Bhādrapadā", PointId.Ju),
                new Tuple<string, PointId>("Uttara Bhādrapadā", PointId.Sa),
                new Tuple<string, PointId>("Revatī", PointId.Me)
            };

            VdNakshatra previousNakshatra = new VdNakshatra(null, PointId.None, new Location(0, 0), null);
            foreach (var nt in nameAndRuler)
            {
                var nakshatraName = nt.Item1;
                var nakshatraRuler = nt.Item2;
                var nakshatraLocation = new Location(previousNakshatra.Location.End, previousNakshatra.Location.End + VdNakshatraLen);

                var subList = new List<VdSub>();

                var previousSub = new VdSub(PointId.None, new Location(0, nakshatraLocation.Start));
                foreach (var ruler in VdRulingYears.Keys.SkipWhile(pid => pid != nakshatraRuler))
                {
                    var len = VdRulingYears[ruler] * VdSubLen;
                    var start = previousSub.Location.End;
                    var end = previousSub.Location.End + len;

                    var sub = new VdSub(ruler, new Location(start, end));

                    subList.Add(sub);

                    previousSub = sub;
                }
                foreach (var ruler in VdRulingYears.Keys.TakeWhile(pid => pid != nakshatraRuler))
                {
                    var len = VdRulingYears[ruler] * VdSubLen;
                    var start = previousSub.Location.End;
                    var end = previousSub.Location.End + len;

                    var sub = new VdSub(ruler, new Location(start, end));

                    subList.Add(sub);

                    previousSub = sub;
                }

                var nakshatra = new VdNakshatra(nakshatraName, nakshatraRuler, nakshatraLocation, subList.ToArray());
                VdNakshatras.Add(nakshatra);

                previousNakshatra = nakshatra;
            }
        }

        /// <summary>
        /// P 41
        /// </summary>
        static void VdInitNaturalRelationship()
        {
            VdNaturalRelationships = new Dictionary<PointId, VdNaturalRelationship>();

            VdNaturalRelationships[PointId.Su] = new VdNaturalRelationship(PointId.Su, new PointId[] { PointId.Mo, PointId.Ma, PointId.Ju }, new PointId[] { PointId.Me }, new PointId[] { PointId.Ve, PointId.Sa });
            VdNaturalRelationships[PointId.Mo] = new VdNaturalRelationship(PointId.Mo, new PointId[] { PointId.Su, PointId.Me }, new PointId[] { PointId.Ma, PointId.Ju, PointId.Ve, PointId.Sa }, new PointId[] { });
            VdNaturalRelationships[PointId.Ma] = new VdNaturalRelationship(PointId.Ma, new PointId[] { PointId.Su, PointId.Mo, PointId.Ju }, new PointId[] { PointId.Ve, PointId.Sa }, new PointId[] { PointId.Me });
            VdNaturalRelationships[PointId.Me] = new VdNaturalRelationship(PointId.Me, new PointId[] { PointId.Su, PointId.Ve }, new PointId[] { PointId.Ma, PointId.Ju, PointId.Sa }, new PointId[] { PointId.Mo });
            VdNaturalRelationships[PointId.Ju] = new VdNaturalRelationship(PointId.Ju, new PointId[] { PointId.Su, PointId.Mo, PointId.Ma }, new PointId[] { PointId.Sa }, new PointId[] { PointId.Me, PointId.Ve });
            VdNaturalRelationships[PointId.Ve] = new VdNaturalRelationship(PointId.Ve, new PointId[] { PointId.Me, PointId.Sa }, new PointId[] { PointId.Ju }, new PointId[] { PointId.Su, PointId.Mo });
            VdNaturalRelationships[PointId.Sa] = new VdNaturalRelationship(PointId.Sa, new PointId[] { PointId.Me, PointId.Ve }, new PointId[] { PointId.Ju }, new PointId[] { PointId.Su, PointId.Mo, PointId.Ma });
        }

        static void VdInitPartyRelationships()
        {
            //VdPartyRelationships = new Dictionary<PointId, VdPartyRelationship>();

            //VdPartyRelationships[PointId.Su] = new VdPartyRelationship(PointId.Su, new PointId[] { PointId.Mo, PointId.Ma, PointId.Ju }, new PointId[] { PointId.Sa, PointId.Ve, PointId.Me });
            //VdPartyRelationships[PointId.Mo] = new VdPartyRelationship(PointId.Mo, new PointId[] { PointId.Su, PointId.Ma, PointId.Ju }, new PointId[] { PointId.Sa, PointId.Ve, PointId.Me });
            //VdPartyRelationships[PointId.Ma] = new VdPartyRelationship(PointId.Ma, new PointId[] { PointId.Su, PointId.Mo, PointId.Ju }, new PointId[] { PointId.Sa, PointId.Ve, PointId.Me });
            //VdPartyRelationships[PointId.Me] = new VdPartyRelationship(PointId.Ju, new PointId[] { PointId.Su, PointId.Mo, PointId.Ma }, new PointId[] { PointId.Sa, PointId.Ve, PointId.Me });
            //VdPartyRelationships[PointId.Ju] = new VdPartyRelationship(PointId.Sa, new PointId[] { PointId.Ve, PointId.Me }, new PointId[] { PointId.Su, PointId.Mo, PointId.Ma, PointId.Ju });
            //VdPartyRelationships[PointId.Ve] = new VdPartyRelationship(PointId.Ve, new PointId[] { PointId.Sa, PointId.Me }, new PointId[] { PointId.Su, PointId.Mo, PointId.Ma, PointId.Ju });
            //VdPartyRelationships[PointId.Sa] = new VdPartyRelationship(PointId.Me, new PointId[] { PointId.Sa, PointId.Ve }, new PointId[] { PointId.Su, PointId.Mo, PointId.Ma, PointId.Ju });

            VdPartyRelationships = new Dictionary<PointId, PointId[]>();

            VdPartyRelationships[PointId.Su] = new PointId[] { PointId.Su, PointId.Mo, PointId.Ma, PointId.Ju };
            VdPartyRelationships[PointId.Sa] = new PointId[] { PointId.Sa, PointId.Ve, PointId.Me, PointId.Ra, PointId.Ke };
        }

        /// <summary>
        /// P 39
        /// </summary>
        static void VdInitDignitiesOfPlanets()
        {
            VdDignitiesOfPlanets = new Dictionary<PointId, VdDignityOfPlanet>();

            VdDignitiesOfPlanets[PointId.Su] = new VdDignityOfPlanet(PointId.Su, new Sign[] { Sign.Leo }, new Tuple<Sign, double>(Sign.Aries, 10), new Tuple<Sign, double>(Sign.Libra, 10), Sign.Leo);
            VdDignitiesOfPlanets[PointId.Mo] = new VdDignityOfPlanet(PointId.Mo, new Sign[] { Sign.Cancer }, new Tuple<Sign, double>(Sign.Taurus, 3), new Tuple<Sign, double>(Sign.Scorpio, 3), Sign.Taurus);
            VdDignitiesOfPlanets[PointId.Ma] = new VdDignityOfPlanet(PointId.Ma, new Sign[] { Sign.Aries, Sign.Scorpio }, new Tuple<Sign, double>(Sign.Capricorn, 28), new Tuple<Sign, double>(Sign.Cancer, 28), Sign.Aries);
            VdDignitiesOfPlanets[PointId.Me] = new VdDignityOfPlanet(PointId.Me, new Sign[] { Sign.Gemini, Sign.Virgo }, new Tuple<Sign, double>(Sign.Virgo, 15), new Tuple<Sign, double>(Sign.Pisces, 15), Sign.Virgo);
            VdDignitiesOfPlanets[PointId.Ju] = new VdDignityOfPlanet(PointId.Ju, new Sign[] { Sign.Sagittarius, Sign.Pisces }, new Tuple<Sign, double>(Sign.Cancer, 5), new Tuple<Sign, double>(Sign.Capricorn, 5), Sign.Sagittarius);
            VdDignitiesOfPlanets[PointId.Ve] = new VdDignityOfPlanet(PointId.Ve, new Sign[] { Sign.Taurus, Sign.Libra }, new Tuple<Sign, double>(Sign.Pisces, 27), new Tuple<Sign, double>(Sign.Virgo, 27), Sign.Libra);
            VdDignitiesOfPlanets[PointId.Sa] = new VdDignityOfPlanet(PointId.Sa, new Sign[] { Sign.Capricorn, Sign.Aquarius }, new Tuple<Sign, double>(Sign.Libra, 20), new Tuple<Sign, double>(Sign.Aries, 20), Sign.Aquarius);
            //VdDignitiesOfPlanets[PointId.Ra] = new VdDignityOfPlanet(PointId.Ra, new Sign[] { Sign.Aquarius }, new Tuple<Sign, double>(Sign.Gemini, -1), new Tuple<Sign, double>(Sign.Sagittarius, -1), Sign.Virgo);
            //VdDignitiesOfPlanets[PointId.Ke] = new VdDignityOfPlanet(PointId.Ke, new Sign[] { Sign.Scorpio }, new Tuple<Sign, double>(Sign.Sagittarius, -1), new Tuple<Sign, double>(Sign.Gemini, -1), Sign.Pisces);
            VdDignitiesOfPlanets[PointId.Ra] = new VdDignityOfPlanet(PointId.Ra, new Sign[] { }, new Tuple<Sign, double>(Sign.Gemini, -1), new Tuple<Sign, double>(Sign.Sagittarius, -1), Sign.Virgo);
            VdDignitiesOfPlanets[PointId.Ke] = new VdDignityOfPlanet(PointId.Ke, new Sign[] { }, new Tuple<Sign, double>(Sign.Sagittarius, -1), new Tuple<Sign, double>(Sign.Gemini, -1), Sign.Pisces);
        }

        /// <summary>
        /// P 340
        /// </summary>
        static void VdInitHaddaLords()
        {
            VdHaddaLords = new Dictionary<Sign, VdHaddaLordRange[]>();

            VdHaddaLords[Sign.Aries] = new VdHaddaLordRange[] 
            { 
                new VdHaddaLordRange(PointId.Ju, new Tuple<double, double>(0, 6)),
                new VdHaddaLordRange(PointId.Ve, new Tuple<double, double>(6, 12)),
                new VdHaddaLordRange(PointId.Me, new Tuple<double, double>(12, 20)),
                new VdHaddaLordRange(PointId.Ma, new Tuple<double, double>(20, 25)),
                new VdHaddaLordRange(PointId.Sa, new Tuple<double, double>(25, 30))
            };

            VdHaddaLords[Sign.Taurus] = new VdHaddaLordRange[] 
            { 
                new VdHaddaLordRange(PointId.Ve, new Tuple<double, double>(0, 8)),
                new VdHaddaLordRange(PointId.Me, new Tuple<double, double>(8, 14)),
                new VdHaddaLordRange(PointId.Ju, new Tuple<double, double>(14, 22)),
                new VdHaddaLordRange(PointId.Sa, new Tuple<double, double>(22, 27)),
                new VdHaddaLordRange(PointId.Ma, new Tuple<double, double>(27, 30))
            };

            VdHaddaLords[Sign.Gemini] = new VdHaddaLordRange[] 
            { 
                new VdHaddaLordRange(PointId.Me, new Tuple<double, double>(0, 6)),
                new VdHaddaLordRange(PointId.Ve, new Tuple<double, double>(6, 12)),
                new VdHaddaLordRange(PointId.Ju, new Tuple<double, double>(12, 17)),
                new VdHaddaLordRange(PointId.Ma, new Tuple<double, double>(17, 24)),
                new VdHaddaLordRange(PointId.Sa, new Tuple<double, double>(24, 30))
            };

            VdHaddaLords[Sign.Cancer] = new VdHaddaLordRange[] 
            { 
                new VdHaddaLordRange(PointId.Ma, new Tuple<double, double>(0, 7)),
                new VdHaddaLordRange(PointId.Ve, new Tuple<double, double>(7, 13)),
                new VdHaddaLordRange(PointId.Me, new Tuple<double, double>(13, 19)),
                new VdHaddaLordRange(PointId.Ju, new Tuple<double, double>(19, 26)),
                new VdHaddaLordRange(PointId.Sa, new Tuple<double, double>(26, 30))
            };

            VdHaddaLords[Sign.Leo] = new VdHaddaLordRange[] 
            { 
                new VdHaddaLordRange(PointId.Ju, new Tuple<double, double>(0, 6)),
                new VdHaddaLordRange(PointId.Ve, new Tuple<double, double>(6, 11)),
                new VdHaddaLordRange(PointId.Sa, new Tuple<double, double>(11, 18)),
                new VdHaddaLordRange(PointId.Me, new Tuple<double, double>(18, 24)),
                new VdHaddaLordRange(PointId.Ma, new Tuple<double, double>(24, 30))
            };

            VdHaddaLords[Sign.Virgo] = new VdHaddaLordRange[] 
            { 
                new VdHaddaLordRange(PointId.Me, new Tuple<double, double>(0, 7)),
                new VdHaddaLordRange(PointId.Ve, new Tuple<double, double>(7, 17)),
                new VdHaddaLordRange(PointId.Ju, new Tuple<double, double>(17, 21)),
                new VdHaddaLordRange(PointId.Ma, new Tuple<double, double>(21, 28)),
                new VdHaddaLordRange(PointId.Sa, new Tuple<double, double>(28, 30))
            };

            VdHaddaLords[Sign.Libra] = new VdHaddaLordRange[] 
            { 
                new VdHaddaLordRange(PointId.Sa, new Tuple<double, double>(0, 6)),
                new VdHaddaLordRange(PointId.Me, new Tuple<double, double>(6, 14)),
                new VdHaddaLordRange(PointId.Ju, new Tuple<double, double>(14, 21)),
                new VdHaddaLordRange(PointId.Ve, new Tuple<double, double>(21, 28)),
                new VdHaddaLordRange(PointId.Ma, new Tuple<double, double>(28, 30))
            };

            VdHaddaLords[Sign.Scorpio] = new VdHaddaLordRange[] 
            { 
                new VdHaddaLordRange(PointId.Ma, new Tuple<double, double>(0, 7)),
                new VdHaddaLordRange(PointId.Ve, new Tuple<double, double>(7, 11)),
                new VdHaddaLordRange(PointId.Me, new Tuple<double, double>(11, 19)),
                new VdHaddaLordRange(PointId.Ju, new Tuple<double, double>(19, 24)),
                new VdHaddaLordRange(PointId.Sa, new Tuple<double, double>(24, 30))
            };

            VdHaddaLords[Sign.Sagittarius] = new VdHaddaLordRange[] 
            { 
                new VdHaddaLordRange(PointId.Ju, new Tuple<double, double>(0, 12)),
                new VdHaddaLordRange(PointId.Ve, new Tuple<double, double>(12, 17)),
                new VdHaddaLordRange(PointId.Me, new Tuple<double, double>(17, 21)),
                new VdHaddaLordRange(PointId.Ma, new Tuple<double, double>(21, 26)),
                new VdHaddaLordRange(PointId.Sa, new Tuple<double, double>(26, 30))
            };

            VdHaddaLords[Sign.Capricorn] = new VdHaddaLordRange[] 
            { 
                new VdHaddaLordRange(PointId.Me, new Tuple<double, double>(0, 7)),
                new VdHaddaLordRange(PointId.Ju, new Tuple<double, double>(7, 14)),
                new VdHaddaLordRange(PointId.Ve, new Tuple<double, double>(14, 22)),
                new VdHaddaLordRange(PointId.Sa, new Tuple<double, double>(22, 26)),
                new VdHaddaLordRange(PointId.Ma, new Tuple<double, double>(26, 30))
            };

            VdHaddaLords[Sign.Aquarius] = new VdHaddaLordRange[] 
            { 
                new VdHaddaLordRange(PointId.Me, new Tuple<double, double>(0, 7)),
                new VdHaddaLordRange(PointId.Ve, new Tuple<double, double>(7, 13)),
                new VdHaddaLordRange(PointId.Ju, new Tuple<double, double>(13, 20)),
                new VdHaddaLordRange(PointId.Ma, new Tuple<double, double>(20, 25)),
                new VdHaddaLordRange(PointId.Sa, new Tuple<double, double>(25, 30))
            };

            VdHaddaLords[Sign.Pisces] = new VdHaddaLordRange[] 
            { 
                new VdHaddaLordRange(PointId.Ve, new Tuple<double, double>(0, 12)),
                new VdHaddaLordRange(PointId.Ju, new Tuple<double, double>(12, 16)),
                new VdHaddaLordRange(PointId.Me, new Tuple<double, double>(16, 19)),
                new VdHaddaLordRange(PointId.Ma, new Tuple<double, double>(19, 28)),
                new VdHaddaLordRange(PointId.Sa, new Tuple<double, double>(28, 30))
            };
        }
        #endregion

        static Tuple<VdNakshatra, VdSub> VdFindNakshatra(double longitude)
        {
            var nakshatra = (from n in VdNakshatras
                             where n.Location.Start <= longitude && longitude < n.Location.End
                             select n).First();

            var sub = (from subItem in nakshatra.Subs
                       where subItem.Location.Start <= longitude && longitude < subItem.Location.End
                       select subItem).First();

            return new Tuple<VdNakshatra, VdSub>(nakshatra, sub);
        }

        static double VdLotLon(VdChart c, VdLotParticipant par, List<VdLot> lots)
        {
            double lon = 0;

            if (par.Longitude != null) lon = par.Longitude.Value;
            else if (par.Planet != PointId.None)
            {
                lon = (from p in c.Points
                       where p.Id == par.Planet
                       select p.Longitude).First();

            }
            else if (par.PlanetLord != PointId.None)
            {
                var planet = (from p in c.Points
                              where p.Id == par.PlanetLord
                              select p).First();

                lon = (from p in c.Points
                       where p.Id == VdLordship[planet.Sign]
                       select p.Longitude).First();
            }
            else if (par.House != ClassicHouse.None)
            {
                var ascLord = (from p in c.Points
                               where p.Id == PointId.Asc
                               select p).First();

                var houseSign = ((double)((int)ascLord.Sign + (int)par.House - 1)).Range(1, 13);

                lon = ((houseSign - 1) * 30d + ascLord.Degree).Range(0, 360);
            }
            else if (par.HouseLord != ClassicHouse.None)
            {
                var ascLord = (from p in c.Points
                               where p.Id == PointId.Asc
                               select p).First();

                var houseSign = ((double)((int)ascLord.Sign + (int)par.HouseLord - 1)).Range(1, 13);

                lon = (from p in c.Points
                       where p.Id == VdLordship[(Sign)houseSign]
                       select p.Longitude).First();
            }
            else if (par.LotMark != null)
            {
                lon = (from l in lots
                       where l.LotMark == par.LotMark
                       select l.Longitude).First();
            }

            return lon;
        }
        static double VdExtractLotLen(bool isNightly, bool differsByNight, bool _add30, double lon1, double lon2, double lon3)
        {
            var add30 = 0;

            double lotLen = 0;

            if (differsByNight)
            {
                if (!isNightly)
                {
                    if (lon2 < lon1) lon2 += 360;
                    if (lon3 < lon1) lon3 += 360;

                    if (_add30 && lon1 < lon3 && lon3 < lon2) add30 = 30;

                    lotLen = (lon1 - lon2 + lon3 + add30).Range(0, 360);
                }
                else
                {
                    if (lon1 < lon2) lon1 += 360;
                    if (lon3 < lon2) lon3 += 360;

                    if (_add30 && lon2 < lon3 && lon3 < lon1) add30 = 30;

                    lotLen = (lon2 - lon1 + lon3 + add30).Range(0, 360);
                }
            }
            else
            {
                if (lon2 < lon1) lon2 += 360;
                if (lon3 < lon1) lon3 += 360;

                if (_add30 && lon1 < lon3 && lon3 < lon2) add30 = 30;

                lotLen = (lon1 - lon2 + lon3 + add30).Range(0, 360);
            }
            return lotLen;
        }

        public static List<VdLot> VdLots(VdChart c)
        {
            var result = new List<VdLot>();

            foreach (var ldef in VdLotDefinitions)
            {
                double lon1 = 0;
                double lon2 = 0;
                double lon3 = 0;

                var def = ldef.Definition(c);

                lon1 = VdLotLon(c, def.Participant1, result);
                lon2 = VdLotLon(c, def.Participant2, result);
                lon3 = VdLotLon(c, def.Participant3, result);

                double lotLen = VdExtractLotLen(c.IsNightly, def.DiffersByNight, def.Add30, lon1, lon2, lon3);

                result.Add(new VdLot(ldef.LotMark, lotLen, Sign.None, ClassicHouse.None, ldef.Description));
            }

            var asc = c.Asc;

            var buffer = new List<VdLot>();
            foreach (var lot in result)
            {
                var sign = (Sign)SignNo(lot.Longitude);
                var house = (ClassicHouse)HouseNo((int)asc.Sign, (int)lot.Sign);
                buffer.Add(new VdLot(lot.LotMark, lot.Longitude, sign, house, lot.Description));
            }
            result = buffer;

            return result;
        }

        public static int VdAspectOf(Point p1, Point p2)
        {
            if (p1 != null && p2 != null)
            {
                var buffer_planets = new PointId[] { p1.Id, p2.Id };
                //if (buffer_planets.Contains(PointId.Sa) && buffer_planets.Contains(PointId.Ve))
                //{
                //    var dummy = 0;
                //}

                var diff = Math.Abs(p1.Longitude - p2.Longitude - 360).Range(0, 360);
                if (diff > 180) diff = Math.Abs(diff - 360).Range(0, 360);

                var degree = 18;
                var boundry = 2;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 24;
                boundry = 2;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 30;
                boundry = 2;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 36;
                boundry = 2;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 54;
                boundry = 2;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 72;
                boundry = 2;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 108;
                boundry = 2;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 135;
                boundry = 3;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 144;
                boundry = 3;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 150;
                boundry = 3;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 162;
                boundry = 2;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 45;
                boundry = 2;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 60;
                boundry = 6;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 90;
                boundry = 6;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 120;
                boundry = 6;
                if (degree - boundry < diff && diff < degree + boundry) return degree;

                degree = 180;
                boundry = 8;
                if (!(buffer_planets.Contains(PointId.Ra) && buffer_planets.Contains(PointId.Ke)))
                    if (degree - boundry < diff && diff < degree + boundry) return degree;

                if (p1.Id != PointId.Asc && p2.Id != PointId.Asc)
                    if (diff < VdDiptamsa[p1.Id] + VdDiptamsa[p2.Id]) return 0;
            }

            return -1;
        }

        public static Tuple<PointId, Sign> VdExtractMuntha(VdChart birthChart, int age)
        {
            var house = (ClassicHouse)((int)Math.Abs((age % 12) + 1));

            var asc = birthChart.Asc;

            var sign = (Sign)((int)asc.Sign + (int)house - 1).Range(1, 13);
            var lord = VdLordship[sign];

            return Tuple.Create(lord, sign);
        }

        public static PointId VdExtractBirthChartLord(VdChart birthChart)
        {
            var asc = birthChart.Asc;

            return VdLordship[asc.Sign];
        }

        public static PointId VdExtractProgressChartLord(VdChart pc)
        {
            var asc = pc.Asc;

            return VdLordship[asc.Sign];
        }

        public static PointId VdExtractTrirashiLord(VdChart pc)
        {
            var asc = pc.Asc;

            if (!pc.IsNightly) { return VdTrirashiDay[asc.Sign]; }
            else { return VdTrirashiNight[asc.Sign]; }
        }

        public static PointId VdExtractDinaRatriLord(VdChart pc)
        {
            if (!pc.IsNightly)
            {
                var sun = pc.Su;

                return VdLordship[sun.Sign];
            }
            else
            {
                var moon = pc.Mo;

                return VdLordship[moon.Sign];
            }
        }

        #region divisionals
        static VdChart VdCalculateD60(VdChart birthChart, int division)
        {
            var partLen = 30d / division;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                var startSign = p.Sign;

                var @double = p.Degree * 2d;

                var index = TimeSpan.FromHours(@double).Degrees() + 1;

                var newSign = (Sign)((int)startSign + index.Range(1, 13)).Range(1, 13);

                planets.Add(new VdPoint(p.Id, newSign, ClassicHouse.None));
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Shashtyamsa", division, planets.ToArray());
        }

        static VdChart VdCalculateD45(VdChart birthChart, int division)
        {
            var partLen = 30d / division;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if (VdTrinityNature[p.Sign] == VdTrinity.Movable)
                {
                    var startSign = Sign.Aries;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdTrinityNature[p.Sign] == VdTrinity.Fixed)
                {
                    var startSign = Sign.Leo;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdTrinityNature[p.Sign] == VdTrinity.Dual)
                {
                    var startSign = Sign.Sagittarius;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Akshavedamsa", division, planets.ToArray());
        }

        static VdChart VdCalculateD40(VdChart birthChart, int division)
        {
            var partLen = 30d / division;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if ((int)p.Sign % 2 != 0)
                {
                    var startSign = Sign.Aries;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else
                {
                    var startSign = Sign.Libra;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Khavedamsa", division, planets.ToArray());
        }

        static VdChart VdCalculateD30(VdChart birthChart)
        {
            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if ((int)p.Sign % 2 != 0)
                {
                    if (0 <= p.Degree && p.Degree < 5) { planets.Add(new VdPoint(p.Id, Sign.Aries, ClassicHouse.None)); }
                    else if (5 <= p.Degree && p.Degree < 10) { planets.Add(new VdPoint(p.Id, Sign.Aquarius, ClassicHouse.None)); }
                    else if (10 <= p.Degree && p.Degree < 18) { planets.Add(new VdPoint(p.Id, Sign.Sagittarius, ClassicHouse.None)); }
                    else if (18 <= p.Degree && p.Degree < 25) { planets.Add(new VdPoint(p.Id, Sign.Gemini, ClassicHouse.None)); }
                    else if (25 <= p.Degree && p.Degree < 30) { planets.Add(new VdPoint(p.Id, Sign.Libra, ClassicHouse.None)); }
                }
                else
                {
                    if (0 <= p.Degree && p.Degree < 5) { planets.Add(new VdPoint(p.Id, Sign.Taurus, ClassicHouse.None)); }
                    else if (5 <= p.Degree && p.Degree < 12) { planets.Add(new VdPoint(p.Id, Sign.Virgo, ClassicHouse.None)); }
                    else if (12 <= p.Degree && p.Degree < 20) { planets.Add(new VdPoint(p.Id, Sign.Pisces, ClassicHouse.None)); }
                    else if (20 <= p.Degree && p.Degree < 25) { planets.Add(new VdPoint(p.Id, Sign.Capricorn, ClassicHouse.None)); }
                    else if (25 <= p.Degree && p.Degree < 30) { planets.Add(new VdPoint(p.Id, Sign.Scorpio, ClassicHouse.None)); }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Trimsamsa", 30, planets.ToArray());
        }

        static VdChart VdCalculateD27(VdChart birthChart, int division)
        {
            var partLen = 30d / 27d;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if (VdElementals[p.Sign] == VdElement.Fire)
                {
                    var startSign = Sign.Aries;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdElementals[p.Sign] == VdElement.Earth)
                {
                    var startSign = Sign.Cancer;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdElementals[p.Sign] == VdElement.Air)
                {
                    var startSign = Sign.Libra;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdElementals[p.Sign] == VdElement.Water)
                {
                    var startSign = Sign.Capricorn;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Nakshatramsa", 27, planets.ToArray());
        }

        static VdChart VdCalculateD24(VdChart birthChart, int division)
        {
            var partLen = 30d / 24d;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if ((int)p.Sign % 2 != 0)
                {
                    var startSign = Sign.Leo;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }

                }
                else
                {
                    var startSign = Sign.Cancer;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Chaturvimsamsa", 24, planets.ToArray());
        }

        static VdChart VdCalculateD20(VdChart birthChart, int division)
        {
            var partLen = 30d / 20d;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if (VdTrinityNature[p.Sign] == VdTrinity.Movable)
                {
                    var startSign = Sign.Aries;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdTrinityNature[p.Sign] == VdTrinity.Fixed)
                {
                    var startSign = Sign.Sagittarius;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdTrinityNature[p.Sign] == VdTrinity.Dual)
                {
                    var startSign = Sign.Leo;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Vimsamsa", 20, planets.ToArray());
        }

        static VdChart VdCalculateD16(VdChart birthChart, int division)
        {
            var partLen = 30d / 16d;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if (VdTrinityNature[p.Sign] == VdTrinity.Movable)
                {
                    var startSign = Sign.Aries;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdTrinityNature[p.Sign] == VdTrinity.Fixed)
                {
                    var startSign = Sign.Leo;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdTrinityNature[p.Sign] == VdTrinity.Dual)
                {
                    var startSign = Sign.Sagittarius;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Shodasamsa", 16, planets.ToArray());
        }

        static VdChart VdCalculateD12(VdChart birthChart, int division)
        {
            var partLen = 30d / 12d;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                var startSign = p.Sign;

                for (int i = 0; i < division; i++)
                {
                    if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Dwadasamsa", 12, planets.ToArray());
        }

        static VdChart VdCalculateD11(VdChart birthChart, int division)
        {
            var partLen = 30d / 11d;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                var startSign = (Sign)((int)Sign.Aries - (p.Sign - Sign.Aries + 1).Range(1, 13) + 1).Range(1, 13);

                for (int i = 0; i < division; i++)
                {
                    if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Rudramsa", 11, planets.ToArray());
        }

        static VdChart VdCalculateD10(VdChart birthChart, int division)
        {
            var partLen = 30d / 10d;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if ((int)p.Sign % 2 != 0)
                {
                    var startSign = p.Sign;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else
                {
                    var startSign = (Sign)((int)p.Sign + 8).Range(1, 13);

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Dasamsa", 10, planets.ToArray());
        }

        static VdChart VdCalculateD9(VdChart birthChart, int division)
        {
            var partLen = 30d / 9d;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if (VdElementals[p.Sign] == VdElement.Fire)
                {
                    var startSign = Sign.Aries;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdElementals[p.Sign] == VdElement.Earth)
                {
                    var startSign = Sign.Capricorn;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdElementals[p.Sign] == VdElement.Air)
                {
                    var startSign = Sign.Libra;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdElementals[p.Sign] == VdElement.Water)
                {
                    var startSign = Sign.Cancer;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Navamsa", 9, planets.ToArray());
        }

        static VdChart VdCalculateD8(VdChart birthChart, int division)
        {
            var partLen = 30d / 8d;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if (VdTrinityNature[p.Sign] == VdTrinity.Movable)
                {
                    var startSign = Sign.Aries;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdTrinityNature[p.Sign] == VdTrinity.Fixed)
                {
                    var startSign = Sign.Sagittarius;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else if (VdTrinityNature[p.Sign] == VdTrinity.Dual)
                {
                    var startSign = Sign.Leo;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Ashtamsa", 8, planets.ToArray());
        }

        static VdChart VdCalculateD7(VdChart birthChart, int division)
        {
            var partLen = 30d / 7d;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if ((int)p.Sign % 2 != 0)
                {
                    var startSign = p.Sign;

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
                else
                {
                    var startSign = (Sign)((int)p.Sign + 6).Range(1, 13);

                    for (int i = 0; i < division; i++)
                    {
                        if (i * partLen <= p.Degree && p.Degree < (i + 1) * partLen) { planets.Add(new VdPoint(p.Id, (Sign)((int)startSign + i).Range(1, 13), ClassicHouse.None)); }
                    }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Saptamsa", 7, planets.ToArray());
        }

        static VdChart VdCalculateD6(VdChart birthChart)
        {
            var partLen = 30d / 6d;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if ((int)p.Sign % 2 != 0)
                {
                    if (0 <= p.Degree && p.Degree < partLen) { planets.Add(new VdPoint(p.Id, Sign.Aries, ClassicHouse.None)); }
                    else if (partLen <= p.Degree && p.Degree < 2 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Taurus, ClassicHouse.None)); }
                    else if (2 * partLen <= p.Degree && p.Degree < 3 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Gemini, ClassicHouse.None)); }
                    else if (3 * partLen <= p.Degree && p.Degree < 4 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Cancer, ClassicHouse.None)); }
                    else if (4 * partLen <= p.Degree && p.Degree < 5 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Leo, ClassicHouse.None)); }
                    else if (5 * partLen <= p.Degree && p.Degree < 6 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Virgo, ClassicHouse.None)); }
                }
                else
                {
                    if (0 <= p.Degree && p.Degree < partLen) { planets.Add(new VdPoint(p.Id, Sign.Libra, ClassicHouse.None)); }
                    else if (partLen <= p.Degree && p.Degree < 2 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Scorpio, ClassicHouse.None)); }
                    else if (2 * partLen <= p.Degree && p.Degree < 3 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Sagittarius, ClassicHouse.None)); }
                    else if (3 * partLen <= p.Degree && p.Degree < 4 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Capricorn, ClassicHouse.None)); }
                    else if (4 * partLen <= p.Degree && p.Degree < 5 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Aquarius, ClassicHouse.None)); }
                    else if (5 * partLen <= p.Degree && p.Degree < 6 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Pisces, ClassicHouse.None)); }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Shashthamsa", 6, planets.ToArray());
        }

        static VdChart VdCalculateD5(VdChart birthChart)
        {
            var partLen = 30d / 5d;

            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if ((int)p.Sign % 2 != 0)
                {
                    if (0 <= p.Degree && p.Degree < partLen) { planets.Add(new VdPoint(p.Id, Sign.Aries, ClassicHouse.None)); }
                    else if (partLen <= p.Degree && p.Degree < 2 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Aquarius, ClassicHouse.None)); }
                    else if (2 * partLen <= p.Degree && p.Degree < 3 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Sagittarius, ClassicHouse.None)); }
                    else if (3 * partLen <= p.Degree && p.Degree < 4 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Gemini, ClassicHouse.None)); }
                    else if (4 * partLen <= p.Degree && p.Degree < 5 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Libra, ClassicHouse.None)); }
                }
                else
                {
                    if (0 <= p.Degree && p.Degree < partLen) { planets.Add(new VdPoint(p.Id, Sign.Taurus, ClassicHouse.None)); }
                    else if (partLen <= p.Degree && p.Degree < 2 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Virgo, ClassicHouse.None)); }
                    else if (2 * partLen <= p.Degree && p.Degree < 3 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Pisces, ClassicHouse.None)); }
                    else if (3 * partLen <= p.Degree && p.Degree < 4 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Capricorn, ClassicHouse.None)); }
                    else if (4 * partLen <= p.Degree && p.Degree < 5 * partLen) { planets.Add(new VdPoint(p.Id, Sign.Scorpio, ClassicHouse.None)); }
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Panchamsa", 5, planets.ToArray());
        }

        static VdChart VdCalculateD4(VdChart birthChart)
        {
            var planets = new List<VdPoint>();

            var partLen = 30d / 4d;

            foreach (var p in birthChart.Points)
            {
                if (0 <= p.Degree && p.Degree < partLen)
                {
                    planets.Add(new VdPoint(p.Id, p.Sign, ClassicHouse.None));
                }
                else if (partLen <= p.Degree && p.Degree < 2 * partLen)
                {
                    planets.Add(new VdPoint(p.Id, (Sign)((int)p.Sign + 3).Range(1, 13), ClassicHouse.None));
                }
                else if (2 * partLen <= p.Degree && p.Degree < 3 * partLen)
                {
                    planets.Add(new VdPoint(p.Id, (Sign)((int)p.Sign + 6).Range(1, 13), ClassicHouse.None));
                }
                else if (3 * partLen <= p.Degree && p.Degree < 4 * partLen)
                {
                    planets.Add(new VdPoint(p.Id, (Sign)((int)p.Sign + 9).Range(1, 13), ClassicHouse.None));
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Chaturthamsa", 4, planets.ToArray());
        }

        static VdChart VdCalculateD3(VdChart birthChart)
        {
            var planets = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if (0 <= p.Degree && p.Degree < 10)
                {
                    planets.Add(new VdPoint(p.Id, p.Sign, ClassicHouse.None));
                }
                else if (10 <= p.Degree && p.Degree < 20)
                {
                    planets.Add(new VdPoint(p.Id, (Sign)((int)p.Sign + 4).Range(1, 13), ClassicHouse.None));
                }
                else if (20 <= p.Degree && p.Degree < 30)
                {
                    planets.Add(new VdPoint(p.Id, (Sign)((int)p.Sign + 8).Range(1, 13), ClassicHouse.None));
                }
            }

            var buffer = new List<VdPoint>();
            var asc = (from p in planets
                       where p.Id == PointId.Asc
                       select p).First();
            foreach (var p in planets)
            {
                buffer.Add(new VdPoint(p.Id, p.Sign, (ClassicHouse)((int)p.Sign - (int)asc.Sign + 1).Range(1, 13)));
            }

            planets = buffer;

            return new VdChart("Drekkana", 3, planets.ToArray());
        }

        static VdChart VdCalculateD2(VdChart birthChart)
        {
            var planetsInSunHora = new List<VdPoint>();
            var planetsInMoonHora = new List<VdPoint>();

            foreach (var p in birthChart.Points)
            {
                if ((int)p.Sign % 2 != 0)
                {
                    if (p.Degree < 15)
                    {
                        //planetsInSunHora
                        planetsInSunHora.Add(p);
                    }
                    else
                    {
                        //planetsInMoonHora
                        planetsInMoonHora.Add(p);
                    }
                }
                else
                {
                    if (p.Degree >= 15)
                    {
                        //planetsInSunHora
                        planetsInSunHora.Add(p);
                    }
                    else
                    {
                        //planetsInMoonHora
                        planetsInMoonHora.Add(p);
                    }
                }
            }

            var planets = new List<VdPoint>();

            var asc = (from p in planetsInSunHora
                       where p.Id == PointId.Asc
                       select p).FirstOrDefault();

            if (asc != null)
            {
                foreach (var p in planetsInSunHora)
                    planets.Add(new VdPoint(p.Id, Sign.Leo, ClassicHouse.H1));
                foreach (var p in planetsInMoonHora)
                    planets.Add(new VdPoint(p.Id, Sign.Cancer, ClassicHouse.H12));
            }
            else
            {
                asc = (from p in planetsInMoonHora
                       where p.Id == PointId.Asc
                       select p).FirstOrDefault();

                foreach (var p in planetsInSunHora)
                    planets.Add(new VdPoint(p.Id, Sign.Leo, ClassicHouse.H2));
                foreach (var p in planetsInMoonHora)
                    planets.Add(new VdPoint(p.Id, Sign.Cancer, ClassicHouse.H1));
            }

            return new VdChart("Hora", 2, planets.ToArray());
        }
        #endregion

        public static VdChart VdDivisional(VdChart birthChart, int division)
        {
            if (division == 1) return birthChart;

            if (division == 2)
            {
                return VdCalculateD2(birthChart);
            }

            if (division == 3)
            {
                return VdCalculateD3(birthChart);
            }

            if (division == 4)
            {
                return VdCalculateD4(birthChart);
            }

            if (division == 5)
            {
                return VdCalculateD5(birthChart);
            }

            if (division == 6)
            {
                return VdCalculateD6(birthChart);
            }

            if (division == 7)
            {
                return VdCalculateD7(birthChart, division);
            }

            if (division == 8)
            {
                return VdCalculateD8(birthChart, division);
            }

            if (division == 9)
            {
                return VdCalculateD9(birthChart, division);
            }

            if (division == 10)
            {
                return VdCalculateD10(birthChart, division);
            }

            if (division == 11)
            {
                return VdCalculateD11(birthChart, division);
            }

            if (division == 12)
            {
                return VdCalculateD12(birthChart, division);
            }

            if (division == 16)
            {
                return VdCalculateD16(birthChart, division);
            }

            if (division == 20)
            {
                return VdCalculateD20(birthChart, division);
            }

            if (division == 24)
            {
                return VdCalculateD24(birthChart, division);
            }

            if (division == 27)
            {
                return VdCalculateD27(birthChart, division);
            }

            if (division == 30)
            {
                return VdCalculateD30(birthChart);
            }

            if (division == 40)
            {
                return VdCalculateD40(birthChart, division);
            }

            if (division == 45)
            {
                return VdCalculateD45(birthChart, division);
            }

            if (division == 60)
            {
                return VdCalculateD60(birthChart, division);
            }

            return null;
        }

        public static VdHoroscope VdBirthChart(
            Event ev)
        {
            var points = new List<VdPoint>();

            var plist = Points(
                new PointId[] 
                { 
                    PointId.Asc,
                    PointId.Su, 
                    PointId.Mo, 
                    PointId.Me, 
                    PointId.Ve, 
                    PointId.Ma, 
                    PointId.Ju, 
                    PointId.Sa, 
                    PointId.Ra, 
                    PointId.Ke
                },
                ev);

            foreach (var p in plist)
            {
                var ns = VdFindNakshatra(p.Longitude);
                var mn = IsFindMansion(p.Longitude);

                points.Add(
                    new VdPoint(
                        p.Id,
                        p.Longitude,
                        p.Latitude,
                        p.Distance,
                        p.SpeedInLongitude,
                        p.SpeedInLatitude,
                        p.SpeedInDistance,
                        p.Sign,
                        p.ClassicHouse,
                        p.Degree,
                        p.IsDirect,
                        ns.Item1,
                        ns.Item2,
                        mn));
            }

            return new VdHoroscope(ev, new VdChart(points.ToArray()));
        }

        public static VdHoroscope VdProgressChart(
            VdHoroscope birthChart,
            int age,
            Conf conf)
        {
            var ev = new Event(
                    birthChart.BirthData.Time.AddYears(age),
                    birthChart.BirthData.Position,
                    conf);

            var targetTime = Converge1(
                PointId.Su,
                birthChart.Chart.Su.Longitude,
                ev);

            ev = new Event(
                    targetTime,
                    birthChart.BirthData.Position,
                    conf);

            return VdBirthChart(ev);
        }

        public static List<VdDasa> VdDasha(VdHoroscope h, int deep)
        {
            var moon = h.Chart.Mo;

            var rulers = VdRulingYears.Keys.SkipWhile(pid => pid != moon.Nakshatra.Ruler).ToList();
            rulers.AddRange(VdRulingYears.Keys);

            var passed = VdRulingYears[moon.Nakshatra.Ruler] * (moon.Longitude - moon.Nakshatra.Location.Start) / VdNakshatraLen;
            var offset = YearLen * -1d * passed;
            var mahadasas = new List<VdDasa>();
            var lastStartTime = h.BirthData.Time.AddDays(offset);
            foreach (var r in rulers)
            {
                var dasaLen = VdRulingYears[r] * YearLen;

                mahadasas.Add(new VdDasa(r, PointId.None, PointId.None, PointId.None, PointId.None, lastStartTime, TimeSpan.FromDays(dasaLen)));

                lastStartTime = lastStartTime.AddDays(dasaLen);
            }

            if (deep == 1) return mahadasas;

            var antardasas = new List<VdDasa>();
            foreach (var md in mahadasas)
            {
                lastStartTime = md.StartTime;
                foreach (var r in VdRulingYears.Keys.SkipWhile(p => p != md.Mahadasa))
                {
                    var dasaLen = (VdRulingYears[r] / 120d) * md.Length.TotalDays;

                    antardasas.Add(new VdDasa(md.Mahadasa, r, PointId.None, PointId.None, PointId.None, lastStartTime, TimeSpan.FromDays(dasaLen)));

                    lastStartTime = lastStartTime.AddDays(dasaLen);
                }
                foreach (var r in VdRulingYears.Keys.TakeWhile(p => p != md.Mahadasa))
                {
                    var dasaLen = (VdRulingYears[r] / 120d) * md.Length.TotalDays;

                    antardasas.Add(new VdDasa(md.Mahadasa, r, PointId.None, PointId.None, PointId.None, lastStartTime, TimeSpan.FromDays(dasaLen)));

                    lastStartTime = lastStartTime.AddDays(dasaLen);
                }
            }

            if (deep == 2) return antardasas;

            var pratyantardasas = new List<VdDasa>();
            foreach (var ad in antardasas)
            {
                lastStartTime = ad.StartTime;
                foreach (var r in VdRulingYears.Keys.SkipWhile(p => p != ad.Antardasa))
                {
                    var dasaLen = (VdRulingYears[r] / 120d) * ad.Length.TotalDays;

                    pratyantardasas.Add(new VdDasa(ad.Mahadasa, ad.Antardasa, r, PointId.None, PointId.None, lastStartTime, TimeSpan.FromDays(dasaLen)));

                    lastStartTime = lastStartTime.AddDays(dasaLen);
                }
                foreach (var r in VdRulingYears.Keys.TakeWhile(p => p != ad.Antardasa))
                {
                    var dasaLen = (VdRulingYears[r] / 120d) * ad.Length.TotalDays;

                    pratyantardasas.Add(new VdDasa(ad.Mahadasa, ad.Antardasa, r, PointId.None, PointId.None, lastStartTime, TimeSpan.FromDays(dasaLen)));

                    lastStartTime = lastStartTime.AddDays(dasaLen);
                }
            }

            if (deep == 3) return pratyantardasas;

            var sookshmadasas = new List<VdDasa>();
            foreach (var pad in pratyantardasas)
            {
                lastStartTime = pad.StartTime;
                foreach (var r in VdRulingYears.Keys.SkipWhile(p => p != pad.Pratyantardasa))
                {
                    var dasaLen = (VdRulingYears[r] / 120d) * pad.Length.TotalDays;

                    sookshmadasas.Add(new VdDasa(pad.Mahadasa, pad.Antardasa, pad.Pratyantardasa, r, PointId.None, lastStartTime, TimeSpan.FromDays(dasaLen)));

                    lastStartTime = lastStartTime.AddDays(dasaLen);
                }
                foreach (var r in VdRulingYears.Keys.TakeWhile(p => p != pad.Pratyantardasa))
                {
                    var dasaLen = (VdRulingYears[r] / 120d) * pad.Length.TotalDays;

                    sookshmadasas.Add(new VdDasa(pad.Mahadasa, pad.Antardasa, pad.Pratyantardasa, r, PointId.None, lastStartTime, TimeSpan.FromDays(dasaLen)));

                    lastStartTime = lastStartTime.AddDays(dasaLen);
                }
            }

            if (deep == 4) return sookshmadasas;

            var pranadasas = new List<VdDasa>();
            foreach (var sd in sookshmadasas)
            {
                lastStartTime = sd.StartTime;
                foreach (var r in VdRulingYears.Keys.SkipWhile(p => p != sd.Sookshmadasa))
                {
                    var dasaLen = (VdRulingYears[r] / 120d) * sd.Length.TotalDays;

                    pranadasas.Add(new VdDasa(sd.Mahadasa, sd.Antardasa, sd.Pratyantardasa, sd.Sookshmadasa, r, lastStartTime, TimeSpan.FromDays(dasaLen)));

                    lastStartTime = lastStartTime.AddDays(dasaLen);
                }
                foreach (var r in VdRulingYears.Keys.TakeWhile(p => p != sd.Sookshmadasa))
                {
                    var dasaLen = (VdRulingYears[r] / 120d) * sd.Length.TotalDays;

                    pranadasas.Add(new VdDasa(sd.Mahadasa, sd.Antardasa, sd.Pratyantardasa, sd.Sookshmadasa, r, lastStartTime, TimeSpan.FromDays(dasaLen)));

                    lastStartTime = lastStartTime.AddDays(dasaLen);
                }
            }

            if (deep == 5) return pranadasas;

            return null;
        }

        // P 339 Pancha Vargeeya Bala
        static Dictionary<PointId, Tuple<VdPoint, double>> KshetraBala(VdChart c)
        {
            var q = from p in c.Points
                    where p.Id == PointId.Su ||
                            p.Id == PointId.Mo ||
                            p.Id == PointId.Me ||
                            p.Id == PointId.Ve ||
                            p.Id == PointId.Ma ||
                            p.Id == PointId.Ju ||
                            p.Id == PointId.Sa
                    let d = VdDignitiesOfPlanets[p.Id]
                    let own = (from s1 in d.OwnRasis where s1 == p.Sign select s1).Any() ? 30d : 0
                    let friends = VdPartyRelationships[PointId.Su].Contains(p.Id) ? VdPartyRelationships[PointId.Su] : VdPartyRelationships[PointId.Sa]
                    let friend = (from f1 in friends from f2 in VdDignitiesOfPlanets[f1].OwnRasis where f2 == p.Sign select f1).Any() ? 15d : 0
                    let enemies = !VdPartyRelationships[PointId.Su].Contains(p.Id) ? VdPartyRelationships[PointId.Su] : VdPartyRelationships[PointId.Sa]
                    let enemy = (from e1 in enemies from e2 in VdDignitiesOfPlanets[e1].OwnRasis where e2 == p.Sign select e1).Any() ? 7.5d : 0
                    let k = own > 0 ? own : (friend > 0 ? friend : enemy)
                    select new { p, k };

            var kshetra = new Dictionary<PointId, Tuple<VdPoint, double>>();
            foreach (var item in q) kshetra[item.p.Id] = new Tuple<VdPoint, double>(item.p, item.k);

            return kshetra;
        }

        static Dictionary<PointId, Tuple<VdPoint, double>> UchchaBala(VdChart c)
        {
            var q = from p in c.Points
                    where p.Id == PointId.Su ||
                            p.Id == PointId.Mo ||
                            p.Id == PointId.Me ||
                            p.Id == PointId.Ve ||
                            p.Id == PointId.Ma ||
                            p.Id == PointId.Ju ||
                            p.Id == PointId.Sa
                    let d = VdDignitiesOfPlanets[p.Id]
                    let dbuff = Math.Abs(p.Longitude - d.DeepDebilitationPoint)
                    let dlon = dbuff > 180d ? 360d - dbuff : dbuff
                    //let u = 20d * dlon / 180d
                    let u = dlon / 9d
                    select new { p, u };

            var uchcha = new Dictionary<PointId, Tuple<VdPoint, double>>();
            foreach (var i in q) uchcha[i.p.Id] = new Tuple<VdPoint, double>(i.p, i.u);

            return uchcha;
        }

        static Dictionary<PointId, Tuple<VdPoint, double>> HaddaBala(VdChart c)
        {
            var q = from p in c.Points
                    where p.Id == PointId.Su ||
                            p.Id == PointId.Mo ||
                            p.Id == PointId.Me ||
                            p.Id == PointId.Ve ||
                            p.Id == PointId.Ma ||
                            p.Id == PointId.Ju ||
                            p.Id == PointId.Sa
                    let d = VdDignitiesOfPlanets[p.Id]
                    let h = (from h1 in VdHaddaLords[p.Sign] where h1.Range.Item1 <= p.Degree && p.Degree < h1.Range.Item2 select h1).First()
                    let own = (h.Lord == p.Id) ? 15 : 0
                    let friends = VdPartyRelationships[PointId.Su].Contains(p.Id) ? VdPartyRelationships[PointId.Su] : VdPartyRelationships[PointId.Sa]
                    let enemies = !VdPartyRelationships[PointId.Su].Contains(p.Id) ? VdPartyRelationships[PointId.Su] : VdPartyRelationships[PointId.Sa]
                    let friend = friends.Contains(h.Lord) ? 7.5 : 0
                    let enemy = enemies.Contains(h.Lord) ? 3.75 : 0
                    let ha = own > 0 ? own : (friend > 0 ? friend : enemy)
                    select new { p, ha };

            var hadda = new Dictionary<PointId, Tuple<VdPoint, double>>();
            foreach (var i in q) hadda[i.p.Id] = new Tuple<VdPoint, double>(i.p, i.ha);

            return hadda;
        }

        static Dictionary<PointId, Tuple<VdPoint, double>> DrekkanaBala(VdChart d3)
        {
            if (d3.Division != 3) throw new ArgumentException("d3.Division != 3");

            var q = from p in d3.Points
                    where p.Id == PointId.Su ||
                            p.Id == PointId.Mo ||
                            p.Id == PointId.Me ||
                            p.Id == PointId.Ve ||
                            p.Id == PointId.Ma ||
                            p.Id == PointId.Ju ||
                            p.Id == PointId.Sa
                    let d = VdDignitiesOfPlanets[p.Id]
                    let own = d.OwnRasis.Contains(p.Sign) ? 10 : 0
                    let friends = VdPartyRelationships[PointId.Su].Contains(p.Id) ? VdPartyRelationships[PointId.Su] : VdPartyRelationships[PointId.Sa]
                    let enemies = !VdPartyRelationships[PointId.Su].Contains(p.Id) ? VdPartyRelationships[PointId.Su] : VdPartyRelationships[PointId.Sa]
                    //let friend = (from f1 in f.Friends from f2 in VdDignitiesOfPlanets[f1].OwnRasis where f2 == p.Sign select f2).Any() ? 5d : 0
                    let friend = (from f1 in friends where VdDignitiesOfPlanets[f1].OwnRasis.Contains(p.Sign) select f1).Any() ? 5d : 0
                    //let enemy = (from e1 in f.Enemies from e2 in VdDignitiesOfPlanets[e1].OwnRasis where e2 == p.Sign select e2).Any() ? 2.5d : 0
                    let enemy = (from e1 in enemies where VdDignitiesOfPlanets[e1].OwnRasis.Contains(p.Sign) select e1).Any() ? 2.5d : 0
                    let dr = own > 0 ? own : (friend > 0 ? friend : enemy)
                    select new { p, dr };

            var drekkana = new Dictionary<PointId, Tuple<VdPoint, double>>();
            foreach (var i in q) drekkana[i.p.Id] = new Tuple<VdPoint, double>(i.p, i.dr);

            return drekkana;
        }

        static Dictionary<PointId, Tuple<VdPoint, double>> NavamsaBala(VdChart d9)
        {
            if (d9.Division != 9) throw new ArgumentException("d9.Division != 9");

            var q = from p in d9.Points
                    where p.Id == PointId.Su ||
                            p.Id == PointId.Mo ||
                            p.Id == PointId.Me ||
                            p.Id == PointId.Ve ||
                            p.Id == PointId.Ma ||
                            p.Id == PointId.Ju ||
                            p.Id == PointId.Sa
                    let d = VdDignitiesOfPlanets[p.Id]
                    let own = d.OwnRasis.Contains(p.Sign) ? 5d : 0
                    let friends = VdPartyRelationships[PointId.Su].Contains(p.Id) ? VdPartyRelationships[PointId.Su] : VdPartyRelationships[PointId.Sa]
                    let enemies = !VdPartyRelationships[PointId.Su].Contains(p.Id) ? VdPartyRelationships[PointId.Su] : VdPartyRelationships[PointId.Sa]
                    //let friend = (from f1 in f.Friends from f2 in VdDignitiesOfPlanets[f1].OwnRasis where f2 == p.Sign select f2).Any() ? 2.5d : 0
                    let friend = (from f1 in friends where VdDignitiesOfPlanets[f1].OwnRasis.Contains(p.Sign) select f1).Any() ? 2.5d : 0
                    //let enemy = (from e1 in f.Enemies from e2 in VdDignitiesOfPlanets[e1].OwnRasis where e2 == p.Sign select e2).Any() ? 1.25d : 0
                    let enemy = (from e1 in enemies where VdDignitiesOfPlanets[e1].OwnRasis.Contains(p.Sign) select e1).Any() ? 1.25d : 0
                    let nb = own > 0 ? own : (friend > 0 ? friend : enemy)
                    select new { p, nb };

            var navamsa = new Dictionary<PointId, Tuple<VdPoint, double>>();
            foreach (var i in q) navamsa[i.p.Id] = new Tuple<VdPoint, double>(i.p, i.nb);

            return navamsa;
        }

        public static Dictionary<PointId, Tuple<VdPoint, double>> PanchaVargeeyaBala(VdChart c, VdChart d3, VdChart d9)
        {
            var pancha = new Dictionary<PointId, Tuple<VdPoint, double>>();

            var kshetra = KshetraBala(c);
            var uchcha = UchchaBala(c);
            var hadda = HaddaBala(c);
            var drekkana = DrekkanaBala(d3);
            var navamsa = NavamsaBala(d9);

            foreach (PointId pid in kshetra.Keys)
            {
                var buffer = Math.Round((kshetra[pid].Item2 + uchcha[pid].Item2 + hadda[pid].Item2 + drekkana[pid].Item2 + navamsa[pid].Item2) / 4d, 2);
                pancha[pid] = new Tuple<VdPoint, double>(kshetra[pid].Item1, buffer);
            }

            return pancha;
        }

        public static Dictionary<PointId, int> DwaadasaVargeeyaBala(params VdChart[] divisionals)
        {
            var dwaadasa = new Dictionary<PointId, int>();

            if (divisionals == null || divisionals.Length == 0) return null;

            var points = (from p in divisionals[0].Points
                          where p.Id == PointId.Su ||
                                  p.Id == PointId.Mo ||
                                  p.Id == PointId.Me ||
                                  p.Id == PointId.Ve ||
                                  p.Id == PointId.Ma ||
                                  p.Id == PointId.Ju ||
                                  p.Id == PointId.Sa ||
                                  p.Id == PointId.Ra ||
                                  p.Id == PointId.Ke
                          select p.Id).ToList();
            //Global.Log("");
            var q = from pid in points
                    let dwaaTotal = (from div in divisionals
                                     let p = div.Points.Where(pitem => pitem.Id == pid).First()
                                     //
                                     let dig = VdDignitiesOfPlanets[pid]
                                     let friends = VdPartyRelationships[PointId.Su].Contains(pid) ? VdPartyRelationships[PointId.Su] : VdPartyRelationships[PointId.Sa]
                                     let enemies = !VdPartyRelationships[PointId.Su].Contains(pid) ? VdPartyRelationships[PointId.Su] : VdPartyRelationships[PointId.Sa]
                                     //
                                     let ex = dig.ExaltationRasi.Item1 == p.Sign ? 1 :
                                                 ((from s1 in dig.OwnRasis where s1 == p.Sign select s1).Any() ? 1 :
                                                 ((from f1 in friends from f2 in VdDignitiesOfPlanets[f1].OwnRasis where f2 == p.Sign select f1).Any() ? 1 :
                                                 (dig.DebilitationRasi.Item1 == p.Sign ? -1 :
                                                 ((from e1 in enemies from e2 in VdDignitiesOfPlanets[e1].OwnRasis where e2 == p.Sign select e1).Any() ? -1 : 0))))
                                     let dwaa = ex//.Pipe(xx => { Global.Log("{0,-7}{1,-7}{2}", div.Division, pid, xx); return xx; })
                                     //
                                     //let ex = dig.ExaltationRasi.Item1 == p.Sign ? 1 : 0
                                     //let own = (from s1 in dig.OwnRasis where s1 == p.Sign select s1).Any() ? 1 : 0
                                     //let friend = (from f1 in friends from f2 in VdDignitiesOfPlanets[f1].OwnRasis where f2 == p.Sign select f1).Any() ? 1 : 0
                                     //let deb = dig.DebilitationRasi.Item1 == p.Sign ? -1 : 0
                                     //let enemy = (from e1 in enemies from e2 in VdDignitiesOfPlanets[e1].OwnRasis where e2 == p.Sign select e1).Any() ? -1 : 0
                                     //let dwaa = ex + own + friend + enemy + deb
                                     //
                                     select dwaa).Sum()
                    select new { K = pid, V = dwaaTotal };

            foreach (var item in q) dwaadasa[item.K] = item.V;

            return dwaadasa;
        }

        public static VdChart Chalit(VdChart D1)
        {
            var ascDegree = D1.Asc.Degree;
            var ascHouse = D1.Asc.ClassicHouse;

            var planets = new List<VdPoint>();

            foreach (var p in D1.Points)
            {
                //if (p.Id == PointId.Asc) continue;

                ClassicHouse h = p.ClassicHouse;
                if (p.Degree < ascDegree) { h = (ClassicHouse)((int)h - 1).Range(1, 13); }

                planets.Add(new VdPoint(p.Id, Sign.None, h));
            }

            return new VdChart("Bhava", 0, planets.ToArray());
        }
    }

    class VdHaddaLordRange
    {
        VdHaddaLordRange() { }
        public VdHaddaLordRange(PointId lord, Tuple<double, double> range)
        {
            this.Lord = lord;
            this.Range = range;
        }

        public readonly PointId Lord;
        /// <summary>
        /// first item, inclusive boundry; second item exclusive boundry
        /// </summary>
        public readonly Tuple<double, double> Range;
    }

    class VdDignityOfPlanet
    {
        VdDignityOfPlanet() { }
        public VdDignityOfPlanet(
            PointId planet,
            Sign[] ownRasis,
            Tuple<Sign, double> exaltationRasi,
            //double deepExaltationPoint,
            Tuple<Sign, double> debilitationRasi,
            //double deepDebilitationPoint,
            Sign moolatrikona)
        {
            this.Planet = planet;
            this.OwnRasis = ownRasis;
            this.ExaltationRasi = exaltationRasi;
            //this.DeepExaltationPoint = deepExaltationPoint;
            this.DeepExaltationPoint = (Convert.ToDouble(this.ExaltationRasi.Item1) - 1d) * 30d + this.ExaltationRasi.Item2;
            this.DebilitationRasi = debilitationRasi;
            //this.DeepDebilitationPoint = deepDebilitationPoint;
            this.DeepDebilitationPoint = (Convert.ToDouble(this.DebilitationRasi.Item1) - 1d) * 30d + this.DebilitationRasi.Item2;
            this.Moolatrikona = moolatrikona;
        }

        public readonly PointId Planet;
        public readonly Sign[] OwnRasis;
        public readonly Tuple<Sign, double> ExaltationRasi;
        public readonly double DeepExaltationPoint;
        public readonly Tuple<Sign, double> DebilitationRasi;
        public readonly double DeepDebilitationPoint;
        public readonly Sign Moolatrikona;
    }

    class VdNaturalRelationship
    {
        VdNaturalRelationship() { }
        public VdNaturalRelationship(
            PointId planet,
            PointId[] friends,
            PointId[] nuetral,
            PointId[] enemies)
        {
            this.Planet = planet;
            this.Friends = friends;
            this.Nuetral = nuetral;
            this.Enemies = enemies;
        }

        public readonly PointId Planet;
        public readonly PointId[] Friends;
        public readonly PointId[] Nuetral;
        public readonly PointId[] Enemies;
    }

    //class VdPartyRelationship
    //{
    //    VdPartyRelationship() { }
    //    public VdPartyRelationship(
    //        PointId planet,
    //        PointId[] friends,
    //        PointId[] enemies)
    //    {
    //        this.Planet = planet;
    //        this.Friends = friends;
    //        this.Enemies = enemies;
    //    }

    //    public readonly PointId Planet;
    //    public readonly PointId[] Friends;
    //    public readonly PointId[] Enemies;
    //}

    class VdDasa
    {
        VdDasa() { }
        public VdDasa(
            PointId mahadasa,
            PointId antardasa,
            PointId pratyantardasa,
            PointId sookshmadasa,
            PointId pranadasa,
            DateTime startTime,
            TimeSpan length)
        {
            Mahadasa = mahadasa;
            Antardasa = antardasa;
            Pratyantardasa = pratyantardasa;
            Sookshmadasa = sookshmadasa;
            Pranadasa = pranadasa;
            StartTime = startTime;
            Length = length;
        }

        public PointId Mahadasa { get; private set; }
        public PointId Antardasa { get; private set; }
        public PointId Pratyantardasa { get; private set; }
        public PointId Sookshmadasa { get; private set; }
        public PointId Pranadasa { get; private set; }
        public DateTime StartTime { get; private set; }
        public TimeSpan Length { get; private set; }
    }

    class VdHoroscope
    {
        VdHoroscope() { }
        public VdHoroscope(
            Event birthData,
            VdChart chart)
        {
            BirthData = birthData;
            Chart = chart;
        }

        public Event BirthData { get; private set; }
        public VdChart Chart { get; private set; }

        public bool IsNightly
        {
            get
            {
                return this.Chart.IsNightly;
            }
        }
    }

    class VdLot
    {
        VdLot() { }
        public VdLot(
            string lotMark,
            double longitude,
            Sign sign,
            ClassicHouse house)
        {
            this.LotMark = lotMark;
            this.Longitude = longitude;
            this.Sign = sign;
            this.House = house;

            Degree = Kernel.SignDegree(Longitude);
        }
        public VdLot(
            string lotMark,
            double longitude,
            Sign sign,
            ClassicHouse house,
            string description)
            : this(lotMark, longitude, sign, house)
        {
            this.Description = description;
        }

        public string LotMark { get; private set; }
        public double Longitude { get; private set; }
        public Sign Sign { get; private set; }
        public ClassicHouse House { get; private set; }
        public double Degree { get; private set; }

        public string Memo { get { return this.LotMark; } }
        public string Description { get; private set; }
    }

    class VdLotDefinition
    {
        VdLotDefinition() { }
        public VdLotDefinition(
            string sanskritName,
            string persianName,
            string englishName,
            Func<VdChart, VdLotDescription> definition)
        {
            this.SanskritName = sanskritName;
            this.PersianName = persianName;
            this.EnglishName = englishName;
            this.Definition = definition;
        }
        public VdLotDefinition(
            string sanskritName,
            string persianName,
            string englishName,
            Func<VdChart, VdLotDescription> definition,
            string description)
            : this(sanskritName, persianName, englishName, definition)
        {
            this.Description = description;
        }

        public string SanskritName { get; private set; }
        public string PersianName { get; private set; }
        public string EnglishName { get; private set; }
        public Func<VdChart, VdLotDescription> Definition { get; private set; }

        public string LotMark { get { return string.Format("{0}-{1}-{2}", this.SanskritName, this.PersianName, this.EnglishName); } }
        public string Description { get; private set; }
    }

    class VdLotDescription
    {
        VdLotDescription() { }
        public VdLotDescription(
            VdLotParticipant participant1,
            VdLotParticipant participant2,
            VdLotParticipant participant3,
            bool differsByNight)
            : this(participant1, participant2, participant3, differsByNight, true) { }

        public VdLotDescription(
            VdLotParticipant participant1,
            VdLotParticipant participant2,
            VdLotParticipant participant3,
            bool differsByNight,
            bool add30)
        {
            this.Participant1 = participant1;
            this.Participant2 = participant2;
            this.Participant3 = participant3;
            this.DiffersByNight = differsByNight;
            this.Add30 = add30;
        }

        public VdLotParticipant Participant1 { get; private set; }
        public VdLotParticipant Participant2 { get; private set; }
        public VdLotParticipant Participant3 { get; private set; }
        public bool DiffersByNight { get; private set; }
        public bool Add30 { get; private set; }
    }

    class VdLotParticipant
    {
        VdLotParticipant() { }
        public VdLotParticipant(double lon) : this(PointId.None, PointId.None, ClassicHouse.None, ClassicHouse.None, null, lon) { }
        public VdLotParticipant(PointId planet) : this(planet, PointId.None, ClassicHouse.None, ClassicHouse.None, null, null) { }
        public VdLotParticipant(PointId planetLord, bool considerLord) : this(PointId.None, planetLord, ClassicHouse.None, ClassicHouse.None, null, null) { }
        public VdLotParticipant(ClassicHouse house) : this(PointId.None, PointId.None, house, ClassicHouse.None, null, null) { }
        public VdLotParticipant(ClassicHouse houseLord, bool considerLord) : this(PointId.None, PointId.None, ClassicHouse.None, houseLord, null, null) { }
        public VdLotParticipant(string lotMark) : this(PointId.None, PointId.None, ClassicHouse.None, ClassicHouse.None, lotMark, null) { }
        public VdLotParticipant(
            PointId planet,
            PointId planetLord,
            ClassicHouse house,
            ClassicHouse houseLord,
            string lotMark,
            double? lon)
        {
            Planet = planet;
            PlanetLord = planetLord;
            House = house;
            HouseLord = houseLord;
            LotMark = lotMark;
            Longitude = lon;
        }

        public PointId Planet { get; private set; }
        public PointId PlanetLord { get; private set; }
        public ClassicHouse House { get; private set; }
        public ClassicHouse HouseLord { get; private set; }
        public string LotMark { get; private set; }
        public double? Longitude { get; private set; }
    }

    class VdChart
    {
        VdChart() { }
        public VdChart(VdPoint[] points) : this("Rasi", 1, points) { }
        public VdChart(
            string name,
            int division,
            VdPoint[] points)
        {
            Division = division;
            Points = points;
            Name = name;

            Asc = FindPoint(PointId.Asc);
            Su = FindPoint(PointId.Su);
            Mo = FindPoint(PointId.Mo);
            Ma = FindPoint(PointId.Ma);
            Me = FindPoint(PointId.Me);
            Ju = FindPoint(PointId.Ju);
            Ve = FindPoint(PointId.Ve);
            Sa = FindPoint(PointId.Sa);
            Ra = FindPoint(PointId.Ra);
            Ke = FindPoint(PointId.Ke);
        }

        public string Name { get; private set; }
        public int Division { get; private set; }
        public VdPoint[] Points { get; private set; }

        public bool IsNightly
        {
            get
            {
                var sun = Su;

                var asc = Asc;

                var sunLen = sun.Longitude;
                var ascLen = asc.Longitude;

                if (sunLen < ascLen) sunLen += 360;

                if (ascLen < sunLen && sunLen < ascLen + 180) return true;
                else return false;
            }
        }

        public VdPoint Asc { get; private set; }
        public VdPoint Su { get; private set; }
        public VdPoint Mo { get; private set; }
        public VdPoint Ma { get; private set; }
        public VdPoint Me { get; private set; }
        public VdPoint Ju { get; private set; }
        public VdPoint Ve { get; private set; }
        public VdPoint Sa { get; private set; }
        public VdPoint Ra { get; private set; }
        public VdPoint Ke { get; private set; }

        VdPoint FindPoint(PointId p)
        {
            return (from pl in Points
                    where pl.Id == p
                    select pl).First();
        }
    }

    class VdPoint : Point
    {
        VdPoint() { }
        public VdPoint(PointId planet, Sign sign, ClassicHouse house) : this(planet, 0, 0, 0, 0, 0, 0, sign, house, 0, true, default(VdNakshatra), default(VdSub)) { }
        public VdPoint(
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
            bool isDirect,
            VdNakshatra nakshatra,
            VdSub sub)
            : base(id, longitude, latitude, distance, speedInLongitude, speedInLatitude, speedInDistance, sign, classicHouse, degree, isDirect)
        {
            Nakshatra = nakshatra;
            Sub = sub;
        }

        public VdPoint(
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
            bool isDirect,
            VdNakshatra nakshatra,
            VdSub sub,
            IsMansion mansion)
            : this(
                id,
                longitude,
                latitude,
                distance,
                speedInLongitude,
                speedInLatitude,
                speedInDistance,
                sign,
                classicHouse,
                degree,
                isDirect,
                nakshatra,
                sub)
        {
            Mansion = mansion;
        }

        public VdNakshatra Nakshatra { get; private set; }
        public VdSub Sub { get; private set; }
        public IsMansion Mansion { get; private set; }
    }

    enum VdElement
    {
        Fire,
        Water,
        Air,
        Earth
    }

    enum VdTrinity
    {
        Movable,
        Fixed,
        Dual
    }

    struct VdNakshatra
    {
        public VdNakshatra(
            string name,
            PointId ruler,
            Location location,
            VdSub[] subs)
        {
            Name = name;
            Ruler = ruler;
            Location = location;
            Subs = subs;
        }

        public readonly string Name;
        public readonly PointId Ruler;
        public readonly Location Location;
        public readonly VdSub[] Subs;
    }

    struct VdSub
    {
        public VdSub(
            PointId ruler,
            Location location)
        {
            Ruler = ruler;
            Location = location;
        }

        public readonly PointId Ruler;
        public readonly Location Location;
    }

    struct Location
    {
        public Location(
            double start,
            double end)
        {
            Start = start;
            End = end;
        }

        public readonly double Start;
        public readonly double End;
    }
}

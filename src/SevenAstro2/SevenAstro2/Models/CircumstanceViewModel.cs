using SevenAstro2;
using System;
//using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace SevenAstro2.Models
{
    class CircumstanceViewModel : ModelBase
    {
        public CircumstanceViewModel()
        {
            ApplyUpdate = true;

            var now = DateTime.Now;
            Age = 1;

            BirthData = new Models.BirthData { Name = "-", Date = now, Time = string.Format("{0:HH:mm:ss}", now), Timezone = "-03:30", DST = "0", Longitude = "51:17", Latitude = "35:44" };
            TransientDateTime = DateTime.Now;
            UpdateTransient();
            //UpdateAge();
            JDData = new Models.JDData { Date = now.Date, Time = string.Format("{0:HH:mm:ss}", now) };
            UpdateJD();
            MatchPointData = new Models.MatchPointData { VDegree = 19, Planet = Calculations.Supplementary.PointId.Su, Sign = Calculations.Supplementary.Sign.Aries };

            Update();
        }

        Models.BirthData _BirthData;
        public Models.BirthData BirthData
        {
            get { return _BirthData; }
            set
            {
                if (Set(ref _BirthData, value, "BirthData"))
                {
                    //_BirthData.PropertyChanged += BirthData_PropertyChanged;

                    //TransientData.BirthData = _BirthData;

                    Update();
                }
            }
        }

        //void BirthData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    Update();
        //}

        Calculations.Supplementary.VdHoroscope _BCHoroscope;
        public Calculations.Supplementary.VdHoroscope BCHoroscope
        {
            get { return _BCHoroscope; }
            set
            {
                if (Set(ref _BCHoroscope, value, "BCHoroscope"))
                {
                    //TransientData.BCHoroscope = _BCHoroscope;
                }
            }
        }

        Calculations.Supplementary.VdHoroscope _PCHoroscope;
        public Calculations.Supplementary.VdHoroscope PCHoroscope { get { return _PCHoroscope; } set { Set(ref _PCHoroscope, value, "PCHoroscope"); } }

        Models.MatchPointData _MatchPointData;
        //Models.TransientData _TransientData;
        public Models.MatchPointData MatchPointData { get { return _MatchPointData; } set { Set(ref _MatchPointData, value, "MatchPointData"); } }
        //public Models.TransientData TransientData { get { return _TransientData; } set { Set(ref _TransientData, value, Here.Member); } }

        int _age = 1;
        public int Age
        {
            get { return _age; }
            set
            {
                var v = value >= 0 ? value : 1;
                if (Set(ref _age, v, "Age")) UpdatePCData();
            }
        }

        ObservableCollection<Models.PlanetData> _BCPlanetDataList;
        ObservableCollection<Models.PlanetData> _PCPlanetDataList;
        public ObservableCollection<Models.PlanetData> BCPlanetDataList { get { return _BCPlanetDataList; } set { Set(ref _BCPlanetDataList, value, "BCPlanetDataList"); } }
        public ObservableCollection<Models.PlanetData> PCPlanetDataList { get { return _PCPlanetDataList; } set { Set(ref _PCPlanetDataList, value, "PCPlanetDataList"); } }

        System.Collections.ObjectModel.ObservableCollection<Models.LotData> _BCLotData;
        ObservableCollection<SevenAstro2.Calculations.Supplementary.VdLot> _BCLots;
        System.Collections.ObjectModel.ObservableCollection<Models.LotData> _PCLotData;
        ObservableCollection<SevenAstro2.Calculations.Supplementary.VdLot> _PCLots;
        public System.Collections.ObjectModel.ObservableCollection<Models.LotData> BCLotData { get { return _BCLotData; } set { Set(ref _BCLotData, value, "BCLotData"); } }
        public ObservableCollection<SevenAstro2.Calculations.Supplementary.VdLot> BCLots { get { return _BCLots; } set { Set(ref _BCLots, value, "BCLots"); } }
        public System.Collections.ObjectModel.ObservableCollection<Models.LotData> PCLotData { get { return _PCLotData; } set { Set(ref _PCLotData, value, "PCLotData"); } }
        public ObservableCollection<SevenAstro2.Calculations.Supplementary.VdLot> PCLots { get { return _PCLots; } set { Set(ref _PCLots, value, "PCLots"); } }

        SevenAstro2.Calculations.Supplementary.VdChart _BC_D1;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D2;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D3;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D4;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D5;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D6;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D7;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D8;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D9;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D10;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D11;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D12;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D16;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D20;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D24;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D27;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D30;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D40;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D45;
        SevenAstro2.Calculations.Supplementary.VdChart _BC_D60;
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D1 { get { return _BC_D1; } set { Set(ref _BC_D1, value, "BC_D1"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D2 { get { return _BC_D2; } set { Set(ref _BC_D2, value, "BC_D2"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D3 { get { return _BC_D3; } set { Set(ref _BC_D3, value, "BC_D3"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D4 { get { return _BC_D4; } set { Set(ref _BC_D4, value, "BC_D4"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D5 { get { return _BC_D5; } set { Set(ref _BC_D5, value, "BC_D5"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D6 { get { return _BC_D6; } set { Set(ref _BC_D6, value, "BC_D6"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D7 { get { return _BC_D7; } set { Set(ref _BC_D7, value, "BC_D7"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D8 { get { return _BC_D8; } set { Set(ref _BC_D8, value, "BC_D8"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D9 { get { return _BC_D9; } set { Set(ref _BC_D9, value, "BC_D9"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D10 { get { return _BC_D10; } set { Set(ref _BC_D10, value, "BC_D10"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D11 { get { return _BC_D11; } set { Set(ref _BC_D11, value, "BC_D11"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D12 { get { return _BC_D12; } set { Set(ref _BC_D12, value, "BC_D12"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D16 { get { return _BC_D16; } set { Set(ref _BC_D16, value, "BC_D16"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D20 { get { return _BC_D20; } set { Set(ref _BC_D20, value, "BC_D20"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D24 { get { return _BC_D24; } set { Set(ref _BC_D24, value, "BC_D24"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D27 { get { return _BC_D27; } set { Set(ref _BC_D27, value, "BC_D27"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D30 { get { return _BC_D30; } set { Set(ref _BC_D30, value, "BC_D30"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D40 { get { return _BC_D40; } set { Set(ref _BC_D40, value, "BC_D40"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D45 { get { return _BC_D45; } set { Set(ref _BC_D45, value, "BC_D45"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart BC_D60 { get { return _BC_D60; } set { Set(ref _BC_D60, value, "BC_D60"); } }

        int _dasaDeep = 1;
        public int DasaDeep
        {
            get { return _dasaDeep; }
            set
            {
                var v = (value >= 1 && value <= 5) ? value : 1;
                if (Set(ref _dasaDeep, v, "DasaDeep")) UpdateDasas();
            }
        }

        System.Collections.ObjectModel.ObservableCollection<Models.Dasa> _Dasas;
        public System.Collections.ObjectModel.ObservableCollection<Models.Dasa> Dasas { get { return _Dasas; } set { Set(ref _Dasas, value, "Dasas"); } }

        DateTime _PCStartTime;
        public DateTime PCStartTime
        {
            get { return _PCStartTime; }
            set
            {
                Set(ref _PCStartTime, value, "PCStartTime");

                RaisePropertyChanged("PCStartTimeStr");
            }
        }
        public string PCStartTimeStr { get { return string.Format("{0:dd/MM/yyyy HH:mm:ss}", PCStartTime); } }

        SevenAstro2.Calculations.Supplementary.VdChart _PC_D1;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D2;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D3;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D4;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D5;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D6;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D7;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D8;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D9;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D10;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D11;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D12;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D16;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D20;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D24;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D27;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D30;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D40;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D45;
        SevenAstro2.Calculations.Supplementary.VdChart _PC_D60;
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D1 { get { return _PC_D1; } set { Set(ref _PC_D1, value, "PC_D1"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D2 { get { return _PC_D2; } set { Set(ref _PC_D2, value, "PC_D2"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D3 { get { return _PC_D3; } set { Set(ref _PC_D3, value, "PC_D3"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D4 { get { return _PC_D4; } set { Set(ref _PC_D4, value, "PC_D4"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D5 { get { return _PC_D5; } set { Set(ref _PC_D5, value, "PC_D5"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D6 { get { return _PC_D6; } set { Set(ref _PC_D6, value, "PC_D6"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D7 { get { return _PC_D7; } set { Set(ref _PC_D7, value, "PC_D7"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D8 { get { return _PC_D8; } set { Set(ref _PC_D8, value, "PC_D8"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D9 { get { return _PC_D9; } set { Set(ref _PC_D9, value, "PC_D9"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D10 { get { return _PC_D10; } set { Set(ref _PC_D10, value, "PC_D10"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D11 { get { return _PC_D11; } set { Set(ref _PC_D11, value, "PC_D11"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D12 { get { return _PC_D12; } set { Set(ref _PC_D12, value, "PC_D12"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D16 { get { return _PC_D16; } set { Set(ref _PC_D16, value, "PC_D16"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D20 { get { return _PC_D20; } set { Set(ref _PC_D20, value, "PC_D20"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D24 { get { return _PC_D24; } set { Set(ref _PC_D24, value, "PC_D24"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D27 { get { return _PC_D27; } set { Set(ref _PC_D27, value, "PC_D27"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D30 { get { return _PC_D30; } set { Set(ref _PC_D30, value, "PC_D30"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D40 { get { return _PC_D40; } set { Set(ref _PC_D40, value, "PC_D40"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D45 { get { return _PC_D45; } set { Set(ref _PC_D45, value, "PC_D45"); } }
        public SevenAstro2.Calculations.Supplementary.VdChart PC_D60 { get { return _PC_D60; } set { Set(ref _PC_D60, value, "PC_D60"); } }

        System.Collections.ObjectModel.ObservableCollection<AspectsRow> _BCAspects;
        public System.Collections.ObjectModel.ObservableCollection<AspectsRow> BCAspects { get { return _BCAspects; } set { Set(ref _BCAspects, value, "BCAspects"); } }

        System.Collections.ObjectModel.ObservableCollection<AspectsRow> _PCAspects;
        public System.Collections.ObjectModel.ObservableCollection<AspectsRow> PCAspects { get { return _PCAspects; } set { Set(ref _PCAspects, value, "PCAspects"); } }

        //ObservableCollection<PanchaViewModel> _BCPancha;
        //ObservableCollection<PanchaViewModel> _PCPancha;
        //ObservableCollection<PanchaViewModel> _TransientPancha;
        //public ObservableCollection<PanchaViewModel> BCPancha { get { return _BCPancha; } set { Set(ref _BCPancha, value, "BCPancha"); } }
        //public ObservableCollection<PanchaViewModel> PCPancha { get { return _PCPancha; } set { Set(ref _PCPancha, value, "PCPancha"); } }
        //public ObservableCollection<PanchaViewModel> TransientPancha { get { return _TransientPancha; } set { Set(ref _TransientPancha, value, "TransientPancha"); } }

        //ObservableCollection<DwaadasaViewModel> _BCDwaadasa;
        //ObservableCollection<DwaadasaViewModel> _PCDwaadasa;
        //ObservableCollection<DwaadasaViewModel> _TransientDwaadasa;
        //public ObservableCollection<DwaadasaViewModel> BCDwaadasa { get { return _BCDwaadasa; } set { Set(ref _BCDwaadasa, value, "BCDwaadasa"); } }
        //public ObservableCollection<DwaadasaViewModel> PCDwaadasa { get { return _PCDwaadasa; } set { Set(ref _PCDwaadasa, value, "PCDwaadasa"); } }
        //public ObservableCollection<DwaadasaViewModel> TransientDwaadasa { get { return _TransientDwaadasa; } set { Set(ref _TransientDwaadasa, value, "TransientDwaadasa"); } }

        ObservableCollection<PanchaDwaadasaViewModel> _BCPanchaDwaadasa;
        ObservableCollection<PanchaDwaadasaViewModel> _PCPanchaDwaadasa;
        ObservableCollection<PanchaDwaadasaViewModel> _TransientPanchaDwaadasa;
        public ObservableCollection<PanchaDwaadasaViewModel> BCPanchaDwaadasa { get { return _BCPanchaDwaadasa; } set { Set(ref _BCPanchaDwaadasa, value, "BCPanchaDwaadasa"); } }
        public ObservableCollection<PanchaDwaadasaViewModel> PCPanchaDwaadasa { get { return _PCPanchaDwaadasa; } set { Set(ref _PCPanchaDwaadasa, value, "PCPanchaDwaadasa"); } }
        public ObservableCollection<PanchaDwaadasaViewModel> TransientPanchaDwaadasa { get { return _TransientPanchaDwaadasa; } set { Set(ref _TransientPanchaDwaadasa, value, "TransientPanchaDwaadasa"); } }

        SevenAstro2.Calculations.Supplementary.VdChart _BCChalit;
        public SevenAstro2.Calculations.Supplementary.VdChart BCChalit { get { return _BCChalit; } set { Set(ref _BCChalit, value, "BCChalit"); } }

        SevenAstro2.Calculations.Supplementary.VdChart _PCChalit;
        public SevenAstro2.Calculations.Supplementary.VdChart PCChalit { get { return _PCChalit; } set { Set(ref _PCChalit, value, "PCChalit"); } }

        static string PolishAspect(int aspect)
        {
            if (aspect == -1) return "";
            if (aspect == 0) return "Cj.";

            return string.Format("{0}" + "°", aspect);
        }

        public event EventHandler<EventArgs> Updated;

        bool _ApplyUpdate;
        public bool ApplyUpdate { get { return _ApplyUpdate; } set { Set(ref _ApplyUpdate, value, "ApplyUpdate"); } }

        public void Update()
        {
            if (!ApplyUpdate) return;

            UpdateBCData();

            UpdatePCData();

            if (Updated != null) Updated(this, EventArgs.Empty);
        }

        //void UpdatePCDwaadasa()
        //{
        //    if (PCDwaadasa == null) PCDwaadasa = new ObservableCollection<DwaadasaViewModel>();
        //    PCDwaadasa.Clear();

        //    var dwaadasa = SevenAstro2.Calculations.Supplementary.Kernel.DwaadasaVargeeyaBala(
        //        //PC_D1, 
        //        PC_D2, PC_D3, PC_D4, PC_D5, PC_D6, PC_D7, PC_D8, PC_D9, PC_D10, PC_D11, PC_D12, PC_D24);
        //    foreach (var p in PCPlanetDataList)
        //    {
        //        if (dwaadasa.ContainsKey(p.PointId)) PCDwaadasa.Add(new DwaadasaViewModel { PointId = p.PointId, Dwaadasa = dwaadasa[p.PointId] });
        //    }
        //}

        private void UpdatePCData()
        {
            UpdatePC();

            UpdatePCStartTime();

            UpdatePCDivisionals();

            UpdatePCPlanetDataList();

            UpdatePCLots();

            UpdatePCAspects();

            UpdateYearLords();

            UpdatePCPanchaDwaadasa();

            UpdatePCChalit();
        }

        void UpdatePCChalit()
        {
            PCChalit = SevenAstro2.Calculations.Supplementary.Kernel.Chalit(PC_D1);
        }

        //void UpdatePCPancha()
        //{
        //    if (PCPancha == null) PCPancha = new ObservableCollection<PanchaViewModel>();
        //    PCPancha.Clear();

        //    var pancha = SevenAstro2.Calculations.Supplementary.Kernel.PanchaVargeeyaBala(PC_D1, PC_D3, PC_D9);
        //    foreach (var p in PCPlanetDataList)
        //    {
        //        if (pancha.ContainsKey(p.PointId)) PCPancha.Add(new PanchaViewModel { PointId = p.PointId, Pancha = pancha[p.PointId].Item2 });
        //    }
        //}

        void UpdatePCPanchaDwaadasa()
        {
            if (PCPanchaDwaadasa == null) PCPanchaDwaadasa = new ObservableCollection<PanchaDwaadasaViewModel>();
            PCPanchaDwaadasa.Clear();

            var pancha = SevenAstro2.Calculations.Supplementary.Kernel.PanchaVargeeyaBala(PC_D1, PC_D3, PC_D9);
            var dwaadasa = SevenAstro2.Calculations.Supplementary.Kernel.DwaadasaVargeeyaBala(
                //PC_D1, 
                PC_D2, PC_D3, PC_D4, PC_D5, PC_D6, PC_D7, PC_D8, PC_D9, PC_D10, PC_D11, PC_D12, PC_D24);

            foreach (var p in PCPlanetDataList)
            {
                var item = new PanchaDwaadasaViewModel { PointId = p.PointId };

                if (pancha.ContainsKey(p.PointId)) item.Pancha = pancha[p.PointId].Item2;
                if (dwaadasa.ContainsKey(p.PointId)) item.Dwaadasa = dwaadasa[p.PointId];

                if (item.Pancha.HasValue || item.Dwaadasa.HasValue)
                    PCPanchaDwaadasa.Add(item);
            }
        }

        private void UpdateYearLords()
        {
            ObservableCollection<Models.YearLordCandidate> buffer = null;
            try { buffer = YearLords; }
            catch (Exception zep) { Global.LogError(zep); }
            YearLords = buffer;
        }

        private void UpdateBCData()
        {
            UpdateBC();

            UpdateBCPlanetDataList();

            UpdateBCLots();

            UpdateBCDivisionals();

            UpdateDasas();

            UpdateBCAspects();

            UpdateBCPanchaDwaadasa();

            UpdateBCChalit();
        }

        void UpdateBCChalit()
        {
            BCChalit = SevenAstro2.Calculations.Supplementary.Kernel.Chalit(BC_D1);
        }

        //void UpdateBCDwaadasa()
        //{
        //    if (BCDwaadasa == null) BCDwaadasa = new ObservableCollection<DwaadasaViewModel>();
        //    BCDwaadasa.Clear();

        //    //Global.Log(new string('-', 80));
        //    //Global.Log(BirthData.Name);

        //    var dwaadasa = SevenAstro2.Calculations.Supplementary.Kernel.DwaadasaVargeeyaBala(
        //        //BC_D1, 
        //        BC_D2, BC_D3, BC_D4, BC_D5, BC_D6, BC_D7, BC_D8, BC_D9, BC_D10, BC_D11, BC_D12, BC_D24);
        //    foreach (var p in BCPlanetDataList)
        //    {
        //        if (dwaadasa.ContainsKey(p.PointId)) BCDwaadasa.Add(new DwaadasaViewModel { PointId = p.PointId, Dwaadasa = dwaadasa[p.PointId] });
        //    }
        //}

        //void UpdateBCPancha()
        //{
        //    if (BCPancha == null) BCPancha = new ObservableCollection<PanchaViewModel>();
        //    BCPancha.Clear();

        //    var pancha = SevenAstro2.Calculations.Supplementary.Kernel.PanchaVargeeyaBala(BC_D1, BC_D3, BC_D9);
        //    foreach (var p in BCPlanetDataList)
        //    {
        //        if (pancha.ContainsKey(p.PointId)) BCPancha.Add(new PanchaViewModel { PointId = p.PointId, Pancha = pancha[p.PointId].Item2 });
        //    }
        //}

        void UpdateBCPanchaDwaadasa()
        {
            if (BCPanchaDwaadasa == null) BCPanchaDwaadasa = new ObservableCollection<PanchaDwaadasaViewModel>();
            BCPanchaDwaadasa.Clear();

            var pancha = SevenAstro2.Calculations.Supplementary.Kernel.PanchaVargeeyaBala(BC_D1, BC_D3, BC_D9);
            var dwaadasa = SevenAstro2.Calculations.Supplementary.Kernel.DwaadasaVargeeyaBala(
                //BC_D1, 
                BC_D2, BC_D3, BC_D4, BC_D5, BC_D6, BC_D7, BC_D8, BC_D9, BC_D10, BC_D11, BC_D12, BC_D24);

            foreach (var p in BCPlanetDataList)
            {
                var item = new PanchaDwaadasaViewModel { PointId = p.PointId };

                if (pancha.ContainsKey(p.PointId)) item.Pancha = pancha[p.PointId].Item2;
                if (dwaadasa.ContainsKey(p.PointId)) item.Dwaadasa = dwaadasa[p.PointId];

                if (item.Pancha.HasValue || item.Dwaadasa.HasValue)
                    BCPanchaDwaadasa.Add(item);
            }
        }

        void UpdatePCAspects()
        {
            if (PCAspects == null) PCAspects = new System.Collections.ObjectModel.ObservableCollection<AspectsRow>();
            PCAspects.Clear();

            PCAspects.Add(new AspectsRow
            {
                Asc = "Asc",
                Su = "Su",
                Mo = "Mo",
                Ma = "Ma",
                Me = "Me",
                Ju = "Ju",
                Ve = "Ve",
                Sa = "Sa",
                Ra = "Ra",
                Ke = "Ke"
            });

            var asc = PC_D1.Asc;
            var su = PC_D1.Su;
            var mo = PC_D1.Mo;
            var ma = PC_D1.Ma;
            var me = PC_D1.Me;
            var ju = PC_D1.Ju;
            var ve = PC_D1.Ve;
            var sa = PC_D1.Sa;
            var ra = PC_D1.Ra;
            var ke = PC_D1.Ke;

            var planets = new Calculations.Supplementary.VdPoint[] { asc, su, mo, ma, me, ju, ve, sa, ra, ke };

            Func<SevenAstro2.Calculations.Supplementary.Point, SevenAstro2.Calculations.Supplementary.Point, string> asp = (p1, p2) => PolishAspect(Calculations.Supplementary.Kernel.VdAspectOf(p1, p2));
            // i starts at 1 because we will present aspects between every planet and asp
            for (int i = 1; i < planets.Length; i++)
            {
                var p = planets[i];

                var row = new AspectsRow
                {
                    First = p.Id.ToString(),
                    Asc = asp(asc, p),
                    Su = i > 1 ? asp(su, p) : "",
                    Mo = i > 2 ? asp(mo, p) : "",
                    Ma = i > 3 ? asp(ma, p) : "",
                    Me = i > 4 ? asp(me, p) : "",
                    Ju = i > 5 ? asp(ju, p) : "",
                    Ve = i > 6 ? asp(ve, p) : "",
                    Sa = i > 7 ? asp(sa, p) : "",
                    Ra = i > 8 ? asp(ra, p) : "",
                    Ke = i > 9 ? asp(ke, p) : ""
                };

                PCAspects.Add(row);
            }
        }

        void UpdateBCAspects()
        {
            if (BCAspects == null) BCAspects = new System.Collections.ObjectModel.ObservableCollection<AspectsRow>();
            BCAspects.Clear();

            BCAspects.Add(new AspectsRow
            {
                Asc = "Asc",
                Su = "Su",
                Mo = "Mo",
                Ma = "Ma",
                Me = "Me",
                Ju = "Ju",
                Ve = "Ve",
                Sa = "Sa",
                Ra = "Ra",
                Ke = "Ke"
            });

            var asc = BC_D1.Asc;
            var su = BC_D1.Su;
            var mo = BC_D1.Mo;
            var ma = BC_D1.Ma;
            var me = BC_D1.Me;
            var ju = BC_D1.Ju;
            var ve = BC_D1.Ve;
            var sa = BC_D1.Sa;
            var ra = BC_D1.Ra;
            var ke = BC_D1.Ke;

            var planets = new Calculations.Supplementary.VdPoint[] { asc, su, mo, ma, me, ju, ve, sa, ra, ke };

            Func<SevenAstro2.Calculations.Supplementary.Point, SevenAstro2.Calculations.Supplementary.Point, string> asp = (p1, p2) => PolishAspect(Calculations.Supplementary.Kernel.VdAspectOf(p1, p2));
            // i starts at 1 because we will present aspects between every planet and asp
            for (int i = 1; i < planets.Length; i++)
            {
                var p = planets[i];

                var row = new AspectsRow
                {
                    First = p.Id.ToString(),
                    Asc = asp(asc, p),
                    Su = i > 1 ? asp(su, p) : "",
                    Mo = i > 2 ? asp(mo, p) : "",
                    Ma = i > 3 ? asp(ma, p) : "",
                    Me = i > 4 ? asp(me, p) : "",
                    Ju = i > 5 ? asp(ju, p) : "",
                    Ve = i > 6 ? asp(ve, p) : "",
                    Sa = i > 7 ? asp(sa, p) : ""
                    //Ra = i > 8 ? asp(ra, p) : "",
                    //Ke = i > 9 ? asp(ke, p) : ""
                };

                BCAspects.Add(row);
            }
        }

        void UpdatePCLots()
        {
            PCLots = new ObservableCollection<Calculations.Supplementary.VdLot>(Calculations.Supplementary.Kernel.VdLots(PCHoroscope.Chart));

            if (PCLotData == null) PCLotData = new System.Collections.ObjectModel.ObservableCollection<LotData>();
            PCLotData.Clear();

            //var lotDataList = new List<Models.LotData>();
            foreach (var l in PCLots)
            {
                var nameParts = l.LotMark.Split('-');
                PCLotData.Add(new Models.LotData { SanskritName = nameParts[0], PersianName = nameParts[1], Sign = l.Sign, House = l.House.ToString(), Degree = TimeSpan.FromHours(l.Degree).TotalHours, LotIndicator = "*", Longitude = l.Longitude, Description = l.Description });
            }
            foreach (var ip in PCHoroscope.Chart.Points)
            {
                PCLotData.Add(new Models.LotData { SanskritName = ip.Id.ToString(), Sign = ip.Sign, House = ip.ClassicHouse.ToString(), Degree = TimeSpan.FromHours(ip.Degree).TotalHours, Longitude = ip.Longitude });
            }

            //PCLotData = lotDataList;
        }

        void UpdatePCPlanetDataList()
        {
            PCPlanetDataList = new ObservableCollection<Models.PlanetData>();
            var p = PCHoroscope.Chart.Asc;
            var deg = TimeSpan.FromHours(p.Degree);
            var lon = TimeSpan.FromHours(p.Longitude);
            PCPlanetDataList.Add(
                new Models.PlanetData
                {
                    PointId = p.Id,
                    Sign = p.Sign,
                    Degree = deg.TotalDegrees(),// string.Format("{0:00}:{1:00}:{2:00}", deg.Degrees(), deg.Minutes, deg.Seconds),
                    Nakshatra = p.Nakshatra.Name,
                    NakshatraAndSubRuler = p.Nakshatra.Ruler.ToString() + "/" + p.Sub.Ruler.ToString(),
                    Longitude = lon.TotalDegrees(), // string.Format("{0:00}:{1:00}:{2:00}", lon.Degrees(), lon.Minutes, lon.Seconds)
                    Mansion = p.Mansion.Name
                });
            var buffer = (from pit in PCHoroscope.Chart.Points
                          let n = pit.Id.ToString()
                          let ord = (Models.Point)Enum.Parse(typeof(Models.Point), n, true)
                          orderby ord
                          select pit).ToList();
            foreach (var pi in buffer)
            {
                if (pi.Id == Calculations.Supplementary.PointId.Asc) continue;

                deg = TimeSpan.FromHours(pi.Degree);
                lon = TimeSpan.FromHours(pi.Longitude);
                PCPlanetDataList.Add(
                    new Models.PlanetData
                    {
                        PointId = pi.Id,
                        Sign = pi.Sign,
                        Degree = deg.TotalDegrees(),// string.Format("{0:00}:{1:00}:{2:00}", deg.Degrees(), deg.Minutes, deg.Seconds),
                        Nakshatra = pi.Nakshatra.Name,
                        NakshatraAndSubRuler = pi.Nakshatra.Ruler.ToString() + "/" + pi.Sub.Ruler.ToString(),
                        Longitude = lon.TotalDegrees(), // string.Format("{0:00}:{1:00}:{2:00}", lon.Degrees(), lon.Minutes, lon.Seconds)
                        Mansion = pi.Mansion.Name
                    });
            }
        }

        void UpdatePCDivisionals()
        {
            PC_D2 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 2);
            PC_D3 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 3);
            PC_D4 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 4);
            PC_D5 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 5);
            PC_D6 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 6);
            PC_D7 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 7);
            PC_D8 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 8);
            PC_D9 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 9);
            PC_D10 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 10);
            PC_D11 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 11);
            PC_D12 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 12);
            PC_D16 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 16);
            PC_D20 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 20);
            PC_D24 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 24);
            PC_D27 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 27);
            PC_D30 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 30);
            PC_D40 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 40);
            PC_D45 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 45);
            PC_D60 = Calculations.Supplementary.Kernel.VdDivisional(PCHoroscope.Chart, 60);
        }

        void UpdatePCStartTime()
        {
            PCStartTime = Global.FromUT(PCHoroscope.BirthData.Time, BirthData.VTimezone, BirthData.VDST);
        }

        void UpdateDasas()
        {
            var calculatedDasas = Calculations.Supplementary.Kernel.VdDasha(BCHoroscope, DasaDeep);

            if (Dasas == null)
            {
                //Dasas = new List<Models.Dasa>();
                Dasas = new System.Collections.ObjectModel.ObservableCollection<Dasa>();
            }

            Dasas.Clear();

            var pc = new System.Globalization.PersianCalendar();
            Func<DateTime, string> transformer = dti => string.Format("{0:00}/{1:00}/{2:0000}", pc.GetDayOfMonth(dti), pc.GetMonth(dti), pc.GetYear(dti));

            foreach (var d in calculatedDasas)
            {
                var dasaStartTime = d.StartTime.Add(TimeSpan.FromHours(BirthData.VTimezone.TotalHours * -1d));

                if (d.Antardasa == Calculations.Supplementary.PointId.None)
                {
                    Dasas.Add(new Models.Dasa { DasaMark = string.Format("{0}", d.Mahadasa), Date = string.Format("{0:dd/MM/yyyy}", dasaStartTime), Time = string.Format("{0:HH:mm:ss}", dasaStartTime), PersianDate = transformer(dasaStartTime) });
                }
                else if (d.Pratyantardasa == Calculations.Supplementary.PointId.None)
                {
                    Dasas.Add(new Models.Dasa { DasaMark = string.Format("{0}\\{1}", d.Mahadasa, d.Antardasa), Date = string.Format("{0:dd/MM/yyyy}", dasaStartTime), Time = string.Format("{0:HH:mm:ss}", dasaStartTime), PersianDate = transformer(dasaStartTime) });
                }
                else if (d.Sookshmadasa == Calculations.Supplementary.PointId.None)
                {
                    Dasas.Add(new Models.Dasa { DasaMark = string.Format("{0}\\{1}\\{2}", d.Mahadasa, d.Antardasa, d.Pratyantardasa), Date = string.Format("{0:dd/MM/yyyy}", dasaStartTime), Time = string.Format("{0:HH:mm:ss}", dasaStartTime), PersianDate = transformer(dasaStartTime) });
                }
                else if (d.Pranadasa == Calculations.Supplementary.PointId.None)
                {
                    Dasas.Add(new Models.Dasa { DasaMark = string.Format("{0}\\{1}\\{2}\\{3}", d.Mahadasa, d.Antardasa, d.Pratyantardasa, d.Sookshmadasa), Date = string.Format("{0:dd/MM/yyyy}", dasaStartTime), Time = string.Format("{0:HH:mm:ss}", dasaStartTime), PersianDate = transformer(dasaStartTime) });
                }
                else
                {
                    Dasas.Add(new Models.Dasa { DasaMark = string.Format("{0}\\{1}\\{2}\\{3}\\{4}", d.Mahadasa, d.Antardasa, d.Pratyantardasa, d.Sookshmadasa, d.Pranadasa), Date = string.Format("{0:dd/MM/yyyy}", dasaStartTime), Time = string.Format("{0:HH:mm:ss}", dasaStartTime), PersianDate = transformer(dasaStartTime) });
                }
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<Dasa> CalculateDasas(int dasaDeep)
        {
            var calculatedDasas = Calculations.Supplementary.Kernel.VdDasha(BCHoroscope, dasaDeep);

            var dasas = new System.Collections.ObjectModel.ObservableCollection<Dasa>();

            foreach (var d in calculatedDasas)
            {
                var dasaStartTime = d.StartTime.Add(TimeSpan.FromHours(BirthData.VTimezone.TotalHours * -1d));

                if (d.Antardasa == Calculations.Supplementary.PointId.None)
                {
                    dasas.Add(new Models.Dasa { DasaMark = string.Format("{0}", d.Mahadasa), Date = string.Format("{0:dd/MM/yyyy}", dasaStartTime), Time = string.Format("{0:HH:mm:ss}", dasaStartTime) });
                }
                else if (d.Pratyantardasa == Calculations.Supplementary.PointId.None)
                {
                    dasas.Add(new Models.Dasa { DasaMark = string.Format("{0}\\{1}", d.Mahadasa, d.Antardasa), Date = string.Format("{0:dd/MM/yyyy}", dasaStartTime), Time = string.Format("{0:HH:mm:ss}", dasaStartTime) });
                }
                else if (d.Sookshmadasa == Calculations.Supplementary.PointId.None)
                {
                    dasas.Add(new Models.Dasa { DasaMark = string.Format("{0}\\{1}\\{2}", d.Mahadasa, d.Antardasa, d.Pratyantardasa), Date = string.Format("{0:dd/MM/yyyy}", dasaStartTime), Time = string.Format("{0:HH:mm:ss}", dasaStartTime) });
                }
                else if (d.Pranadasa == Calculations.Supplementary.PointId.None)
                {
                    dasas.Add(new Models.Dasa { DasaMark = string.Format("{0}\\{1}\\{2}\\{3}", d.Mahadasa, d.Antardasa, d.Pratyantardasa, d.Sookshmadasa), Date = string.Format("{0:dd/MM/yyyy}", dasaStartTime), Time = string.Format("{0:HH:mm:ss}", dasaStartTime) });
                }
                else
                {
                    dasas.Add(new Models.Dasa { DasaMark = string.Format("{0}\\{1}\\{2}\\{3}\\{4}", d.Mahadasa, d.Antardasa, d.Pratyantardasa, d.Sookshmadasa, d.Pranadasa), Date = string.Format("{0:dd/MM/yyyy}", dasaStartTime), Time = string.Format("{0:HH:mm:ss}", dasaStartTime) });
                }
            }

            return dasas;
        }

        void UpdateBCDivisionals()
        {
            BC_D2 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 2);
            BC_D3 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 3);
            BC_D4 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 4);
            BC_D5 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 5);
            BC_D6 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 6);
            BC_D7 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 7);
            BC_D8 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 8);
            BC_D9 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 9);
            BC_D10 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 10);
            BC_D11 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 11);
            BC_D12 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 12);
            BC_D16 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 16);
            BC_D20 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 20);
            BC_D24 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 24);
            BC_D27 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 27);
            BC_D30 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 30);
            BC_D40 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 40);
            BC_D45 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 45);
            BC_D60 = Calculations.Supplementary.Kernel.VdDivisional(BCHoroscope.Chart, 60);
        }

        void UpdateBCLots()
        {
            BCLots = new ObservableCollection<Calculations.Supplementary.VdLot>(Calculations.Supplementary.Kernel.VdLots(BCHoroscope.Chart));

            if (BCLotData == null) BCLotData = new System.Collections.ObjectModel.ObservableCollection<LotData>();
            BCLotData.Clear();

            //var lotDataList = new List<Models.LotData>();
            foreach (var l in BCLots)
            {
                var nameParts = l.LotMark.Split('-');
                BCLotData.Add(new Models.LotData { SanskritName = nameParts[0], PersianName = nameParts[1], Sign = l.Sign, House = l.House.ToString(), Degree = TimeSpan.FromHours(l.Degree).TotalHours, LotIndicator = "*", Longitude = l.Longitude, Description = l.Description });
            }
            foreach (var ip in BCHoroscope.Chart.Points)
            {
                BCLotData.Add(new Models.LotData { SanskritName = ip.Id.ToString(), Sign = ip.Sign, House = ip.ClassicHouse.ToString(), Degree = TimeSpan.FromHours(ip.Degree).TotalHours, Longitude = ip.Longitude });
            }

            //BCLotData = lotDataList;
        }

        void UpdateBCPlanetDataList()
        {
            BCPlanetDataList = new ObservableCollection<PlanetData>();
            var p = BCHoroscope.Chart.Asc;
            var deg = TimeSpan.FromHours(p.Degree);
            var lon = TimeSpan.FromHours(p.Longitude);
            BCPlanetDataList.Add(
                new Models.PlanetData
                {
                    PointId = p.Id,
                    Sign = p.Sign,
                    Degree = deg.TotalDegrees(),// string.Format("{0:00}:{1:00}:{2:00}", deg.Degrees(), deg.Minutes, deg.Seconds),
                    Nakshatra = p.Nakshatra.Name,
                    NakshatraAndSubRuler = p.Nakshatra.Ruler.ToString() + "/" + p.Sub.Ruler.ToString(),
                    Longitude = lon.TotalDegrees(), // string.Format("{0:00}:{1:00}:{2:00}", lon.Degrees(), lon.Minutes, lon.Seconds)
                    Mansion = p.Mansion.Name
                });
            var buffer = (from pit in BCHoroscope.Chart.Points
                          let n = pit.Id.ToString()
                          let ord = (Models.Point)Enum.Parse(typeof(Models.Point), n, true)
                          orderby ord
                          select pit).ToList();
            foreach (var pi in buffer)
            {
                if (pi.Id == Calculations.Supplementary.PointId.Asc) continue;

                deg = TimeSpan.FromHours(pi.Degree);
                lon = TimeSpan.FromHours(pi.Longitude);
                BCPlanetDataList.Add(
                    new Models.PlanetData
                    {
                        PointId = pi.Id,
                        Sign = pi.Sign,
                        Degree = deg.TotalDegrees(),// string.Format("{0:00}:{1:00}:{2:00}", deg.Degrees(), deg.Minutes, deg.Seconds),
                        Nakshatra = pi.Nakshatra.Name,
                        NakshatraAndSubRuler = pi.Nakshatra.Ruler.ToString() + "/" + pi.Sub.Ruler.ToString(),
                        Longitude = lon.TotalDegrees(), // string.Format("{0:00}:{1:00}:{2:00}", lon.Degrees(), lon.Minutes, lon.Seconds)
                        Mansion = pi.Mansion.Name
                    });
            }
        }

        void UpdatePC()
        {
            PCHoroscope = Calculations.Supplementary.Kernel.VdProgressChart(
                BCHoroscope,
                Age,
                Global.Conf);

            PC_D1 = PCHoroscope.Chart;
        }

        void UpdateBC()
        {
            BCHoroscope = Calculations.Supplementary.Kernel.VdBirthChart(
                new Calculations.Supplementary.Event(
                    Global.ToUT(BirthData.VDateTime, BirthData.VTimezone, BirthData.VDST),
                    new Calculations.Position(BirthData.VLongitude, BirthData.VLatitude, 0),
                    Global.Conf));

            BC_D1 = BCHoroscope.Chart;
        }

        private DateTime _TransientDate;
        public DateTime TransientDate
        {
            get { return _TransientDate; }
            set { Set(ref _TransientDate, value, "TransientDate"); }
        }

        private string _TransientTime;
        public string TransientTime
        {
            get { return _TransientTime; }
            set
            {
                var m = Regex.Match(value, "(?<hour>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*");

                if (!m.Success) throw new ApplicationException();

                Set(ref _TransientTime, value, "TransientTime");
            }
        }

        //private DateTime _TransientDateTime;
        public DateTime TransientDateTime
        {
            get
            {
                var m = Regex.Match(_TransientTime, "(?<hour>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*");

                if (!m.Success) throw new ApplicationException();

                int hour = int.Parse(m.Groups["hour"].Value);
                int minute = int.Parse(m.Groups["minute"].Value);
                int second = 0;
                if (m.Groups["second"].Success) int.TryParse(m.Groups["second"].Value, out second);

                var ts = new TimeSpan(hour, minute, second);

                return this._TransientDate.Date.Add(ts);
            }
            set
            {
                var v = value;
                TransientDate = v.Date;
                TransientTime = string.Format("{0:HH:mm:ss}", v);

                RaisePropertyChanged("TransientDateTime");
            }
        }

        bool _transientStarted = false;
        public bool TransientStarted { get { return _transientStarted; } set { Set(ref _transientStarted, value, "TransientStarted"); } }

        int _direction = 1;
        public int Direction { get { return _direction; } set { Set(ref _direction, value, "Direction"); } }

        double _transientStep = 0;
        public double TransientStep
        {
            get { return _transientStep; }
            set
            {
                if (Set(ref _transientStep, value, "TransientStep"))
                {
                    //if (_transientStep > 0) TransientDateTime = TransientDateTime.Add(TimeSpan.FromSeconds(_transientStep * Direction));
                }
            }
        }

        SevenAstro2.Calculations.Supplementary.VdHoroscope _transientChart;
        public SevenAstro2.Calculations.Supplementary.VdHoroscope TransientChart { get { return _transientChart; } set { Set(ref _transientChart, value, "TransientChart"); } }

        ObservableCollection<Models.PlanetData> _TransientPlanetData;
        public ObservableCollection<Models.PlanetData> TransientPlanetData { get { return _TransientPlanetData; } set { Set(ref _TransientPlanetData, value, "TransientPlanetData"); } }

        private SevenAstro2.Calculations.Supplementary.VdChart _TransientD9;
        public SevenAstro2.Calculations.Supplementary.VdChart TransientD9 { get { return _TransientD9; } set { Set(ref _TransientD9, value, "TransientD9"); } }

        public void UpdateTransient()
        {
            if (TransientStep > 0) TransientDateTime = TransientDateTime.Add(TimeSpan.FromSeconds(TransientStep * Direction));
            TransientChart = Calculations.Supplementary.Kernel.VdBirthChart(
                new Calculations.Supplementary.Event(
                    Global.ToUT(TransientDateTime, BirthData.VTimezone, BirthData.VDST),
                    BCHoroscope.BirthData.Position,
                    Global.Conf));

            //transientView.TransientChart = transientChart.Chart;
            //transientView.ClearAndDrawDiagram();

            //transientD9Chart.Chart = Calculations.Supplementary.Kernel.VdDivisional(transientChart.Chart, 9);
            //transientD9Chart.ClearAndDrawChart();

            TransientD9 = Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 9);

            TransientPlanetData = new ObservableCollection<PlanetData>();
            var buffer = (from pit in TransientChart.Chart.Points
                          let n = pit.Id.ToString()
                          let ord = (Models.Point)Enum.Parse(typeof(Models.Point), n, true)
                          orderby ord
                          select pit).ToList();
            foreach (var p in buffer)
            {
                var deg = TimeSpan.FromHours(p.Degree);
                var lon = TimeSpan.FromHours(p.Longitude);
                TransientPlanetData.Add(
                    new Models.PlanetData
                    {
                        PointId = p.Id,
                        Sign = p.Sign,
                        Degree = deg.TotalHours,
                        Nakshatra = p.Nakshatra.Name,
                        NakshatraAndSubRuler = p.Nakshatra.Ruler.ToString() + "/" + p.Sub.Ruler.ToString(),
                        Longitude = lon.TotalHours,
                        Mansion = p.Mansion.Name
                    });
            }
            //dgTransientData.DataContext = planetDataList;

            UpdateTransientPanchaDwaadasa();
        }

        //void UpdateTransientDwaadasa()
        //{
        //    if (TransientDwaadasa == null) TransientDwaadasa = new ObservableCollection<DwaadasaViewModel>();
        //    TransientDwaadasa.Clear();

        //    var dwaadasa = SevenAstro2.Calculations.Supplementary.Kernel.DwaadasaVargeeyaBala(
        //        //TransientChart.Chart,
        //        Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 2),
        //        Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 3),
        //        Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 4),
        //        Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 5),
        //        Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 6),
        //        Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 7),
        //        Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 8),
        //        TransientD9,
        //        Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 10),
        //        Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 11),
        //        Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 12),
        //        Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 24));
        //    foreach (var p in dwaadasa.Keys)
        //    {
        //        if (dwaadasa.ContainsKey(p)) TransientDwaadasa.Add(new DwaadasaViewModel { PointId = p, Dwaadasa = dwaadasa[p] });
        //    }
        //}


        //void UpdateTransientPancha()
        //{
        //    if (TransientPancha == null) TransientPancha = new ObservableCollection<PanchaViewModel>();
        //    TransientPancha.Clear();

        //    var pancha = SevenAstro2.Calculations.Supplementary.Kernel.PanchaVargeeyaBala(TransientChart.Chart, Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 3), TransientD9);
        //    foreach (var p in TransientPlanetData)
        //    {
        //        if (pancha.ContainsKey(p.PointId)) TransientPancha.Add(new PanchaViewModel { PointId = p.PointId, Pancha = pancha[p.PointId].Item2 });
        //    }
        //}

        void UpdateTransientPanchaDwaadasa()
        {
            if (TransientPanchaDwaadasa == null) TransientPanchaDwaadasa = new ObservableCollection<PanchaDwaadasaViewModel>();
            TransientPanchaDwaadasa.Clear();

            var pancha = SevenAstro2.Calculations.Supplementary.Kernel.PanchaVargeeyaBala(TransientChart.Chart, Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 3), TransientD9);
            var dwaadasa = SevenAstro2.Calculations.Supplementary.Kernel.DwaadasaVargeeyaBala(
                //TransientChart.Chart,
                Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 2),
                Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 3),
                Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 4),
                Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 5),
                Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 6),
                Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 7),
                Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 8),
                TransientD9,
                Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 10),
                Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 11),
                Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 12),
                Calculations.Supplementary.Kernel.VdDivisional(TransientChart.Chart, 24));

            foreach (var p in dwaadasa.Keys)
            {
                var item = new PanchaDwaadasaViewModel { PointId = p };

                if (pancha.ContainsKey(p)) item.Pancha = pancha[p].Item2;
                if (dwaadasa.ContainsKey(p)) item.Dwaadasa = dwaadasa[p];

                if (item.Pancha.HasValue || item.Dwaadasa.HasValue)
                    TransientPanchaDwaadasa.Add(item);
            }
        }

        private Calculations.Supplementary.PointId _MatchPlanet;
        public Calculations.Supplementary.PointId MatchPlanet { get { return _MatchPlanet; } set { Set(ref _MatchPlanet, value, "MatchPlanet"); } }

        private Calculations.Supplementary.Sign _MatchSign = Calculations.Supplementary.Sign.Aries;
        public Calculations.Supplementary.Sign MatchSign { get { return _MatchSign; } set { Set(ref _MatchSign, value, "MatchSign"); } }

        private string _MatchDegreeStr = "00:00:00";
        public string MatchDegreeStr
        {
            get { return _MatchDegreeStr; }
            set
            {
                var m = Regex.Match(value, "(?<degree>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*");

                if (!m.Success) throw new ApplicationException();

                Set(ref _MatchDegreeStr, value, "MatchDegreeStr");
            }
        }

        public double MatchDegree
        {
            get
            {
                var m = Regex.Match(_MatchDegreeStr, "(?<degree>\\d+):(?<minute>\\d+)(:(?<second>\\d+))*");

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
                MatchDegreeStr = ts.ShowAsDegree();

                RaisePropertyChanged("MatchDegree");
            }
        }

        public double MatchLongitude
        {
            get { return ((int)MatchSign - 1) * 30d + MatchDegree; }
            //set
            //{
            //    MatchDegree = value % 30;
            //    RaisePropertyChanged();
            //}
        }

        public string[] MatchPlanets
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

        public string[] MatchSigns
        {
            get
            {
                //return new string[]
                //{
                //    Calculations.Supplementary.Sign.Aries.ToString(),
                //    Calculations.Supplementary.Sign.Taurus.ToString(),
                //    Calculations.Supplementary.Sign.Gemini.ToString(),
                //    Calculations.Supplementary.Sign.Cancer.ToString(),
                //    Calculations.Supplementary.Sign.Leo.ToString(),
                //    Calculations.Supplementary.Sign.Virgo.ToString(),
                //    Calculations.Supplementary.Sign.Libra.ToString(),
                //    Calculations.Supplementary.Sign.Scorpio.ToString(),
                //    Calculations.Supplementary.Sign.Sagittarius.ToString(),
                //    Calculations.Supplementary.Sign.Capricorn.ToString(),
                //    Calculations.Supplementary.Sign.Aquarius.ToString(),
                //    Calculations.Supplementary.Sign.Pisces.ToString()
                //};
                return (from sn in Enum.GetNames(typeof(Calculations.Supplementary.Sign))
                        where sn != Calculations.Supplementary.Sign.None.ToString()
                        select sn).ToArray();
            }
        }

        ObservableCollection<Models.YearLordCandidate> _YearLords;
        public ObservableCollection<Models.YearLordCandidate> YearLords
        {
            get
            {
                var yearLords = new ObservableCollection<Models.YearLordCandidate>();
                var muntha = Calculations.Supplementary.Kernel.VdExtractMuntha(BCHoroscope.Chart, Age);
                yearLords.Add(new Models.YearLordCandidate { Name = "Muntha", Memo = string.Format("{0} ({1})", muntha.Item1, muntha.Item2) });
                var bcLord = Calculations.Supplementary.Kernel.VdExtractBirthChartLord(BCHoroscope.Chart);
                yearLords.Add(new Models.YearLordCandidate { Name = "Birth Chart Asc Lord", Memo = bcLord.ToString() });
                var pcLord = Calculations.Supplementary.Kernel.VdExtractProgressChartLord(PCHoroscope.Chart);
                yearLords.Add(new Models.YearLordCandidate { Name = "Progress Chart Asc Lord", Memo = pcLord.ToString() });
                var trirashiLord = Calculations.Supplementary.Kernel.VdExtractTrirashiLord(PCHoroscope.Chart);
                yearLords.Add(new Models.YearLordCandidate { Name = "Trirashi Lord", Memo = trirashiLord.ToString() });
                var dinaRatriLord = Calculations.Supplementary.Kernel.VdExtractDinaRatriLord(PCHoroscope.Chart);
                yearLords.Add(new Models.YearLordCandidate { Name = "Dina-Ratri Lord", Memo = dinaRatriLord.ToString() });

                _YearLords = yearLords;

                return _YearLords;
            }
            set { RaisePropertyChanged("YearLords"); }
        }

        Models.JDData _JDData;
        public Models.JDData JDData { get { return _JDData; } set { Set(ref _JDData, value, "JDData"); } }

        private string _JD;
        public string JD { get { return _JD; } set { Set(ref _JD, value, "JD"); } }

        private double _JDPCStart;
        public double JDPCStart { get { return _JDPCStart; } set { Set(ref _JDPCStart, value, "JDPCStart"); } }

        private double _JDEvent;
        public double JDEvent { get { return _JDEvent; } set { Set(ref _JDEvent, value, "JDEvent"); } }

        private string _JDDiff;
        public string JDDiff { get { return _JDDiff; } set { Set(ref _JDDiff, value, "JDDiff"); } }

        public void UpdateJD()
        {
            var eventTime = Global.ToUT(JDData.VDateTime, BirthData.VTimezone, BirthData.VDST);
            if (eventTime < BirthData.VDateTime)
            {
                JD = "Event time must be greater than birth time.";
                return;
            }

            var bufferAge = Age;
            int deltaYears = Convert.ToInt32(Math.Floor((eventTime - BirthData.VDateTime).TotalDays / Calculations.Supplementary.Kernel.YearLen));
            Age = deltaYears;

            var pcTime = PCHoroscope.BirthData.Time;

            var jdDegree = Calculations.Supplementary.Kernel.DeltaJD(pcTime, eventTime);

            if (jdDegree == -1) JD = "Event time must be greater than PC start time.";
            else JD = string.Format("{0:0.00}" + "°", jdDegree);

            var pcChanged = false;
            if (bufferAge != Age)
            {
                JD = string.Format("{0} (PC Changed)", JD);
                pcChanged = true;
            }

            JDPCStart = Calculations.Supplementary.Kernel.JD(pcTime);
            JDEvent = Calculations.Supplementary.Kernel.JD(eventTime);
            JDDiff = Math.Round(JDEvent - JDPCStart, 2).ToString();

            if (pcChanged) JDDiff = string.Format("{0} (PC Changed)", JDDiff);
        }

        public void UpdateAge()
        {
            var eventTime = Global.ToUT(DateTime.Now, BirthData.VTimezone, BirthData.VDST);
            int deltaYears = Convert.ToInt32(Math.Floor((eventTime - BirthData.VDateTime).TotalDays / Calculations.Supplementary.Kernel.YearLen));
            Age = deltaYears;
        }

        ICommand _UpdateAgeCommand;
        public ICommand UpdateAgeCommand
        {
            get
            {
                if (_UpdateAgeCommand == null)
                {
                    _UpdateAgeCommand = new RelayCommand(o =>
                    {
                        UpdateAge();
                    });
                }

                return _UpdateAgeCommand;
            }
        }
    }

    class AspectsRow
    {
        public string First { get; set; }
        public string Asc { get; set; }
        public string Su { get; set; }
        public string Mo { get; set; }
        public string Ma { get; set; }
        public string Me { get; set; }
        public string Ju { get; set; }
        public string Ve { get; set; }
        public string Sa { get; set; }
        public string Ra { get; set; }
        public string Ke { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SevenAstro2.Views
{
    /// <summary>
    /// Interaction logic for BirthDataView.xaml
    /// </summary>
    internal partial class BirthDataView : Window
    {
        public BirthDataView()
        {
            InitializeComponent();

            LocationTextBox.TextChanged += LocationTextBox_TextChanged;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Global.BirthDataView = null;
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            var data = this.DataContext as Models.CircumstanceViewModel;

            if (data != null)
            {
                data.Update();

                //data.UpdateAge();
            }
        }

        void LocationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LocationDB == null || LocationDB.Count == 0)
            {
                ClearSuggestions();
                return;
            }

            string entry = LocationTextBox.Text;
            if (string.IsNullOrWhiteSpace(entry))
            {
                ClearSuggestions();
                return;
            }

            var autos = new List<string>();

            foreach (var loc in LocationDB)
            {
                if (loc == null || string.IsNullOrWhiteSpace(loc.Name)) continue;

                if (loc.Name.Text().ToUpper().StartsWith(entry.Text().ToUpper())) autos.Add(loc.Name);
            }

            if (autos.Count == 0)
            {
                ClearSuggestions();
                return;
            }

            LocationSuggestionListBox.ItemsSource = autos;
            LocationSuggestionListBox.Visibility = System.Windows.Visibility.Visible;
        }

        void LocationSuggestionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LocationSuggestionListBox.ItemsSource == null) return;

            LocationSuggestionListBox.Visibility = System.Windows.Visibility.Collapsed;
            if (LocationSuggestionListBox.SelectedIndex == -1) return;

            LocationTextBox.TextChanged -= LocationTextBox_TextChanged;

            //LocationTextBox.Text = LocationSuggestionListBox.SelectedItem.ToString();
            var data = this.DataContext as Models.CircumstanceViewModel;
            if (data != null)
            {
                //var loc = LocationDB[LocationSuggestionListBox.SelectedIndex];
                var loc = (from l in LocationDB where string.Compare(l.Name, LocationSuggestionListBox.SelectedItem.ToString(), true) == 0 select l).FirstOrDefault();

                if (loc != null)
                {
                    data.BirthData.BirthPlace = loc.Name;
                    data.BirthData.Longitude = loc.Longitude;
                    data.BirthData.Latitude = loc.Latitude;
                    data.BirthData.Timezone = loc.Timezone;
                    data.BirthData.DST = loc.DST;
                }
            }

            LocationTextBox.TextChanged += LocationTextBox_TextChanged;

        }

        void ClearSuggestions()
        {
            LocationSuggestionListBox.Visibility = System.Windows.Visibility.Collapsed;
            LocationSuggestionListBox.ItemsSource = null;
        }

        List<Models.Location> _locationDB;
        List<Models.Location> LocationDB
        {
            get
            {
                if (_locationDB == null)
                {
                    _locationDB = new LocationDB().GetLocations();
                }

                return _locationDB;
            }
            set
            {
                _locationDB = value;
                new LocationDB().SetLocations(_locationDB);
            }
        }

        private void btnRememberLocation_Click(object sender, RoutedEventArgs e)
        {
            var data = this.DataContext as Models.CircumstanceViewModel;

            if (data == null) return;
            var loc = (from l in _locationDB where string.Compare(l.Name, data.BirthData.BirthPlace, true) == 0 select l).FirstOrDefault();

            if (loc == null)
            {
                loc = new Models.Location { Name = data.BirthData.BirthPlace, Longitude = data.BirthData.Longitude, Latitude = data.BirthData.Latitude, Timezone = data.BirthData.Timezone, DST = data.BirthData.DST };
                _locationDB.Add(loc);
            }
            else
            {
                loc.Name = data.BirthData.BirthPlace;
                loc.Longitude = data.BirthData.Longitude;
                loc.Latitude = data.BirthData.Latitude;
                loc.Timezone = data.BirthData.Timezone;
                loc.DST = data.BirthData.DST;
            }

            new LocationDB().SetLocations(_locationDB);

            MessageBox.Show("Location Saved");

            _locationDB = null;
        }
    }

    class LocationDB
    {
        public List<Models.Location> GetLocations()
        {
            var dbFile = System.IO.Path.Combine(Global.WorkingDir, "locations.set");

            if (!System.IO.File.Exists(dbFile))
            {
                CreateSampleDB(dbFile);
            }

            List<string[]> records = null;

            using (var csvFile = new Microsoft.VisualBasic.FileIO.TextFieldParser(dbFile, Encoding.UTF8)) records = csvFile.Records().ToList();
            if (records.Count == 0)
            {
                CreateSampleDB(dbFile);

                using (var csvFile = new Microsoft.VisualBasic.FileIO.TextFieldParser(dbFile, Encoding.UTF8)) records = csvFile.Records().ToList();
            }

            return (from rec in records let l = Models.Location.From(rec) orderby l.Name select l).ToList();
        }

        public void SetLocations(List<Models.Location> locations)
        {
            var dbFile = System.IO.Path.Combine(Global.WorkingDir, "locations.set");

            using (var writer = new System.IO.StreamWriter(dbFile, false, Encoding.UTF8))
            {
                foreach (var loc in locations.OrderBy(l => l.Name)) writer.WriteRecord(loc.Fields);

                writer.Flush();
            }
        }

        static void CreateSampleDB(string dbFile)
        {
            using (var writer = new System.IO.StreamWriter(dbFile, false, Encoding.UTF8))
            {
                foreach (var loc in SampleLocations().OrderBy(l => l.Name)) writer.WriteRecord(loc.Fields);

                writer.Flush();
            }
        }

        static IEnumerable<Models.Location> SampleLocations()
        {
            yield return new Models.Location { Name = "Tehran", Longitude = "51:17", Latitude = "35:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حسن آباد", Longitude = "52:27:21", Latitude = "30:31:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوهنجان", Longitude = "52:57:22", Latitude = "29:13:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اسیر", Longitude = "52:39:56", Latitude = "27:43:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دوزه", Longitude = "52:57:30", Latitude = "28:42:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "عمادده", Longitude = "53:51:42", Latitude = "27:26:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خومه زار", Longitude = "51:34:41", Latitude = "30:00:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هماشهر", Longitude = "52:05:15", Latitude = "30:06:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جهرم", Longitude = "53:34:00", Latitude = "28:29:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قطب آباد", Longitude = "53:38:22", Latitude = "28:38:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زاهدشهر", Longitude = "53:48:16", Latitude = "28:44:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نوبندگان", Longitude = "53:49:33", Latitude = "28:51:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قیر", Longitude = "53:02:38", Latitude = "28:28:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کارزین", Longitude = "53:06:30", Latitude = "28:24:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "افزر", Longitude = "52:57:56", Latitude = "28:20:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جویم", Longitude = "53:58:47", Latitude = "28:15:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اشکنان", Longitude = "53:36:29", Latitude = "27:13:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اهل", Longitude = "53:39:35", Latitude = "27:12:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بیرم", Longitude = "53:30:54", Latitude = "27:25:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خنج", Longitude = "53:25:46", Latitude = "27:53:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "علامرودشت", Longitude = "53:00:02", Latitude = "27:37:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مهر", Longitude = "52:53:01", Latitude = "27:33:01", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "وراوی", Longitude = "53:03:07", Latitude = "27:27:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گله دار", Longitude = "52:39:36", Latitude = "27:38:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اوز", Longitude = "54:00:44", Latitude = "27:45:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "امام شهر", Longitude = "53:09:02", Latitude = "28:26:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مبارک آباد", Longitude = "53:19:43", Latitude = "28:21:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لامرد", Longitude = "53:11:16", Latitude = "27:19:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سده", Longitude = "52:10:34", Latitude = "30:42:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کامفیروز", Longitude = "52:11:39", Latitude = "30:19:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اردکان", Longitude = "51:59:31", Latitude = "30:13:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مصیری", Longitude = "51:31:44", Latitude = "30:14:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نورآباد", Longitude = "51:31:20", Latitude = "30:06:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مرودشت", Longitude = "52:48:36", Latitude = "29:51:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آباده طشک", Longitude = "53:43:50", Latitude = "29:48:35", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ارسنجان", Longitude = "53:18:02", Latitude = "29:54:35", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زرقان", Longitude = "52:43:18", Latitude = "29:46:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خرامه", Longitude = "53:18:36", Latitude = "29:29:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سروستان", Longitude = "53:13:25", Latitude = "29:15:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رونیز", Longitude = "53:46:09", Latitude = "29:11:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لپوئی", Longitude = "52:39:07", Latitude = "29:47:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "داریان", Longitude = "52:55:12", Latitude = "29:33:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کازرون", Longitude = "51:39:06", Latitude = "29:36:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خشت", Longitude = "51:20:07", Latitude = "29:33:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اقلید", Longitude = "52:42:18", Latitude = "30:54:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صفاشهر", Longitude = "53:11:55", Latitude = "30:36:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بوانات", Longitude = "53:38:20", Latitude = "30:27:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قادرآباد", Longitude = "53:15:18", Latitude = "30:16:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قائمیه", Longitude = "51:35:27", Latitude = "29:50:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بالاده", Longitude = "51:56:20", Latitude = "29:17:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نودان", Longitude = "51:41:39", Latitude = "29:48:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "باب انار", Longitude = "53:12:32", Latitude = "28:58:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خاوران", Longitude = "53:18:47", Latitude = "28:56:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فسا", Longitude = "53:38:15", Latitude = "28:55:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ششده", Longitude = "53:59:44", Latitude = "28:56:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سعادت شهر", Longitude = "53:08:10", Latitude = "30:04:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کره ای", Longitude = "53:42:56", Latitude = "30:01:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ایزدخواست", Longitude = "52:07:29", Latitude = "31:31:01", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بیضا", Longitude = "52:23:51", Latitude = "29:58:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بهمن", Longitude = "52:29:07", Latitude = "31:11:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صغاد", Longitude = "52:30:55", Latitude = "31:11:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آباده", Longitude = "52:39:00", Latitude = "31:08:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سورمق", Longitude = "52:50:22", Latitude = "31:02:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیدان", Longitude = "53:00:20", Latitude = "30:00:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کنارتخته", Longitude = "51:23:41", Latitude = "29:32:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوار", Longitude = "52:41:12", Latitude = "29:11:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شیراز", Longitude = "52:31:44", Latitude = "29:33:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "داراب", Longitude = "54:32:49", Latitude = "28:45:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جنت شهر", Longitude = "54:41:06", Latitude = "28:39:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شهرپیر", Longitude = "54:20:06", Latitude = "28:18:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دبیران", Longitude = "54:11:12", Latitude = "28:23:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حاجی آباد", Longitude = "54:25:13", Latitude = "28:21:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لار", Longitude = "54:19:13", Latitude = "27:38:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گراش", Longitude = "54:08:42", Latitude = "27:40:01", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خور", Longitude = "54:20:38", Latitude = "27:38:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فدامی", Longitude = "55:08:06", Latitude = "28:13:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لطیفی", Longitude = "54:23:12", Latitude = "27:41:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نی ریز", Longitude = "54:19:21", Latitude = "29:11:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ایج", Longitude = "54:14:43", Latitude = "29:01:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مشکان", Longitude = "54:20:45", Latitude = "29:28:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فراشبند", Longitude = "52:05:37", Latitude = "28:51:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فیروزآباد", Longitude = "52:34:11", Latitude = "28:50:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میمند", Longitude = "52:45:03", Latitude = "28:52:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بنارویه", Longitude = "54:02:44", Latitude = "28:05:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "استهبان", Longitude = "54:02:40", Latitude = "29:07:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خانه زنیان", Longitude = "52:08:58", Latitude = "29:40:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قطرویه", Longitude = "54:42:12", Latitude = "29:08:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دهرم", Longitude = "52:18:15", Latitude = "28:29:35", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نوجین", Longitude = "52:00:48", Latitude = "29:07:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رامجرد", Longitude = "52:35:33", Latitude = "30:04:29", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دوبرجی", Longitude = "55:11:33", Latitude = "28:18:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دژکرد", Longitude = "51:57:28", Latitude = "30:42:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حسامی", Longitude = "53:52:20", Latitude = "29:58:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شهر جدید صدرا", Longitude = "52:30:30", Latitude = "29:48:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شاپورآباد", Longitude = "51:44:35", Latitude = "32:50:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گلشن", Longitude = "51:45:09", Latitude = "31:55:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نصرآباد", Longitude = "52:03:43", Latitude = "32:16:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دهاقان", Longitude = "51:39:15", Latitude = "31:55:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کمه", Longitude = "51:35:36", Latitude = "31:03:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ونک", Longitude = "51:19:35", Latitude = "31:31:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سمیرم", Longitude = "51:34:18", Latitude = "31:24:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حنا", Longitude = "51:43:32", Latitude = "31:11:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ورزنه", Longitude = "52:39:02", Latitude = "32:25:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حسن آباد", Longitude = "52:37:24", Latitude = "32:08:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هرند", Longitude = "52:26:14", Latitude = "32:33:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اژیه", Longitude = "52:22:46", Latitude = "32:26:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نیک آباد", Longitude = "52:12:18", Latitude = "32:18:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محمدآباد", Longitude = "52:06:14", Latitude = "32:19:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شهرضا", Longitude = "51:50:57", Latitude = "31:58:35", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "منظریه", Longitude = "51:52:19", Latitude = "31:56:35", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بهارستان", Longitude = "51:46:44", Latitude = "32:29:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فلاورجان", Longitude = "51:30:22", Latitude = "32:33:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کلیشادوسودرجان", Longitude = "51:32:09", Latitude = "32:32:29", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بهاران شهر", Longitude = "51:32:29", Latitude = "32:30:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پیربکران", Longitude = "51:33:21", Latitude = "32:27:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ایمانشهر", Longitude = "51:27:40", Latitude = "32:28:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فولادشهر", Longitude = "51:24:26", Latitude = "32:29:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زیباشهر", Longitude = "51:33:53", Latitude = "32:22:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دیزیچه", Longitude = "51:30:14", Latitude = "32:22:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ابریشم", Longitude = "51:34:25", Latitude = "32:33:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مبارکه", Longitude = "51:30:02", Latitude = "32:20:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کرکوند", Longitude = "51:26:14", Latitude = "32:20:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ورنامخواست", Longitude = "51:22:48", Latitude = "32:21:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سده لنجان", Longitude = "51:19:08", Latitude = "32:22:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زاینده رود", Longitude = "51:16:23", Latitude = "32:22:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "باغ بهادران", Longitude = "51:11:28", Latitude = "32:22:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چرمهین", Longitude = "51:11:43", Latitude = "32:20:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قهدریجان", Longitude = "51:27:21", Latitude = "32:34:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "طالخونچه", Longitude = "51:33:51", Latitude = "32:15:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زرین شهر", Longitude = "51:23:09", Latitude = "32:23:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چمگردان", Longitude = "51:20:07", Latitude = "32:23:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جوزدان", Longitude = "51:22:18", Latitude = "32:33:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نیاسر", Longitude = "51:08:56", Latitude = "33:58:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جوشقان وکامو", Longitude = "51:13:41", Latitude = "33:35:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "برزک", Longitude = "51:13:48", Latitude = "33:46:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کمشچه", Longitude = "51:48:32", Latitude = "32:54:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کهریزسنگ", Longitude = "51:28:37", Latitude = "32:37:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آران وبیدگل", Longitude = "51:28:56", Latitude = "34:03:29", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نوش آباد", Longitude = "51:26:16", Latitude = "34:04:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "انارک", Longitude = "53:41:49", Latitude = "33:18:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نائین", Longitude = "53:05:44", Latitude = "32:51:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تودشک", Longitude = "52:40:44", Latitude = "32:43:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوهپایه", Longitude = "52:25:57", Latitude = "32:42:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سگزی", Longitude = "52:07:49", Latitude = "32:40:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حبیب آباد", Longitude = "51:46:33", Latitude = "32:49:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دولت آباد", Longitude = "51:41:36", Latitude = "32:48:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دستگرد", Longitude = "51:39:56", Latitude = "32:48:07", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خورزوق", Longitude = "51:38:59", Latitude = "32:46:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خوراسگان", Longitude = "51:45:34", Latitude = "32:39:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خمینی شهر", Longitude = "51:32:00", Latitude = "32:40:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گلدشت", Longitude = "51:26:33", Latitude = "32:37:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تیران", Longitude = "51:09:11", Latitude = "32:42:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رضوانشهر", Longitude = "51:06:13", Latitude = "32:41:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چادگان", Longitude = "50:37:36", Latitude = "32:46:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "عسگران", Longitude = "50:51:07", Latitude = "32:51:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رزوه", Longitude = "50:34:09", Latitude = "32:50:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "داران", Longitude = "50:24:32", Latitude = "32:59:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فریدونشهر", Longitude = "50:08:21", Latitude = "32:56:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "برف انبار", Longitude = "50:11:37", Latitude = "32:59:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نجف آباد", Longitude = "51:21:02", Latitude = "32:37:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شاهین شهر", Longitude = "51:33:16", Latitude = "32:51:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوشک", Longitude = "51:30:00", Latitude = "32:38:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بوئین ومیاندشت", Longitude = "50:09:31", Latitude = "33:04:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "افوس", Longitude = "50:05:34", Latitude = "33:01:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دامنه", Longitude = "50:29:15", Latitude = "33:00:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خوانسار", Longitude = "50:19:07", Latitude = "33:12:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گلپایگان", Longitude = "50:17:00", Latitude = "33:27:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گوگد", Longitude = "50:20:43", Latitude = "33:28:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گلشهر", Longitude = "50:27:56", Latitude = "33:30:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دهق", Longitude = "50:57:33", Latitude = "33:06:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "علویچه", Longitude = "51:04:53", Latitude = "33:03:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "وزوان", Longitude = "51:11:00", Latitude = "33:25:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میمه", Longitude = "51:10:13", Latitude = "33:26:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اردستان", Longitude = "52:22:41", Latitude = "33:21:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زواره", Longitude = "52:29:11", Latitude = "33:26:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مهاباد", Longitude = "52:13:02", Latitude = "33:31:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بادرود", Longitude = "52:00:40", Latitude = "33:41:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خالدآباد", Longitude = "51:58:53", Latitude = "33:42:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نطنز", Longitude = "51:55:08", Latitude = "33:30:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قمصر", Longitude = "51:25:50", Latitude = "33:45:07", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ابوزیدآباد", Longitude = "51:46:04", Latitude = "33:54:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کاشان", Longitude = "51:25:55", Latitude = "33:59:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مشکات", Longitude = "51:16:05", Latitude = "34:10:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سفیدشهر", Longitude = "51:21:05", Latitude = "34:07:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گرگاب", Longitude = "51:35:51", Latitude = "32:51:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قهجاورستان", Longitude = "51:50:06", Latitude = "32:42:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لای بید", Longitude = "50:41:38", Latitude = "33:27:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جندق", Longitude = "54:24:52", Latitude = "34:02:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خور", Longitude = "55:05:01", Latitude = "33:46:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "درچه پیاز", Longitude = "51:32:51", Latitude = "32:36:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اصفهان", Longitude = "51:39:32", Latitude = "32:34:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گزبرخوار", Longitude = "51:37:07", Latitude = "32:48:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فرخی", Longitude = "54:56:48", Latitude = "33:50:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بافران", Longitude = "53:08:31", Latitude = "32:50:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پول", Longitude = "51:35:23", Latitude = "36:23:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "امیرکلا", Longitude = "52:39:45", Latitude = "36:35:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بابل", Longitude = "52:40:33", Latitude = "36:31:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کیاکلا", Longitude = "52:48:53", Latitude = "36:34:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کله بست", Longitude = "52:37:36", Latitude = "36:38:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بهنمیر", Longitude = "52:45:44", Latitude = "36:40:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گتاب", Longitude = "52:39:23", Latitude = "36:24:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جویبار", Longitude = "52:53:59", Latitude = "36:38:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قائم شهر", Longitude = "52:51:54", Latitude = "36:27:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ساری", Longitude = "53:03:36", Latitude = "36:34:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سورک", Longitude = "53:12:42", Latitude = "36:35:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نکا", Longitude = "53:17:37", Latitude = "36:39:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رستمکلا", Longitude = "53:25:33", Latitude = "36:40:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بهشهر", Longitude = "53:32:04", Latitude = "36:41:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گلوگاه", Longitude = "53:48:39", Latitude = "36:43:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوهی خیل", Longitude = "52:54:25", Latitude = "36:41:08", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مرزیکلا", Longitude = "52:44:09", Latitude = "36:21:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شیرگاه", Longitude = "52:52:51", Latitude = "36:17:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زیرآب", Longitude = "52:58:28", Latitude = "36:10:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پل سفید", Longitude = "53:03:27", Latitude = "36:06:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گلوگاه", Longitude = "52:37:21", Latitude = "36:18:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فریم", Longitude = "53:16:00", Latitude = "36:10:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کیاسر", Longitude = "53:32:21", Latitude = "36:14:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خوش رودپی", Longitude = "52:33:56", Latitude = "36:22:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خلیل شهر", Longitude = "53:38:22", Latitude = "36:42:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آلاشت", Longitude = "52:50:16", Latitude = "36:03:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گزنک", Longitude = "52:13:09", Latitude = "35:54:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رینه", Longitude = "52:10:11", Latitude = "35:52:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آمل", Longitude = "52:21:21", Latitude = "36:28:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کلاردشت", Longitude = "51:08:41", Latitude = "36:29:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مرزن آباد", Longitude = "51:17:44", Latitude = "36:26:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بلده", Longitude = "51:48:21", Latitude = "36:12:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دابودشت", Longitude = "52:27:07", Latitude = "36:28:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چمستان", Longitude = "52:07:16", Latitude = "36:28:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رامسر", Longitude = "50:39:44", Latitude = "36:54:57", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کتالم وسادات شهر", Longitude = "50:43:01", Latitude = "36:52:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تنکابن", Longitude = "50:52:27", Latitude = "36:48:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خرم آباد", Longitude = "50:52:02", Latitude = "36:47:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شیرود", Longitude = "50:47:25", Latitude = "36:50:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نشتارود", Longitude = "51:02:06", Latitude = "36:44:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "عباس آباد", Longitude = "51:06:57", Latitude = "36:43:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سلمان شهر", Longitude = "51:11:59", Latitude = "36:41:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چالوس", Longitude = "51:25:28", Latitude = "36:39:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کلارآباد", Longitude = "51:15:45", Latitude = "36:41:29", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نوشهر", Longitude = "51:29:59", Latitude = "36:38:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رویان", Longitude = "51:58:07", Latitude = "36:34:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زرگر محله", Longitude = "52:34:27", Latitude = "36:30:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نور", Longitude = "52:00:57", Latitude = "36:34:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ایزدشهر", Longitude = "52:08:21", Latitude = "36:36:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محمودآباد", Longitude = "52:15:08", Latitude = "36:37:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فریدونکنار", Longitude = "52:31:00", Latitude = "36:40:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرخرود", Longitude = "52:27:24", Latitude = "36:40:35", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بابلسر", Longitude = "52:38:57", Latitude = "36:41:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فرون آباد", Longitude = "51:37:37", Latitude = "35:30:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بومهن", Longitude = "51:52:07", Latitude = "35:43:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پردیس", Longitude = "51:49:17", Latitude = "35:44:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رودهن", Longitude = "51:54:37", Latitude = "35:44:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آبعلی", Longitude = "51:57:46", Latitude = "35:45:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قدس", Longitude = "51:06:46", Latitude = "35:42:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چهاردانگه", Longitude = "51:18:16", Latitude = "35:35:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تهران", Longitude = "51:20:59", Latitude = "35:42:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ری", Longitude = "51:26:19", Latitude = "35:36:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تجریش", Longitude = "51:27:53", Latitude = "35:48:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "وحیدیه", Longitude = "51:01:33", Latitude = "35:36:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فردوسیه", Longitude = "51:03:47", Latitude = "35:36:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شاهدشهر", Longitude = "51:05:14", Latitude = "35:34:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صباشهر", Longitude = "51:06:51", Latitude = "35:34:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نسیم شهر", Longitude = "51:10:10", Latitude = "35:33:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رباط کریم", Longitude = "51:04:47", Latitude = "35:28:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گلستان", Longitude = "51:09:43", Latitude = "35:30:57", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صالح آباد", Longitude = "51:11:20", Latitude = "35:30:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حسن آباد", Longitude = "51:14:34", Latitude = "35:22:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اسلامشهر", Longitude = "51:13:31", Latitude = "35:30:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کهریزک", Longitude = "51:21:18", Latitude = "35:31:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "باقرشهر", Longitude = "51:24:11", Latitude = "35:31:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جوادآباد", Longitude = "51:40:23", Latitude = "35:12:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پیشوا", Longitude = "51:43:20", Latitude = "35:18:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ورامین", Longitude = "51:38:04", Latitude = "35:19:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قرچک", Longitude = "51:35:12", Latitude = "35:24:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پاکدشت", Longitude = "51:40:32", Latitude = "35:28:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شریف آباد", Longitude = "51:47:09", Latitude = "35:25:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لواسان", Longitude = "51:38:14", Latitude = "35:49:09", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دماوند", Longitude = "52:02:49", Latitude = "35:41:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فشم", Longitude = "51:31:11", Latitude = "35:54:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آبسرد", Longitude = "52:09:00", Latitude = "35:37:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کیلان", Longitude = "52:09:46", Latitude = "35:33:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صفادشت", Longitude = "50:50:26", Latitude = "35:41:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فیروزکوه", Longitude = "52:46:25", Latitude = "35:45:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ارجمند", Longitude = "52:30:49", Latitude = "35:48:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نصیرآباد", Longitude = "51:08:27", Latitude = "35:29:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اندیشه", Longitude = "51:01:20", Latitude = "35:41:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شهریار", Longitude = "51:03:34", Latitude = "35:37:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ملارد", Longitude = "50:58:45", Latitude = "35:39:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "باغستان", Longitude = "51:07:59", Latitude = "35:38:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرگز", Longitude = "56:39:25", Latitude = "27:56:35", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پارسیان", Longitude = "53:02:33", Latitude = "27:12:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوشکنار", Longitude = "52:52:02", Latitude = "27:15:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میناب", Longitude = "57:04:26", Latitude = "27:05:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زیارتعلی", Longitude = "57:13:34", Latitude = "27:44:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حاجی آباد", Longitude = "55:54:05", Latitude = "28:18:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فارغان", Longitude = "56:15:07", Latitude = "28:00:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هرمز", Longitude = "56:27:26", Latitude = "27:05:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فین", Longitude = "55:52:55", Latitude = "27:38:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بستک", Longitude = "54:22:00", Latitude = "27:11:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جناح", Longitude = "54:17:09", Latitude = "27:01:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دهبارز", Longitude = "57:11:51", Latitude = "27:26:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رویدر", Longitude = "55:25:03", Latitude = "27:27:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هشتبندی", Longitude = "57:27:04", Latitude = "27:09:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بیکاه", Longitude = "57:10:38", Latitude = "27:20:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کنگ", Longitude = "54:56:11", Latitude = "26:35:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بندرلنگه", Longitude = "54:53:20", Latitude = "26:32:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بندرچارک", Longitude = "54:16:24", Latitude = "26:44:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "درگهان", Longitude = "56:03:58", Latitude = "26:57:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سوزا", Longitude = "56:03:54", Latitude = "26:46:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قشم", Longitude = "56:16:22", Latitude = "26:56:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خمیر", Longitude = "55:35:16", Latitude = "26:57:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیریک", Longitude = "57:06:33", Latitude = "26:31:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بندرجاسک", Longitude = "57:48:02", Latitude = "25:39:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ابوموسی", Longitude = "55:01:47", Latitude = "25:53:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بندرعباس", Longitude = "56:17:29", Latitude = "27:11:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کیش", Longitude = "53:58:25", Latitude = "26:32:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گوهران", Longitude = "57:53:51", Latitude = "26:35:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سردشت", Longitude = "57:54:04", Latitude = "26:27:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سندرک", Longitude = "57:25:35", Latitude = "26:50:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قلعه قاضی", Longitude = "56:32:40", Latitude = "27:26:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تخت", Longitude = "56:38:04", Latitude = "27:30:01", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صالح مشطط", Longitude = "48:08:53", Latitude = "32:18:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گوریه", Longitude = "48:45:20", Latitude = "31:51:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مشراگه", Longitude = "49:26:16", Latitude = "31:00:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سماله", Longitude = "48:51:27", Latitude = "32:11:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میداود", Longitude = "49:48:49", Latitude = "31:22:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چغامیش", Longitude = "48:32:44", Latitude = "32:12:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بندرامام خمینی", Longitude = "49:04:32", Latitude = "30:26:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شادگان", Longitude = "48:39:48", Latitude = "30:38:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لالی", Longitude = "49:05:34", Latitude = "32:19:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شوشتر", Longitude = "48:50:04", Latitude = "32:03:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گتوند", Longitude = "48:49:04", Latitude = "32:14:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سالند", Longitude = "48:50:03", Latitude = "32:29:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حر", Longitude = "48:23:25", Latitude = "32:08:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شوش", Longitude = "48:15:26", Latitude = "32:12:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میانرود", Longitude = "48:25:28", Latitude = "32:13:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دزآب", Longitude = "48:25:41", Latitude = "32:17:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دزفول", Longitude = "48:25:09", Latitude = "32:22:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بستان", Longitude = "47:58:52", Latitude = "31:43:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رفیع", Longitude = "47:53:39", Latitude = "31:35:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هویزه", Longitude = "48:04:38", Latitude = "31:27:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سوسنگرد", Longitude = "48:11:22", Latitude = "31:33:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حمیدیه", Longitude = "48:25:56", Latitude = "31:28:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شیبان", Longitude = "48:47:13", Latitude = "31:23:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ویس", Longitude = "48:52:28", Latitude = "31:29:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ملاثانی", Longitude = "48:53:13", Latitude = "31:35:07", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مقاومت", Longitude = "48:11:43", Latitude = "30:24:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خرمشهر", Longitude = "48:10:43", Latitude = "30:26:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آبادان", Longitude = "48:16:48", Latitude = "30:21:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بندرماهشهر", Longitude = "49:10:48", Latitude = "30:28:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چمران", Longitude = "49:10:36", Latitude = "30:42:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اروندکنار", Longitude = "48:31:00", Latitude = "29:58:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حمزه", Longitude = "48:34:42", Latitude = "32:23:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صالح شهر", Longitude = "48:40:19", Latitude = "32:12:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ترکالکی", Longitude = "48:50:46", Latitude = "32:14:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شرافت", Longitude = "48:46:03", Latitude = "32:05:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "الوان", Longitude = "48:20:28", Latitude = "31:52:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دارخوین", Longitude = "48:25:45", Latitude = "30:44:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چوئبده", Longitude = "48:33:11", Latitude = "30:12:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اهواز", Longitude = "48:40:16", Latitude = "31:14:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مینوشهر", Longitude = "48:12:37", Latitude = "30:21:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ایذه", Longitude = "49:51:53", Latitude = "31:49:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صیدون", Longitude = "50:04:53", Latitude = "31:21:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بهبهان", Longitude = "50:14:29", Latitude = "30:35:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زهره", Longitude = "49:40:58", Latitude = "30:28:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سردشت", Longitude = "50:13:04", Latitude = "30:19:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صفی آباد", Longitude = "48:25:17", Latitude = "32:14:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قلعه خواجه", Longitude = "49:26:55", Latitude = "32:12:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دهدز", Longitude = "50:17:18", Latitude = "31:42:35", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قلعه تل", Longitude = "49:53:24", Latitude = "31:37:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "باغ ملک", Longitude = "49:53:25", Latitude = "31:30:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رامهرمز", Longitude = "49:36:19", Latitude = "31:15:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هفتگل", Longitude = "49:31:59", Latitude = "31:26:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رامشیر", Longitude = "49:24:31", Latitude = "30:53:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "امیدیه", Longitude = "49:42:44", Latitude = "30:45:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هندیجان", Longitude = "49:42:41", Latitude = "30:13:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جایزان", Longitude = "49:51:15", Latitude = "30:52:29", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آغاجاری", Longitude = "49:49:47", Latitude = "30:41:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حسینیه", Longitude = "48:14:44", Latitude = "32:39:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اندیمشک", Longitude = "48:22:23", Latitude = "32:25:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مسجدسلیمان", Longitude = "49:17:10", Latitude = "31:57:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شاوور", Longitude = "48:18:03", Latitude = "32:03:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جنت مکان", Longitude = "48:48:58", Latitude = "32:11:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گزیک", Longitude = "60:13:21", Latitude = "33:00:01", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ارسک", Longitude = "57:22:18", Latitude = "33:42:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نهبندان", Longitude = "60:02:16", Latitude = "31:32:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شوسف", Longitude = "60:00:31", Latitude = "31:48:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بشرویه", Longitude = "57:25:20", Latitude = "33:51:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سه قلعه", Longitude = "58:23:57", Latitude = "33:39:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرایان", Longitude = "58:31:20", Latitude = "33:51:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اسفدن", Longitude = "59:46:43", Latitude = "33:38:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سربیشه", Longitude = "59:47:54", Latitude = "32:34:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آیسک", Longitude = "58:22:53", Latitude = "33:53:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اسدیه", Longitude = "60:01:28", Latitude = "32:54:57", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مود", Longitude = "59:31:26", Latitude = "32:42:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خوسف", Longitude = "58:53:09", Latitude = "32:46:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نیمبلوک", Longitude = "58:55:45", Latitude = "33:54:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قائن", Longitude = "59:10:51", Latitude = "33:43:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آرین شهر", Longitude = "59:13:58", Latitude = "33:19:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زهان", Longitude = "59:48:39", Latitude = "33:25:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حاجی آباد", Longitude = "59:59:51", Latitude = "33:36:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فردوس", Longitude = "58:10:20", Latitude = "34:01:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اسلامیه", Longitude = "58:13:12", Latitude = "34:02:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خضری دشت بیاض", Longitude = "58:48:23", Latitude = "34:01:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محمدشهر", Longitude = "59:01:03", Latitude = "32:52:29", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بیرجند", Longitude = "59:13:10", Latitude = "32:51:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قهستان", Longitude = "59:42:56", Latitude = "33:08:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "طبس مسینا", Longitude = "60:13:22", Latitude = "32:48:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مرادلو", Longitude = "47:44:46", Latitude = "38:44:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رضی", Longitude = "48:05:40", Latitude = "38:37:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آبی بیگلو", Longitude = "48:33:09", Latitude = "38:17:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "عنبران", Longitude = "48:26:06", Latitude = "38:29:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نمین", Longitude = "48:28:50", Latitude = "38:25:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هیر", Longitude = "48:29:52", Latitude = "38:04:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرعین", Longitude = "48:04:34", Latitude = "38:08:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هشتجین", Longitude = "48:19:28", Latitude = "37:21:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تازه کند", Longitude = "48:00:58", Latitude = "39:34:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گیوی", Longitude = "48:20:22", Latitude = "37:41:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خلخال", Longitude = "48:31:38", Latitude = "37:37:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کلور", Longitude = "48:43:22", Latitude = "37:23:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لاهرود", Longitude = "47:49:49", Latitude = "38:30:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مشگین شهر", Longitude = "47:40:21", Latitude = "38:23:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نیر", Longitude = "48:00:26", Latitude = "38:02:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جعفرآباد", Longitude = "48:05:52", Latitude = "39:25:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بیله سوار", Longitude = "48:20:33", Latitude = "39:22:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گرمی", Longitude = "48:05:00", Latitude = "39:00:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اصلاندوز", Longitude = "47:24:36", Latitude = "39:26:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پارس آباد", Longitude = "47:54:35", Latitude = "39:38:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تازه کندانگوت", Longitude = "47:44:37", Latitude = "39:02:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فخرآباد", Longitude = "47:51:42", Latitude = "38:31:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اردبیل", Longitude = "48:17:39", Latitude = "38:14:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوراییم", Longitude = "48:14:07", Latitude = "37:57:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هفت چشمه", Longitude = "47:45:34", Latitude = "34:12:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شول آباد", Longitude = "49:11:27", Latitude = "33:11:08", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "معمولان", Longitude = "47:57:42", Latitude = "33:22:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چقابل", Longitude = "47:30:28", Latitude = "33:16:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کونانی", Longitude = "47:19:31", Latitude = "33:24:08", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مؤمن آباد", Longitude = "49:31:06", Latitude = "33:35:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نورآباد", Longitude = "47:58:23", Latitude = "34:04:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "درب گنبد", Longitude = "47:08:53", Latitude = "33:41:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوهدشت", Longitude = "47:36:30", Latitude = "33:32:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گراب", Longitude = "47:14:13", Latitude = "33:28:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پلدختر", Longitude = "47:42:51", Latitude = "33:09:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سپیددشت", Longitude = "48:53:02", Latitude = "33:13:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ویسیان", Longitude = "48:01:49", Latitude = "33:29:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سراب دوره", Longitude = "48:01:12", Latitude = "33:33:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "الشتر", Longitude = "48:15:35", Latitude = "33:51:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فیروزآباد", Longitude = "48:06:11", Latitude = "33:53:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زاغه", Longitude = "48:42:27", Latitude = "33:29:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چغلوندی", Longitude = "48:33:34", Latitude = "33:38:57", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چالانچولان", Longitude = "48:54:23", Latitude = "33:39:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دورود", Longitude = "49:03:24", Latitude = "33:29:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ازنا", Longitude = "49:27:15", Latitude = "33:27:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "الیگودرز", Longitude = "49:41:35", Latitude = "33:24:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بروجرد", Longitude = "48:45:16", Latitude = "33:53:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اشترینان", Longitude = "48:38:31", Latitude = "34:01:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خرم آباد", Longitude = "48:20:56", Latitude = "33:27:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دوساری", Longitude = "57:56:40", Latitude = "28:25:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هماشهر", Longitude = "55:48:34", Latitude = "29:39:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قلعه گنج", Longitude = "57:52:58", Latitude = "27:30:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کهنوج", Longitude = "57:41:49", Latitude = "27:55:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جیرفت", Longitude = "57:44:31", Latitude = "28:40:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مردهک", Longitude = "58:09:35", Latitude = "28:20:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رودبار", Longitude = "57:59:40", Latitude = "28:01:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "منوجان", Longitude = "57:29:48", Latitude = "27:24:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نودژ", Longitude = "57:26:57", Latitude = "27:31:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ارزوئیه", Longitude = "56:21:48", Latitude = "28:27:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "عنبرآباد", Longitude = "57:50:33", Latitude = "28:28:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فاریاب", Longitude = "57:13:41", Latitude = "28:05:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محمدآباد", Longitude = "59:01:02", Latitude = "28:38:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پاریز", Longitude = "55:45:01", Latitude = "29:52:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیرجان", Longitude = "55:40:29", Latitude = "29:26:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بافت", Longitude = "56:35:49", Latitude = "29:13:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رابر", Longitude = "56:54:46", Latitude = "29:17:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بردسیر", Longitude = "56:34:43", Latitude = "29:55:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نجف شهر", Longitude = "55:43:14", Latitude = "29:23:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زیدآباد", Longitude = "55:32:05", Latitude = "29:35:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بزنجان", Longitude = "56:41:39", Latitude = "29:15:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نگار", Longitude = "56:48:10", Latitude = "29:51:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرچشمه", Longitude = "55:47:40", Latitude = "29:59:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رفسنجان", Longitude = "55:59:15", Latitude = "30:22:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کشکوئیه", Longitude = "55:38:40", Latitude = "30:31:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زرند", Longitude = "56:33:06", Latitude = "30:48:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چترود", Longitude = "56:54:33", Latitude = "30:36:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زنگی آباد", Longitude = "56:54:53", Latitude = "30:24:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "باغین", Longitude = "56:48:47", Latitude = "30:11:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بهرمان", Longitude = "55:43:38", Latitude = "30:54:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اختیارآباد", Longitude = "56:55:05", Latitude = "30:19:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ماهان", Longitude = "57:17:23", Latitude = "30:03:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جوپار", Longitude = "57:06:44", Latitude = "30:03:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شهداد", Longitude = "57:42:06", Latitude = "30:25:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اندوهجرد", Longitude = "57:45:14", Latitude = "30:13:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نظام شهر", Longitude = "58:33:05", Latitude = "28:54:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بم", Longitude = "58:20:55", Latitude = "29:04:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بروات", Longitude = "58:24:30", Latitude = "29:02:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گلزار", Longitude = "57:02:24", Latitude = "29:42:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "راین", Longitude = "57:26:06", Latitude = "29:35:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "درب بهشت", Longitude = "57:20:12", Latitude = "29:14:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گلباف", Longitude = "57:43:51", Latitude = "29:53:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دهج", Longitude = "54:52:41", Latitude = "30:41:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شهربابک", Longitude = "55:07:14", Latitude = "30:07:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محی آباد", Longitude = "57:13:48", Latitude = "30:04:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوهبنان", Longitude = "56:16:45", Latitude = "31:24:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کیانشهر", Longitude = "56:22:51", Latitude = "31:09:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "راور", Longitude = "56:47:54", Latitude = "31:14:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "انار", Longitude = "55:16:17", Latitude = "30:52:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "امین شهر", Longitude = "55:20:21", Latitude = "30:50:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "یزدان شهر", Longitude = "56:22:33", Latitude = "30:51:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خانوک", Longitude = "56:46:31", Latitude = "30:43:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کاظم آباد", Longitude = "56:50:37", Latitude = "30:33:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هجدک", Longitude = "56:59:46", Latitude = "30:45:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جبالبارز", Longitude = "57:52:57", Latitude = "28:54:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جوزم", Longitude = "55:01:54", Latitude = "30:30:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خورسند", Longitude = "55:05:15", Latitude = "30:09:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کرمان", Longitude = "57:03:51", Latitude = "30:16:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فهرج", Longitude = "58:53:06", Latitude = "28:56:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نرماشیر", Longitude = "58:41:57", Latitude = "28:57:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صفائیه", Longitude = "55:48:40", Latitude = "30:49:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خاتون آباد", Longitude = "55:25:11", Latitude = "29:59:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لاله زار", Longitude = "56:48:58", Latitude = "29:31:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ریحان شهر", Longitude = "56:44:09", Latitude = "30:45:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نخل تقی", Longitude = "52:35:02", Latitude = "27:29:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خورموج", Longitude = "51:22:57", Latitude = "28:37:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کاکی", Longitude = "51:31:21", Latitude = "28:20:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ریز", Longitude = "52:04:37", Latitude = "28:03:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دلوار", Longitude = "51:04:12", Latitude = "28:44:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بردخون", Longitude = "51:28:40", Latitude = "28:03:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آبدان", Longitude = "51:46:14", Latitude = "28:04:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "عسلویه", Longitude = "52:36:29", Latitude = "27:28:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بندرکنگان", Longitude = "52:03:42", Latitude = "27:50:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بنک", Longitude = "52:01:38", Latitude = "27:52:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جم", Longitude = "52:20:27", Latitude = "27:49:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بندردیر", Longitude = "51:56:06", Latitude = "27:50:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیراف", Longitude = "52:20:19", Latitude = "27:40:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کلمه", Longitude = "51:27:33", Latitude = "28:56:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اهرم", Longitude = "51:16:43", Latitude = "28:53:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چغادک", Longitude = "51:01:30", Latitude = "28:59:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "برازجان", Longitude = "51:13:00", Latitude = "29:16:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دالکی", Longitude = "51:17:28", Latitude = "29:25:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تنگ ارم", Longitude = "51:31:38", Latitude = "29:09:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سعدآباد", Longitude = "51:06:59", Latitude = "29:23:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "وحدتیه", Longitude = "51:14:10", Latitude = "29:28:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شبانکاره", Longitude = "50:59:38", Latitude = "29:28:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بندرگناوه", Longitude = "50:31:06", Latitude = "29:34:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بندرریگ", Longitude = "50:37:35", Latitude = "29:29:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خارک", Longitude = "50:19:09", Latitude = "29:14:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "امام حسن", Longitude = "50:15:49", Latitude = "29:50:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آبپخش", Longitude = "51:03:52", Latitude = "29:21:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بندردیلم", Longitude = "50:09:47", Latitude = "30:03:08", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بوشهر", Longitude = "50:50:15", Latitude = "28:55:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "انارستان", Longitude = "52:03:59", Latitude = "28:01:57", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شنبه", Longitude = "51:45:50", Latitude = "28:23:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بردستان", Longitude = "51:57:34", Latitude = "27:52:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سی سخت", Longitude = "51:27:31", Latitude = "30:51:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "باشت", Longitude = "51:09:22", Latitude = "30:21:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دیشموک", Longitude = "50:24:06", Latitude = "31:17:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سوق", Longitude = "50:27:35", Latitude = "30:51:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لنده", Longitude = "50:25:10", Latitude = "30:58:57", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قلعه رئیسی", Longitude = "50:26:38", Latitude = "31:11:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لیکک", Longitude = "50:05:34", Latitude = "30:53:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دهدشت", Longitude = "50:33:48", Latitude = "30:47:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چرام", Longitude = "50:44:48", Latitude = "30:45:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دوگنبدان", Longitude = "50:46:58", Latitude = "30:21:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گراب سفلی", Longitude = "50:53:57", Latitude = "30:56:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مارگون", Longitude = "51:04:41", Latitude = "30:59:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مادوان", Longitude = "51:33:16", Latitude = "30:42:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "یاسوج", Longitude = "51:34:56", Latitude = "30:39:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چیتاب", Longitude = "51:19:29", Latitude = "30:47:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پاتاوه", Longitude = "51:16:14", Latitude = "30:57:09", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سورشجان", Longitude = "50:40:43", Latitude = "32:18:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جونقان", Longitude = "50:41:11", Latitude = "32:08:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "طاقانک", Longitude = "50:50:12", Latitude = "32:13:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فرادنبه", Longitude = "51:12:53", Latitude = "32:00:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گندمان", Longitude = "51:09:12", Latitude = "31:51:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آلونی", Longitude = "51:03:25", Latitude = "31:33:07", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مال خلیفه", Longitude = "51:15:55", Latitude = "31:17:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بروجن", Longitude = "51:17:21", Latitude = "31:58:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بلداجی", Longitude = "51:03:09", Latitude = "31:56:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سفیددشت", Longitude = "51:10:58", Latitude = "32:07:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فرخ شهر", Longitude = "50:58:22", Latitude = "32:16:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گهرو", Longitude = "50:53:10", Latitude = "32:00:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شلمزار", Longitude = "50:49:01", Latitude = "32:02:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هفشجان", Longitude = "50:47:40", Latitude = "32:13:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "باباحیدر", Longitude = "50:28:07", Latitude = "32:19:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شهرکرد", Longitude = "50:51:29", Latitude = "32:18:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فارسان", Longitude = "50:33:56", Latitude = "32:15:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کیان", Longitude = "50:53:21", Latitude = "32:16:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نافچ", Longitude = "50:47:18", Latitude = "32:25:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سامان", Longitude = "50:54:37", Latitude = "32:27:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بن", Longitude = "50:44:46", Latitude = "32:32:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سودجان", Longitude = "50:24:01", Latitude = "32:31:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چلگرد", Longitude = "50:07:48", Latitude = "32:28:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ناغان", Longitude = "50:43:50", Latitude = "31:56:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لردگان", Longitude = "50:49:48", Latitude = "31:30:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اردل", Longitude = "50:39:41", Latitude = "31:59:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نقنه", Longitude = "51:19:43", Latitude = "31:55:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "عقدا", Longitude = "53:37:53", Latitude = "32:26:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ندوشن", Longitude = "53:32:54", Latitude = "32:01:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خضرآباد", Longitude = "53:57:08", Latitude = "31:51:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ابرکوه", Longitude = "53:16:46", Latitude = "31:07:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مهردشت", Longitude = "53:21:20", Latitude = "31:01:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میبد", Longitude = "54:01:05", Latitude = "32:14:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اردکان", Longitude = "54:01:06", Latitude = "32:18:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "احمدآباد", Longitude = "53:59:28", Latitude = "32:21:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بفروئیه", Longitude = "53:59:48", Latitude = "32:16:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بافق", Longitude = "55:23:46", Latitude = "31:36:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مروست", Longitude = "54:12:38", Latitude = "30:27:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هرات", Longitude = "54:22:10", Latitude = "30:03:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نیر", Longitude = "54:07:43", Latitude = "31:29:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مهریز", Longitude = "54:26:16", Latitude = "31:33:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تفت", Longitude = "54:12:27", Latitude = "31:44:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شاهدیه", Longitude = "54:16:48", Latitude = "31:56:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زارچ", Longitude = "54:14:33", Latitude = "31:59:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بهاباد", Longitude = "56:01:35", Latitude = "31:52:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اشکذر", Longitude = "54:12:24", Latitude = "32:00:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حمیدیا", Longitude = "54:23:53", Latitude = "31:49:09", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "یزد", Longitude = "54:20:51", Latitude = "31:53:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "طبس", Longitude = "56:56:07", Latitude = "33:34:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دیهوک", Longitude = "57:30:57", Latitude = "33:16:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "عشق آباد", Longitude = "56:55:41", Latitude = "34:21:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فامنین", Longitude = "48:58:20", Latitude = "35:06:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رزن", Longitude = "49:02:06", Latitude = "35:23:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کبودرآهنگ", Longitude = "48:43:29", Latitude = "35:12:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مریانج", Longitude = "48:27:47", Latitude = "34:49:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جورقان", Longitude = "48:33:10", Latitude = "34:52:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جوکار", Longitude = "48:41:09", Latitude = "34:25:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دمق", Longitude = "48:49:19", Latitude = "35:26:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شیرین سو", Longitude = "48:27:02", Latitude = "35:29:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صالح آباد", Longitude = "48:20:35", Latitude = "34:55:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "برزول", Longitude = "48:15:39", Latitude = "34:12:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فیروزان", Longitude = "48:06:58", Latitude = "34:21:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گل تپه", Longitude = "48:12:19", Latitude = "35:13:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گیان", Longitude = "48:14:38", Latitude = "34:10:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اسدآباد", Longitude = "48:07:17", Latitude = "34:47:07", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نهاوند", Longitude = "48:22:31", Latitude = "34:10:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فرسفج", Longitude = "48:17:17", Latitude = "34:29:09", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تویسرکان", Longitude = "48:26:53", Latitude = "34:32:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرکان", Longitude = "48:26:52", Latitude = "34:35:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لالجین", Longitude = "48:28:42", Latitude = "34:58:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بهار", Longitude = "48:26:27", Latitude = "34:53:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ازندریان", Longitude = "48:41:34", Latitude = "34:30:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سامن", Longitude = "48:42:12", Latitude = "34:12:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ملایر", Longitude = "48:48:59", Latitude = "34:17:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زنگنه", Longitude = "49:00:30", Latitude = "34:09:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قهاوند", Longitude = "49:00:12", Latitude = "34:51:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "همدان", Longitude = "48:31:34", Latitude = "34:47:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قروه در جزین", Longitude = "49:06:00", Latitude = "35:18:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آوج", Longitude = "49:13:18", Latitude = "35:34:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آبگرم", Longitude = "49:17:14", Latitude = "35:45:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سگزآباد", Longitude = "49:56:22", Latitude = "35:45:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوهین", Longitude = "49:39:29", Latitude = "36:22:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تاکستان", Longitude = "49:41:46", Latitude = "36:04:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ارداق", Longitude = "49:49:27", Latitude = "36:03:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محمودآبادنمونه", Longitude = "49:54:09", Latitude = "36:17:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اقبالیه", Longitude = "49:55:35", Latitude = "36:13:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قزوین", Longitude = "50:00:07", Latitude = "36:16:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "الوند", Longitude = "50:04:26", Latitude = "36:10:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بیدستان", Longitude = "50:07:17", Latitude = "36:13:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محمدیه", Longitude = "50:10:57", Latitude = "36:13:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خاکعلی", Longitude = "50:10:32", Latitude = "36:07:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آبیک", Longitude = "50:31:46", Latitude = "36:02:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "معلم کلایه", Longitude = "50:28:41", Latitude = "36:26:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رازمیان", Longitude = "50:12:44", Latitude = "36:32:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیردان", Longitude = "49:11:17", Latitude = "36:39:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دانسفهان", Longitude = "49:44:36", Latitude = "35:48:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شال", Longitude = "49:46:06", Latitude = "35:53:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اسفرورین", Longitude = "49:45:01", Latitude = "35:56:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بوئین زهرا", Longitude = "50:03:43", Latitude = "35:45:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نرجه", Longitude = "49:37:13", Latitude = "35:59:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ضیاءآباد", Longitude = "49:26:57", Latitude = "35:59:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خرمدشت", Longitude = "49:30:42", Latitude = "35:55:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شریفیه", Longitude = "50:09:04", Latitude = "36:12:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زابل", Longitude = "61:29:37", Latitude = "31:00:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بنجار", Longitude = "61:34:02", Latitude = "31:02:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دوست محمد", Longitude = "61:47:34", Latitude = "31:08:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نگور", Longitude = "61:08:23", Latitude = "25:23:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گلمورتی", Longitude = "59:26:50", Latitude = "27:28:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بنت", Longitude = "59:31:21", Latitude = "26:17:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فنوج", Longitude = "59:38:41", Latitude = "26:34:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کنارک", Longitude = "60:23:49", Latitude = "25:21:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خاش", Longitude = "61:12:10", Latitude = "28:12:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نوک آباد", Longitude = "60:45:31", Latitude = "28:32:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ایرانشهر", Longitude = "60:40:30", Latitude = "27:12:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بمپور", Longitude = "60:27:22", Latitude = "27:11:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بزمان", Longitude = "60:10:31", Latitude = "27:51:01", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محمدان", Longitude = "60:33:38", Latitude = "27:11:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زابلی", Longitude = "61:40:21", Latitude = "27:06:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سوران", Longitude = "61:59:41", Latitude = "27:17:09", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سراوان", Longitude = "62:19:59", Latitude = "27:21:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جالق", Longitude = "62:42:02", Latitude = "27:34:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هیدوج", Longitude = "62:07:09", Latitude = "27:00:07", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گشت", Longitude = "61:57:03", Latitude = "27:47:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محمدی", Longitude = "62:23:24", Latitude = "27:19:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نیک شهر", Longitude = "60:13:36", Latitude = "26:14:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قصرقند", Longitude = "60:44:30", Latitude = "26:12:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرباز", Longitude = "61:15:29", Latitude = "26:37:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "راسک", Longitude = "61:24:19", Latitude = "26:13:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اسپکه", Longitude = "60:10:20", Latitude = "26:50:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیرکان", Longitude = "62:38:18", Latitude = "26:49:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پیشین", Longitude = "61:44:54", Latitude = "26:04:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ادیمی", Longitude = "61:24:27", Latitude = "31:07:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محمدآباد", Longitude = "61:27:42", Latitude = "30:52:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زهک", Longitude = "61:40:58", Latitude = "30:53:35", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نصرت آباد", Longitude = "59:58:25", Latitude = "29:51:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "علی اکبر", Longitude = "61:19:40", Latitude = "30:56:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میرجاوه", Longitude = "61:26:58", Latitude = "29:00:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زاهدان", Longitude = "60:51:07", Latitude = "29:28:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زرآباد", Longitude = "59:23:46", Latitude = "25:35:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چاه بهار", Longitude = "60:38:51", Latitude = "25:17:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بانه", Longitude = "45:53:19", Latitude = "35:59:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سروآباد", Longitude = "46:22:01", Latitude = "35:18:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چناره", Longitude = "46:18:32", Latitude = "35:37:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شویشه", Longitude = "46:40:42", Latitude = "35:21:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سنندج", Longitude = "47:00:23", Latitude = "35:14:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دیواندره", Longitude = "47:01:29", Latitude = "35:54:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دهگلان", Longitude = "47:25:08", Latitude = "35:16:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بابارشانی", Longitude = "47:47:50", Latitude = "35:40:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سریش آباد", Longitude = "47:46:42", Latitude = "35:14:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "یاسوکند", Longitude = "47:44:48", Latitude = "36:17:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زرینه", Longitude = "46:55:07", Latitude = "36:03:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کامیاران", Longitude = "46:56:22", Latitude = "34:47:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "موچش", Longitude = "47:09:16", Latitude = "35:03:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قروه", Longitude = "47:48:36", Latitude = "35:09:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بیجار", Longitude = "47:37:05", Latitude = "35:52:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دزج", Longitude = "47:57:50", Latitude = "35:03:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دلبران", Longitude = "47:59:18", Latitude = "35:14:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مریوان", Longitude = "46:10:51", Latitude = "35:31:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آرمرده", Longitude = "45:47:48", Latitude = "35:55:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بوئین سفلی", Longitude = "45:56:10", Latitude = "35:56:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کانی سور", Longitude = "45:44:55", Latitude = "36:03:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سقز", Longitude = "46:16:22", Latitude = "36:14:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صاحب", Longitude = "46:27:42", Latitude = "36:12:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کانی دینار", Longitude = "46:12:12", Latitude = "35:28:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بلبان آباد", Longitude = "47:19:15", Latitude = "35:08:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ماه نشان", Longitude = "47:40:14", Latitude = "36:44:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دندی", Longitude = "47:37:12", Latitude = "36:33:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چورزق", Longitude = "48:46:42", Latitude = "36:59:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زرین آباد", Longitude = "48:16:37", Latitude = "36:25:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حلب", Longitude = "48:03:49", Latitude = "36:17:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سجاس", Longitude = "48:33:11", Latitude = "36:14:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قیدار", Longitude = "48:35:23", Latitude = "36:06:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آب بر", Longitude = "48:57:22", Latitude = "36:55:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سلطانیه", Longitude = "48:47:43", Latitude = "36:26:01", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صائین قلعه", Longitude = "49:04:20", Latitude = "36:18:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هیدج", Longitude = "49:07:46", Latitude = "36:15:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خرمدره", Longitude = "49:11:43", Latitude = "36:12:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ابهر", Longitude = "49:13:28", Latitude = "36:08:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زرین رود", Longitude = "48:28:51", Latitude = "35:45:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گرماب", Longitude = "48:12:04", Latitude = "35:50:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سهرورد", Longitude = "48:26:20", Latitude = "36:04:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زنجان", Longitude = "48:29:34", Latitude = "36:40:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ارمغانخانه", Longitude = "48:22:18", Latitude = "36:58:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قوچان", Longitude = "58:30:39", Latitude = "37:06:08", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نوخندان", Longitude = "58:59:20", Latitude = "37:31:09", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "درگز", Longitude = "59:06:20", Latitude = "37:26:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چاپشلو", Longitude = "59:04:37", Latitude = "37:20:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لطف آباد", Longitude = "59:20:05", Latitude = "37:30:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نقاب", Longitude = "57:24:29", Latitude = "36:42:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جغتای", Longitude = "57:04:37", Latitude = "36:38:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سلطان آباد", Longitude = "58:02:23", Latitude = "36:24:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سبزوار", Longitude = "57:40:56", Latitude = "36:12:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "روداب", Longitude = "57:18:44", Latitude = "36:01:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "داورزن", Longitude = "56:52:40", Latitude = "36:21:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چکنه", Longitude = "58:30:15", Latitude = "36:49:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فیروزه", Longitude = "58:35:20", Latitude = "36:17:08", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "باجگیران", Longitude = "58:24:55", Latitude = "37:37:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کاریز", Longitude = "60:49:31", Latitude = "34:48:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "یونسی", Longitude = "58:26:13", Latitude = "34:48:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بجستان", Longitude = "58:10:54", Latitude = "34:30:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بیدخت", Longitude = "58:45:26", Latitude = "34:20:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قاسم آباد", Longitude = "59:51:43", Latitude = "34:21:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جنگل", Longitude = "59:13:24", Latitude = "34:42:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سلامی", Longitude = "59:58:35", Latitude = "34:44:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بایگ", Longitude = "59:02:22", Latitude = "35:22:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کدکن", Longitude = "58:52:44", Latitude = "35:35:08", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رباط سنگ", Longitude = "59:11:39", Latitude = "35:32:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ملک آباد", Longitude = "59:35:36", Latitude = "35:59:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فریمان", Longitude = "59:50:53", Latitude = "35:42:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قلندرآباد", Longitude = "59:57:03", Latitude = "35:35:56", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دولت آباد", Longitude = "59:31:19", Latitude = "35:16:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کاخک", Longitude = "58:38:40", Latitude = "34:08:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گناباد", Longitude = "58:41:08", Latitude = "34:21:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فیض آباد", Longitude = "58:46:30", Latitude = "35:00:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رشتخوار", Longitude = "59:37:22", Latitude = "34:58:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "باخرز", Longitude = "60:19:07", Latitude = "34:58:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تایباد", Longitude = "60:46:37", Latitude = "34:44:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خواف", Longitude = "60:08:39", Latitude = "34:33:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سنگان", Longitude = "60:15:19", Latitude = "34:23:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مشهدریزه", Longitude = "60:30:27", Latitude = "34:47:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نشتیفان", Longitude = "60:10:35", Latitude = "34:26:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ریوش", Longitude = "58:27:42", Latitude = "35:28:35", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خلیل آباد", Longitude = "58:17:06", Latitude = "35:14:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کاشمر", Longitude = "58:27:35", Latitude = "35:14:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کندر", Longitude = "58:09:02", Latitude = "35:12:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بردسکن", Longitude = "57:58:19", Latitude = "35:15:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "انابد", Longitude = "57:48:35", Latitude = "35:15:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ششتمد", Longitude = "57:46:12", Latitude = "35:57:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تربت حیدریه", Longitude = "59:12:46", Latitude = "35:16:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فرهادگرد", Longitude = "59:43:47", Latitude = "35:45:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تربت جام", Longitude = "60:37:17", Latitude = "35:14:29", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نیل شهر", Longitude = "60:46:23", Latitude = "35:07:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نصرآباد", Longitude = "60:18:55", Latitude = "35:25:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صالح آباد", Longitude = "61:05:25", Latitude = "35:41:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سفیدسنگ", Longitude = "60:05:35", Latitude = "35:39:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "احمدآبادصولت", Longitude = "60:41:21", Latitude = "35:06:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "عشق آباد", Longitude = "58:40:59", Latitude = "36:02:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نیشابور", Longitude = "58:47:38", Latitude = "36:12:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خرو", Longitude = "59:01:37", Latitude = "36:08:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قدمگاه", Longitude = "59:03:37", Latitude = "36:06:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "درود", Longitude = "59:06:44", Latitude = "36:08:08", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چناران", Longitude = "59:07:15", Latitude = "36:38:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شاندیز", Longitude = "59:17:53", Latitude = "36:23:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "طرقبه", Longitude = "59:22:34", Latitude = "36:18:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رضویه", Longitude = "59:46:11", Latitude = "36:12:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شهرزو", Longitude = "59:55:27", Latitude = "36:44:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرخس", Longitude = "61:08:54", Latitude = "36:32:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مزدآوند", Longitude = "60:31:39", Latitude = "36:09:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بار", Longitude = "58:43:08", Latitude = "36:29:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گلمکان", Longitude = "59:09:34", Latitude = "36:28:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کلات", Longitude = "59:44:59", Latitude = "36:59:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مشهد", Longitude = "59:34:32", Latitude = "36:18:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شهرآباد", Longitude = "57:56:05", Latitude = "35:08:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شادمهر", Longitude = "59:02:16", Latitude = "35:10:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "همت آباد", Longitude = "58:27:50", Latitude = "36:17:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صفی آباد", Longitude = "57:55:37", Latitude = "36:41:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حصارگرمخان", Longitude = "57:29:05", Latitude = "37:30:57", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لوجلی", Longitude = "57:51:27", Latitude = "37:36:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شیروان", Longitude = "57:55:27", Latitude = "37:24:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اسفراین", Longitude = "57:30:34", Latitude = "37:04:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فاروج", Longitude = "58:13:08", Latitude = "37:13:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پیش قلعه", Longitude = "57:00:05", Latitude = "37:38:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "راز", Longitude = "57:06:27", Latitude = "37:56:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بجنورد", Longitude = "57:19:38", Latitude = "37:26:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آشخانه", Longitude = "56:55:26", Latitude = "37:33:29", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قاضی", Longitude = "56:44:55", Latitude = "37:29:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شوقان", Longitude = "56:53:13", Latitude = "37:20:29", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سنخواست", Longitude = "56:51:05", Latitude = "37:05:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "درق", Longitude = "56:12:52", Latitude = "36:58:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ایور", Longitude = "56:15:38", Latitude = "36:58:07", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گرمه", Longitude = "56:18:05", Latitude = "36:58:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جاجرم", Longitude = "56:21:27", Latitude = "36:57:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تیتکانلو", Longitude = "58:17:22", Latitude = "37:16:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خواجه", Longitude = "46:35:12", Latitude = "38:09:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لیلان", Longitude = "46:12:20", Latitude = "37:00:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خسروشهر", Longitude = "46:03:02", Latitude = "37:57:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ایلخچی", Longitude = "45:58:55", Latitude = "37:56:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آذرشهر", Longitude = "45:58:35", Latitude = "37:45:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "عجب شیر", Longitude = "45:53:41", Latitude = "37:28:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بناب", Longitude = "46:03:58", Latitude = "37:20:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مراغه", Longitude = "46:14:00", Latitude = "37:23:09", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ملکان", Longitude = "46:06:01", Latitude = "37:08:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تبریز", Longitude = "46:17:20", Latitude = "38:04:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اسکو", Longitude = "46:07:28", Latitude = "37:55:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ممقان", Longitude = "45:58:17", Latitude = "37:50:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گوگان", Longitude = "45:54:16", Latitude = "37:46:08", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "باسمنج", Longitude = "46:28:29", Latitude = "37:59:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آقکند", Longitude = "48:03:55", Latitude = "37:15:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زرنق", Longitude = "47:04:54", Latitude = "38:05:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کلیبر", Longitude = "47:02:26", Latitude = "38:51:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هوراند", Longitude = "47:21:54", Latitude = "38:51:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اهر", Longitude = "47:04:15", Latitude = "38:28:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هریس", Longitude = "47:07:01", Latitude = "38:14:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ورزقان", Longitude = "46:39:01", Latitude = "38:30:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مهربان", Longitude = "47:07:54", Latitude = "38:04:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کلوانق", Longitude = "46:59:35", Latitude = "38:06:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بخشایش", Longitude = "46:56:50", Latitude = "38:07:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خاروانا", Longitude = "46:10:07", Latitude = "38:40:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زنوز", Longitude = "45:50:03", Latitude = "38:35:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بناب جدید", Longitude = "45:54:25", Latitude = "38:25:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صوفیان", Longitude = "45:58:53", Latitude = "38:16:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیس", Longitude = "45:48:50", Latitude = "38:11:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شبستر", Longitude = "45:42:05", Latitude = "38:10:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "یامچی", Longitude = "45:38:24", Latitude = "38:31:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کشکسرای", Longitude = "45:34:08", Latitude = "38:27:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خامنه", Longitude = "45:37:58", Latitude = "38:11:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شندآباد", Longitude = "45:37:21", Latitude = "38:08:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوزه کنان", Longitude = "45:34:38", Latitude = "38:11:01", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شرفخانه", Longitude = "45:29:24", Latitude = "38:10:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تسوج", Longitude = "45:22:13", Latitude = "38:18:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هادیشهر", Longitude = "45:39:46", Latitude = "38:50:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جلفا", Longitude = "45:38:19", Latitude = "38:56:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیه رود", Longitude = "46:00:06", Latitude = "38:52:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سردرود", Longitude = "46:08:52", Latitude = "38:01:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "وایقان", Longitude = "45:42:46", Latitude = "38:07:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آبش احمد", Longitude = "47:19:02", Latitude = "39:02:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خمارلو", Longitude = "47:02:05", Latitude = "39:08:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نظرکهریزی", Longitude = "46:45:43", Latitude = "37:20:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خراجو", Longitude = "46:31:53", Latitude = "37:18:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قره آغاج", Longitude = "46:58:20", Latitude = "37:07:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ترک", Longitude = "47:46:05", Latitude = "37:36:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میانه", Longitude = "47:42:58", Latitude = "37:25:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سراب", Longitude = "47:32:07", Latitude = "37:56:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ترکمانچای", Longitude = "47:23:29", Latitude = "37:34:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هشترود", Longitude = "47:03:34", Latitude = "37:28:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تیکمه داش", Longitude = "46:56:44", Latitude = "37:43:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بستان آباد", Longitude = "46:49:51", Latitude = "37:50:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شربیان", Longitude = "47:05:59", Latitude = "37:52:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دوزدوزان", Longitude = "47:07:11", Latitude = "37:56:57", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مرند", Longitude = "45:46:19", Latitude = "38:25:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شهرجدیدسهند", Longitude = "46:07:15", Latitude = "37:56:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اشنویه", Longitude = "45:06:00", Latitude = "37:02:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چهاربرج", Longitude = "45:58:32", Latitude = "37:07:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آواجیق", Longitude = "44:09:11", Latitude = "39:19:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ماکو", Longitude = "44:29:32", Latitude = "39:17:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شوط", Longitude = "44:46:16", Latitude = "39:13:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بازرگان", Longitude = "44:23:12", Latitude = "39:23:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیه چشمه", Longitude = "44:23:14", Latitude = "39:04:09", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تکاب", Longitude = "47:06:36", Latitude = "36:24:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قطور", Longitude = "44:24:20", Latitude = "38:28:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قوشچی", Longitude = "45:02:16", Latitude = "37:59:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نوشین", Longitude = "45:03:08", Latitude = "37:44:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شاهین دژ", Longitude = "46:33:59", Latitude = "36:40:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محمودآباد", Longitude = "46:31:08", Latitude = "36:42:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محمدیار", Longitude = "45:31:19", Latitude = "36:58:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گردکشانه", Longitude = "45:16:26", Latitude = "36:48:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مهاباد", Longitude = "45:43:42", Latitude = "36:46:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سلماس", Longitude = "44:45:57", Latitude = "38:11:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تازه شهر", Longitude = "44:41:26", Latitude = "38:10:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خوی", Longitude = "44:57:16", Latitude = "38:31:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فیرورق", Longitude = "44:50:12", Latitude = "38:34:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ایواوغلی", Longitude = "45:12:41", Latitude = "38:43:07", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قره ضیاءالدین", Longitude = "45:01:30", Latitude = "38:53:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پلدشت", Longitude = "45:04:06", Latitude = "39:20:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرو", Longitude = "44:39:01", Latitude = "37:43:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیلوانه", Longitude = "44:51:04", Latitude = "37:25:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ربط", Longitude = "45:33:06", Latitude = "36:12:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پیرانشهر", Longitude = "45:08:28", Latitude = "36:41:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میاندوآب", Longitude = "46:06:21", Latitude = "36:57:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بوکان", Longitude = "46:12:23", Latitude = "36:30:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نالوس", Longitude = "45:08:35", Latitude = "36:59:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نقده", Longitude = "45:23:20", Latitude = "36:57:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میرآباد", Longitude = "45:22:29", Latitude = "36:24:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سردشت", Longitude = "45:28:44", Latitude = "36:09:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیمینه", Longitude = "46:09:11", Latitude = "36:43:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کشاورز", Longitude = "46:21:30", Latitude = "36:50:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "باروق", Longitude = "46:19:11", Latitude = "36:57:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دیزج دیز", Longitude = "45:01:24", Latitude = "38:27:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ارومیه", Longitude = "45:03:27", Latitude = "37:32:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خلیفان", Longitude = "45:47:47", Latitude = "36:30:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "طالقان", Longitude = "50:46:06", Latitude = "36:10:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گرمدره", Longitude = "51:04:07", Latitude = "35:44:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اشتهارد", Longitude = "50:21:41", Latitude = "35:43:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نظرآباد", Longitude = "50:36:16", Latitude = "35:57:07", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هشتگرد", Longitude = "50:41:01", Latitude = "35:57:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کمال شهر", Longitude = "50:53:02", Latitude = "35:50:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کرج", Longitude = "50:57:09", Latitude = "35:43:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مشکین دشت", Longitude = "50:56:26", Latitude = "35:44:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محمدشهر", Longitude = "50:53:58", Latitude = "35:44:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ماهدشت", Longitude = "50:48:22", Latitude = "35:43:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شهرجدیدهشتگرد", Longitude = "50:44:28", Latitude = "35:59:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوهسار", Longitude = "50:47:36", Latitude = "35:57:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چهارباغ", Longitude = "50:50:52", Latitude = "35:50:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تنکمان", Longitude = "50:36:53", Latitude = "35:53:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیف آباد", Longitude = "50:46:13", Latitude = "35:54:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آسارا", Longitude = "51:10:25", Latitude = "36:01:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لوشان", Longitude = "49:30:53", Latitude = "36:37:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بره سر", Longitude = "49:45:08", Latitude = "36:46:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جیرنده", Longitude = "49:47:26", Latitude = "36:42:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "توتکابن", Longitude = "49:31:32", Latitude = "36:53:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رستم آباد", Longitude = "49:29:29", Latitude = "36:53:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "منجیل", Longitude = "49:25:06", Latitude = "36:44:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رودبار", Longitude = "49:25:08", Latitude = "36:48:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خمام", Longitude = "49:39:06", Latitude = "37:22:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سنگر", Longitude = "49:41:51", Latitude = "37:10:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لاهیجان", Longitude = "50:00:38", Latitude = "37:12:08", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خشکبیجار", Longitude = "49:45:31", Latitude = "37:22:26", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لشت نشاء", Longitude = "49:51:42", Latitude = "37:21:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کیاشهر", Longitude = "49:55:49", Latitude = "37:25:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوچصفهان", Longitude = "49:46:05", Latitude = "37:16:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیاهکل", Longitude = "49:52:20", Latitude = "37:09:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آستانه اشرفیه", Longitude = "49:56:41", Latitude = "37:15:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آستارا", Longitude = "48:52:09", Latitude = "38:23:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دیلمان", Longitude = "49:54:25", Latitude = "36:53:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فومن", Longitude = "49:18:39", Latitude = "37:13:32", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مرجقل", Longitude = "49:22:46", Latitude = "37:17:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بازارجمعه", Longitude = "49:07:05", Latitude = "37:24:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هشتپر", Longitude = "48:54:15", Latitude = "37:47:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اسالم", Longitude = "48:56:45", Latitude = "37:44:08", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پره سر", Longitude = "49:04:09", Latitude = "37:36:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رضوانشهر", Longitude = "49:08:22", Latitude = "37:33:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ماسال", Longitude = "49:07:54", Latitude = "37:21:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گوراب زرمیخ", Longitude = "49:13:07", Latitude = "37:18:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صومعه سرا", Longitude = "49:19:12", Latitude = "37:18:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شفت", Longitude = "49:24:11", Latitude = "37:09:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "احمدسرگوراب", Longitude = "49:22:04", Latitude = "37:08:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چوبر", Longitude = "49:25:18", Latitude = "37:05:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ماسوله", Longitude = "48:59:21", Latitude = "37:09:21", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رودبنه", Longitude = "50:00:29", Latitude = "37:15:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لنگرود", Longitude = "50:08:50", Latitude = "37:11:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کومله", Longitude = "50:10:34", Latitude = "37:09:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شلمان", Longitude = "50:12:59", Latitude = "37:09:35", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اطاقور", Longitude = "50:06:49", Latitude = "37:06:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "املش", Longitude = "50:11:27", Latitude = "37:05:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رانکوه", Longitude = "50:14:11", Latitude = "37:02:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رحیم آباد", Longitude = "50:20:13", Latitude = "37:01:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رشت", Longitude = "49:35:25", Latitude = "37:17:02", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لوندویل", Longitude = "48:51:48", Latitude = "38:18:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حویق", Longitude = "48:53:25", Latitude = "38:09:15", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لیسار", Longitude = "48:54:58", Latitude = "37:56:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بندرانزلی", Longitude = "49:27:45", Latitude = "37:28:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رودسر", Longitude = "50:18:00", Latitude = "37:07:57", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کلاچای", Longitude = "50:23:57", Latitude = "37:04:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "واجارگاه", Longitude = "50:24:49", Latitude = "37:02:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چابکسر", Longitude = "50:33:16", Latitude = "36:58:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لولمان", Longitude = "49:49:30", Latitude = "37:15:08", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چاف وچمخاله", Longitude = "50:13:32", Latitude = "37:12:59", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نوکنده", Longitude = "53:54:47", Latitude = "36:44:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بندرگز", Longitude = "53:57:02", Latitude = "36:45:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مراوه تپه", Longitude = "55:57:42", Latitude = "37:54:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اینچه برون", Longitude = "54:43:08", Latitude = "37:27:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گمیش تپه", Longitude = "54:04:31", Latitude = "37:04:17", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سیمین شهر", Longitude = "54:13:55", Latitude = "37:00:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آق قلا", Longitude = "54:27:24", Latitude = "37:00:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "انبارآلوم", Longitude = "54:37:07", Latitude = "37:07:57", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خان ببین", Longitude = "54:59:16", Latitude = "37:00:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دلند", Longitude = "55:02:32", Latitude = "37:02:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رامیان", Longitude = "55:08:31", Latitude = "37:01:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آزادشهر", Longitude = "55:10:10", Latitude = "37:05:11", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نوده خاندوز", Longitude = "55:15:43", Latitude = "37:04:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نگین شهر", Longitude = "55:09:50", Latitude = "37:08:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گنبد کاووس", Longitude = "55:10:32", Latitude = "37:14:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مینودشت", Longitude = "55:22:30", Latitude = "37:13:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گالیکش", Longitude = "55:25:51", Latitude = "37:16:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کلاله", Longitude = "55:29:29", Latitude = "37:22:52", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرخنکلاته", Longitude = "54:34:09", Latitude = "36:52:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "علی آباد", Longitude = "54:51:27", Latitude = "36:54:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فاضل آباد", Longitude = "54:45:04", Latitude = "36:53:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کردکوی", Longitude = "54:06:35", Latitude = "36:47:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جلین", Longitude = "54:32:12", Latitude = "36:51:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گرگان", Longitude = "54:25:43", Latitude = "36:50:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ترکمن", Longitude = "54:04:30", Latitude = "36:54:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هندودر", Longitude = "49:13:51", Latitude = "33:46:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "غرق آباد", Longitude = "49:49:49", Latitude = "35:06:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نوبران", Longitude = "49:42:33", Latitude = "35:07:47", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نراق", Longitude = "50:50:16", Latitude = "34:00:42", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سنجان", Longitude = "49:37:27", Latitude = "34:03:03", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "داودآباد", Longitude = "49:51:18", Latitude = "34:17:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "توره", Longitude = "49:17:19", Latitude = "34:02:46", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رازقان", Longitude = "49:57:21", Latitude = "35:19:51", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خنداب", Longitude = "49:11:20", Latitude = "34:23:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شازند", Longitude = "49:24:39", Latitude = "33:56:01", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آستانه", Longitude = "49:21:07", Latitude = "33:53:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قورچی باشی", Longitude = "49:52:38", Latitude = "33:40:29", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خمین", Longitude = "50:04:49", Latitude = "33:38:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "محلات", Longitude = "50:27:14", Latitude = "33:54:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نیمور", Longitude = "50:34:25", Latitude = "33:53:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دلیجان", Longitude = "50:40:53", Latitude = "33:59:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میلاجرد", Longitude = "49:12:34", Latitude = "34:37:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کمیجان", Longitude = "49:19:20", Latitude = "34:43:08", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "فرمهین", Longitude = "49:41:03", Latitude = "34:29:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آشتیان", Longitude = "50:00:20", Latitude = "34:31:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تفرش", Longitude = "50:01:21", Latitude = "34:41:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ساوه", Longitude = "50:21:53", Latitude = "35:01:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مأمونیه", Longitude = "50:29:50", Latitude = "35:18:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زاویه", Longitude = "50:34:13", Latitude = "35:22:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پرندک", Longitude = "50:40:50", Latitude = "35:21:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "خشکرود", Longitude = "50:20:08", Latitude = "35:23:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کرهرود", Longitude = "49:39:00", Latitude = "34:03:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اراک", Longitude = "49:43:19", Latitude = "34:05:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جاورسیان", Longitude = "49:19:37", Latitude = "34:15:23", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ساروق", Longitude = "49:30:27", Latitude = "34:24:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شهرجدیدمهاجران", Longitude = "49:26:03", Latitude = "34:03:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ایوانکی", Longitude = "52:04:08", Latitude = "35:20:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گرمسار", Longitude = "52:20:01", Latitude = "35:13:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آرادان", Longitude = "52:29:51", Latitude = "35:15:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شهمیرزاد", Longitude = "53:20:00", Latitude = "35:46:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مهدی شهر", Longitude = "53:20:45", Latitude = "35:41:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سمنان", Longitude = "53:22:54", Latitude = "35:34:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرخه", Longitude = "53:12:37", Latitude = "35:27:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "درجزین", Longitude = "53:20:01", Latitude = "35:38:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شاهرود", Longitude = "54:58:20", Latitude = "36:24:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دیباج", Longitude = "54:13:44", Latitude = "36:25:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دامغان", Longitude = "54:20:41", Latitude = "36:09:54", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مجن", Longitude = "54:38:52", Latitude = "36:28:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بسطام", Longitude = "55:00:03", Latitude = "36:29:04", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کلاته خیج", Longitude = "55:18:01", Latitude = "36:40:14", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میامی", Longitude = "55:39:05", Latitude = "36:24:33", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بیارجمند", Longitude = "55:48:26", Latitude = "36:04:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "امیریه", Longitude = "54:08:31", Latitude = "36:01:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کهک", Longitude = "50:51:50", Latitude = "34:23:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دستجرد", Longitude = "50:14:53", Latitude = "34:33:10", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قنوات", Longitude = "51:01:31", Latitude = "34:36:39", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جعفریه", Longitude = "50:30:59", Latitude = "34:46:28", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سلفچگان", Longitude = "50:27:27", Latitude = "34:28:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قم", Longitude = "50:52:27", Latitude = "34:35:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "حمیل", Longitude = "46:46:22", Latitude = "33:56:16", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کوزران", Longitude = "46:36:07", Latitude = "34:29:44", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هلشی", Longitude = "47:05:25", Latitude = "34:06:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بیستون", Longitude = "47:26:51", Latitude = "34:23:40", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صحنه", Longitude = "47:41:06", Latitude = "34:28:31", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سنقر", Longitude = "47:35:58", Latitude = "34:45:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سطر", Longitude = "47:27:39", Latitude = "34:48:48", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میان راهان", Longitude = "47:26:40", Latitude = "34:35:01", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "هرسین", Longitude = "47:34:49", Latitude = "34:16:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کنگاور", Longitude = "47:57:43", Latitude = "34:30:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سومار", Longitude = "45:38:27", Latitude = "33:51:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ازگله", Longitude = "45:50:30", Latitude = "34:50:00", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرپل ذهاب", Longitude = "45:51:46", Latitude = "34:26:55", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کرندغرب", Longitude = "46:13:55", Latitude = "34:16:43", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گهواره", Longitude = "46:24:57", Latitude = "34:20:38", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "تازه آباد", Longitude = "46:09:05", Latitude = "34:44:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "باینگان", Longitude = "46:16:13", Latitude = "34:58:53", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "جوانرود", Longitude = "46:29:31", Latitude = "34:48:22", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "قصرشیرین", Longitude = "45:35:21", Latitude = "34:30:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "گیلانغرب", Longitude = "45:55:29", Latitude = "34:08:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرمست", Longitude = "46:19:54", Latitude = "34:01:37", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "اسلام آبادغرب", Longitude = "46:31:14", Latitude = "34:06:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "روانسر", Longitude = "46:39:21", Latitude = "34:42:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "رباط", Longitude = "46:48:23", Latitude = "34:16:06", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پاوه", Longitude = "46:22:01", Latitude = "35:01:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نودشه", Longitude = "46:15:12", Latitude = "35:10:49", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "کرمانشاه", Longitude = "47:05:49", Latitude = "34:18:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "نوسود", Longitude = "46:12:23", Latitude = "35:09:27", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "شاهو", Longitude = "46:27:39", Latitude = "34:55:50", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "توحید", Longitude = "47:04:05", Latitude = "33:43:36", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "لومار", Longitude = "46:48:40", Latitude = "33:34:01", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دره شهر", Longitude = "47:22:53", Latitude = "33:08:58", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "پهله", Longitude = "46:53:00", Latitude = "33:00:41", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آبدانان", Longitude = "47:25:26", Latitude = "32:59:18", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مورموری", Longitude = "47:40:30", Latitude = "32:43:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دهلران", Longitude = "47:16:15", Latitude = "32:41:24", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "موسیان", Longitude = "47:22:37", Latitude = "32:31:12", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ایوان", Longitude = "46:18:22", Latitude = "33:49:45", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "زرنه", Longitude = "46:11:12", Latitude = "33:55:34", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "آسمان آباد", Longitude = "46:27:56", Latitude = "33:50:57", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ایلام", Longitude = "46:24:59", Latitude = "33:34:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "چوار", Longitude = "46:17:49", Latitude = "33:41:30", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "مهران", Longitude = "46:10:22", Latitude = "33:07:19", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "صالح آباد", Longitude = "46:11:27", Latitude = "33:28:07", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سرابله", Longitude = "46:33:40", Latitude = "33:46:05", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "بدره", Longitude = "47:02:17", Latitude = "33:18:25", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "ارکواز", Longitude = "46:35:51", Latitude = "33:23:13", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "میمه", Longitude = "46:55:07", Latitude = "33:13:35", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "دلگشا", Longitude = "46:35:39", Latitude = "33:24:20", Timezone = "-3:30", DST = "0" };
            yield return new Models.Location { Name = "سراب باغ", Longitude = "47:34:21", Latitude = "32:53:48", Timezone = "-3:30", DST = "0" };
        }
    }
}

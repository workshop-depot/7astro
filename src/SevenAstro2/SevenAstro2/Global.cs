using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace SevenAstro2
{
    static partial class Global
    {
        public static readonly string WorkingDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "7astro");
        //public static readonly string CalcConfDir = System.IO.Path.Combine(WorkingDir, "Configuration");
        //public static readonly string CalcConfPath = System.IO.Path.Combine(CalcConfDir, "calc-conf.conf");
        public static readonly string PersonsDir = System.IO.Path.Combine(WorkingDir, "Persons");
        public static string SwephPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sweph");
        public static readonly string AppData = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "7astro");
        public static Calculations.Supplementary.Conf Conf;
        public static readonly Random RND;

        static Global()
        {
            if (!System.IO.Directory.Exists(SwephPath)) SwephPath = null;

            Conf = new Calculations.Supplementary.Conf(
                SwephPath,
                Calculations.Supplementary.SiderealMode.Lahiri,
                Calculations.Supplementary.PositionType.Sidereal,
                Calculations.Supplementary.HouseSystem.Koch,
                Calculations.Supplementary.NodeType.TrueNode,
                Calculations.Supplementary.ApogType.OscuApog);

            if (!System.IO.Directory.Exists(PersonsDir)) System.IO.Directory.CreateDirectory(PersonsDir);
            if (!System.IO.Directory.Exists(AppData)) System.IO.Directory.CreateDirectory(AppData);

            RND = new Random((int)DateTime.Now.Ticks);
        }

        #region logging
        public static readonly Logger AppLog = LogManager.GetLogger("AppLog");

        [Conditional("DEBUG")]
        public static void LogError(object o)
        {
            try { AppLog.Error(o.Text().Trim()); }
            catch { }
        }

        [Conditional("DEBUG")]
        public static void LogError(string format, object o, params object[] rest)
        {
            try
            {
                var all = new List<object>();
                all.Add(o);
                all.AddRange(rest);

                AppLog.Error(string.Format(format, all.ToArray()).Trim());
            }
            catch { }
        }

        [Conditional("DEBUG")]
        public static void Log(object o)
        {
            try { AppLog.Info(o.Text()); }
            catch { }
        }

        [Conditional("DEBUG")]
        public static void Log(string format, object o, params object[] rest)
        {
            try
            {
                var all = new List<object>();
                all.Add(o);
                all.AddRange(rest);

                AppLog.Info(string.Format(format, all.ToArray()).Trim());
            }
            catch { }
        }

        static System.Diagnostics.EventLog __createEventLog()
        {
            var eventLog = new System.Diagnostics.EventLog();
            eventLog.Source = "7astro";

            return eventLog;
        }

        public static void LogEvent(object s, System.Diagnostics.EventLogEntryType t = EventLogEntryType.Information, int id = 100)
        {
            try { using (var syslog = __createEventLog()) syslog.WriteEntry(s.Text(), t, id); }
            catch { }
        }
        #endregion

        #region fx
        public static string Text(this object oj) { return (oj ?? string.Empty).ToString(); }
        public static string TextBin(this IEnumerable<byte> seq) { return string.Join(" ", (from b in seq let s = string.Format("{0:X2}", b) select s).ToArray()); }

        public static string Tidy(this ManagementObject mob, string name)
        {
            try { return mob.Properties[name].Value.Text().Trim(); }
            catch { return string.Empty; }
        }

        public static string Tidy(this ManagementBaseObject mob, string name)
        {
            try { return mob.Properties[name].Value.Text().Trim(); }
            catch { return string.Empty; }
        }

        public static string Tidy(this string str)
        {
            try
            {
                str = str.Text().Trim();
                return Regex.Replace(str, "\\s", string.Empty);
            }
            catch { return string.Empty; }
        }

        public static string Description(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static bool IsSupplied(this string arg) { return !string.IsNullOrWhiteSpace(arg); }

        public static bool IsSupplied<T>(this T t) { return !object.Equals(t, default(T)); }

        public static R Pipe<T, R>(this T o, Func<T, R> func)
        {
            if (func == null) throw new ArgumentNullException("func", "'func' can not be null.");
            T buffer = o;
            return func(buffer);
        }

        public static T Pipe<T>(this T o, Action<T> action)
        {
            if (action == null) throw new ArgumentNullException("action", "'action' can not be null.");
            T buffer = o;
            action(buffer);
            return buffer;
        }

        public static string BinaryText(this byte[] bytes)
        {
            var data = new StringBuilder();
            foreach (var b in bytes)
            {
                data.Append(b.ToString("X2"));
                data.Append(" ");
            }
            return data.ToString().Trim();
        }

        public static string JSONSerialize<T>(this T t)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(t);
        }

        public static T JSONDeserialize<T>(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static void Delay(this int delay) { System.Threading.Thread.Sleep(delay); }

        public const string ToTimeSpanPattern = "(?<sign>[+-]*)(?<hour>\\d+)(:(?<minute>\\d+):(?<second>\\d+))*";
        public static TimeSpan ToTimeSpan(this string s)
        {
            var m = Regex.Match(s, ToTimeSpanPattern);

            if (!m.Success) return TimeSpan.Zero;

            int hour = int.Parse(m.Groups["hour"].Value);
            int minute = 0;
            if (m.Groups["minute"].Success)
                minute = int.Parse(m.Groups["minute"].Value);
            int second = 0;
            if (m.Groups["second"].Success)
                second = int.Parse(m.Groups["second"].Value);
            var sign = m.Groups["sign"].Value;

            var ts = new TimeSpan(hour, minute, second);

            if (sign == "-") return TimeSpan.FromHours(ts.TotalHours * -1);
            return ts;
        }

        public static IEnumerable<string[]> Records(this Microsoft.VisualBasic.FileIO.TextFieldParser csv) { return csv.Records(true); }
        public static IEnumerable<string[]> Records(this Microsoft.VisualBasic.FileIO.TextFieldParser csv, bool ignoreInvalidRecords)
        {
            if (csv.Delimiters == null || csv.Delimiters.Length == 0) csv.SetDelimiters(",");

            string[] record = null;
            while (!csv.EndOfData)
            {
                record = null;

                if (ignoreInvalidRecords)
                {
                    try { record = csv.ReadFields(); }
                    catch { }
                }
                else record = csv.ReadFields();

                if (record != null) yield return record;
            }
        }
        static string PutInQuotes(object o) { return string.Format("\"{0}\"", o); }
        public static void WriteRecord(this System.IO.StreamWriter writer, params object[] fields)
        {
            string delimiter = ",";
            writer.WriteLine(string.Join(delimiter, (from o in fields
                                                     let str = (o ?? string.Empty).ToString().Replace("\"", "\"\"")
                                                     select (str.Contains(delimiter) || str.Contains("\"")) ? PutInQuotes(str) : str).ToArray()));
        }
        #endregion

        #region calc
        public static DateTime ToUT(DateTime time, TimeSpan zone, TimeSpan dst)
        {
            return time.Add(zone).Add(dst);
        }

        public static DateTime FromUT(DateTime time, TimeSpan zone, TimeSpan dst)
        {
            return time.AddHours(zone.TotalHours * -1d).AddHours(dst.TotalHours * -1d);
        }

        public static double TotalDegrees(this TimeSpan ts) { return ts.TotalHours; }
        public static int Degrees(this TimeSpan ts) { return (int)Math.Truncate(ts.TotalHours); }
        public static double FractionOfSecond(this TimeSpan ts) { return ts.TotalSeconds - Math.Truncate(ts.TotalSeconds); }
        public static string ShowAsDegree(this TimeSpan ts)
        {
            var degrees = ts.Degrees();
            var sign = Math.Sign(degrees);

            degrees = Math.Abs(degrees);
            var minutes = Math.Abs(ts.Minutes);
            var seconds = Math.Abs(ts.Seconds);

            return string.Format("{0}{1:00}:{2:00}:{3:00}", sign < 0 ? "-" : string.Empty, degrees, minutes, seconds);
        }
        public static TimeSpan ParseAsDegree(this string s)
        {
            var pattern = @"^(?<sign>[-+]*)(?<deg>\d{1,3}):(?<min>\d{1,2}):(?<sec>\d{1,2})$";

            var m = Regex.Match(s, pattern);

            if (!m.Success) return default(TimeSpan);

            var s_sign = m.Groups["sign"].Value;
            var s_deg = m.Groups["deg"].Value;
            var s_min = m.Groups["min"].Value;
            var s_sec = m.Groups["sec"].Value;

            var deg = int.Parse(s_deg);
            var min = int.Parse(s_min);
            var sec = int.Parse(s_sec);

            var ts = new TimeSpan(deg, min, sec);

            ts = TimeSpan.FromHours((s_sign == "-" ? -1 : 1) * ts.TotalDegrees());

            return ts;
        }

        public static double Range(this double d, double inclusiveStart, double exclusiveEnd)
        {
            var len = exclusiveEnd - inclusiveStart;
            while (d >= exclusiveEnd) d -= len;
            while (d < inclusiveStart) d += len;
            return d;
        }

        public static int Range(this int d, int inclusiveStart, int exclusiveEnd)
        {
            var len = exclusiveEnd - inclusiveStart;
            while (d >= exclusiveEnd) d -= len;
            while (d < inclusiveStart) d += len;
            return d;
        }
        #endregion

        public static void SetDateFormat()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture(System.Globalization.CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //ci.DateTimeFormat.LongDatePattern = "dd/MM/yyyy HH:mm:ss";
            //ci.DateTimeFormat.FullDateTimePattern = "dd/MM/yyyy HH:mm:ss";
            //ci.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
            //ci.DateTimeFormat.DateSeparator = "/";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
        }

        public static SevenAstro2.Views.BirthDataView BirthDataView { get; set; }
        public static SevenAstro2.Views.TimeStepperView TimeStepperView { get; set; }
        public static SevenAstro2.Views.ConvertDateView ConvertDateView { get; set; }
        //public static MainWindow MainWindow { get; set; }

        public static T TryFindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = GetParentObject(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                //use recursion to proceed with next level
                return TryFindParent<T>(parentObject);
            }
        }

        static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null) return null;

            //handle content elements separately
            ContentElement contentElement = child as ContentElement;
            if (contentElement != null)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;

                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            FrameworkElement frameworkElement = child as FrameworkElement;
            if (frameworkElement != null)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null) return parent;
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }
    }

    #region class Args
    class Args
    {
        #region body
        const string ARG_TEMPLATE = "(?<parameter>(?<prefix>^-{1,2}|^/)(?<switch>[^=:]+)(?<splitter>[=:]{0,1})(?<value>.*$))|(?<argument>.*)";
        readonly List<Arg> _args;

        public Args() { _args = Parser(Environment.GetCommandLineArgs()); }
        public Args(string[] args) { _args = Parser(args); }

        public string this[string @switch]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(@switch)) throw new ArgumentNullException("@switch");

                var @param = (from a in _args where a.IsParameter && string.Compare(a.Switch, @switch, true) == 0 select a).FirstOrDefault();
                if (@param != null)
                {
                    if (!string.IsNullOrWhiteSpace(@param.Value) || !string.IsNullOrWhiteSpace(@param.Splitter)) return @param.Value;
                    return @param.Switch;
                }

                @param = (from a in _args where !a.IsParameter && string.Compare(a.Value, @switch, true) == 0 select a).FirstOrDefault();
                if (@param != null) return @param.Value;

                return null;
            }
        }
        public string[] this[string @switch, int maxNumberOfArguments]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(@switch)) throw new ArgumentNullException("@switch");

                maxNumberOfArguments = maxNumberOfArguments == -1 ? int.MaxValue - 1 : maxNumberOfArguments;

                Arg @param = null;
                var args = new List<Arg>();

                var part1 = _args.SkipWhile(p => string.Compare(p.Switch, @switch, true) != 0);
                @param = part1.Take(1).FirstOrDefault();
                if (@param != null)
                {
                    args = part1.Skip(1).TakeWhile(a => !a.IsParameter).ToList();
                    args = args.Take(maxNumberOfArguments).ToList();
                }

                if (@param != null)
                {
                    return (from a in args select a.Value).ToArray();
                }
                else
                    return null;
            }
        }

        static List<Arg> Parser(string[] args)
        {
            if (args == null || args.Length == 0) return new List<Arg>();

            Func<Capture, string> getVal = c => { return c == null ? null : c.Value; };

            var result = new List<Arg>();

            foreach (var a in args)
            {
                var m = Regex.Match(a, ARG_TEMPLATE);
                if (m.Groups["parameter"] != null && m.Groups["parameter"].Success)
                {
                    result.Add(new Arg(
                        getVal(m.Groups["prefix"]),
                        getVal(m.Groups["switch"]),
                        getVal(m.Groups["splitter"]),
                        getVal(m.Groups["value"])));
                }
                else
                {
                    result.Add(new Arg(getVal(m.Groups["argument"])));
                }
            }

            return result;
        }
        #endregion

        #region class Arg
        class Arg
        {
            /// <summary>
            /// is a parameter or an argument
            /// </summary>
            public bool IsParameter { get; private set; }
            public string Prefix { get; private set; }
            public string Switch { get; private set; }
            public string Splitter { get; private set; }
            public string Value { get; private set; }

            private Arg() { }
            public Arg(
                string prefix,
                string switch_,
                string splitter,
                string val)
            {
                this.Prefix = prefix;
                this.Switch = switch_;
                this.Splitter = splitter;
                this.Value = val;
                this.IsParameter = true;
            }
            public Arg(
                string val)
            {
                this.Value = val;
                this.IsParameter = false;
            }

            public override string ToString()
            {
                return this.IsParameter ?
                    string.Format("{0}{1}{2}{3}",
                        Prefix ?? string.Empty,
                        Switch ?? string.Empty,
                        Splitter ?? string.Empty,
                        Value ?? string.Empty) :
                    string.Format("{0}", Value ?? string.Empty);
            }
        }
        #endregion
    }
    #endregion

    #region Converters
    class FontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double actualHeight = System.Convert.ToDouble(value);
            int fontSize = (int)(actualHeight * .5);
            return fontSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class GenderToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var s = (value ?? string.Empty).ToString();

            if (string.Compare(s, SevenAstro2.Models.Gender.Female.ToString(), true) == 0) return SevenAstro2.Models.Gender.Female;
            if (string.Compare(s, SevenAstro2.Models.Gender.Male.ToString(), true) == 0) return SevenAstro2.Models.Gender.Male;

            throw new ArgumentOutOfRangeException();
        }
    }

    class NodeTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var s = (value ?? string.Empty).ToString();

            if (string.Compare(s, Calculations.Supplementary.NodeType.TrueNode.ToString(), true) == 0) return Calculations.Supplementary.NodeType.TrueNode;
            if (string.Compare(s, Calculations.Supplementary.NodeType.MeanNode.ToString(), true) == 0) return Calculations.Supplementary.NodeType.MeanNode;

            throw new ArgumentOutOfRangeException();
        }
    }

    class SignToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var s = (value ?? string.Empty).ToString();

            var strings = Enum.GetNames(typeof(Calculations.Supplementary.Sign));
            var found = (from str in strings
                         where str == s
                         select str).First();
            Calculations.Supplementary.Sign sign;
            Enum.TryParse(found, out sign);
            return sign;
        }
    }

    class PlanetToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var s = (value ?? string.Empty).ToString();

            var strings = Enum.GetNames(typeof(Calculations.Supplementary.PointId));
            var found = (from str in strings
                         where str == s
                         select str).First();
            Calculations.Supplementary.PointId planet;
            Enum.TryParse(found, out planet);
            return planet;
        }
    }

    class DegreeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = value.ToString();

            if (value is double)
            {
                var deg = TimeSpan.FromHours((double)value);

                result = string.Format("{0:00}:{1:00}:{2:00}", deg.Degrees(), deg.Minutes, deg.Seconds);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Text().ToTimeSpan().TotalHours;
        }
    }
    #endregion

    [DebuggerNonUserCode]
    sealed class WeakEventHandler<TEventArgs> where TEventArgs : EventArgs
    {
        private readonly WeakReference _targetReference;
        private readonly MethodInfo _method;

        public WeakEventHandler(EventHandler<TEventArgs> callback)
        {
            _method = callback.Method;
            _targetReference = new WeakReference(callback.Target, true);
        }

        [DebuggerNonUserCode]
        public void Handler(object sender, TEventArgs e)
        {
            var target = _targetReference.Target;
            if (target != null)
            {
                var callback = (Action<object, TEventArgs>)Delegate.CreateDelegate(typeof(Action<object, TEventArgs>), target, _method, true);
                if (callback != null)
                {
                    callback(sender, e);
                }
            }
        }
    }
    
    #region Here
    /*
    static class Here
    {
        static StackFrame GetCaller(int index) { return new StackTrace(true).GetFrame(index); }

        public static string FileName
        {
            get
            {
                var first = GetCaller(2);
                if (first != null) return first.GetFileName();
                return string.Empty;
            }
        }
        public static int FileLine
        {
            get
            {
                var first = GetCaller(2);
                if (first != null) return first.GetFileLineNumber();
                return -1;
            }
        }
        public static string Member
        {
            get
            {
                var first = GetCaller(2);
                if (first != null)
                {
                    var mn = first.GetMethod().Name;

                    if (mn.StartsWith("get_") || mn.StartsWith("set_")) mn = mn.Substring(4);

                    return mn;
                }
                return string.Empty;
            }
        }
        public static Type Type
        {
            get
            {
                var first = GetCaller(2);
                if (first != null) return first.GetMethod().DeclaringType;
                return null;
            }
        }
        public static string CallerMemberName
        {
            get
            {
                var first = GetCaller(3);
                if (first != null)
                {
                    var mn = first.GetMethod().Name;

                    if (mn.StartsWith("get_") || mn.StartsWith("set_")) mn = mn.Substring(4);

                    return mn;
                }
                return string.Empty;
            }
        }
        public static int CallerLineNumber
        {
            get
            {
                var first = GetCaller(3);
                if (first != null) return first.GetFileLineNumber();
                return -1;
            }
        }
        public static string CallerFilePath
        {
            get
            {
                var first = GetCaller(3);
                if (first != null) return first.GetFileName();
                return string.Empty;
            }
        }
    }
    */
    #endregion
}

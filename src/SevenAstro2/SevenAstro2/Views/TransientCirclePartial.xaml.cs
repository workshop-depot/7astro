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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SevenAstro2.Views
{
    /// <summary>
    /// Interaction logic for TransientCirclePartial.xaml
    /// </summary>
    internal partial class TransientCirclePartial : UserControl
    {
        public TransientCirclePartial()
        {
            InitializeComponent();

            ClearAndDrawDiagram();
        }

        private void SizeChangedAct(object sender, SizeChangedEventArgs e)
        {
            ClearAndDrawDiagram();
        }

        internal void ClearAndDrawDiagram()
        {
            //var m = Math.Min(this.ActualHeight, this.ActualWidth);

            //this.Height = m;
            //this.Width = m;

            canArea.Children.Clear();

            DrawDiagram();
        }

        void DrawDiagram()
        {
            var ww = canArea.RenderSize.Width;
            var hh = canArea.RenderSize.Height;

            var xCenter = ww / 2d;
            var yCenter = hh / 2d;

            var min = Math.Min(xCenter, yCenter);

            var stroke = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));

            var innerCircleDiameter = min * 30d / 100d;
            var innerCircle = new Ellipse
            {
                Width = innerCircleDiameter,
                Height = innerCircleDiameter,
                Stroke = stroke,
                StrokeThickness = 1d,
                Margin = new Thickness(min - innerCircleDiameter / 2d)
            };
            canArea.Children.Add(innerCircle);

            var middleCircleDiameter = min * 125d / 100d;
            var middleCircle = new Ellipse
            {
                Width = middleCircleDiameter,
                Height = middleCircleDiameter,
                Stroke = stroke,
                StrokeThickness = 1d,
                Margin = new Thickness(min - middleCircleDiameter / 2d)
            };
            canArea.Children.Add(middleCircle);

            var outerCircleDiameter = min * 197d / 100d;
            var outerCircle = new Ellipse
            {
                Width = outerCircleDiameter,
                Height = outerCircleDiameter,
                Stroke = stroke,
                StrokeThickness = 1d,
                Margin = new Thickness(min - outerCircleDiameter / 2d)
            };
            canArea.Children.Add(outerCircle);

            // V 1
            //var initialAngle = 15;
            //for (int i = 0; i < 3; i++)
            //    DrawRadius(xCenter, yCenter, stroke, innerCircle, outerCircle, initialAngle + i * 30d);

            // V 2
            DrawRadius(min, stroke, innerCircleDiameter, outerCircleDiameter);

            TextBlock tb = null;
            double x;
            double y;
            var tblen = 17;

            double radius = 0;
            double degree = 0;

            radius = (middleCircle.Width / 2d) + ((middleCircle.Width / 2d) * 5d / 100d);
            for (int i = 0; i < 12; i++)
            {
                degree = i * 30d;
                FindPointOnArc(min, min, radius, degree, out x, out y);
                tb = new TextBlock
                {
                    Text = ((House)i + 1).ToString(),
                    Foreground = Brushes.CadetBlue,
                    TextAlignment = System.Windows.TextAlignment.Center,
                    Width = tblen,
                    Height = tblen
                };
                Canvas.SetLeft(tb, x - tblen / 2);
                Canvas.SetTop(tb, y - tblen / 2);
                canArea.Children.Add(tb);
            }

            degree = 90;
            radius = 1;
            var tbWidth = 50;
            var tbHeight = 35;
            FindPointOnArc(min, min, radius, degree, out x, out y);
            tb = new TextBlock
            {
                Text = "Gochara\r\nReport",
                Foreground = Brushes.CadetBlue,
                TextAlignment = System.Windows.TextAlignment.Center,
                Width = tbWidth,
                Height = tbHeight,
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeights.Bold
            };
            Canvas.SetLeft(tb, x - tbWidth / 2);
            Canvas.SetTop(tb, y - tbHeight / 2);
            canArea.Children.Add(tb);

            if (BirthChart != null && TransientChart != null)
            {
                var bcAsc = BirthChart.Asc;

                radius = (outerCircle.Width / 2d) - ((outerCircle.Width / 2d) * 5d / 100d);
                var sign = (int)bcAsc.Sign;
                for (int i = 0; i < 12; i++)
                {
                    degree = i * 30d;
                    FindPointOnArc(min, min, radius, degree, out x, out y);
                    tb = new TextBlock
                    {
                        Text = ((Sign)(sign + i).Range(1, 13)).ToString(),
                        Foreground = Brushes.CadetBlue,
                        TextAlignment = System.Windows.TextAlignment.Center,
                        Width = tblen,
                        Height = tblen
                    };
                    Canvas.SetLeft(tb, x - tblen / 2);
                    Canvas.SetTop(tb, y - tblen / 2);
                    canArea.Children.Add(tb);
                }

                tblen = 30;

                radius = (middleCircle.Width / 2d) - ((middleCircle.Width / 2d) * 15d / 100d);
                var ascDiff = bcAsc.Degree - 15;
                foreach (var p in BirthChart.Points)
                {
                    var pColor = GetPlanetColor(p.Id);

                    degree = p.Longitude - bcAsc.Longitude + ascDiff;
                    FindPointOnArc(min, min, radius, degree, out x, out y);
                    tb = new TextBlock
                    {
                        Text = p.Id.ToString(),
                        Foreground = pColor,
                        TextAlignment = System.Windows.TextAlignment.Center,
                        Width = tblen,
                        Height = tblen,
                        FontWeight = FontWeights.Bold,
                        FontSize = 15
                    };
                    Canvas.SetLeft(tb, x - tblen / 2);
                    Canvas.SetTop(tb, y - tblen / 2);
                    canArea.Children.Add(tb);

                    double X1;
                    double Y1;
                    double R1 = middleCircle.Width / 2d;
                    FindPointOnArc(min, min, R1, degree, out X1, out Y1);

                    double X2;
                    double Y2;
                    double R2 = middleCircle.Width / 2d - 15d;
                    FindPointOnArc(min, min, R2, degree, out X2, out Y2);

                    canArea.Children.Add(new Line
                    {
                        X1 = X1,
                        Y1 = Y1,
                        X2 = X2,
                        Y2 = Y2,
                        Stroke = pColor
                    });
                }

                radius = (middleCircle.Width / 2d) + ((middleCircle.Width / 2d) * 15d / 100d);
                foreach (var p in TransientChart.Points)
                {
                    var pColor = GetPlanetColor(p.Id);

                    degree = p.Longitude - bcAsc.Longitude + ascDiff;
                    FindPointOnArc(min, min, radius, degree, out x, out y);
                    tb = new TextBlock
                    {
                        Text = p.Id.ToString(),
                        Foreground = pColor,
                        TextAlignment = System.Windows.TextAlignment.Center,
                        Width = tblen,
                        Height = tblen,
                        FontWeight = FontWeights.Bold,
                        FontSize = 15
                    };
                    Canvas.SetLeft(tb, x - tblen / 2);
                    Canvas.SetTop(tb, y - tblen / 2);
                    canArea.Children.Add(tb);

                    double X1;
                    double Y1;
                    double R1 = middleCircle.Width / 2d;
                    FindPointOnArc(min, min, R1, degree, out X1, out Y1);

                    double X2;
                    double Y2;
                    double R2 = middleCircle.Width / 2d + 15d;
                    FindPointOnArc(min, min, R2, degree, out X2, out Y2);

                    canArea.Children.Add(new Line
                    {
                        X1 = X1,
                        Y1 = Y1,
                        X2 = X2,
                        Y2 = Y2,
                        Stroke = pColor
                    });
                }

                {
                    var R1 = (middleCircle.Width / 2d) + ((middleCircle.Width / 2d) * 30d / 100d);
                    var R2 = outerCircle.Width / 2d;
                    var transientAsc = (from p in TransientChart.Points
                                        where p.Id == Calculations.Supplementary.PointId.Asc
                                        select p).First();
                    degree = transientAsc.Longitude - bcAsc.Longitude + ascDiff;

                    double X1;
                    double Y1;
                    FindPointOnArc(min, min, R1, degree, out X1, out Y1);

                    double X2;
                    double Y2;
                    FindPointOnArc(min, min, R2, degree, out X2, out Y2);

                    canArea.Children.Add(new Line
                    {
                        X1 = X1,
                        Y1 = Y1,
                        X2 = X2,
                        Y2 = Y2,
                        Stroke = Brushes.OrangeRed,
                        StrokeThickness = 5
                    });

                    degree = (degree + 90).Range(0, 360);

                    FindPointOnArc(min, min, R1, degree, out X1, out Y1);

                    FindPointOnArc(min, min, R2, degree, out X2, out Y2);

                    canArea.Children.Add(new Line
                    {
                        X1 = X1,
                        Y1 = Y1,
                        X2 = X2,
                        Y2 = Y2,
                        Stroke = Brushes.Black,
                        StrokeThickness = 5
                    });

                    degree = (degree + 90).Range(0, 360);

                    FindPointOnArc(min, min, R1, degree, out X1, out Y1);

                    FindPointOnArc(min, min, R2, degree, out X2, out Y2);

                    canArea.Children.Add(new Line
                    {
                        X1 = X1,
                        Y1 = Y1,
                        X2 = X2,
                        Y2 = Y2,
                        Stroke = Brushes.OrangeRed,
                        StrokeThickness = 5
                    });

                    degree = (degree + 90).Range(0, 360);

                    FindPointOnArc(min, min, R1, degree, out X1, out Y1);

                    FindPointOnArc(min, min, R2, degree, out X2, out Y2);

                    canArea.Children.Add(new Line
                    {
                        X1 = X1,
                        Y1 = Y1,
                        X2 = X2,
                        Y2 = Y2,
                        Stroke = Brushes.SkyBlue,
                        StrokeThickness = 5
                    });
                }
            }
        }

        void DrawRadius(double min, SolidColorBrush stroke, double innerCircleDiameter, double outerCircleDiameter)
        {
            double XX1;
            double YY1;
            double XX2;
            double YY2;
            double aa = 15;
            for (int i = 0; i < 12; i++)
            {
                FindPointOnArc(min, min, innerCircleDiameter / 2d, aa + i * 30d, out XX1, out YY1);
                FindPointOnArc(min, min, outerCircleDiameter / 2d, aa + i * 30d, out XX2, out YY2);
                canArea.Children.Add(new Line
                {
                    X1 = XX1,
                    Y1 = YY1,
                    X2 = XX2,
                    Y2 = YY2,
                    Stroke = stroke
                });
            }
        }

        static SolidColorBrush GetPlanetColor(Calculations.Supplementary.PointId p)
        {
            switch (p)
            {
                case Calculations.Supplementary.PointId.Asc: return Brushes.Black;
                case Calculations.Supplementary.PointId.Ju: return Brushes.Blue;
                case Calculations.Supplementary.PointId.Ke: return Brushes.Brown;
                case Calculations.Supplementary.PointId.Ma: return Brushes.Red;
                case Calculations.Supplementary.PointId.Me: return Brushes.Green;
                case Calculations.Supplementary.PointId.Mo: return Brushes.DarkBlue;
                case Calculations.Supplementary.PointId.Ra: return Brushes.Brown;
                case Calculations.Supplementary.PointId.Sa: return Brushes.Violet;
                case Calculations.Supplementary.PointId.Su: return Brushes.DarkKhaki;
                case Calculations.Supplementary.PointId.Ve: return Brushes.Pink;
                default: return Brushes.Black;
            }
        }

        static void FindPointOnArc(double xCenter, double yCenter, double radius, double degree, out double x, out double y)
        {
            degree += 90d;
            degree = degree % 360;
            var radian = 2d * Math.PI * degree / 360d;

            x = xCenter + radius * Math.Cos(radian);
            y = yCenter - radius * Math.Sin(radian);
        }

        static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as TransientCirclePartial;

            if (ctrl != null) ctrl.ClearAndDrawDiagram();
        }

        public Calculations.Supplementary.VdChart TransientChart
        {
            get { return (Calculations.Supplementary.VdChart)GetValue(TransientChartProperty); }
            set { SetValue(TransientChartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TransientChart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TransientChartProperty =
            DependencyProperty.Register("TransientChart", typeof(Calculations.Supplementary.VdChart), typeof(TransientCirclePartial), new FrameworkPropertyMetadata(Changed));

        public Calculations.Supplementary.VdChart BirthChart
        {
            get { return (Calculations.Supplementary.VdChart)GetValue(BirthChartProperty); }
            set { SetValue(BirthChartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BirthChart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BirthChartProperty =
            DependencyProperty.Register("BirthChart", typeof(Calculations.Supplementary.VdChart), typeof(TransientCirclePartial));
    }

    enum House
    {
        I = 1,
        II = 2,
        III = 3,
        IV = 4,
        V = 5,
        VI = 6,
        VII = 7,
        VIII = 8,
        IX = 9,
        X = 10,
        XI = 11,
        XII = 12
    }

    enum Sign
    {
        Ar = 1,
        Ta = 2,
        Ge = 3,
        Ca = 4,
        Le = 5,
        Vi = 6,
        Li = 7,
        Sc = 8,
        Sg = 9,
        Cp = 10,
        Aq = 11,
        Pi = 12
    }
}

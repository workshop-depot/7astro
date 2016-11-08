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
    /// Interaction logic for ChartPartial.xaml
    /// </summary>
    internal partial class ChartPartial : UserControl
    {
        public ChartPartial()
        {
            InitializeComponent();

            ChartBackground = (Brush)FindResource("brush_c_FFFF99");
            ChartFontSize = ShowDegree ? 12d : 14d;
        }

        void SizeChangedAct(object sender, SizeChangedEventArgs e)
        {
            //var m = Math.Min(this.ActualHeight, this.ActualWidth);
            //var mm = ShowDegree ? 550 : 300;
            //if (m < mm) m = mm;

            //this.Height = m;
            //this.Width = m;

            ClearAndDrawChart(sender, e);
        }

        public void ClearAndDrawChart(object sender, SizeChangedEventArgs e)
        {
            canArea.Children.Clear();

            DrawChart(sender, e);
        }

        double? lastCalculatedFontSize = null;
        void DrawChart(object sender, SizeChangedEventArgs e)
        {
            var ww = canArea.ActualWidth;
            var hh = canArea.ActualHeight;
            var ss = Math.Min(ww, hh);

            var xCenter = ww / 2d;
            var yCenter = hh / 2d;
            var wCenter = 15d;

            canArea.Children.Add(
                new Line
                {
                    X1 = xCenter - wCenter,
                    Y1 = yCenter - wCenter,
                    X2 = xCenter + wCenter,
                    Y2 = yCenter - wCenter,
                    Stroke = Brushes.Black
                });

            canArea.Children.Add(
                new Line
                {
                    X1 = xCenter + wCenter,
                    Y1 = yCenter - wCenter,
                    X2 = xCenter + wCenter,
                    Y2 = yCenter + wCenter,
                    Stroke = Brushes.Black
                });

            canArea.Children.Add(
                new Line
                {
                    X1 = xCenter + wCenter,
                    Y1 = yCenter + wCenter,
                    X2 = xCenter - wCenter,
                    Y2 = yCenter + wCenter,
                    Stroke = Brushes.Black
                });

            canArea.Children.Add(
                new Line
                {
                    X1 = xCenter - wCenter,
                    Y1 = yCenter + wCenter,
                    X2 = xCenter - wCenter,
                    Y2 = yCenter - wCenter,
                    Stroke = Brushes.Black
                });

            if (Chart != null)
            {
                var tb = new TextBlock
                {
                    Text = HideCenterChar ? string.Empty : Chart.Division.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = 20,
                    Height = 20,
                    FontSize = 14
                };
                Canvas.SetTop(tb, yCenter - wCenter + 5);
                Canvas.SetLeft(tb, xCenter - wCenter + 5);
                canArea.Children.Add(tb);
            }

            canArea.Children.Add(new Line
            {
                X1 = 0,
                Y1 = 0,
                X2 = xCenter - wCenter,
                Y2 = yCenter - wCenter,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww,
                Y1 = 0,
                X2 = xCenter + wCenter,
                Y2 = yCenter - wCenter,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww,
                Y1 = hh,
                X2 = xCenter + wCenter,
                Y2 = yCenter + wCenter,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = 0,
                Y1 = hh,
                X2 = xCenter - wCenter,
                Y2 = yCenter + wCenter,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = 0,
                Y1 = 0,
                X2 = ww,
                Y2 = 0,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww,
                Y1 = 0,
                X2 = ww,
                Y2 = hh,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = 0,
                Y1 = hh,
                X2 = ww,
                Y2 = hh,
                Stroke = Brushes.Black
            });
            canArea.Children.Add(new Line
            {
                X1 = 0,
                Y1 = hh,
                X2 = 0,
                Y2 = 0,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww / 2d,
                Y1 = 0,
                X2 = ww,
                Y2 = hh / 2d,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww / 2d,
                Y1 = hh,
                X2 = ww,
                Y2 = hh / 2d,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww / 2d,
                Y1 = hh,
                X2 = 0,
                Y2 = hh / 2d,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww / 2d,
                Y1 = 0,
                X2 = 0,
                Y2 = hh / 2d,
                Stroke = Brushes.Black
            });

            var g1 = new Grid { Width = ww / 4d, Height = hh / 4d };
            var wpb1 = PrepWrapPanel(g1);
            Canvas.SetLeft(g1, ww * 3d / 8d);
            Canvas.SetTop(g1, hh / 8d);

            var g2 = new Grid { Width = ww / 4d, Height = hh / 8d };
            var wpb2 = PrepWrapPanel(g2);
            Canvas.SetLeft(g2, ww / 8d);
            Canvas.SetTop(g2, 0);

            var g3 = new Grid { Width = ww / 8d, Height = hh / 4d };
            var wpb3 = PrepWrapPanel(g3);
            Canvas.SetLeft(g3, 0);
            Canvas.SetTop(g3, hh / 8d);

            var g4 = new Grid { Width = ww / 4d, Height = hh / 4d };
            var wpb4 = PrepWrapPanel(g4);
            Canvas.SetLeft(g4, ww / 8d);
            Canvas.SetTop(g4, hh * 3d / 8d);

            var g5 = new Grid { Width = ww / 8d, Height = hh / 4d };
            var wpb5 = PrepWrapPanel(g5);
            Canvas.SetLeft(g5, 0);
            Canvas.SetTop(g5, hh * 5d / 8d);

            var g6 = new Grid { Width = ww / 4d, Height = hh / 8d };
            var wpb6 = PrepWrapPanel(g6);
            Canvas.SetLeft(g6, ww / 8d);
            Canvas.SetTop(g6, hh * 7d / 8d);

            var g7 = new Grid { Width = ww / 4d, Height = hh / 4d };
            var wpb7 = PrepWrapPanel(g7);
            Canvas.SetLeft(g7, ww * 3d / 8d);
            Canvas.SetTop(g7, hh * 5d / 8d);

            var g8 = new Grid { Width = ww / 4d, Height = hh / 8d };
            var wpb8 = PrepWrapPanel(g8);
            Canvas.SetLeft(g8, ww * 5d / 8d);
            Canvas.SetTop(g8, hh * 7d / 8d);

            var g9 = new Grid { Width = ww / 8d, Height = hh / 4d };
            var wpb9 = PrepWrapPanel(g9);
            Canvas.SetLeft(g9, ww * 7d / 8d);
            Canvas.SetTop(g9, hh * 5d / 8d);

            var g10 = new Grid { Width = ww / 4d, Height = hh / 4d };
            var wpb10 = PrepWrapPanel(g10);
            Canvas.SetLeft(g10, ww * 5d / 8d);
            Canvas.SetTop(g10, hh * 3d / 8d);

            var g11 = new Grid { Width = ww / 8d, Height = hh / 4d };
            var wpb11 = PrepWrapPanel(g11);
            Canvas.SetLeft(g11, ww * 7d / 8d);
            Canvas.SetTop(g11, hh / 8d);

            var g12 = new Grid { Width = ww / 4d, Height = hh / 8d };
            var wpb12 = PrepWrapPanel(g12);
            Canvas.SetLeft(g12, ww * 5d / 8d);
            Canvas.SetTop(g12, 0);

            canArea.Children.Add(g1);
            canArea.Children.Add(g2);
            canArea.Children.Add(g3);
            canArea.Children.Add(g4);
            canArea.Children.Add(g5);
            canArea.Children.Add(g6);
            canArea.Children.Add(g7);
            canArea.Children.Add(g8);
            canArea.Children.Add(g9);
            canArea.Children.Add(g10);
            canArea.Children.Add(g11);
            canArea.Children.Add(g12);

            if (Chart != null)
            {
                var grouped = (from p in Chart.Points
                               orderby p.Degree
                               group p by p.ClassicHouse into h
                               select h).ToList();

                var asc = Chart.Asc;

                // V 1
                //var planetsFontSize = ww * 2d / 100d;
                //planetsFontSize = planetsFontSize < 12 ? 12 : planetsFontSize;
                //planetsFontSize = planetsFontSize > 12 ? 12 : planetsFontSize;
                //if (ShowDegree) planetsFontSize = planetsFontSize > 12 ? 12 : planetsFontSize;

                // V 2
                //var planetsFontSize = 10.5d;

                // V 3
                //double planetsFontSize = ChartFontSize;

                //if (e != null)
                //{
                //    planetsFontSize = Math.Min(e.NewSize.Width, e.NewSize.Height) * 0.04;
                //    lastCalculatedFontSize = planetsFontSize;
                //}
                //else if (lastCalculatedFontSize.HasValue) planetsFontSize = lastCalculatedFontSize.Value;

                // V 4
                double planetsFontSize = 0;
                var succeeded = true;
                var parentWindow = this.TryFindParent<Window>();

                if (parentWindow != null)
                {
                    try
                    {
                        var min1 = Math.Min(parentWindow.ActualWidth, parentWindow.ActualHeight);
                        var min2 = Math.Min(parentWindow.Width, parentWindow.Height);
                        var min = new double[] { min1, min2 }.Where(d => d > 0).Max();

                        double ratio = 0.0185;
                        if (parentWindow is PrintView) ratio = 0.015;

                        planetsFontSize = min * ratio;
                        lastCalculatedFontSize = planetsFontSize;
                    }
                    catch { succeeded = false; }
                }

                if ((!succeeded || planetsFontSize == 0) && e != null)
                {
                    try
                    {
                        planetsFontSize = Math.Min(e.NewSize.Width, e.NewSize.Height) * 0.04;
                        lastCalculatedFontSize = planetsFontSize;
                    }
                    catch { succeeded = false; }
                }

                if ((!succeeded || planetsFontSize == 0) && lastCalculatedFontSize.HasValue && lastCalculatedFontSize.Value > 0)
                {
                    planetsFontSize = lastCalculatedFontSize.Value;
                    succeeded = true;
                }

                if (!succeeded || planetsFontSize == 0)
                {
                    planetsFontSize = ChartFontSize;
                    lastCalculatedFontSize = planetsFontSize;
                }

                foreach (var g in grouped)
                {
                    switch (g.Key)
                    {
                        case Calculations.Supplementary.ClassicHouse.H1:
                            {
                                foreach (var gi in g.Reverse())
                                {
                                    var p = gi;

                                    if (Chart.Division != 1 && p != null && p.Id == Calculations.Supplementary.PointId.Asc) continue;

                                    var panel = new StackPanel { Margin = new Thickness(2) };

                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };

                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb1.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H2:
                            {
                                foreach (var gi in g.Reverse())
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb2.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H3:
                            {
                                foreach (var gi in g)
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb3.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H4:
                            {
                                foreach (var gi in g)
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb4.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H5:
                            {
                                foreach (var gi in g)
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb5.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H6:
                            {
                                foreach (var gi in g)
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb6.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H7:
                            {
                                foreach (var gi in g)
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb7.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H8:
                            {
                                foreach (var gi in g)
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb8.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H9:
                            {
                                foreach (var gi in g.Reverse())
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb9.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H10:
                            {
                                foreach (var gi in g.Reverse())
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb10.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H11:
                            {
                                foreach (var gi in g.Reverse())
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb11.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H12:
                            {
                                foreach (var gi in g.Reverse())
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb12.Children.Add(panel);
                                }
                            }
                            break;
                    }
                }
            }

            if (!HideSigns && Chart != null)
            {
                var sign = (int)Chart.Asc.Sign;

                var signsFontSize = ww * 1.5d / 100d;
                signsFontSize = signsFontSize < 10 ? 10 : signsFontSize;
                signsFontSize = signsFontSize > 16 ? 16 : signsFontSize;

                var width = ww * 3d / 100d;
                width = width < 14 ? 14 : width;
                width = width > 20 ? 20 : width;

                var height = hh * 3d / 100d;
                height = height < 14 ? 14 : height;
                height = height > 20 ? 20 : height;

                //H1
                var tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 2d - tb.Width / 2d);
                Canvas.SetTop(tb, hh / 2d - tb.Height / 2d - 30);
                canArea.Children.Add(tb);

                //H2
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 4d - tb.Width / 2d);
                Canvas.SetTop(tb, hh / 4d - tb.Height * 2d);
                canArea.Children.Add(tb);

                //H3
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 4d - tb.Width * 2d);
                Canvas.SetTop(tb, hh / 4d - tb.Height / 2d);
                canArea.Children.Add(tb);

                //H4
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 2d - tb.Width / 2d - 30);
                Canvas.SetTop(tb, hh / 2d - tb.Height / 2d);
                canArea.Children.Add(tb);

                //H5
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 4d - tb.Width * 2d);
                Canvas.SetTop(tb, hh * 3d / 4d - tb.Height / 2d);
                canArea.Children.Add(tb);

                //H6
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 4d - tb.Width / 2d);
                Canvas.SetTop(tb, hh * 3d / 4d + tb.Height);
                canArea.Children.Add(tb);

                //H7
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 2d - tb.Width / 2d);
                Canvas.SetTop(tb, hh / 2d - tb.Height / 2d + 30);
                canArea.Children.Add(tb);

                //H8
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww * 3d / 4d - tb.Width / 2d);
                Canvas.SetTop(tb, hh * 3d / 4d + tb.Height);
                canArea.Children.Add(tb);

                //H9
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww * 3d / 4d + tb.Width);
                Canvas.SetTop(tb, hh * 3d / 4d - tb.Height / 2d);
                canArea.Children.Add(tb);

                //H10
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 2d + tb.Width / 2d + 15);
                Canvas.SetTop(tb, hh / 2d - tb.Height / 2d);
                canArea.Children.Add(tb);

                //H11
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww * 3d / 4d + tb.Width);
                Canvas.SetTop(tb, hh / 4d - tb.Height / 2d);
                canArea.Children.Add(tb);

                //H12
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww * 3d / 4d - tb.Width / 2d);
                Canvas.SetTop(tb, hh / 4d - tb.Height * 2d);
                canArea.Children.Add(tb);
            }

            var buffer = new List<WrapPanel>();
            foreach (var c1 in canArea.Children)
            {
                var g = c1 as Grid;
                if (g == null) continue;

                foreach (var c2 in g.Children)
                {
                    var vbox = c2 as Viewbox;
                    if (vbox == null) continue;

                    var wp = vbox.Child as WrapPanel;

                    if (wp == null) continue;
                    if (wp.Children.Count == 0) continue;

                    buffer.Add(wp);
                }
            }

            foreach (WrapPanel wp in buffer)
            {
                //if (wp.Children.Count < 5) continue;

                //foreach (var c3 in wp.Children)
                //{
                //    var sp = c3 as StackPanel;
                //    if (sp == null) continue;
                //    //if (sp.Children.Count > 1) continue;

                //    foreach (var c4 in sp.Children)
                //    {
                //        var tb = c4 as TextBlock;
                //        if (tb == null) continue;

                //        tb.FontSize = tb.FontSize * 0.6;
                //    }
                //}

                if (wp.Children.Count < 3) continue;

                var extraRatioFactor = this.Chart.Division == 1 ? 1 : 0.85;

                var for3 = 0.9 * extraRatioFactor;
                var for4 = 0.8 * extraRatioFactor;
                var for5 = 0.7 * extraRatioFactor;
                var forRest = 0.6 * extraRatioFactor;

                foreach (var c3 in wp.Children)
                {
                    var sp = c3 as StackPanel;
                    if (sp == null) continue;

                    foreach (var c4 in sp.Children)
                    {
                        var tb = c4 as TextBlock;
                        if (tb == null) continue;

                        switch (wp.Children.Count)
                        {
                            case 3:
                                tb.FontSize = tb.FontSize * for3;
                                break;
                            case 4:
                                tb.FontSize = tb.FontSize * for4;
                                break;
                            case 5:
                                tb.FontSize = tb.FontSize * for5;
                                break;
                            default:
                                tb.FontSize = tb.FontSize * forRest;
                                break;
                        }
                    }
                }
            }

            canArea.UpdateLayout();
        }

        private static WrapPanel PrepWrapPanel(Grid g)
        {
            var wpb1 = CreateWrapPanel(g);
            PutInViewBox(g, wpb1);
            return wpb1;
        }

        private static void PutInViewBox(Grid g, WrapPanel wpb)
        {
            var globvb = new Viewbox
            {
                Stretch = Stretch.Uniform,
                StretchDirection = StretchDirection.DownOnly
            };
            globvb.Child = wpb;
            g.Children.Add(globvb);
        }

        private static WrapPanel CreateWrapPanel(Grid g)
        {
            var wpb = new WrapPanel
            {
                MaxWidth = g.Width,
                MaxHeight = g.Height,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            return wpb;
        }

        void DrawChart_Backup()
        {
            var ww = canArea.ActualWidth;
            var hh = canArea.ActualHeight;
            var ss = Math.Min(ww, hh);

            var xCenter = ww / 2d;
            var yCenter = hh / 2d;
            var wCenter = 15d;

            canArea.Children.Add(
                new Line
                {
                    X1 = xCenter - wCenter,
                    Y1 = yCenter - wCenter,
                    X2 = xCenter + wCenter,
                    Y2 = yCenter - wCenter,
                    Stroke = Brushes.Black
                });

            canArea.Children.Add(
                new Line
                {
                    X1 = xCenter + wCenter,
                    Y1 = yCenter - wCenter,
                    X2 = xCenter + wCenter,
                    Y2 = yCenter + wCenter,
                    Stroke = Brushes.Black
                });

            canArea.Children.Add(
                new Line
                {
                    X1 = xCenter + wCenter,
                    Y1 = yCenter + wCenter,
                    X2 = xCenter - wCenter,
                    Y2 = yCenter + wCenter,
                    Stroke = Brushes.Black
                });

            canArea.Children.Add(
                new Line
                {
                    X1 = xCenter - wCenter,
                    Y1 = yCenter + wCenter,
                    X2 = xCenter - wCenter,
                    Y2 = yCenter - wCenter,
                    Stroke = Brushes.Black
                });

            if (Chart != null)
            {
                var tb = new TextBlock
                {
                    Text = HideCenterChar ? string.Empty : Chart.Division.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = 20,
                    Height = 20,
                    FontSize = 14
                };
                Canvas.SetTop(tb, yCenter - wCenter + 5);
                Canvas.SetLeft(tb, xCenter - wCenter + 5);
                canArea.Children.Add(tb);
            }

            canArea.Children.Add(new Line
            {
                X1 = 0,
                Y1 = 0,
                X2 = xCenter - wCenter,
                Y2 = yCenter - wCenter,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww,
                Y1 = 0,
                X2 = xCenter + wCenter,
                Y2 = yCenter - wCenter,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww,
                Y1 = hh,
                X2 = xCenter + wCenter,
                Y2 = yCenter + wCenter,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = 0,
                Y1 = hh,
                X2 = xCenter - wCenter,
                Y2 = yCenter + wCenter,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = 0,
                Y1 = 0,
                X2 = ww,
                Y2 = 0,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww,
                Y1 = 0,
                X2 = ww,
                Y2 = hh,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = 0,
                Y1 = hh,
                X2 = ww,
                Y2 = hh,
                Stroke = Brushes.Black
            });
            canArea.Children.Add(new Line
            {
                X1 = 0,
                Y1 = hh,
                X2 = 0,
                Y2 = 0,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww / 2d,
                Y1 = 0,
                X2 = ww,
                Y2 = hh / 2d,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww / 2d,
                Y1 = hh,
                X2 = ww,
                Y2 = hh / 2d,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww / 2d,
                Y1 = hh,
                X2 = 0,
                Y2 = hh / 2d,
                Stroke = Brushes.Black
            });

            canArea.Children.Add(new Line
            {
                X1 = ww / 2d,
                Y1 = 0,
                X2 = 0,
                Y2 = hh / 2d,
                Stroke = Brushes.Black
            });

            var g1 = new Grid { Width = ww / 4d, Height = hh / 4d };
            var wpb1 = new WrapPanel { HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            g1.Children.Add(wpb1);
            Canvas.SetLeft(g1, ww * 3d / 8d);
            Canvas.SetTop(g1, hh / 8d);

            var g2 = new Grid { Width = ww / 4d, Height = hh / 8d };
            var wpb2 = new WrapPanel { HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            g2.Children.Add(wpb2);
            Canvas.SetLeft(g2, ww / 8d);
            Canvas.SetTop(g2, 0);

            var g3 = new Grid { Width = ww / 8d, Height = hh / 4d };
            var wpb3 = new WrapPanel { Orientation = System.Windows.Controls.Orientation.Vertical, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            g3.Children.Add(wpb3);
            Canvas.SetLeft(g3, 0);
            Canvas.SetTop(g3, hh / 8d);

            var g4 = new Grid { Width = ww / 4d, Height = hh / 4d };
            var wpb4 = new WrapPanel { Orientation = System.Windows.Controls.Orientation.Vertical, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            g4.Children.Add(wpb4);
            Canvas.SetLeft(g4, ww / 8d);
            Canvas.SetTop(g4, hh * 3d / 8d);

            var g5 = new Grid { Width = ww / 8d, Height = hh / 4d };
            var wpb5 = new WrapPanel { Orientation = System.Windows.Controls.Orientation.Vertical, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            g5.Children.Add(wpb5);
            Canvas.SetLeft(g5, 0);
            Canvas.SetTop(g5, hh * 5d / 8d);

            var g6 = new Grid { Width = ww / 4d, Height = hh / 8d };
            var wpb6 = new WrapPanel { HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            g6.Children.Add(wpb6);
            Canvas.SetLeft(g6, ww / 8d);
            Canvas.SetTop(g6, hh * 7d / 8d);

            var g7 = new Grid { Width = ww / 4d, Height = hh / 4d };
            var wpb7 = new WrapPanel { HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            g7.Children.Add(wpb7);
            Canvas.SetLeft(g7, ww * 3d / 8d);
            Canvas.SetTop(g7, hh * 5d / 8d);

            var g8 = new Grid { Width = ww / 4d, Height = hh / 8d };
            var wpb8 = new WrapPanel { HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            g8.Children.Add(wpb8);
            Canvas.SetLeft(g8, ww * 5d / 8d);
            Canvas.SetTop(g8, hh * 7d / 8d);

            var g9 = new Grid { Width = ww / 8d, Height = hh / 4d };
            var wpb9 = new WrapPanel { Orientation = System.Windows.Controls.Orientation.Vertical, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            g9.Children.Add(wpb9);
            Canvas.SetLeft(g9, ww * 7d / 8d);
            Canvas.SetTop(g9, hh * 5d / 8d);

            var g10 = new Grid { Width = ww / 4d, Height = hh / 4d };
            var wpb10 = new WrapPanel { Orientation = System.Windows.Controls.Orientation.Vertical, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            g10.Children.Add(wpb10);
            Canvas.SetLeft(g10, ww * 5d / 8d);
            Canvas.SetTop(g10, hh * 3d / 8d);

            var g11 = new Grid { Width = ww / 8d, Height = hh / 4d };
            var wpb11 = new WrapPanel { Orientation = System.Windows.Controls.Orientation.Vertical, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            g11.Children.Add(wpb11);
            Canvas.SetLeft(g11, ww * 7d / 8d);
            Canvas.SetTop(g11, hh / 8d);

            var g12 = new Grid { Width = ww / 4d, Height = hh / 8d };
            var wpb12 = new WrapPanel { HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            g12.Children.Add(wpb12);
            Canvas.SetLeft(g12, ww * 5d / 8d);
            Canvas.SetTop(g12, 0);

            canArea.Children.Add(g1);
            canArea.Children.Add(g2);
            canArea.Children.Add(g3);
            canArea.Children.Add(g4);
            canArea.Children.Add(g5);
            canArea.Children.Add(g6);
            canArea.Children.Add(g7);
            canArea.Children.Add(g8);
            canArea.Children.Add(g9);
            canArea.Children.Add(g10);
            canArea.Children.Add(g11);
            canArea.Children.Add(g12);

            if (Chart != null)
            {
                var grouped = (from p in Chart.Points
                               orderby p.Degree
                               group p by p.ClassicHouse into h
                               select h).ToList();

                var asc = Chart.Asc;

                // V 1
                //var planetsFontSize = ww * 2d / 100d;
                //planetsFontSize = planetsFontSize < 12 ? 12 : planetsFontSize;
                //planetsFontSize = planetsFontSize > 12 ? 12 : planetsFontSize;
                //if (ShowDegree) planetsFontSize = planetsFontSize > 12 ? 12 : planetsFontSize;

                // V 2
                //var planetsFontSize = 10.5d;

                // V 3
                var planetsFontSize = ChartFontSize;

                foreach (var g in grouped)
                {
                    switch (g.Key)
                    {
                        case Calculations.Supplementary.ClassicHouse.H1:
                            {
                                foreach (var gi in g.Reverse())
                                {
                                    var p = gi;

                                    if (Chart.Division != 1 && p != null && p.Id == Calculations.Supplementary.PointId.Asc) continue;

                                    var panel = new StackPanel { Margin = new Thickness(2) };

                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };

                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb1.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H2:
                            {
                                foreach (var gi in g.Reverse())
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb2.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H3:
                            {
                                foreach (var gi in g)
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb3.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H4:
                            {
                                foreach (var gi in g)
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb4.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H5:
                            {
                                foreach (var gi in g)
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb5.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H6:
                            {
                                foreach (var gi in g)
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb6.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H7:
                            {
                                foreach (var gi in g)
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb7.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H8:
                            {
                                foreach (var gi in g)
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb8.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H9:
                            {
                                foreach (var gi in g.Reverse())
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb9.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H10:
                            {
                                foreach (var gi in g.Reverse())
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb10.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H11:
                            {
                                foreach (var gi in g.Reverse())
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb11.Children.Add(panel);
                                }
                            }
                            break;
                        case Calculations.Supplementary.ClassicHouse.H12:
                            {
                                foreach (var gi in g.Reverse())
                                {
                                    var p = gi;

                                    var panel = new StackPanel { Margin = new Thickness(2) };
                                    var tb = new TextBlock
                                    {
                                        Text = p.Id.ToString(),
                                        TextAlignment = TextAlignment.Center,
                                        Foreground = Brushes.Black,
                                        Padding = new Thickness(0),
                                        FontSize = planetsFontSize
                                    };
                                    panel.Children.Add(tb);
                                    if (ShowDegree)
                                    {
                                        tb.FontWeight = FontWeights.Bold;
                                        tb.FontSize = planetsFontSize * 1.2;
                                        tb.Text += (p == null ? "" : (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0 ? "-R" : ""));

                                        if (p != null && asc != null)
                                        {
                                            var isNightly = IsNightly(asc, p);

                                            if (!isNightly) tb.Foreground = new SolidColorBrush(Color.FromRgb(0x04, 0x53, 0x9A));
                                        }

                                        var tbDegree = new TextBlock
                                        {
                                            Text = ShowAsDegree(gi.Degree),
                                            TextAlignment = TextAlignment.Center,
                                            Foreground = Brushes.Black,
                                            Padding = new Thickness(0),
                                            FontSize = planetsFontSize * 0.9
                                        };

                                        panel.Children.Add(tbDegree);
                                    }

                                    wpb12.Children.Add(panel);
                                }
                            }
                            break;
                    }
                }
            }

            if (!HideSigns && Chart != null)
            {
                var sign = (int)Chart.Asc.Sign;

                var signsFontSize = ww * 1.5d / 100d;
                signsFontSize = signsFontSize < 10 ? 10 : signsFontSize;
                signsFontSize = signsFontSize > 16 ? 16 : signsFontSize;

                var width = ww * 3d / 100d;
                width = width < 14 ? 14 : width;
                width = width > 20 ? 20 : width;

                var height = hh * 3d / 100d;
                height = height < 14 ? 14 : height;
                height = height > 20 ? 20 : height;

                //H1
                var tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 2d - tb.Width / 2d);
                Canvas.SetTop(tb, hh / 2d - tb.Height / 2d - 30);
                canArea.Children.Add(tb);

                //H2
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 4d - tb.Width / 2d);
                Canvas.SetTop(tb, hh / 4d - tb.Height * 2d);
                canArea.Children.Add(tb);

                //H3
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 4d - tb.Width * 2d);
                Canvas.SetTop(tb, hh / 4d - tb.Height / 2d);
                canArea.Children.Add(tb);

                //H4
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 2d - tb.Width / 2d - 30);
                Canvas.SetTop(tb, hh / 2d - tb.Height / 2d);
                canArea.Children.Add(tb);

                //H5
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 4d - tb.Width * 2d);
                Canvas.SetTop(tb, hh * 3d / 4d - tb.Height / 2d);
                canArea.Children.Add(tb);

                //H6
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 4d - tb.Width / 2d);
                Canvas.SetTop(tb, hh * 3d / 4d + tb.Height);
                canArea.Children.Add(tb);

                //H7
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 2d - tb.Width / 2d);
                Canvas.SetTop(tb, hh / 2d - tb.Height / 2d + 30);
                canArea.Children.Add(tb);

                //H8
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww * 3d / 4d - tb.Width / 2d);
                Canvas.SetTop(tb, hh * 3d / 4d + tb.Height);
                canArea.Children.Add(tb);

                //H9
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww * 3d / 4d + tb.Width);
                Canvas.SetTop(tb, hh * 3d / 4d - tb.Height / 2d);
                canArea.Children.Add(tb);

                //H10
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww / 2d + tb.Width / 2d + 15);
                Canvas.SetTop(tb, hh / 2d - tb.Height / 2d);
                canArea.Children.Add(tb);

                //H11
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww * 3d / 4d + tb.Width);
                Canvas.SetTop(tb, hh / 4d - tb.Height / 2d);
                canArea.Children.Add(tb);

                //H12
                sign = Range12(sign + 1);
                tb = new TextBlock
                {
                    Text = sign.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = width,
                    Height = height,
                    FontSize = signsFontSize
                };
                Canvas.SetLeft(tb, ww * 3d / 4d - tb.Width / 2d);
                Canvas.SetTop(tb, hh / 4d - tb.Height * 2d);
                canArea.Children.Add(tb);
            }
        }

        static bool IsNightly(Calculations.Supplementary.VdPoint asc, Calculations.Supplementary.VdPoint p)
        {
            if (asc.Id == p.Id) return true;

            bool isNightly = false;

            var pLen = p.Longitude;
            var ascLen = asc.Longitude;

            if (pLen < ascLen) pLen += 360;

            if (ascLen < pLen && pLen < ascLen + 180) isNightly = true;
            else isNightly = false;

            return isNightly;
        }

        int Range12(int h)
        {
            return h.Range(1, 13);
        }

        string ShowAsDegree(double d)
        {
            var ts = TimeSpan.FromHours(d);

            return string.Format("{0:00}:{1:00}", Math.Floor(ts.TotalHours), ts.Minutes);
        }

        string ShowDirection(Calculations.Supplementary.VdPoint p)
        {
            if (p.Id != Calculations.Supplementary.PointId.Ra && p.Id != Calculations.Supplementary.PointId.Ke && p.SpeedInLongitude < 0) return " R";

            return string.Empty;
        }

        #region HideCenterChar
        public bool HideCenterChar
        {
            get { return (bool)GetValue(HideCenterCharProperty); }
            set { SetValue(HideCenterCharProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HideCenterChar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HideCenterCharProperty =
            DependencyProperty.Register("HideCenterChar", typeof(bool), typeof(ChartPartial));
        #endregion

        #region HideSigns
        public bool HideSigns
        {
            get { return (bool)GetValue(HideSignsProperty); }
            set { SetValue(HideSignsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HideSigns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HideSignsProperty =
            DependencyProperty.Register("HideSigns", typeof(bool), typeof(ChartPartial));
        #endregion

        #region ChartBackground
        public Brush ChartBackground
        {
            get { return (Brush)GetValue(ChartBackgroundProperty); }
            set { SetValue(ChartBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChartBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChartBackgroundProperty =
            DependencyProperty.Register("ChartBackground", typeof(Brush), typeof(ChartPartial));
        #endregion

        #region Chart
        public SevenAstro2.Calculations.Supplementary.VdChart Chart
        {
            get { return (SevenAstro2.Calculations.Supplementary.VdChart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Chart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChartProperty =
            DependencyProperty.Register("Chart", typeof(SevenAstro2.Calculations.Supplementary.VdChart), typeof(ChartPartial), new FrameworkPropertyMetadata(Changed));
        #endregion

        #region ChartFontSize
        public double ChartFontSize
        {
            get { return (double)GetValue(ChartFontSizeProperty); }
            set { SetValue(ChartFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChartFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChartFontSizeProperty =
            DependencyProperty.Register("ChartFontSize", typeof(double), typeof(ChartPartial));
        #endregion

        #region ShowDegree
        public bool ShowDegree
        {
            get { return (bool)GetValue(ShowDegreeProperty); }
            set { SetValue(ShowDegreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowDegree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowDegreeProperty =
            DependencyProperty.Register("ShowDegree", typeof(bool), typeof(ChartPartial));
        #endregion

        static void Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = dependencyObject as ChartPartial;
            ctrl.ClearAndDrawChart(null, null);
        }

        #region //
        //protected override Size MeasureOverride(Size availableSize)
        //{
        //    //this.Width = availableSize.Width;
        //    //this.Height = availableSize.Width;

        //    //gSingle.Measure(new Size(availableSize.Width, availableSize.Width));

        //    //ClearAndDrawChart();

        //    //return new Size(availableSize.Width, availableSize.Width);

        //    return availableSize;
        //} 
        #endregion
    }
}

using SevenAstro2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
    /// Interaction logic for PrintView.xaml
    /// </summary>
    internal partial class PrintView : Window
    {
        public PrintView(CircumstanceViewModel e)
        {
            this.DataContext = e;

            InitializeComponent();

            this.placeholder.Text = string.Format("7astro @ {0:yyyy-MM-dd HH:mm}", DateTime.Now);

            var cir = this.DataContext as CircumstanceViewModel;

            prati = cir.CalculateDasas(3);
            maha = cir.CalculateDasas(1);

            var startAt = DateTime.Now.AddMonths(-4);
            var q = (from d in prati
                     let year = int.Parse(d.Date.Substring(6))
                     where year >= startAt.Year
                     select d).Take(40).ToList();
            this.pratiDasas.ItemsSource = q;

            this.mahaDasas.ItemsSource = maha;

            this.UpdateLayout();
        }

        System.Collections.ObjectModel.ObservableCollection<Dasa> prati;
        System.Collections.ObjectModel.ObservableCollection<Dasa> maha;

        private void btnPrint_Click(object sender, RoutedEventArgs ea)
        {
            var pd = new PrintDialog();
            if (pd.ShowDialog() == false) return;

            var v = this.printArea;

            var e = v as FrameworkElement;
            if (e == null) return;

            PrintCapabilities cap;
            var printableArea = GetPrintableArea(pd, out cap);

            var scale = Math.Min(cap.PageImageableArea.ExtentWidth / e.ActualWidth, cap.PageImageableArea.ExtentHeight / e.ActualHeight);
            e.LayoutTransform = new ScaleTransform(scale, scale);

            var size = new Size(cap.PageImageableArea.ExtentWidth, cap.PageImageableArea.ExtentHeight);

            e.Measure(size);
            e.Arrange(new Rect(new System.Windows.Point(cap.PageImageableArea.OriginWidth, cap.PageImageableArea.OriginHeight), size));

            pd.PrintVisual(e, "By 7astro");

            this.Close();
        }

        private static Rect GetPrintableArea(PrintDialog printDialog, out PrintCapabilities cap)
        {
            cap = null;
            try
            {
                cap = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
            }
            catch (PrintQueueException)
            {
                return Rect.Empty;
            }

            if (cap.PageImageableArea == null)
            {
                return Rect.Empty;
            }

            var leftMargin = cap.OrientedPageMediaWidth.HasValue ? (cap.OrientedPageMediaWidth.Value - cap.PageImageableArea.ExtentWidth) / 2 : 0;
            var topMargin = cap.OrientedPageMediaHeight.HasValue ? (cap.OrientedPageMediaHeight.Value - cap.PageImageableArea.ExtentHeight) / 2 : 0;
            var width = cap.PageImageableArea.ExtentWidth;
            var height = cap.PageImageableArea.ExtentHeight;

            return new Rect(leftMargin, topMargin, width, height);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
    }
}

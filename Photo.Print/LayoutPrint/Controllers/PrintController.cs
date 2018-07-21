using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Photo.Print.Layout.Base;

namespace Photo.Print.LayoutPrint.Controllers
{
    class PrintController : Base.ILayoutPrint
    {
        private readonly IWin32Window parentWindow;

        public PrintController(IWin32Window parentWindow)
        {
            this.parentWindow = parentWindow;
        }

        class PrintItem : IDisposable
        {
            public Area Area { get; }

            public Image Image { get; }

            public PrintItem(Area area)
            {
                this.Area = area;
                try
                {
                    this.Image = Bitmap.FromFile(area.Photo.Value.FileName);
                }
                catch
                {
                }
            }

            public void Dispose()
            {
                this.Image?.Dispose();
            }
        }

        class LayoutDocument : PrintDocument
        {
            private readonly Layout.Base.Layout layout;

            readonly List<PrintItem> printItems = new List<PrintItem>();

            public LayoutDocument(Layout.Base.Layout layout)
            {
                this.layout = layout;
                foreach (var i in layout.Items.Where(q => q.Photo != null))
                    printItems.Add(new PrintItem(i));
            }

            protected override void Dispose(bool disposing)
            {
                foreach (var pi in printItems)
                    pi.Dispose();
                printItems.Clear();

                base.Dispose(disposing);
            }

            protected override void OnPrintPage(PrintPageEventArgs e)
            {
                foreach (var pi in printItems.Where(q => q.Image != null))
                {
                    var l = e.MarginBounds.Left + (pi.Area.Left) * e.MarginBounds.Width;                    
                    var t = e.MarginBounds.Top + (pi.Area.Top) * e.MarginBounds.Height;

                    var border_wh = System.Math.Min(e.MarginBounds.Width, e.MarginBounds.Height);

                    l += pi.Area.Border * border_wh;
                    t += pi.Area.Border * border_wh;

                    var tw = (pi.Area.Width) * e.MarginBounds.Width;
                    var th = (pi.Area.Height) * e.MarginBounds.Height;

                    tw -= 2.0 * pi.Area.Border * border_wh;
                    th -= 2.0 * pi.Area.Border * border_wh;

                    var tc_x = l + tw / 2;
                    var tc_y = t + th / 2;

                    var sw_source = 100f * (float)pi.Image.Width / (pi.Image.HorizontalResolution);
                    var sh_source = 100f * (float)pi.Image.Height / (pi.Image.VerticalResolution);
                    var dm = new Matrix();
                    dm.Rotate(pi.Area.Angle);
                    var corners = new[] { new PointF(0, 0), new PointF(sw_source, 0), new PointF(sw_source, sh_source), new PointF(0, sh_source) };
                    dm.TransformPoints(corners);
                    var sx_max = corners.Select(q => q.X).Max();
                    var sx_min = corners.Select(q => q.X).Min();

                    var sy_max = corners.Select(q => q.Y).Max();
                    var sy_min = corners.Select(q => q.Y).Min();

                    var scale_x = tw/(sx_max - sx_min);
                    var scale_y = th/(sy_max - sy_min);

                    double scale = 1;
                    switch(pi.Area.FitType)
                    {
                        case FitType.Fit:
                            scale = System.Math.Min(scale_x, scale_y);
                            break;
                        case FitType.Crop:
                            scale = System.Math.Max(scale_x, scale_y);
                            break;
                    }

                    using (var m2d = new Matrix())
                    {
                        m2d.Translate(-sw_source / 2f, -sh_source / 2f, MatrixOrder.Append);
                        m2d.Rotate(pi.Area.Angle, MatrixOrder.Append);
                        m2d.Scale((float)scale, (float)scale, MatrixOrder.Append);
                        m2d.Translate((float)tc_x, (float)tc_y, MatrixOrder.Append);

                        using (var invert = m2d.Clone())
                        {
                            invert.Invert();

                            var cont = e.Graphics.BeginContainer();
                            e.Graphics.Transform = m2d;
                            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                            var targClipPoints = new[] { new PointF((float)l, (float)t), new PointF((float)l + (float)tw, (float)t), new PointF((float)l + (float)tw, (float)t + (float)th), new PointF((float)l, (float)t + (float)th) };

                            invert.TransformPoints(targClipPoints);
                            using (var gp = new GraphicsPath())
                            {
                                gp.AddPolygon(targClipPoints);
                                e.Graphics.SetClip(gp);
                                e.Graphics.DrawImage(pi.Image, 0, 0);
                                e.Graphics.ResetClip();                                
                            }

                            e.Graphics.EndContainer(cont);
                        }
                    }
                }
            }
        }

        public void Print(Layout.Base.Layout layout)
        {
            using (var print = new LayoutDocument(layout))
            {
                print.DefaultPageSettings.Landscape = true;
                SetupPageSettings(print.DefaultPageSettings);
                //print.QueryPageSettings += new QueryPageSettingsEventHandler(print_QueryPageSettings);                

                using (var pd = new PrintDialog() { UseEXDialog = true })
                {
                    pd.Document = print;
                    if (pd.ShowDialog(parentWindow) == DialogResult.OK)
                    {
                        using (PageSetupDialog psd = new PageSetupDialog())
                        {
                            psd.Document = print;                            
                            if (psd.ShowDialog(parentWindow) == DialogResult.OK)
                            {
                                //using (PrintPreviewDialog prev = new PrintPreviewDialog())
                                //{
                                //    prev.Document = print;
                                //    prev.ShowDialog(parentWindow);
                                //}
                                print.Print();
                            }
                        }
                    }
                }
            }
        }

        private void SetupPageSettings(PageSettings pageSettings)
        {   
            pageSettings.Margins = new Margins(0, 0, 0, 0);
            foreach (var r in pageSettings.PrinterSettings.PrinterResolutions.OfType<PrinterResolution>())
                if (r.Kind == PrinterResolutionKind.High)
                    pageSettings.PrinterResolution = r;
        }

        //private void print_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        //{
        //    SetupPageSettings(e.PageSettings);
        //}
    }
}

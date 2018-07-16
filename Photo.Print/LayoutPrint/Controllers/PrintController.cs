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
                this.Image = Bitmap.FromFile(area.Photo.Value.FileName);
            }

            public void Dispose()
            {
                this.Image.Dispose();
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

            protected override void OnPrintPage(PrintPageEventArgs e)
            {
                e.Graphics.FillRectangle(Brushes.Yellow, e.MarginBounds);
                e.Graphics.FillRectangle(Brushes.Red, new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width / 2, e.MarginBounds.Height));

                foreach (var pi in printItems)
                {
                    var l = e.MarginBounds.Left + (pi.Area.Left) * e.MarginBounds.Width;                    
                    var t = e.MarginBounds.Top + (pi.Area.Top) * e.MarginBounds.Height;

                    var tw = (pi.Area.Width) * e.MarginBounds.Width;
                    var th = (pi.Area.Height) * e.MarginBounds.Height;

                    var tc_x = l + tw / 2;
                    var tc_y = t + th / 2;

                    var sw = (float)pi.Image.Width;
                    var sh = (float)pi.Image.Height;

                    var scale_x = tw/sw;
                    var scale_y = th/sh;

                    var scale = System.Math.Max(scale_x, scale_y);
                    using (var m2d = new Matrix())
                    {
                        m2d.Translate(-sw / 2, -sh / 2, MatrixOrder.Append);
                        m2d.Scale(scale, scale, MatrixOrder.Append);
                        m2d.Translate(tc_x, tc_y, MatrixOrder.Append);

                        using (var invert = m2d.Clone())
                        {
                            invert.Invert();

                            var cont = e.Graphics.BeginContainer();
                            e.Graphics.Transform = m2d;
                            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                            var targClipPoints = new[] { new PointF(l, t), new PointF(l + tw, t), new PointF(l + tw, t + th), new PointF(l, t + th) };

                            invert.TransformPoints(targClipPoints);
                            using (var gp = new GraphicsPath())
                            {
                                gp.AddPolygon(targClipPoints);
                                e.Graphics.SetClip(gp);
                                e.Graphics.DrawImage(pi.Image, 0, 0);
                                e.Graphics.ResetClip();
                                e.Graphics.EndContainer(cont);
                            }
                        }
                    }
                }
            }
        }

        public void Print(Layout.Base.Layout layout)
        {
            using (var print = new LayoutDocument(layout))
            {                
                print.QueryPageSettings += new QueryPageSettingsEventHandler(print_QueryPageSettings);                

                using (var pd = new PrintDialog() { UseEXDialog = true })
                {
                    pd.Document = print;
                    if (pd.ShowDialog() == DialogResult.OK)
                    {
                        using (PageSetupDialog psd = new PageSetupDialog())
                        {
                            psd.Document = print;
                            SetupPageSettings(psd.PageSettings);
                            if (psd.ShowDialog() == DialogResult.OK)
                            {
                                using (PrintPreviewDialog prev = new PrintPreviewDialog())
                                {
                                    prev.Document = print;
                                    prev.ShowDialog();
                                }
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

        private void print_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            SetupPageSettings(e.PageSettings);
        }

        
    }
}

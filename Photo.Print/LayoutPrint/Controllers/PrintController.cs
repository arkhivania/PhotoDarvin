using System;
using System.Collections.Generic;
using System.Drawing;
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

        class LayoutDocument : PrintDocument
        {
            private readonly Layout.Base.Layout layout;

            public LayoutDocument(Layout.Base.Layout layout)
            {
                this.layout = layout;
            }

            protected override void OnPrintPage(PrintPageEventArgs e)
            {
                e.Graphics.FillRectangle(Brushes.Yellow, e.MarginBounds);
                e.Graphics.FillRectangle(Brushes.Red, new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width / 2, e.MarginBounds.Height));
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
        }

        private void print_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            SetupPageSettings(e.PageSettings);
        }

        
    }
}

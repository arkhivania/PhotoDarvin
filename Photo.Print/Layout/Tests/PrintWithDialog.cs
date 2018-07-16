using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Photo.Print.Layout.Tests
{
    [TestFixture]
    class PrintWithDialog
    {
        [Test, RequiresThread(System.Threading.ApartmentState.STA)]
        public void Show()
        {
            using (PrintDocument print = new PrintDocument())
            {
                print.PrintPage += new PrintPageEventHandler(print_PrintPage);
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
            
        }

        private void print_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            SetupPageSettings(e.PageSettings);
        }

        private void print_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Yellow, e.MarginBounds);
            e.Graphics.FillRectangle(Brushes.Red, new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width / 2, e.MarginBounds.Height));
        }
    }
}

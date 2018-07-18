using Ninject;
using NUnit.Framework;
using Photo.Print.Layout.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Photo.Print.LayoutPrint.Tests
{
    [TestFixture]
    class PrintLayout
    {
        [Test, RequiresThread(ApartmentState.STA)]
        public void PrintSample()
        {
            using (var kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false }))
            {
                using (var frm = new Form())
                {
                    frm.CreateControl();

                    kernel.Load<Module>();
                    kernel.Bind<IWin32Window>().ToConstant(frm);

                    var layout = new Layout.Base.Layout();
                    layout.Items = new[] 
                    {
                        new Area
                        {
                            Width = 1f, Height = 0.5f,
                            Left = 0, Top = 0,
                            Photo = new Layout.Base.Photo { FileName = @"C:\temp\Print\DSC_0313.JPG" }
                        },

                        new Area
                        {
                            Width = 1f, Height = 0.5f,
                            Left = 0f, Top = 0.5f,
                            Photo = new Layout.Base.Photo { FileName = @"C:\temp\Print\DSC_0319.JPG" }
                        },
                    };

                    kernel.Get<Base.ILayoutPrint>().Print(layout);
                }
            }
        }
    }
}

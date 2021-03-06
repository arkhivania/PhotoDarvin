﻿using Ninject;
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
                            Width = 0.5f, Height = 1,
                            Left = 0, Top = 0,
                            Photo = new Layout.Base.Photo { FileName = @"C:\temp\Print\DSC_0333.JPG" },
                            Angle = -90,
                            FitType = FitType.Crop
                        },

                        new Area
                        {
                            Width = 0.5f, Height = 1f,
                            Left = 0.5f, Top = 0,
                            Photo = new Layout.Base.Photo { FileName = @"C:\temp\Print\DSC_0344.JPG" },
                            Angle = -90,
                            FitType = FitType.Crop
                        },
                    };

                    kernel.Get<Base.ILayoutPrint>().Print(layout);
                }
            }
        }
    }
}

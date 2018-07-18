using Nailhang.Core;
using Ninject;
using NUnit.Framework;
using Photo.PrintTool.AreaLayouts.Base;
using Photo.PrintTool.PhotoLayout.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Photo.PrintTool.AreaLayouts.Tests
{
    [TestFixture]
    class DIsplay
    {
        [Test, RequiresThread(System.Threading.ApartmentState.STA)]
        public void Show()
        {
            using (var kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false }))
            {
                kernel.Load<AreaLayouts.Module>();
                kernel.Load<PhotoItemTools.Module>();
                kernel.Load<PhotoLayout.Module>();

                var areas = kernel.Get<Base.IAreaLayouts>();
                areas.LayoutState.Value = new Base.Layout
                {
                    Areas = new Area[] 
                    {
                        new Area { Width = 0.5f, Height = 0.5f, Left = 0.0f, Top = 0.0f, Id = "0" },
                        new Area { Width = 0.5f, Height = 0.5f, Left = 0.5f, Top = 0.0f, Id = "1" },
                        new Area { Width = 0.5f, Height = 0.5f, Left = 0.0f, Top = 0.5f, Id = "2" },
                        new Area { Width = 0.5f, Height = 0.5f, Left = 0.5f, Top = 0.5f, Id = "3" },
                    }
                };

                var pb = kernel.Get<IPhotoBag>();
                pb.Items.Add(new PhotoItem { AreaID = "0", FileName = @"C:\temp\Print\DSC_0344.JPG" });
                pb.Items.Add(new PhotoItem { AreaID = "1", FileName = @"C:\temp\Print\DSC_0333.JPG" });

                var window = new Window();
                window.Content = kernel.Get<Base.IToolViewFactory>().CreateView();
                window.ShowDialog();
                Dispatcher.CurrentDispatcher.InvokeShutdown();
            }
        }
    }
}

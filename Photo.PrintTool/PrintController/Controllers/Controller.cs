using Microsoft.Practices.Prism.Commands;
using Photo.Print.LayoutPrint.Base;
using Photo.PrintTool.AreaLayouts.Base;
using Photo.PrintTool.PhotoLayout.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.PrintTool.PrintController.Controllers
{
    class Controller : IDisposable
    {
        private readonly ILayoutPrint layoutPrint;
        private readonly IPhotoBag photoBag;
        private readonly IAreaLayouts areaLayouts;

        public DelegateCommand PrintCommand { get; }

        public Controller(ILayoutPrint layoutPrint, IPhotoBag photoBag, IAreaLayouts areaLayouts)
        {            
            this.layoutPrint = layoutPrint;
            this.photoBag = photoBag;
            this.areaLayouts = areaLayouts;

            PrintCommand = new DelegateCommand(() => PrintPage(), () => photoBag.Items.Any());
            photoBag.Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PrintCommand.RaiseCanExecuteChanged();
        }

        private void PrintPage()
        {
            var sl = areaLayouts.LayoutState.Value;
            var print_areas = new List<Print.Layout.Base.Area>();
            foreach(var sa in sl.Areas)
            {
                var pi = photoBag.Items.FirstOrDefault(q => q.AreaID == sa.Id);
                if(pi != null)
                {
                    print_areas.Add(new Print.Layout.Base.Area()
                    {
                        Angle = pi.Angle.Value,
                        FitType = pi.FitType.Value,
                        Width = sa.Width,
                        Height = sa.Height,
                        Left = sa.Left,
                        Top = sa.Top,
                        Photo = new Print.Layout.Base.Photo { FileName = pi.FileName }
                    });
                }
            }

            layoutPrint.Print(new Print.Layout.Base.Layout { Items = print_areas.ToArray() });
        }

        public void Dispose()
        {
            photoBag.Items.CollectionChanged -= Items_CollectionChanged;
        }
    }
}

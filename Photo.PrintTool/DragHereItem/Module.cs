using Nailhang.Core;
using Ninject.Modules;
using Photo.PrintTool.AreaLayouts.Base;
using Photo.PrintTool.PhotoLayout.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Photo.PrintTool.DragHereItem
{
    public class Module : NinjectModule, IPhotoItemArrange
    {
        public IEnumerable<IDisposable> Arrange(PhotoItem item, Grid grid)
        {
            if (item != null)
                yield break;

            var drag_photo_here = new Grid();
            var rectangle = new Rectangle
            {
                Fill = Brushes.LightGray,
                Margin = new System.Windows.Thickness(5),
                RadiusX = 5,
                RadiusY = 5
            };
            drag_photo_here.Children.Add(rectangle);

            var tb = new TextBlock { Text = "Drag photo file here", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
            drag_photo_here.Children.Add(tb);

            grid.Children.Add(drag_photo_here);
            yield return new ActionThroughDispose(() => grid.Children.Remove(drag_photo_here));
        }        

        public override void Load()
        {
            Kernel.Bind<IPhotoItemArrange>()
                .ToConstant(this);
        }
    }
}

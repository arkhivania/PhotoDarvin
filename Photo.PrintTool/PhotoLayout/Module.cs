using Nailhang.Core;
using Ninject;
using Ninject.Modules;
using Ninject.Parameters;
using Photo.PrintTool.AreaLayouts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Photo.PrintTool.PhotoLayout
{
    public class Module : NinjectModule, IAreaArrange
    {
        public IEnumerable<IDisposable> ArrangeArea(Grid grid, Area area)
        {
            var item = Kernel.Get<ViewItems.PhotoViewItem>(new ConstructorArgument("area", area));
            grid.Children.Add(item);
            yield return new ActionThroughDispose(() => grid.Children.Remove(item));
            yield return item;
        }

        public override void Load()
        {
            Kernel.Bind<Base.IPhotoBag>()
                .To<Processing.PhotoBag>()
                .InSingletonScope();
            Kernel.Bind<IAreaArrange>()
                .ToConstant(this);
        }
    }
}

using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Photo.PrintTool.AreaLayouts
{
    public class Module : NinjectModule, Base.IToolViewFactory
    {
        public UserControl CreateView()
        {
            return Kernel.Get<Views.LayoutView>();
        }

        public override void Load()
        {
            Kernel.Bind<Base.IAreaLayouts>()
                .To<Controllers.AreaLayouts>()
                .InSingletonScope();

            Kernel.Bind<Base.IToolViewFactory>()
                .ToConstant(this);
        }
    }
}

using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Print.LayoutPrint
{
    public class Module : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<Base.ILayoutPrint>()
                .To<Controllers.PrintController>()
                .InSingletonScope();
        }
    }
}

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.SourceExplorer.Explorer
{
    public class Module : NinjectModule, IModule
    {
        public override void Load()
        {
            Kernel.Bind<Base.OperatingState>().ToSelf().InSingletonScope();
            Kernel.Bind<ViewModel.ExplorerViewModel>().ToSelf().InSingletonScope();
        }        

        public void Initialize()
        {
            Kernel.Get<IRegionManager>().AddToRegion("SourceExplorer", Kernel.Get<View.ExplorerView>());
        }
    }
}

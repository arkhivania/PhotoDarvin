using Ninject.Modules;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace Photo.FolderStore.Tool
{
    public class Module : NinjectModule, IModule
    {

        public override void Load()
        {
            Kernel.Bind<ViewModel.TargetFolderViewModel>().ToSelf().InSingletonScope();
        }

        public void Initialize()
        {
            Kernel.Get<IRegionManager>().AddToRegion("TopPanel", Kernel.Get<View.TargetFolderView>());
        }
    }
}

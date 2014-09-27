using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Ninject.Modules;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.SourceExplorer.PhotosList
{
    public class Module : NinjectModule, IModule
    {
        public void Initialize()
        {
            Kernel.Get<IRegionManager>().AddToRegion("PhotosList", Kernel.Get<View.PhotosList>());
        }

        public override void Load()
        {
            Kernel.Bind<ViewModel.PhotosListViewModel>()
                .ToSelf()
                .InSingletonScope();
        }
    }
}

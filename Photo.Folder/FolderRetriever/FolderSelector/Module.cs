using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photo.Folder.FolderRetriever.FolderSelector
{
    class Module : NinjectModule, IModule
    {
        public override void Load()
        {
            Kernel.Bind<Base.OperatingState>().ToSelf().InSingletonScope();
            Kernel.Bind<ViewModel.SelectorViewModel>().ToSelf().InSingletonScope();
            Kernel.Bind<Photo.Base.IPhotoSource>()
                .ToMethod(w => Kernel.Get<Retriever>())
                .WhenInjectedInto<ViewModel.SelectorViewModel>();
        }

        public void Initialize()
        {
            Kernel.Get<IRegionManager>().AddToRegion("TopPanel", Kernel.Get<Views.SelectorView>());
        }
    }
}

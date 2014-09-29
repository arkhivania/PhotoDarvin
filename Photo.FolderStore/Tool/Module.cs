using System.Windows.Input;
using Ninject.Modules;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using OSWrap;

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
            Kernel.Get<ICommandRegistry>().RegisterCommand(Kernel.Get<ViewModel.TargetFolderViewModel>().PutToFolderCommand, new KeyGesture(Key.S, ModifierKeys.Control));
        }
    }
}

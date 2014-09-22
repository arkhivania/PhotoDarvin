using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.SourceExplorer.Explorer
{
    public class Module : NinjectModule, Base.IExplorerViewFactory
    {
        public override void Load()
        {
            Kernel.Bind<Base.OperatingState>().ToSelf().InSingletonScope();
            Kernel.Bind<ViewModel.ExplorerViewModel>().ToSelf().InSingletonScope();
            
            Kernel.Bind<Base.IExplorerViewFactory>().ToConstant(this);
        }

        public System.Windows.Controls.UserControl CreateExplorer()
        {
            return Kernel.Get<View.ExplorerView>();
        }
    }
}

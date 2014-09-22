using Ninject.Modules;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.FolderStore.Tool
{
    public class Module : NinjectModule, Photo.Base.ISourceViewExtension
    {
        public override void Load()
        {
            Kernel.Bind<Photo.Base.ISourceViewExtension>().ToConstant(this);
            Kernel.Bind<ViewModel.TargetFolderViewModel>().ToSelf().InSingletonScope();
        }

        public System.Windows.Controls.UserControl CreateSourceView()
        {
            return Kernel.Get<View.TargetFolderView>();
        }
    }
}

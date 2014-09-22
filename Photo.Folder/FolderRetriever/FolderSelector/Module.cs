using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photo.Folder.FolderRetriever.FolderSelector
{
    class Module : NinjectModule, Photo.Base.ISourceViewExtension
    {
        public override void Load()
        {
            Kernel.Bind<Base.OperatingState>().ToSelf().InSingletonScope();
            Kernel.Bind<ViewModel.SelectorViewModel>().ToSelf().InSingletonScope();
            Kernel.Bind<Photo.Base.ISourceViewExtension>().ToConstant(this);
            Kernel.Bind<Photo.Base.IPhotoSource>()
                .ToMethod(w => Kernel.Get<Retriever>())
                .WhenInjectedInto<ViewModel.SelectorViewModel>();
        }

        public System.Windows.Controls.UserControl CreateSourceView()
        {
            return Kernel.Get<Views.SelectorView>();
        }
    }
}

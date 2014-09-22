using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photo.Folder.FolderRetriever
{
    public class Module : NinjectModule
    {
        public override void Load()
        {
            Kernel.Load<FolderSelector.Module>();
            Kernel.Bind<Base.IPhotoSource>().To<Retriever>();
        }
    }
}

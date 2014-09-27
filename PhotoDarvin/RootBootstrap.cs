using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Modularity;
using Ninject;
using System.Windows;

namespace PhotoDarvin
{
    class RootBootstrap : NinjectBootstrapper
    {
        private readonly Window parentWindow;

        public DependencyObject OpenedShell { get { return Shell; } }

        public RootBootstrap(Window parentWindow)
        {
            this.parentWindow = parentWindow;
        }

        protected override Microsoft.Practices.Prism.Regions.RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var res = base.ConfigureRegionAdapterMappings();
            res.RegisterMapping(typeof(StackPanel), Kernel.Get<StackPanelRegionAdapter>());
            return res;
        }

        protected override Microsoft.Practices.Prism.Modularity.IModuleCatalog CreateModuleCatalog()
        {
            var catalog = new ModuleCatalog();
            
            return catalog;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Kernel.Load<Photo.Folder.FolderRetriever.Module>();
            Kernel.Load<Photo.SourceExplorer.Explorer.Module>();
            Kernel.Load<Photo.SourceExplorer.PhotosList.Module>();

            Kernel.Load<Photo.FolderStore.Tool.Module>();

            Kernel.Bind<OSWrap.IDialogs>().ToMethod(w => new OSWrap.Windows.Dialogs(parentWindow));
        }

        protected override System.Windows.DependencyObject CreateShell()
        {
            return Kernel.Get<RootUserControl>();
        }
    }
}

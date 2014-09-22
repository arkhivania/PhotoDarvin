using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PhotoDarvin.Tool
{
    class SampleToolModule : IModule
    {
        private readonly IRegionManager regionManager;

        public SampleToolModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            regionManager.AddToRegion("BottomTool", new TextBlock() { Text = "WOW" });
        }
    }
}

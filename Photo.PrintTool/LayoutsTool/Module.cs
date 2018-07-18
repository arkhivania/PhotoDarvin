using Nailhang.Core;
using Ninject;
using Ninject.Modules;
using Photo.PrintTool.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Photo.PrintTool.LayoutsTool
{
    public class Module : NinjectModule, IToolBarArrange
    {
        public IEnumerable<IDisposable> ArrangeToolBar(ToolBar toolBar, TrayType trayType)
        {
            if (trayType == TrayType.Layout)
            {
                var view = Kernel.Get<Views.LayoutsView>();
                toolBar.Items.Add(view);
                yield return new ActionThroughDispose(() => toolBar.Items.Remove(view));
            }
        }

        public override void Load()
        {
            Kernel.Bind<ViewModel.SetupLayout>()
                .ToSelf().InSingletonScope();

            Kernel.Bind<IToolBarArrange>()
                .ToConstant(this);
        }
    }
}

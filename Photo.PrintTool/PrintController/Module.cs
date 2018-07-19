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

namespace Photo.PrintTool.PrintController
{
    public class Module : NinjectModule, IToolBarArrange
    {
        public IEnumerable<IDisposable> ArrangeToolBar(ToolBar toolBar, TrayType trayType)
        {
            if (trayType == TrayType.Tools)
            {

                var controller = Kernel.Get<Controllers.Controller>();
                var sp = new StackPanel() { Orientation = Orientation.Horizontal };
                sp.Children.Add(new MaterialDesignThemes.Wpf.PackIcon() { Kind = MaterialDesignThemes.Wpf.PackIconKind.Printer, VerticalAlignment = System.Windows.VerticalAlignment.Center, Margin = new System.Windows.Thickness(5) });
                sp.Children.Add(new TextBlock() { Text = "Print", VerticalAlignment = System.Windows.VerticalAlignment.Center, Margin = new System.Windows.Thickness(5) });

                var printButton = new Button() { Content = sp, Command = controller.PrintCommand };
                toolBar.Items.Add(printButton);

                yield return new ActionThroughDispose(() => toolBar.Items.Remove(printButton));
                yield return new ActionThroughDispose(() => printButton.Command = null);
            }
        }

        public override void Load()
        {
            Kernel.Bind<Controllers.Controller>()
                .ToSelf().InSingletonScope();

            Kernel.Bind<IToolBarArrange>()
                .ToConstant(this);
        }
    }
}

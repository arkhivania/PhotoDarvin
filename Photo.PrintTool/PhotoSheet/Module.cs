using Nailhang.Core;
using Nailhang.MVVM;
using Ninject;
using Ninject.Modules;
using Photo.PrintTool.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Photo.PrintTool.PhotoSheet
{
    public class Module : NinjectModule, IMainGridArrange, Base.ISheetLayout, IToolBarArrange
    {
        public CurrentItemState<bool> IsLandscape { get; } = new CurrentItemState<bool>() { Value = true };

        public IEnumerable<IDisposable> Arrange(Grid mainGrid)
        {
            var view = Kernel.Get<Views.SheetViewView>();
            mainGrid.Children.Add(view);
            yield return new ActionThroughDispose(() => mainGrid.Children.Remove(view));
        }

        public IEnumerable<IDisposable> ArrangeToolBar(ToolBar toolBar, TrayType trayType)
        {
            if (trayType == TrayType.Layout)
            {
                var state = Kernel.Get<Base.ISheetLayout>();
                var binding = new Binding("Value") { Source = state.IsLandscape, Mode = BindingMode.TwoWay };
                var checkBox = new CheckBox { Content = "Landscape" };
                BindingOperations.SetBinding(checkBox, CheckBox.IsCheckedProperty, binding);

                toolBar.Items.Add(checkBox);


                yield return new ActionThroughDispose(() =>
                {
                    toolBar.Items.Remove(checkBox);
                    BindingOperations.ClearBinding(checkBox, CheckBox.IsCheckedProperty);
                });
            }
        }

        public override void Load()
        {
            Kernel.Bind<IMainGridArrange, IToolBarArrange, Base.ISheetLayout>()
                .ToConstant(this);
        }
    }
}

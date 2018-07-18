using Nailhang.Core;
using Ninject.Modules;
using Photo.PrintTool.PhotoLayout.Base;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Photo.PrintTool.PhotoItemTools
{
    public class Module : NinjectModule, IPhotoItemArrange
    {
        public IEnumerable<IDisposable> Arrange(PhotoItem item, Grid grid)
        {
            if (item == null)
                yield break;

            var vm = new ViewModel.PhotoItemViewModel(item);
            yield return vm;

            var stackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Margin = new System.Windows.Thickness(10)
            };
            grid.Children.Add(stackPanel);
            yield return new ActionThroughDispose(() => grid.Children.Remove(stackPanel));

            {
                var buttton = new Button()
                {
                    Content = "Fit Type",
                    Command = vm.SwitchFitTypeCommand,
                    Margin = new System.Windows.Thickness(10, 10, 10, 10),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = System.Windows.VerticalAlignment.Top
                };
                stackPanel.Children.Add(buttton);
                yield return new ActionThroughDispose(() => stackPanel.Children.Remove(buttton));
                yield return new ActionThroughDispose(() => buttton.Command = null);
            }

            {
                var buttton = new Button()
                {
                    Content = "Rotate CCW",
                    Command = vm.RotateLeft,
                    Margin = new System.Windows.Thickness(2, 10, 2, 10),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    VerticalAlignment = System.Windows.VerticalAlignment.Top
                };
                stackPanel.Children.Add(buttton);
                yield return new ActionThroughDispose(() => stackPanel.Children.Remove(buttton));
                yield return new ActionThroughDispose(() => buttton.Command = null);
            }

            {
                var buttton = new Button()
                {
                    Content = "Rotate CW",
                    Command = vm.RotateRight,
                    Margin = new System.Windows.Thickness(2, 10, 2, 10),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                    VerticalAlignment = System.Windows.VerticalAlignment.Top
                };
                stackPanel.Children.Add(buttton);
                yield return new ActionThroughDispose(() => stackPanel.Children.Remove(buttton));
                yield return new ActionThroughDispose(() => buttton.Command = null);
            }

            

        }

        public override void Load()
        {
            Kernel.Bind<IPhotoItemArrange>()
                .ToConstant(this);
        }
    }
}

using Nailhang.Core;
using Ninject;
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

            var vm = new ViewModel.PhotoItemViewModel(Kernel.Get<IPhotoBag>(), item);
            yield return vm;

            var rightTopPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                Margin = new System.Windows.Thickness(10)
            };
            grid.Children.Add(rightTopPanel);
            yield return new ActionThroughDispose(() => grid.Children.Remove(rightTopPanel));

            {
                var buttton = new Button()
                {
                    Content = new MaterialDesignThemes.Wpf.PackIcon() { Kind = MaterialDesignThemes.Wpf.PackIconKind.Delete, VerticalAlignment = System.Windows.VerticalAlignment.Center },
                    ToolTip = "Remove image",
                    Command = vm.RemovePhotoCommand,
                    Margin = new System.Windows.Thickness(10),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                    VerticalAlignment = System.Windows.VerticalAlignment.Top
                };
                rightTopPanel.Children.Add(buttton);
                yield return new ActionThroughDispose(() => rightTopPanel.Children.Remove(buttton));
                yield return new ActionThroughDispose(() => buttton.Command = null);
            }

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
                    Content = new MaterialDesignThemes.Wpf.PackIcon() { Kind = MaterialDesignThemes.Wpf.PackIconKind.MoveResize, VerticalAlignment = System.Windows.VerticalAlignment.Center },
                    ToolTip = "Crop/Fit",
                    Command = vm.SwitchFitTypeCommand,
                    Margin = new System.Windows.Thickness(2, 10, 10, 10),
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
                    Content = new MaterialDesignThemes.Wpf.PackIcon() { Kind = MaterialDesignThemes.Wpf.PackIconKind.RotateLeft, VerticalAlignment = System.Windows.VerticalAlignment.Center },
                    ToolTip = "Rotate left",
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
                    Content = new MaterialDesignThemes.Wpf.PackIcon() { Kind = MaterialDesignThemes.Wpf.PackIconKind.RotateRight, VerticalAlignment = System.Windows.VerticalAlignment.Center },
                    ToolTip = "Rotate right",
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

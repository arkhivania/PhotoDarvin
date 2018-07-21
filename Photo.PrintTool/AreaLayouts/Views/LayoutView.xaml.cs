using Nailhang.Core;
using Photo.PrintTool.AreaLayouts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Photo.PrintTool.AreaLayouts.Views
{
    /// <summary>
    /// Interaction logic for LayoutView.xaml
    /// </summary>
    partial class LayoutView : UserControl
    {
        private readonly IAreaLayouts areaLayouts;
        private readonly Base.IAreaArrange[] arrangers;

        public LayoutView(IAreaLayouts areaLayouts, IAreaArrange[] arrangers)
        {
            this.areaLayouts = areaLayouts;
            this.arrangers = arrangers;

            InitializeComponent();


            Loaded += LayoutView_Loaded;
            Unloaded += LayoutView_Unloaded;
            canvas_Main.SizeChanged += Canvas_Main_SizeChanged;
        }

        class AreaItem : Grid
        {
            private readonly Area area;
            public Area Area => area;

            public AreaItem(Area area)
            {
                this.area = area;
            }
        }

        private void Canvas_Main_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ArrangeItems();
        }

        private void ArrangeItems()
        {
            var layout = areaLayouts.LayoutState.Value;

            var width = canvas_Main.ActualWidth;
            var height = canvas_Main.ActualHeight;
            var border_wh = System.Math.Min(width, height);

            if (width > 0 && height > 0)
            {
                foreach (var i in canvas_Main.Children.OfType<AreaItem>())
                {
                    Canvas.SetLeft(i, i.Area.Left * width + layout.BorderSize * border_wh);
                    Canvas.SetTop(i, i.Area.Top * height + layout.BorderSize * border_wh);

                    i.Width = i.Area.Width * width - 2 * layout.BorderSize * border_wh;
                    i.Height = i.Area.Height * height - 2 * layout.BorderSize * border_wh;
                }
            }
        }

        private void LayoutView_Unloaded(object sender, RoutedEventArgs e)
        {
            areaLayouts.LayoutState.ValueChanged -= LayoutState_ValueChanged;
            disposables.Dispose();
        }

        readonly DisposableList disposables = new Nailhang.Core.DisposableList();

        private void LayoutView_Loaded(object sender, RoutedEventArgs e)
        {
            if (disposables.Count > 0)
                return;

            areaLayouts.LayoutState.ValueChanged += LayoutState_ValueChanged;
            RebuildLayout();
        }

        private void LayoutState_ValueChanged(object sender, EventArgs e)
        {
            RebuildLayout();
        }

        private void RebuildLayout()
        {
            disposables.Dispose();

            if (areaLayouts.LayoutState.Value.Areas != null)
                foreach (var a in areaLayouts.LayoutState.Value.Areas)
                {
                    var ai = new AreaItem(a)
                    {
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        UseLayoutRounding = true
                    };
                    foreach (var arr in arrangers)
                        disposables.AddRange(arr.ArrangeArea(ai, a));

                    canvas_Main.Children.Add(ai);
                    disposables.Add(new ActionThroughDispose(() => canvas_Main.Children.Remove(ai)));
                }

            disposables.Add(new ActionThroughDispose(() => { }));
            ArrangeItems();
        }
    }
}

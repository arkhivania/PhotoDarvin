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

namespace Photo.PrintTool.PhotoSheet.Views
{
    /// <summary>
    /// Interaction logic for SheetViewView.xaml
    /// </summary>
    partial class SheetViewView : UserControl
    {
        private readonly Base.ISheetLayout sheetLayout;
        readonly UserControl toolView;

        public SheetViewView(Base.ISheetLayout sheetLayout, IToolViewFactory toolViewFactory)
        {
            InitializeComponent();
            this.sheetLayout = sheetLayout;

            this.toolView = toolViewFactory.CreateView();
            canvas.Children.Add(toolView);

            Loaded += SheetViewView_Loaded;
            Unloaded += SheetViewView_Unloaded;
        }

        private void SheetViewView_Unloaded(object sender, RoutedEventArgs e)
        {
            initialized = false;
            sheetLayout.IsLandscape.ValueChanged -= IsLandscape_ValueChanged;
        }


        bool initialized = false;
        private void SheetViewView_Loaded(object sender, RoutedEventArgs e)
        {
            if (initialized)
                return;

            initialized = true;
            sheetLayout.IsLandscape.ValueChanged += IsLandscape_ValueChanged;
            UpdatePosition();
        }

        private void IsLandscape_ValueChanged(object sender, EventArgs e)
        {
            UpdatePosition();
        }

        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if(canvas.ActualWidth > 0 && canvas.ActualHeight > 0)
            {
                var source_width = 210.0;
                var source_height = 297.0;

                if(sheetLayout.IsLandscape.Value)
                {
                    source_width = 297.0;
                    source_height = 210.0;
                }

                var target_width = canvas.ActualWidth;
                var target_height = canvas.ActualHeight;

                var scale_x = target_width / source_width;
                var scale_y = target_height / source_height;

                var minScale = Math.Min(scale_x, scale_y);

                var m = new Matrix();
                m.Translate(-source_width / 2, -source_height / 2);
                m.Scale(minScale, minScale);
                m.Translate(target_width / 2, target_height / 2);

                var lt = m.Transform(new Point(0, 0));
                var rb = m.Transform(new Point(source_width, source_height));

                Canvas.SetLeft(toolView, lt.X);
                Canvas.SetTop(toolView, lt.Y);

                toolView.Width = rb.X - lt.X;
                toolView.Height = rb.Y - lt.Y;                
            }
        }
    }
}

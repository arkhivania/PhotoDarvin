using MaterialDesignThemes.Wpf;
using Nailhang.Core;
using Ninject;
using Photo.PrintTool.AreaLayouts.Base;
using Photo.PrintTool.Base;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoPrintTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly StandardKernel kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false });

        readonly DisposableList disposables = new DisposableList();



        public MainWindow()
        {
            new PaletteHelper().SetLightDark(true);
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        class WFWin32Window : System.Windows.Forms.IWin32Window
        {
            public IntPtr Handle { get; set; }
        }

        bool initialized = false;
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (initialized)
                return;

            initialized = true;

            var helper = new WindowInteropHelper(this);

            kernel.Bind<System.Windows.Forms.IWin32Window>().ToConstant(new WFWin32Window { Handle = helper.Handle });

            kernel.Load<Photo.Print.LayoutPrint.Module>();
            kernel.Load<Photo.PrintTool.AreaLayouts.Module>();
            kernel.Load<Photo.PrintTool.DragHereItem.Module>();
            kernel.Load<Photo.PrintTool.PhotoItemTools.Module>();
            kernel.Load<Photo.PrintTool.PhotoLayout.Module>();
            kernel.Load<Photo.PrintTool.LayoutsTool.Module>();
            kernel.Load<Photo.PrintTool.PrintController.Module>();
            kernel.Load<Photo.PrintTool.PhotoSheet.Module>();

            foreach (var a in kernel.GetAll<IToolBarArrange>())
            {
                disposables.AddRange(a.ArrangeToolBar(toolBar_Main, TrayType.Main));
                disposables.AddRange(a.ArrangeToolBar(toolBar_Layout, TrayType.Layout));
                disposables.AddRange(a.ArrangeToolBar(toolBar_Tools, TrayType.Tools));
            }

            foreach (var q in kernel.GetAll<IMainGridArrange>())
                disposables.AddRange(q.Arrange(grid_Main));

            foreach (var tb in new[] { toolBar_Main, toolBar_Layout, toolBar_Tools })
                if (tb.Items.Count == 0)
                    tb.Visibility = Visibility.Collapsed;
        }
    }
}

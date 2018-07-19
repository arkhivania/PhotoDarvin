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

namespace Photo.PrintTool.LayoutsTool.Views
{
    /// <summary>
    /// Interaction logic for LayoutsView.xaml
    /// </summary>
    partial class LayoutsView : UserControl
    {
        public LayoutsView(ViewModel.SetupLayout setupLayout)
        {
            InitializeComponent();

            Loaded += delegate { grid_LayoutRoot.DataContext = setupLayout; };
            Unloaded += delegate { grid_LayoutRoot.DataContext = null; };
        }
    }
}

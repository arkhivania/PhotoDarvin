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

namespace Photo.FolderStore.Tool.View
{
    /// <summary>
    /// Interaction logic for TargetFolderView.xaml
    /// </summary>
    partial class TargetFolderView : UserControl
    {
        public TargetFolderView(ViewModel.TargetFolderViewModel viewModel)
        {
            InitializeComponent();
            layoutRoot.DataContext = viewModel;
        }
    }
}

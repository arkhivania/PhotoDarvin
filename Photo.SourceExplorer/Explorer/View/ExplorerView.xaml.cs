﻿using System;
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

namespace Photo.SourceExplorer.Explorer.View
{
    /// <summary>
    /// Interaction logic for ExplorerView.xaml
    /// </summary>
    partial class ExplorerView : UserControl
    {
        public ExplorerView(ViewModel.ExplorerViewModel viewModel)
        {
            InitializeComponent();
            layoutRoot.DataContext = viewModel;
        }
    }
}
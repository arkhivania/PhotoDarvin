﻿using Ninject;
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
using Photo.SourceExplorer.Base;

namespace PhotoDarvin
{
    /// <summary>
    /// Interaction logic for RootUserControl.xaml
    /// </summary>
    public partial class RootUserControl : UserControl
    {
        public RootUserControl(IKernel kernel)
        {
            InitializeComponent();
        }
    }
}

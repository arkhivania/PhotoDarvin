using System.Windows.Controls.Primitives;
using Microsoft.Practices.Prism.PubSubEvents;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Regions;
using CommonServiceLocator.NinjectAdapter.Unofficial;
using Microsoft.Practices.Prism.Logging;

namespace PhotoDarvin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly StandardKernel kernel = new StandardKernel();

        public MainWindow()
        {
            InitializeComponent();

            var bs = new RootBootstrap(this);
            bs.Run();
            Content = bs.OpenedShell;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Photo.Base
{
    public interface ISourceViewExtension
    {
        UserControl CreateSourceView();
    }
}

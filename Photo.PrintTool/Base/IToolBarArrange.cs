using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Photo.PrintTool.Base
{
    public interface IToolBarArrange
    {
        IEnumerable<IDisposable> ArrangeToolBar(ToolBar toolBar, TrayType trayType);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Photo.PrintTool.AreaLayouts.Base
{
    public interface IAreaArrange
    {
        IEnumerable<IDisposable> ArrangeArea(Grid grid, Area area);
    }
}

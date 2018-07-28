using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Photo.PrintTool.Base
{
    public interface IMainGridArrange
    {
        IEnumerable<IDisposable> Arrange(Grid mainGrid);
    }
}

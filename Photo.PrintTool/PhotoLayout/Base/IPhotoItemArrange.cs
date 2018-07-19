using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Photo.PrintTool.PhotoLayout.Base
{
    public interface IPhotoItemArrange
    {
        IEnumerable<IDisposable> Arrange(PhotoItem item, Grid grid);
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.PrintTool.PhotoLayout.Base
{
    public interface IPhotoBag
    {
        ObservableCollection<PhotoItem> Items { get; }
    }
}

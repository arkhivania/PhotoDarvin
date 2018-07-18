using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.PrintTool.PhotoLayout.Base;

namespace Photo.PrintTool.PhotoLayout.Processing
{
    class PhotoBag : Base.IPhotoBag
    {
        public ObservableCollection<PhotoItem> Items { get; } = new ObservableCollection<PhotoItem>();
    }
}

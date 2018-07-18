using Nailhang.MVVM;
using Photo.Print.Layout.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.PrintTool.PhotoLayout.Base
{
    public class PhotoItem
    {
        public string FileName { get; set; }
        public string AreaID { get; set; }

        public CurrentItemState<FitType> FitType { get; } = new CurrentItemState<FitType>() { Value = Print.Layout.Base.FitType.Crop };
        public CurrentItemState<float> Angle { get; } = new CurrentItemState<float>();
    }
}

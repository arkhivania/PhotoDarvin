using Nailhang.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.PrintTool.PhotoSheet.Base
{
    public interface ISheetLayout
    {
        CurrentItemState<bool> IsLandscape { get; }
    }
}

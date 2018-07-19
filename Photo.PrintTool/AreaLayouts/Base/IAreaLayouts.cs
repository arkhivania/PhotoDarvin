using Nailhang.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.PrintTool.AreaLayouts.Base
{
    public interface IAreaLayouts
    {
        CurrentItemState<Layout> LayoutState { get; }
    }
}

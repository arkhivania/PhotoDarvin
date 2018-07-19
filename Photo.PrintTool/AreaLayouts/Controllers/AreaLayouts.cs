using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nailhang.MVVM;
using Photo.PrintTool.AreaLayouts.Base;

namespace Photo.PrintTool.AreaLayouts.Controllers
{
    class AreaLayouts : Base.IAreaLayouts
    {
        public CurrentItemState<Layout> LayoutState { get; } = new CurrentItemState<Layout>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Print.LayoutPrint.Base
{
    public interface ILayoutPrint
    {
        void Print(Layout.Base.Layout layout);
    }
}

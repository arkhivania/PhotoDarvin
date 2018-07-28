using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Print.Layout.Base
{
    public struct Layout
    {
        public Area[] Items { get; set; }
        public bool IsLandscape { get; set; }
    }
}

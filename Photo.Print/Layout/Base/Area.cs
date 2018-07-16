using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Print.Layout.Base
{
    public struct Area
    {
        public float Left { get; set; }
        public float Top { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public Photo? Photo { get; set; }
    }
}

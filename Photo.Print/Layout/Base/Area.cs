using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Print.Layout.Base
{
    public struct Area
    {
        public double Left { get; set; }
        public double Top { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }

        public Photo? Photo { get; set; }
        public float Angle { get; set; }
        public FitType FitType { get; set; }

        public double Border { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.PrintTool.LayoutsTool.Base
{
    public struct Layout
    {
        public int Rows { get; set; }
        public int Columns { get; set; }

        public string Name => $"{Columns}x{Rows}";
    }
}

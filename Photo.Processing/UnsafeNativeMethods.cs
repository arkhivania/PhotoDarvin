using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Processing
{
    static class UnsafeNativeMethods
    {
        [DllImport("Native.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int Sample_Call(int a);
    }
}

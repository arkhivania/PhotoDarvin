using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photo.Base
{
    public class Photo
    {
        public string FilePath { get; set; }
        public DateTime FileTimeUtc { get; set; }

        public DateTime FileTime { get 
        {
            return FileTimeUtc.ToLocalTime();
        } }
    }
}

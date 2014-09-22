using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photo.Base
{
    public interface IPhotoSource
    {
        IEnumerable<Photo> RetrievePhotos();
    }
}

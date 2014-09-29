using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Photo.Folder.FolderRetriever
{
    class Retriever : Base.IPhotoSource
    {
        private readonly FolderSelector.Base.OperatingState operatingState;

        public Retriever(FolderSelector.Base.OperatingState operatingState)
        {
            this.operatingState = operatingState;
        }

        public IEnumerable<Base.Photo> RetrievePhotos()
        {
            if(string.IsNullOrEmpty(operatingState.Folder))
                return Enumerable.Empty<Base.Photo>();

            return from f in new DirectoryInfo(operatingState.Folder).GetFiles("*.jpg")
                   orderby f.LastWriteTimeUtc
                   select new Photo.Base.Photo { FilePath = f.FullName, PhotoTime = f.LastWriteTimeUtc };
        }
    }
}

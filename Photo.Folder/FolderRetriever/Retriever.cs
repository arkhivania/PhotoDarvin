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
                   orderby f.CreationTimeUtc
                   select new Photo.Base.Photo { FilePath = f.FullName, PhotoTime = (GetDateTime(f.FullName) ?? (DateTime?)f.CreationTime).Value };
        }

        public static DateTime? GetDateTime(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var bitmapFrame = BitmapFrame.Create(fileStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                BitmapMetadata bitmapMetadata = bitmapFrame.Metadata as BitmapMetadata;
                if(!string.IsNullOrEmpty(bitmapMetadata.DateTaken))
                {
                    DateTime res;
                    if (DateTime.TryParse(bitmapMetadata.DateTaken, out res))
                        return res;
                }
            }
            return null;
        }
    }
}

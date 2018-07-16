using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.SourceExplorer.Base
{
    public class OperatingState
    {
        Photo.Base.Photo[] explorePhotos;
        public Photo.Base.Photo[] ExplorePhotos
        {
            get { return explorePhotos; }
            set 
            {
                explorePhotos = value;
                if (ExplorePhotosChanged != null)
                    ExplorePhotosChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler ExplorePhotosChanged;

        private Photo.Base.Photo? selectedPhoto;
        public Photo.Base.Photo? SelectedPhoto
        {
            get { return selectedPhoto; }
            set 
            {
                selectedPhoto = value;
                if (SelectedPhotoChanged != null)
                    SelectedPhotoChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler SelectedPhotoChanged;
    }
}

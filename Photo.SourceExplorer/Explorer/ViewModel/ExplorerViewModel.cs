using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Base;
using System.Windows.Media;

namespace Photo.SourceExplorer.Explorer.ViewModel
{
    class ExplorerViewModel : IDisposable
    {
        private readonly IEventAggregator eventAggregator;
        private readonly Base.OperatingState operatingState;

        ImageSource selectedImage;

        public ImageSource SelectedImage
        {
            get { return selectedImage; }
            private set 
            {
                selectedImage = value;
                if (SelectedImageChanged != null)
                    SelectedImageChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler SelectedImageChanged;

        public Base.OperatingState OperatingState
        {
            get { return operatingState; }
        } 


        readonly IDisposable watch;


        public ExplorerViewModel(IEventAggregator eventAggregator, Base.OperatingState operatingState)
        {
            this.eventAggregator = eventAggregator;

            this.operatingState = operatingState;
            watch = eventAggregator.GetEvent<Photo.Base.SelectedSourceChangedEvent>().Subscribe(ps => UpdateBy(ps));
            operatingState.SelectedPhotoChanged += operatingState_SelectedPhotoChanged;
        }

        void operatingState_SelectedPhotoChanged(object sender, EventArgs e)
        {
            SelectedImage = null;
            if (operatingState.SelectedPhoto != null)
                SelectedImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(operatingState.SelectedPhoto.FilePath));
        }

        private void UpdateBy(IPhotoSource ps)
        {
            operatingState.ExplorePhotos = ps.RetrievePhotos().ToArray();
            operatingState.SelectedPhoto = operatingState.ExplorePhotos.FirstOrDefault();
        }

        public void Dispose()
        {
            operatingState.SelectedPhotoChanged -= operatingState_SelectedPhotoChanged;
            watch.Dispose();
        }
    }
}

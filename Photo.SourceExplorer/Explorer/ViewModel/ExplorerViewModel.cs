using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Base;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;

namespace Photo.SourceExplorer.Explorer.ViewModel
{
    class ExplorerViewModel : IDisposable
    {
        private readonly IEventAggregator eventAggregator;
        private readonly Base.OperatingState operatingState;


        DisplayImage displayImage;
        public DisplayImage DisplayImage
        {
            get { return displayImage; }
            set 
            {
                displayImage = value;
                if (DisplayImageChanged != null)
                    DisplayImageChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler DisplayImageChanged;
        

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
            DisplayImage = null;
            if (operatingState.SelectedPhoto != null)
                DisplayImage = new DisplayImage 
                {
                    Image = new System.Windows.Media.Imaging.BitmapImage(new Uri(operatingState.SelectedPhoto.Value.FilePath)),
                    Angle = DefineAngle(operatingState.SelectedPhoto.Value.FilePath)
                };
        }

        private double DefineAngle(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var bitmapFrame = BitmapFrame.Create(fileStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                BitmapMetadata bitmapMetadata = bitmapFrame.Metadata as BitmapMetadata;

                const string _orientationQuery = "System.Photo.Orientation";
                if ((bitmapMetadata != null) && (bitmapMetadata.ContainsQuery(_orientationQuery)))
                {
                    object o = bitmapMetadata.GetQuery(_orientationQuery);

                    if (o != null)
                    {
                        //refer to http://www.impulseadventure.com/photo/exif-orientation.html for details on orientation values
                        switch ((ushort)o)
                        {
                            case 6:
                                return 90D;
                            case 3:
                                return 180D;
                            case 8:
                                return 270D;                            
                        }
                    }
                }
            }

            return 0;
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

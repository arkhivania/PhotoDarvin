using Microsoft.Practices.Prism.Commands;
using Photo.Print.Layout.Base;
using Photo.PrintTool.PhotoLayout.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.PrintTool.PhotoItemTools.ViewModel
{
    class PhotoItemViewModel : IDisposable
    {
        private readonly IPhotoBag photoBag;
        private readonly PhotoItem photoItem;

        public DelegateCommand RotateLeft { get; }
        public DelegateCommand RotateRight { get; }
        public DelegateCommand SwitchFitTypeCommand { get; }
        public DelegateCommand RemovePhotoCommand { get; }

        public PhotoItemViewModel(IPhotoBag photoBag, PhotoItem photoItem)
        {
            this.photoBag = photoBag;
            this.photoItem = photoItem;

            RotateLeft = new DelegateCommand(() => photoItem.Angle.Value -= 90);
            RotateRight = new DelegateCommand(() => photoItem.Angle.Value += 90);
            SwitchFitTypeCommand = new DelegateCommand(() => SwitchFitType());
            RemovePhotoCommand = new DelegateCommand(() => Remove());
        }

        private void Remove()
        {
            var index = photoItem.AreaID;
            photoBag.Items.Remove(photoItem);
            foreach(var pi in photoBag.Items
                .Where(q => q.AreaID > index)
                .ToArray())
            {
                photoBag.Items.Remove(pi);
                pi.AreaID--;
                photoBag.Items.Add(pi);
            }
        }

        void SwitchFitType()
        {
            var type = (int)photoItem.FitType.Value;
            var fitValues = Enum.GetValues(typeof(FitType)).OfType<FitType>().ToArray();
            var count = fitValues.Length;
            photoItem.FitType.Value = fitValues[(++type) % count];
        }

        public void Dispose()
        {
            
        }
    }
}

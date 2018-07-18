using Nailhang.Core;
using Photo.Print.Layout.Base;
using Photo.PrintTool.AreaLayouts.Base;
using Photo.PrintTool.PhotoLayout.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Area = Photo.PrintTool.AreaLayouts.Base.Area;

namespace Photo.PrintTool.PhotoLayout.ViewItems
{
    class PhotoViewItem : Grid, IDisposable
    {
        private readonly IPhotoBag photoBag;
        private readonly Area area;
        private readonly IPhotoItemArrange[] photoItemArranges;
        PhotoItem photoItem = new PhotoItem();

        readonly Image displayImage = new Image();
        readonly Canvas canvas = new Canvas() { VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };

        public PhotoViewItem(IPhotoBag photoBag, Area area, Base.IPhotoItemArrange[] photoItemArranges)
        {
            this.photoBag = photoBag;
            this.area = area;
            this.photoItemArranges = photoItemArranges;
            Background = Brushes.Transparent;
            this.ClipToBounds = true;

            canvas.Children.Add(displayImage);
            Children.Add(canvas);

            SetupPhotoItem(photoBag.Items.FirstOrDefault(q => q.AreaID == area.Id));
            photoBag.Items.CollectionChanged += Items_CollectionChanged;
            SizeChanged += PhotoViewItem_SizeChanged;

            AllowDrop = true;
            DragEnter += PhotoViewItem_DragEnter;
            Drop += PhotoViewItem_Drop;
        }

        private void PhotoViewItem_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetFormats().Contains("FileNameW"))
            {
                var filePath = e.Data.GetData("FileNameW") as string[];
                if (filePath != null && filePath.Length > 0)
                {
                    if(photoItem != null)
                        photoBag.Items.Remove(photoItem);

                    photoBag.Items.Add(new PhotoItem { AreaID = area.Id, FileName = filePath[0] });
                }
            }
        }

        private void PhotoViewItem_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetFormats().Contains("FileNameW"))
            {
                var filePath = e.Data.GetData("FileNameW") as string[];
                if(filePath != null)
                {
                    e.Effects = DragDropEffects.Copy;
                    //e.Handled = true;
                }
            }
        }

        private void PhotoViewItem_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            UpdateTransform();
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetupPhotoItem(photoBag.Items.FirstOrDefault(q => q.AreaID == area.Id));
        }

        void UpdateTransform()
        {
            if (ActualWidth > 0 && ActualHeight > 0 && displayImage.Source != null && photoItem != null)
            {
                var angle = photoItem.Angle.Value;

                var bmpSource = displayImage.Source;
                var targetWidth = ActualWidth;
                var targetHeight = ActualHeight;

                var sw_source = bmpSource.Width;
                var sh_source = bmpSource.Height;

                var dm = new Matrix();
                dm.Rotate(angle);

                var corners = new[] { new Point(0, 0), new Point(sw_source, 0), new Point(sw_source, sh_source), new Point(0, sh_source) };
                dm.Transform(corners);

                var sx_max = corners.Select(q => q.X).Max();
                var sx_min = corners.Select(q => q.X).Min();

                var sy_max = corners.Select(q => q.Y).Max();
                var sy_min = corners.Select(q => q.Y).Min();

                var scale_x = targetWidth / (sx_max - sx_min);
                var scale_y = targetHeight / (sy_max - sy_min);

                double scale = 1;
                switch (photoItem.FitType.Value)
                {
                    case FitType.Fit:
                        scale = System.Math.Min(scale_x, scale_y);
                        break;
                    case FitType.Crop:
                        scale = System.Math.Max(scale_x, scale_y);
                        break;
                }

                var m2d = new Matrix();

                m2d.Translate(-sw_source / 2f, -sh_source / 2f);
                m2d.Rotate(angle);
                m2d.Scale(scale, scale);
                m2d.Translate(ActualWidth/2, ActualHeight/2);

                displayImage.RenderTransform = new MatrixTransform(m2d);
            }
        }

        readonly DisposableList itemDisposables = new DisposableList();

        private void SetupPhotoItem(PhotoItem photoItem)
        {
            if (this.photoItem == photoItem)
                return;

            itemDisposables.Dispose();

            if (this.photoItem != null)
            {
                this.photoItem.Angle.ValueChanged -= Angle_ValueChanged;
                this.photoItem.FitType.ValueChanged -= FitType_ValueChanged;
            }

            this.photoItem = photoItem;
            if (photoItem == null)
                displayImage.Source = null;
            else
            {
                var image = new BitmapImage();

                using (var stream = new FileStream(photoItem.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                }

                displayImage.Source = image;
                displayImage.Width = image.Width;
                displayImage.Height = image.Height;
                UpdateTransform();
            }

            if (this.photoItem != null)
            {
                this.photoItem.Angle.ValueChanged += Angle_ValueChanged;
                this.photoItem.FitType.ValueChanged += FitType_ValueChanged;
            }

            foreach (var a in photoItemArranges)
                itemDisposables.AddRange(a.Arrange(this.photoItem, this));
        }

        private void FitType_ValueChanged(object sender, EventArgs e)
        {
            UpdateTransform();
        }

        private void Angle_ValueChanged(object sender, EventArgs e)
        {
            UpdateTransform();
        }

        public void Dispose()
        {
            Children.Remove(canvas);

            SizeChanged -= PhotoViewItem_SizeChanged;
            photoBag.Items.CollectionChanged -= Items_CollectionChanged;
        }
    }
}

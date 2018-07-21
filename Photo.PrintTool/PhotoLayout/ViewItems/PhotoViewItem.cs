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
        private readonly LayoutCache layoutCache;
        PhotoItem photoItem = new PhotoItem();

        readonly Image displayImage = new Image();
        readonly Canvas canvas = new Canvas() { VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch, CacheMode = new BitmapCache() { } };

        public PhotoViewItem(IPhotoBag photoBag,
            Area area,
            IPhotoItemArrange[] photoItemArranges,
            LayoutCache layoutCache)
        {
            this.photoBag = photoBag;
            this.area = area;
            this.photoItemArranges = photoItemArranges;
            this.layoutCache = layoutCache;
            Background = Brushes.Transparent;
            this.ClipToBounds = true;
            RenderOptions.SetBitmapScalingMode(displayImage, BitmapScalingMode.Fant);

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
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                    if (files.Length == 1)
                    {
                        var dir = new DirectoryInfo(files[0]);
                        if (dir.Exists)
                            files = dir.GetFiles("*.jpg", SearchOption.AllDirectories)
                                .Concat(dir.GetFiles("*.jpeg", SearchOption.AllDirectories))
                                .Concat(dir.GetFiles("*.png", SearchOption.AllDirectories))
                                .Select(q => q.FullName).ToArray();
                    }

                    if (files.Length == 1)
                    {
                        if (photoItem != null)
                            photoBag.Items.Remove(photoItem);

                        layoutCache.RemoveCached(files[0]);

                        photoBag.Items.Add(new PhotoItem
                        {
                            AreaID = area.Id,
                            FileName = files[0]
                        });
                    }
                    else
                    {
                        var targetIndex = area.Id;

                        foreach (var filePath in files)
                        {
                            var targetPI = photoBag.Items.FirstOrDefault(q => q.AreaID == targetIndex);

                            if (photoItem != null)
                                photoBag.Items.Remove(targetPI);

                            layoutCache.RemoveCached(filePath);
                            photoBag.Items.Add(new PhotoItem
                            {
                                AreaID = targetIndex,
                                FileName = filePath
                            });
                            targetIndex++;
                        }
                    }
                }
            }
            catch { }
        }

        private void PhotoViewItem_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                    e.Effects = DragDropEffects.Copy;
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
                m2d.Translate(ActualWidth / 2, ActualHeight / 2);

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
                try
                {
                    var image = layoutCache.TryGetFromCache(photoItem.FileName);
                    if (image == null)
                    {
                        image = new BitmapImage();
                        using (var stream = new FileStream(photoItem.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            image.BeginInit();
                            image.StreamSource = stream;
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.EndInit();
                        }
                        layoutCache.StoreInCache(photoItem.FileName, image);
                    }

                    displayImage.Source = image;
                    displayImage.Width = image.Width;
                    displayImage.Height = image.Height;
                }
                catch
                {
                    displayImage.Source = null;
                }
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

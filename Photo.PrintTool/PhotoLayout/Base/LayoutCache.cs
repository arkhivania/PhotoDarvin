using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Photo.PrintTool.PhotoLayout.Base
{
    class LayoutCache
    {
        readonly Dictionary<string, WeakReference> cache = new Dictionary<string, WeakReference>();

        public void StoreInCache(string filePath, BitmapImage image)
        {
            cache[filePath] = new WeakReference(image);
        }

        public BitmapImage TryGetFromCache(string filePath)
        {
            if(cache.TryGetValue(filePath, out WeakReference r))
            {
                var image = (BitmapImage)r.Target;
                if (image != null)
                    return image;
            }

            return null;
        }

        internal void RemoveCached(string filePath)
        {
            cache.Remove(filePath);
        }
    }
}

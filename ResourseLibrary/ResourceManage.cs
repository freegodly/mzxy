using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
namespace ResourseLibrary
{
    public class ResourceManage
    {
        public static BitmapImage GetResource(string name)
        {
            BitmapImage bitmapImage = null;
            try
            {
                Bitmap bit = (Bitmap)MyResource.ResourceManager.GetObject(name);
                MemoryStream MS = new MemoryStream();
                bit.Save(MS, ImageFormat.Png);
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(MS.ToArray());
                bitmapImage.EndInit();
            }
            catch (Exception)
            {

            }
            return bitmapImage;
        }
    }
}

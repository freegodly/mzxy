using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;

namespace Dream
{
    public class GetShowMapImage : DependencyObject
    {

       // private DispatcherTimer DTimer;
        private GetMapResource map;

        public int Width;
        public int Height;
        private int SubMapWidth = 320;
        private int SubMapHeight = 240;
        private int oldx = -1;
        private int oldy = -1;

        public byte[,] mapMask;
        public int setx = 0;
        public int sety = 0;

        

        public Point SpiritPoint
        {
            get { return (Point)GetValue(SpiritPointProperty); }
            set { SetValue(SpiritPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SpiritPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpiritPointProperty =
            DependencyProperty.Register("SpiritPoint", typeof(Point), typeof(GetShowMapImage), new UIPropertyMetadata(new Point(100,100)));




        public ImageBrush MapBrush
        {
            get { return (ImageBrush)GetValue(MapBrushProperty); }
            set { SetValue(MapBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MapBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapBrushProperty =
            DependencyProperty.Register("MapBrush", typeof(ImageBrush), typeof(GetShowMapImage), new UIPropertyMetadata());

        

        public BitmapSource MapImage
        {
            get { return (BitmapSource)GetValue(MapPropertyProperty); }
            set { SetValue(MapPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MapProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapPropertyProperty =
            DependencyProperty.Register("MapImage", typeof(BitmapSource), typeof(GetShowMapImage), new UIPropertyMetadata());

        

        public  GetShowMapImage(string mapname)
        {
            map = new GetMapResource();
            map.ReadMap(mapname);
            mapMask = map.MapMask;
            Width = map.Width;
            Height = map.Height;
            MapBrush = new ImageBrush();
            //DTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 100), DispatcherPriority.ApplicationIdle, new EventHandler(UpDate), this.Dispatcher);
        }
      

        /// <summary>
        /// 更新地图
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public void MapUpdate()
        {
            setx = (int)SpiritPoint.X - SubMapWidth-SubMapWidth/2;
            sety = (int)SpiritPoint.Y - SubMapHeight-SubMapHeight/2;
            if (setx < 0) setx = 0;
            if (setx > map.Width - SubMapWidth * 3) setx = (int)map.Width - SubMapWidth * 3;
            if (sety < 0) sety = 0;
            if (sety > map.Height - SubMapHeight * 3) sety = (int)map.Height - SubMapHeight * 3;
            if (setx != oldx || sety != oldy)
            {
                MapImage = new CroppedBitmap(map.MapMax, new Int32Rect(setx, sety, SubMapWidth * 3, SubMapHeight * 3));
                MapBrush.ImageSource = MapImage;
            }
           
        }
    }
}

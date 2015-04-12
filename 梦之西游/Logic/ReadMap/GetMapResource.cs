using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using ResourseLibrary;

namespace Dream
{
    public class GetMapResource
    {

      

        public String MapPath { get; set; }


        public int Width;
        public int Height;
        public BitmapImage[,] BI;
        public BitmapSource MapMax;
        private ReadMapFileClass rmc;

        public int SubMapWidth = 320;
        public int SubMapHeight = 240;
        public int Rows;
        public int Cols;


        public byte[,] MapMask;

        public void ReadMap(string mappath)
        {
            MapPath = mappath;
            rmc = new ReadMapFileClass(MapPath);
            rmc.ReadMap();
            Width = (int)rmc.head.MapWidth;
            Height = (int)rmc.head.MapHeight;
            Rows = rmc.Rows;
            Cols = rmc.Cols;
            //BI = new BitmapImage[Rows, Cols];
            DrawingVisual DV = new DrawingVisual();
            DrawingContext drawingContext = DV.RenderOpen();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                  
                   // BI[i, j] = PGC.ByteArrayToBitmapImage(rmc.unit[(i ) * Cols + j ].UnitData);
                    drawingContext.DrawImage(PGC.ByteArrayToBitmapImage(rmc.unit[(i) * Cols + j].UnitData), new Rect(j * SubMapWidth, i * SubMapHeight, SubMapWidth, SubMapHeight));
                }
            }
            drawingContext.Close();
            RenderTargetBitmap composeImage = new RenderTargetBitmap(Width, Height, 0, 0, PixelFormats.Pbgra32);
            composeImage.Render(DV);
            MapMax = composeImage;

            try
            {
                FileInfo fmask = new FileInfo(mappath.Split('.')[0] + "-mask.data");
                FileStream fs = fmask.OpenRead();
                byte[] ms = new byte[fmask.Length];
                fs.Read(ms, 0, ms.Length);
                fs.Close();
                MapInfo mask = (MapInfo)PGC.BytesToObject(ms);
                MapMask = new byte[mask.Data.GetLength(0), mask.Data.GetLength(1)];
                for (int i = 0; i < mask.Data.GetLength(0); i++)
                {
                    for (int j = 0; j < mask.Data.GetLength(1); j++)
                    {
                        MapMask[i, j] = (byte)mask.Data[i, j];
                    }
                }
            }
            catch
            { };

      
        }
    }
}

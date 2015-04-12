using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dream
{
    /// <summary>
    /// 公共的静态
    /// </summary>
   public static class PGC
    {
      

        //方向
        public  enum Directions { S = 0, W = 1, E = 2, N = 3, WS = 4, ES = 5, WN = 6, EN = 7 };



        /// <summary>
        /// 通过正切值获取精灵的朝向代号
        /// </summary>
        /// <param name="targetX">目标点的X值</param>
        /// <param name="targetY">目标点的Y值</param>
        /// <param name="currentX">当前点的X值</param>
        /// <param name="currentY">当前点的Y值</param>
        public static Directions GetDirectionByTan(double targetX, double targetY, double currentX, double currentY)
        {

            double tan = (targetY - currentY) / (targetX - currentX);

            if (Math.Abs(tan) >= Math.Tan(Math.PI * 3 / 8) && targetY <= currentY)
            {

                return Directions.N;

            }
            else if (Math.Abs(tan) > Math.Tan(Math.PI / 8) && Math.Abs(tan) < Math.Tan(Math.PI * 3 / 8) && targetX > currentX && targetY < currentY)
            {

                return Directions.EN;

            }
            else if (Math.Abs(tan) <= Math.Tan(Math.PI / 8) && targetX >= currentX)
            {

                return Directions.E;

            }
            else if (Math.Abs(tan) > Math.Tan(Math.PI / 8) && Math.Abs(tan) < Math.Tan(Math.PI * 3 / 8) && targetX > currentX && targetY > currentY)
            {

                return Directions.ES;

            }
            else if (Math.Abs(tan) >= Math.Tan(Math.PI * 3 / 8) && targetY >= currentY)
            {

                return Directions.S;

            }
            else if (Math.Abs(tan) > Math.Tan(Math.PI / 8) && Math.Abs(tan) < Math.Tan(Math.PI * 3 / 8) && targetX < currentX && targetY > currentY)
            {

                return Directions.WS;

            }
            else if (Math.Abs(tan) <= Math.Tan(Math.PI / 8) && targetX <= currentX)
            {

                return Directions.W;

            }
            else if (Math.Abs(tan) > Math.Tan(Math.PI / 8) && Math.Abs(tan) < Math.Tan(Math.PI * 3 / 8) && targetX < currentX && targetY < currentY)
            {

                return Directions.WN;

            }
            else
            {

                return Directions.S;

            }

        }


       

        public static BitmapImage ByteArrayToBitmapImage(byte[] byteArray) 
        { 
            BitmapImage bmp = null;

            try 
            { 
                bmp = new BitmapImage(); 
                bmp.BeginInit(); 
                bmp.StreamSource = new MemoryStream(byteArray); 
                bmp.EndInit(); 
            } 
            catch 
            { 
                bmp = null; 
            }

            return bmp; 
        }

       //BitmapImage转换为byte[]：
        public static byte[] BitmapImageToByteArray(BitmapImage bmp) 
        { 
            byte[] byteArray = null;

            try 
            { 
                Stream sMarket = bmp.StreamSource;

                if (sMarket != null && sMarket.Length > 0) 
                { 
                    //很重要，因为Position经常位于Stream的末尾，导致下面读取到的长度为0。 
                    sMarket.Position = 0;

                    using (BinaryReader br = new BinaryReader(sMarket)) 
                    { 
                        byteArray = br.ReadBytes((int)sMarket.Length); 
                    } 
                } 
            } 
            catch 
            { 
                //other exception handling 
            }

            return byteArray; 
        }


        /// <summary>         
        /// 将一个object对象序列化，返回一个byte[]        
        /// /// </summary>        
        /// /// <param name="obj">能序列化的对象</param>         
        /// <returns></returns>        
        public static byte[] ObjectToBytes(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }
        /// <summary>         
        /// 将一个序列化后的byte[]数组还原         
        /// </summ
        /// <param name="Bytes"></param>
        /// <returns></returns>     
        public static object BytesToObject(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(ms);
            }
        }
    }
}

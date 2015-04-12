using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Dream
{
   public class ReadMapFileClass
    {
        string Mappath;
       FileStream Mapfile;
       public Head head;
       public Unit[] unit;
       public int Rows;
       public int Cols;
       public MaskHead maskhead;
       public MaskUnit[] maskunit;

       public ReadMapFileClass(string mappath)
       {
           Mappath = mappath;
       }


       public void ReadMap()
       {
           Mapfile = File.OpenRead(Mappath);
           byte []h=new byte[sizeof(UInt32)*3];
           Mapfile.Read(h, 0, h.Length);

           head.Flag = BitConverter.ToUInt32(h, 0);
           head.MapWidth = BitConverter.ToUInt32(h, 4);
           head.MapHeight = BitConverter.ToUInt32(h, 8);
           Rows = (int)Math.Ceiling( (double)(head.MapHeight / 240.00));
           Cols = (int)Math.Ceiling((double)(head.MapWidth / 320.00));
           head.IndexLength = Rows*Cols;
           head.MapIndexList = new UInt32[head.IndexLength];

           byte[] by = new byte[sizeof(UInt32) * head.IndexLength];
           Mapfile.Read(by, 0, by.Length);
           for (int i = 0; i < head.IndexLength; i++)
           {
               head.MapIndexList[i] = BitConverter.ToUInt32(by, i * sizeof(UInt32));
           }
           unit = new Unit[Rows*Cols];

           //读取maskhead数据
           byte[] d = new byte[8];
           Mapfile.Read(d, 0, d.Length);
           maskhead.Unknown = BitConverter.ToUInt32(d, 0);
           maskhead.MaskNum = BitConverter.ToInt32(d, 4);

           maskhead.MaskList = new uint[maskhead.MaskNum];
           maskunit = new MaskUnit[maskhead.MaskNum];

           byte[] by1 = new byte[sizeof(UInt32) * maskhead.MaskNum];
           Mapfile.Read(by1, 0, by1.Length);
           for (int i = 0; i < maskhead.MaskNum; i++)
           {
               maskhead.MaskList[i] = BitConverter.ToUInt32(by1, i * sizeof(UInt32));
           }


           //读取所有数据单元的数据
           for (int i = 0; i < Rows*Cols; i++)
           {
               
                   ReadUnit(i);
              
           }
           //读取所有mask单元数据
           for (int i = 0; i < maskhead.MaskNum; i++)
           {
               ReadMaskUint(i);
           }


           Mapfile.Close();

       }

       /// <summary>
       /// 读取数据单元的数据
       /// </summary>
       /// <param name="H"></param>
       /// <param name="V"></param>
       public void ReadUnit(int index)
       {
           int startaddress = (int)head.MapIndexList[index];
           byte[] n = new byte[4];
           Mapfile.Seek(startaddress, SeekOrigin.Begin);
           Mapfile.Read(n, 0, n.Length);
           unit[index].UnitN = BitConverter.ToUInt32(n, 0);
           unit[index].UnitNData = new byte[unit[index].UnitN * 4];
           Mapfile.Read(unit[index].UnitNData, 0, unit[index].UnitNData.Length);

           byte[] h = new byte[sizeof(UInt32) * 2];
           Mapfile.Read(h, 0, h.Length);

           //读取数据快标识
           unit[index].UnitFlag = BitConverter.ToUInt32(h, 0);
           unit[index].Size = BitConverter.ToUInt32(h, sizeof(UInt32));

           switch (unit[index].UnitFlag)
                {
                    // GEPJ "47 45 50 4A"
                    case 0x4A504547:
                        ReadJPEG(Mapfile, unit[index].UnitFlag, unit[index].Size, index);
                        break;
                    // LLEC "4C 4C 45 43"
                    case 0x43454C4C:
                        ReadCELL(Mapfile, unit[index].UnitFlag, unit[index].Size, index);
                        break;
                    // GIRB "47 49 52 42"
                    case 0x42524947:
                        ReadBRIG(Mapfile, unit[index].UnitFlag, unit[index].Size, index);
                        break;
                    // 默认处理
                    default:
                        // 错误标志
                        break;
                }
       }

       private void ReadBRIG(FileStream Mapfile, uint p1, uint p2,  int index)
       {
           throw new NotImplementedException();
       }

       private void ReadCELL(FileStream Mapfile, uint p1, uint p2,  int index)
       {
           throw new NotImplementedException();
       }

       private void ReadJPEG(FileStream Mapfile, uint p1, uint p2, int index)
       {
           unit[index].UnitData = new byte[unit[index].Size];
           Mapfile.Read(unit[index].UnitData, 0, unit[index].UnitData.Length);

           //转换数据

           MemoryStream ms = new MemoryStream(4096);
           bool isFilled = false;
           //ms.Position = 0;
           ms.Write(unit[index].UnitData, 0, 2);
           int p = 4;
           int start = 4;
           for (; p < unit[index].UnitData.Length - 2; p++)
           {
               if ((!isFilled) && (unit[index].UnitData[p] == (byte)255))
               {
                   p++;
                   if (unit[index].UnitData[p] == (byte)218)
                   {
                       isFilled = true;
                       unit[index].UnitData[(p + 2)] = 0x0C; //12
                       ms.Write(unit[index].UnitData, start, p + 10 - start);
                      
                       ms.WriteByte(0);
                       ms.WriteByte(63);
                       ms.WriteByte(0);
                       start = p + 10;
                       p += 9;
                   }
               }
               if ((isFilled) && ((int)unit[index].UnitData[p] == (byte)255))
               {
                   ms.Write(unit[index].UnitData, start, p + 1 - start);
                 
                   ms.WriteByte(0);
                   start = p + 1;
               }
           }
           ms.Write(unit[index].UnitData, start, unit[index].UnitData.Length - start);

           unit[index].UnitData = ms.ToArray();
       }

       /// <summary>
       /// 读取mask的数据
       /// </summary>
       /// <param name="index"></param>
       private void ReadMaskUint(int index)
       {
           int startaddress = (int)maskhead.MaskList[index];
           maskunit[index] = new MaskUnit();
           byte[] n = new byte[4*5];
           Mapfile.Seek(startaddress, SeekOrigin.Begin);
           Mapfile.Read(n, 0, n.Length);

           maskunit[index].StartX = BitConverter.ToInt32(n, 0);
           maskunit[index].StartY = BitConverter.ToInt32(n, 4);
           maskunit[index].Width = BitConverter.ToInt32(n, 8);
           maskunit[index].Height= BitConverter.ToInt32(n, 12);
           maskunit[index].MaskN = BitConverter.ToInt32(n, 16);
           maskunit[index].MaskData = new byte[maskunit[index].MaskN];

           Mapfile.Read(maskunit[index].MaskData, 0, maskunit[index].MaskData.Length);

       }

       //4字节0.1M (M1.0) 0x302E314D
       //4字节地图的宽度
       //4字节地图的高度

       //4*n字节 地图单元的引索n=地图的宽度/640*2 * 地图高度/480*2

       /// <summary>
       /// 
       /// </summary>
       public struct Head
       {
           public UInt32 Flag;
           public UInt32 MapWidth;
           public UInt32 MapHeight;
           public Int32 IndexLength;
           public UInt32[] MapIndexList; //4*n
       }



        //开头4字节未知，接下来4字节是遮盖的个数，然后是遮盖的索引列表
        //，大小是上4个字节的大小乘4，最后就是遮盖的“真实”数据了。
        //4字节是遮盖的在地图上的坐标x
        //4字节是遮盖的在地图上的坐标y
        //4字节是遮盖的宽度
        //4字节是遮盖的高度
        //4字节是遮盖剩下部分数据的大小，具体大小在这用n来表示
        //n字节是我无法分析出来的，这里应该是遮盖的具体数据。
        //另外每个遮盖数据块的最后都是11 00 00

       public struct MaskHead
       {
           public UInt32 Unknown;
           public Int32 MaskNum;
           public UInt32[] MaskList; 
       }

       public struct MaskUnit
       {
           public Int32 StartX;
           public Int32 StartY;
           public Int32 Width;
           public Int32 Height;
           public Int32 MaskN;
           public byte[] MaskData;
       
       }

        //4字节地图单元引索的开始位置。
        //n*4字节n为上面的值，n为时不存在。

        //4字节GEPJ (JPEG)图象数据
        //4字节大小
        //n字节数据

        //4字节LLEC (CELL)地图规则，一字节代表一个游戏坐标
        //4字节大小
        //n字节数据

        //4字节BRIG (GIRB)光亮规则
        //4字节大小
        //n字节数据

        //4字节结束单元(0x00 0x00 0x00 0x00)。
       public struct Unit
       {
           public UInt32 UnitN;
           public Byte[] UnitNData;

           public UInt32 UnitFlag;
           public UInt32 Size;
           public Byte[] UnitData; //Size byte
       }

    }
}

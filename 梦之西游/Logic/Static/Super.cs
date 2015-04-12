using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Input;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using 梦之西游.Controls;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using 梦之西游.SpiritInfo;
using 梦之西游.Logic.Static;
using System.Collections;

namespace 梦之西游.Logic.Static
{
    public static class Super {

        #region 静态属性

        /// <summary>
        /// 游戏背景音乐
        /// </summary>
        public static MediaPlayer BackMusic = new MediaPlayer();
        public static Random random=new Random();

        /// <summary>
        /// 所有精灵的属性移动动画板
        /// </summary>
        public static Dictionary<string, Storyboard> storyboard = new Dictionary<string, Storyboard>();

        /// <summary>
        /// 游戏中的鼠标光标图片
        /// </summary>
        public static Cursor[] GameCursors = new Cursor[10];

        /// <summary>
        /// 将系统配置Config.xml加载进内存
        /// </summary>
        public static XElement SystemConfig;
        /// <summary>
        /// 缓存从System文件夹中的xml文件获取的XElement节段
        /// </summary>
        public static Dictionary<string, XElement> SystemXElement = new Dictionary<string, XElement>();
        /// <summary>
        /// 将系怪Monster.xml加载进内存
        /// </summary>
        public static XElement MonsterConfig;
        /// <summary>
        /// 缓存从怪Monster xml文件获取的XElement节段
        /// </summary>
        public static Dictionary<string, XElement> MonsterXElement = new Dictionary<string, XElement>();
        /// <summary>
        /// 各等级升级需要经验值表(参考魔兽世界)
        /// </summary>
        public static int[] LevelUpExperienceList;
        
        #endregion

        #region 静态方法

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="name"></param>
        public static void PlayBackGroundMusic(string name)
        {
            BackMusic.Open(new Uri("Audio/BGM/" + name, UriKind.Relative));
            BackMusic.Play();
        }

        /// <summary>
        /// 获取指定的图片
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BitmapImage GetImage(string sort, string name)
        {

            return new BitmapImage(new Uri(string.Format(@"Graphics/{0}/{1}.png", sort, name),UriKind.Relative));
        
        }


        /// <summary>
        /// 拼装角色+装备后切割成系列帧图片并保存进内存(装备角色)
        /// </summary>
        /// <param name="SourcePath">源文件路径</param>
        /// <param name="SavaToPath">保存文件到目标路径</param>
        /// <param name="imgnum">源图片数量</param>
        /// <param name="imgwidth">单位图片宽</param>
        /// <param name="imgheight">单位图片高</param>
        public static void ComposeImage(string SourcePath, string SavaToPath, int imgNum, int imgWidth, int imgHeight) {
            System.IO.FileStream fileStream = new System.IO.FileStream(SavaToPath, System.IO.FileMode.Create);
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            int count = 1;
            //for (int i = 0; i < 8; i++) {
            //    for (int j = 0; j < imgNum / 8; j++) {
            //        drawingContext.DrawImage(new BitmapImage(new Uri(SourcePath + count + "-1.png")), new Rect(j * imgWidth, i * imgHeight, imgWidth, imgHeight));
            //        count += 1;
            //    }
            //}
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < imgNum / 8; j++) {
                    if (i < 6) {
                        drawingContext.DrawImage(new BitmapImage(new Uri(string.Format("{0}{1}-1.png", SourcePath, count))), new Rect(j * imgWidth, (i + 2) * imgHeight, imgWidth, imgHeight));
                    } else if (i == 6) {
                        drawingContext.DrawImage(new BitmapImage(new Uri(string.Format("{0}{1}-1.png", SourcePath, count))), new Rect(j * imgWidth, 0, imgWidth, imgHeight));
                    } else if (i == 7) {
                        drawingContext.DrawImage(new BitmapImage(new Uri(string.Format("{0}{1}-1.png", SourcePath, count))), new Rect(j * imgWidth, imgHeight, imgWidth, imgHeight));
                    }
                    count += 1;
                }
            }
            drawingContext.Close();
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((imgNum / 8) * imgWidth, 8 * imgHeight, 0, 0, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(drawingVisual);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            encoder.Save(fileStream);
            fileStream.Close();
        }

     

        public static double GetAngle(double x, double y) {
            return Math.Atan2(x, y) / Math.PI * 180;
        }

        /// <summary>
        /// 通过正切值获取精灵的朝向代号
        /// </summary>
        /// <param name="targetX">目标点的X值</param>
        /// <param name="targetY">目标点的Y值</param>
        /// <param name="currentX">当前点的X值</param>
        /// <param name="currentY">当前点的Y值</param>
        /// <returns>精灵朝向代号(以北为0顺时针依次1,2,3,4,5,6,7)</returns>//
        public static int GetDirectionByTan(double targetX, double targetY, double currentX, double currentY) {
            double tan = (targetY - currentY) / (targetX - currentX);
            if (Math.Abs(tan) >= Math.Tan(Math.PI * 3 / 8) && targetY <= currentY) {
                return 6;//N
            } else if (Math.Abs(tan) > Math.Tan(Math.PI / 8) && Math.Abs(tan) < Math.Tan(Math.PI * 3 / 8) && targetX > currentX && targetY < currentY) {
                return 3;
            } else if (Math.Abs(tan) <= Math.Tan(Math.PI / 8) && targetX >= currentX) {
                return 7;//E
            } else if (Math.Abs(tan) > Math.Tan(Math.PI / 8) && Math.Abs(tan) < Math.Tan(Math.PI * 3 / 8) && targetX > currentX && targetY > currentY) {
                return 0;
            } else if (Math.Abs(tan) >= Math.Tan(Math.PI * 3 / 8) && targetY >= currentY) {
                return 4;//S
            } else if (Math.Abs(tan) > Math.Tan(Math.PI / 8) && Math.Abs(tan) < Math.Tan(Math.PI * 3 / 8) && targetX < currentX && targetY > currentY) {
                return 1;
            } else if (Math.Abs(tan) <= Math.Tan(Math.PI / 8) && targetX <= currentX) {
                return 5;//W
            } else if (Math.Abs(tan) > Math.Tan(Math.PI / 8) && Math.Abs(tan) < Math.Tan(Math.PI * 3 / 8) && targetX < currentX && targetY < currentY) {
                return 2;
            } else {
                return 4;
            }
        }

        /// <summary>
        /// 寻路模式中根据单元格方向来判断精灵朝向
        /// </summary>
        /// <param name="targetX">目标点的X值</param>
        /// <param name="targetY">目标点的Y值</param>
        /// <param name="currentX">当前点的X值</param>
        /// <param name="currentY">当前点的Y值</param>
        /// <returns>精灵朝向代号(以北为0顺时针依次1,2,3,4,5,6,7)</returns>
        public static int GetDirectionByAspect(int targetX, int targetY, int currentX, int currentY) {
            int direction = 2;
            if (targetX < currentX) {
                if (targetY < currentY) {
                    direction = 2;
                } else if (targetY == currentY) {
                    direction = 5;
                } else if (targetY > currentY) {
                    direction = 1;
                }
            } else if (targetX == currentX) {
                if (targetY < currentY) {
                    direction = 6;
                } else if (targetY > currentY) {
                    direction = 4;
                }
            } else if (targetX > currentX) {
                if (targetY < currentY) {
                    direction =3;
                } else if (targetY == currentY) {
                    direction = 7;
                } else if (targetY > currentY) {
                    direction = 0;
                }
            }
            return direction;
        }

     


        /// <summary>
        /// 判断点是否在多边形内
        /// </summary>
        /// <param name="range">多边形顶点范围</param>
        /// <param name="target">要判断的点</param>
        /// <returns></returns>
        public static bool PointInPolygon(System.Drawing.Point[] range, System.Drawing.Point target) {
            System.Drawing.Drawing2D.GraphicsPath myGraphicsPath = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Region myRegion = new System.Drawing.Region();
            myGraphicsPath.Reset();
            myGraphicsPath.AddPolygon(range);
            myRegion.MakeEmpty();
            myRegion.Union(myGraphicsPath);
            return myRegion.IsVisible(target);
        }

        /// <summary>
        /// 获取XML文件树节点
        /// </summary>
        /// <param name="xml">XML文件载体</param>
        /// <param name="mainnode">要查找的主节点</param>
        /// <param name="attribute">主节点条件属性名</param>
        /// <param name="value">主节点条件属性值</param>
        /// <returns>以该主节点为根的XElement</returns>
        public static XElement GetTreeNode(XElement XML, string newroot, string attribute, string value) {
            return XML.DescendantsAndSelf(newroot).Single(X => X.Attribute(attribute).Value == value);
        }

        /// <summary>
        /// 获取XML文件树节点
        /// </summary>
        /// <param name="XML">XML文件载体</param>
        /// <param name="newroot">要查找的独立节点</param>
        /// <returns>独立节点XElement</returns>
        public static XElement GetTreeNode(XElement XML, string newroot) {
            return XML.DescendantsAndSelf(newroot).Single();
        }

        /// <summary>
        /// 加载XElement
        /// </summary>
        /// <param name="key">字典中的键名</param>
        /// <param name="element">XElement</param>
        public static void LoadXElement(string key, XElement element) {
            if (!SystemXElement.ContainsKey(key)) {
                SystemXElement.Add(key, element);
            }
        }

        /// <summary>
        /// 加载XElement
        /// </summary>
        /// <param name="key">字典中的键名</param>
        /// <param name="element">XElement</param>
        public static void LoadXElement(string key, XElement element,Dictionary<string, XElement> Dname)
        {
            if (!Dname.ContainsKey(key))
            {
                Dname.Add(key, element);
            }
        }
        /// <summary>
        /// 获取系统参数节点段XElement
        /// </summary>
        /// <param name="key">字典中的键名</param>
        /// <returns>XElement</returns>
        public static XElement GetXElement(string key) {
            return SystemXElement[key];
        }
        /// <summary>
        /// 获取系统参数节点段XElement
        /// </summary>
        /// <param name="key">字典中的键名</param>
        /// <returns>XElement</returns>
        public static XElement GetXElement(string key, Dictionary<string, XElement> Dname)
        {
            return Dname[key];
        }

        /// <summary>
        /// 返回指定标号光标
        /// </summary>
        /// <param name="sign">标号</param>
        /// <returns>光标</returns>
        public static Cursor getCursor(int sign) {
            if (GameCursors[sign] == null) {
                GameCursors[sign] = new Cursor(new FileStream(string.Format(@"Cursors\{0}.ani", sign), FileMode.Open, FileAccess.Read, FileShare.Read));
            }
            return GameCursors[sign];
        }

     
        /// <summary>
        /// 从项目相对路径加载图片并截取指定部分
        /// </summary>
        /// <param name="address">文件地址(包括文件名及扩展名)</param>
        /// <param name="x">左上角点X</param>
        /// <param name="y">左上角点Y</param>
        /// <param name="width">截取的图片宽</param>
        /// <param name="height">截取的图片高</param>
        /// <returns>截取后的图片源</returns>
        public static BitmapSource getImage(string address, int x, int y, int width, int height) {
            return new CroppedBitmap(BitmapFrame.Create(new Uri(string.Format(@"{0}", address), UriKind.Relative)), new Int32Rect(x, y, width, height));
        }

        /// <summary>
        /// 创建彩虹笔刷
        /// </summary>
        /// <param name="brush">笔刷</param>
        /// <param name="r0">渐变起步红</param>
        /// <param name="g0">渐变起步绿</param>
        /// <param name="b0">渐变起步蓝</param>
        /// <param name="p0">渐变起步点</param>
        /// <param name="r1">渐变转折红</param>
        /// <param name="g1">渐变转折绿</param>
        /// <param name="b1">渐变转折蓝</param>
        /// <param name="p1">渐变转折点</param>
        /// <param name="r2">渐变结束红</param>
        /// <param name="g2">渐变结束绿</param>
        /// <param name="b2">渐变结束蓝</param>
        /// <param name="p2">渐变结束点</param>
        public static LinearGradientBrush CreateRainbowBrush(byte r0, byte g0, byte b0, double p0, byte r1, byte g1, byte b1, double p1, byte r2, byte g2, byte b2, double p2) {
            LinearGradientBrush rainbowBrush = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(0, 1) };
            rainbowBrush.GradientStops.Add(new GradientStop(Color.FromRgb(r0, g0, b0), p0));
            rainbowBrush.GradientStops.Add(new GradientStop(Color.FromRgb(r1, g1, b1), p1));
            rainbowBrush.GradientStops.Add(new GradientStop(Color.FromRgb(r2, g2, b2), p2));
            rainbowBrush.Freeze();
            return rainbowBrush;
        }

        /// <summary>
        /// 判断点是否在圆内
        /// </summary>
        /// <param name="targetX">目标点X坐标</param>
        /// <param name="targetY">目标点Y坐标</param>
        /// <param name="centerX">圆心X坐标</param>
        /// <param name="centerY">圆心X坐标</param>
        /// <param name="radius">圆半径</param>
        /// <returns></returns>
        public static bool InCircle(double targetX, double targetY, double centerX, double centerY, double radius) {
            return Math.Pow(targetX - centerX, 2) + Math.Pow(targetY - centerY, 2) <= Math.Pow(radius, 2) ? true : false;
        }

      
     


        /// <summary>
        /// 创建新的精灵Storyboard移动动画板
        /// </summary>
        /// <param name="key">精灵名</param>
        public static void NewSpiritStoryboard(string key) {
            if (!storyboard.ContainsKey(key)) {
                storyboard.Add(key, new Storyboard());
            }
        }

        /// <summary>
        /// 获取精灵Storyboard移动动画板
        /// </summary>
        /// <param name="key">字典中的键名(精灵名)</param>
        /// <returns>动画板</returns>
        public static Storyboard GetStoryboard(string key) {
            return storyboard.Single(X => X.Key == key).Value;
        }

        /// <summary>
        /// 暂停精灵Storyboard移动动画板
        /// </summary>
        /// <param name="spirit">对象精灵</param>
        public static void PauseSpiritStoryboard(Spirit spirit) {
            if (storyboard.ContainsKey(spirit.Name)) {
                GetStoryboard(spirit.Name).Pause();
                storyboard.Remove(spirit.Name);
            }
        }

        /// <summary>
        /// 获取一个指定的怪物
        /// </summary>
        /// <param name="sortName"></param>
        /// <returns></returns>
        public static SpiritObject GetMonster(string sortName)
        {

            Super.LoadXElement(sortName, Super.GetTreeNode(Super.MonsterConfig, "Monster", "SortName", sortName), Super.MonsterXElement);
            int takeLevel = int.Parse(Super.GetXElement(sortName,Super.MonsterXElement).Element("Property").Attribute("TakeLevel").Value);
            GG.SpiritTypes type=(GG.SpiritTypes)int.Parse(Super.GetXElement(sortName,Super.MonsterXElement).Element("Property").Attribute("Type").Value);
            string name=Super.GetXElement(sortName,Super.MonsterXElement).Element("Property").Attribute("SName").Value;
            string faceimage=Super.GetXElement(sortName,Super.MonsterXElement).Element("Property").Attribute("FaceImage").Value;


            Dictionary<int, GG.ImageInfo> ImagePath = new Dictionary<int, GG.ImageInfo>();
            IEnumerable ActionInfos = Super.GetXElement(sortName, Super.MonsterXElement).Element("ActionInfos").Elements();
            foreach (XElement xe in ActionInfos)
            {
                 ImagePath.Add(int.Parse(xe.Attribute("Action").Value),
                     new GG.ImageInfo(xe.Attribute("Name").Value,int.Parse(xe.Attribute("WFrameNum").Value),int.Parse(xe.Attribute("HFrameNum").Value),
                         int.Parse(xe.Attribute("StartX").Value),int.Parse(xe.Attribute("StartY").Value),
                         int.Parse(xe.Attribute("EndX").Value),int.Parse(xe.Attribute("EndY").Value)));
            
            }



            SpiritObject SO = new SpiritObject();
            SO.UID = GetUID();
            SO.TakeLevel = takeLevel;
            SO.SType=type;
            SO.Direction=0;
            SO.Level=random.Next(takeLevel+1,takeLevel+6);
            SO.Name=name;
            SO.FaceImage=faceimage;
            SO.TZ = SO.Level + random.Next(10, 20);
            SO.ML = SO.Level + random.Next(10, 20);
            SO.LL = SO.Level + random.Next(10, 20);
            SO.NL = SO.Level + random.Next(10, 20);
            SO.MJ = SO.Level + random.Next(10, 20);
            SO.QL=0;

            SO.MaxLife=(int)(SO.TZ*GG.RTZ);
            SO.NowLife=(int)(SO.TZ*GG.RTZ);
            SO.MaxMagic=(int)(SO.ML*GG.RML);
            SO.NowMagic=(int)(SO.ML*GG.RML);
            SO.Hurt=(int)(SO.LL*GG.RLL);
            SO.Defense=(int)(SO.NL*GG.RNL);
            SO.Speed=(int)(SO.MJ*GG.RMJ);
            SO.Wakan=(int)(SO.MJ*GG.RWakan);

           SO.LevelUpExp = SO.Level * SO.Level * GG.RLeveUpExp+100;
           SO.NowExp = 0;
           SO.SortName=sortName;
           SO.Loyal=100;

           SO.ImagePath = ImagePath;

            return SO;
        }

        /// <summary>
        /// 获取一个唯一的UID
        /// </summary>
        /// <returns></returns>
        public static string GetUID()
        {

            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString();
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
        #endregion
    }
}

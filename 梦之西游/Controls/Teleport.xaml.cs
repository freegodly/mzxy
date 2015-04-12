using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using 梦之西游.Logic.Static;

namespace 梦之西游.Controls
{
    /// <summary>
    /// Teleport.xaml 的交互逻辑
    /// </summary>
    public partial class Teleport : UserControl,GameObject 
    {
        public DispatcherTimer timer;
        private string ResName;
        private int WFrameNum;
        private int HFrameNum;
        private int StartX;
        private int StartY;
        private int EndX;
        private int EndY;
        private int w;
        private int h;
        private BitmapSource[,] PhotoSource;
        private int PhotoDirectionNum = 0;
        private int PhotoFrameNum = 0;
        private int PhotoNum = 0;
        public Teleport(string name,int w,int h,int sx,int sy,int ex,int ey,int code,int x,int y,int tox,int toy,string tip)
        {
            InitializeComponent();
            ResName = name;
            WFrameNum = w;
            HFrameNum = h;
            StartX = sx;
            StartY = sy;
            EndX = ex;
            EndY = ey;
            Code = code;
            X = x;
            Y = y;
            ToX = tox;
            ToY = toy;
            Tip = tip;


            LoadImage();
            timer = new DispatcherTimer(DispatcherPriority.Normal, this.Dispatcher);
            timer.Interval = TimeSpan.FromMilliseconds(150);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void LoadImage()
        {
            BitmapImage NowUsedImage = Super.GetImage("Windowskins", "90传送点");
            int SingleWidth = (int)(EndX - StartX) / WFrameNum;
            int SingleHeight = (int)((EndY - StartY) /HFrameNum);
            w = SingleWidth;
            h = SingleHeight;
            h += 20;
            Width_ = w ;
            Height_ = h;
            CenterX = w / 2;
            CenterY = h - 20;
            PhotoDirectionNum = HFrameNum;
            PhotoFrameNum = WFrameNum;
            PhotoSource = new BitmapSource[HFrameNum, WFrameNum];
            for (int i = 0; i < HFrameNum; i++)
            {
                for (int j = 0; j < WFrameNum; j++)
                {
                   
                    PhotoSource[i, j] = new CroppedBitmap(NowUsedImage, new Int32Rect(j * SingleWidth + StartX, i * SingleHeight + StartY, SingleWidth, SingleHeight));
                }
            }
        }
        public string Tip
        {
            get { return Details.Text; }
            set { Details.Text = value; }
        }

       

        public double ToX { get; set; }

        public double ToY { get; set; }

        public int ToDirection { get; set; }

        /// <summary>
        /// 获取或设置帧推进器
        /// </summary>
        public int FrameCounter { get; set; }

        /// <summary>
        /// 获取或设置当前开始图片列号
        /// </summary>
        public int CurrentStartFrame { get; set; }

        /// <summary>
        /// 获取或设置当前结束图片列号
        /// </summary>
        public int CurrentEndFrame { get; set; }

        /// <summary>
        /// 获取或设置代号
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 获取或设置朝向
        /// </summary>
        public int Direction { get; set; }

        /// <summary>
        /// 获取或设置中心点距离左边X坐标
        /// </summary>
        public double CenterX { get; set; }

        /// <summary>
        /// 获取或设置中心点距离顶边Y坐标
        /// </summary>
        public double CenterY { get; set; }

        /// <summary>
        /// 获取或设置实际宽
        /// </summary>
        public double Width_ { get; set; }

        /// <summary>
        /// 获取或设置实际高
        /// </summary>
        public double Height_ { get; set; }

        /// <summary>
        /// 获取或设置X坐标(关联属性)
        /// </summary>
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X",
            typeof(double),
            typeof(Teleport)
        );

        /// <summary>
        /// 获取或设置Y坐标(关联属性)
        /// </summary>
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y",
            typeof(double),
            typeof(Teleport)
        );

        /// <summary>
        /// 获取或设置与父画布容器左边距离
        /// </summary>
        public double Left
        {
            get { return (double)this.GetValue(Canvas.LeftProperty); }
            set { this.SetValue(Canvas.LeftProperty, value); }
        }

        /// <summary>
        /// 获取或设置与父画布容器顶边距离
        /// </summary>
        public double Top
        {
            get { return (double)this.GetValue(Canvas.TopProperty); }
            set { this.SetValue(Canvas.TopProperty, value); }
        }

        /// <summary>
        /// 获取或设置层次
        /// </summary>
        public int ZIndex
        {
            get { return (int)this.GetValue(Canvas.ZIndexProperty); }
            set { this.SetValue(Canvas.ZIndexProperty, value); }
        }

        /// <summary>
        /// 线程间隔事件
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            PhotoNum = PhotoNum >= PhotoFrameNum - 1 ? 0 : PhotoNum + 1;
            Body.Source = PhotoSource[0, PhotoNum];
        }
    }
}

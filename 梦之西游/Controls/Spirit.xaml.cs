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
using System.Windows.Media.Animation;
using AStar;
using System.Windows.Threading;
using System.Threading;
using 梦之西游.Logic.Static;
using 梦之西游.SpiritInfo;
namespace 梦之西游.Controls
{
    /// <summary>
    /// Spirit.xaml 的交互逻辑
    /// </summary>
    public partial class Spirit : UserControl, GameObject
    {
        
        DispatcherTimer SpiritTimer;
        public Dictionary<int, GG.ImageInfo> ImagePathTable;
        private BitmapSource[,] PhotoSource;
        private BitmapSource Shadow;
        private int OldAcion=-1;

        private int PhotoDirectionNum = 0;
        
        public Spirit(string name, Point location, Dictionary<int, GG.ImageInfo> imagepathtable, int direction,GG.SpiritTypes type)
        {
            InitializeComponent();
            Type = type;
            VRunSpeed = 200;
            VSName = name;
            HoldHeight = 1;
            HoldWidth = 1;
            X = location.X;
            Y = location.Y;
            Direction = direction;
            this.ImagePathTable = imagepathtable;
            VSNameColor = Brushes.Yellow;

            Action = (int)GG.Actions.Stand;
            Shadow = Super.GetImage("Windowskins", "shadow");
            SpiritTimer = new DispatcherTimer();
            SpiritTimer.Interval = TimeSpan.FromMilliseconds(200);
            SpiritTimer.Tick += new EventHandler(Timer_Tick);
            SpiritTimer.Start();
        }

        public Spirit(SpiritObject so)
        {
            InitializeComponent();
            this.SInfo = so;
        }
      
        #region 身份属性

        private SpiritObject SpiritInfo;

        /// <summary>
        /// 精灵详细信息
        /// </summary>
        public SpiritObject SInfo
        {
            get
            {
              return  SpiritInfo;
            }
            set 
            {
                SpiritInfo = value;
                Direction = SInfo.Direction;
                Type = SInfo.SType;
                VRunSpeed = 160;
                VSName = SInfo.Name;
                HoldHeight = 1;
                HoldWidth = 1;
                X = SInfo.Location.X;
                Y = SInfo.Location.Y;
                this.ImagePathTable = SInfo.ImagePath;
                if (SInfo.SType == GG.SpiritTypes.Leader)
                {
                    VSNameColor = Brushes.Green;
                }
                else if (SInfo.SType == GG.SpiritTypes.NPC)
                {
                    VSNameColor = Brushes.Yellow;
                }

                Action = (int)GG.Actions.Stand;
                Shadow = Super.GetImage("Windowskins", "shadow");
                SpiritTimer = new DispatcherTimer();
                SpiritTimer.Interval = TimeSpan.FromMilliseconds(200);
                SpiritTimer.Tick += new EventHandler(Timer_Tick);
                SpiritTimer.Start(); 
            }
        }

        /// <summary>
        /// 获取或设置姓名
        /// </summary>
        public string VSName
        {
            get { return SName.Text; }
            set { SName.Text = value; }
        }
        /// <summary>
        /// 移动速度
        /// </summary>
        public int VRunSpeed { get; set; }

        /// <summary>
        /// 获取或设置锁定的目标精灵对象
        /// </summary>
        public Spirit LockSpirit { get; set; }

        /// <summary>
        /// 获取或设置精灵当前动作
        /// </summary>
        public int Action
        {
            get { return (int)GetValue(ActionProperty); }
            set 
            {
                SetValue(ActionProperty, value);ChangeAction(); 
            }
            //get { return _Action; }
            //set { if (_Action != value) { _Action = value; ChangeAction(); } }
        }

        public static readonly DependencyProperty ActionProperty = DependencyProperty.Register(
         "Action",
         typeof(int),
         typeof(Spirit));
        /// <summary>
        /// 精灵战斗行动
        /// </summary>
        public GG.SpiritFightAction FightAction { get; set; }

        /// <summary>
        /// 获取或设置精灵类型
        /// </summary>
        public GG.SpiritTypes Type { get; set; }

        /// <summary>
        /// 获取或设置精灵移动目的地
        /// </summary>
        public Point Destination { get; set; }

        /// <summary>
        /// 获取或设置精灵的魔法攻击目标坐标
        /// </summary>
        public Point MagicTarget { get; set; }

        /// <summary>
        /// 获取或设置是否为移动后施法过程
        /// </summary>
        public bool IsMagicMove { get; set; }

        /// <summary>
        /// 获取或设置精灵当前准备或右键魔法相关参数
        /// [0],代号
        /// [1],等级
        /// </summary>
        public int[] MagicArgs { get; set; }

        /// <summary>
        /// 施法距离
        /// </summary>
        public int MagicRange { get; set; }

        /// <summary>
        /// 获取或设置精灵装备代码
        /// [0],衣服
        /// [1],武器
        /// </summary>
        public int[] Equipment { get; set; }

        /// <summary>
        /// 获取或设置帧推进器
        /// </summary>
        public int FrameCounter { get; set; }

        /// <summary>
        /// 获取或设置精灵当前动作结束图片列号
        /// </summary>
        public int FrameNum { get; set; }

        /// <summary>
        /// 获取或设置各动作产生实际效果的针序号
        /// </summary>
        public int[] EffectiveFrame { get; set; }

        /// <summary>
        /// 设置精灵生命线程状态
        /// </summary>
        public GG.TimerStates TimerState
        {
            set
            {
                switch (value)
                {
                    case GG.TimerStates.Start:
                        SpiritTimer.IsEnabled = true;
                        break;
                    case GG.TimerStates.Stop:
                        SpiritTimer.IsEnabled = false;
                        break;
                }
            }
        }

        #endregion
       
       
        #region 界面属性


        /// <summary>
        /// 获取或设置精灵身体透明度
        /// </summary>
        public double BodyOpacity
        {
            get { return Body.Opacity; }
            set { Body.Opacity = value; }
        }

        /// <summary>
        /// 设置头顶生命值条颜色笔刷
        /// </summary>
        public Brush LifeBrush
        {
            set { Life.Fill = value; }
        }

        public Brush VSNameColor
        {
            get { return SName.Foreground; }
            set { SName.Foreground = value; }
        }
        /// <summary>
        /// 获取或设置头顶生命值条当前宽度
        /// </summary>
        public double LifeWidth
        {
            get { return Life.Width; }
            set { if (LifeWidth != value) { Life.Width = value; } }
        }

    
       

        /// <summary>
        /// 获取或设置精灵单位图片左上角点与精灵图片中角色脚底的X距离
        /// </summary>
        public double CenterX { get; set; }

        /// <summary>
        /// 获取或设置精灵单位图片左上角点与精灵图片中角色脚底的Y距离
        /// </summary>
        public double CenterY { get; set; }

        /// <summary>
        /// 获取或设置精灵X坐标(关联属性)
        /// </summary>
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", //属性名
            typeof(double), //属性类型
            typeof(Spirit) //属性主人类型
        );

        /// <summary>
        /// 获取或设置精灵Y坐标(关联属性)
        /// </summary>
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y",
            typeof(double),
            typeof(Spirit)
        );
        /// <summary>
        /// 获取或设置精灵X坐标(关联属性)
        /// </summary>
        public double FX
        {
            get { return (double)GetValue(FXProperty); }
            set { SetValue(FXProperty, value); }
        }
        public static readonly DependencyProperty FXProperty = DependencyProperty.Register(
            "FX", //属性名
            typeof(double), //属性类型
            typeof(Spirit) //属性主人类型
        );

        /// <summary>
        /// 获取或设置精灵Y坐标(关联属性)
        /// </summary>
        public double FY
        {
            get { return (double)GetValue(FYProperty); }
            set { SetValue(FYProperty, value); }
        }
        public static readonly DependencyProperty FYProperty = DependencyProperty.Register(
            "FY",
            typeof(double),
            typeof(Spirit)
        );

        /// <summary>
        ///  获取或设置精灵的方向
        /// </summary>
        public int Direction
        {   
            get { return (int)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Direction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(int), typeof(Spirit), new UIPropertyMetadata());

        

        /// <summary>
        /// 获取或设置精灵与父画布容器左边距离
        /// </summary>
        public double Left
        {
            get { return (double)this.GetValue(Canvas.LeftProperty); }
            set { this.SetValue(Canvas.LeftProperty, value); }
        }
        /// <summary>
        /// 获取或设置精灵与父画布容器顶边距离
        /// </summary>
        public double Top 
        {
            get { return (double)this.GetValue(Canvas.TopProperty); }
            set { this.SetValue(Canvas.TopProperty, value); }
        }
        /// <summary>
        /// 获取或设置精灵层次
        /// </summary>
        public int ZIndex
        {
            get { return (int)this.GetValue(Canvas.ZIndexProperty); }
            set { this.SetValue(Canvas.ZIndexProperty, value); }
        }

        /// <summary>
        /// 获取或设置精灵图片宽
        /// </summary>
        public double Width_
        {
            get { return Body.Width; }
            set { Body.Width = value;  }
        }

        /// <summary>
        /// 获取或设置精灵图片高
        /// </summary>
        public double Height_
        {
            get { return Body.Height; }
            set { Body.Height = value;}
        }

        /// <summary>
        /// 精灵X方向缩放
        /// </summary>
        public double RatioX { get; set; }

        /// <summary>
        /// 精灵Y方向缩放
        /// </summary>
        public double RatioY { get; set; }

       

        /// <summary>
        /// 获取或设置脚底示为障碍物区域扩展宽度
        /// </summary>
        public int HoldWidth { get; set; }

        /// <summary>
        /// 获取或设置脚底示为障碍物区域扩展高度
        /// </summary>
        public int HoldHeight { get; set; }

        /// <summary>
        /// 获取或设置精灵图片实体部分(共4个部分x1-x2,y1-y2)
        /// </summary>
        public double[] EfficaciousSection { get; set; }



       

        /// <summary>
        /// 获取或设置精灵各动作对应的帧列范围(暂时只有5个动作)
        /// </summary>
        public int[] EachActionFrameRange { get; set; }

        /// <summary>
        /// 获取或设置精灵单张图片宽
        /// </summary>
        public int SingleWidth { get; set; }

        /// <summary>
        /// 获取或设置精灵单张图片高
        /// </summary>
        public int SingleHeight { get; set; }

        /// <summary>
        /// 获取或设置每张精灵合成大图总宽
        /// </summary>
        public int TotalWidth { get; set; }

        /// <summary>
        /// 获取或设置每张精灵合成大图总高
        /// </summary>
        public int TotalHeight { get; set; }

       

     
        /// <summary>
        /// 设置人名是否显示
        /// </summary>
        public Visibility NameVisibility
        {
            get { return SName.Visibility; }
            set { SName.Visibility = value; }
        }


        /// <summary>
        /// 获取或设置精灵生命线程底层线程级别
        /// </summary>
        public ThreadPriority ThreadLevel
        {
            get { return SpiritTimer.Dispatcher.Thread.Priority; }
            set { if (ThreadLevel != value) { SpiritTimer.Dispatcher.Thread.Priority = value; } }
        }

        #endregion

        #region 方法与事件

        /// <summary>
        /// 精灵线程间隔事件
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            ChangeAction();
            //跟新人物显示帧
            FrameCounter = FrameCounter >= FrameNum - 1 ? 0 : FrameCounter + 1;
           if(PhotoSource.GetLength(0)>Direction-1) Body.Source = PhotoSource[Direction, FrameCounter];

            //跟新生命值
            if (this.SInfo != null )
            {
                Life.Width = this.Width * (SInfo.NowLife / SInfo.MaxLife);
            }
        }
        /// <summary>
        /// 改变动作
        /// </summary>
        private void ChangeAction()
        {
            if (OldAcion != Action)
            {
                OldAcion = Action;
                LoadingImage(Action);
            }
        }
        private void LoadingImage(int key)
        {
           
            if (Type == GG.SpiritTypes.NPC)
            {
                ReadNPCImage(key);
            }
            else if (Type == GG.SpiritTypes.Leader)
            {
                ReadLeaderImage(key);
            }
            else 
            {
                ReadMonsterImage(key);
            }
           
            UpdateSpirit();
        }
        /// <summary>
        /// 跟新信息
        /// </summary>
        private void UpdateSpirit()
        {
            this.Width = Width_;
            this.Height = Height_ + 20;
            CenterX = this.Width / 2;
            CenterY = this.Height - 20;
            FrameCounter = 0;
        }

        private void ReadNPCImage(int key)
        {
            BitmapImage NowUsedImage =Super.GetImage("NPC",ImagePathTable[key].SpiritImageName);
            if (NowUsedImage == null) new Exception("初始化人物图片失败!");
            int SingleWidth = (int)((ImagePathTable[key].EndX - ImagePathTable[key].StartX) / ImagePathTable[key].WFrameNum);
            int SingleHeight = (int)((ImagePathTable[key].EndY - ImagePathTable[key].StartY) / ImagePathTable[key].HFrameNum);
            Width_ = SingleWidth;
            Height_ = SingleHeight;
            PhotoDirectionNum = ImagePathTable[key].HFrameNum;
            FrameNum = ImagePathTable[key].WFrameNum;
            PhotoSource = new BitmapSource[ImagePathTable[key].HFrameNum, ImagePathTable[key].WFrameNum];
            for (int i = 0; i < ImagePathTable[key].HFrameNum; i++)
            {
                for (int j = 0; j < ImagePathTable[key].WFrameNum; j++)
                {
                    DrawingVisual DV = new DrawingVisual();
                    DrawingContext drawingContext = DV.RenderOpen();
                    //drawingContext.DrawImage(Shadow, new Rect(w / 2 - 51 / 2, h - 41, 51, 41));
                    drawingContext.DrawImage(new CroppedBitmap(NowUsedImage, new Int32Rect(j * SingleWidth + ImagePathTable[key].StartX, i * SingleHeight + ImagePathTable[key].StartY, SingleWidth, SingleHeight)), new Rect(Width_ / 2 - SingleHeight / 2 + 15, Height_ - SingleHeight - 12, SingleWidth, SingleHeight));
                    drawingContext.Close();
                    RenderTargetBitmap composeImage = new RenderTargetBitmap((int)Width_, (int)Height_, 0, 0, PixelFormats.Pbgra32);
                    composeImage.Render(DV);
                    PhotoSource[i, j] = composeImage;
                }
            }

            EfficaciousSection = new double[] { 20, Width_ - 20, 25, Height_ - 20 };
        }
        private void ReadLeaderImage(int key)
        {
            BitmapImage NowUsedImage = Super.GetImage("Hero", ImagePathTable[key].SpiritImageName);
            if (NowUsedImage == null) new Exception("初始化人物图片失败!");
            int SingleWidth = (int)(NowUsedImage.PixelWidth / ImagePathTable[key].WFrameNum);
            int SingleHeight = (int)(NowUsedImage.PixelHeight / ImagePathTable[key].HFrameNum);
            Width_ = SingleWidth;
            Height_ = SingleHeight;
            BitmapImage WeaponImage = null;
            int WeaponW = 0;
            int WeaponH = 0;
            if (ImagePathTable[key].Weapom != null) //有武器
            {
                WeaponImage = Super.GetImage("Hero", ImagePathTable[key].Weapom);
                WeaponW = (int)(WeaponImage.PixelWidth / ImagePathTable[key].WFrameNum);
                WeaponH = (int)(WeaponImage.PixelHeight / ImagePathTable[key].HFrameNum);
                Width_ = Width_ > WeaponW ? Width_ : WeaponW;
                Height_ = Height_ > WeaponH ? Height_ : WeaponH;
            }
            Height_ += 20;
            Width_ = Width_;
            Height_ = Height_;
            CenterX = Width_ / 2;
            CenterY = Height_ - 20;
            PhotoDirectionNum = ImagePathTable[key].HFrameNum;
            FrameNum = ImagePathTable[key].WFrameNum;
            PhotoSource = new BitmapSource[ImagePathTable[key].HFrameNum, ImagePathTable[key].WFrameNum];
            for (int i = 0; i < ImagePathTable[key].HFrameNum; i++)
            {
                for (int j = 0; j < ImagePathTable[key].WFrameNum; j++)
                {
                    if (WeaponImage != null)
                    {

                        DrawingVisual DV = new DrawingVisual();
                        DrawingContext drawingContext = DV.RenderOpen();
                        drawingContext.DrawImage(Shadow, new Rect(Width_ / 2 - 51 / 2, Height_ - 45, 51, 41));
                        drawingContext.DrawImage(new CroppedBitmap(NowUsedImage, new Int32Rect(j * SingleWidth, i * SingleHeight, SingleWidth, SingleHeight)), new Rect(Width_ / 2 - SingleHeight / 2 + 15, Height_ - SingleHeight - 12, SingleWidth, SingleHeight));
                        drawingContext.DrawImage(new CroppedBitmap(WeaponImage, new Int32Rect(j * WeaponW, i * WeaponH, WeaponW, WeaponH)), new Rect(Width_ / 2 - WeaponW / 2, Height_ - WeaponH - 15, WeaponW, WeaponH));

                        drawingContext.Close();
                        RenderTargetBitmap composeImage = new RenderTargetBitmap((int)Width_, (int)Height_, 0, 0, PixelFormats.Pbgra32);
                        composeImage.Render(DV);
                        PhotoSource[i, j] = composeImage;
                    }
                    else
                    {
                        DrawingVisual DV = new DrawingVisual();
                        DrawingContext drawingContext = DV.RenderOpen();
                        drawingContext.DrawImage(Shadow, new Rect(Width_ / 2 - 51 / 2, Height_ - 41, 55, 41));
                        drawingContext.DrawImage(new CroppedBitmap(NowUsedImage, new Int32Rect(j * SingleWidth, i * SingleHeight, SingleWidth, SingleHeight)), new Rect(Width_ / 2 - SingleHeight / 2 + 15, Height_ - SingleHeight - 12, SingleWidth, SingleHeight));
                        drawingContext.Close();
                        RenderTargetBitmap composeImage = new RenderTargetBitmap((int)Width_, (int)Height_, 0, 0, PixelFormats.Pbgra32);
                        composeImage.Render(DV);
                        PhotoSource[i, j] = composeImage;
                    }
                }
            }
            EfficaciousSection = new double[] { 20, Width_ - 20, 25, Height_ - 20 };
        }
        private void ReadMonsterImage(int key)
        {
            BitmapImage NowUsedImage = Super.GetImage("Monster", ImagePathTable[key].SpiritImageName);
            if (NowUsedImage == null) new Exception("初始化人物图片失败!");
            int SingleWidth = (int)((ImagePathTable[key].EndX - ImagePathTable[key].StartX) / ImagePathTable[key].WFrameNum);
            int SingleHeight = (int)((ImagePathTable[key].EndY - ImagePathTable[key].StartY) / ImagePathTable[key].HFrameNum);
            Width_ = SingleWidth;
            Height_ = SingleHeight;
            PhotoDirectionNum = ImagePathTable[key].HFrameNum;
            FrameNum = ImagePathTable[key].WFrameNum;
            PhotoSource = new BitmapSource[ImagePathTable[key].HFrameNum, ImagePathTable[key].WFrameNum];
            for (int i = 0; i < ImagePathTable[key].HFrameNum; i++)
            {
                for (int j = 0; j < ImagePathTable[key].WFrameNum; j++)
                {
                    DrawingVisual DV = new DrawingVisual();
                    DrawingContext drawingContext = DV.RenderOpen();
                    //drawingContext.DrawImage(Shadow, new Rect(w / 2 - 51 / 2, h - 41, 51, 41));
                    drawingContext.DrawImage(new CroppedBitmap(NowUsedImage, new Int32Rect(j * SingleWidth + ImagePathTable[key].StartX, i * SingleHeight + ImagePathTable[key].StartY, SingleWidth, SingleHeight)), new Rect(0, 0, SingleWidth, SingleHeight));
                    drawingContext.Close();
                    RenderTargetBitmap composeImage = new RenderTargetBitmap((int)Width_, (int)Height_, 0, 0, PixelFormats.Pbgra32);
                    composeImage.Render(DV);
                    PhotoSource[i, j] = composeImage;
                }
            }
            EfficaciousSection = new double[] { 10, Width_ - 10, 10, Height_ - 10 };
        }

        public  void ToStand()
        {
            throw new NotImplementedException();
        }

        public  void ToRun(Point stopPoint)
        {
            throw new NotImplementedException();
        }

        public void ToAttack(Spirit targetBiont)
        {
            throw new NotImplementedException();
        }

        public void ToMagic(Spirit[] targetBionts, int magicId)
        {
            throw new NotImplementedException();
        }

        public  void ToDeath()
        {
            throw new NotImplementedException();
        }
       
        #endregion


       
    }
}

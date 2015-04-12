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
using System.Xml.Linq;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Media.Animation;
using 梦之西游.Controls;
using AStar;
using 梦之西游.Logic.Static;
using System.Threading;
using 梦之西游.SpiritInfo;
using System.IO;
using System.Media;

namespace 梦之西游
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Page
    {
       
        public static Spirit Hero;

        GameMap gameMap;
        Spirit[] npc;
        /// <summary>
        /// 战斗敌人列表
        /// </summary>
        List<Spirit> MonsterFightingList=new List<Spirit>();
        /// <summary>
        /// 战斗有方列表
        /// </summary>
        List<Spirit> HeroFightingList = new List<Spirit>();
        Teleport[] teleport;
        HeroStateShow heroStateShow;
        MapHeadShow mapHeadShow;
        GameMenuShow gameMenuShow;

        HeroPropertyShow heroPropertyShow;
        MonsterPropertyShow monsterPropertyShow;
        PackageShow packageShow;
        SystemSettingShow systemSettingShow;
        MessageShow messageShow;
        BattleButtonsShow battleButtonsShow;

        int mapCode = 0;
        GG.GameStateType GameState;
        Spirit SelectSpirit;
        /// <summary>
        /// 回合准备
        /// </summary>
        bool BoutReadIsOk=true;
        /// <summary>
        /// 记录Hero的信息
        /// </summary>
        SpiritObject SO;
       
        public MainWindow()
        {
           
            InitializeComponent();
            this.AllowDrop = true;
            InitializeGameArgs();
            InitializeGameSetting();
            InitializeGameTool();
            InitializeMap();
            InitHero();
           
           
        }

        #region 游戏系统参数初始化

        
        /// <summary>
        /// 初始化游戏参数
        /// </summary>
        private void InitializeGameArgs()
        {
            FileStream fs = File.Open("Hero.Data", FileMode.Open);
            byte[] b = new byte[fs.Length];
            fs.Read(b, 0, (int)fs.Length);
            fs.Close();
            fs.Dispose();
            SO = (SpiritObject)Super.BytesToObject(b);
            SO.TZ = 20;
            SO.ML = 10;
            SO.LL = 25;
            SO.NL = 15;
            SO.MJ = 10;
            SO.QL = 10;
            SO.NowLife = (int)(SO.TZ * GG.RTZ);
            SO.MaxLife = (int)(SO.TZ * GG.RTZ);
            SO.NowMagic = (int)(SO.ML * GG.RML);
            SO.MaxMagic = (int)(SO.ML * GG.RML);
            SO.MaxVigour = 100;
            SO.MaxBeef = 100;
            SO.LevelUpExp = SO.Level * SO.Level * GG.RLeveUpExp + 100;
            Super.SystemConfig = XElement.Load(@"System\Config.xml");
            Super.MonsterConfig = XElement.Load(@"System\Monster.xml");
            GameState = GG.GameStateType.Default;
            //根据地图代号获取该代号地图数据
            if (SO != null)
            {
                SO.TakeNum = 2;
                SO.PetList = new SpiritObject[10];
                SO.PetList[0] = Super.GetMonster("大海龟"); SO.PetList[0].QL = 10;
                SO.PetList[1] = Super.GetMonster("大海龟"); SO.PetList[1].QL = 20;
                SO.TakeAttackID = 1;
                mapCode=SO.MapCode;
            }
            Super.LoadXElement(string.Format("MapData{0}", mapCode.ToString()), Super.GetTreeNode(Super.SystemConfig, "Map", "Sign", mapCode.ToString()));
        }
     
        #endregion
       
        #region 游戏相关设置初始化

        delegate void WorkDelegate(); //工作委托
        DispatcherTimer AuxiliaryThread; //游戏窗体辅线程
        BackgroundWorker BackWorker; //游戏后台数据处理工作线程
        DispatcherTimer FightTimer;//战斗计时器  
        /// <summary>
        /// 初始化游戏设置
        /// </summary>
        private void InitializeGameSetting()
        {
            CompositionTarget.Rendering += new EventHandler(Timer_Tick); //启动界面刷新线程事件(与刷新率同步)
            //设置游戏窗体辅助线程
            AuxiliaryThread = new DispatcherTimer(DispatcherPriority.Normal);
            AuxiliaryThread.Tick += new EventHandler(AuxiliaryThread_Tick);
            AuxiliaryThread.Interval = TimeSpan.FromMilliseconds(1000);
            AuxiliaryThread.Start();
            //设置后台线程
            BackWorker = new BackgroundWorker();
            BackWorker.DoWork += new DoWorkEventHandler(BackWorker_DoWork);
            BackWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackWorker_RunWorkerCompleted);

            FightTimer = new DispatcherTimer();
            FightTimer.Interval = TimeSpan.FromSeconds(10);
            FightTimer.Tick += new EventHandler(FightTimer_Tick);
        }

     
        #endregion

        #region 游戏工具
        /// <summary>
        /// 游戏的界面工具
        /// </summary>
        private void InitializeGameTool()
        {
            heroStateShow = new HeroStateShow();
            GameWindow.Children.Add(heroStateShow);
            Canvas.SetTop(heroStateShow,0);
            Canvas.SetRight(heroStateShow, 0);

            mapHeadShow = new MapHeadShow();
            GameWindow.Children.Add(mapHeadShow);
            Canvas.SetTop(mapHeadShow, 0);
            Canvas.SetLeft(mapHeadShow, 0);

            gameMenuShow = new GameMenuShow();
            GameWindow.Children.Add(gameMenuShow);
            Canvas.SetBottom(gameMenuShow, 0);
            Canvas.SetLeft(gameMenuShow, 2);
        }
        #endregion

        #region 游戏对象初始化
        private void InitializeMap()
        {
            //抽离地图数据中的地表层参数并用其来初始化地图地表层
            InitMapSurface(Super.GetXElement(string.Format("MapData{0}", mapCode.ToString())).Element("Surface"));
            //抽离精灵数据中的NPC集合并用其来初始化地图上的NPC
            InitNPC(Super.GetXElement(string.Format("MapData{0}", mapCode.ToString())).Element("Spirits").Elements("NPC"));
            //初始化传送点
            InitTeleport(Super.GetXElement(string.Format("MapData{0}", mapCode.ToString())).Element("Teleports").Elements("Teleport"));
        }

        private void InitTeleport(IEnumerable<XElement> arglist)
        {
           int count = arglist.Count();
            teleport = new Teleport[count];
            for (int i = 0; i < count; i++)
            {
                XElement args = arglist.ElementAt(i);
                teleport[i] = new Teleport("90传送点", 8, 1, 0, 0, 1136, 85,
                    Int32.Parse(args.Attribute("Code").Value),Int32.Parse(args.Attribute("X").Value),
                    Int32.Parse(args.Attribute("Y").Value),Int32.Parse(args.Attribute("ToX").Value),
                    Int32.Parse(args.Attribute("ToY").Value),args.Attribute("Tip").Value);
                teleport[i].Name = args.Attribute("Name").Value;
                World.RegisterName(teleport[i].Name, teleport[i]);
                Add(teleport[i]);
            }
        }


        
        /// <summary>
        /// 初始NPC
        /// </summary>
        private void InitNPC(IEnumerable<XElement> iEnumerable)
        {
             int monsterNum = iEnumerable.Count();
             npc = new Spirit[monsterNum];
            for (int i = 0; i < monsterNum; i++)
            {
                XElement args = iEnumerable.ElementAt(i);
                Dictionary<int, GG.ImageInfo> imagepathtable = new Dictionary<int, GG.ImageInfo>();
                imagepathtable.Add((int)GG.Actions.Stand, new GG.ImageInfo(args.Attribute("Name").Value, Int32.Parse(args.Attribute("WFrameNum").Value), Int32.Parse(args.Attribute("HFrameNum").Value), Int32.Parse(args.Attribute("StartX").Value), Int32.Parse(args.Attribute("StartY").Value), Int32.Parse(args.Attribute("EndX").Value), Int32.Parse(args.Attribute("EndY").Value)));

                npc[i] = new Spirit(args.Attribute("SName").Value, new Point(Int32.Parse(args.Attribute("X").Value), Int32.Parse(args.Attribute("Y").Value)), imagepathtable, Int32.Parse(args.Attribute("Direction").Value), (GG.SpiritTypes)Int32.Parse(args.Attribute("Type").Value));
                Register(npc[i].VSName, npc[i]);
                Add(npc[i]);
            }
        }

        private void InitMapSurface(XElement xElement)
        {
            mapHeadShow.MapName = xElement.Attribute("MapName").Value;

            Super.PlayBackGroundMusic(xElement.Attribute("BKMusic").Value);
            gameMap = new GameMap();
            gameMap.SetMap("Map/" + xElement.Attribute("FileName").Value);
            Add(gameMap);
        }
        
        private void InitHero()
        {
            if (SO == null)
            {
                Dictionary<int, GG.ImageInfo> imagepath = new Dictionary<int, GG.ImageInfo>();
                imagepath.Add((int)GG.Actions.Stand, new GG.ImageInfo("剑侠客_站立", 9, 8, "000刀_剑侠客_站立"));
                imagepath.Add((int)GG.Actions.Run, new GG.ImageInfo("剑侠客_走动", 8, 8, "000刀_剑侠客_走动"));
                imagepath.Add((int)GG.Actions.Hello, new GG.ImageInfo("剑侠客_打招呼", 7, 4));
                imagepath.Add((int)GG.Actions.Salute, new GG.ImageInfo("剑侠客_行礼", 9, 4));
                imagepath.Add((int)GG.Actions.Angry, new GG.ImageInfo("剑侠客_发火", 12, 8));
                imagepath.Add((int)GG.Actions.Attack, new GG.ImageInfo("剑侠客_攻击_刀", 9, 4, "000刀_剑侠客_攻击"));
                imagepath.Add((int)GG.Actions.Fight, new GG.ImageInfo("剑侠客_战斗_刀", 8, 4, "000刀_剑侠客_战斗"));
                imagepath.Add((int)GG.Actions.Onrush, new GG.ImageInfo("剑侠客_向前冲", 5, 4, "000刀_剑侠客_向前冲"));
                SO = new SpiritObject()
               {
                   Name = "离歌笑",
                   FaceImage = "剑侠客默认",
                   Direction = 4,
                   Location = new Point(100, 100),
                   Level = 1,
                   LevelUpExp = 1000,
                   MapCode = 0,
                   MaxEnergy = 150,
                   MaxLife = 500,
                   MaxMagic = 500,
                   NowEnergy = 100,
                   NowExp = 0,
                   NowLife = 200,
                   NowMagic = 200,
                   ImagePath = imagepath,
                   SType = GG.SpiritTypes.Leader,
                   TakeAttackID=-1

               };
            }

            Hero = new Spirit(SO);
            Register(Hero.VSName, Hero);
            Add(Hero);
        }
        #endregion

        #region 游戏画面刷新间隔事件 界面位移  刷怪物

        /// <summary>
        /// 游戏窗口刷新主线程间隔事件
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (GameState == GG.GameStateType.Default)
            {
                AllMove();
                if (Math.Abs(Hero.Destination.X + Hero.Destination.Y - Hero.X - Hero.Y) == 0)
                {
                    Hero.Action = (int)GG.Actions.Stand;
                }
                
                TryGoToFighting();
            }
            else if (GameState == GG.GameStateType.Fighting)
            {
               //if(DateTime.Now.Second%10==0) FightRun();
               RefreshFightSpirit();
            }
        }

       /// <summary>
       /// 刷新战斗中怪得位置
       /// </summary>
       private void RefreshFightSpirit()
       {
           for (int i = 0; i < MonsterFightingList.Count; i++)
           {
               MonsterFightingList[i].Left = MonsterFightingList[i].FX;
               MonsterFightingList[i].Top = MonsterFightingList[i].FY;
           }
           for (int i = 0; i < HeroFightingList.Count; i++)
           {
               HeroFightingList[i].Left = HeroFightingList[i].FX;
               HeroFightingList[i].Top = HeroFightingList[i].FY;
           }
       }
        /// <summary>
        /// 窗口中的所有对象物体以主角为参照物进行相对位置时时改变
        /// </summary>
        private void AllMove()
        {
            if (Hero.X <= this.Width / 2 && Hero.Y <= this.Height / 2)
            {
                //地图左上
                RefreshAllObject(0, 0);
            }
            else if (Hero.X <= this.Width / 2 && Hero.Y >= gameMap.Height - this.Height / 2)
            {
                //地图左下
                RefreshAllObject(0, gameMap.Height - 2 * this.Height / 2);
            }
            else if (Hero.X >= gameMap.Width - this.Width / 2 && Hero.Y <= this.Height / 2)
            {
                //地图右上
                RefreshAllObject(gameMap.Width - 2 * this.Width / 2, 0);
            }
            else if (Hero.X >= gameMap.Width - this.ActualWidth / 2 && Hero.Y >= gameMap.Height - this.ActualHeight / 2)
            {
                //地图右下
                RefreshAllObject(gameMap.Width - 2 * this.ActualWidth / 2, gameMap.Height - 2 * this.ActualHeight / 2);
            }
            else if (Hero.X <= this.ActualWidth / 2)
            {
                //地图左中
                RefreshAllObject(0, Hero.Y - this.ActualHeight / 2);
            }
            else if (Hero.X >= gameMap.Width - this.ActualWidth / 2)
            {
                //地图右中
                RefreshAllObject(gameMap.Width - 2 * this.ActualWidth / 2, Hero.Y - this.ActualHeight / 2);
            }
            else if (Hero.Y <= this.ActualHeight / 2)
            {
                //地图中上
                RefreshAllObject(Hero.X - this.ActualWidth / 2, 0);
            }
            else if (Hero.Y >= gameMap.Height - this.ActualHeight / 2)
            {
                //地图中下
                RefreshAllObject(Hero.X - this.ActualWidth / 2, gameMap.Height - 2 * this.ActualHeight / 2);
            }
            else if ((Hero.X > this.ActualWidth / 2 || Hero.X < gameMap.Width - this.ActualWidth / 2) && (Hero.Y > this.ActualHeight / 2 || Hero.Y < gameMap.Height - this.ActualHeight / 2))
            {
                //地图正中
                RefreshAllObject(Hero.X - this.ActualWidth / 2, Hero.Y - this.ActualHeight / 2);
            }
            else
            {
                //移动失败处理
                AllMove();
            }
        }

        /// <summary>
        /// 刷新画布中所有对象方法
        /// </summary>
        /// <param name="deviationX">X方向上的偏移量</param>
        /// <param name="deviationY">Y方向上的偏移量</param>
        private void RefreshAllObject(double deviationX, double deviationY)
        {
            for (int i = 0; i < World.Children.Count; i++)
            {
                if (World.Children[i] is GameObject)
                {
                    //假如子控件为物体对象，则获取之
                    GameObject obj = World.Children[i] as GameObject;
                    //设置物体对象在游戏窗口中的显示位置(定位到它的图片左上角并进行对应偏移)
                    obj.Left = obj.X - obj.CenterX - deviationX;
                    obj.Top = obj.Y - obj.CenterY - deviationY;
                    //画家方法，使所有物体对象之间的遮挡关系以Y值为衡量标准产生远近层次
                    obj.ZIndex = Convert.ToInt32(obj.Y);
                    if (obj is Spirit)
                    {
                        Spirit spirit = obj as Spirit;
                    }
                 
                }
                Canvas.SetLeft(gameMap, -deviationX);
                Canvas.SetTop(gameMap, -deviationY);
            }
        }
        #endregion

        #region 游戏辅助线程间隔事件及后台工作处理

        /// <summary>
        /// 辅助线程间隔事件
        /// </summary>
        private void AuxiliaryThread_Tick(object sender, EventArgs e)
        {
            //异步刷新面板及障碍物等
            if (!BackWorker.IsBusy) { BackWorker.RunWorkerAsync(); }
        }
        /// <summary>
        /// 后台数据处理工作事件
        /// </summary>
        private void BackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //游戏Dispatcher跨线程异步刷新障碍物
            this.Dispatcher.BeginInvoke(new WorkDelegate(RefreshFace), DispatcherPriority.Normal, null);
          
           
        }

        /// <summary>
        /// 后台数据处理工作完成事件
        /// </summary>
        private void BackWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
             
        }

        /// <summary>
        /// 刷新界面元素
        /// </summary>
        private void RefreshFace()
        {
            UpdateObstructionAndSeekEnemy();
            UpdateMapHeadShow();
            UpdateGameMenuShow();
            UpdateHeroStateShow();
            UpDateAndSaveHeroInfo();
            UpDateMusic();
        }
        #region 刷新界面元素
        private void UpDateMusic()
        {
            if (Super.BackMusic.Position == Super.BackMusic.NaturalDuration.TimeSpan)
            {
                Super.BackMusic.Position = new TimeSpan(0);
                Super.BackMusic.Play();
            }
        }

        private void UpdateGameMenuShow()
        {
            
        }

        private void UpdateMapHeadShow()
        {
            mapHeadShow.HeroLocation = Hero.SInfo.Location;
           
        }

        private void UpdateHeroStateShow()
        {
            BitmapImage bi = null;
            bi=Super.GetImage("Head", Hero.SInfo.FaceImage);
            if (bi != null)
                heroStateShow.HeroFace = new CroppedBitmap(bi, new Int32Rect(0, 0, (int)bi.Width, (int)bi.Height));

            heroStateShow.HeroLifePercent = Hero.SInfo.NowLife / (double)Hero.SInfo.MaxLife;
            heroStateShow.HeroMagicPercent = Hero.SInfo.NowMagic / (double)Hero.SInfo.MaxMagic;
            heroStateShow.HeroEnergyPercent = Hero.SInfo.NowEnergy / (double)Hero.SInfo.MaxEnergy;
            heroStateShow.HeroExpPercent = Hero.SInfo.NowExp / (double)Hero.SInfo.LevelUpExp;

            if (Hero.SInfo.TakeAttackID != -1 && Hero.SInfo.PetList[Hero.SInfo.TakeAttackID] != null)
            {
                bi = null;
                bi = Super.GetImage("Head", Hero.SInfo.PetList[Hero.SInfo.TakeAttackID].FaceImage);
                if (bi != null)
                    heroStateShow.PetFace = new CroppedBitmap(bi, new Int32Rect(0, 0, (int)bi.PixelWidth - 1, (int)bi.PixelHeight - 1));
                heroStateShow.PetLifePercent = Hero.SInfo.PetList[Hero.SInfo.TakeAttackID].NowLife / (double)Hero.SInfo.PetList[Hero.SInfo.TakeAttackID].MaxLife;
                heroStateShow.PetMagicPercent = Hero.SInfo.PetList[Hero.SInfo.TakeAttackID].NowMagic / (double)Hero.SInfo.PetList[Hero.SInfo.TakeAttackID].MaxMagic;
                heroStateShow.PetExpPercent = Hero.SInfo.PetList[Hero.SInfo.TakeAttackID].NowExp / (double)Hero.SInfo.PetList[Hero.SInfo.TakeAttackID].LevelUpExp > 1 ? 1 : Hero.SInfo.PetList[Hero.SInfo.TakeAttackID].NowExp / (double)Hero.SInfo.PetList[Hero.SInfo.TakeAttackID].LevelUpExp;
            }
        }

        private void UpDateAndSaveHeroInfo()
        {
            //update
            Hero.SInfo.Location = new Point(Hero.X, Hero.Y);
            Hero.SInfo.MapCode = mapCode;
           
            FileStream fs = File.Open("Hero.Data", FileMode.OpenOrCreate);
            byte[] b = Super.ObjectToBytes(Hero.SInfo);
            fs.Write(b, 0, b.Length);
            fs.Flush();
            fs.Close();
            fs.Dispose();
            
        }
        /// <summary>
        /// 刷新动态障碍物数组及追踪对象
        /// </summary>
        private void UpdateObstructionAndSeekEnemy()
        {
            int x = (int)(Hero.X / 20), y = (int)(Hero.Y / 20);
            Teleport got=null;
            for (int i = 0; i < teleport.Length; i++)
            {
                if (Math.Abs(Hero.X - teleport[i].X) < 50 && Math.Abs(Hero.Y - teleport[i].Y) <50)
                {
                    got = teleport[i];
                }
            
            }
            //判断传送
            if (got!=null)
            {
                mapCode = got.Code;
                Super.LoadXElement(string.Format("MapData{0}", mapCode), Super.GetTreeNode(Super.SystemConfig, "Map", "Sign", mapCode.ToString()));

                Remove(gameMap);
                
                InitMapSurface(Super.GetXElement(string.Format("MapData{0}", mapCode)).Element("Surface"));

                ChangeDirection(Hero, got.ToDirection);
                Super.NewSpiritStoryboard(Hero.Name);
                Super.GetStoryboard(Hero.Name).Children.Clear();
                DoubleAnimation doubleAnimation = new DoubleAnimation(
                  got.ToX,
                  new Duration(TimeSpan.Zero)
                );
                doubleAnimation.SetValue(Storyboard.TargetProperty, Hero);
                doubleAnimation.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("X"));
                Super.GetStoryboard(Hero.Name).Children.Add(doubleAnimation);
                doubleAnimation = new DoubleAnimation(
                  got.ToY,
                  new Duration(TimeSpan.Zero)
                );
                doubleAnimation.SetValue(Storyboard.TargetProperty, Hero);
                doubleAnimation.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Y"));
                Super.GetStoryboard(Hero.Name).Children.Add(doubleAnimation);
                Super.GetStoryboard(Hero.Name).Begin();
                Hero.Action = (int)GG.Actions.Stand;
                for (int i = 0; i < npc.Count(); i++)
                {
                    Remove(npc[i]);
                    World.UnregisterName( npc[i].VSName);
                    npc[i] = null;
                }
                InitNPC(Super.GetXElement(string.Format("MapData{0}", mapCode)).Element("Spirits").Elements("NPC"));
               
                for (int i = 0; i < teleport.Count(); i++)
                {
                    Remove(teleport[i]);
                    World.UnregisterName( teleport[i].Name);
                    teleport[i] = null;
                }
                InitTeleport(Super.GetXElement(string.Format("MapData{0}", mapCode)).Element("Teleports").Elements("Teleport"));
             
            }
           
        }
        #endregion


        #endregion

        #region 相关方法及特殊事件

        /// <summary>
        /// 在画布中以注册Name精灵控件
        /// </summary>
        private void Register(string name, Spirit spirit)
        {
            World.RegisterName(name, spirit);
        }

        /// <summary>
        /// 在画布中查找Name精灵控件
        /// </summary>
        private Spirit Find(string name)
        {
            return World.FindName(name) as Spirit;
        }

        /// <summary>
        /// 添加控件进画布
        /// </summary>
        private void Add(UIElement u)
        {
            World.Children.Add(u);
        }

        /// <summary>
        /// 移除控件从画布
        /// </summary>
        private void Remove(UIElement u)
        {
            World.Children.Remove(u);
            u = null;
        }

        /// <summary>
        /// 改变精灵的朝向
        /// </summary>
        /// <param name="spirit">对象精灵</param>
        private void ChangeDirection(Spirit spirit, Point p)
        {
            spirit.ApplyAnimationClock(Spirit.DirectionProperty, null);
            spirit.Direction = Super.GetDirectionByTan(p.X, p.Y, spirit.X, spirit.Y);
        }

        /// <summary>
        /// 改变精灵的朝向
        /// </summary>
        /// <param name="spirit">对象精灵</param>
        private void ChangeDirection(Spirit spirit, int direction)
        {
            spirit.ApplyAnimationClock(Spirit.DirectionProperty, null);
            spirit.Direction = direction;
        }

        /// <summary>
        /// 修改所有NPC的透明度UI
        /// </summary>
        /// <param name="?"></param>
        private void ChangeAllNpcOpactit(double op)
        {
            for (int i = 0; i < npc.Length; i++)
            {
                npc[i].Opacity = op;
            }
        }

        #endregion

        #region 命中测试方法

        //命中测试用精灵收容器
        List<Spirit> SpiritList = new List<Spirit>();

        /// <summary>
        /// 命中测试，并返回命中的Spirit对象
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Spirit SpiritTest(MouseEventArgs e)
        {
            bool targetIsFound = false;
            StartHitTest(e.GetPosition(World));
            if (SpiritList.Count > 0)
            {
                for (int i = 0; i < SpiritList.Count; i++)
                {
                    if (!targetIsFound && isEfficaciousSection(SpiritList[i].EfficaciousSection, e.GetPosition(SpiritList[i])))
                    {
                        //更改鼠标光标及对象透明度
                      //  this.Cursor = IsOpposition(Leader, SpiritList[i]) ? Super.getCursor(2) : Super.getCursor(1);
                        SpiritList[i].BodyOpacity = 0.6;
                        targetIsFound = true;
                        return SpiritList[i];
                    }
                    else
                    {
                        if (!targetIsFound) 
                        { 
                           // this.Cursor = Super.getCursor(0); 
                        }
                        SpiritList[i].BodyOpacity = 1;
                        return null;
                    }
                }
            }
            return null;
        }
      

        /// <summary>
        /// 启动命中测试
        /// </summary>
        /// <param name="p">相对于画布中的点</param>
        private void StartHitTest(Point p)
        {
            SpiritList.Clear();
            VisualTreeHelper.HitTest(World, new HitTestFilterCallback(HitFilter), new HitTestResultCallback(HitResult), new PointHitTestParameters(p));
        }

        /// <summary>
        /// 命中测试过滤器
        /// </summary>
        private HitTestFilterBehavior HitFilter(DependencyObject dObject)
        {
            if (dObject is Spirit)
            {
                SpiritList.Add(dObject as Spirit);
            }
            return HitTestFilterBehavior.Continue;
        }

        /// <summary>
        /// 命中测试回调结果
        /// </summary>
        private HitTestResultBehavior HitResult(HitTestResult result)
        {
            return HitTestResultBehavior.Continue;
        }

        /// <summary>
        /// 判断点是否在精灵实体有效区域
        /// </summary>
        /// <param name="efficaciousSection">精灵实体有效区域范围</param>
        /// <param name="p">相对于精灵Canvas中的点</param>
        /// <returns>是/否</returns>
        private bool isEfficaciousSection(double[] efficaciousSection, Point p)
        {
            return p.X >= efficaciousSection[0] && p.X <= efficaciousSection[1] && p.Y >= efficaciousSection[2] && p.Y <= efficaciousSection[3] ? true : false;
        }

        #endregion

        #region 移动，寻路
      
        /// <summary>
        /// 向目标移动
        /// </summary>
        /// <param name="target">目标坐标</param>
        private void MoveTo(Spirit spirit, Point target)
        {
            spirit.Destination = target; //设置精灵的最终移动目的地
            AStarMoveTo(spirit, target);
            ////如果前方就是障碍物则启动A*寻路，否则直线移动
            //if (WillCollide(spirit))
            //{
            //    AStarMoveTo(spirit, target);
            //}
            //else
            //{
            //    StraightMoveTo(spirit, target);
            //}
            
        }
        /// <summary>
        /// 判断是否将要碰撞到障碍物(障碍物预测法)
        /// </summary>
        private bool WillCollide(Spirit spirit)
        {
            switch ((int)spirit.Direction)
            {
                case 0:
                    return  gameMap.mapMask[(int)(spirit.X / 20), (int)(spirit.Y / 20) - spirit.HoldHeight - 1] == 0 ? true : false;
                case 1:
                    return  gameMap.mapMask[(int)(spirit.X / 20) + spirit.HoldWidth + 1, (int)(spirit.Y / 20) - spirit.HoldHeight - 1] == 0 ? true : false;
                case 2:
                    return  gameMap.mapMask[(int)(spirit.X / 20) + spirit.HoldWidth + 1, (int)(spirit.Y / 20)] == 0 ? true : false;
                case 3:
                    return  gameMap.mapMask[(int)(spirit.X / 20) + spirit.HoldWidth + 1, (int)(spirit.Y / 20) + spirit.HoldHeight + 1] == 0 ? true : false;
                case 4:
                    return  gameMap.mapMask[(int)(spirit.X / 20), (int)(spirit.Y / 20) + spirit.HoldHeight + 1] == 0 ? true : false;
                case 5:
                    return  gameMap.mapMask[(int)(spirit.X / 20) - spirit.HoldWidth - 1, (int)(spirit.Y / 20) + spirit.HoldHeight + 1] == 0 ? true : false;
                case 6:
                    return  gameMap.mapMask[(int)(spirit.X / 20) - spirit.HoldWidth - 1, (int)(spirit.Y / 20)] == 0 ? true : false;
                case 7:
                    return  gameMap.mapMask[(int)(spirit.X / 20) - spirit.HoldWidth - 1, (int)(spirit.Y / 20) - spirit.HoldHeight - 1] == 0 ? true : false;
                default:
                    return true;
            }
        }
        /// <summary>
        /// 直线移动方法
        /// </summary>
        private void StraightMoveTo(Spirit spirit, Point p)
        {
           
            Super.NewSpiritStoryboard(spirit.Name);
            Super.GetStoryboard(spirit.Name).Children.Clear();
            //设置朝向
            ChangeDirection(spirit, p);
            //总的移动花费
            double totalcost = Math.Sqrt(Math.Pow((p.X - spirit.X) / 20, 2) + Math.Pow((p.Y - spirit.Y) / 20, 2)) * spirit.VRunSpeed * 0.8; //*0.8属于微调
            //创建X轴方向属性动画
            DoubleAnimation doubleAnimation = new DoubleAnimation(
              spirit.X,
              p.X,
              new Duration(TimeSpan.FromMilliseconds(totalcost))
            );
            doubleAnimation.SetValue(Storyboard.TargetProperty, spirit);
            doubleAnimation.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("X"));
            Super.GetStoryboard(spirit.Name).Children.Add(doubleAnimation);
            //创建Y轴方向属性动画
            doubleAnimation = new DoubleAnimation(
              spirit.Y,
              p.Y,
              new Duration(TimeSpan.FromMilliseconds(totalcost))
            );
            doubleAnimation.SetValue(Storyboard.TargetProperty, spirit);
            doubleAnimation.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Y"));
            Super.GetStoryboard(spirit.Name).Children.Add(doubleAnimation);
            //启动属性动画
            Super.GetStoryboard(spirit.Name).Begin();
            spirit.Action = (int)GG.Actions.Run;
        }

        private void AStarMoveTo(Spirit spirit, Point target)
        {
            int start_x = (int)(spirit.X / 20);
            int start_y = (int)(spirit.Y / 20);
            int end_x = (int)(target.X / 20);
            int end_y = (int)(target.Y / 20);

            AStar.AStar astar = new AStar.AStar(gameMap.mapMask);
            Node start = new Node();
            Node end = new Node();
            start.X = start_x;
            start.Y = start_y;
            end.X = end_x;
            end.Y = end_y;

            astar.Find(start, end);
            List<Node> path = astar.MoveList; //开始寻路
            Super.NewSpiritStoryboard(spirit.Name);
            Super.GetStoryboard(spirit.Name).Children.Clear();
            if (path==null|| path.Count < 2)
            {
                    //路径不存在
                 if (spirit.Action == (int)GG.Actions.Run) { spirit.Action = (int)GG.Actions.Stand; Super.PauseSpiritStoryboard(spirit); }
                } else {
                    DoubleAnimationUsingKeyFrames keyFramesAnimationX = new DoubleAnimationUsingKeyFrames();
                    keyFramesAnimationX.Duration = new Duration(TimeSpan.FromMilliseconds(path.Count * spirit.VRunSpeed));
                    keyFramesAnimationX.SetValue(Storyboard.TargetProperty, spirit);
                    keyFramesAnimationX.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("X"));
                    //创建Y轴方向逐帧动画
                    DoubleAnimationUsingKeyFrames keyFramesAnimationY = new DoubleAnimationUsingKeyFrames();
                    keyFramesAnimationY.Duration = new Duration(TimeSpan.FromMilliseconds(path.Count * spirit.VRunSpeed));
                    keyFramesAnimationY.SetValue(Storyboard.TargetProperty, spirit);
                    keyFramesAnimationY.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Y"));
                    //创建朝向动画
                    Int32AnimationUsingKeyFrames keyFramesAnimationDirection = new Int32AnimationUsingKeyFrames();
                    keyFramesAnimationDirection.Duration = new Duration(TimeSpan.FromMilliseconds(path.Count * spirit.VRunSpeed));
                    keyFramesAnimationDirection.SetValue(Storyboard.TargetProperty, spirit);
                    keyFramesAnimationDirection.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Direction"));
                    for (int i = 0; i < path.Count; i++)
                    {
                        //加入X轴方向的匀速关键帧
                        LinearDoubleKeyFrame keyFrame = new LinearDoubleKeyFrame();
                        //平滑衔接动画(将寻路坐标系中的坐标放大回地图坐标系中的坐标)
                        if (i == path.Count - 1) {
                            keyFrame.Value = spirit.Destination.X;
                        } else if (i == 0) {
                            keyFrame.Value = spirit.X;
                            
                        } else {
                            keyFrame.Value = path[i].X * 20 + 20 / 2; //+ GridSize / 2为偏移处理
                        }
                        keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(spirit.VRunSpeed * (i)));
                        keyFramesAnimationX.KeyFrames.Add(keyFrame);
                        //加入X轴方向的匀速关键帧
                        keyFrame = new LinearDoubleKeyFrame();
                        if (i == path.Count - 1) {
                            keyFrame.Value = spirit.Destination.Y;
                            
                        } else if (i == 0) {
                            keyFrame.Value = spirit.Y;
                            
                        } else {
                            keyFrame.Value = path[i].Y * 20 + 20 / 2; //+ GridSize / 2偏移处理
                        }
                        keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(spirit.VRunSpeed * (i)));
                        keyFramesAnimationY.KeyFrames.Add(keyFrame);
                        //加入朝向匀速关键帧
                        LinearInt32KeyFrame keyFramed = new LinearInt32KeyFrame();
                        keyFramed.Value = i == 0
                            ? (int)Super.GetDirectionByAspect((int)path[i + 1].X, (int)path[i + 1].Y, (int)path[i].X, (int)path[i].Y)
                            : (int)Super.GetDirectionByAspect((int)path[i].X, (int)path[i].Y, (int)path[i - 1].X, (int)path[i - 1].Y)
                        ;
                        keyFramed.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(spirit.VRunSpeed * (i)));
                        keyFramesAnimationDirection.KeyFrames.Add(keyFramed);
                    }
                    Super.GetStoryboard(spirit.Name).Children.Add(keyFramesAnimationX);
                    Super.GetStoryboard(spirit.Name).Children.Add(keyFramesAnimationY);
                    Super.GetStoryboard(spirit.Name).Children.Add(keyFramesAnimationDirection);
                    //启动属性动画
                    Super.GetStoryboard(spirit.Name).Begin();
                    spirit.Action = (int)GG.Actions.Run;
                }
         }

        #endregion

        #region 战斗相关

        /// <summary>
        ///概率遇怪 开始战斗
        /// </summary>
        private void TryGoToFighting()
        {
            //判断该地图是否有怪
            IEnumerable<XElement> iEnumerable = Super.GetXElement(string.Format("MapData{0}", mapCode.ToString())).Element("Spirits").Elements("Monster");
            if (iEnumerable.Count()<1) return;  //没有怪返回

            //判断是否遇怪概率事件
            if (DateTime.Now.Second < 30 && Super.random.Next(0, 10) < 8)
            {
                #region 添加怪物
                int MonsterNum = Super.random.Next(1, 11);
                //int MonsterNum = 10;
                MonsterFightingList.Clear();
                while (MonsterNum > 0)
                {
                    MonsterNum--;
                    MonsterFightingList.Add(
                        new Spirit(
                            Super.GetMonster(
                            iEnumerable.ElementAt(Super.random.Next(0, iEnumerable.Count())).Attribute("Name").Value)));
                }
                #endregion
                #region  添加友方人Hero
                HeroFightingList.Clear();
                HeroFightingList.Add(Hero);
                if (Hero.SInfo.TakeAttackID != -1 && Hero.SInfo.PetList[Hero.SInfo.TakeAttackID] != null)
                {
                    HeroFightingList.Add(new Spirit(Hero.SInfo.PetList[Hero.SInfo.TakeAttackID]));
                }
                #endregion

                GoToFighting();
            }
        }

        /// <summary>
        /// 初始化战斗
        /// </summary>
        private void GoToFighting()
        {
            for (int i = 0; i < MonsterFightingList.Count; i++)
            {
                ShowFightSpirit(MonsterFightingList[i],true,i);
            }
            for (int i = 0; i < HeroFightingList.Count; i++)
            {
                if (HeroFightingList[i].SInfo.SType == GG.SpiritTypes.Leader)
                {
                    ShowFightSpirit(HeroFightingList[i], false, i+5);
                    HeroFightingList[i].Action = (int)GG.Actions.Fight;
                }
                else 
                {
                    ShowFightSpirit(HeroFightingList[i], false, i-1);
                }
            }
            //设置状态
            GameState = GG.GameStateType.Fighting;
            ChangeAllNpcOpactit(0);
            FightImage.Opacity = 1;
            FightTimer.Start();
        }

        /// <summary>
        /// 显示战斗的精灵
        /// </summary>
        private void ShowFightSpirit(Spirit spirit, bool diren, int n)
        {

            if (World.Children.Contains(spirit) == false) World.Children.Add(spirit);
            spirit.ZIndex = 50;
            if (diren) //是敌人
            {
                ChangeDirection(spirit, 0);
                int bx = 180;
                int by = 200;
                int sp = 60;
                if (n < 3)
                {
                    spirit.FX = bx + n * sp - spirit.CenterX;
                    spirit.FY = by - n * sp - spirit.CenterY;
                }
                else if (n < 5)
                {
                    n = n - 2;
                    spirit.FX = bx - n * sp - spirit.CenterX;
                    spirit.FY = by + n * sp - spirit.CenterY;
                }
                else if (n < 8)
                {
                    n = n - 5;
                    spirit.FX = bx + 80 + n * sp - spirit.CenterX;
                    spirit.FY = by + sp - n * sp - spirit.CenterY;
                }
                else
                {
                    n = n - 7;
                    spirit.FX = bx + 80 - n * sp - spirit.CenterX;
                    spirit.FY = by + sp + n * sp - spirit.CenterY;
                }

            }
            else //是自己人
            {
                //改变方向
                ChangeDirection(spirit, 2);
                //显示当前人物血调
                spirit.Life.Visibility = Visibility.Visible;
                int bx = (int)this.Width - 240;
                int by = (int)this.Height - 240;
                int sp = 60;
                if (n < 3)
                {
                    spirit.FX = bx + n * sp - spirit.CenterX;
                    spirit.FY = by - n * sp - spirit.CenterY;
                }
                else if (n < 5)
                {
                    n = n - 2;
                    spirit.FX = bx - n * sp - spirit.CenterX;
                    spirit.FY = by + n * sp - spirit.CenterY;
                }
                else if (n < 8)
                {
                    n = n - 5;
                    spirit.FX = bx + 80 + n * sp - spirit.CenterX;
                    spirit.FY = by + sp - n * sp - spirit.CenterY;
                }
                else
                {
                    n = n - 7;
                    spirit.FX = bx + 80 - n * sp - spirit.CenterX;
                    spirit.FY = by + sp + n * sp - spirit.CenterY;
                }
            }


        }


        /// <summary>
        /// 战斗动作
        /// </summary>
        private void StartSelectFightActionToSpirit()
        {
         
            for (int i = 0; i < MonsterFightingList.Count; i++)
            {
                 RandomAttackForSpirit(MonsterFightingList[i], true);
            }
            for (int i = 0; i < HeroFightingList.Count; i++)
            {
                if (HeroFightingList[i]== Hero)
                {
                    //主人要自己选择
                  // ShowBattleButtonsShow(Hero);
                   
                }
                else
                {
                    RandomAttackForSpirit(HeroFightingList[i], false);
                }
            }
        }
       

        /// <summary>
        /// 获取随机的攻击对象
        /// </summary>
        /// <param name="spirit"></param>
        /// <param name="diren"></param>
        private void RandomAttackForSpirit(Spirit spirit, bool diren)
        {
            if (diren)//敌人要攻击我方
            {
                spirit.LockSpirit = HeroFightingList[Super.random.Next(HeroFightingList.Count)];
                spirit.FightAction = GG.SpiritFightAction.Attack;
            }
            else
            {
                spirit.LockSpirit = MonsterFightingList[Super.random.Next(MonsterFightingList.Count)];
                spirit.FightAction = GG.SpiritFightAction.Attack;
            }
        
        }

        /// <summary>
        /// 游戏窗口刷新主线程间隔事件
        /// </summary>
        private void FightTimer_Tick(object sender, EventArgs e)
        {
            if (GameState!=GG.GameStateType.Default)
            {
                FightRun();

                Thread.Sleep(2000);
            }
        }
        
        private void FightRun()
        {
                StartSelectFightActionToSpirit();
                //while (Hero.LockSpirit == null)
                {
                    
                    ShowBattleButtonsShow(Hero);
                }
                
                if (Hero.LockSpirit != null)
                {
                    BoutReadIsOk = false;
                    int starttime=10000;
                    for (int i = 0; i < HeroFightingList.Count; i++)
                    {
                        DoSpiritFightAction(HeroFightingList[i], false,starttime*i);
                    }
                    for (int j = 0; j < MonsterFightingList.Count; j++)
                    {
                        DoSpiritFightAction(MonsterFightingList[j], true, starttime * (HeroFightingList.Count + j));
                    }
                  
                }
            
           
        }
      

        /// <summary>
        /// 执行战斗的动作
        /// </summary>
        /// <param name="spirit"></param>
        private void DoSpiritFightAction(Spirit spirit,bool Isdiren,int starttime)
        {
            double fx = spirit.FX;
            double fy = spirit.FY;
            int actiontime = 2300;

            Super.NewSpiritStoryboard(spirit.Name);
            Super.GetStoryboard(spirit.Name).Children.Clear();
            //设置朝向
            if(Isdiren) ChangeDirection(spirit, 0);
            else ChangeDirection(spirit, 2);
            //总的移动花费
            double totalcost = Math.Sqrt(Math.Pow((spirit.LockSpirit.FX - spirit.FX) / 20, 2) + Math.Pow((spirit.LockSpirit.FY - spirit.FY) / 20, 2)) * spirit.VRunSpeed * 0.8; //*0.8属于微调
            //创建X轴方向属性动画
            DoubleAnimation doubleAnimation = new DoubleAnimation(
              spirit.FX,
              spirit.LockSpirit.FX,
              new Duration(TimeSpan.FromMilliseconds(totalcost))
            );
            doubleAnimation.SetValue(Storyboard.TargetProperty, spirit);
            doubleAnimation.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("FX"));
            doubleAnimation.BeginTime = TimeSpan.FromMilliseconds(starttime);
            Super.GetStoryboard(spirit.Name).Children.Add(doubleAnimation);
            //创建Y轴方向属性动画
            doubleAnimation = new DoubleAnimation(
              spirit.FY,
              spirit.LockSpirit.FY,
              new Duration(TimeSpan.FromMilliseconds(totalcost))
            );
            doubleAnimation.SetValue(Storyboard.TargetProperty, spirit);
            doubleAnimation.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("FY"));
            doubleAnimation.BeginTime = TimeSpan.FromMilliseconds(starttime);
            Super.GetStoryboard(spirit.Name).Children.Add(doubleAnimation);


            /////////////////////////////////////////////////////////////////动作

            Int32AnimationUsingKeyFrames ActionFrames = new Int32AnimationUsingKeyFrames();
            ActionFrames.Duration = new Duration(TimeSpan.FromMilliseconds(2 * totalcost + actiontime));
            ActionFrames.SetValue(Storyboard.TargetProperty, spirit);
            ActionFrames.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Action"));
            ActionFrames.BeginTime = TimeSpan.FromMilliseconds(starttime);
            DiscreteInt32KeyFrame keyFrame = new DiscreteInt32KeyFrame();
            keyFrame.Value = (int)GG.Actions.Onrush;
            keyFrame.KeyTime = TimeSpan.FromMilliseconds(200);
            ActionFrames.KeyFrames.Add(keyFrame);
            keyFrame = new DiscreteInt32KeyFrame();
            keyFrame.Value = (int)GG.Actions.Attack;
            keyFrame.KeyTime = TimeSpan.FromMilliseconds(totalcost);
            ActionFrames.KeyFrames.Add(keyFrame);
            keyFrame = new DiscreteInt32KeyFrame();
            keyFrame.Value = (int)GG.Actions.Onrush;
            keyFrame.KeyTime = TimeSpan.FromMilliseconds(totalcost + actiontime);
            ActionFrames.KeyFrames.Add(keyFrame);
            keyFrame = new DiscreteInt32KeyFrame();
            keyFrame.Value = (int)GG.Actions.Stand;
            keyFrame.KeyTime = TimeSpan.FromMilliseconds(2 * totalcost + actiontime - 200);
            ActionFrames.KeyFrames.Add(keyFrame);
            Super.GetStoryboard(spirit.Name).Children.Add(ActionFrames);


            /////////////////////////////////////////////////////////////////方向
            int todorection = 0;
            //设置朝向
            if (Isdiren) todorection = 2;
            else todorection = 0;

            //创建朝向动画
            Int32AnimationUsingKeyFrames DirectionFrames = new Int32AnimationUsingKeyFrames();
            DirectionFrames.Duration = new Duration(TimeSpan.FromMilliseconds(totalcost ));
            DirectionFrames.SetValue(Storyboard.TargetProperty, spirit);
            DirectionFrames.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Direction"));
            DirectionFrames.BeginTime = TimeSpan.FromMilliseconds(totalcost + actiontime + starttime);
            DiscreteInt32KeyFrame DkeyFrame = new DiscreteInt32KeyFrame();
            DkeyFrame.Value = todorection;
            DkeyFrame.KeyTime = TimeSpan.FromMilliseconds(000);
            DirectionFrames.KeyFrames.Add(DkeyFrame);

            DkeyFrame = new DiscreteInt32KeyFrame();
            if (Isdiren) DkeyFrame.Value = 0;
            else DkeyFrame.Value = 2;
            DkeyFrame.KeyTime = TimeSpan.FromMilliseconds(totalcost-50);
            DirectionFrames.KeyFrames.Add(DkeyFrame);

            Super.GetStoryboard(spirit.Name).Children.Add(DirectionFrames);
            /////////////////////////////////////////////////////////////////////////////返回
            //创建X轴方向属性动画
            doubleAnimation = new DoubleAnimation(
              spirit.LockSpirit.FX,
              fx,
              new Duration(TimeSpan.FromMilliseconds(totalcost))
            );
            doubleAnimation.SetValue(Storyboard.TargetProperty, spirit);
            doubleAnimation.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("FX"));
            doubleAnimation.BeginTime = TimeSpan.FromMilliseconds(totalcost + actiontime + starttime);
            Super.GetStoryboard(spirit.Name).Children.Add(doubleAnimation);
            //创建Y轴方向属性动画
            doubleAnimation = new DoubleAnimation(
              spirit.LockSpirit.FY,
              fy,
              new Duration(TimeSpan.FromMilliseconds(totalcost))
            );
            doubleAnimation.SetValue(Storyboard.TargetProperty, spirit);
            doubleAnimation.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("FY"));
            doubleAnimation.BeginTime = TimeSpan.FromMilliseconds(totalcost + actiontime + starttime);
            Super.GetStoryboard(spirit.Name).Children.Add(doubleAnimation);




            
            //启动属性动画
          
            Super.GetStoryboard(spirit.Name).Begin();
        }

        #endregion

        #region 外部调用方法
        //显示主角的属性信息
        public void ShowHeroPropertyShow()
        {
            if (heroPropertyShow == null)
            {
                heroPropertyShow = new HeroPropertyShow(Hero);

                GameWindow.Children.Add(heroPropertyShow);
                Canvas.SetTop(heroPropertyShow, 0);
                Canvas.SetLeft(heroPropertyShow, 0);
                Canvas.SetZIndex(heroPropertyShow, 100);
            }
            else CloseHeroPropertyShow();
        }
        public void CloseHeroPropertyShow()
        {
            if (heroPropertyShow != null)
            {
                GameWindow.Children.Remove(heroPropertyShow);
                heroPropertyShow = null;
            }
        }
        public void MoveHeroPropertyShow(double x, double y)
        {
            Canvas.SetTop(heroPropertyShow, Canvas.GetTop(heroPropertyShow) + y);
            Canvas.SetLeft(heroPropertyShow,Canvas.GetLeft(heroPropertyShow)+x );
        }
        //显示宠物的属性信息
        public void ShowMonsterPropertyShow()
        {
            if (monsterPropertyShow == null)
            {
                monsterPropertyShow = new MonsterPropertyShow(Hero);

                GameWindow.Children.Add(monsterPropertyShow);
                Canvas.SetTop(monsterPropertyShow, 0);
                Canvas.SetLeft(monsterPropertyShow, 0);
                Canvas.SetZIndex(monsterPropertyShow, 100);
            }
            else CloseMonsterPropertyShow();
        }
        public void CloseMonsterPropertyShow()
        {
            if (monsterPropertyShow != null)
            {
                GameWindow.Children.Remove(monsterPropertyShow);
                monsterPropertyShow = null;
            }
        }
        public void MoveMonsterPropertyShow(double x, double y)
        {
            Canvas.SetTop(monsterPropertyShow, Canvas.GetTop(monsterPropertyShow) + y);
            Canvas.SetLeft(monsterPropertyShow, Canvas.GetLeft(monsterPropertyShow) + x);
        }
        //显示包裹的属性信息
        public void ShowPackageShow()
        {
            if (packageShow == null)
            {
                packageShow = new PackageShow(Hero);

                GameWindow.Children.Add(packageShow);
                Canvas.SetTop(packageShow, 0);
                Canvas.SetLeft(packageShow, 0);
                Canvas.SetZIndex(packageShow, 100);
            }
            else ClosePackageShow();
        }
        public void ClosePackageShow()
        {
            if (packageShow != null)
            {
                GameWindow.Children.Remove(packageShow);
                packageShow = null;
            }
        }
        public void MovePackageShow(double x, double y)
        {
            Canvas.SetTop(packageShow, Canvas.GetTop(packageShow) + y);
            Canvas.SetLeft(packageShow, Canvas.GetLeft(packageShow) + x);
        }
        //显示系统设置信息
        public void ShowSystemSettingShow()
        {
            if (systemSettingShow == null)
            {
                systemSettingShow = new SystemSettingShow();

                GameWindow.Children.Add(systemSettingShow);
                Canvas.SetTop(systemSettingShow, 100);
                Canvas.SetLeft(systemSettingShow,200);
                Canvas.SetZIndex(systemSettingShow, 100);
            }
            else CloseSystemSettingShow();
        }
        public void CloseSystemSettingShow()
        {
            if (systemSettingShow != null)
            {
                GameWindow.Children.Remove(systemSettingShow);
                systemSettingShow = null;
            }
        }
        public void MoveSystemSettingShow(double x, double y)
        {
            Canvas.SetTop(systemSettingShow, Canvas.GetTop(systemSettingShow) + y);
            Canvas.SetLeft(systemSettingShow, Canvas.GetLeft(systemSettingShow) + x);
        }
        //显示战斗动作选择
        public void ShowBattleButtonsShow(Spirit s)
        {
            if (battleButtonsShow == null)
            {
                battleButtonsShow = new BattleButtonsShow(s);
                GameWindow.Children.Add(battleButtonsShow);
                Canvas.SetTop(battleButtonsShow, 100);
                Canvas.SetRight(battleButtonsShow, 50);
                Canvas.SetZIndex(battleButtonsShow, 100);
            }
            //else CloseBattleButtonsShow();
        }
        public void CloseBattleButtonsShow()
        {
            if (battleButtonsShow != null)
            {
                GameWindow.Children.Remove(battleButtonsShow);
                systemSettingShow = null;
            }
        }
        public void MoveBattleButtonsShow(double x, double y)
        {
            Canvas.SetTop(battleButtonsShow, Canvas.GetTop(battleButtonsShow) + y);
            Canvas.SetLeft(battleButtonsShow, Canvas.GetLeft(battleButtonsShow) + x);
        }
        //消息框
        public void ShowMessageShow()
        {
            if (messageShow == null)
            {
                messageShow = new MessageShow();

                GameWindow.Children.Add(messageShow);
                Canvas.SetTop(messageShow, 100);
                Canvas.SetLeft(messageShow, 200);
                Canvas.SetZIndex(messageShow, 100);
            }
            else CloseMessageShow();
        }
        public void CloseMessageShow()
        {
            if (messageShow != null)
            {
                GameWindow.Children.Remove(messageShow);
                messageShow = null;
            }
        }
        #endregion

      

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            SelectSpirit=SpiritTest(e);
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GameState == GG.GameStateType.Default)
            {
                Point p = e.GetPosition(gameMap);
                MoveTo(Hero, p);
            }
            else if (GameState == GG.GameStateType.Fighting)
            {
                if (battleButtonsShow != null && battleButtonsShow.IsSelect && SelectSpirit != null)
                {
                    battleButtonsShow.spirit.LockSpirit = SelectSpirit;
                    CloseBattleButtonsShow();
                    App.MW.Cursor = Super.getCursor(0);
                }
            
            }
        }
        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GameState == GG.GameStateType.Default)
            {
               // Point p = e.GetPosition(gameMap);
                //MoveTo(Hero, p);
            }
            else if (GameState == GG.GameStateType.Fighting)
            {
                if (battleButtonsShow != null)
                {
                    battleButtonsShow.Opacity = 1;
                    battleButtonsShow.IsSelect = false;
                    App.MW.Cursor = Super.getCursor(0);
                }

            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
          
        }
     
    }
}

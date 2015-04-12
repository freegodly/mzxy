using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using 梦之西游.Controls;
using 梦之西游.Logic.Static;

namespace 梦之西游.Controls
{

    public partial class MagicShow : UserControl, GameObject, IMagic {

        public DispatcherTimer MagicTimer;
        /// <summary>
        /// 魔法控件
        /// </summary>
        public MagicShow()
        {
            InitializeComponent();
            MagicTimer = new DispatcherTimer(DispatcherPriority.Normal, this.Dispatcher);
            MagicTimer.Interval = TimeSpan.FromMilliseconds(120);
            MagicTimer.Tick += new EventHandler(Timer_Tick);
            MagicTimer.Start();
        }

        /// <summary>
        /// 获取或设置帧推进器
        /// </summary>
        public int FrameCounter { get; set; }

        /// <summary>
        /// 获取或设置魔法当前开始图片列号
        /// </summary>
        public int CurrentStartFrame { get; set; }

        /// <summary>
        /// 获取或设置魔法当前结束图片列号
        /// </summary>
        public int CurrentEndFrame { get; set; }

        /// <summary>
        /// 获取或设置魔法起效针
        /// </summary>
        public int EffectiveFrame { get; set; }

        /// <summary>
        /// 获取或设置施法者名字
        /// </summary>
        public string MagicOwner{ get; set; }

        /// <summary>
        /// 获取或设置魔法代号
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 获取或设置魔法等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 获取或设置魔法有效半径
        /// </summary>
        public int Radius { get; set; }

        /// <summary>
        /// 获取或设置魔法的攻击力
        /// </summary>
        public int ATK { get; set; }

        /// <summary>
        /// 获取或设置施放魔法需要消耗的魔力值
        /// </summary>
        public int Consumption { get; set; }

        /// <summary>
        /// 获取或设置魔法附加属性代号
        /// </summary>
        public int ExtraAttribute { get; set; }

        /// <summary>
        /// 获取或设置魔法附加属性伤害效果
        /// </summary>
        public double ExtraEffect { get; set; }

        /// <summary>
        /// 获取或设置魔法附加属性持续时间
        /// </summary>
        public double ExtraTime { get; set; }

        /// <summary>
        /// 获取或设置魔法朝向
        /// </summary>
        public int Direction { get; set; }

        /// <summary>
        /// 获取或设置魔法效果装饰代号
        /// </summary>
        public int DecorationCode { get; set; }

        /// <summary>
        /// 获取或设置魔法攻击模式
        /// 单体,过程动画,击中目标再另外动画一次性去血
        /// 单体,直接在目标处动画一次性去血
        /// 单体,过程动画,击中目标后另外动画并多帧段去血
        /// 群体,直接在目标一个圆形区域内动画一次性去血
        /// 群体,穿梭,经过一个由4个点组成的斜矩形区域
        /// 群体,一个圆形区域或由N个点组成的闭合区域内多帧段攻击
        /// 单体,加血、加攻、加防等,点中目标则给目标加,否则给自己加(点中所有都可以,包括障碍物)
        /// 群体,加血、加攻、加防等
        /// 陷阱类,一直动画直到被采另外动画一次性去血,一个人一次只能用一个,第二次使用取消前面的
        /// </summary>
        public int Mode { get; set; }

        /// <summary>
        /// 获取或设置魔法中心点距离左边X坐标
        /// </summary>
        public double CenterX { get; set; }

        /// <summary>
        /// 获取或设置魔法中心点距离顶边Y坐标
        /// </summary>
        public double CenterY { get; set; }

        /// <summary>
        /// 获取或设置魔法实际宽
        /// </summary>
        public double Width_ { get; set; }

        /// <summary>
        /// 获取或设置魔法实际高
        /// </summary>
        public double Height_ { get; set; }

        /// <summary>
        /// 获取或设置魔法X坐标(关联属性)
        /// </summary>
        public double X {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X",
            typeof(double),
            typeof(MagicShow)
        );

        /// <summary>
        /// 获取或设置魔法Y坐标(关联属性)
        /// </summary>
        public double Y {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y",
            typeof(double),
            typeof(MagicShow)
        );

        /// <summary>
        /// 获取或设置魔法与父画布容器左边距离
        /// </summary>
        public double Left {
            get { return (double)this.GetValue(Canvas.LeftProperty); }
            set { this.SetValue(Canvas.LeftProperty, value); }
        }

        /// <summary>
        /// 获取或设置魔法与父画布容器顶边距离
        /// </summary>
        public double Top {
            get { return (double)this.GetValue(Canvas.TopProperty); }
            set { this.SetValue(Canvas.TopProperty, value); }
        }

        /// <summary>
        /// 获取或设置魔法层次
        /// </summary>
        public int ZIndex {
            get { return (int)this.GetValue(Canvas.ZIndexProperty); }
            set { this.SetValue(Canvas.ZIndexProperty, value); }
        }

        /// <summary>
        /// 魔法线程间隔事件
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e) {
           
        }
    }
}

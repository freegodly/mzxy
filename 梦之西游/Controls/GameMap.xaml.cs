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
using Dream;

namespace 梦之西游.Controls
{
    /// <summary>
    /// Map.xaml 的交互逻辑
    /// </summary>
    public partial class GameMap : UserControl, GameObject
    {
        #region (地图表层/遮罩)属性访问器

        /// <summary>
        /// 获取或设置关键中心点与它左上角X距离值
        /// </summary>
        public double CenterX { get; set; }

        /// <summary>
        /// 获取或设置关键中心点与它左上角Y距离值
        /// </summary>
        public double CenterY { get; set; }

        /// <summary>
        /// 获取或设置X坐标
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 获取或设置Y坐标
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 获取或设置层次深度
        /// </summary>
        public int ZIndex
        {
            get { return (int)this.GetValue(Canvas.ZIndexProperty); }
            set { if (ZIndex != value) { this.SetValue(Canvas.ZIndexProperty, value); } }
        }

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
        /// 获取或设置宽度
        /// </summary>
        public double Width_
        {
            get { return IMap.Width; }
            set { this.Width = value; IMap.Width = value; }
        }

        /// <summary>
        /// 获取或设置高度
        /// </summary>
        public double Height_
        {
            get { return IMap.Height; }
            set { this.Height = value; IMap.Height = value; }
        }

        /// <summary>
        /// 获取或设置图片源
        /// </summary>
        public ImageSource Source
        {
            get { return IMap.Source; }
            set { IMap.Source = value; }
        }

        /// <summary>
        /// 获取或设置透明度
        /// </summary>
        public double Opacity_
        {
            get { return this.Opacity; }
            set { this.Opacity = value; }
        }

        #endregion
        private GetMapResource map;

       
       

        public byte[,] mapMask;
        public GameMap()
        {
            InitializeComponent();
        }
        public void  SetMap(string mapname)
        {
            map = new GetMapResource();
            map.ReadMap(mapname);
            mapMask = map.MapMask;
            Width_ = map.Width;
            Height_ = map.Height;
            Source = map.MapMax;
        }
    }
}

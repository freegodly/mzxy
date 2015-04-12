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

namespace 梦之西游.Controls
{
    /// <summary>
    /// MapHeadShow.xaml 的交互逻辑
    /// </summary>
    public partial class MapHeadShow : UserControl
    {
        /// <summary>
        /// 设置显示坐标
        /// </summary>
        public Point HeroLocation
        {
            set { THeroLocation.Text = "X:" + ((int)value.X).ToString() +"  Y:"+ ((int)value.Y).ToString(); }
        }
        /// <summary>
        /// 设置显示地图名
        /// </summary>
        public string MapName
        {
            set { TMapName.Text = value; }
        }

        public MapHeadShow()
        {
            InitializeComponent();
        }
    }
}

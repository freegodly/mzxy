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
    /// SystemSettingShow.xaml 的交互逻辑
    /// </summary>
    public partial class SystemSettingShow : UserControl
    {
        public SystemSettingShow()
        {
            InitializeComponent();
        }
        Point p0;
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            p0 = e.GetPosition(App.MW.GameWindow);
        }
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p1 = e.GetPosition(App.MW.GameWindow);
                App.MW.MoveSystemSettingShow(p1.X - p0.X, p1.Y - p0.Y);
                p0 = p1;
            }
        }
        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.MW.CloseSystemSettingShow();
        }
    }
}

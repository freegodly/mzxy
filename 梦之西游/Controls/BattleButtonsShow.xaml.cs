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
using 梦之西游.Logic.Static;
using 梦之西游.Logic.Static;

namespace 梦之西游.Controls
{
    /// <summary>
    /// BattleButtonsShow.xaml 的交互逻辑
    /// </summary>
    public partial class BattleButtonsShow : UserControl
    {
        public Spirit spirit;
        public bool IsSelect;
        public BattleButtonsShow(Spirit s)
        {
            InitializeComponent();
            spirit = s;
            spirit.FightAction = GG.SpiritFightAction.UnKnow;
            IsSelect = false;
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
               // App.MW.MoveBattleButtonsShow(p1.X - p0.X, p1.Y - p0.Y);
                p0 = p1;
            }
        }
        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
           // App.MW.CloseHeroPropertyShow();
        }

        private void GJ_Click(object sender, RoutedEventArgs e)
        {
            spirit.FightAction = GG.SpiritFightAction.Attack;
            this.Opacity = 0;
            IsSelect = true;
            App.MW.Cursor = Super.getCursor(1);
        }

        private void FS_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FY_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DJ_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BZ_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ZH_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TP_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
